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
using ERPService.Sales;
using BusinessLogic.Shipping;
using System.Transactions;

namespace ERPSMS_v01.Shipping
{
    public partial class GoodsOutwardNote : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Shipping Plan PK
        /// </summary>
        private int ShippingPlanPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ShippingPlanPK] = value;
            }
        }
        /// <summary>
        /// For Get ConfigValue for show  netweight and gross weight
        /// </summary>
        private bool IsShowNetGrsWeight
        {
            get
            {
                return GetGlobalResourceObject("ConfigurationsRes", "IsShowDONetGrsWeight").ToString() == "1";
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
                return this.ViewState[ViewstateStrings.DespatchID] == null ? 0 : (int)this.ViewState[ViewstateStrings.DespatchID];
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
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForSP];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSosForSP] = value;
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
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        //private ServiceUtility serviceUtilityObj;
        private SAL_DESPATCH_HDR salDespatchHdrObj;
        private SAL_DESPATCH_DTL salDespatchDtlObj;
        private SAL_ORDER_DTL SalOrderDtlObj;
        private SAL_SHIPPING_PLAN_HDR ShippingHdrObj;
        private List<SAL_ORDER_DTL> SalOrderDtlList;
        private List<SAL_SHIPPING_PLAN_DTL> salShippingPlanDtlList;
        private SAL_SHIPPING_PLAN_DTL salShippingPlanDtlObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_CONST_MST admConstMstObj;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
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

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private DataSet dsLoadingPlan;
        DataTable dtFromPort;
        DataTable dtHSCodeList;
        private string despatchNo;
        private bool updateDespatch;
        private bool isSave;

        private string refID;
        private string inboxFlag;

        DataTable dtPageData;
        DataTable dtShipBy;
        DataTable dtShipmentTerms;

        private DataSet dsShippingPlanHDR;
        
        private int tabLevel;
        private int prevCompany = 0;
        private int shipByPK;
        double TotalNetWeight = 0.0;
        double TotalGrossWeight = 0.0;
        double TotalDespNowPcs = 0.0;
        double TotalDespNow = 0.0;
        double TotalOrdrQty = 0.0;
        double TotalDespatcQty = 0.0;

        private SAL_CONTAINER_EVAL_HDR SalContainerEvalHdrObj;
        private List<SAL_CONTAINER_EVAL_HDR> SalContainerEvalHdrList;

        private SAL_CONTAINER_INSP_HDR SalContainerInspHdrObj;
        private List<SAL_CONTAINER_INSP_HDR> SalContainerInspHdrList;

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
                    ConfigSetting();
                    GetFieldValues(ControlsEnum.HISCODE);
                    SetFieldValues(ControlsEnum.HISCODE);

                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    lblDeliveryOrderNo.Focus();
                    ShippingPlanPK = Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] != null ? (int)Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] : 0;
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    SelectedInvoicesCrDr = null;
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    txtCYDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfSOTypeCheckEnabled.Value = GetGlobalResourceObject("ConfigurationsRes", "IsEnableTypeFilter").ToString();

                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    // hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
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
                    EntryStatus = EntryStatus.ENTRYMODE;
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                        Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanPK = GetApplicationID(ucrWrkf.RefID);
                    }

                    if (ShippingPlanPK > 0)
                    {
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }

                        GetFieldValues(ControlsEnum.CARRIER);
                        SetFieldValues(ControlsEnum.CARRIER);
                        GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                        GetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                        SetFieldValues(ControlsEnum.CONTAINEREVALUATION);
                        GetFieldValues(ControlsEnum.CONTAINERINSPECTION);
                        SetFieldValues(ControlsEnum.CONTAINERINSPECTION);
                        GetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                        GetFieldValues(ControlsEnum.SHIPMENTTERMS);
                        SetFieldValues(ControlsEnum.SHIPMENTTERMS);
                        SetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.FROMPORT);
                        SetFieldValues(ControlsEnum.FROMPORT);
                        if (EntryStatus == EntryStatus.ENTRYMODE)
                        {
                            if (prevCompany != null && prevCompany != 0)
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(prevCompany.ToString())));
                            }
                            else
                            {
                                ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_COMPANY.ToString())));
                            }
                        }

                        if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                        {
                            ModifiedDatePnl.Visible = true;
                            CurrPK = SaleDespatchDtlList.First().SAL_DESPATCH_HDR.DPH_PK;
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;

                            //SetFieldValues(ControlsEnum.COMPANY);
                            //GetFieldValues(ControlsEnum.DELIVERYORDERHDR);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            if (SelectedSosFroDO != null || ShippingPlanPK > 0) //if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text))
                            {
                                pnlPrint.Visible = false;
                            }
                            else
                            {
                                pnlPrint.Visible = true;
                            }
                            SetFieldValues(ControlsEnum.FROMPORT);
                        }
                        else
                        {
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = "DPH_PK";
                            grdDeliveryOrderList.DataKeyNames = datakeyarray;

                            if (SelectedSosFroDO != null || ShippingPlanPK > 0)
                            {
                                SelectedSOListForDO = SelectedSosFroDO;
                                GetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                                GetFieldValues(ControlsEnum.SHIPPINGPLANDTLLIST);
                                if (salShippingPlanDtlList != null && salShippingPlanDtlList.Count > 0
                                    && salShippingPlanDtlList[0].SAL_SHIPPING_PLAN_HDR != null)
                                {
                                    txtDateOfShipment.Text = txtETD.Text = salShippingPlanDtlList[0].SAL_SHIPPING_PLAN_HDR.SNH_ETD.ToString(Resources.Constants.DateFormatShort);
                                }
                                SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                                //SetFieldValues(ControlsEnum.COMPANY);
                                GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                                EntryStatus = EntryStatus.NEWMODE;
                                updateDespatch = false;
                                GetFieldValues(ControlsEnum.INVOICENO);
                                //lblDispInvoiceNo.Text = hdfDespatchNo.Value;
                                if (SelectedSosFroDO != null || ShippingPlanPK > 0)
                                {
                                    pnlPrint.Visible = false;
                                }
                                else
                                {
                                    pnlPrint.Visible = true;
                                }

                                //--- new ---
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && ucrWrkf.HasPageTaskPermission)
                                {
                                    EntryStatus = EntryStatus.NEWMODE;
                                    ucrWrkf.ViewType = 1;
                                    //btnSave.Visible = true;
                                }
                                else
                                {
                                    Response.Redirect(Resources.PageURL.ShippingPlan);
                                }
                                //---- end new ----
                            }
                            else
                            {
                                Response.Redirect(Resources.PageURL.ShippingPlan);
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                    }

                    SetTabVisibility();
                    hdfType.Value = ApplicationType.DO;
                }
                hdfCurPk.Value = CurrPK.ToString();
            }
            catch (Exception ex)
            {
                ucrWrkf.ViewType = 0;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        private void ConfigSetting()
        {
            if (!IsShowNetGrsWeight)
            {
                grdDeliveryList.Columns[11].Visible = false;
                grdDeliveryList.Columns[12].Visible = false;
                grdDeliveryList.Width = 1200;
            }
            else
            {
                grdDeliveryList.Columns[11].Visible = true;
                grdDeliveryList.Columns[12].Visible = true;
                grdDeliveryList.Width = 1360;
            }
            hdfIsShowDONetGrsWt.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowDONetGrsWeight").ToString();
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
            SaleOrderService saleOrderServiceClient;
            saleOrderServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            DeliveryOrderService deliveryOrderServiceClient;
            deliveryOrderServiceClient = null;

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            ShippingPlanHdrService shippingPlanHdrServiceClient;
            shippingPlanHdrServiceClient = null;

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            ContainerEvaluationService ContainerEvaluationServiceClient = null;
            ContainerInspectionService ContainerInspectionServiceClient = null;

            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        //dsLoadingPlan = BusinessLogic.Shipping.ShippingPlanBL.GetPlanInfo(ShippingPlanPK);
                        dsLoadingPlan = new DataSet();
                        dsLoadingPlan = LoadingPlanBL.GetLoadingPlan(ShippingPlanPK, currentUser.SBUID, 1);
                        //To get Previous transaction based company
                        if (CurrPK == 0)
                        {
                            prevCompany = BusinessLogic.Shipping.ShippingPlanBL.GetPrevCompany(ShippingPlanPK);
                        }
                        break;
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
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFromDate.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtToDate.Text.Trim());
                        serviceUtilityObj.NeedAdvanceFilter = false;
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
                        SalOrderDtlList = saleOrderServiceClient.GetSelectedSaleOrderDetails(SelectedSosFroDO, serviceUtilityObj, ShippingPlanPK);
                        SalDetailList = SalOrderDtlList;
                        break;
                    case ControlsEnum.SHIPPINGPLANDTLLIST:
                        shippingPlanHdrServiceClient = new ShippingPlanHdrService();
                        shippingPlanHdrServiceClient = CommonFunctions.InitiateClient(shippingPlanHdrServiceClient);
                        salShippingPlanDtlObj = new SAL_SHIPPING_PLAN_DTL();
                        serviceUtilityObj = new ServiceUtility();
                        salShippingPlanDtlObj.SND_PLAN_HDR = ShippingPlanPK;
                        salShippingPlanDtlList = shippingPlanHdrServiceClient.GetShippingPlanDtl(salShippingPlanDtlObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.DELIVERYORDERDETAILS:
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        SalOrderDtlObj = new SAL_ORDER_DTL();
                        serviceUtilityObj = new ServiceUtility();
                        //SalOrderDtlObj.SOH_ACTIVE = 1;
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        //  serviceUtilityObj.SortBy =Resources.DataFieldRes.DespatchedDate;
                        //serviceUtilityObj.SortBy = "DPD_SO_DTL";//"DPD_DATE";
                        //serviceUtilityObj.ThenBy = Resources.DataFieldRes.DespatchedNo;(hide this line for solve bugId 4228)
                        //serviceUtilityObj.ThenBy = "DPD_SO_DTL";
                        //serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        //salDespatchDtlList = deliveryOrderServiceClient.GetDespatchedSaleOrders(DespatchID, serviceUtilityObj, ShippingPlanPK);
                        salDespatchDtlList = deliveryOrderServiceClient.GetDespatchedSaleOrderDetails(DespatchID, serviceUtilityObj, ShippingPlanPK);
                        SaleDespatchDtlList = salDespatchDtlList;
                        break;
                    case ControlsEnum.DESPATCHNO:
                        //Generate Invoice No
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        despatchNo = deliveryOrderServiceClient.GetDespatchNo(ApplicationType.DO, 0, currentUser.CurrentDeptPK,
                            Convert.ToDateTime(txtDateOfShipment.Text.Trim()), currentUser.PKUser, updateDespatch, 0, Convert.ToInt32(ddlCompany.SelectedValue));
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
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToByte(DbActiveStatus.ACTIVE), null, 13, 5, null);
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

                    case ControlsEnum.ENCLOSURECONFIG:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("AccountType").ToString();
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanPK, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.CONTAINERTYPE:
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToInt16(CommonConstants.ACTIVE), (int)ConstGroupType.ContainerType, null, null, currentUser.SBUID);
                        break;
                    #region Get SalContainerEvalHdr List
                    case ControlsEnum.CONTAINEREVALUATION:
                        ContainerEvaluationServiceClient = new ContainerEvaluationService();
                        ContainerEvaluationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerEvaluationServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                        //SalContainerEvalHdrObj.CVH_PK = CurrPK;
                        SalContainerEvalHdrObj.CVH_SHIPPING_PLAN = ShippingPlanPK;
                        SalContainerEvalHdrObj.CVH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        SalContainerEvalHdrList = ContainerEvaluationServiceClient.GetContainerEvaluationList(SalContainerEvalHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region SalContainerInspHdr List
                    case ControlsEnum.CONTAINERINSPECTION:
                        ContainerInspectionServiceClient = new ContainerInspectionService();
                        ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        SalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                        SalContainerInspHdrObj.CSH_SHIPPING_PLAN = ShippingPlanPK;
                        SalContainerInspHdrObj.CSH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        SalContainerInspHdrList = ContainerInspectionServiceClient.GetContainerInspectionList(SalContainerInspHdrObj, serviceUtilityObj);
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
                        break;
                    #endregion

                    case ControlsEnum.HISCODE:
                        //get the HIS Code
                        //CommonServiceClient = new CommonService();
                        //admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToInt16(CommonConstants.ACTIVE), null, (int)ConstGroupType.HISCode, (int)ConstGroup.HIScode, currentUser.SBUID);
                        dtHSCodeList = BusinessLogic.CommonManagement.CommonBL.GetHSCodeList(currentUser.SBUID);

                        break;
                    #region Port of loading
                    case ControlsEnum.FROMPORT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.FromPort, 1, currentUser.SBUID);
                        break;
                    #endregion
                    case ControlsEnum.SHIPPINGHEADER:
                        saleOrderServiceClient = new SaleOrderService();
                        saleOrderServiceClient = CommonFunctions.InitiateClient(saleOrderServiceClient);
                        ShippingHdrObj = saleOrderServiceClient.GetShippingPlanHeader(ShippingPlanPK);
                        break;
                    case ControlsEnum.SHIPBY:
                        dtShipBy = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.ShipBy, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.PORTDETAILS:
                        dtFromPort = BusinessLogic.CommonManagement.CommonBL.GetPortDetailsByPK(Convert.ToInt16(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT), Convert.ToInt16(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;
                    case ControlsEnum.SHIPMENTTERMS:
                        dtShipmentTerms = BusinessLogic.Sales.SaleOrderBL.GetShipmentTermsBySaleOrder(SalDetailList.First().SAL_ORDER_HDR.SOH_PK);
                        break;
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
                saleOrderServiceClient = null;
                ContainerInspectionServiceClient = null;
                ContainerEvaluationServiceClient = null;
                finTrxServiceClient = null;
                deliveryOrderServiceClient = null;
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
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.DELIVERYORDERHDR:
                        BindGrid(ControlsEnum.DELIVERYORDERHDR);
                        break;
                    case ControlsEnum.DELIVERYORDERLIST:
                        BindGrid(ControlsEnum.DELIVERYORDERLIST);
                        break;
                    case ControlsEnum.CARRIER:
                        BindDropDown(ControlsEnum.CARRIER);
                        break;

                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANLEVEL);
                        break;
                    case ControlsEnum.CONTAINERTYPE:
                        BindDropDown(ControlsEnum.CONTAINERTYPE);
                        break;
                    #region Set SalContainerEvalHdr List
                    case ControlsEnum.CONTAINEREVALUATION:
                        GetUIValuesFromObject(ControlsEnum.CONTAINEREVALUATION);
                        break;
                    #endregion
                    #region Set SalContainerInspHdr List
                    case ControlsEnum.CONTAINERINSPECTION:
                        GetUIValuesFromObject(ControlsEnum.CONTAINERINSPECTION);
                        break;
                    #endregion
                    case ControlsEnum.DELIVERYORDERDETAILS:
                        GetUIValuesFromObject(ControlsEnum.DELIVERYORDERLIST);
                        break;
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.HISCODE:
                        BindDropDown(ControlsEnum.HISCODE);
                        break;
                    case ControlsEnum.FROMPORT:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.SHIPBY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.SHIPMENTTERMS:
                        if (dtShipmentTerms != null && dtShipmentTerms.Rows.Count > 0)
                            hdfShippingTermValue.Value = dtShipmentTerms.Rows[0]["CON_VALUE"].ToString();
                        else
                            hdfShippingTermValue.Value = "-1";
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
                int i = 0;
                bool bIsChecked = false;
                int packingSpec = 0;
                byte SwapByer = 0;

                switch (controlType)
                {
                    #region Sales Despatch Header
                    case ControlsEnum.SALDESPATCHHDR:

                        salDespatchHdrObj.DPH_PK = CurrPK;
                        salDespatchHdrObj.DPH_SHIPPING_PLAN = ShippingPlanPK;
                        //if (CurrPK == 0)
                        //{
                        //    updateDespatch = true;
                        //    GetFieldValues(ControlsEnum.DESPATCHNO);
                        //    lblDeliveryOrderNo.Text = hdfDespatchNo.Value;
                        //}
                        salDespatchHdrObj.DPH_NO = "";
                        if (SalDetailList != null && SalDetailList.Count > 0)
                        {
                            SalOrderDtlList = (List<SAL_ORDER_DTL>)SalDetailList;
                            salDespatchHdrObj.DPH_CUSTOMER = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            foreach (SAL_ORDER_DTL salOrderDtlObj in SalOrderDtlList)
                            {
                                if (!salDespatchHdrObj.DPH_REFERENCE.Contains(salOrderDtlObj.SOD_NO))
                                {
                                    salDespatchHdrObj.DPH_REFERENCE += string.IsNullOrEmpty(salDespatchHdrObj.DPH_REFERENCE) ?
                                        salOrderDtlObj.SOD_NO + (string.IsNullOrEmpty(salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE) ?
                                        "" : (" (" + GetLocalResourceObject("PONo").ToString() + " " + salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE + ")"))
                                        : ", " + salOrderDtlObj.SOD_NO + (string.IsNullOrEmpty(salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE) ?
                                        "" : " " + (" (" + GetLocalResourceObject("PONo").ToString() + " " + salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE + ")"));
                                }
                            }
                            salDespatchHdrObj.DPH_REF_NO = SalOrderDtlList[0].SOD_NO;
                        }
                        if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                        {
                            salDespatchDtlList = (List<SAL_DESPATCH_DTL>)SaleDespatchDtlList;
                            salDespatchHdrObj.DPH_CUSTOMER = salDespatchDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            //salDespatchHdrObj.DPH_REFERENCE = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REFERENCE;
                            //salDespatchHdrObj.DPH_REF_NO = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REF_NO;
                        }

                        salDespatchHdrObj.DPH_ENCLOS_TERM_TEXT = txtEnclosure.Text;


                        salDespatchHdrObj.DPH_NO = lblDeliveryOrderNo.Text;
                        salDespatchHdrObj.DPH_TO_PORT = lblDestinationPort.Text;

                        //  salDespatchHdrObj.DPH_FROM_PORT = Convert.ToInt32(hdfFromPortID.Value);
                        //As per the discussion with Manoj sir, need to change from port, after workflow submit
                        //previous code from the hidden field hdfPortOfLoading
                        if (!string.IsNullOrEmpty(hdfFromPortID.Value) && Convert.ToInt32(hdfFromPortID.Value) > 0)
                        {
                            salDespatchHdrObj.DPH_FROM_PORT = Convert.ToInt32(hdfFromPortID.Value);
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

                        salDespatchHdrObj.DPH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainer.Text.Trim());
                        salDespatchHdrObj.DPH_SEAL_NO = txtSealNo.Text.Trim() == string.Empty ? null : HttpUtility.HtmlEncode(txtSealNo.Text.Trim());

                        if (!string.IsNullOrEmpty(hdfCompany.Value) && Convert.ToInt32(hdfCompany.Value) > 0)
                        {
                            salDespatchHdrObj.DPH_COMP = Convert.ToInt32(hdfCompany.Value);
                        }

                        salDespatchHdrObj.DPH_DRIVER = txtDriver.Text.Trim() == string.Empty ? null : txtDriver.Text.Trim();
                        salDespatchHdrObj.DPH_REMARKS = txtRemarks.Text == string.Empty ? null : txtRemarks.Text;
                        salDespatchHdrObj.DPH_LORRY_NO = txtLorryNo.Text.Trim() == string.Empty ? null : txtLorryNo.Text.Trim();

                        salDespatchHdrObj.DPH_FEEDER_VESSEL = txtFeederVessel.Text.Trim() == string.Empty ? null : txtFeederVessel.Text.Trim();
                        salDespatchHdrObj.DPH_MOTHER_VESSEL = txtMotherVessel.Text.Trim() == string.Empty ? null : txtMotherVessel.Text.Trim();
                        if (!string.IsNullOrEmpty(txtDateOfShipment.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_SHIPMENT_DATE = Convert.ToDateTime(txtDateOfShipment.Text.Trim());
                        }

                        salDespatchHdrObj.DPH_BOOKING_NO = HttpUtility.HtmlEncode(txtBookingNo.Text.Trim()) == string.Empty ? null : HttpUtility.HtmlEncode(txtBookingNo.Text.Trim());
                        if (!string.IsNullOrEmpty(txtBookingDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_BOOKING_DATE = Convert.ToDateTime(txtBookingDate.Text.Trim());
                        }
                        salDespatchHdrObj.DPH_SHIPPING_MARK = HttpUtility.HtmlEncode(txtShippingMark.Text.Trim());

                        salDespatchHdrObj.DPH_STATUS = 0;
                        salDespatchHdrObj.DPH_ACTIVE = 1;
                        salDespatchHdrObj.DPH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        salDespatchHdrObj.DPH_CRTD_DT = DateTime.Now;
                        salDespatchHdrObj.DPH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        salDespatchHdrObj.DPH_MOD_DT = LastModifiedTime;
                        salDespatchHdrObj.DPH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        salDespatchHdrObj.DPH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        if (!string.IsNullOrEmpty(hdfConsignee.Value))
                            salDespatchHdrObj.DPH_CONSIGNEE = Convert.ToInt16(hdfConsignee.Value);
                        salDespatchHdrObj.DPH_CONSIGNEE_NAME = txtConsignee.Text == string.Empty ? null : txtConsignee.Text;
                        salDespatchHdrObj.DPH_CONSIGNEE_ADDRESS = txtConsigneeDetails.Text == string.Empty ? null : txtConsigneeDetails.Text;


                        if (Session["DPH_CONSIGNEE_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_COUNTRY = Convert.ToInt16(Session["DPH_CONSIGNEE_COUNTRY"].ToString());

                        if (Session["DPH_CONSIGNEE_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_FAX"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_FAX = Session["DPH_CONSIGNEE_FAX"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_FAX = null;
                        if (Session["DPH_CONSIGNEE_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_PHONE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_PHONE = Session["DPH_CONSIGNEE_PHONE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_PHONE = null;
                        if (Session["DPH_CONSIGNEE_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_MOBILE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_MOBILE = Session["DPH_CONSIGNEE_MOBILE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_MOBILE = null;
                        if (Session["DPH_CONSIGNEE_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_EMAIL"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_EMAIL = Session["DPH_CONSIGNEE_EMAIL"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_EMAIL = null;
                        if (Session["DPH_CONSIGNEE_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_ZIP"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_ZIP = Session["DPH_CONSIGNEE_ZIP"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_ZIP = null;
                        if (!string.IsNullOrEmpty(hdfNotifyParty.Value))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY = Convert.ToInt16(hdfNotifyParty.Value);
                        salDespatchHdrObj.DPH_NOTIFY_PARTY_NAME = txtNotifyParty.Text == string.Empty ? null : txtNotifyParty.Text;
                        salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = txtNotifyPartyDetails.Text == string.Empty ? null : txtNotifyPartyDetails.Text;
                        //if (Session["DPH_NOTIFY_PARTY_ADDRESS"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_ADDRESS"].ToString().Trim()))
                        //    salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = Session["DPH_NOTIFY_PARTY_ADDRESS"].ToString();
                        //else
                        //    salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = null;
                        if (Session["DPH_NOTIFY_PARTY_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_COUNTRY = Convert.ToInt16(Session["DPH_NOTIFY_PARTY_COUNTRY"].ToString());

                        if (Session["DPH_NOTIFY_PARTY_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_FAX"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_FAX = Session["DPH_NOTIFY_PARTY_FAX"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_FAX = null;
                        if (Session["DPH_NOTIFY_PARTY_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_PHONE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_PHONE = Session["DPH_NOTIFY_PARTY_PHONE"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_PHONE = null;
                        if (Session["DPH_NOTIFY_PARTY_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_MOBILE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_MOBILE = Session["DPH_NOTIFY_PARTY_MOBILE"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_MOBILE = null;
                        if (Session["DPH_NOTIFY_PARTY_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_EMAIL"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_EMAIL = Session["DPH_NOTIFY_PARTY_EMAIL"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_EMAIL = null;
                        if (Session["DPH_NOTIFY_PARTY_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_ZIP"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ZIP = Session["DPH_NOTIFY_PARTY_ZIP"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ZIP = null;
                        if (!string.IsNullOrEmpty(hdfPaymentTerms.Value))
                            salDespatchHdrObj.DPH_PAYMENT_TERM = Convert.ToInt16(hdfPaymentTerms.Value);
                        salDespatchHdrObj.DPH_PAYMENT_TERM_TEXT = txtPaymentTerms.Text;

                        salDespatchHdrObj.DPH_SUPP_DTL = txtSupplimentary.Text == string.Empty ? null : txtSupplimentary.Text;

                        if (ddlShipBy.SelectedIndex != 0)
                            salDespatchHdrObj.DPH_SHIP_BY = Convert.ToInt16(ddlShipBy.SelectedItem.Value);
                        //if (!string.IsNullOrEmpty(hdfModeofTransport.Value))
                        //salDespatchHdrObj.DPH_SHIP_BY = Convert.ToInt16(hdfModeofTransport.Value);

                        salDespatchHdrObj.DPH_CUSTOMER_NAME = lblDeliveryTo.Text;
                        if (!string.IsNullOrEmpty(txtDeliveryAddress.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_CUSTOMER_ADDRESS = txtDeliveryAddress.Text.Trim();
                            // salDespatchHdrObj.DPH_SHIPPING_ADDRESS = txtDeliveryAddress.Text.Trim();
                        }
                        if (ChkPrintShipAddress.Checked)
                        {
                            if (Session["DPH_SHIPPING_ADDRESS"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_ADDRESS"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_ADDRESS = Session["DPH_SHIPPING_ADDRESS"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_ADDRESS = null;
                            if (Session["DPH_SHIPPING_NAME"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_NAME"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_NAME = Session["DPH_SHIPPING_NAME"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_NAME = null;
                            if (Session["DPH_SHIPPING_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_FAX"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_FAX = Session["DPH_SHIPPING_FAX"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_FAX = null;
                            if (Session["DPH_SHIPPING_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_EMAIL"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_EMAIL = Session["DPH_SHIPPING_EMAIL"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_EMAIL = null;
                            if (Session["DPH_SHIPPING_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_MOBILE"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_MOBILE = Session["DPH_SHIPPING_MOBILE"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_MOBILE = null;
                            if (Session["DPH_SHIPPING_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_PHONE"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_PHONE = Session["DPH_SHIPPING_PHONE"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_PHONE = null;
                            if (Session["DPH_SHIPPING_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_ZIP"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_ZIP = Session["DPH_SHIPPING_ZIP"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_ZIP = null;
                            if (Session["DPH_SHIPPING_COUNTRY"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_COUNTRY"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_COUNTRY = Convert.ToInt32(Session["DPH_SHIPPING_COUNTRY"].ToString());
                            else
                                salDespatchHdrObj.DPH_SHIPPING_COUNTRY = null;
                        }
                        else
                        {
                            salDespatchHdrObj.DPH_SHIPPING_ADDRESS = null;
                            salDespatchHdrObj.DPH_SHIPPING_NAME = null;
                            salDespatchHdrObj.DPH_SHIPPING_FAX = null;
                            salDespatchHdrObj.DPH_SHIPPING_EMAIL = null;
                            salDespatchHdrObj.DPH_SHIPPING_MOBILE = null;
                            salDespatchHdrObj.DPH_SHIPPING_PHONE = null;
                            salDespatchHdrObj.DPH_SHIPPING_ZIP = null;
                            salDespatchHdrObj.DPH_SHIPPING_COUNTRY = null;
                        }
                        salDespatchHdrObj.DPH_PRINT_SHIP_TO = ChkPrintShipAddress.Checked;

                        if (Session["DPH_CUSTOMER_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_COUNTRY = Convert.ToInt16(Session["DPH_CUSTOMER_COUNTRY"].ToString());
                        if (Session["DPH_CUSTOMER_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_FAX"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_FAX = Session["DPH_CUSTOMER_FAX"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_FAX = null;
                        if (Session["DPH_CUSTOMER_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_PHONE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_PHONE = Session["DPH_CUSTOMER_PHONE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_PHONE = null;
                        if (Session["DPH_CUSTOMER_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_MOBILE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_MOBILE = Session["DPH_CUSTOMER_MOBILE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_MOBILE = null;
                        if (Session["DPH_CUSTOMER_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_EMAIL"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_EMAIL = Session["DPH_CUSTOMER_EMAIL"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_EMAIL = null;
                        if (Session["DPH_CUSTOMER_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_ZIP"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_ZIP = Session["DPH_CUSTOMER_ZIP"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_ZIP = null;
                        salDespatchHdrObj.DPH_PORT_OF_DISCHARGE = txtPortofDischarge.Text.Trim();
                        salDespatchHdrObj.DPH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);

                        //Additional Details
                        if (Convert.ToInt32(ddlHIS.SelectedValue) >= 0)
                        {
                            salDespatchHdrObj.DPH_HIS_CODE = Convert.ToInt32(ddlHIS.SelectedValue);
                        }
                        else
                        {
                            salDespatchHdrObj.DPH_HIS_CODE = null;
                        }
                        salDespatchHdrObj.DPH_SHIPPED_BOARD = HttpUtility.HtmlEncode(txtShippedBoard.Text.Trim());
                        salDespatchHdrObj.DPH_TRANSHIPMENT = HttpUtility.HtmlEncode(txtTranshipmentto.Text.Trim());
                        salDespatchHdrObj.DPH_ADNL_BUYER = HttpUtility.HtmlEncode(txtBuyer.Text.Trim());

                        if (cbkSwapByer.Checked)
                            SwapByer = 1;
                        salDespatchHdrObj.DPH_SWAP_BUYER = SwapByer;

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
                        salDespatchHdrObj.DPH_SHIPPING_PLAN = ShippingPlanPK;
                        updateDespatch = true;
                        if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text) || lblDeliveryOrderNo.Text == "[NEW]")
                        {
                            GetFieldValues(ControlsEnum.DESPATCHNO);
                            lblDeliveryOrderNo.Text = hdfDespatchNo.Value;
                        }
                        salDespatchHdrObj.DPH_NO = "";
                        if (SalDetailList != null && SalDetailList.Count > 0)
                        {
                            SalOrderDtlList = (List<SAL_ORDER_DTL>)SalDetailList;
                            salDespatchHdrObj.DPH_CUSTOMER = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            foreach (SAL_ORDER_DTL salOrderDtlObj in SalOrderDtlList)
                            {
                                if (!salDespatchHdrObj.DPH_REFERENCE.Contains(salOrderDtlObj.SOD_NO))
                                {
                                    salDespatchHdrObj.DPH_REFERENCE += string.IsNullOrEmpty(salDespatchHdrObj.DPH_REFERENCE) ?
                                        salOrderDtlObj.SOD_NO + (string.IsNullOrEmpty(salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE) ?
                                        "" : (" (" + GetLocalResourceObject("PONo").ToString() + " " + salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE + ")"))
                                        : ", " + salOrderDtlObj.SOD_NO + (string.IsNullOrEmpty(salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE) ?
                                        "" : " " + (" (" + GetLocalResourceObject("PONo").ToString() + " " + salOrderDtlObj.SAL_ORDER_HDR.SOH_REFERENCE + ")"));
                                }
                            }
                            salDespatchHdrObj.DPH_REF_NO = SalOrderDtlList[0].SOD_NO;
                        }
                        if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                        {
                            salDespatchDtlList = (List<SAL_DESPATCH_DTL>)SaleDespatchDtlList;
                            salDespatchHdrObj.DPH_CUSTOMER = salDespatchDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            salDespatchHdrObj.DPH_REFERENCE = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REFERENCE;
                            salDespatchHdrObj.DPH_REF_NO = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REF_NO;
                        }

                        salDespatchHdrObj.DPH_ENCLOS_TERM_TEXT = txtEnclosure.Text;

                        salDespatchHdrObj.DPH_NO = lblDeliveryOrderNo.Text;
                        salDespatchHdrObj.DPH_TO_PORT = lblDestinationPort.Text;
                        if (!string.IsNullOrEmpty(hdfFromPortID.Value) && Convert.ToInt32(hdfFromPortID.Value) > 0)
                        {
                            salDespatchHdrObj.DPH_FROM_PORT = Convert.ToInt32(hdfFromPortID.Value);

                        }
                        else
                        {
                            txtFromPort.Text = "Select/Type";
                        }
                        //if (!string.IsNullOrEmpty(hdfPortOfLoading.Value))
                        //{
                        //    salDespatchHdrObj.DPH_FROM_PORT = Convert.ToInt32(hdfPortOfLoading.Value);
                        //}
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

                        salDespatchHdrObj.DPH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainer.Text.Trim());
                        salDespatchHdrObj.DPH_SEAL_NO = txtSealNo.Text.Trim() == string.Empty ? null : HttpUtility.HtmlEncode(txtSealNo.Text.Trim());

                        if (!string.IsNullOrEmpty(hdfCompany.Value) && Convert.ToInt32(hdfCompany.Value) > 0)
                        {
                            salDespatchHdrObj.DPH_COMP = Convert.ToInt32(hdfCompany.Value);
                        }

                        salDespatchHdrObj.DPH_DRIVER = txtDriver.Text.Trim() == string.Empty ? null : txtDriver.Text.Trim();
                        salDespatchHdrObj.DPH_REMARKS = txtRemarks.Text == string.Empty ? null : txtRemarks.Text;
                        salDespatchHdrObj.DPH_LORRY_NO = txtLorryNo.Text.Trim() == string.Empty ? null : txtLorryNo.Text.Trim();

                        salDespatchHdrObj.DPH_FEEDER_VESSEL = txtFeederVessel.Text.Trim() == string.Empty ? null : txtFeederVessel.Text.Trim();
                        salDespatchHdrObj.DPH_MOTHER_VESSEL = txtMotherVessel.Text.Trim() == string.Empty ? null : txtMotherVessel.Text.Trim();
                        if (!string.IsNullOrEmpty(txtDateOfShipment.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_SHIPMENT_DATE = Convert.ToDateTime(txtDateOfShipment.Text.Trim());
                        }

                        salDespatchHdrObj.DPH_BOOKING_NO = HttpUtility.HtmlEncode(txtBookingNo.Text.Trim()) == string.Empty ? null : HttpUtility.HtmlEncode(txtBookingNo.Text.Trim());
                        if (!string.IsNullOrEmpty(txtBookingDate.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_BOOKING_DATE = Convert.ToDateTime(txtBookingDate.Text.Trim());
                        }
                        salDespatchHdrObj.DPH_SHIPPING_MARK = HttpUtility.HtmlEncode(txtShippingMark.Text.Trim());

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
                        salDespatchHdrObj.DPH_CONSIGNEE_NAME = txtConsignee.Text == string.Empty ? null : txtConsignee.Text;
                        salDespatchHdrObj.DPH_CONSIGNEE_ADDRESS = txtConsigneeDetails.Text == string.Empty ? null : txtConsigneeDetails.Text;

                        //if (Session["DPH_CONSIGNEE_ADDRESS"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_ADDRESS"].ToString().Trim()))
                        //salDespatchHdrObj.DPH_CONSIGNEE_ADDRESS = Session["DPH_CONSIGNEE_ADDRESS"].ToString();
                        //else
                        //salDespatchHdrObj.DPH_CONSIGNEE_ADDRESS = null;
                        if (Session["DPH_CONSIGNEE_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CONSIGNEE_COUNTRY = Convert.ToInt16(Session["DPH_CONSIGNEE_COUNTRY"].ToString());
                        if (Session["DPH_CONSIGNEE_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_FAX"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_FAX = Session["DPH_CONSIGNEE_FAX"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_FAX = null;
                        if (Session["DPH_CONSIGNEE_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_PHONE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_PHONE = Session["DPH_CONSIGNEE_PHONE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_PHONE = null;
                        if (Session["DPH_CONSIGNEE_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_MOBILE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_MOBILE = Session["DPH_CONSIGNEE_MOBILE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_MOBILE = null;
                        if (Session["DPH_CONSIGNEE_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_EMAIL"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_EMAIL = Session["DPH_CONSIGNEE_EMAIL"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_EMAIL = null;
                        if (Session["DPH_CONSIGNEE_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_CONSIGNEE_ZIP"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CONSIGNEE_ZIP = Session["DPH_CONSIGNEE_ZIP"].ToString();
                        else
                            salDespatchHdrObj.DPH_CONSIGNEE_ZIP = null;
                        if (!string.IsNullOrEmpty(hdfNotifyParty.Value))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY = Convert.ToInt16(hdfNotifyParty.Value);
                        salDespatchHdrObj.DPH_NOTIFY_PARTY_NAME = txtNotifyParty.Text == string.Empty ? null : txtNotifyParty.Text;
                        salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = txtNotifyPartyDetails.Text == string.Empty ? null : txtNotifyPartyDetails.Text;

                        //if (Session["DPH_NOTIFY_PARTY_ADDRESS"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_ADDRESS"].ToString().Trim()))
                        //salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = Session["DPH_NOTIFY_PARTY_ADDRESS"].ToString();
                        //else
                        ////salDespatchHdrObj.DPH_NOTIFY_PARTY_ADDRESS = null;
                        if (Session["DPH_NOTIFY_PARTY_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_COUNTRY = Convert.ToInt16(Session["DPH_NOTIFY_PARTY_COUNTRY"].ToString());
                        if (Session["DPH_NOTIFY_PARTY_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_FAX"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_FAX = Session["DPH_NOTIFY_PARTY_FAX"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_FAX = null;
                        if (Session["DPH_NOTIFY_PARTY_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_PHONE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_PHONE = Session["DPH_NOTIFY_PARTY_PHONE"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_PHONE = null;
                        if (Session["DPH_NOTIFY_PARTY_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_MOBILE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_MOBILE = Session["DPH_NOTIFY_PARTY_MOBILE"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_MOBILE = null;
                        if (Session["DPH_NOTIFY_PARTY_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_EMAIL"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_EMAIL = Session["DPH_NOTIFY_PARTY_EMAIL"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_EMAIL = null;
                        if (Session["DPH_NOTIFY_PARTY_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_NOTIFY_PARTY_ZIP"].ToString().Trim()))
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ZIP = Session["DPH_NOTIFY_PARTY_ZIP"].ToString();
                        else
                            salDespatchHdrObj.DPH_NOTIFY_PARTY_ZIP = null;
                        if (!string.IsNullOrEmpty(hdfPaymentTerms.Value))
                            salDespatchHdrObj.DPH_PAYMENT_TERM = Convert.ToInt16(hdfPaymentTerms.Value);
                        salDespatchHdrObj.DPH_PAYMENT_TERM_TEXT = txtPaymentTerms.Text;

                        salDespatchHdrObj.DPH_SUPP_DTL = txtSupplimentary.Text == string.Empty ? null : txtSupplimentary.Text;

                        if (ddlShipBy.SelectedIndex != 0)
                            salDespatchHdrObj.DPH_SHIP_BY = Convert.ToInt16(ddlShipBy.SelectedItem.Value);
                        //if (!string.IsNullOrEmpty(hdfModeofTransport.Value))
                        //salDespatchHdrObj.DPH_SHIP_BY = Convert.ToInt16(hdfModeofTransport.Value);

                        salDespatchHdrObj.DPH_CUSTOMER_NAME = lblDeliveryTo.Text;
                        if (!string.IsNullOrEmpty(txtDeliveryAddress.Text.Trim()))
                        {
                            salDespatchHdrObj.DPH_CUSTOMER_ADDRESS = txtDeliveryAddress.Text.Trim();
                            salDespatchHdrObj.DPH_SHIPPING_ADDRESS = txtDeliveryAddress.Text.Trim();
                        }
                        salDespatchHdrObj.DPH_PRINT_SHIP_TO = ChkPrintShipAddress.Checked;
                        if (Session["DPH_CUSTOMER_COUNTRY"] != null)
                            salDespatchHdrObj.DPH_CUSTOMER_COUNTRY = Convert.ToInt16(Session["DPH_CUSTOMER_COUNTRY"].ToString());
                        if (Session["DPH_CUSTOMER_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_FAX"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_FAX = Session["DPH_CUSTOMER_FAX"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_FAX = null;
                        if (Session["DPH_CUSTOMER_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_PHONE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_PHONE = Session["DPH_CUSTOMER_PHONE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_PHONE = null;
                        if (Session["DPH_CUSTOMER_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_MOBILE"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_MOBILE = Session["DPH_CUSTOMER_MOBILE"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_MOBILE = null;
                        if (Session["DPH_CUSTOMER_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_EMAIL"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_EMAIL = Session["DPH_CUSTOMER_EMAIL"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_EMAIL = null;
                        if (Session["DPH_CUSTOMER_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_CUSTOMER_ZIP"].ToString().Trim()))
                            salDespatchHdrObj.DPH_CUSTOMER_ZIP = Session["DPH_CUSTOMER_ZIP"].ToString();
                        else
                            salDespatchHdrObj.DPH_CUSTOMER_ZIP = null;

                        salDespatchHdrObj.DPH_PORT_OF_DISCHARGE = txtPortofDischarge.Text.Trim();
                        salDespatchHdrObj.DPH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        //Additional Details
                        if (Convert.ToInt32(ddlHIS.SelectedValue) >= 0)
                        {
                            salDespatchHdrObj.DPH_HIS_CODE = Convert.ToInt32(ddlHIS.SelectedValue);
                        }
                        else
                        {
                            salDespatchHdrObj.DPH_HIS_CODE = null;
                        }
                        salDespatchHdrObj.DPH_SHIPPED_BOARD = HttpUtility.HtmlEncode(txtShippedBoard.Text.Trim());
                        salDespatchHdrObj.DPH_TRANSHIPMENT = HttpUtility.HtmlEncode(txtTranshipmentto.Text.Trim());
                        salDespatchHdrObj.DPH_ADNL_BUYER = HttpUtility.HtmlEncode(txtBuyer.Text.Trim());

                        if (ChkPrintShipAddress.Checked)
                        {
                            if (Session["DPH_SHIPPING_ADDRESS"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_ADDRESS"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_ADDRESS = Session["DPH_SHIPPING_ADDRESS"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_ADDRESS = null;
                            if (Session["DPH_SHIPPING_NAME"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_NAME"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_NAME = Session["DPH_SHIPPING_NAME"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_NAME = null;
                            if (Session["DPH_SHIPPING_FAX"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_FAX"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_FAX = Session["DPH_SHIPPING_FAX"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_FAX = null;
                            if (Session["DPH_SHIPPING_EMAIL"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_EMAIL"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_EMAIL = Session["DPH_SHIPPING_EMAIL"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_EMAIL = null;
                            if (Session["DPH_SHIPPING_MOBILE"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_MOBILE"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_MOBILE = Session["DPH_SHIPPING_MOBILE"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_MOBILE = null;
                            if (Session["DPH_SHIPPING_PHONE"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_PHONE"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_PHONE = Session["DPH_SHIPPING_PHONE"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_PHONE = null;
                            if (Session["DPH_SHIPPING_ZIP"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_ZIP"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_ZIP = Session["DPH_SHIPPING_ZIP"].ToString();
                            else
                                salDespatchHdrObj.DPH_SHIPPING_ZIP = null;
                            if (Session["DPH_SHIPPING_COUNTRY"] != null && !string.IsNullOrEmpty(Session["DPH_SHIPPING_COUNTRY"].ToString().Trim()))
                                salDespatchHdrObj.DPH_SHIPPING_COUNTRY = Convert.ToInt32(Session["DPH_SHIPPING_COUNTRY"].ToString());
                            else
                                salDespatchHdrObj.DPH_SHIPPING_COUNTRY = null;
                        }
                        else
                        {
                            salDespatchHdrObj.DPH_SHIPPING_ADDRESS = null;
                            salDespatchHdrObj.DPH_SHIPPING_NAME = null;
                            salDespatchHdrObj.DPH_SHIPPING_FAX = null;
                            salDespatchHdrObj.DPH_SHIPPING_EMAIL = null;
                            salDespatchHdrObj.DPH_SHIPPING_MOBILE = null;
                            salDespatchHdrObj.DPH_SHIPPING_PHONE = null;
                            salDespatchHdrObj.DPH_SHIPPING_ZIP = null;
                            salDespatchHdrObj.DPH_SHIPPING_COUNTRY = null;
                        }
                        salDespatchHdrObj.DPH_PRINT_SHIP_TO = ChkPrintShipAddress.Checked;

                        if (cbkSwapByer.Checked)
                            SwapByer = 1;
                        salDespatchHdrObj.DPH_SWAP_BUYER = SwapByer;

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
                        HiddenField hdfUOMSales;
                        HiddenField hdfSalesUonConvRate;
                        HiddenField hdfIsPackingMaterial;
                        TextBox txtDespNowSales;
                        TextBox txtLotNo;
                        foreach (GridViewRow grdrow in grdDeliveryList.Rows)
                        {
                            txtDespNowSales = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtDespNowSales");
                            txtDespNow = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtDespNow");
                            HiddenField hdfDpdPk = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfDpdPk");
                            if (txtDespNowSales != null && !string.IsNullOrEmpty(txtDespNowSales.Text.Trim()))// && Convert.ToDouble(txtDespNow.Text) > 0
                            {
                                if (Convert.ToDouble(txtDespNowSales.Text) > 0)
                                {
                                    salDespatchDtlObj = CommonFunctions.Initilize<SAL_DESPATCH_DTL>();

                                    salDespatchDtlObj.DPD_PK = string.IsNullOrEmpty(hdfDpdPk.Value) ? 0 : Convert.ToInt32(hdfDpdPk.Value); // CurrMpgPK;
                                    if (IsShowNetGrsWeight)
                                    {
                                        salDespatchDtlObj.DPD_NET_WT = double.Parse(((TextBox)grdDeliveryList.Rows[rowID].FindControl("txtNetWeight")).Text);
                                        salDespatchDtlObj.DPD_GROSS_WT = double.Parse(((TextBox)grdDeliveryList.Rows[rowID].FindControl("txtGrossWeight")).Text);
                                    }
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
                                    hdfIsPackingMaterial = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfIsPackingMaterial");
                                    hdfUOM = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfUOM");
                                    txtLotNo = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtLotNo");

                                    if (!string.IsNullOrEmpty(hdfBrandCode.Value) && hdfIsPackingMaterial.Value == "0")
                                    {
                                        salDespatchDtlObj.DPD_CUST_ITEM = Convert.ToInt32(hdfBrandCode.Value);
                                    }
                                    salDespatchDtlObj.DPD_ITEM = Convert.ToInt32(hdfIGPLCode.Value);
                                    salDespatchDtlObj.DPD_QTY_DESPATCHED = Convert.ToDouble(txtDespNow.Text);
                                    // salDespatchDtlObj.DPD_QTY_DESPATCHED = Convert.ToDouble(txtDespNowSales.Text); 
                                    salDespatchDtlObj.DPD_QTY_APPROVED = 0;
                                    salDespatchDtlObj.DPD_UOM = Convert.ToInt32(hdfUOM.Value);
                                    salDespatchDtlObj.DPD_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                                    salDespatchDtlObj.DPD_LOT_NO = txtLotNo.Text;

                                    hdfSalesUonConvRate = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfSalesUonConvRate");
                                    hdfIsPackingMaterial = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfIsPackingMaterial");
                                    salDespatchDtlObj.DPD_IS_PACK_MAT = Convert.ToByte(hdfIsPackingMaterial.Value);

                                    hdfUOMSales = (HiddenField)grdDeliveryList.Rows[rowID].FindControl("hdfUOMSales");
                                    salDespatchDtlObj.DPD_SALE_QTY = Convert.ToDouble(txtDespNowSales.Text);
                                    salDespatchDtlObj.DPD_SALE_UOM = string.IsNullOrEmpty(hdfUOMSales.Value) ? (int?)null : Convert.ToInt32(hdfUOMSales.Value);
                                    salDespatchDtlObj.DPD_SALE_UOM_CONV = Convert.ToDouble(hdfSalesUonConvRate.Value);
                                    if (SalDetailList != null)
                                    {
                                        SAL_ORDER_DTL salOrderDtl = SalDetailList.SingleOrDefault(aa => aa.SOD_PK == salDespatchDtlObj.DPD_SO_DTL);
                                        if (salOrderDtl != null)
                                        {
                                            if (salOrderDtl.ADM_PACK_SPEC_MST != null)
                                            {
                                                salDespatchDtlObj.DPD_PACKING_SPEC = salOrderDtl.ADM_PACK_SPEC_MST.APS_PK;
                                                if (salOrderDtl.ADM_PACK_CUST_ITEM_MAP != null)
                                                    salDespatchDtlObj.DPD_ART_WORK = salOrderDtl.ADM_PACK_CUST_ITEM_MAP.PIM_PK;
                                            }
                                        }
                                    }
                                    salDespatchDtlList.Add(salDespatchDtlObj);
                                    isSave = true;
                                }
                                rowID++;
                            }

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
        /// Set Totals
        /// </summary>
        /// <returns></returns>
        private void SetTotal()
        {
            //Set the Total Netweight & GrossWeight  

            //Label lblTotalNetWeight = ((Label)grdDeliveryList.FooterRow.FindControl("lblTotalNetWeightFooter"));
            //Label lblTotalGrossWeight = ((Label)grdDeliveryList.FooterRow.FindControl("lblTotalGrossWeightFooter"));
            //if (lblTotalNetWeight != null)
            //{
            //    lblTotalNetWeight.Text = lblTotalNetWeight.ToolTip = String.Format("{0:n3}", TotalNetWeight);
            //}
            //if (lblTotalGrossWeight != null)
            //{
            //    lblTotalGrossWeight.Text = lblTotalGrossWeight.ToolTip = String.Format("{0:n3}", TotalGrossWeight);
            //}


            // Label lblTotalPayNowFooterSplit = ((Label)grdDeliveryList.FooterRow.FindControl("lblTotalPayNowFooterSplit"));
            //Label lblTotalPayNowFooterSplitSales = ((Label)grdDeliveryList.FooterRow.FindControl("lblTotalPayNowFooterSplitSales"));
            //if (lblTotalPayNowFooterSplit != null)
            //{
            //    lblTotalPayNowFooterSplit.Text = lblTotalPayNowFooterSplit.ToolTip = String.Format("{0:n0}", TotalDespNowPcs);
            //}
            //if (lblTotalPayNowFooterSplitSales != null)
            //{
            //    lblTotalPayNowFooterSplitSales.Text = lblTotalPayNowFooterSplit.ToolTip = String.Format("{0:n0}", TotalDespNow);
            //}


            Label lblTotalOrderQtyFooter = ((Label)grdDeliveryList.FooterRow.FindControl("lblTotalOrderQtyFooter"));
            Label lblTotalDespatchedQtyFooter = ((Label)grdDeliveryList.FooterRow.FindControl("lblTotalDespatchedQtyFooter"));
            if (lblTotalOrderQtyFooter != null)
            {
                lblTotalOrderQtyFooter.Text = lblTotalOrderQtyFooter.ToolTip = String.Format("{0:n2}", TotalOrdrQty);
            }
            if (lblTotalDespatchedQtyFooter != null)
            {
                lblTotalDespatchedQtyFooter.Text = lblTotalDespatchedQtyFooter.ToolTip = String.Format("{0:n2}", TotalDespatcQty);
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
                            lblDeliveryTo.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.CRM_CUSTOMER_MST.CUS_NAME);
                            lblDeliveryOrderNo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NO == "" || salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NO == null ? "[NEW]" : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NO;
                            string address = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_ADDRESS) ? string.Empty : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_ADDRESS;
                            //string countryName = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST1 == null ? string.Empty : salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_COUNTRY_MST1.CNT_NAME;
                            //if (string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = string.Empty;
                            //else if (string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = countryName;
                            //else if (!string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = address;
                            //else
                            //    txtDeliveryAddress.Text = address + ","+countryName;
                            if (!string.IsNullOrEmpty(address))
                                txtDeliveryAddress.Text = HttpUtility.HtmlDecode(address);
                            else
                                txtDeliveryAddress.Text = string.Empty;

                            ChkPrintShipAddress.Checked = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PRINT_SHIP_TO;

                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CY_DATE.HasValue)
                            {
                                txtCYDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CY_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            lblDestinationPort.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_TO_PORT);
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_RTN_DATE.HasValue)
                            {
                                txtRtnDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_RTN_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT != null)
                            {
                                //lblPortOfLoading.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_NAME;
                                GetFieldValues(ControlsEnum.PORTDETAILS);
                                hdfPortOfLoading.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT.ToString();
                                if (dtFromPort != null && dtFromPort.Rows.Count > 0)
                                    txtFromPort.Text = dtFromPort.Rows[0]["PRM_NAME"].ToString();
                                //SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_NAME.ToString();
                                hdfFromPortID.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT.ToString();
                            }
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETD.HasValue)
                            {
                                txtETD.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETD.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETA.HasValue)
                            {
                                hdfETA.Value = txtETA.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ETA.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            lblFinalDestination.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FINAL_DESTINATION);

                            ddlCarrier.SelectedValue = (salDespatchDtlList[0].SAL_DESPATCH_HDR != null && salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CARRIER.HasValue) ?
                                salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CARRIER.Value.ToString() : CommonConstants.SELECTVAL;

                            txtContainer.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONTAINER_NO);
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_COMP.HasValue)
                            {
                                hdfCompany.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_COMP.Value.ToString();
                                txtCompany.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST != null ?
                                    HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST.VEN_NAME) : string.Empty;
                            }
                            else
                            {
                                hdfCompany.Value = string.Empty;
                                txtCompany.Text = string.Empty;
                            }

                            //if (salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST != null)
                            //{
                            //    ddlCarrier.SelectedValue = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST.CON_PK.ToString();
                            //}
                            //txtContainer.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONTAINER_NO;
                            txtSealNo.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SEAL_NO);

                            //if (salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST != null)
                            //{
                            //    hdfCompany.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST.VEN_PK.ToString();
                            //    txtCompany.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.PUR_VENDOR_MST.VEN_NAME;
                            //}
                            txtDriver.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_DRIVER;
                            txtRemarks.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_REMARKS;
                            txtLorryNo.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_LORRY_NO;

                            txtFeederVessel.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FEEDER_VESSEL;
                            txtMotherVessel.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_MOTHER_VESSEL;
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPMENT_DATE.HasValue)
                            {
                                txtDateOfShipment.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPMENT_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            }
                            txtBookingNo.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_NO);
                            //if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_DATE.HasValue)
                            //{
                            txtBookingDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_DATE == null ? string.Empty : ((DateTime)salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_DATE).ToString(Resources.Constants.DateFormatShort);
                            //    txtBookingDate.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_BOOKING_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                            //}
                            txtShippingMark.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_MARK);

                            LastModifiedTime = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);

                            hdfConsignee.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE == null ? null :
                                                salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE.ToString();
                            txtConsignee.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_NAME);
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SWAP_BUYER == 1)
                                cbkSwapByer.Checked = true;
                            //Session["DPH_SHIPPING_NAME"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_NAME;
                            //Session["DPH_SHIPPING_ADDRESS"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_ADDRESS;
                            //Session["DPH_SHIPPING_COUNTRY"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_COUNTRY;
                            //Session["DPH_SHIPPING_FAX"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_FAX;
                            //Session["DPH_SHIPPING_PHONE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_PHONE;
                            //Session["DPH_SHIPPING_MOBILE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_MOBILE;
                            //Session["DPH_SHIPPING_EMAIL"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_EMAIL;
                            //Session["DPH_SHIPPING_ZIP"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPING_ZIP;

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
                                    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS;
                                //string consigneeDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS) ? "" :
                                //    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS + Environment.NewLine;

                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX) ? "" :
                                //    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE) ? "" :
                                //    GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE) ? "" :
                                //    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL) ? "" :
                                //    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP) ? "" :
                                //    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;

                                txtConsigneeDetails.Text = HttpUtility.HtmlDecode(consigneeDetails);


                            }
                            else
                            {
                                string consigneeDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS) ? "" : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS;
                                //string consigneeDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS) ? "" : salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ADDRESS + Environment.NewLine;

                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX) ? "" :
                                //    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_FAX + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE) ? "" :
                                //    GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_PHONE + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE) ? "" :
                                //    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_MOBILE + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL) ? "" :
                                //    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_EMAIL + Environment.NewLine;
                                //consigneeDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP) ? "" :
                                //    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CONSIGNEE_ZIP;

                                txtConsigneeDetails.Text = HttpUtility.HtmlDecode(consigneeDetails);


                            }
                            hdfNotifyParty.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY == null ? null :
                                                   salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY.ToString();
                            txtNotifyParty.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_NAME);

                            Session["DPH_NOTIFY_PARTY_ADDRESS"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS;
                            Session["DPH_NOTIFY_PARTY_COUNTRY"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_COUNTRY;
                            Session["DPH_NOTIFY_PARTY_FAX"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX;
                            Session["DPH_NOTIFY_PARTY_PHONE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE;
                            Session["DPH_NOTIFY_PARTY_MOBILE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE;
                            Session["DPH_NOTIFY_PARTY_EMAIL"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL;
                            Session["DPH_NOTIFY_PARTY_ZIP"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;
                            if (SalOrderDtlList != null)
                            {
                                Session["DPH_SHIPPING_NAME"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_NAME;
                                Session["DPH_SHIPPING_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ADDRESS;
                                Session["DPH_SHIPPING_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_COUNTRY;
                                Session["DPH_SHIPPING_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_FAX;
                                Session["DPH_SHIPPING_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_PHONE;
                                Session["DPH_SHIPPING_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_MOBILE;
                                Session["DPH_SHIPPING_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_EMAIL;
                                Session["DPH_SHIPPING_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ZIP;
                            }
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_COUNTRY == null)
                            {
                                string partyDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS) ? "" :
                                    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS;
                                //string partyDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS) ? "" :
                                //    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX) ? "" :
                                //    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE) ? ""
                                //    : GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE) ? "" :
                                //    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL) ? "" :
                                //    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP) ? "" :
                                //    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;
                                txtNotifyPartyDetails.Text = partyDetails;
                            }
                            else
                            {
                                string partyDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS) ? "" :
                                    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS;
                                //string partyDetails = string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS) ? "" :
                                //    salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;

                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX) ? "" :
                                //    GetLocalResourceObject("Fax1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_FAX + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE) ? ""
                                //    : GetLocalResourceObject("Phone1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE) ? "" :
                                //    GetLocalResourceObject("Mobile1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL) ? "" :
                                //    GetLocalResourceObject("Email1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                                //partyDetails += string.IsNullOrEmpty(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP) ? "" :
                                //    GetLocalResourceObject("Zip1").ToString() + salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_NOTIFY_PARTY_ZIP;

                                txtNotifyPartyDetails.Text = partyDetails;
                            }

                            hdfPaymentTerms.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM == null ? null :
                                                   salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM.ToString();
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM_TEXT);
                            txtPaymentTerms.ToolTip = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PAYMENT_TERM_TEXT);
                            txtSupplimentary.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SUPP_DTL;

                            hdfModeofTransport.Value = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIP_BY == null ? null :
                                                       salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIP_BY.ToString();

                            GetFieldValues(ControlsEnum.SHIPBY);
                            if (salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIP_BY.HasValue)
                                shipByPK = Convert.ToInt32(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);
                            //txtModeofTransport.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST_1 == null ? "" :
                            //salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST_1.CON_NAME;


                            Session["DPH_CUSTOMER_COUNTRY"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_COUNTRY;
                            Session["DPH_CUSTOMER_FAX"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_FAX;
                            Session["DPH_CUSTOMER_PHONE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_PHONE;
                            Session["DPH_CUSTOMER_MOBILE"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_MOBILE;
                            Session["DPH_CUSTOMER_EMAIL"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_EMAIL;
                            Session["DPH_CUSTOMER_ZIP"] = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_CUSTOMER_ZIP;
                            txtEnclosure.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ENCLOS_TERM_TEXT;

                            txtPortofDischarge.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_PORT_OF_DISCHARGE);

                            ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_COMPANY.ToString())));

                            //AdditionalDeatils 
                            ddlHIS.SelectedIndex = Convert.ToInt32(ddlHIS.Items.IndexOf(ddlHIS.Items.FindByValue(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_HIS_CODE.ToString())));
                            txtTranshipmentto.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_TRANSHIPMENT);
                            txtBuyer.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_ADNL_BUYER);

                            txtShippedBoard.Text = HttpUtility.HtmlDecode(salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_SHIPPED_BOARD);
                        }
                        else if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        {
                            //txtFeederVessel.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FEEDER_VESSEL;
                            //txtMotherVessel.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_MOTHER_VESSEL;
                            lblFinalDestination.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION);
                            //txtContainer.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONTAINER_NO;
                            //if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST != null)
                            //{
                            //    ddlCarrier.SelectedValue = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_PK.ToString();
                            //}

                            CurrPK = 0;
                            lblDeliveryTo.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME);
                            lblDeliveryOrderNo.Text = Resources.Messages.DocGenerationNew;


                            //string address = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS) ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS;
                            //string countryName = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST1 == null ? string.Empty : ","+SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST1.CNT_NAME;
                            //if (string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = string.Empty;
                            //else if (string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = countryName;
                            //else if (!string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = address;
                            //else
                            //    txtDeliveryAddress.Text = address + countryName;

                            //txtDeliveryAddress.Text = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ADDRESS) ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ADDRESS;
                            string addr = "";
                            if (!string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS) :
                                    string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS.Trim()) ?
                                    string.Empty : ", " + HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS);
                            }
                            //(	SELECT	[CNT_NAME]
                            //        FROM	[ADM_COUNTRY_MST]
                            //        WHERE	[CNT_PK] =	[CUS_COUNTRY]	)		AS	[SOH_CUSTOMER_COUNTRY_TEXT]
                            //[CRM_CUSTOMER_MST]

                            //if (!string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.ADM_COUNTRY_MST.CNT_NAME))
                            //{
                            //    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.ADM_COUNTRY_MST.CNT_NAME) :
                            //        string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.ADM_COUNTRY_MST.CNT_NAME.Trim()) ?
                            //        string.Empty : ", " + HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.ADM_COUNTRY_MST.CNT_NAME);
                            //}


                            txtDeliveryAddress.Text = addr;// string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS) ? string.Empty : HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS);

                            lblDestinationPort.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_TO_PORT);

                            //if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST != null)
                            //{
                            //    lblPortOfLoading.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_NAME;
                            //    hdfPortOfLoading.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_PK.ToString();
                            //    //txtPortOfLoading.Text = txtPortOfLoading.ToolTip = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_NAME;

                            //}

                            if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT != null)
                            {
                                //lblPortOfLoading.Text = salDespatchDtlList[0].SAL_DESPATCH_HDR.ADM_CONST_MST1.CON_NAME;
                                hdfPortOfLoading.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK.ToString();
                                txtFromPort.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_NAME.ToString();
                                hdfFromPortID.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK.ToString();
                                //salDespatchDtlList[0].SAL_DESPATCH_HDR.DPH_FROM_PORT.ToString();
                            }


                            lblFinalDestination.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION);
                            txtPortofDischarge.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION);

                            txtShippingMark.Text = string.Empty;
                            if (GetGlobalResourceObject("ConfigurationsRes", "ShippingMarkLotNoGenerate").ToString() == "1")
                            {
                                //Get distinct LOT_NO
                                List<string> SalOrderDtllotnosList = SalOrderDtlList.Select(dtl => dtl.SOD_LOT_NO).Distinct().ToList();
                                foreach (string Obj in SalOrderDtllotnosList)
                                {
                                    if (string.IsNullOrEmpty(txtShippingMark.Text))
                                    {
                                        txtShippingMark.Text = HttpUtility.HtmlDecode(Obj);
                                    }
                                    else
                                    {
                                        txtShippingMark.Text = txtShippingMark.Text + Environment.NewLine + HttpUtility.HtmlDecode(Obj);
                                    }
                                }
                            }

                            Session["DPH_SHIPPING_NAME"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_NAME;
                            Session["DPH_SHIPPING_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ADDRESS;
                            Session["DPH_SHIPPING_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_COUNTRY;
                            Session["DPH_SHIPPING_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_FAX;
                            Session["DPH_SHIPPING_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_PHONE;
                            Session["DPH_SHIPPING_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_MOBILE;
                            Session["DPH_SHIPPING_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_EMAIL;
                            Session["DPH_SHIPPING_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ZIP;

                            hdfConsignee.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE.ToString();
                            txtConsignee.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_NAME);

                            Session["DPH_CONSIGNEE_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS;
                            Session["DPH_CONSIGNEE_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_COUNTRY;
                            Session["DPH_CONSIGNEE_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX;
                            Session["DPH_CONSIGNEE_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE;
                            Session["DPH_CONSIGNEE_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE;
                            Session["DPH_CONSIGNEE_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL;
                            Session["DPH_CONSIGNEE_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;
                            string consigneeDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS;
                            //string consigneeDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS + Environment.NewLine;

                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX) ? "" : GetLocalResourceObject("Fax1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE) ? "" : GetLocalResourceObject("Phone1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE) ? "" : GetLocalResourceObject("Mobile1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL) ? "" : GetLocalResourceObject("Email1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP) ? "" : GetLocalResourceObject("Zip1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;

                            txtConsigneeDetails.Text = HttpUtility.HtmlDecode(consigneeDetails);

                            hdfNotifyParty.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY.ToString();
                            txtNotifyParty.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_NAME);

                            Session["DPH_NOTIFY_PARTY_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS;
                            Session["DPH_NOTIFY_PARTY_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_COUNTRY;
                            Session["DPH_NOTIFY_PARTY_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX;
                            Session["DPH_NOTIFY_PARTY_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE;
                            Session["DPH_NOTIFY_PARTY_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE;
                            Session["DPH_NOTIFY_PARTY_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL;
                            Session["DPH_NOTIFY_PARTY_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;
                            string partyDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS;
                            //string partyDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;

                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX) ? "" : GetLocalResourceObject("Fax1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE) ? "" : GetLocalResourceObject("Phone1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE) ? "" : GetLocalResourceObject("Mobile1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL) ? "" : GetLocalResourceObject("Email1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP) ? "" : GetLocalResourceObject("Zip1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;

                            txtNotifyPartyDetails.Text = partyDetails;

                            hdfPaymentTerms.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM.ToString();
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM_TEXT);
                            txtPaymentTerms.ToolTip = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM_TEXT);

                            hdfModeofTransport.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY.ToString();

                            GetFieldValues(ControlsEnum.SHIPBY);
                            if (SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY.HasValue)
                                shipByPK = Convert.ToInt32(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);
                            //txtModeofTransport.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST_1 == null ? "" :
                            //SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST_1.CON_NAME;

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
                    #region Default
                    case ControlsEnum.DEFAULT:
                        if (dsLoadingPlan != null && dsLoadingPlan.Tables[0] != null && dsLoadingPlan.Tables[0].Rows.Count > 0)
                        {
                            lblCustomerHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 25);
                            lblCustomerHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CUS_TEXT].ToString(), 300);

                            lblDestinationPortHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 20);
                            lblDestinationPortHdr.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_SHIP_TO_PORT].ToString(), 300);
                            lblInTimeHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;
                            lblInTimeHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CVH_BOOKING_DATE]).ToString(Resources.Constants.DateFormatShort), 20) + " " + ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.CSH_IN_TIME]).ToString(Resources.Constants.TimeFormatShort), 20) : string.Empty;

                            lblContainerTypeValueHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString(), 20);
                            lblContainerTypeValueHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_CONTAINER_TYPE_TEXT].ToString();

                            lblShippingPlanNoHdr.Text = ERP.Utilities.CommonFunctions.GetShortString(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString(), 20);
                            lblShippingPlanNoHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_NO].ToString();

                            lblShippingPlanDateHdr.Text = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? ERP.Utilities.CommonFunctions.GetShortString(Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort), 20) : string.Empty;
                            lblShippingPlanDateHdr.ToolTip = dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString() != string.Empty ? Convert.ToDateTime(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_DATE].ToString()).ToString(Resources.Constants.DateFormatShort) : string.Empty;

                            //ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsLoadingPlan.Tables[0].Rows[0][ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_COMPANY].ToString())));
                            //lblDeliveryOrderNo.Text="
                        }

                        break;

                    #endregion
                    case ControlsEnum.DELIVERYORDERLIST:
                        if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        {
                            //txtFeederVessel.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FEEDER_VESSEL;
                            //txtMotherVessel.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_MOTHER_VESSEL;
                            lblFinalDestination.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION);
                            //txtContainer.Text = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONTAINER_NO;
                            //if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST != null)
                            //{
                            //    ddlCarrier.SelectedValue = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_PK.ToString();
                            //}

                            CurrPK = 0;
                            lblDeliveryTo.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME);
                            //ddlCompany.SelectedIndex
                            //string address = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS) ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS;
                            //string countryName = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST1 == null ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.ADM_COUNTRY_MST1.CNT_NAME;
                            //if (string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = string.Empty;
                            //else if (string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = countryName;
                            //else if (!string.IsNullOrEmpty(address) && string.IsNullOrEmpty(countryName))
                            //    txtDeliveryAddress.Text = address;
                            //else
                            //    txtDeliveryAddress.Text = address + countryName;

                            //  txtDeliveryAddress.Text = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ADDRESS) ? string.Empty : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIPPING_ADDRESS;
                            txtDeliveryAddress.Text = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS) ? string.Empty : HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CUSTOMER_ADDRESS);

                            lblDestinationPort.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_TO_PORT);

                            if (SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT != null)
                            {
                                // lblPortOfLoading.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_NAME;
                                hdfPortOfLoading.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK.ToString();
                                txtFromPort.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_NAME.ToString();
                                hdfFromPortID.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_FROM_PORT.PRM_PK.ToString();
                                //lblPortOfLoading.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_NAME;
                                //hdfPortOfLoading.Value = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST.CON_PK.ToString();
                            }

                            hdfSaleOrderType.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_TYPE.ToString();
                            lblFinalDestination.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION);
                            txtPortofDischarge.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_FINAL_DESTINATION);

                            hdfConsignee.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE.ToString();
                            txtConsignee.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_NAME);

                            Session["DPH_CONSIGNEE_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS;
                            Session["DPH_CONSIGNEE_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_COUNTRY;
                            Session["DPH_CONSIGNEE_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX;
                            Session["DPH_CONSIGNEE_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE;
                            Session["DPH_CONSIGNEE_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE;
                            Session["DPH_CONSIGNEE_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL;
                            Session["DPH_CONSIGNEE_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;
                            string consigneeDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS;
                            //string consigneeDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ADDRESS + Environment.NewLine;

                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX) ? "" : GetLocalResourceObject("Fax1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_FAX + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE) ? "" : GetLocalResourceObject("Phone1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_PHONE + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE) ? "" : GetLocalResourceObject("Mobile1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_MOBILE + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL) ? "" : GetLocalResourceObject("Email1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_EMAIL + Environment.NewLine;
                            //consigneeDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP) ? "" : GetLocalResourceObject("Zip1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_CONSIGNEE_ZIP;

                            txtConsigneeDetails.Text = HttpUtility.HtmlDecode(consigneeDetails);

                            hdfNotifyParty.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY.ToString();
                            txtNotifyParty.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_NAME);

                            Session["DPH_NOTIFY_PARTY_ADDRESS"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS;
                            Session["DPH_NOTIFY_PARTY_COUNTRY"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_COUNTRY;
                            Session["DPH_NOTIFY_PARTY_FAX"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX;
                            Session["DPH_NOTIFY_PARTY_PHONE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE;
                            Session["DPH_NOTIFY_PARTY_MOBILE"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE;
                            Session["DPH_NOTIFY_PARTY_EMAIL"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL;
                            Session["DPH_NOTIFY_PARTY_ZIP"] = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;
                            string partyDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS;
                            //string partyDetails = string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS) ? "" : SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ADDRESS + Environment.NewLine;

                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX) ? "" : GetLocalResourceObject("Fax1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_FAX + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE) ? "" : GetLocalResourceObject("Phone1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_PHONE + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE) ? "" : GetLocalResourceObject("Mobile1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_MOBILE + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL) ? "" : GetLocalResourceObject("Email1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_EMAIL + Environment.NewLine;
                            //partyDetails += string.IsNullOrEmpty(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP) ? "" : GetLocalResourceObject("Zip1").ToString() + SalOrderDtlList[0].SAL_ORDER_HDR.SOH_NOTIFY_PARTY_ZIP;

                            txtNotifyPartyDetails.Text = partyDetails;

                            hdfPaymentTerms.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM.ToString();
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_PAYMENT_TERM_TEXT);

                            hdfModeofTransport.Value = SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY.ToString();

                            GetFieldValues(ControlsEnum.SHIPBY);

                            if (SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY.HasValue)
                                shipByPK = Convert.ToInt32(SalOrderDtlList[0].SAL_ORDER_HDR.SOH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);

                            //txtModeofTransport.Text = SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST_1 == null ? "" :
                            //SalOrderDtlList[0].SAL_ORDER_HDR.ADM_CONST_MST_1.CON_NAME;



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
                    #region Shipping Plan Header
                    case ControlsEnum.SHIPPINGPLANLEVEL:
                        if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
                        {
                            txtContainerType.Text = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CONTAINER_TYPE_TEXT"].ToString();
                            hdfContainerType.Value = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CONTAINER_TYPE"].ToString();
                            //txtCompany.Text = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CUSTOMER_TEXT"].ToString();
                            if (CurrPK == 0)
                            {
                                hdfETA.Value = txtETA.Text = string.IsNullOrEmpty(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_ETA"].ToString()) ? string.Empty : Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_ETA"]).ToString(Resources.Constants.DateFormatShort);
                                txtFeederVessel.Text = string.IsNullOrEmpty(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_FEEDER_VESSEL"].ToString()) ? string.Empty : dsShippingPlanHDR.Tables[0].Rows[0]["SNH_FEEDER_VESSEL"].ToString();
                                txtMotherVessel.Text = string.IsNullOrEmpty(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_MOTHER_VESSEL"].ToString()) ? string.Empty : dsShippingPlanHDR.Tables[0].Rows[0]["SNH_MOTHER_VESSEL"].ToString();
                            }
                        }
                        break;
                    #endregion
                    #region Set SalContainerEvalHdr List
                    case ControlsEnum.CONTAINEREVALUATION:
                        GetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                        if (SalOrderDtlList[0].SAL_ORDER_HDR.SOH_TYPE == 2)
                        {
                            if (SalContainerEvalHdrList != null && SalContainerEvalHdrList.Count > 0)
                            {
                                if (SalContainerEvalHdrList[0].CVH_TRANS_COMP.HasValue)
                                {
                                    hdfCompany.Value = SalContainerEvalHdrList[0].CVH_TRANS_COMP.Value.ToString();
                                    txtCompany.Text = SalContainerEvalHdrList[0].PUR_VENDOR_MST == null ? string.Empty : HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].PUR_VENDOR_MST.VEN_NAME);
                                }
                                ddlCarrier.SelectedValue = SalContainerEvalHdrList[0].CVH_CARRIER == null ? "0" : SalContainerEvalHdrList[0].CVH_CARRIER.ToString();
                                txtContainer.Text = string.IsNullOrEmpty(SalContainerEvalHdrList[0].CVH_CONTAINER_NO) ? string.Empty : HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_CONTAINER_NO);
                                txtBookingDate.Text = SalContainerEvalHdrList[0].CVH_BOOKING_DATE == null ? string.Empty : ((DateTime)SalContainerEvalHdrList[0].CVH_BOOKING_DATE).ToString(Resources.Constants.DateFormatShort);
                                txtBookingNo.Text = HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_BOOKING_NO) == null ? string.Empty : HttpUtility.HtmlDecode(SalContainerEvalHdrList[0].CVH_BOOKING_NO.ToString());

                            }
                        }
                        else
                        {
                            GetFieldValues(ControlsEnum.SHIPPINGHEADER);
                            txtBookingNo.Text = HttpUtility.HtmlDecode(ShippingHdrObj.SNH_BOOKING_NO) == null ? string.Empty : HttpUtility.HtmlDecode(ShippingHdrObj.SNH_BOOKING_NO.ToString());
                            txtBookingDate.Text = ShippingHdrObj.SNH_PAC_DATE == null ? string.Empty : ((DateTime)ShippingHdrObj.SNH_PAC_DATE).ToString(Resources.Constants.DateFormatShort);

                        }


                        break;
                    #endregion
                    #region Set SalContainerInspHdr List
                    case ControlsEnum.CONTAINERINSPECTION:
                        if (SalContainerInspHdrList != null && SalContainerInspHdrList.Count > 0)
                        {
                            txtSealNo.Text = HttpUtility.HtmlDecode(SalContainerInspHdrList[0].CSH_SEAL_NO);
                            txtContainer.Text = HttpUtility.HtmlDecode(SalContainerInspHdrList[0].CSH_CONTAINER_NO);
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
                case ControlsEnum.SHIPBY:
                    ddlShipBy.Items.Clear();
                    if (dtShipBy != null)
                    {
                        ddlShipBy.DataSource = CommonFunctions.HtmlDecodeDataTable(dtShipBy, "CON_NAME");
                        ddlShipBy.DataTextField = "CON_NAME";
                        ddlShipBy.DataValueField = "CON_PK";
                        ddlShipBy.DataBind();
                    }
                    ddlShipBy.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (shipByPK > 0 && ddlShipBy.Items.FindByValue(shipByPK.ToString()) != null)
                        ddlShipBy.SelectedValue = shipByPK.ToString();
                    break;
                case ControlsEnum.FROMPORT:
                    //ddlFromPort.Items.Clear();
                    //if (dtPageData != null)
                    //{
                    //    ddlFromPort.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                    //    ddlFromPort.DataTextField = "CON_NAME";
                    //    ddlFromPort.DataValueField = "CON_PK";
                    //    ddlFromPort.DataBind();
                    //}

                    //if (Convert.ToInt32(hdfPortOfLoading.Value) > 0 && ddlFromPort.Items.FindByValue(hdfPortOfLoading.Value) != null)
                    //    ddlFromPort.SelectedValue = hdfPortOfLoading.Value;
                    break;
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
                case ControlsEnum.CONTAINERTYPE:
                    //ddlContainerType.Items.Clear();
                    //if (admConstMstList != null && admConstMstList.Count > 0)
                    //{
                    //    ddlContainerType.DataSource = admConstMstList;
                    //    ddlContainerType.DataTextField = Resources.DataFieldRes.ConstName;
                    //    ddlContainerType.DataValueField = Resources.DataFieldRes.ConstPK;
                    //    ddlContainerType.DataBind();
                    //}
                    //ddlContainerType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #region Company
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    break;
                #endregion


                case ControlsEnum.HISCODE:
                    ddlHIS.Items.Clear();
                    if (dtHSCodeList != null && dtHSCodeList.Rows.Count > 0)
                    {
                        ddlHIS.DataSource = dtHSCodeList;
                        ddlHIS.DataTextField = "GCM_CODE";
                        ddlHIS.DataValueField = "GCM_PK";
                        ddlHIS.DataBind();
                    }
                    ddlHIS.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                        else if (salShippingPlanDtlList != null && salShippingPlanDtlList.Count > 0)
                        {
                            //lblDispCustomerName.Text = SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME;
                            grdDeliveryList.DataSource = salShippingPlanDtlList;
                            grdDeliveryList.DataBind();
                            SetTotal();
                        }
                        //else if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        //{
                        //    //lblDispCustomerName.Text = SalOrderDtlList[0].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME;
                        //    grdDeliveryList.DataSource = SalOrderDtlList;
                        //    grdDeliveryList.DataBind();
                        //}
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
            try
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
                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanPK, ucrWrkf.ProcessID);
                ucrWrkf.FillWorkFlowDetails();
                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                {
                    ucrWrkf.ViewType = 1;
                    //btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.ViewType = 0;
                    ucrWrkf.ViewAction();
                    //EntryStatus = EntryStatus.VIEWMODE;
                    //btnSave.Visible = false;
                    pnlPrint.Visible = true;
                }
                ucrWrkf.ViewAction();
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
            txtPaymentTerms.ToolTip = "";
            txtNotifyParty.Text = "";
            hdfNotifyParty.Value = "";
            txtNotifyPartyDetails.Text = "";
            txtSupplimentary.Text = "";
            //txtModeofTransport.Text = "";
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

            ModifiedDatePnl.Visible = false;
        }
        /// <summary>
        /// Set Tab Visibility
        /// </summary>
        private void SetTabVisibility()
        {
            GetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
            SetFieldValues(ControlsEnum.SHIPPINGPLANLEVEL);
            if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables.Count > 0 && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
            {
                tabLevel = Convert.ToInt32(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_TRX_STATUS"]);
            }
            //spnShippingPlan.Visible = lnkShippingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ShippingPlan;
            spnContainerEval.Visible = lnkContainerEval.Visible = tabLevel >= (int)ShippingTabsEnum.PaymentCleared;
            spnContainerInspection.Visible = lnkContainerInspection.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerEvaluated;
            spnUploadQADocs.Visible = lnkUploadQADocs.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerInspected;
            spnUploadExportDocs.Visible = lnkUploadExportDocs.Visible = tabLevel >= (int)ShippingTabsEnum.QADocsUploaded;
            spnLoadingPlan.Visible = lnkLoadingPlan.Visible = tabLevel >= (int)ShippingTabsEnum.ExportDocsUploaded;
            spnUploadPhotographs.Visible = lnkUploadPhotographs.Visible = tabLevel >= (int)ShippingTabsEnum.LoadingPlanCompleted;
            spnDeliveryOrder.Visible = lnkDeliveryOrder.Visible = tabLevel >= (int)ShippingTabsEnum.PhotographsUploaded;
            spnContainerRelease.Visible = lnkContainerRelease.Visible = tabLevel >= (int)ShippingTabsEnum.DeliveryOrderCompleted;
            spnBillofLoading.Visible = lnkBillofLoading.Visible = tabLevel >= (int)ShippingTabsEnum.ContainerReleased;
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                DeliveryOrderService deliveryOrderServiceClient;
                deliveryOrderServiceClient = null;

                GridViewRow gvr;
                bool bIsChecked = false;
                int Status = 1;
                DropDownList ddlWkfAction;
                long result;
                string action;
                TextBox WrkfComments;
                isSave = false;
                string soPK;
                TextBox txtDespNow;
                Decimal Desptotal = 0;
                int rowID = 0;


                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
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
                        if (grdDeliveryList != null)
                            foreach (GridViewRow grdrow in grdDeliveryList.Rows)
                            {
                                txtDespNow = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtDespNow");
                                if (txtDespNow != null && !string.IsNullOrEmpty(txtDespNow.Text.Trim()))// && Convert.ToDouble(txtDespNow.Text) > 0
                                {
                                    Desptotal = Desptotal + Convert.ToDecimal(txtDespNow.Text);
                                }
                                else
                                {
                                    txtDespNow.Text = "0";
                                }
                                rowID++;
                            }
                        if (Desptotal > 0)
                        {
                            rowID = 0;
                            Desptotal = 0;

                            if (!IsValid)
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }

                            else//valid
                            {
                                result = 0;
                                if (grdDeliveryList.Rows.Count > 0)
                                {
                                    salDespatchHdrList = new List<SAL_DESPATCH_HDR>();
                                    deliveryOrderServiceClient = new DeliveryOrderService();
                                    deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                                    salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                                    //if (BusinessLogic.Shipping.ShippingPlanBL.IsContainerReleaseCartonExist(ShippingPlanPK))
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_ContainerReleaseExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    //    return;
                                    //}

                                    #region SaveDetails
                                    salDespatchHdrObj = (SAL_DESPATCH_HDR)SetUIValuesToObject(ControlsEnum.SALDESPATCHHDR);
                                    if (isSave)
                                    {
                                        if (salDespatchHdrObj != null)
                                        {
                                            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                                            //Check for Cr/Dr Note exist
                                            if (BusinessLogic.Shipping.ShippingPlanBL.IsCrDrNoteExist(salDespatchHdrObj.DPH_PK) > 0)
                                            {

                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("CrDrNoteExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }

                                            if (salDespatchDtlList != null && salDespatchDtlList.Count > 0 && salDespatchDtlList[0].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR != null)
                                            {
                                                if (salDespatchDtlList[0].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR.Where(inv => inv.ICH_DEL_STATUS == 0 && inv.ICH_HAS_JRNL_ENTRY).Count() > 0)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_InvoicePosted").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    return;
                                                }
                                                else if (salDespatchDtlList[0].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR.Where(inv => inv.ICH_DEL_STATUS == 0 && !inv.ICH_HAS_JRNL_ENTRY).Count() > 0)
                                                {
                                                    if ((hdfIscontYes.Value != "1"))
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyInvoiced", "$(document).ready(function(){ResetInvoice();});", true);
                                                        return;
                                                    }
                                                }
                                            }
                                            if (Convert.ToString(GetGlobalResourceObject("ConfigurationsRes", "PackingSpecValidationInDO")) == "1")
                                            {
                                                if (salDespatchHdrObj.SAL_DESPATCH_DTL.Where(r => !r.DPD_PACKING_SPEC.HasValue && r.DPD_IS_PACK_MAT != 1 && r.DPD_IS_PACK_MAT != 2).Count() > 0)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_EmptypackingSpec").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    return;
                                                }
                                            }
                                            salDespatchHdrList.Add(salDespatchHdrObj);
                                            #region Transaction Begin
                                            using (TransactionScope scope = new TransactionScope())
                                            {
                                                try
                                                {
                                                    //Concurrency Checking
                                                    if (salDespatchHdrObj.DPH_PK == 0)
                                                    {
                                                        if (deliveryOrderServiceClient.CheckforAlreadyDespatch(salDespatchHdrObj.DPH_SHIPPING_PLAN) > 0)
                                                        {

                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("ConcurrencyCheck").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                            return;
                                                        }
                                                    }

                                                    result = deliveryOrderServiceClient.SaveDespatchHdr(salDespatchHdrList);

                                                    //Commit Transaction
                                                    scope.Complete();
                                                    //IsSuccess = true;
                                                }
                                                catch (Exception ex)
                                                {
                                                    // IsSuccess = false;
                                                    // Handler for unknown exceptions
                                                    // Throws a new exception to client with class name - method name - server side exception process result as exception message
                                                    string Error = CommonFunctions.ProcessException(ex);
                                                    if (ex.Message.Contains(GetLocalResourceObject("ExceptionContainsDo").ToString()))
                                                    {
                                                        // "SalDespatchDtlManager-SaveDespatchHdr-Unknown Error-000-GTI_EXCEPTION_DETAILS :DO quanitity is less than Container Release quantity.<br/> 
                                                        //  First change Container release-GTI_EXCEPTION"
                                                        string str = ex.Message;
                                                        int start, end, length;
                                                        start = str.LastIndexOf(GetLocalResourceObject("MessageStartIndexText").ToString());
                                                        end = str.LastIndexOf(GetLocalResourceObject("MessageEndIndexText").ToString());
                                                        length = end - start;
                                                        string message = str.Substring(start, length);
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                                                    }
                                                    if (Error == "547")
                                                    {
                                                        string message = GetLocalResourceObject("ERR_Reference").ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + message + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                                                    }
                                                }
                                                finally
                                                {
                                                    //Disposing used objects
                                                    scope.Dispose();

                                                }
                                            }

                                            #endregion
                                            if (result > 0)
                                            {

                                                hdfIscontYes.Value = "0";
                                                GetFieldValues(ControlsEnum.SHIPPINGHEADER);
                                                if (ShippingHdrObj != null)
                                                {
                                                    if ((ShippingHdrObj.SNH_STATUS == 2) || (ShippingHdrObj.SNH_STATUS == 92) || (ShippingHdrObj.SNH_STATUS >= 99))
                                                    {
                                                        //update sales contract qtys
                                                        int reslt = BusinessLogic.Shipping.ShippingPlanBL.UpdateProductStockDetails(Convert.ToInt32(result), 7, 4, (int)From_ERP.DO);
                                                    }
                                                }

                                                //update DO  after invoice  
                                                int ResultDO = BusinessLogic.Shipping.ShippingPlanBL.UpdateModifyDODetails(Convert.ToInt32(result));

                                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DeliveryOrder);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                if (CurrPK > 0)
                                                {
                                                    deliveryOrderServiceClient.UpdateInvoiceDtls(salDespatchHdrList[0]); // for Final destination and Port of loading
                                                }
                                                ResetForm();
                                                SelectedSosFroDO = null;
                                                Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
                                                GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                                                if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                                                {
                                                    ModifiedDatePnl.Visible = true;
                                                    CurrPK = SaleDespatchDtlList.First().SAL_DESPATCH_HDR.DPH_PK;
                                                    salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                                                    salDespatchHdrObj.DPH_PK = CurrPK;
                                                    GetFieldValues(ControlsEnum.CARRIER);
                                                    SetFieldValues(ControlsEnum.CARRIER);
                                                    GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                                                    SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                                                }
                                                else
                                                {
                                                    Response.Redirect(Resources.PageURL.ShippingPlan);
                                                }

                                                //Shipping Plan Summary save for mobile app
                                                int resultSummary = BusinessLogic.Shipping.ShippingPlanBL.SaveSummary(ShippingPlanPK, Convert.ToInt32(hdfProcessID.Value));
                                                if (resultSummary <= 0)
                                                {

                                                    litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("msg_atleast_one_item_hav_quantity").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }

                                    #endregion


                                }

                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        else
                        {
                            rowID = 0;
                            Desptotal = 0;
                            litErrorMsg.Text = GetLocalResourceObject("Err_DespNow_Valid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        if (SelectedSosFroDO != null || ShippingPlanPK > 0)
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
                        GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                        if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                        {
                            ModifiedDatePnl.Visible = true;
                            CurrPK = SaleDespatchDtlList.First().SAL_DESPATCH_HDR.DPH_PK;
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.CARRIER);
                            SetFieldValues(ControlsEnum.CARRIER);
                            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                        }
                        else
                        {
                            Response.Redirect(Resources.PageURL.ShippingPlan);
                        }
                        break;

                    #endregion
                    #region Invoice Details
                    case ActionsEnum.DELIVERYDETAIL:

                        if (SelectedSosFroDO != null || ShippingPlanPK > 0)
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
                            if (SelectedSosFroDO != null || ShippingPlanPK > 0) //if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text))
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
                                if (SelectedSosFroDO != null || ShippingPlanPK > 0) //if (string.IsNullOrEmpty(lblDeliveryOrderNo.Text))
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
                    #region Tab navigation
                    case ActionsEnum.DEFAULT:
                        Response.Redirect(Resources.PageURL.SalesOrderListing);
                        break;
                    case ActionsEnum.SHIPPINGPLAN:
                        Response.Redirect(Resources.PageURL.ShippingPlan);
                        break;
                    case ActionsEnum.CONTAINEREVALUATION:
                        Response.Redirect(Resources.PageURL.ContainerEvaulation);
                        break;
                    case ActionsEnum.CONTAINERINSPECTION:
                        Response.Redirect(Resources.PageURL.ContainerInspection);
                        break;
                    case ActionsEnum.UPLOADQA:
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.QA;
                        Response.Redirect(Resources.PageURL.UploadQa);
                        break;
                    case ActionsEnum.UPLOADEXPORT:
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Export;
                        Response.Redirect(Resources.PageURL.UploadExport);
                        break;
                    case ActionsEnum.LOADINGPLAN:
                        Response.Redirect(Resources.PageURL.LoadingPlan);
                        break;
                    case ActionsEnum.UPLOADPHOTOGRAPHS:
                        Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Photographs;
                        Response.Redirect(Resources.PageURL.UploadPhotographs);
                        break;
                    case ActionsEnum.GOODOUTWARD:
                        Response.Redirect(Resources.PageURL.GoodOutward);
                        break;
                    case ActionsEnum.BL:
                        Response.Redirect(Resources.PageURL.BillofLoading);
                        break;
                    case ActionsEnum.CONTAINERRELEASE:
                        Response.Redirect(Resources.PageURL.ContainerRelease);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        deliveryOrderServiceClient = new DeliveryOrderService();
                        deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                        DespatchID = DespatchID == 0 ? CurrPK : DespatchID;
                        result = deliveryOrderServiceClient.DeleteSalDespatch(DespatchID);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DeliveryOrder);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                            ResetForm();
                            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                            {
                                ModifiedDatePnl.Visible = true;
                                CurrPK = SaleDespatchDtlList.First().SAL_DESPATCH_HDR.DPH_PK;
                                salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                                salDespatchHdrObj.DPH_PK = CurrPK;
                                GetFieldValues(ControlsEnum.CARRIER);
                                SetFieldValues(ControlsEnum.CARRIER);
                                GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                                SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            }
                            else
                            {
                                Response.Redirect(Resources.PageURL.ShippingPlan);
                            }
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
                        PrinterControl1.ShippingPlanID = ShippingPlanPK;
                        PrinterControl1.SetCommericalInvoice(PrinterControl1.ShippingPlanID);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
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
                                Status = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
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
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:

                        if (grdDeliveryList != null)
                            foreach (GridViewRow grdrow in grdDeliveryList.Rows)
                            {
                                txtDespNow = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtDespNow");
                                if (txtDespNow != null && !string.IsNullOrEmpty(txtDespNow.Text.Trim()))// && Convert.ToDouble(txtDespNow.Text) > 0
                                {
                                    Desptotal = Desptotal + Convert.ToDecimal(txtDespNow.Text);
                                }
                                else
                                {
                                    txtDespNow.Text = "0";
                                }
                                rowID++;
                            }
                        if (Desptotal > 0)
                        {
                            rowID = 0;
                            Desptotal = 0;
                            //Show WorkFlow Popup
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            rowID = 0;
                            Desptotal = 0;
                            litErrorMsg.Text = GetLocalResourceObject("Err_DespNow_Valid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:

                        if (grdDeliveryList != null)
                            foreach (GridViewRow grdrow in grdDeliveryList.Rows)
                            {
                                txtDespNow = (TextBox)grdDeliveryList.Rows[rowID].FindControl("txtDespNow");
                                if (txtDespNow != null && !string.IsNullOrEmpty(txtDespNow.Text.Trim()))// && Convert.ToDouble(txtDespNow.Text) > 0
                                {
                                    Desptotal = Desptotal + Convert.ToDecimal(txtDespNow.Text);
                                }
                                else
                                {
                                    txtDespNow.Text = "0";
                                }
                                rowID++;
                            }
                        if (Desptotal > 0)
                        {
                            rowID = 0;
                            Desptotal = 0;
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            //Show WorkFlow Popup                       
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            rowID = 0;
                            Desptotal = 0;
                            litErrorMsg.Text = GetLocalResourceObject("Err_DespNow_Valid").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
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
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (grdDeliveryList.Rows.Count > 0)
                                {
                                    salDespatchHdrList = new List<SAL_DESPATCH_HDR>();
                                    deliveryOrderServiceClient = new DeliveryOrderService();
                                    deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                                    salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();

                                    //if (BusinessLogic.Shipping.ShippingPlanBL.IsContainerReleaseCartonExist(ShippingPlanPK))
                                    //{
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_ContainerReleaseExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    //    return;
                                    //}

                                    #region Transaction Begin
                                    using (TransactionScope scope = new TransactionScope())
                                    {
                                        try
                                        {
                                            #region SaveDetails
                                            salDespatchHdrObj = (SAL_DESPATCH_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                            if (isSave)
                                            {
                                                if (salDespatchHdrObj != null)
                                                {

                                                    salDespatchHdrList.Add(salDespatchHdrObj);
                                                    //Concurrency Checking
                                                    if (salDespatchHdrObj.DPH_PK == 0)
                                                    {
                                                        if (deliveryOrderServiceClient.CheckforAlreadyDespatch(salDespatchHdrObj.DPH_SHIPPING_PLAN) > 0)
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("ConcurrencyCheck").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                            return;
                                                        }
                                                    }
                                                    if (Convert.ToString(GetGlobalResourceObject("ConfigurationsRes", "PackingSpecValidationInDO")) == "1")
                                                    {
                                                        if (salDespatchHdrObj.SAL_DESPATCH_DTL.Where(r => !r.DPD_PACKING_SPEC.HasValue && r.DPD_IS_PACK_MAT != 1 && r.DPD_IS_PACK_MAT != 2).Count() > 0)
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_EmptypackingSpec").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                            return;
                                                        }
                                                    }
                                                    result = deliveryOrderServiceClient.SaveDespatchHdr(salDespatchHdrList);
                                                    if (result > 0)// Save Success ! do WorkFlow
                                                    {
                                                        //Workflow submission
                                                        ucrWrkf.ApplicationID = ShippingPlanPK;
                                                        //int reslt = BusinessLogic.Shipping.ShippingPlanBL.UpdateProductStockDetails(Convert.ToInt32(result), 7, 4, (int)From_ERP.DO);                                                                                                               
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

                                            #endregion
                                            //Commit Transaction
                                            scope.Complete();
                                            //IsSuccess = true;                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            // IsSuccess = false;
                                            // Handler for unknown exceptions
                                            // Throws a new exception to client with class name - method name - server side exception process result as exception message
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        finally
                                        {
                                            //Disposing used objects
                                            scope.Dispose();
                                            #region Shipping Plan Summary save
                                            int resultSummary = BusinessLogic.Shipping.ShippingPlanBL.SaveSummary(ShippingPlanPK, Convert.ToInt32(hdfProcessID.Value));
                                            if (resultSummary <= 0)
                                            {

                                                litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                            }
                                            #endregion
                                        }
                                    }

                                    #endregion
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = ShippingPlanPK;

                            if (ucrWrkf.ApplicationID > 0)
                            {

                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        SetTabVisibility();
                                        WrkfComments.Text = "";
                                        ucrWrkf.FillWorkFlowDetails();
                                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                            ucrWrkf.ViewType = 1;
                                        else
                                        {
                                            ucrWrkf.ViewType = 0;
                                            //EntryStatus = EntryStatus.VIEWMODE;
                                        }
                                        ucrWrkf.ViewAction();
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.DeliveryOrder;
                                        args[1] = lblDeliveryOrderNo.Text;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);

                                        ResetForm();
                                        SelectedSosFroDO = null;
                                        Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                            GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                                            if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                                            {
                                                ModifiedDatePnl.Visible = true;
                                                CurrPK = SaleDespatchDtlList.First().SAL_DESPATCH_HDR.DPH_PK;
                                                salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                                                salDespatchHdrObj.DPH_PK = CurrPK;
                                                GetFieldValues(ControlsEnum.CARRIER);
                                                SetFieldValues(ControlsEnum.CARRIER);
                                                GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                                                SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                                                SetTabVisibility();
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);
                                                //Response.Redirect(Resources.PageURL.ShippingPlan);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DeliveryOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }


                            //if (grdDeliveryList.Rows.Count > 0)
                            //{
                            //salDespatchHdrList = new List<SAL_DESPATCH_HDR>();
                            //deliveryOrderServiceClient = new DeliveryOrderService();
                            //deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                            //salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            //salDespatchHdrObj = (SAL_DESPATCH_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                            //if (isSave)
                            //{
                            //if (salDespatchHdrObj != null)
                            //{
                            //salDespatchHdrList.Add(salDespatchHdrObj);
                            //result = deliveryOrderServiceClient.SaveDespatchHdr(salDespatchHdrList);
                            //if (result > 0)// Save Success ! do WorkFlow
                            //{
                            //    //Workflow submission
                            //    ucrWrkf.ApplicationID = ShippingPlanPK;
                            //ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            //WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            ////Do WorkFlow if WorkFlow has Actions
                            //if (ddlWkfAction.Items.Count > 0)
                            //{
                            //    action = ddlWkfAction.SelectedItem.ToString();
                            //    result = ucrWrkf.DoWorkFlow();
                            //    if (result > 0)
                            //    {
                            //        SetTabVisibility();
                            //        WrkfComments.Text = "";
                            //        ucrWrkf.FillWorkFlowDetails();
                            //        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            //            ucrWrkf.ViewType = 1;
                            //        else
                            //        {
                            //            ucrWrkf.ViewType = 0;
                            //            //EntryStatus = EntryStatus.VIEWMODE;
                            //        }
                            //        ucrWrkf.ViewAction();
                            //        //Show Save success message and reset Contract Entry
                            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                            //        object[] args = new object[2];
                            //        args[0] = Resources.PageNameRes.DeliveryOrder;
                            //        args[1] = lblDeliveryOrderNo.Text;
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);

                            //        ResetForm();
                            //        SelectedSosFroDO = null;
                            //        Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
                            //        GetFieldValues(ControlsEnum.DELIVERYORDERDETAILS);
                            //        if (SaleDespatchDtlList != null && SaleDespatchDtlList.Count > 0)
                            //        {
                            //            ModifiedDatePnl.Visible = true;
                            //            CurrPK = SaleDespatchDtlList.First().SAL_DESPATCH_HDR.DPH_PK;
                            //            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            //            salDespatchHdrObj.DPH_PK = CurrPK;
                            //            GetFieldValues(ControlsEnum.CARRIER);
                            //            SetFieldValues(ControlsEnum.CARRIER);
                            //            GetUIValuesFromObject(ControlsEnum.DELIVERYORDERDETAILS);
                            //            SetFieldValues(ControlsEnum.DELIVERYORDERLIST);
                            //            SetTabVisibility();
                            //        }
                            //        else
                            //        {
                            //            Response.Redirect(Resources.PageURL.ShippingPlan);
                            //        }
                            //    }
                            //}
                            //}
                            //else
                            //{
                            //    if (result == (int)DbSaveStatus.SQLERROR)
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CODEEXIST)
                            //    {
                            //        litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //    else
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.ErpRes.Information + "');", true);
                            //    }
                            //}
                            //}
                            //}
                            //else
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("msg_atleast_one_item_hav_quantity").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //}
                            //}
                            //else
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //}
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
                    #region Show SC Popup
                    case ActionsEnum.SHOWPOPUP:
                        //for SC Print
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
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


                        LinkButton lnkSoNo = e.Row.FindControl("lnkSoNo") as LinkButton;
                        Label lblSONo = e.Row.FindControl("lblSONo") as Label;
                        Label lblSODate = e.Row.FindControl("lblSODate") as Label;
                        Label lblIGPLCode = e.Row.FindControl("lblIGPLCode") as Label;
                        Label lblBrandCode = e.Row.FindControl("lblBrandCode") as Label;
                        Label lblUOM = e.Row.FindControl("lblUOM") as Label;

                        Label lblOrderQty = e.Row.FindControl("lblOrderQty") as Label;
                        Label lblDespatchedQty = e.Row.FindControl("lblDespatchedQty") as Label;
                        TextBox txtDespNow = e.Row.FindControl("txtDespNow") as TextBox;
                        TextBox txtDespNowSales = e.Row.FindControl("txtDespNowSales") as TextBox;
                        HiddenField hdfSaleOrderHdrPK = e.Row.FindControl("hdfSaleOrderHdrPK") as HiddenField;
                        HiddenField hdfSaleOrderDtlPK = e.Row.FindControl("hdfSaleOrderDtlPK") as HiddenField;
                        HiddenField hdfIGPLCode = e.Row.FindControl("hdfIGPLCode") as HiddenField;
                        HiddenField hdfBrandCode = e.Row.FindControl("hdfBrandCode") as HiddenField;
                        HiddenField hdfUOM = e.Row.FindControl("hdfUOM") as HiddenField;
                        HiddenField hdfOldDespNow = e.Row.FindControl("hdfOldDespNow") as HiddenField;
                        HiddenField hdfIsPackingMaterial = e.Row.FindControl("hdfIsPackingMaterial") as HiddenField;

                        HiddenField hdfSaleUonConvRate = e.Row.FindControl("hdfSalesUonConvRate") as HiddenField;
                        HiddenField hdfOldDespNowSale = e.Row.FindControl("hdfOldDespNowSales") as HiddenField;
                        HiddenField hdfUOMSales = e.Row.FindControl("hdfUOMSales") as HiddenField;
                        Label lblUOMSales = e.Row.FindControl("lblUOMSales") as Label;
                        TextBox txtLotNo = e.Row.FindControl("txtLotNo") as TextBox;
                        HiddenField hdfDpdPk = e.Row.FindControl("hdfDpdPk") as HiddenField;

                        TextBox txtReleasedQty = e.Row.FindControl("txtReleasedQty") as TextBox;

                        Label lblPlantCode = e.Row.FindControl("lblPlantCode") as Label;
                        lblPlantCode.Visible = GetConfigData().IsMultiplePlant;

                        //if (SalOrderDtlList != null && SalOrderDtlList.Count > 0)
                        //{
                        if (salShippingPlanDtlList != null && salShippingPlanDtlList.Count > 0
                            && salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL != null
                            && salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR != null)
                        {
                            hdfSaleOrderHdrPK.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfSaleOrderDtlPK.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_PK.ToString();

                            lnkSoNo.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_NO;
                            lnkSoNo.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_NO;
                            lnkSoNo.CommandArgument = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_PK.ToString();
                            //lblSONo.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_NO;
                            //lblSONo.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_NO;

                            lblSODate.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //lblIGPLCode.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_CODE;
                            lblIGPLCode.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_CODE), 16);
                            lblIGPLCode.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_CODE;
                            hdfIGPLCode.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_PK.ToString();

                            // txtReleasedQty.Text = String.Format("{0:N}", salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL..ToString());

                            try
                            {
                                if (salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP != null)
                                {
                                    lblBrandCode.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    lblBrandCode.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    hdfBrandCode.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_PK.ToString();
                                }
                                if (salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_IS_PACK_MAT == 1)
                                {
                                    lblBrandCode.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_NAME;
                                    lblBrandCode.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_NAME;
                                    hdfBrandCode.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_PK.ToString();
                                }
                            }
                            catch
                            {
                            }
                            lblUOM.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_UOM_MST.UOM_CODE;
                            lblUOM.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_UOM_MST.UOM_CODE;
                            hdfUOM.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_UOM_MST.UOM_PK.ToString();

                            if (salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_UOM.HasValue)
                            {
                                lblUOMSales.Text = salShippingPlanDtlList[e.Row.RowIndex].ADM_CONFIG_MST.CFG_DATA;
                                lblUOMSales.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].ADM_CONFIG_MST.CFG_DATA;
                                hdfUOMSales.Value = salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_UOM.HasValue ? Convert.ToString(salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_UOM) : string.Empty;
                            }
                            //lblOrderQty.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY.ToString();
                            txtDespNowSales.Text = String.Format("{0:N}", salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_QTY.ToString());
                            hdfSaleUonConvRate.Value = salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_UOM_CONV.HasValue ? Convert.ToString(salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_UOM_CONV.Value) : "1";
                            hdfOldDespNowSale.Value = salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_QTY.ToString();
                            hdfIsPackingMaterial.Value = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_IS_PACK_MAT.ToString();
                            lblOrderQty.Text = (salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY / Convert.ToDouble(hdfSaleUonConvRate.Value)).ToString();
                            lblOrderQty.Text = String.Format("{0:N}", decimal.Parse(lblOrderQty.Text));
                            lblOrderQty.ToolTip = lblOrderQty.Text;
                            //lblDespatchedQty.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED.ToString();
                            lblDespatchedQty.Text = (salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED / Convert.ToDouble(hdfSaleUonConvRate.Value)).ToString();
                            if (Convert.ToDouble(lblDespatchedQty.Text) < 0)
                            {
                                lblDespatchedQty.Text = "0";
                            }
                            if (txtReleasedQty.Text == null || txtReleasedQty.Text == "" || txtReleasedQty.Text == string.Empty)
                            {
                                txtReleasedQty.Text = "0";
                            }
                            lblDespatchedQty.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text;
                            //txtDespNow.Text = Math.Round(decimal.Parse(salShippingPlanDtlList[e.Row.RowIndex].SND_PLAN_QTY.ToString())
                            //    , Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();

                            txtDespNow.Text = String.Format("{0:N}", salShippingPlanDtlList[e.Row.RowIndex].SND_PLAN_QTY.ToString());

                            hdfOldDespNow.Value = salShippingPlanDtlList[e.Row.RowIndex].SND_PLAN_QTY.ToString();
                            //txtDespNow.Text = (decimal.Parse(lblOrderQty.Text) - decimal.Parse(lblDespatchedQty.Text)).ToString();
                            //txtDespNow.Text = (decimal.Parse(salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY.ToString()) -
                            //    decimal.Parse(salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED.ToString())).ToString();
                            //txtDespNow.Text = Math.Round(decimal.Parse(txtDespNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            if (GetGlobalResourceObject("ConfigurationsRes", "DOLotNoVisible").ToString() == "1")
                            {
                                e.Row.Cells[13].Visible = true;
                                txtLotNo.Visible = true;
                                txtLotNo.Text = txtLotNo.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_LOT_NO == null ? string.Empty : salShippingPlanDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_LOT_NO.ToString();
                            }
                            else
                            {
                                txtLotNo.Visible = false;
                                e.Row.Cells[13].Visible = false;
                            }

                            if (GetGlobalResourceObject("ConfigurationsRes", "DisableDespatchQty").ToString() == "1")
                            {
                                txtDespNowSales.Enabled = false;
                            }

                            #region Plant DisplayCode Setting
                            lblPlantCode.Text = salShippingPlanDtlList[e.Row.RowIndex].SAL_SHIPPING_PLAN_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblPlantCode.ToolTip = salShippingPlanDtlList[e.Row.RowIndex].SAL_SHIPPING_PLAN_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblPlantCode.CssClass = salShippingPlanDtlList[e.Row.RowIndex].SAL_SHIPPING_PLAN_HDR.ADM_COMPANY_MST.CMP_LINE_COLOUR;
                            #endregion

                            #region Net Weight/Gross Weight
                            if (IsShowNetGrsWeight)
                            {
                                TextBox txtNetW = e.Row.FindControl("txtNetWeight") as TextBox;
                                TextBox txtGrossW = e.Row.FindControl("txtGrossWeight") as TextBox;
                                DataTable dtResult = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanWeightDetails(Convert.ToInt32(hdfSaleOrderHdrPK.Value), string.Empty, Convert.ToDouble(txtDespNow.Text), Convert.ToInt32(hdfSaleOrderDtlPK.Value));
                                if (dtResult != null & dtResult.Rows.Count > 0)
                                {
                                    txtNetW.Text = String.Format("{0:N}", decimal.Parse(dtResult.Rows[0]["SND_QTY_NET_WT"].ToString()));
                                    txtGrossW.Text = String.Format("{0:N}", decimal.Parse(dtResult.Rows[0]["SND_QTY_GROSS_WT"].ToString()));

                                    //TotalNetWeight = TotalNetWeight + Convert.ToDouble(dtResult.Rows[0]["SND_QTY_NET_WT"].ToString());
                                    //TotalGrossWeight = TotalGrossWeight + Convert.ToDouble(dtResult.Rows[0]["SND_QTY_GROSS_WT"].ToString());
                                }
                            }
                            #endregion

                            #region Footer Total
                            TotalDespNowPcs = TotalDespNowPcs + Convert.ToDouble(salShippingPlanDtlList[e.Row.RowIndex].SND_PLAN_QTY.ToString());
                            TotalDespNow = TotalDespNow + Convert.ToDouble(salShippingPlanDtlList[e.Row.RowIndex].SND_SALE_QTY.ToString());
                            TotalOrdrQty = TotalOrdrQty + Convert.ToDouble(lblOrderQty.Text);
                            TotalDespatcQty = TotalDespatcQty + Convert.ToDouble(lblDespatchedQty.Text);

                            #endregion


                        }

                        if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        {
                            hdfSaleOrderHdrPK.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfSaleOrderDtlPK.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_PK.ToString();

                            lnkSoNo.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lnkSoNo.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lnkSoNo.CommandArgument = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            //lblSONo.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            //lblSONo.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;

                            txtReleasedQty.Text = salDespatchDtlList[e.Row.RowIndex].DPD_RELEASE_QTY == null || salDespatchDtlList[e.Row.RowIndex].DPD_RELEASE_QTY.ToString() == "" ? "0" : String.Format("{0:N}", salDespatchDtlList[e.Row.RowIndex].DPD_RELEASE_QTY);

                            lblSODate.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            //lblIGPLCode.Text = salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE;
                            lblIGPLCode.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE), 16);
                            lblIGPLCode.ToolTip = salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_CODE;
                            hdfIGPLCode.Value = salDespatchDtlList[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();
                            if (IsShowNetGrsWeight)
                            {
                                TextBox txtNetW = e.Row.FindControl("txtNetWeight") as TextBox;
                                TextBox txtGrossW = e.Row.FindControl("txtGrossWeight") as TextBox;
                                txtNetW.Text = String.Format("{0:N}", salDespatchDtlList[e.Row.RowIndex].DPD_NET_WT);
                                txtGrossW.Text = String.Format("{0:N}", salDespatchDtlList[e.Row.RowIndex].DPD_GROSS_WT);
                            }
                            try
                            {
                                if (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP != null)
                                {
                                    lblBrandCode.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    lblBrandCode.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_BRAND_NAME;
                                    hdfBrandCode.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_PK.ToString();
                                }
                                if (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_IS_PACK_MAT == 1 || salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_IS_PACK_MAT == 2)
                                {
                                    lblBrandCode.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_NAME;
                                    lblBrandCode.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_NAME;
                                    hdfBrandCode.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.INV_ITEM_MST.ITM_PK.ToString();
                                }
                            }
                            catch
                            {
                            }
                            lblUOM.Text = salDespatchDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_CODE;
                            lblUOM.ToolTip = salDespatchDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_CODE;
                            hdfUOM.Value = salDespatchDtlList[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();

                            if (salDespatchDtlList[e.Row.RowIndex].DPD_SALE_UOM.HasValue)
                            {
                                lblUOMSales.Text = salDespatchDtlList[e.Row.RowIndex].ADM_CONFIG_MST.CFG_DATA;
                                lblUOMSales.ToolTip = salDespatchDtlList[e.Row.RowIndex].ADM_CONFIG_MST.CFG_DATA;
                                hdfUOMSales.Value = salDespatchDtlList[e.Row.RowIndex].DPD_SALE_UOM.HasValue ? Convert.ToString(salDespatchDtlList[e.Row.RowIndex].DPD_SALE_UOM) : string.Empty;
                            }
                            //lblOrderQty.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY.ToString();
                            //txtDespNowSales.Text = salDespatchDtlList[e.Row.RowIndex].DPD_SALE_QTY.ToString();


                            hdfSaleUonConvRate.Value = salDespatchDtlList[e.Row.RowIndex].DPD_SALE_UOM_CONV.HasValue ? Convert.ToString(salDespatchDtlList[e.Row.RowIndex].DPD_SALE_UOM_CONV.Value) : "1";
                            hdfOldDespNowSale.Value = salDespatchDtlList[e.Row.RowIndex].DPD_SALE_QTY.ToString();
                            hdfIsPackingMaterial.Value = salDespatchDtlList[e.Row.RowIndex].DPD_IS_PACK_MAT.ToString();
                            txtDespNowSales.Text = (salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED / Convert.ToDouble(hdfSaleUonConvRate.Value)).ToString();
                            txtDespNowSales.Text = String.Format("{0:N}", txtDespNowSales.Text);

                            lblOrderQty.Text = (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY / Convert.ToDouble(hdfSaleUonConvRate.Value)).ToString();
                            lblOrderQty.Text = String.Format("{0:N}", decimal.Parse(lblOrderQty.Text));
                            lblOrderQty.ToolTip = lblOrderQty.Text;
                            int WKFStatus = salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.SAL_SHIPPING_PLAN_HDR.SNH_STATUS;
                            hdfDelstatus.Value = salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.SAL_SHIPPING_PLAN_HDR.SNH_DEL_STATUS.ToString();
                            //hdfInvoiced.Value = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED - (WKFStatus !=92 && WKFStatus!=99 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED)
                            //lblDespatchedQty.Text = (Convert.ToInt32((salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED)) - (WKFStatus != 92 && WKFStatus < 99 && WKFStatus != 2 ? 0 : Convert.ToInt32(salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED))).ToString();// - (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();
                            lblDespatchedQty.Text = (Convert.ToDouble((salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED / Convert.ToDouble(hdfSaleUonConvRate.Value))) - (((WKFStatus != 92 && WKFStatus < 99 && WKFStatus != 2) || Convert.ToDouble(txtDespNowSales.Text) <= 0) ? 0 : Convert.ToDouble(salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED / Convert.ToDouble(hdfSaleUonConvRate.Value)))).ToString();// - (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();
                            if (Convert.ToDouble(lblDespatchedQty.Text) < 0)
                            {
                                lblDespatchedQty.Text = "0";
                            }
                            lblDespatchedQty.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            txtDespNow.Text = salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED.ToString();
                            txtDespNow.Text = String.Format("{0:N}", txtDespNow.Text);
                            //txtDespNow.Text = Math.Round(decimal.Parse(txtDespNow.Text),
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            hdfOldDespNow.Value = salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED.ToString();
                            if (GetGlobalResourceObject("ConfigurationsRes", "DOLotNoVisible").ToString() == "1")
                            {
                                e.Row.Cells[13].Visible = true;
                                txtLotNo.Visible = true;
                                txtLotNo.Text = txtLotNo.ToolTip = salDespatchDtlList[e.Row.RowIndex].DPD_LOT_NO == null ? string.Empty : salDespatchDtlList[e.Row.RowIndex].DPD_LOT_NO.ToString();
                            }
                            else
                            {
                                e.Row.Cells[13].Visible = false;
                                txtLotNo.Visible = false;
                            }
                            hdfDpdPk.Value = salDespatchDtlList[e.Row.RowIndex].DPD_PK.ToString();
                            //txtDespNow.Text = salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED.ToString();
                            //txtDespNow.Text = Math.Round(decimal.Parse(txtDespNow.Text),
                            //    Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();

                            //lblDespatchedQty.Text = (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED - salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED).ToString();
                            //lblDespatchedQty.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            //lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            //lblDespatchedQty.Text = (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY-(salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();
                            //lblDespatchedQty.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            //lblDespatchedQty.ToolTip = lblDespatchedQty.Text;



                            //txtDespNow.Enabled = true;
                            //try
                            //{
                            //    if (salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR != null)
                            //    {
                            //        if (salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR.Where(inv => inv.ICH_DEL_STATUS == 0 && inv.ICH_HAS_JRNL_ENTRY).Count() > 0)
                            //        {
                            //            txtDespNow.Enabled = false;
                            //        }
                            //        //double invoicedDoQty = 0;
                            //        //List<FIN_INVOICE_CUS_HDR> invHeaderLst = salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR.Where(invhrd => invhrd.ICH_DEL_STATUS == 0).ToList();
                            //        //foreach (FIN_INVOICE_CUS_HDR invHeader in invHeaderLst)
                            //        //{
                            //        //    invoicedDoQty += invHeader.FIN_INVOICE_CUS_DTL.Where(invdtl => invdtl.CID_ITEM == Convert.ToInt32(hdfIGPLCode.Value)).Sum(inv => inv.CID_QTY_INVOICED);
                            //        //}
                            //        //if (salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED <= invoicedDoQty && salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED > 0)
                            //        //{
                            //        //    txtDespNow.Enabled = false;
                            //        //}
                            //    }
                            //}
                            //catch
                            //{
                            //    txtDespNow.Enabled = true;
                            //}

                            txtDespNowSales.Enabled = true;
                            try
                            {
                                if (salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR != null)
                                {
                                    if (salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR.Where(inv => inv.ICH_DEL_STATUS == 0 && inv.ICH_HAS_JRNL_ENTRY).Count() > 0)
                                    {
                                        txtDespNowSales.Enabled = false;
                                    }
                                    //double invoicedDoQty = 0;
                                    //List<FIN_INVOICE_CUS_HDR> invHeaderLst = salDespatchDtlList[e.Row.RowIndex].SAL_DESPATCH_HDR.FIN_INVOICE_CUS_HDR.Where(invhrd => invhrd.ICH_DEL_STATUS == 0).ToList();
                                    //foreach (FIN_INVOICE_CUS_HDR invHeader in invHeaderLst)
                                    //{
                                    //    invoicedDoQty += invHeader.FIN_INVOICE_CUS_DTL.Where(invdtl => invdtl.CID_ITEM == Convert.ToInt32(hdfIGPLCode.Value)).Sum(inv => inv.CID_QTY_INVOICED);
                                    //}
                                    //if (salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED <= invoicedDoQty && salDespatchDtlList[e.Row.RowIndex].DPD_QTY_DESPATCHED > 0)
                                    //{
                                    //    txtDespNow.Enabled = false;
                                    //}
                                }
                            }
                            catch
                            {
                                txtDespNowSales.Enabled = true;
                            }

                            #region Plant DisplayCode Setting
                            lblPlantCode.Text = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblPlantCode.ToolTip = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE;
                            lblPlantCode.CssClass = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_COMPANY_MST.CMP_LINE_COLOUR;
                            #endregion

                        }



                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        if (GetGlobalResourceObject("ConfigurationsRes", "DOLotNoVisible").ToString() == "1")
                        {
                            e.Row.Cells[13].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[13].Visible = false;
                        }

                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        //if (salShippingPlanDtlList != null && salShippingPlanDtlList.Count > 0)
                        //{
                        //    e.Row.Cells[9].Text = GetLocalResourceObject("DespNow").ToString() + " (" + salShippingPlanDtlList[0].SAL_ORDER_DTL.INV_UOM_MST.UOM_CODE + ")";
                        //}
                        //else if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        //{
                        //    e.Row.Cells[9].Text = GetLocalResourceObject("DespNow").ToString() + " (" + salDespatchDtlList[0].INV_UOM_MST.UOM_CODE + ")";
                        //}

                        //if (salShippingPlanDtlList != null && salShippingPlanDtlList.Count > 0)
                        //{
                        if (GetGlobalResourceObject("ConfigurationsRes", "DOLotNoVisible").ToString() == "1")
                        {
                            grdDeliveryList.Width = 1650;
                            e.Row.Cells[13].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[13].Visible = false;
                        }

                        //}
                        //else if (salDespatchDtlList != null && salDespatchDtlList.Count > 0)
                        //{
                        //    if (GetGlobalResourceObject("ConfigurationsRes", "DOLotNoVisible").ToString() == "1")
                        //    {
                        //        e.Row.Cells[11].Visible = false;
                        //    }
                        //    else
                        //    {
                        //        e.Row.Cells[11].Visible = false;
                        //    }
                        //}
                    }
                }





                else if (((GridView)sender).ID == "grdDeliveryOrderList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        //Image imgPosted = e.Row.FindControl("imgPosted") as Image;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        //HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        switch (appstatus)
                        {
                            case (short)WkfStatusEnum.APPROVED:
                                imgApproved.CssClass = GetLocalResourceObject("mark").ToString();
                                imgApproved.ToolTip = Resources.Captions.Approved;
                                break;
                            case (short)WkfStatusEnum.DRAFTED:
                                imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                                imgApproved.ToolTip = Resources.Captions.Drafted;
                                break;
                            case (short)WkfStatusEnum.NEW:
                                imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                                imgApproved.ToolTip = Resources.Captions.Drafted;
                                break;
                        }

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
            uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkShippingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerEval.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerInspection.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadQADocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadExportDocs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkLoadingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadPhotographs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerRelease.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkBillofLoading.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkPrintShippingDocs.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.lnkShippingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkContainerEval.Load += new EventHandler(btnAction_Load);
            this.lnkContainerInspection.Load += new EventHandler(btnAction_Load);
            this.lnkUploadQADocs.Load += new EventHandler(btnAction_Load);
            this.lnkUploadExportDocs.Load += new EventHandler(btnAction_Load);
            this.lnkLoadingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkUploadPhotographs.Load += new EventHandler(btnAction_Load);
            this.lnkDeliveryOrder.Load += new EventHandler(btnAction_Load);
            this.lnkContainerRelease.Load += new EventHandler(btnAction_Load);
            this.lnkBillofLoading.Load += new EventHandler(btnAction_Load);
            this.lnkPrintShippingDocs.Load += new EventHandler(btnAction_Load);
        }
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowRelatedDetails", "$(document).ready(function(){ShowHideRelatedDetails(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowAdditionalDetails", "$(document).ready(function(){ShowHideAdditionalDetails(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotalFooter();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalFoot", "$(document).ready(function(){CalculateTotFooter();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalFoot_", "$(document).ready(function(){Test();});", true);
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


            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                }
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
            SHIPPINGPLANDTLLIST,
            SHIPPINGPLANLEVEL,
            CONTAINERTYPE,
            CONTAINEREVALUATION,
            CONTAINERINSPECTION,
            DEFAULT,
            HISCODE,
            FROMPORT,
            SHIPPINGHEADER,
            SHIPBY,
            PORTDETAILS,
            SHIPMENTTERMS
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 1,
            APPROVED = 2,
            NEW = 0
        }
        #endregion
    }
}

