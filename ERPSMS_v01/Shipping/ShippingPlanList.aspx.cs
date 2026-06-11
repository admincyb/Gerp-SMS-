using System;
using System.Collections.Generic;
using System.Linq;
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
using BusinessObject.Shipping;
using CustomControls;
using BusinessObject;

namespace ERPSMS_v01.Shipping
{
    public partial class ShippingPlanList : ERP.Store.UI.WorkFlowBasePage
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
        /// <summary>
        /// Shipping Plan PK
        /// </summary>
        private int ShippingPlanID
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShippingPlanPK] != null ? (int)this.ViewState[ViewstateStrings.ShippingPlanPK] : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.ShippingPlanPK] = value;
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
        /// To maintain keep SC Details List
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
        /// Posted
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
        /// To identify whether the logined user is customer or not
        /// </summary>
        private bool IsCustomer
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomer] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomer]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomer] = value;
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
        #endregion

        #region Variables
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        //Object for service utility
        private ServiceUtility serviceUtilityObj;
        //Object for GON Header
        private SAL_DESPATCH_HDR salDespatchHdrObj;
        //Object for GON Details
        private SAL_DESPATCH_DTL salDespatchDtlObj;
        //Object for Sale Contracts Details
        private SAL_ORDER_DTL SalOrderDtlObj;

        //List For SaleContacts Details
        private List<SAL_ORDER_DTL> SalOrderDtlList;

        private ADM_CONST_MST admConstMstObj;
        //List For Shipping Plan Container types
        private List<ADM_CONST_MST> admConstMstList;

        //List for binding details to controls  
        //List For GON Header
        private List<SAL_DESPATCH_HDR> salDespatchHdrList;
        //List For GON Details
        private List<SAL_DESPATCH_DTL> salDespatchDtlList;
        //List For Selected SC's for shipping
        private List<long> SelectedSOListForSP;

        private ADM_CONFIG_MST admConfigMstObj;
        //List for workflow status
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;

        private string despatchNo;
        private bool updateDespatch;
        //For Shipping Plan Saving or not
        private bool isSave;

        //Work flow variables
        private string refID;
        private string inboxFlag;

        //dataset for binding details to controls  
        private DataSet dsPageData;
        private DataTable dtPageData;
        private DataSet dsShippingDetails;
        private DataSet dsSPDetails;
        private DataSet dsShippingList;
        private DataSet dsShippingPlanHDR;

        private int listingRefID;
        private string dOPK = string.Empty;
        private string dONo = string.Empty;
        //List for Shipping Plan Number
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        ShippingPlanOrder ShippingPlanOrderObj;
        ShippingPlanBO ShippingPlanObj;

        //For Shipping Plan number
        private string ShippingPlanNo;

        private int SPID;
        //For calculating Total Plan Qty
        double TotalPlandQty = 0.0;

        private DataTable dtCartonAllocationStatus;

        #endregion

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
            int cusPK;
            try
            {
                //workflow integration purpose
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    //get and set the carton allocation Status
                    GetFieldValues(ControlsEnum.CARTONALLOCATIONSTATUS);
                    SetFieldValues(ControlsEnum.CARTONALLOCATIONSTATUS);

                    //get and set the workflow Status
                    GetFieldValues(ControlsEnum.STATUS);
                    SetFieldValues(ControlsEnum.STATUS);

                    //get and set the Container Types
                    GetFieldValues(ControlsEnum.SPCONTAINERTYPE);
                    SetFieldValues(ControlsEnum.SPCONTAINERTYPE);

                    //for shipping plan no
                    AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    SelectedInvoicesCrDr = null;

                    DateTime PrevMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
                    //txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    //hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtFromDate.Text = PrevMonth.ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = PrevMonth.ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    txtGeneratedOn.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtETD.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    //workflow integration purpose
                    FillProcessID();

                    //if the user login as customer set the customer details
                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        if (int.TryParse(dtPageData.Rows[0]["CUS_PK"].ToString(), out cusPK) && cusPK > 0)
                        {
                            txtCustomer.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CUS_NAME"].ToString());
                            hdfCustomerID.Value = cusPK.ToString();
                            txtCustomer.Enabled = false;
                            IsCustomer = true;
                        }
                    }

                    //For Listing
                    SelectedSOListForSP = new List<long>();
                    GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                    SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;

                    hdfType.Value = ApplicationType.SPLN;
                    hdfDOType.Value = ApplicationType.DO;
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
            //For Common Service
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            //get the userdeatils
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            SaleOrderService salesOrderServiceClient;
            salesOrderServiceClient = null;

            string getxml;

            try
            {
                //initialize the common service
                CommonServiceClient = new CommonService();
                switch (type)
                {
                    case ControlsEnum.SHIPPINGPLANHDR:
                        //For Lising
                        int cusID = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        string PlanNo = string.IsNullOrEmpty(txtPlanNo.Text.Trim()) ? string.Empty : txtPlanNo.Text.Trim() + "%";
                        string ScNo = string.IsNullOrEmpty(txtSCno.Text.Trim()) ? string.Empty : txtSCno.Text.Trim() + "%";
                        string DONo = string.IsNullOrEmpty(txtDespatchNumber.Text.Trim()) ? string.Empty : txtDespatchNumber.Text.Trim() + "%";
                        int cartnAllocStatus = string.IsNullOrEmpty(ddlCartnAllocStatus.SelectedValue)?-1: Convert.ToInt32(ddlCartnAllocStatus.SelectedValue);
                        //initialize the Service Utility for paging ,sorting ,fillter etc
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdShippingPlanList.PageSize;
                        serviceUtilityObj.TotalRecords = 0;
                        //get the shipping plan list
                        dsPageData = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SPPk : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = Resources.DataFieldRes.SPStatus,
                                SearchValue = Status.ToString(),
                            }, currentUser, 0, Convert.ToInt32(CommonConstants.ACTIVE), Status, cusID, 0, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize, PlanNo, ScNo, string.Empty, DONo,0,cartnAllocStatus);
                        if (dsPageData != null)
                        {
                            //set Total Page Count
                            serviceUtilityObj.TotalRecords = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;
                            TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                        (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                        (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        }
                        break;
                    case ControlsEnum.SHIPPINGPLANLIST:
                        //For SC Details
                        ShippingPlanOrderObj = (ShippingPlanOrder)SetUIValuesToObject(type);
                        getxml = CommonFunctions.XmlSerialize<ShippingPlanOrder>(ShippingPlanOrderObj);
                        dsShippingList = BusinessLogic.Shipping.ShippingPlanBL.GetShippingOrderList(getxml);
                        break;
                    case ControlsEnum.SHIPPINGPLANDETAILS:
                        //For Shipping Header and Details
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanID, Convert.ToInt32(CommonConstants.ACTIVE));
                        dsShippingDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanDetails(0, ShippingPlanID, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.PLANNO:
                        //Generate PLANNO No
                        ShippingPlanNo = CommonServiceClient.GetTrxDocNo(ApplicationType.SPLN, 0, 1,
                           DateTime.Now, currentUser.PKUser, true, 0);
                        break;
                    case ControlsEnum.STATUS:
                        //get the work flow Status
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.AppStatusName;
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.SPLN, null, Convert.ToByte(CommonConstants.ACTIVE), serviceUtilityObj);

                        break;
                    case ControlsEnum.SPDEATILS:
                        //get the shipping plan details
                        dsSPDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanDetails(0, SPID, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.SPCONTAINERTYPE:
                        //get the container types
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToInt16(CommonConstants.ACTIVE), (int)ConstGroupType.ContainerType, null, null, currentUser.SBUID);
                        break;
                    case ControlsEnum.USERCUSTOMER:
                        //get the customer details
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.GRIDSHOWMY:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        dOPK = salesOrderServiceClient.GetPkFromDONo(dONo);
                        break;
                    #region CARTONALLOCATIONSTATUS
                    case ControlsEnum.CARTONALLOCATIONSTATUS:
                        dtCartonAllocationStatus = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "CARTON ALLOCATION TYPE");
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
                CommonServiceClient = null;
                admConfigMstObj = null;
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
                    case ControlsEnum.SHIPPINGPLANHDR:
                        BindGrid(ControlsEnum.SHIPPINGPLANHDR);
                        break;
                    case ControlsEnum.SHIPPINGPLANLIST:
                        BindGrid(ControlsEnum.SHIPPINGPLANLIST);
                        break;
                    case ControlsEnum.STATUS:
                        BindDropDown(ControlsEnum.STATUS);
                        break;
                    case ControlsEnum.SPCONTAINERTYPE:
                        BindDropDown(ControlsEnum.SPCONTAINERTYPE);
                        break;
                    #region CARTONALLOCATIONSTATUS
                    case ControlsEnum.CARTONALLOCATIONSTATUS:
                        BindDropDown(controlType);
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

        #region Helper Methods

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.STATUS:
                    ddlStatus.Items.Clear();
                    if (workflowStatusList != null && workflowStatusList.Count > 0)
                    {
                        ddlStatus.DataSource = workflowStatusList;
                        ddlStatus.DataTextField = "ASC_NAME";
                        ddlStatus.DataValueField = "ASC_VALUE";
                        ddlStatus.DataBind();
                    }
                    ddlStatus.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.SPCONTAINERTYPE:
                    ddlContainerType.Items.Clear();
                    if (admConstMstList != null && admConstMstList.Count > 0)
                    {
                        ddlContainerType.DataSource = admConstMstList;
                        ddlContainerType.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlContainerType.DataValueField = Resources.DataFieldRes.ConstPK;
                        ddlContainerType.DataBind();
                    }
                    ddlContainerType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #region CARTONALLOCATIONSTATUS
                case ControlsEnum.CARTONALLOCATIONSTATUS:
                    ddlCartnAllocStatus.Items.Clear();
                    if (dtCartonAllocationStatus != null && dtCartonAllocationStatus.Rows.Count > 0)
                    {
                        ddlCartnAllocStatus.DataSource = dtCartonAllocationStatus;
                        ddlCartnAllocStatus.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlCartnAllocStatus.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlCartnAllocStatus.DataBind();
                    }
                    if (ddlCartnAllocStatus.Items.Count > 1)
                        ddlCartnAllocStatus.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
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
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                List<SaleOrderPK> saleOrderPks;
                List<ShippingDetails> ShippingDetails;
                int rowID = 0;
                HiddenField hdfSaleOrderDtlPK;
                HiddenField hdfSaleOrderHdrPK;
                TextBox txtplayNow;
                HiddenField hdfSONumber;
                HiddenField hdfTotlPcs;
                switch (controlType)
                {
                    #region Shipping Plan
                    case ControlsEnum.SHIPPINGPLANHDR:
                        ShippingPlanObj.SNH_PK = ShippingPlanID;
                        ShippingPlanObj.SNH_NO = ShippingPlanNo;
                        ShippingPlanObj.SNH_DATE = Convert.ToDateTime(txtGeneratedOn.Text.Trim());
                        ShippingPlanObj.SNH_CUSTOMER = Convert.ToInt32(hdnCustomerID.Value);
                        ShippingPlanObj.SNH_SHIP_TO_PORT = txtShipPort.Text.Trim();
                        if (ddlContainerType.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            ShippingPlanObj.SNH_CONTAINER_TYPE = ddlContainerType.SelectedValue;
                        }
                        if (txtETD.Text.Trim() != string.Empty)
                        {
                            ShippingPlanObj.SNH_ETD = Convert.ToDateTime(txtETD.Text.Trim());
                        }
                        ShippingPlanObj.SNH_DESC = string.Empty;
                        //ShippingPlanObj.SNH_PLAN_QTY =
                        Label lblTotalCTN = ((Label)grdShippingList.FooterRow.FindControl("lblTotalCTN"));
                        if (lblTotalCTN != null)
                        {
                            ShippingPlanObj.SNH_CTN_QTY = lblTotalCTN.Text.Trim() != "" ? Convert.ToDouble(lblTotalCTN.Text.Trim()) : 0.0;
                        }
                        GetTotalPlandQty();
                        ShippingPlanObj.SNH_PLAN_QTY = TotalPlandQty;
                        ShippingPlanObj.SNH_TRX_STATUS = Convert.ToInt32(CommonConstants.ACTIVE);
                        ShippingPlanObj.SNH_DEPT = currentUser.CurrentDeptPK;
                        ShippingPlanObj.BIZUNIT_PK = currentUser.SBUID;
                        ShippingPlanObj.ACTIVE = Convert.ToInt32(CommonConstants.ACTIVE);
                        ShippingPlanObj.USER_PK = currentUser.PKUser;
                        ShippingPlanObj.LAST_MOD_DT = DateTime.Now;

                        ShippingDetails = new List<ShippingDetails>();

                        foreach (GridViewRow grdrow in grdShippingList.Rows)
                        {
                            txtplayNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtPlanNow");
                            if (txtplayNow != null && !string.IsNullOrEmpty(txtplayNow.Text.Trim())
                                && Convert.ToDouble(txtplayNow.Text) > 0)
                            {
                                ShippingDetails sd = new ShippingDetails();
                                hdfSaleOrderDtlPK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSaleOrderDtlPK");
                                sd.SND_PK = Convert.ToInt32(hdfSaleOrderDtlPK.Value);
                                hdfSaleOrderHdrPK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSaleOrderHdrPK");
                                //sd.SND_PLAN_HDR = Convert.ToInt32(hdfSaleOrderHdrPK.Value);
                                sd.SND_PLAN_QTY = Convert.ToDouble(txtplayNow.Text);
                                hdfTotlPcs = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdnTotalPcs");
                                hdfTotlPcs.Value = hdfTotlPcs.Value == string.Empty ? "0.0" : hdfTotlPcs.Value;
                                sd.SND_CTN_QTY = Math.Ceiling(Convert.ToDouble(txtplayNow.Text) / Convert.ToDouble(hdfTotlPcs.Value));
                                hdfSONumber = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSONumber");
                                sd.SND_SOD = Convert.ToInt32(hdfSONumber.Value);
                                sd.ACTIVE = Convert.ToInt32(CommonConstants.ACTIVE);
                                sd.SND_MOD_BY = currentUser.PKUser;
                                sd.SND_MOD_DT = DateTime.Now;
                                ShippingDetails.Add(sd);
                                isSave = true;
                            }
                            rowID++;
                        }
                        ShippingPlanObj.ShippingDetails = ShippingDetails;
                        retObject = ShippingPlanObj;

                        break;
                    #endregion

                    #region ShippingList
                    case ControlsEnum.SHIPPINGPLANLIST:
                        ShippingPlanOrderObj = new ShippingPlanOrder();
                        saleOrderPks = new List<SaleOrderPK>();
                        foreach (long pk in SelectedSOListForSP)
                        {
                            SaleOrderPK item = new SaleOrderPK();
                            item.SOH_PK = pk;
                            saleOrderPks.Add(item);
                        }
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
        /// SetTotalCTN
        /// </summary>
        /// <returns></returns>
        private void SetTotalCTN()
        {
            int rowID = 0;
            TextBox txtplayNow;
            HiddenField hdfTotlPcs;
            Label lblOrderQty;
            Label lblPackedQty;
            Label lblCTNQty;
            double Plan_Qty;
            double CTN_Qty;
            double TotalCTNQty = 0.0;
            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                txtplayNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtPlanNow");
                if (txtplayNow != null && !string.IsNullOrEmpty(txtplayNow.Text.Trim())
                    && Convert.ToDouble(txtplayNow.Text) > 0)
                {

                    lblOrderQty = (Label)grdShippingList.Rows[rowID].FindControl("lblOrderQty");
                    lblPackedQty = (Label)grdShippingList.Rows[rowID].FindControl("lblPackedQty");
                    Plan_Qty = txtplayNow.Text != string.Empty ? Convert.ToDouble(txtplayNow.Text) : 0;
                    hdfTotlPcs = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdnTotalPcs");
                    hdfTotlPcs.Value = hdfTotlPcs.Value == string.Empty ? "0.0" : hdfTotlPcs.Value;
                    CTN_Qty = Math.Ceiling(Convert.ToDouble(txtplayNow.Text) / Convert.ToDouble(hdfTotlPcs.Value));
                    lblCTNQty = (Label)grdShippingList.Rows[rowID].FindControl("lblCTNQty");
                    lblCTNQty.Text = CTN_Qty.ToString();
                    TotalCTNQty = TotalCTNQty + CTN_Qty;
                    lblOrderQty.Text = lblOrderQty.Text != string.Empty ? lblOrderQty.Text : "0";
                    //lblPackedQty.Text = Math.Ceiling(Convert.ToDouble(lblOrderQty.Text) - Plan_Qty).ToString();
                }
                rowID++;
            }
            Label lblTotalCTN = ((Label)grdShippingList.FooterRow.FindControl("lblTotalCTN"));
            if (lblTotalCTN != null)
            {
                lblTotalCTN.Text = Math.Ceiling(TotalCTNQty).ToString();
            }
        }

        /// <summary>
        /// GetTotalPlandQty
        /// </summary>
        /// <returns></returns>
        private void GetTotalPlandQty()
        {
            int rowID = 0;
            TextBox txtplayNow;
            double Plan_Qty;
            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                txtplayNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtPlanNow");
                if (txtplayNow != null && !string.IsNullOrEmpty(txtplayNow.Text.Trim())
                    && Convert.ToDouble(txtplayNow.Text) > 0)
                {
                    Plan_Qty = txtplayNow.Text != string.Empty ? Convert.ToDouble(txtplayNow.Text) : 0;
                    TotalPlandQty = TotalPlandQty + Plan_Qty;
                }
                rowID++;
            }
        }

        private void ConfigurationSettings()
        {
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
        }

        /// <summary>
        /// SetPlandQty
        /// </summary>
        /// <returns></returns>
        private void SetPlandQty()
        {
            //int rowID = 0;
            //TextBox txtplayNow;
            //Label lblOrderQty;
            //Label lblPackedQty;
            //double Plan_Qty;
            //foreach (GridViewRow grdrow in grdShippingList.Rows)
            //{
            //    txtplayNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtPlanNow");
            //    if (txtplayNow != null && !string.IsNullOrEmpty(txtplayNow.Text.Trim())
            //        && Convert.ToDouble(txtplayNow.Text) > 0)
            //    {
            //        lblOrderQty = (Label)grdShippingList.Rows[rowID].FindControl("lblOrderQty");
            //        lblPackedQty = (Label)grdShippingList.Rows[rowID].FindControl("lblPackedQty");
            //        lblPackedQty.Text = lblPackedQty.Text != string.Empty ? lblPackedQty.Text : "0";
            //        Plan_Qty = txtplayNow.Text != string.Empty ? Convert.ToDouble(txtplayNow.Text) : 0;
            //        lblOrderQty.Text = lblOrderQty.Text != string.Empty ? lblOrderQty.Text : "0";
            //        lblPackedQty.Text = Math.Ceiling(Convert.ToDouble(lblOrderQty.Text) - (Plan_Qty + Convert.ToDouble(lblOrderQty.Text))).ToString();

            //    }
            //    rowID++;
            //}
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
                    case ControlsEnum.SHIPPINGPLANDETAILS:
                        if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
                        {
                            lblShippingPlanNo.Text = ((dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO] == null || dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO].ToString() == "") ? Resources.Messages.DocGenerationNew : dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO].ToString());
                            hdfShippingPlanNo.Value = (dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO] == null ? string.Empty : dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO].ToString());
                            txtGeneratedOn.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            txtETD.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPETD].ToString()).ToString(Resources.Constants.DateFormatShort);
                            txtShipPort.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPShipToPort].ToString();
                            lblDeliveryTo.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPCustomerText].ToString();
                            hdnCustomerID.Value = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPCustomer].ToString();
                            lblSPStatus.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPStatusText].ToString();
                            ddlContainerType.SelectedValue = (dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPContainerType] != null && dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPContainerType].ToString() != string.Empty) ? dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPContainerType].ToString() : CommonConstants.SELECTVAL;
                        }
                        else if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            txtShipPort.Text = dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SohToPort].ToString();
                            lblDeliveryTo.Text = dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SohCustomerName].ToString();
                            hdnCustomerID.Value = dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SaleOrderCustomer].ToString();
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SHIPPINGPLANHDR:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdShippingPlanList.DataSource = dsPageData.Tables[1];
                        grdShippingPlanList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    case ControlsEnum.SHIPPINGPLANLIST:
                        if (dsShippingDetails != null && dsShippingDetails.Tables[0].Rows.Count > 0)
                        {
                            grdShippingList.DataSource = dsShippingDetails.Tables[0].DefaultView;
                            grdShippingList.DataBind();
                            SetTotalCTN();
                        }
                        else if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            grdShippingList.DataSource = dsShippingList.Tables[0].DefaultView;
                            grdShippingList.DataBind();
                            SetTotalCTN();
                            SetPlandQty();
                        }
                        else
                        {
                            grdShippingList.DataSource = null;
                            grdShippingList.DataBind();
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
                ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanID, ucrWrkf.ProcessID);
                ucrWrkf.FillWorkFlowDetails();
                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && (ucrWrkf.RefID == 0 || ucrWrkf.HasPageTaskPermission))
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
                    //pnlPrint.Visible = true;
                }
                ucrWrkf.ViewAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Set values to Session for handling  navigation
        /// </summary>
        /// <param name="mode"></param>
        private void SetTabURL(ActionsEnum mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdShippingPlanList.Rows)
                {
                    RadioButton rbtn;
                    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    if (rbtn.Checked)
                    {
                        int trxStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxStatus")).Value);
                        switch (mode)
                        {
                            case ActionsEnum.DEFAULT:
                                Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                Response.Redirect(Resources.PageURL.SalesOrderListing);
                                break;
                            case ActionsEnum.SHIPPINGPLAN:
                                Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                Response.Redirect(Resources.PageURL.ShippingPlan);
                                break;
                            case ActionsEnum.CONTAINEREVALUATION:
                                if (trxStatus >= (int)ShippingTabsEnum.PaymentCleared)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Response.Redirect(Resources.PageURL.ContainerEvaulation);
                                }
                                else
                                {
                                    //litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("PaymentClear").ToString());
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level1").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.CONTAINERINSPECTION:
                                if (trxStatus >= (int)ShippingTabsEnum.ContainerEvaluated)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Response.Redirect(Resources.PageURL.ContainerInspection);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerEvaluation);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.UPLOADQA:
                                if (trxStatus >= (int)ShippingTabsEnum.ContainerInspected)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.QA;
                                    Response.Redirect(Resources.PageURL.UploadQa);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerInspection);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.UPLOADEXPORT:
                                if (trxStatus >= (int)ShippingTabsEnum.QADocsUploaded)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Export;
                                    Response.Redirect(Resources.PageURL.UploadExport);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.UploadQADocs);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.BL:
                                if (trxStatus >= (int)ShippingTabsEnum.ContainerReleased)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.BillofLoading;
                                    Response.Redirect(Resources.PageURL.BillofLoading);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ContainerRelease);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.LOADINGPLAN:
                                if (trxStatus >= (int)ShippingTabsEnum.ExportDocsUploaded)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Response.Redirect(Resources.PageURL.LoadingPlan);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.UploadExportDocs);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.UPLOADPHOTOGRAPHS:
                                if (trxStatus >= (int)ShippingTabsEnum.LoadingPlanCompleted)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGUPLOADTYPE] = (int)ShippingUploadsEnum.Photographs;
                                    Response.Redirect(Resources.PageURL.UploadPhotographs);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.LoadingPlan);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.GOODOUTWARD:
                                if (trxStatus >= (int)ShippingTabsEnum.PhotographsUploaded)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Response.Redirect(Resources.PageURL.GoodOutward);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.UploadPhotographs);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                            case ActionsEnum.CONTAINERRELEASE:
                                if (trxStatus >= (int)ShippingTabsEnum.DeliveryOrderCompleted)
                                {
                                    Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    Response.Redirect(Resources.PageURL.ContainerRelease);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Level_Check").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DeliveryOrder);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                break;
                        }
                    }
                }
                litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            ShippingPlanID = 0;
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
            if (!IsCustomer)
            {
                txtCustomer.Text = "Select/Type";
                hdfCustomerID.Value = string.Empty;
            }
            SelectedSosForSP = null;
            Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
            ModifiedDatePnl.Visible = false;

            DateTime PrevMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
            //txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            //hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            txtFromDate.Text = PrevMonth.ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = PrevMonth.ToString(Resources.Constants.DateFormatShort);
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();

            lblShippingPlanNo.Text = string.Empty;
            hdfShippingPlanNo.Value = string.Empty;
            txtGeneratedOn.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            txtShipPort.Text = string.Empty;
            lblDeliveryTo.Text = string.Empty;
            hdnCustomerID.Value = "0";
            txtETD.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            lblSPStatus.Text = string.Empty;
            ddlContainerType.SelectedValue = CommonConstants.SELECTVAL;
            base.WkfRefID = 0;
            txtSCno.Text = string.Empty;
            txtPlanNo.Text = string.Empty;
            txtDespatchNumber.Text = string.Empty;
            ddlCartnAllocStatus.SelectedValue = CommonConstants.SELECTVAL;

        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.SPLN, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
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
                GridViewRow gvr;
                GridView grd;
                string arg;

                string saveXml;
                string soPK;
                string doPK;
                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                int result;
                List<object> lstResult = new List<object>();
                string action;
                isSave = false;

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
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {

                    #region extra grid ondemand data population
                    // Do Action for , when click btnOrderDetails button
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
                            if (grdShippingList.Rows.Count > 0)
                            {
                                if (hdfShippingPlanNo.Value == string.Empty && AST_DOC_MODE.Value == ((int)DOCMODE.Draft).ToString())
                                {
                                    GetFieldValues(ControlsEnum.PLANNO);
                                }
                                else
                                {
                                    ShippingPlanNo = hdfShippingPlanNo.Value;
                                }
                                ShippingPlanObj = new ShippingPlanBO();
                                ShippingPlanObj = (ShippingPlanBO)SetUIValuesToObject(ControlsEnum.SHIPPINGPLANHDR);
                                if (isSave)
                                {
                                    if (ShippingPlanObj != null)
                                    {
                                        saveXml = CommonFunctions.XmlSerialize<ShippingPlanBO>(ShippingPlanObj);
                                        lstResult = BusinessLogic.Shipping.ShippingPlanBL.SaveShippingPlan(saveXml);
                                        result = Convert.ToInt32(lstResult[0]);        
                                        if (result >= 0) // Success ! re-initialize the page
                                        {
                                            //Show Save success message and reset Contract Entry
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                            SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);

                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            //        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);


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
                                                litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
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

                    #region New
                    // Do Action for , when click NEW button
                    case ActionsEnum.NEW:
                        //if selected SC's then create Shipping plan otherwise listing
                        if (SelectedSosForSP != null)
                        {
                            SelectedSOListForSP = SelectedSosForSP;
                            ModifiedDatePnl.Visible = false;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateDespatch = false;
                            //GetFieldValues(ControlsEnum.PLANNO);
                            //lblDispInvoiceNo.Text = hdfShippingPlanNo.Value;
                        }
                        else
                        {
                            SelectedSOListForSP = new List<long>();
                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            EntryStatus = EntryStatus.LISTMODE;

                        }
                        break;
                    #endregion

                    #region SEARCH
                    // Do Action for , when click Search button
                    case ActionsEnum.SEARCH:
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        break;
                    #endregion

                    #region Cancel
                    // Do Action for , when click cancel button
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        //this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #endregion

                    #region ShippingPlan Details
                    // Do Action for , when click Details tab
                    case ActionsEnum.SHIPPINGPLANDETAIL:

                        if (SelectedSosForSP != null)
                        {
                            SelectedSOListForSP = SelectedSosForSP;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateDespatch = false;
                        }
                        else
                        {
                            foreach (GridViewRow grdrow in grdShippingPlanList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    ShippingPlanID = CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                    break;
                                }
                            }
                            if (bIsChecked)
                            {
                                SetUIEditView(commonActions);
                                ModifiedDatePnl.Visible = true;
                                GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                                SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);

                                GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);

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
                    // Do Action for , when click Edit button
                    case ActionsEnum.EDIT:

                        foreach (GridViewRow grdrow in grdShippingPlanList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ShippingPlanID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
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
                            GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                        }
                        else
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region View
                    // Do Action for , when click view button
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdShippingPlanList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ShippingPlanID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
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
                    // Do Action for , when click clear button
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        //hdfCustomerID.Value = string.Empty;
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region SP Hdr
                    // Do Action for , when click Radio button in  shipping plan list grid
                    case ActionsEnum.SHOWDETAILS:
                        HiddenField hdfDept;
                        int dept;
                        gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                        ShippingPlanID = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfShippingPlanID")).Value);

                        hdfDept = gvr.FindControl("hdfDept") as HiddenField;// grdShippingPlanList.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        listingRefID = workflowCore.GetRefID(ShippingPlanID, PageProcessID);
                        break;
                    #endregion

                    #region Tab navigation
                    // Do Action for , when click SaleContract tab
                    case ActionsEnum.DEFAULT:
                        //SetTabURL(commonActions);
                        //Response.Redirect(Resources.PageURL.ShippingSoListing);
                        break;
                    // Do Action for , when click Shipping Plan tab
                    case ActionsEnum.SHIPPINGPLAN:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm();
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        //Response.Redirect(Resources.PageURL.ShippingPlanCreate);
                        break;
                    // Do Action for , when click Cont. Eval. tab
                    case ActionsEnum.CONTAINEREVALUATION:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click Cont. Insp. tab
                    case ActionsEnum.CONTAINERINSPECTION:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click QA Docs tab
                    case ActionsEnum.UPLOADQA:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click Exp.Docs tab
                    case ActionsEnum.UPLOADEXPORT:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click Load Plan tab
                    case ActionsEnum.LOADINGPLAN:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click Photos tab
                    case ActionsEnum.UPLOADPHOTOGRAPHS:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click GON tab
                    case ActionsEnum.GOODOUTWARD:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click Cont. Release tab
                    case ActionsEnum.CONTAINERRELEASE:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click B/L tab
                    case ActionsEnum.BL:
                        SetTabURL(commonActions);
                        break;
                    // Do Action for , when click Print Docs tab
                    case ActionsEnum.PRINT:
                        if (ShippingPlanID == 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            PrinterControl1.ShippingPlanID = ShippingPlanID;
                            PrinterControl1.SetCommericalInvoice(ShippingPlanID);
                            Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanID.ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                        }
                        break;
                    #endregion

                    #region Delete
                    //case ActionsEnum.DELETE:
                    //    deliveryOrderServiceClient = new DeliveryOrderService();
                    //    deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                    //    result = deliveryOrderServiceClient.DeleteSalDespatch(ShippingPlanID);
                    //    if (result > 0)
                    //    {
                    //        litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                    //        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.DeliveryOrder);
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    //        EntryStatus = EntryStatus.LISTMODE;
                    //        ResetForm();
                    //        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                    //        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                    //        //btnNew.Focus();
                    //    }
                    //    break;
                    #endregion

                    #region Shipping Plan List
                    // Do Action for , when click List tab
                    case ActionsEnum.SHIPPINGPLANLIST:
                        CurrPK = 0;
                        ShippingPlanID = 0;
                        SelectedSosForSP = null;
                        Session[ERP.Utilities.SessionStrings.SelectedSosForSP] = null;
                        SelectedSOListForSP = new List<long>();
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region dropdownchange
                    // Do Action for , when change Dropdown
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SPLN + "&APPSUBTYPE="+ ddlPrint.SelectedValue), false);
                        break;
                    #endregion

                    #region SUBMIT
                    // Do Action for , when click Submit button
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

                            if (grdShippingList.Rows.Count > 0)
                            {
                                //generate number
                                if (hdfShippingPlanNo.Value == string.Empty)
                                {
                                    GetFieldValues(ControlsEnum.PLANNO);
                                }
                                else
                                {
                                    ShippingPlanNo = hdfShippingPlanNo.Value;
                                }

                                ShippingPlanObj = new ShippingPlanBO();
                                ShippingPlanObj = (ShippingPlanBO)SetUIValuesToObject(ControlsEnum.SHIPPINGPLANHDR);
                                if (isSave)
                                {
                                    if (ShippingPlanObj != null)
                                    {
                                        saveXml = CommonFunctions.XmlSerialize<ShippingPlanBO>(ShippingPlanObj);
                                        lstResult = BusinessLogic.Shipping.ShippingPlanBL.SaveShippingPlan(saveXml);
                                        result = Convert.ToInt32(lstResult[0]);         
                                        if (result >= 0) // Success ! re-initialize the page
                                        {
                                            //Workflow submission
                                            ucrWrkf.ApplicationID = result;
                                            ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                            //Do WorkFlow if WorkFlow has Actions
                                            if (ddlWkfAction.Items.Count > 0)
                                            {
                                                action = ddlWkfAction.SelectedItem.ToString();
                                                result = ucrWrkf.DoWorkFlow();
                                                //Show Save success message and reset Contract Entry
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.Invoicing) + "');", true);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                EntryStatus = EntryStatus.LISTMODE;
                                                ResetForm();
                                                GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                                SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);

                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.ShippingPlan) + "');", true);

                                            }

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
                                                litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = Resources.PageNameRes.RFQ + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQListing) + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
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
                        //    SelectedSosForSP.Remove(DOPk);
                        //    SelectedSOListForSP = SelectedSosForSP;
                        //    //salDespatchDtlList = (List<SAL_DESPATCH_HDR>)SaleDespatchDtlList;
                        //    //salDespatchHdrObj = salDespatchDtlList.SingleOrDefault(so => so.DPD_PK == DOPk);
                        //    //PurOrderHdrList.Remove(PurOrderHdrObj);
                        //    //PoHeaderList = PurOrderHdrList;
                        //    //SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);//  POINVOICELIST);
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

                    #region TOTALCTN
                    // Do Action for , when click setTotal Button
                    case ActionsEnum.SHOW:
                        SetTotalCTN();
                        SetPlandQty();
                        break;
                    #endregion

                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region Show DO Popup
                    case ActionsEnum.GRIDSHOWMY:
                        ConfigurationSettings();
                        dOPK = ((LinkButton)sender).CommandArgument;
                       // GetFieldValues(ControlsEnum.GRIDSHOWMY);
                        if (dOPK != string.Empty)
                        {
                            if (IsExportExcel)
                            {
                                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + dOPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6");
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + dOPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6") + "');", true);
                            }
                        }
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
                GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
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
                if (((GridView)sender).ID == "grdShippingList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblSONo = e.Row.FindControl("lblSONo") as Label;
                        Label lblSODate = e.Row.FindControl("lblSODate") as Label;
                        Label lblIGPLCode = e.Row.FindControl("lblIGPLCode") as Label;
                        Label lblBrandCode = e.Row.FindControl("lblBrandCode") as Label;
                        Label lblUOM = e.Row.FindControl("lblUOM") as Label;
                        Label lblPackedQty = e.Row.FindControl("lblPackedQty") as Label;

                        Label lblOrderQty = e.Row.FindControl("lblOrderQty") as Label;
                        Label lblDespatchedQty = e.Row.FindControl("lblDespatchedQty") as Label;
                        Label lblCTNQty = e.Row.FindControl("lblCTNQty") as Label;
                        TextBox txtPlanNow = e.Row.FindControl("txtPlanNow") as TextBox;
                        HiddenField hdfSaleOrderHdrPK = e.Row.FindControl("hdfSaleOrderHdrPK") as HiddenField;
                        HiddenField hdfSaleOrderDtlPK = e.Row.FindControl("hdfSaleOrderDtlPK") as HiddenField;
                        HiddenField hdfIGPLCode = e.Row.FindControl("hdfIGPLCode") as HiddenField;
                        HiddenField hdfBrandCode = e.Row.FindControl("hdfBrandCode") as HiddenField;
                        HiddenField hdfUOM = e.Row.FindControl("hdfUOM") as HiddenField;
                        HiddenField hdnTotalPcs = e.Row.FindControl("hdnTotalPcs") as HiddenField;
                        HiddenField hdfSONumber = e.Row.FindControl("hdfSONumber") as HiddenField;
                        if (dsShippingDetails != null && dsShippingDetails.Tables[0].Rows.Count > 0)
                        {
                            hdfSaleOrderHdrPK.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPPlanHDR].ToString();
                            hdfSaleOrderDtlPK.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPK].ToString();
                            lblSONo.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SONumber].ToString();
                            lblSONo.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SONumber].ToString();
                            lblSODate.Text = Convert.ToDateTime(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = Convert.ToDateTime(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblIGPLCode.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.ItemCode].ToString(), 13);
                            lblIGPLCode.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.ItemName].ToString();
                            //hdfIGPLCode.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();

                            lblBrandCode.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.BrandName].ToString(), 18);
                            lblBrandCode.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.BrandName].ToString();
                            //hdfBrandCode.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].CRM_CUST_ITEM_MAP.CIM_PK.ToString();

                            lblUOM.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Disp_UomCode].ToString();
                            lblUOM.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Disp_UomCode].ToString();
                            //hdfUOM.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();
                            lblOrderQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.OrderQty].ToString();
                            lblOrderQty.Text = Math.Floor(Convert.ToDecimal(lblOrderQty.Text.Trim())).ToString();
                            lblOrderQty.ToolTip = lblOrderQty.Text;
                            lblDespatchedQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SodQtyDispatched].ToString();
                            lblDespatchedQty.Text = Math.Floor(Convert.ToDecimal(lblDespatchedQty.Text.Trim())).ToString();
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            lblCTNQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.CartonsQty].ToString();
                            lblCTNQty.Text = lblCTNQty.Text == string.Empty ? "0" : lblCTNQty.Text;
                            lblCTNQty.Text = Math.Ceiling(Convert.ToDecimal(lblCTNQty.Text.Trim())).ToString();
                            lblCTNQty.ToolTip = lblCTNQty.Text;

                            hdnTotalPcs.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.TotalPcs].ToString();
                            hdfSONumber.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDeatilsSOD].ToString();


                            txtPlanNow.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.PlanQty].ToString() != string.Empty ? dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.PlanQty].ToString() : "0";
                            txtPlanNow.Text = Math.Round(decimal.Parse(txtPlanNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            txtPlanNow.Text = Math.Floor(Convert.ToDecimal(txtPlanNow.Text.Trim())).ToString();

                            lblPackedQty.Text = (decimal.Parse(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPTotalPlanQty].ToString()) -
                           decimal.Parse(txtPlanNow.Text)).ToString();
                            lblPackedQty.Text = lblPackedQty.Text == string.Empty ? "0" : lblPackedQty.Text;
                            lblPackedQty.Text = Math.Ceiling(Convert.ToDecimal(lblPackedQty.Text.Trim())).ToString();
                            lblPackedQty.ToolTip = lblPackedQty.Text;

                        }

                        if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            //hdfSaleOrderHdrPK.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            //hdfSaleOrderDtlPK.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPlanPK] != null ? dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPlanPK].ToString() : "0";
                            hdfSaleOrderDtlPK.Value = "0";
                            lblSONo.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Order].ToString();
                            lblSONo.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Order].ToString();
                            lblSODate.Text = Convert.ToDateTime(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderHDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = Convert.ToDateTime(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderHDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblIGPLCode.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPIGPLCode].ToString(), 13);
                            lblIGPLCode.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPIGPLName].ToString();
                            //hdfIGPLCode.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();

                            lblBrandCode.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPBrandName].ToString(), 18);
                            lblBrandCode.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPBrandName].ToString();
                            //hdfBrandCode.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_PK.ToString();

                            lblUOM.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPUOMTEXT].ToString();
                            lblUOM.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPUOMTEXT].ToString();
                            //hdfUOM.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();
                            lblOrderQty.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPOrderQty].ToString();
                            lblOrderQty.Text = Math.Floor(Convert.ToDecimal(lblOrderQty.Text.Trim())).ToString();
                            lblOrderQty.ToolTip = lblOrderQty.Text;

                            lblDespatchedQty.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDQty].ToString();// - (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();
                            lblDespatchedQty.Text = Math.Floor(Convert.ToDecimal(lblDespatchedQty.Text.Trim())).ToString();
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            //txtPlanNow.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            //txtPlanNow.Text = Math.Round(decimal.Parse(txtPlanNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();

                            hdnTotalPcs.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPPQty].ToString();
                            hdfSONumber.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.OrderDtlPK].ToString();

                            lblCTNQty.Text = Math.Ceiling(Convert.ToDouble(txtPlanNow.Text != string.Empty ? txtPlanNow.Text.Trim() : "0") / Convert.ToDouble(hdnTotalPcs.Value != string.Empty ? hdnTotalPcs.Value : "1")).ToString();
                            lblCTNQty.ToolTip = lblCTNQty.Text;

                            lblPackedQty.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDeatilsPlanQty].ToString();
                            lblPackedQty.Text = lblPackedQty.Text == string.Empty ? "0" : lblPackedQty.Text;
                            lblPackedQty.Text = Math.Ceiling(Convert.ToDecimal(lblPackedQty.Text.Trim())).ToString();
                            lblPackedQty.ToolTip = lblPackedQty.Text;

                            txtPlanNow.Text = (decimal.Parse(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPOrderQty].ToString()) -
                           decimal.Parse(lblPackedQty.Text)).ToString();
                            txtPlanNow.Text = Math.Round(decimal.Parse(txtPlanNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            txtPlanNow.Text = Math.Floor(Convert.ToDecimal(txtPlanNow.Text.Trim())).ToString();
                            txtPlanNow.Text = txtPlanNow.Text.Contains('-') ? "0" : txtPlanNow.Text;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {

                    }
                }
                else if (((GridView)sender).ID == "grdShippingPlanList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        RadioButton rbtSelect = e.Row.FindControl("rbtSelect") as RadioButton;
                        LinkButton lnkDoNumber = e.Row.FindControl("lnkDoNumber") as LinkButton;
                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        
                        switch (appstatus)
                        {
                            case (short)WkfStatusEnum.CANCELLED:
                                rbtSelect.Enabled = false;
                                lnkDoNumber.Enabled = false;
                                break;
                        }
                    }
                }
                if (((GridView)sender).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                        Label lblPlantCode = e.Row.FindControl("lblPlantCode") as Label;
                        lblPlantCode.Visible = GetConfigData().IsMultiplePlant;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        public string AddCommas(object number)
        {          
            int curGroup1 = 3;//First seperation after how many digits
            int curGroup2 = 3;//Remaining seperations after how many digits           
            int.TryParse(hdfCurrencyGroup1.Value, out curGroup1);
            int.TryParse(hdfCurrencyGroup2.Value, out curGroup2);
            return CommonFunctions.AddCommaSeperations(number, curGroup1, curGroup2);           
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
            //this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkShippingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerEval.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerInspection.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkEnquiry.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkQuotation.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkLoadingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkUploadPhotographs.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkContainerRelease.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkBillofLoading.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkPrintShippingDocs.PreRender += new EventHandler(btnAction_PreRender);

            this.lnkShippingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkContainerEval.Load += new EventHandler(btnAction_Load);
            this.lnkContainerInspection.Load += new EventHandler(btnAction_Load);
            this.lnkEnquiry.Load += new EventHandler(btnAction_Load);
            this.lnkQuotation.Load += new EventHandler(btnAction_Load);
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
                GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
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
                base.WkfRefID = listingRefID;
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
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
            string path;
            path = GetLocalResourceObject("wkfBaseURL").ToString();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                PageProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                base.WkfPageUrl = path;
                base.WkfPageType = (int)PageTypeEnum.Listing;
            }
        }

        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            SHIPPINGPLANLIST,
            SHIPPINGPLANHDR,
            SHIPPINGPLANDETAILS,
            PLANNO,
            SHIPPINGPLANDTL,
            WRKFSUBMIT,
            STATUS,
            SPDEATILS,
            SPCONTAINERTYPE,
            PRINTLIST,
            GONDETAILS,
            ADMAPPSUBTYPES,
            SPHDR,
            USERCUSTOMER,
            GRIDSHOWMY,
            CARTONALLOCATIONSTATUS,
            BOISTATUS
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 1,
            APPROVED = 2,
            NEW = 0,
            CANCELLED = 4
        }

        #endregion
    }
}