using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.SaleOrder;
using BusinessObject.Sales;
using ERP.Utilities;
using ERPData;
using ERPService;

namespace CustomerPortal.Sales
{
    public partial class InternalOrderDetails : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrQuotationPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrQuotationPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrQuotationPK] = value;
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
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        private SaleOrderBO SaleOrderHeaderSession
        {
            get
            {
                return (SaleOrderBO)Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession] = value;
            }
        }

        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private DataSet dsPageData;
        //page related Entity Object
        private SaleOrderBO saleOrderHeaderObj;
        private SaleOrderDetailsBO saleOrderDetailsObj;
        List<SaleOrderDetailsBO> saleOrderDetailsList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService commonServiceObj;
        SaleOrderDetailsBO saleOrderResponseDtlObj;
        string selectedVendor;
        DataSet dsSaleOrderHeader;
        DataSet dsSaleOrderTaxDetails;
        DataSet dsVendor;
        DataTable dtPageData;
        DataTable dtGrid;
        private int notifyPartyPK;
        private int consigneePK;
        private int agentPK;
        private int selectedItem;
        private int fromPortPK;
        private int originOfGoodsPK;
        private int bankDetailPK;
        private int soTypePK;
        private int custPK;

        private int transhipmentPK;
        private int shipByPK;
        private int deliveryTermPK;
        private int paymentTermPK;
        private int specialCausePK;
        private int inspectionPK;
        private int exportDocPK;

        private int custprodPK;
        private int artPK;

        private string refID;
        private string inboxFlag;
        private int processPK;
        private BusinessObject.User currentUser;

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
            string prefID;
            int cusPK;
            int referenceID;
            int preferenceID;
            int processID;
            int appId;
            referenceID = 0;
            preferenceID = 0;
            processID = 0;
            appId = 0;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    Session[ERP.Utilities.SessionStrings.QuotationHeader] = null;
                    SaleOrderHeaderSession = null;

                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        if (int.TryParse(dtPageData.Rows[0]["CUS_PK"].ToString(), out cusPK) && cusPK > 0)
                        {
                            custPK = cusPK;
                            txtCustomer.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CUS_NAME"].ToString());
                            hdfCustomer.Value = cusPK.ToString();
                            txtCustomer.Enabled = false;
                        }
                    }
                    processID = FillProcessID(1);
                    ucrWrkf.ProcessID = processID;
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                           EntryStatus = EntryStatus.VIEWMODE;
                          //  btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                           EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        referenceID = int.Parse(refID);
                        appId = GetApplicationID(referenceID);
                        if (processPK == ucrWrkf.ProcessID)
                        {
                            CurrPK = appId;
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                        Session[ERP.Utilities.SessionStrings.RefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                          //  btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                          //  EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        preferenceID = int.Parse(prefID);
                        appId = GetApplicationID(preferenceID);
                        //processID = FillProcessID(2);
                        //if (processPK == processID)
                        //{
                        //    CurrQuotationPK = appId;
                        //}
                        CurrQuotationPK = appId;
                        Session[ERP.Utilities.SessionStrings.PRefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }
                    if (Session[ERP.Utilities.SessionStrings.QUOTATIONPK] != null || Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    {
                        AST_DOC_MODE.Value = "0";
                        if (Session[ERP.Utilities.SessionStrings.QUOTATIONPK] != null)
                            CurrQuotationPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.QUOTATIONPK]);
                        if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                            CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERPK]);

                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (Session[ERP.Utilities.SessionStrings.SaleOrderMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.SaleOrderMode];
                        }
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                           // EntryStatus = EntryStatus.VIEWMODE;
                        }
                    }
                    if (CurrPK > 0 || CurrQuotationPK > 0)
                    {
                        GetFieldValues(ControlsEnum.SALEORDER);
                        SetFieldValues(ControlsEnum.SALEORDERHEADER);
                        SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                        if (grdItemDetails.Rows.Count > 0)
                        {
                            SetSubTotal();
                        }
                        if (lblSaleOrderNo.Text.Trim().Equals(string.Empty))
                        {
                            AST_DOC_MODE.Value = GetDOCMODE();
                        }
                        //hdfAppType.Value = ApplicationType.SO;
                        //hdfAppSubType.Value = string.Empty;                        
                    }
                    else
                    {
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InternalOrderListing), false);
                    }
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.SALEORDER:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleOrderHeader(CurrQuotationPK, CurrPK);
                        SaleOrderHeaderSession = saleOrderHeaderObj;
                        if (saleOrderHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "','Quotation.aspx');", true);
                        }
                        break;
                    case ControlsEnum.FROMPORT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.FromPort, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.ORIGINOFGOODS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.OriginOfGoods, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SOTYPE:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break;
                    case ControlsEnum.BANKDETAILS:
                        dtPageData = BusinessLogic.Sales.Enquiry.GetBankDetails(0, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.NOTIFYPARTY:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(notifyPartyPK, custPK, notifyPartyPK > 0 ? 2 : 1, (int)CustomerAddressType.NotifyingParty).Tables[0];
                        break;
                    case ControlsEnum.CONSIGNEE:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(consigneePK, custPK, consigneePK > 0 ? 2 : 1, (int)CustomerAddressType.Consignee).Tables[0];
                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(agentPK, custPK, agentPK > 0 ? 2 : 1, (int)CustomerAddressType.ShippingAgent).Tables[0];
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.Transhipment, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SHIPBY:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.ShipBy, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.DELIVERYTERMS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, deliveryTermPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, specialCausePK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.USERCUSTOMER:
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.ARTWORK:
                        dtGrid = BusinessLogic.Sales.CustomerProduct.GetArtWork(artPK, custprodPK, artPK > 0 ? 2 : 1, string.Empty);
                        break;
                    case ControlsEnum.INSPECTION:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO INSP TYPE");
                        break;
                    case ControlsEnum.EXPORTDOC:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO EXP DOC");
                        break;
                    case ControlsEnum.SALECONTRACT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.SaleContract, 1, 1, currentUser.SBUID);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

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
                    case ControlsEnum.SALEORDERHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SALEORDERDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.BANKDETAILS:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.FROMPORT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.ORIGINOFGOODS:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.CONSIGNEE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.NOTIFYPARTY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SOTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SHIPBY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.DELIVERYTERMS:
                        BindDropDownList(controlType);
                        txtDeliveryTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        BindDropDownList(controlType);
                        txtPaymentTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        BindDropDownList(controlType);
                        txtSpecialCause.Text = string.Empty;
                        break;
                    case ControlsEnum.INSPECTION:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.EXPORTDOC:
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
        #region Helper Methods
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            HiddenField hdfSODPK;
            TextBox txtLotNo;
            TextBox txtLotSize;
            HiddenField hdfArtWork;
            int saleOrdderDtlPK;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SALEORDERHEADER:
                        if (SaleOrderHeaderSession != null)
                        {
                            saleOrderHeaderObj = SaleOrderHeaderSession;
                            saleOrderHeaderObj.SOH_PK = CurrPK;
                            saleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim()) ? string.Empty : lblSaleOrderNo.Text.Trim();
                            saleOrderHeaderObj.SOH_TYPE = Convert.ToInt32(ddlSaleOrderType.SelectedValue);
                            //saleOrderHeaderObj.SOH_VERSION = 1;
                            saleOrderHeaderObj.SOH_DATE = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtSaleOrderDate.Text.Trim();
                            saleOrderHeaderObj.SOH_PK = CurrPK;
                            //saleOrderHeaderObj.SOH_STATUS = 0;
                            //saleOrderHeaderObj.SOH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            if (ddlBankDetails.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_BANK = ddlBankDetails.SelectedValue;
                            if (ddlFromPort.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_FROM_PORT = ddlFromPort.SelectedValue;
                            if (ddlOriginofGoods.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_ORG_GOODS = ddlOriginofGoods.SelectedValue;

                            if (ddlTranshipment.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_TRANSHIPMENT = ddlTranshipment.SelectedValue;
                            if (ddlShipBy.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_SHIP_BY = ddlShipBy.SelectedValue;

                            saleOrderHeaderObj.SOH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                            if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_DEL_TERM = ddlDeliveryTerms.SelectedValue;
                            saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT = HttpUtility.HtmlEncode(txtPaymentTerms.Text);
                            if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                            saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT = HttpUtility.HtmlEncode(txtSpecialCause.Text);
                            if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_SPECIAL_TERM = ddlSpecialCause.SelectedValue;
                            saleOrderHeaderObj.SOH_TO_PORT = HttpUtility.HtmlEncode(txtToPort.Text);

                            saleOrderHeaderObj.SOH_INSP_TYPE = Convert.ToInt32(ddlInspection.SelectedValue);
                            saleOrderHeaderObj.SOH_EXP_DOC = Convert.ToInt32(ddlExportDoc.SelectedValue);

                            if (ddlNotifyParty.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY = ddlNotifyParty.SelectedValue;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME = hdfNPName.Value;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS = hdfNPAddress.Value;
                                if (hdfNPCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY = hdfNPCountry.Value.Trim();
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT = hdfNPCountryText.Value;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL = hdfNPEmail.Value;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX = hdfNPFax.Value;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE = hdfNPMobile.Value;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE = hdfNPPhone.Value;
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP = hdfNPZip.Value;
                            }

                            if (ddlConsigneeDetails.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_CONSIGNEE = ddlConsigneeDetails.SelectedValue;
                                saleOrderHeaderObj.SOH_CONSIGNEE_NAME = hdfCNEName.Value;
                                saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS = hdfCNEAddress.Value;
                                if (hdfCNECountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY = hdfCNECountry.Value.Trim();
                                saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT = hdfCNECountryText.Value;
                                saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL = hdfCNEEmail.Value;
                                saleOrderHeaderObj.SOH_CONSIGNEE_FAX = hdfCNEFax.Value;
                                saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE = hdfCNEMobile.Value;
                                saleOrderHeaderObj.SOH_CONSIGNEE_PHONE = hdfCNEPhone.Value;
                                saleOrderHeaderObj.SOH_CONSIGNEE_ZIP = hdfCNEZip.Value;
                            }
                            if (ddlAgent.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_SHIPPING_TO = ddlAgent.SelectedValue;

                            saleOrderHeaderObj.SOH_SHIP_INT_TO = HttpUtility.HtmlEncode(txtShppingIntimationto.Text);
                            saleOrderHeaderObj.SOH_FAX = HttpUtility.HtmlEncode(txtShppingIntimationtoFax.Text);
                            saleOrderHeaderObj.SOH_PACKING_INSTRN = HttpUtility.HtmlEncode(txtPackingInstruction.Text);
                            saleOrderHeaderObj.SOH_FINAL_DESTINATION = HttpUtility.HtmlEncode(txtPortofDischarge.Text);
                            //Agent
                            saleOrderHeaderObj.SOH_SUPP_DTL = HttpUtility.HtmlEncode(txtSupplimentarydetails.Text);
                            saleOrderHeaderObj.SOH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            saleOrderHeaderObj.SOH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            saleOrderHeaderObj.ACTIVE = saleOrderHeaderObj.SOH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            ////saleOrderHeaderObj.SOH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                            ////saleOrderHeaderObj.SOH_CRTD_DT = DateTime.Now;
                            ////saleOrderHeaderObj.SOH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                            ////saleOrderHeaderObj.SOH_MOD_DT = DateTime.Now;

                            //saleOrderResponsDetailsList = new List<SaleOrderDetailsBO>();
                            //saleOrderResponsDetailsList = (List<SaleOrderDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                            //if (saleOrderResponsDetailsList != null && saleOrderResponsDetailsList.Count > 0)
                            //    saleOrderHeaderObj.SaleOrderDetails = saleOrderResponsDetailsList;
                            SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);

                            //saleOrderHeaderObj.SOH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);

                            saleOrderHeaderObj.USER_PK = currentUser.PKUser;
                            saleOrderHeaderObj.LAST_MOD_DT = DateTime.Now;
                            saleOrderHeaderObj.APT_CODE = ApplicationType.SO;
                            saleOrderHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            saleOrderHeaderObj.WKF_FLAG = 0;
                            if (commonActions == ActionsEnum.SAVE)
                            {
                                saleOrderHeaderObj.WKF_FLAG = 0;
                            }
                            else if (commonActions == ActionsEnum.WRKFSUBMIT)
                            {
                                saleOrderHeaderObj.WKF_FLAG = 1;
                            }
                            retObject = saleOrderHeaderObj;
                        }
                        break;
                    case ControlsEnum.SALEORDERDETAIL:
                        if (SaleOrderHeaderSession.SaleOrderDetails != null && SaleOrderHeaderSession.SaleOrderDetails.Count > 0)
                        {
                            foreach (GridViewRow grdrow in grdItemDetails.Rows)
                            {
                                hdfSODPK = (HiddenField)grdrow.FindControl("hdfSODPK");
                                saleOrdderDtlPK = hdfSODPK == null ? 0 : Convert.ToInt32(hdfSODPK.Value);
                                saleOrderDetailsObj = SaleOrderHeaderSession.SaleOrderDetails.SingleOrDefault(dtl => dtl.SOD_PK == saleOrdderDtlPK);
                                if (saleOrderDetailsObj != null)
                                {
                                    txtLotNo = (TextBox)grdrow.FindControl("txtLotNo");
                                    if (txtLotNo != null)
                                        saleOrderDetailsObj.SOD_LOT_NO = HttpUtility.HtmlEncode(txtLotNo.Text);
                                    txtLotSize = (TextBox)grdrow.FindControl("txtLotSize");
                                    if (txtLotSize != null)
                                        saleOrderDetailsObj.SOD_LOT_SIZE = HttpUtility.HtmlEncode(txtLotSize.Text);
                                    hdfArtWork = (HiddenField)grdrow.FindControl("hdfArtWork");
                                    if (hdfArtWork != null)
                                        saleOrderDetailsObj.SOD_ART_WORK = hdfArtWork.Value;
                                }
                            }
                        }
                        retObject = null;
                        break;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            string addr;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SALEORDERHEADER:
                        if (saleOrderHeaderObj != null)
                        {
                            //if (custPK > 0)
                            //{
                            //    btnPrintIO.Visible = false;
                            //    //if (saleOrderHeaderObj.SOH_STATUS != 2)
                            //    //    btnPrintSO.Visible = false;
                            //}
                            //else
                            //{
                            //    if (saleOrderHeaderObj.SOH_STATUS != 2)
                            //        btnPrintIO.Visible = false;
                            //}

                            txtCustomer.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_TEXT);
                            txtBuyerAddress.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS)
                                + (string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT.Trim()) ? "" : (", " +
                                HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT)));
                            custPK = saleOrderHeaderObj.SOH_CUSTOMER;
                            hdfCustomer.Value = saleOrderHeaderObj.SOH_CUSTOMER.ToString();
                            lblSaleOrderNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_NO) ? string.Empty : saleOrderHeaderObj.SOH_NO;
                            CurrPK = saleOrderHeaderObj.SOH_PK;
                            GetFieldValues(ControlsEnum.SOTYPE);
                            soTypePK = saleOrderHeaderObj.SOH_TYPE;
                            SetFieldValues(ControlsEnum.SOTYPE);
                            txtSaleOrderDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_DATE) ? string.Empty : saleOrderHeaderObj.SOH_DATE;

                            lblReferenceNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_NO) ? string.Empty : saleOrderHeaderObj.SOH_REF_NO;

                            GetFieldValues(ControlsEnum.FROMPORT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FROM_PORT))
                                fromPortPK = Convert.ToInt32(saleOrderHeaderObj.SOH_FROM_PORT);
                            SetFieldValues(ControlsEnum.FROMPORT);
                            GetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_ORG_GOODS))
                                originOfGoodsPK = Convert.ToInt32(saleOrderHeaderObj.SOH_ORG_GOODS);
                            SetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            GetFieldValues(ControlsEnum.BANKDETAILS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BANK))
                                bankDetailPK = Convert.ToInt32(saleOrderHeaderObj.SOH_BANK);
                            SetFieldValues(ControlsEnum.BANKDETAILS);

                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_BY))
                            //{
                            //    hdfShipBy.Value = saleOrderHeaderObj.SOH_SHIP_BY;
                            //    txtShipBy.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_BY_TEXT);
                            //}

                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_TO_PORT))
                            //{
                            //    txtToPort.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TO_PORT);
                            //}

                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_TRANSHIPMENT))
                            //{
                            //    hdfTranshipment.Value = saleOrderHeaderObj.SOH_TRANSHIPMENT;
                            //    txtTranshipment.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TRANSHIPMENT_TEXT);
                            //}

                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM))
                            //{
                            //    hdfDeliveryTerms.Value = saleOrderHeaderObj.SOH_DEL_TERM;
                            //    txtDeliveryTerms_Txt.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_NAME);
                            //}
                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM_TEXT))
                            //    txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_TEXT);

                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM))
                            //{
                            //    hdfPaymentTerms.Value = saleOrderHeaderObj.SOH_PAYMENT_TERM;
                            //    txtPaymentTerms_Txt.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_NAME);
                            //}
                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT))
                            //    txtPaymentTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT);

                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM))
                            //{
                            //    hdfSpecialCause.Value = saleOrderHeaderObj.SOH_SPECIAL_TERM;
                            //    txtSpecialCause_Txt.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_NAME);
                            //}
                            //if (!String.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT))
                            //    txtSpecialCause.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT);

                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_TRANSHIPMENT))
                                transhipmentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);
                            GetFieldValues(ControlsEnum.SHIPBY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_BY))
                                shipByPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);

                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM))
                                deliveryTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_DEL_TERM);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM))
                                paymentTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_PAYMENT_TERM);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM))
                                specialCausePK = Convert.ToInt32(saleOrderHeaderObj.SOH_SPECIAL_TERM);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);

                            txtToPort.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TO_PORT);

                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_TEXT);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT);
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT);

                            hdfCurrency.Value = saleOrderHeaderObj.SOH_CURRENCY.ToString();
                            txtCurrency.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CURRENCY_TEXT);

                            GetFieldValues(ControlsEnum.INSPECTION);
                            inspectionPK = saleOrderHeaderObj.SOH_INSP_TYPE;
                            SetFieldValues(ControlsEnum.INSPECTION);
                            GetFieldValues(ControlsEnum.EXPORTDOC);
                            exportDocPK = saleOrderHeaderObj.SOH_EXP_DOC;
                            SetFieldValues(ControlsEnum.EXPORTDOC);

                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY))
                                notifyPartyPK = Convert.ToInt32(saleOrderHeaderObj.SOH_NOTIFY_PARTY);
                            SetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME))
                            {
                                hdfNPName.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME;
                            }
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY))
                            {
                                hdfNPCountry.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY;
                            }
                            txtNotifyParty.Text = hdfNPAddress.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT))
                                hdfNPCountryText.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL))
                                hdfNPEmail.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX))
                                hdfNPFax.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE))
                                hdfNPMobile.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE))
                                hdfNPPhone.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP))
                                hdfNPZip.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP;

                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE))
                                consigneePK = Convert.ToInt32(saleOrderHeaderObj.SOH_CONSIGNEE);
                            SetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_NAME))
                                hdfCNEName.Value = saleOrderHeaderObj.SOH_CONSIGNEE_NAME;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY))
                                hdfCNECountry.Value = saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY;
                            txtConsigneeDetails.Text = hdfCNEAddress.Value = saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS;

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT))
                                hdfCNECountryText.Value = saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL))
                                hdfCNEEmail.Value = saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_FAX))
                                hdfCNEFax.Value = saleOrderHeaderObj.SOH_CONSIGNEE_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE))
                                hdfCNEMobile.Value = saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_PHONE))
                                hdfCNEPhone.Value = saleOrderHeaderObj.SOH_CONSIGNEE_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP))
                                hdfCNEZip.Value = saleOrderHeaderObj.SOH_CONSIGNEE_ZIP;

                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_TO))
                                agentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIPPING_TO);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);

                            txtPackingInstruction.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PACKING_INSTRN);
                            txtPortofDischarge.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FINAL_DESTINATION);
                            txtShppingIntimationto.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_INT_TO);
                            txtShppingIntimationtoFax.Text = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FAX) ? string.Empty : HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FAX);
                            txtSupplimentarydetails.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SUPP_DTL);

                            //if (saleOrderHeaderObj.SOH_STATUS == 1 || saleOrderHeaderObj.SOH_STATUS == 7)
                            //{
                            //    txtSaleOrderDate.Enabled = false;
                            //    ddlSaleOrderType.Enabled = false;
                            //    ddlBankDetails.Enabled = false;
                            //    ddlFromPort.Enabled = false;
                            //    ddlOriginofGoods.Enabled = false;

                            //    ddlSpecialCause.Enabled = false;
                            //    txtSpecialCause.Enabled = false;
                            //    ddlDeliveryTerms.Enabled = false;
                            //    txtDeliveryTerms.Enabled = false;
                            //    ddlPaymentTerms.Enabled = false;
                            //    txtPaymentTerms.Enabled = false;
                            //    ddlShipBy.Enabled = false;
                            //    ddlTranshipment.Enabled = false;
                            //    txtToPort.Enabled = false;
                            //    GetFieldValues(ControlsEnum.SALECONTRACT);
                            //    lnkTerms.Visible = true;
                            //    ltrTerms.Text = (dtPageData != null && dtPageData.Rows.Count > 0) ? dtPageData.Rows[0]["CON_DESC"].ToString() : string.Empty;
                            //}
                            //else
                                lnkTerms.Visible = false;
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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.NOTIFYPARTY:
                        ddlNotifyParty.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlNotifyParty.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlNotifyParty.DataTextField = "CAD_NAME";
                            ddlNotifyParty.DataValueField = "CAD_PK";
                            ddlNotifyParty.DataBind();
                        }
                        ddlNotifyParty.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (notifyPartyPK > 0)
                            ddlNotifyParty.SelectedValue = notifyPartyPK.ToString();
                        break;
                    case ControlsEnum.CONSIGNEE:
                        ddlConsigneeDetails.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlConsigneeDetails.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlConsigneeDetails.DataTextField = "CAD_NAME";
                            ddlConsigneeDetails.DataValueField = "CAD_PK";
                            ddlConsigneeDetails.DataBind();
                        }
                        ddlConsigneeDetails.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (consigneePK > 0)
                            ddlConsigneeDetails.SelectedValue = consigneePK.ToString();
                        break;
                    case ControlsEnum.FROMPORT:
                        ddlFromPort.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlFromPort.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlFromPort.DataTextField = "CON_NAME";
                            ddlFromPort.DataValueField = "CON_PK";
                            ddlFromPort.DataBind();
                        }
                        ddlFromPort.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (fromPortPK > 0)
                            ddlFromPort.SelectedValue = fromPortPK.ToString();
                        break;
                    case ControlsEnum.ORIGINOFGOODS:
                        ddlOriginofGoods.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlOriginofGoods.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlOriginofGoods.DataTextField = "CON_NAME";
                            ddlOriginofGoods.DataValueField = "CON_PK";
                            ddlOriginofGoods.DataBind();
                        }
                        ddlOriginofGoods.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (originOfGoodsPK > 0)
                            ddlOriginofGoods.SelectedValue = originOfGoodsPK.ToString();
                        break;
                    case ControlsEnum.BANKDETAILS:
                        ddlBankDetails.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlBankDetails.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CBM_NAME");
                            ddlBankDetails.DataTextField = "CBM_NAME";
                            ddlBankDetails.DataValueField = "CBM_PK";
                            ddlBankDetails.DataBind();
                        }
                        ddlBankDetails.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (bankDetailPK > 0)
                            ddlBankDetails.SelectedValue = bankDetailPK.ToString();
                        break;
                    case ControlsEnum.SOTYPE:
                        ddlSaleOrderType.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlSaleOrderType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlSaleOrderType.DataTextField = "CFG_DATA";
                            ddlSaleOrderType.DataValueField = "CFG_VALUE";
                            ddlSaleOrderType.DataBind();
                        }
                        ddlSaleOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (soTypePK > 0)
                            ddlSaleOrderType.SelectedValue = soTypePK.ToString();
                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        ddlAgent.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlAgent.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlAgent.DataTextField = "CAD_NAME";
                            ddlAgent.DataValueField = "CAD_PK";
                            ddlAgent.DataBind();
                        }
                        ddlAgent.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (agentPK > 0)
                            ddlAgent.SelectedValue = agentPK.ToString();
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        ddlTranshipment.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlTranshipment.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlTranshipment.DataTextField = "CON_NAME";
                            ddlTranshipment.DataValueField = "CON_PK";
                            ddlTranshipment.DataBind();
                        }
                        ddlTranshipment.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (transhipmentPK > 0)
                            ddlTranshipment.SelectedValue = transhipmentPK.ToString();
                        break;
                    case ControlsEnum.SHIPBY:
                        ddlShipBy.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlShipBy.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                            ddlShipBy.DataTextField = "CON_NAME";
                            ddlShipBy.DataValueField = "CON_PK";
                            ddlShipBy.DataBind();
                        }
                        ddlShipBy.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (shipByPK > 0)
                            ddlShipBy.SelectedValue = shipByPK.ToString();
                        break;
                    case ControlsEnum.DELIVERYTERMS:
                        ddlDeliveryTerms.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlDeliveryTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "TCH_NAME");
                            ddlDeliveryTerms.DataTextField = "TCH_NAME";
                            ddlDeliveryTerms.DataValueField = "TCH_PK";
                            ddlDeliveryTerms.DataBind();
                        }
                        ddlDeliveryTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (deliveryTermPK > 0)
                            ddlDeliveryTerms.SelectedValue = deliveryTermPK.ToString();
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        ddlPaymentTerms.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlPaymentTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "TCH_NAME");
                            ddlPaymentTerms.DataTextField = "TCH_NAME";
                            ddlPaymentTerms.DataValueField = "TCH_PK";
                            ddlPaymentTerms.DataBind();
                        }
                        ddlPaymentTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (paymentTermPK > 0)
                            ddlPaymentTerms.SelectedValue = paymentTermPK.ToString();
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        ddlSpecialCause.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlSpecialCause.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "TCH_NAME");
                            ddlSpecialCause.DataTextField = "TCH_NAME";
                            ddlSpecialCause.DataValueField = "TCH_PK";
                            ddlSpecialCause.DataBind();
                        }
                        ddlSpecialCause.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (specialCausePK > 0)
                            ddlSpecialCause.SelectedValue = specialCausePK.ToString();
                        break;
                    case ControlsEnum.INSPECTION:
                        ddlInspection.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlInspection.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlInspection.DataTextField = "CFG_DATA";
                            ddlInspection.DataValueField = "CFG_VALUE";
                            ddlInspection.DataBind();
                        }
                        ddlInspection.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (inspectionPK > 0)
                            ddlInspection.SelectedValue = inspectionPK.ToString();
                        break;
                    case ControlsEnum.EXPORTDOC:
                        ddlExportDoc.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlExportDoc.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CFG_DATA");
                            ddlExportDoc.DataTextField = "CFG_DATA";
                            ddlExportDoc.DataValueField = "CFG_VALUE";
                            ddlExportDoc.DataBind();
                        }
                        ddlExportDoc.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (exportDocPK > 0)
                            ddlExportDoc.SelectedValue = exportDocPK.ToString();
                        break;
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
                    case ControlsEnum.SALEORDERDETAIL:
                        if (saleOrderHeaderObj != null)
                        {
                            saleOrderDetailsList = new List<SaleOrderDetailsBO>();
                            saleOrderDetailsList = saleOrderHeaderObj.SaleOrderDetails;
                            if (saleOrderDetailsList != null)
                            {
                                grdItemDetails.DataSource = saleOrderDetailsList;
                                grdItemDetails.DataBind();
                            }
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Validate for invalid entry in Autocomplete fields
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>Validation Status</returns>
        private bool ValidateForm(ActionsEnum mode)
        {
            HiddenField hdfArtWork;
            bool flag;
            flag = true;
            switch (mode)
            {
                case ActionsEnum.SAVE:
                case ActionsEnum.WRKFSUBMIT:
                    foreach (GridViewRow grdrow in grdItemDetails.Rows)
                    {
                        hdfArtWork = (HiddenField)grdrow.FindControl("hdfArtWork");
                        if (hdfArtWork != null)
                        {
                            if (hdfArtWork.Value == string.Empty || hdfArtWork.Value == CommonConstants.SELECT_VALUE_ZERO)
                            {
                                litErrorMsg.Text = this.GetLocalResourceObject("Err_ArtWork").ToString();
                                flag = false;
                            }
                        }
                    }
                    break;
            }
            return flag;
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.SALEORDERHEADER:
                    CurrPK = 0;
                    CurrQuotationPK = 0;
                    break;
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            commonServiceObj = new CommonService();
            AppTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.SO, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        private void SetSubTotal()
        {
            Label lblSubTotalFooter;
            lblSubTotalFooter = grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label;
            if (lblSubTotalFooter != null)
            {
                SaleOrderHeaderSession.SOH_TOTAL_AMT = Convert.ToDouble(SaleOrderHeaderSession.SaleOrderDetails.Sum(dtl => dtl.SOD_NET_AMOUNT));
                lblSubTotalFooter.Text = SaleOrderHeaderSession.SOH_TOTAL_AMT.ToString("c");
            }
        }
        public string GetCeiledInteger(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return Math.Ceiling(num).ToString();
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
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID(int type)
        {
            int processID = 0;
            string path = string.Empty;
            if (type == 1)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            else
            {
                path = "Sales/Quotation.aspx";
            }
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
            }
            return processID;
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
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                string itemAmount;
                //List<RFQTaxSplit> tempRfqTaxDtlSplit;
                //RFQTaxSplit tempRFQTaxSplitObj;
                List<QuotationTaxHdr> tempQuotationTaxHdrSplit;
                QuotationTaxHdr tempQuotationTaxSplitObj = null;
                HiddenField hdfSODPK;
                HiddenField hdfItemPK;
                HiddenField hdfCusItemPK;
                TextBox txtAmount;
                TextBox txtTax;
                TextBox txtDiscount;
                TextBox txtSubTotal;
                string selectedItemPK;
                IEnumerable<SaleOrderDetailsBO> selectedQuotationDetails;
                double amount;
                double discount;
                int count;
                string action;
                DropDownList ddlWkfAction;
                string addr;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    //if (((DropDownList)sender).ID == "ddlVendor")
                    //{
                    //    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    //}
                    if (((DropDownList)sender).ID == "ddlNotifyParty")
                    {
                        commonActions = ActionsEnum.NOTIFYPARTYSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlConsigneeDetails")
                    {
                        commonActions = ActionsEnum.CONSIGNEESELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlCustAddress")
                    {
                        commonActions = ActionsEnum.ADDRESSSELECTED;
                    }
                    if (((DropDownList)sender).ID == "ddlDeliveryTerms")
                    {
                        commonActions = ActionsEnum.DELTERMSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlPaymentTerms")
                    {
                        commonActions = ActionsEnum.PAYTERMSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlSpecialCause")
                    {
                        commonActions = ActionsEnum.SPECAUSESELECTED;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.NOTIFYPARTYSELECTED:
                        notifyPartyPK = Convert.ToInt32(ddlNotifyParty.SelectedValue);
                        if (notifyPartyPK > 0)
                        {
                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfNPName.Value = dtPageData.Rows[0]["CAD_NAME"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfNPCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                                }
                                hdfNPAddress.Value = txtNotifyParty.Text = addr;

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                    hdfNPCountryText.Value = dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                    hdfNPEmail.Value = dtPageData.Rows[0]["CAD_EMAIL"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                    hdfNPFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                    hdfNPMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                    hdfNPPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                    hdfNPZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                            }
                            else
                            {
                                txtNotifyParty.Text = string.Empty;
                                hdfNPName.Value = string.Empty;
                                hdfNPAddress.Value = string.Empty;
                                hdfNPCountry.Value = string.Empty;
                                hdfNPCountryText.Value = string.Empty;
                                hdfNPEmail.Value = string.Empty;
                                hdfNPFax.Value = string.Empty;
                                hdfNPMobile.Value = string.Empty;
                                hdfNPPhone.Value = string.Empty;
                                hdfNPZip.Value = string.Empty;
                            }
                        }
                        else
                        {
                            txtNotifyParty.Text = string.Empty;
                            hdfNPName.Value = string.Empty;
                            hdfNPAddress.Value = string.Empty;
                            hdfNPCountry.Value = string.Empty;
                            hdfNPCountryText.Value = string.Empty;
                            hdfNPEmail.Value = string.Empty;
                            hdfNPFax.Value = string.Empty;
                            hdfNPMobile.Value = string.Empty;
                            hdfNPPhone.Value = string.Empty;
                            hdfNPZip.Value = string.Empty;
                        }
                        break;
                    case ActionsEnum.CONSIGNEESELECTED:
                        consigneePK = Convert.ToInt32(ddlConsigneeDetails.SelectedValue);
                        if (consigneePK > 0)
                        {
                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfCNEName.Value = dtPageData.Rows[0]["CAD_NAME"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfCNECountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString());
                                }
                                else if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_OTHER"].ToString());
                                }
                                hdfCNEAddress.Value = txtConsigneeDetails.Text = addr;

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                    hdfCNECountryText.Value = dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                    hdfCNEEmail.Value = dtPageData.Rows[0]["CAD_EMAIL"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                    hdfCNEFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                    hdfCNEMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                    hdfCNEPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                    hdfCNEZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                            }
                            else
                            {
                                txtConsigneeDetails.Text = string.Empty;
                                hdfCNEName.Value = string.Empty;
                                hdfCNEAddress.Value = string.Empty;
                                hdfCNECountry.Value = string.Empty;
                                hdfCNECountryText.Value = string.Empty;
                                hdfCNEEmail.Value = string.Empty;
                                hdfCNEFax.Value = string.Empty;
                                hdfCNEMobile.Value = string.Empty;
                                hdfCNEPhone.Value = string.Empty;
                                hdfCNEZip.Value = string.Empty;
                            }
                        }
                        else
                        {
                            txtConsigneeDetails.Text = string.Empty;
                            hdfCNEName.Value = string.Empty;
                            hdfCNEAddress.Value = string.Empty;
                            hdfCNECountry.Value = string.Empty;
                            hdfCNECountryText.Value = string.Empty;
                            hdfCNEEmail.Value = string.Empty;
                            hdfCNEFax.Value = string.Empty;
                            hdfCNEMobile.Value = string.Empty;
                            hdfCNEPhone.Value = string.Empty;
                            hdfCNEZip.Value = string.Empty;
                        }
                        break;
                    case ActionsEnum.DELTERMSELECTED:
                        if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            deliveryTermPK = Convert.ToInt32(ddlDeliveryTerms.SelectedValue);
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtDeliveryTerms.Text = string.Empty;
                        break;
                    case ActionsEnum.PAYTERMSELECTED:
                        if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            paymentTermPK = Convert.ToInt32(ddlPaymentTerms.SelectedValue);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtPaymentTerms.Text = string.Empty;
                        break;
                    case ActionsEnum.SPECAUSESELECTED:
                        if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            specialCausePK = Convert.ToInt32(ddlSpecialCause.SelectedValue);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtSpecialCause.Text = string.Empty;
                        break;
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (ValidateForm(commonActions))
                        {
                            saleOrderHeaderObj = new SaleOrderBO();
                            saleOrderHeaderObj = (SaleOrderBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);

                            if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleOrderDetails != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<SaleOrderBO>(saleOrderHeaderObj);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
                                // save Process Control inspection details
                                result = BusinessLogic.Sales.SaleOrderBL.SaveSaleOrderDetails(xmlDoc);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    // Show Save Message and redired to listing page
                                    //string routeURL = "RFQResponse.aspx";
                                    //GetFieldValues(ControlsEnum.SALEORDER);
                                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InternalOrder);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InternalOrderListing) + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
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
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (CurrPK > 0)
                        {
                            //Workflow submission
                            ucrWrkf.ApplicationID = CurrPK;
                            ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            //Do WorkFlow if WorkFlow has Actions
                            if (ddlWkfAction.Items.Count > 0)
                            {
                                action = ddlWkfAction.SelectedItem.ToString();
                                result = ucrWrkf.DoWorkFlow();
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.InternalOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InternalOrderListing) + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINTSO:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE="), false);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);

                        break;
                    case ActionsEnum.PRINTIO:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE="), false);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);

                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.SALEORDERHEADER);
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InternalOrderListing), false);

                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {

            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            HiddenField hdfArtWorkUrl;
            HiddenField hdfArtWork;
            try
            {
                if ((sender as GridView).ID == "grdItemDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        hdfArtWork = e.Row.FindControl("hdfArtWork") as HiddenField;
                        artPK = string.IsNullOrEmpty(hdfArtWork.Value) ? 0 : Convert.ToInt32(hdfArtWork.Value);
                        if (artPK > 0)
                        {
                            hdfArtWorkUrl = e.Row.FindControl("hdfArtWorkUrl") as HiddenField;
                            GetFieldValues(ControlsEnum.ARTWORK);
                            if (dtGrid != null && dtGrid.Rows.Count > 0)
                            {
                                hdfArtWorkUrl.Value = dtGrid.Rows[0]["CIA_FILE_PATH"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
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
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintIO.PreRender += new EventHandler(btnAction_PreRender);

            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnPrintIO.Load += new EventHandler(btnAction_Load);
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
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
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
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            BANKDETAILS,
            SALEORDER,
            SALEORDERHEADER,
            SALEORDERDETAIL,
            FROMPORT,
            ORIGINOFGOODS,
            NOTIFYPARTY,
            CONSIGNEE,
            SOTYPE,
            SHIPPINGAGENT,
            TRANSHIPMENT,
            SHIPBY,
            DELIVERYTERMS,
            PAYMENTTERMS,
            SPECIALCAUSE,
            USERCUSTOMER,
            ARTWORK,
            INSPECTION,
            EXPORTDOC,
            SALECONTRACT
        }

        #endregion
    }
}