using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessObject.SaleOrder;
using ERPService;
using ERPData;
using System.Threading;
using BusinessObject.PurchaseOrderManagement;

namespace ERPSMS_v01.Sales
{
    public partial class EnquiryDetails : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Is Customer User
        /// </summary>
        private bool IsCustomerUser
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomerUser] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomerUser]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomerUser] = value;
            }
        }
        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
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
        private List<BusinessObject.SaleOrder.EnquiryDetails> EnqDtlList
        {
            get
            {
                return (List<BusinessObject.SaleOrder.EnquiryDetails>)Session["EnqDtlList"];
            }
            set
            {
                Session["EnqDtlList"] = value;
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
        private DataSet dsPageData;
        private DataTable dtPageData;
        private string trxNo;
        private BusinessObject.User currentUser;
        private EnquiryHeader rfqHeadObj;
        BusinessObject.SaleOrder.EnquiryDetails enqDtlObj;
        private CommonService commonServiceObj;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> appTypeDetailsList;
        private int productPK;
        private int custprodPK;
        private int custPK;
        private int addressPK;
        private int addressActive;
        private int fromPortPK;
        private int transhipmentPK;
        private int shipByPK;
        private int originOfGoodsPK;
        private double exchangeRate;

        private int deliveryTermPK;
        private int paymentTermPK;
        private int specialCausePK;
        private int bankDetailPK;

        private string refID;
        private string inboxFlag;

        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            int cusPK;
            string resultXml;
            string xmlHeader;
            string addr;
            resultXml = string.Empty;
            xmlHeader = string.Empty;

            DateDefaultEnum defaultDate;
            int defaultAddMonths;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }
                    AST_DOC_MODE.Value = "0";
                    //txtReqByDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                    txtEnqDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();

                    defaultDate = DateDefaultEnum.CurrentDate;
                    defaultAddMonths = 0;
                    Enum.TryParse(GetLocalResourceObject("DefaultDayReqdBy").ToString(), out defaultDate);
                    int.TryParse(GetLocalResourceObject("DefaultAddMonthsReqdBy").ToString(), out defaultAddMonths);
                    txtReqByDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ?
                        defaultAddMonths + 1 : defaultAddMonths).AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                        defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "CED_SL_NO";
                    grdEnquiryDetails.DataKeyNames = itemkeyarray;
                    EnqDtlList = null;

                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        if (int.TryParse(dtPageData.Rows[0]["CUS_PK"].ToString(), out cusPK) && cusPK > 0)
                        {
                            custPK = cusPK;
                            txtCustomer.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CUS_NAME"].ToString());
                            hdfCustomer.Value = cusPK.ToString();
                            txtCustomer.Enabled = false;
                            IsCustomerUser = true;
                        }
                    }

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
                            //btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                        Session[ERP.Utilities.SessionStrings.RefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }
                    else if (Session[ERP.Utilities.SessionStrings.ENQUIRYPK] != null)
                    {
                        CurrPK = (int)Session[ERP.Utilities.SessionStrings.ENQUIRYPK];
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (Session["EnquiryMode"] != null)
                        {
                            EntryStatus = (EntryStatus)Session["EnquiryMode"];
                        }
                        else
                            EntryStatus = EntryStatus.VIEWMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                    }
                    if (CurrPK > 0)
                    {
                        GetFieldValues(ControlsEnum.ENQUIRY);
                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsPageData.Tables[0].Rows)
                            {
                                xmlHeader += dr[0].ToString();
                            }
                            if (!string.IsNullOrEmpty(xmlHeader))
                                rfqHeadObj = CommonFunctions.XmlDeserialize<EnquiryHeader>(xmlHeader);
                        }
                        else
                            rfqHeadObj = null;
                        SetFieldValues(ControlsEnum.ENQUIRY);
                        txtEnqDate.Focus();
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.TRANSHIPMENT);
                        SetFieldValues(ControlsEnum.TRANSHIPMENT);
                        if (dtPageData != null && dtPageData.Rows.Count > 0
                             && ddlTranshipment.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                            ddlTranshipment.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();
                        GetFieldValues(ControlsEnum.SHIPBY);
                        SetFieldValues(ControlsEnum.SHIPBY);
                        if (custPK != 0)
                        {
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);

                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                             && ddlCustAddress.Items.FindByValue(dtPageData.Rows[0]["CAD_PK"].ToString()) != null)
                            {
                                ddlCustAddress.SelectedValue = dtPageData.Rows[0]["CAD_PK"].ToString();
                                addr = string.Empty;
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
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                }
                                txtShippingAddress.Text = addr;
                            }
                            else
                                txtShippingAddress.Text = string.Empty;
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                             && ddlSpecialCause.Items.FindByValue(dtPageData.Rows[0]["TCH_PK"].ToString()) != null)
                            {
                                ddlSpecialCause.SelectedValue = dtPageData.Rows[0]["TCH_PK"].ToString();
                                txtSpecialCause.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                            }
                            else
                                txtSpecialCause.Text = string.Empty;
                            GetFieldValues(ControlsEnum.CUSTOMERCURRENCY);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                hdfCurrency.Value = dsPageData.Tables[0].Rows[0]["CUR_PK"].ToString();
                                txtCurrency.Text = string.Format(Resources.ErpRes.NameCodeFormat, dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString()
                                    , dsPageData.Tables[0].Rows[0]["CUR_NAME"].ToString());
                                txtToPort.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_SHIP_TO_PORT"].ToString());
                            }
                            else
                            {
                                hdfCurrency.Value = string.Empty;
                                txtCurrency.Text = string.Empty;
                                txtToPort.Text = string.Empty;
                            }
                            GetFieldValues(ControlsEnum.CUSTOMERENQUIRY);
                            if (dsPageData != null && dsPageData.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsPageData.Tables[0].Rows)
                                {
                                    xmlHeader += dr[0].ToString();
                                }
                                if (!string.IsNullOrEmpty(xmlHeader))
                                    rfqHeadObj = CommonFunctions.XmlDeserialize<EnquiryHeader>(xmlHeader);
                            }
                            else
                                rfqHeadObj = null;
                            SetFieldValues(ControlsEnum.CUSTOMERENQUIRY);
                            txtEnqDate.Focus();
                        }
                        else
                        {
                            dtPageData = null;
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            // txtCustomer.Focus();//Commented for tab Order Correction For New Design
                        }
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewAction();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false)
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.NEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        lblEnqTrxNoTxt.Text = Resources.Messages.DocGenerationNew;
                        AST_DOC_MODE.Value = GetDOCMODE();
                    }

                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = null;
                    Session["EnquiryMode"] = null;

                    hdfAppType.Value = ApplicationType.CENQ;
                    hdfAppSubType.Value = string.Empty;
                }
                //if (hdfSubTab.Value.Equals("Hdr"))
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return HideDtl();});", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){return HideHdr();});", true);
                //}
                ////ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "return HideDtl();", true);
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
            try
            {
                switch (type)
                {
                    case ControlsEnum.DEFAULT:

                        break;
                    case ControlsEnum.TRXNO:
                        trxNo = BusinessLogic.CommonManagement.CommonBL.GetTrxNo(ApplicationType.CENQ, 0, 0, currentUser.PKUser);
                        break;
                    case ControlsEnum.ENQUIRY:
                        dsPageData = BusinessLogic.Sales.Enquiry.GetEnquiryDetails(CurrPK, 0);
                        break;
                    case ControlsEnum.CUSTOMERENQUIRY:
                        dsPageData = BusinessLogic.Sales.Enquiry.GetEnquiryDetails(0, custPK);
                        break;
                    case ControlsEnum.PRODUCTSELECTED:
                        dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerProduct(custprodPK, productPK, custPK, string.Empty, string.Empty, currentUser.SBUID, custprodPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(addressPK, custPK, addressActive == 2 ? 2 : 1, (int)CustomerAddressType.ShippingAddress).Tables[0];
                        break;
                    //case ControlsEnum.FROMPORT:
                    //    dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.FromPort, 1, currentUser.SBUID);
                    //    break;
                    case ControlsEnum.TRANSHIPMENT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.Transhipment, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.SHIPBY:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.ShipBy, 1, currentUser.SBUID);
                        break;
                    //case ControlsEnum.ORIGINOFGOODS:
                    //    dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.OriginOfGoods, 1, currentUser.SBUID);
                    //    break;
                    case ControlsEnum.DELIVERYTERMS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, deliveryTermPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, specialCausePK > 0 ? 2 : 1);
                        break;
                    //case ControlsEnum.BANKDETAILS:
                    //    dtPageData = BusinessLogic.Sales.Enquiry.GetBankDetails(0, 1, currentUser.SBUID);
                    //    break;
                    case ControlsEnum.USERCUSTOMER:
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtEnqDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            exchangeRate = string.IsNullOrEmpty(dsExchangeRate.Tables[0].Rows[0][0].ToString()) ? -1 :
                                Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            exchangeRate = -1;
                        }
                        break;
                    case ControlsEnum.CUSTOMERCURRENCY:
                        dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomer(custPK, string.Empty, currentUser.SBUID, 2);
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
                    case ControlsEnum.DEFAULT:
                        break;
                    case ControlsEnum.ENQUIRY:
                    case ControlsEnum.CUSTOMERENQUIRY:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SELECTEDITEM:
                        txtCustomer.Enabled = (!IsCustomerUser) && (EnqDtlList == null || EnqDtlList.Count == 0);
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        BindDropDownList(controlType);
                        txtShippingAddress.Text = string.Empty;
                        break;
                    //case ControlsEnum.FROMPORT:
                    //    BindDropDownList(controlType);
                    //    break;
                    case ControlsEnum.TRANSHIPMENT:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.SHIPBY:
                        BindDropDownList(controlType);
                        break;
                    //case ControlsEnum.ORIGINOFGOODS:
                    //    BindDropDownList(controlType);
                    //    break;
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
                    //case ControlsEnum.BANKDETAILS:
                    //    BindDropDownList(controlType);
                    //    break;
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
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;

            int result;
            string saveXml;
            int selectedItemPK;
            string action;
            //int wkStatus = 0;
            DropDownList ddlWkfAction;
            string xmlHeader;
            string addr;
            xmlHeader = string.Empty;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
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
                    else if (((DropDownList)sender).ID == "ddlCustAddress")
                    {
                        commonActions = ActionsEnum.ADDRESSSELECTED;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.CUSTOMERSELECTED:
                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);

                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                             && ddlCustAddress.Items.FindByValue(dtPageData.Rows[0]["CAD_PK"].ToString()) != null)
                            {
                                ddlCustAddress.SelectedValue = dtPageData.Rows[0]["CAD_PK"].ToString();
                                addr = string.Empty;
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
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                {
                                    addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                                }
                                txtShippingAddress.Text = addr;
                            }
                            else
                                txtShippingAddress.Text = string.Empty;
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                             && ddlSpecialCause.Items.FindByValue(dtPageData.Rows[0]["TCH_PK"].ToString()) != null)
                            {
                                ddlSpecialCause.SelectedValue = dtPageData.Rows[0]["TCH_PK"].ToString();
                                txtSpecialCause.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                            }
                            else
                                txtSpecialCause.Text = string.Empty;
                            GetFieldValues(ControlsEnum.CUSTOMERCURRENCY);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                hdfCurrency.Value = dsPageData.Tables[0].Rows[0]["CUR_PK"].ToString();
                                txtCurrency.Text = string.Format(Resources.ErpRes.NameCodeFormat, dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString()
                                    , dsPageData.Tables[0].Rows[0]["CUR_NAME"].ToString());
                                txtToPort.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_SHIP_TO_PORT"].ToString());
                            }
                            else
                            {
                                hdfCurrency.Value = string.Empty;
                                txtCurrency.Text = string.Empty;
                                txtToPort.Text = string.Empty;
                            }
                            GetFieldValues(ControlsEnum.CUSTOMERENQUIRY);
                            if (dsPageData != null && dsPageData.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsPageData.Tables[0].Rows)
                                {
                                    xmlHeader += dr[0].ToString();
                                }
                                if (!string.IsNullOrEmpty(xmlHeader))
                                    rfqHeadObj = CommonFunctions.XmlDeserialize<EnquiryHeader>(xmlHeader);
                            }
                            else
                                rfqHeadObj = null;
                            SetFieldValues(ControlsEnum.CUSTOMERENQUIRY);
                            txtToPort.Focus(); //txtEnqDate.Focus();//Commented for tab Order Correction For New Design
                        }
                        else
                        {
                            //txtCustomer.Focus(); //Commented for tab Order Correction For New Design
                            dtPageData = null;
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                        }
                        EnqDtlList = new List<BusinessObject.SaleOrder.EnquiryDetails>();
                        SetFieldValues(ControlsEnum.SELECTEDITEM);
                        ResetForm();
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
                    case ActionsEnum.ADDRESSSELECTED:
                        if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            addressPK = Convert.ToInt32(ddlCustAddress.SelectedValue);
                            addressActive = 2;
                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            addr = string.Empty;
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
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                            {
                                addr += string.IsNullOrEmpty(addr) ? HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) :
                                    string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString().Trim()) ?
                                    string.Empty : Environment.NewLine + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                            }
                            txtShippingAddress.Text = addr;
                        }
                        else
                            txtShippingAddress.Text = string.Empty;
                        break;
                    case ActionsEnum.PRODUCTSELECTED:
                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                        }
                        if (hdfBrand.Value != CommonConstants.SELECT_VALUE_ZERO && hdfBrand.Value != string.Empty)
                        {
                            custprodPK = Convert.ToInt32(hdfBrand.Value);
                            GetFieldValues(ControlsEnum.PRODUCTSELECTED);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                hdfBrandCode.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CIM_BRAND_CODE"].ToString());
                                hdfProduct.Value = dsPageData.Tables[0].Rows[0]["CIM_ITEM"].ToString();
                                txtProduct.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CIM_ITEM_TEXT"].ToString());
                                txtQty.Text = dsPageData.Tables[0].Rows[0]["CIM_LAST_ORDR_QTY"].ToString();
                                hdfUOM.Value = dsPageData.Tables[0].Rows[0]["CIM_UOM"].ToString();
                                hdfPackingSpec.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CIM_PACKING_SPEC"].ToString());
                                txtUOM.Text = dsPageData.Tables[0].Rows[0]["CIM_UOM_TEXT"].ToString();
                                //txtExpMinRate.Text = dsPageData.Tables[0].Rows[0]["CED_EXP_MIN_RATE"].ToString();
                                //txtExpMaxRate.Text = dsPageData.Tables[0].Rows[0]["CED_EXP_MAX_RATE"].ToString();
                                txtPacking.Text = dsPageData.Tables[0].Rows[0]["CIM_PACKING_SPEC_NAME"].ToString();
                                txtPacking.ToolTip = dsPageData.Tables[0].Rows[0]["CIM_PACKING_SPEC_NAME"].ToString();
                                txtPiecesPerCarton.Text = dsPageData.Tables[0].Rows[0]["APS_TOTAL_PCS"].ToString();
                                txtBrandQuantity.Text = dsPageData.Tables[0].Rows[0]["CIM_LAST_ORDR_QTY"].ToString();
                                txtBrandUOM.Text = dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_TEXT"].ToString();
                                hdfBrandUOMPK.Value = dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM"].ToString();
                                if (!dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_CONV"].Equals(DBNull.Value))
                                {
                                    hdfBrandUOMConvFactor.Value = dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_CONV"].ToString();
                                }
                                else
                                {
                                    hdfBrandUOMConvFactor.Value = "1";
                                }
                                txtExpMinRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_TEXT"].ToString();
                                txtExpMaxRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_TEXT"].ToString();
                            }
                        }
                        else if (hdfProduct.Value != CommonConstants.SELECT_VALUE_ZERO && hdfProduct.Value != string.Empty)
                        {
                            productPK = Convert.ToInt32(hdfProduct.Value);
                            GetFieldValues(ControlsEnum.PRODUCTSELECTED);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                hdfBrand.Value = dsPageData.Tables[0].Rows[0]["CIM_PK"].ToString();
                                txtBrand.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CIM_BRAND_NAME"].ToString());
                                hdfBrandCode.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CIM_BRAND_CODE"].ToString());
                                txtQty.Text = dsPageData.Tables[0].Rows[0]["CIM_LAST_ORDR_QTY"].ToString();
                                hdfUOM.Value = dsPageData.Tables[0].Rows[0]["CIM_UOM"].ToString();
                                hdfPackingSpec.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CIM_PACKING_SPEC"].ToString());
                                txtUOM.Text = dsPageData.Tables[0].Rows[0]["CIM_UOM_TEXT"].ToString();
                                //txtExpMinRate.Text = dsPageData.Tables[0].Rows[0]["CED_EXP_MIN_RATE"].ToString();
                                //txtExpMaxRate.Text = dsPageData.Tables[0].Rows[0]["CED_EXP_MAX_RATE"].ToString();
                                txtPacking.Text = dsPageData.Tables[0].Rows[0]["CIM_PACKING_SPEC_NAME"].ToString();
                                txtPacking.ToolTip = dsPageData.Tables[0].Rows[0]["CIM_PACKING_SPEC_NAME"].ToString();
                                txtPiecesPerCarton.Text = dsPageData.Tables[0].Rows[0]["APS_TOTAL_PCS"].ToString();
                                txtBrandQuantity.Text = dsPageData.Tables[0].Rows[0]["CIM_LAST_ORDR_QTY"].ToString();
                                txtBrandUOM.Text = dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_TEXT"].ToString();
                                hdfBrandUOMPK.Value = dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM"].ToString();
                                if (!dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_CONV"].Equals(DBNull.Value))
                                {
                                    hdfBrandUOMConvFactor.Value = dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_CONV"].ToString();
                                }
                                else
                                {
                                    hdfBrandUOMConvFactor.Value = "1";
                                }
                                txtExpMinRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_TEXT"].ToString();
                                txtExpMaxRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + dsPageData.Tables[0].Rows[0]["CIM_SALE_UOM_TEXT"].ToString();
                            }
                        }
                        break;
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (CurrSlNo != 0)
                            {
                                enqDtlObj = EnqDtlList.SingleOrDefault(itm => itm.CED_SL_NO == CurrSlNo);
                                if (enqDtlObj != null)
                                {
                                    enqDtlObj.CED_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                    enqDtlObj.CED_ENQ_QTY = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    enqDtlObj.CED_SALE_QTY = Math.Round(Convert.ToDouble(txtBrandQuantity.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);

                                    enqDtlObj.CED_REQUIRED_DATE = txtReqByDate.Text;
                                    enqDtlObj.CED_ITEM = Convert.ToInt32(hdfProduct.Value);
                                    enqDtlObj.CED_REMARKS = txtDtlRemark.Text;
                                    enqDtlObj.CED_UOM = Convert.ToInt32(hdfUOM.Value);
                                    if (hdfPackingSpec.Value.Trim() != string.Empty)
                                        enqDtlObj.CED_PACKING_SPEC = hdfPackingSpec.Value;
                                    enqDtlObj.CIM_BRAND_NAME = txtBrand.Text;
                                    enqDtlObj.CIM_BRAND_CODE = hdfBrandCode.Value;
                                    enqDtlObj.CIM_ITEM_TEXT = txtProduct.Text;
                                    enqDtlObj.CIM_UOM_TEXT = txtUOM.Text;
                                    if (!string.IsNullOrEmpty(txtExpMinRate.Text))
                                        enqDtlObj.CED_EXP_MIN_RATE = txtExpMinRate.Text;
                                    else
                                        enqDtlObj.CED_EXP_MIN_RATE = null;
                                    if (!string.IsNullOrEmpty(txtExpMaxRate.Text))
                                        enqDtlObj.CED_EXP_MAX_RATE = txtExpMaxRate.Text;
                                    else
                                        enqDtlObj.CED_EXP_MAX_RATE = null;
                                    enqDtlObj.CIM_PACKING_SPEC_NAME = txtPacking.Text;
                                    enqDtlObj.APS_TOTAL_PCS = String.IsNullOrEmpty(txtPiecesPerCarton.Text.Trim()) ? 0 : Convert.ToDouble(txtPiecesPerCarton.Text.Trim());
                                    enqDtlObj.CED_SALE_UOM_TEXT = txtBrandUOM.Text;
                                    enqDtlObj.CED_SALE_UOM = Convert.ToInt32(hdfBrandUOMPK.Value);
                                    if (!string.IsNullOrEmpty(hdfBrandUOMConvFactor.Value))
                                    {
                                        enqDtlObj.CED_SALE_UOM_CONV = Convert.ToDouble(hdfBrandUOMConvFactor.Value);
                                    }
                                    else
                                    {
                                        enqDtlObj.CED_SALE_UOM_CONV = 1;
                                    }

                                    //For getting CBM and Weight 
                                    double Qty = 0;
                                    int PackingSpecPK = 0;
                                    if (hdfPackingSpec.Value.Trim() != string.Empty)
                                        PackingSpecPK = Convert.ToInt32(hdfPackingSpec.Value);
                                    Qty = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    DataTable dtCBMWEIGHT = BusinessLogic.Sales.CustomerProduct.GetCBMWeight(Convert.ToInt32(hdfBrand.Value), PackingSpecPK, 0, Qty);
                                    if (dtCBMWEIGHT.Rows.Count > 0)
                                    {
                                        string CBMFourDec = Convert.ToDouble(dtCBMWEIGHT.Rows[0]["CBM"]).ToString("N4");
                                        string NETWTThreeDec = Convert.ToDouble(dtCBMWEIGHT.Rows[0]["NET_WT"]).ToString("N3");
                                        enqDtlObj.CBM = Convert.ToDouble(CBMFourDec);
                                        enqDtlObj.NET_WT = Convert.ToDouble(NETWTThreeDec);
                                    }
                                    //End
                                }
                            }
                            else
                            {
                                int slno = 1;
                                if (EnqDtlList == null || EnqDtlList.Count == 0)
                                {
                                    EnqDtlList = new List<BusinessObject.SaleOrder.EnquiryDetails>();
                                    slno = 1;
                                }
                                else
                                {
                                    slno = EnqDtlList.Max(itm => itm.CED_SL_NO);
                                    slno++;
                                }
                                if (EnqDtlList.SingleOrDefault(itm => itm.CED_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)) == null)
                                {
                                    BusinessObject.SaleOrder.EnquiryDetails enquiryDetlObj = new BusinessObject.SaleOrder.EnquiryDetails();
                                    enquiryDetlObj.CED_SL_NO = slno;
                                    enquiryDetlObj.CED_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                    enquiryDetlObj.CED_ENQ_QTY = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    enquiryDetlObj.CED_SALE_QTY = Math.Round(Convert.ToDouble(txtBrandQuantity.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    enquiryDetlObj.CED_REQUIRED_DATE = txtReqByDate.Text;
                                    enquiryDetlObj.CED_VERSION = 1;
                                    enquiryDetlObj.CED_ITEM = Convert.ToInt32(hdfProduct.Value);
                                    enquiryDetlObj.CED_PK = 0;
                                    enquiryDetlObj.CED_REMARKS = txtDtlRemark.Text;
                                    enquiryDetlObj.CED_UOM = Convert.ToInt32(hdfUOM.Value);
                                    if (hdfPackingSpec.Value.Trim() != string.Empty)
                                        enquiryDetlObj.CED_PACKING_SPEC = hdfPackingSpec.Value;
                                    enquiryDetlObj.CIM_BRAND_NAME = txtBrand.Text;
                                    enquiryDetlObj.CIM_BRAND_CODE = hdfBrandCode.Value;
                                    enquiryDetlObj.CIM_ITEM_TEXT = txtProduct.Text;
                                    enquiryDetlObj.CIM_UOM_TEXT = txtUOM.Text;
                                    if (!string.IsNullOrEmpty(txtExpMinRate.Text))
                                        enquiryDetlObj.CED_EXP_MIN_RATE = txtExpMinRate.Text;
                                    if (!string.IsNullOrEmpty(txtExpMaxRate.Text))
                                        enquiryDetlObj.CED_EXP_MAX_RATE = txtExpMaxRate.Text;
                                    enquiryDetlObj.CIM_PACKING_SPEC_NAME = txtPacking.Text;
                                    enquiryDetlObj.APS_TOTAL_PCS = String.IsNullOrEmpty(txtPiecesPerCarton.Text.Trim()) ? 0 : Convert.ToDouble(txtPiecesPerCarton.Text.Trim());
                                    enquiryDetlObj.CED_SALE_UOM_TEXT = txtBrandUOM.Text;
                                    enquiryDetlObj.CED_SALE_UOM = Convert.ToInt32(hdfBrandUOMPK.Value);
                                    if (!string.IsNullOrEmpty(hdfBrandUOMConvFactor.Value))
                                    {
                                        enquiryDetlObj.CED_SALE_UOM_CONV = Convert.ToDouble(hdfBrandUOMConvFactor.Value);
                                    }
                                    else
                                    {
                                        enquiryDetlObj.CED_SALE_UOM_CONV = 1;
                                    }

                                    //For getting CBM and Weight 
                                    double Qty = 0;
                                    int PackingSpecPK = 0;
                                    if (hdfPackingSpec.Value.Trim() != string.Empty)
                                        PackingSpecPK = Convert.ToInt32(hdfPackingSpec.Value);
                                    Qty = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                    DataTable dtCBMWEIGHT = BusinessLogic.Sales.CustomerProduct.GetCBMWeight(Convert.ToInt32(hdfBrand.Value), PackingSpecPK, 0, Qty);
                                    if (dtCBMWEIGHT.Rows.Count > 0)
                                    {
                                        string CBMFourDec = String.IsNullOrEmpty(dtCBMWEIGHT.Rows[0]["CBM"].ToString()) ? "0" : Convert.ToDouble(dtCBMWEIGHT.Rows[0]["CBM"]).ToString("N4");
                                        string NETWTThreeDec = String.IsNullOrEmpty(dtCBMWEIGHT.Rows[0]["NET_WT"].ToString()) ? "0" : Convert.ToDouble(dtCBMWEIGHT.Rows[0]["NET_WT"]).ToString("N3");
                                        enquiryDetlObj.CBM = Convert.ToDouble(CBMFourDec);
                                        enquiryDetlObj.NET_WT = Convert.ToDouble(NETWTThreeDec);
                                    }
                                    //End

                                    EnqDtlList.Add(enquiryDetlObj);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('"
                                        + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Same_Brand").ToString())
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                            }
                            SetFieldValues(ControlsEnum.SELECTEDITEM);
                            ResetForm();
                        }
                        break;
                    case ActionsEnum.SAVE:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (EnqDtlList == null || EnqDtlList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            rfqHeadObj = (EnquiryHeader)SetUIValuesToObject(commonActions);
                            if (rfqHeadObj != null)
                            {
                                saveXml = CommonFunctions.XmlSerialize<EnquiryHeader>(rfqHeadObj);
                                result = BusinessLogic.Sales.Enquiry.SaveEnquiryDetails(saveXml);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    //Show Save success message and reset Contract Entry
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
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
                                        litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (EnqDtlList == null || EnqDtlList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                rfqHeadObj = (EnquiryHeader)SetUIValuesToObject(commonActions);
                                if (rfqHeadObj != null)
                                {
                                    saveXml = CommonFunctions.XmlSerialize<EnquiryHeader>(rfqHeadObj);
                                    result = BusinessLogic.Sales.Enquiry.SaveEnquiryDetails(saveXml);
                                    if (result > 0) // Success ! re-initialize the page
                                    {
                                        //Workflow submission
                                        ucrWrkf.ApplicationID = result;
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        return;
                                    }
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.Enquiry.DeleteRFQDetails(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.Enquiry + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    case ActionsEnum.REMOVEITEM:
                        if (EnqDtlList != null && EnqDtlList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdEnquiryDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                EnqDtlList = EnqDtlList.Where(row => selectedItemPK != row.CED_SL_NO).ToList();
                                SetFieldValues(ControlsEnum.SELECTEDITEM);
                            }
                        }
                        ResetForm();
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Min_Items").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    case ActionsEnum.EDITITEM:
                        if (EnqDtlList != null && EnqDtlList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdEnquiryDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                enqDtlObj = EnqDtlList.SingleOrDefault(row => selectedItemPK == row.CED_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                            }
                        }
                        break;
                    case ActionsEnum.CLEARITEM:
                        ResetForm();
                        break;
                    case ActionsEnum.CANCEL:
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.EnquiryListing), false);
                        break;
                    case ActionsEnum.PRINT:
                        break;
                    case ActionsEnum.QUOTATION:
                        if (CurrPK > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK;
                            Session["EnquiryMode"] = EntryStatus;
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Quotation), false);

                            //int.TryParse(hdfStatus.Value, out wkStatus);
                            //if (wkStatus > (int)EnqWorkFlowStatus.EnquiryReviewedwithmoreInfo || wkStatus == (int)EnqWorkFlowStatus.EnquiryApproved)
                            //{
                            //    Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK;
                            //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Quotation), false);
                            //}
                            //else
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                            //           CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Enquiry_Approve").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                       CommonFunctions.FormatErrorMessage(GetLocalResourceObject("SelectEnquiry").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.ENQUIRY:

                        break;
                    case ActionsEnum.ENQUIRYLIST:
                        if (Session["QuotationToEnquiry"] != null && Session["QuotationToEnquiry"].ToString().Equals("1"))
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.QuotationListing), false);
                        }
                        else
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.EnquiryListing), false);
                        }
                        break;
                    case ActionsEnum.EXCHANGERATE:
                        if (hdfCurrency.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCurrency.Value != string.Empty)
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            if (exchangeRate > 0)
                            {
                                hdfExchangeRate.Value = exchangeRate.ToString();
                            }
                            else
                            {
                                txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                hdfCurrency.Value = "0";
                                hdfExchangeRate.Value = "1";
                                litErrorMsg.Text = GetLocalResourceObject("Err_ExchangeRate").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                            hdfCurrency.Value = "0";
                            hdfExchangeRate.Value = "1";
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
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
                Label lblItemCartonsOrBags;
                Label lblItemTotalQty;
                Label lblItemTotalCarton;
                Label lblTotalCBM;
                Label lblTotalWeight;
                BusinessObject.SaleOrder.EnquiryDetails enqItem;
                if ((sender as GridView).ID == "grdEnquiryDetails")
                {
                    if (EntryStatus != EntryStatus.ENTRYMODE && EntryStatus != EntryStatus.NEWMODE)
                    //if ((!string.IsNullOrEmpty(hdfStatus.Value)
                    //    && !IsEnquiryEdit(hdfStatus.Value)) || (EntryStatus != EntryStatus.ENTRYMODE && EntryStatus != EntryStatus.NEWMODE))
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.Footer)
                        {
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[8].Width = Unit.Percentage(19);
                        }
                        else if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                        {
                            e.Row.Cells[9].Visible = false;
                            e.Row.Cells[8].Width = Unit.Percentage(19);
                            (e.Row.FindControl("lblRemarksLst") as Label).Text = CommonFunctions.GetShortString((e.Row.DataItem as BusinessObject.SaleOrder.EnquiryDetails).CED_REMARKS, 18);
                        }
                    }

                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        lblItemCartonsOrBags = e.Row.FindControl("lblItemCartonsOrBags") as Label;
                        enqItem = (BusinessObject.SaleOrder.EnquiryDetails)e.Row.DataItem;
                        if (enqItem != null)
                        {
                            if (lblItemCartonsOrBags != null)
                            {
                                lblItemCartonsOrBags.Text = Math.Ceiling(enqItem.CED_ENQ_QTY / enqItem.APS_TOTAL_PCS).ToString();
                                lblItemCartonsOrBags.ToolTip = HttpUtility.HtmlDecode(enqItem.PACKING_TEXT);
                            }
                        }
                    }

                    if (e.Row.RowType == DataControlRowType.Footer && EnqDtlList != null)
                    {
                        lblItemTotalQty = e.Row.FindControl("lblItemTotalQty") as Label;
                        lblItemTotalCarton = e.Row.FindControl("lblItemTotalCarton") as Label;
                        lblTotalCBM = e.Row.FindControl("lblTotalCBM") as Label;
                        lblTotalWeight = e.Row.FindControl("lblTotalWeight") as Label;

                        if (lblItemTotalQty != null)
                        {
                            lblItemTotalQty.Text = lblItemTotalQty.ToolTip = string.Format("{0:c}", Convert.ToDecimal(GetFormattedNumber(EnqDtlList.Sum(itm => itm.CED_ENQ_QTY))));
                        }
                        if (lblItemTotalCarton != null)
                        {
                            lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = EnqDtlList.Sum(itm =>
                                 Math.Ceiling(itm.CED_ENQ_QTY / itm.APS_TOTAL_PCS)).ToString();
                        }

                        if (lblTotalCBM != null)
                        {
                            lblTotalCBM.Text = lblTotalCBM.ToolTip = (EnqDtlList.Sum(itm => itm.CBM)).ToString("N4");
                        }
                        if (lblTotalWeight != null)
                        {
                            lblTotalWeight.Text = lblTotalWeight.ToolTip = (EnqDtlList.Sum(itm => itm.NET_WT)).ToString("N3");
                        }

                    }


                    //(e.Row.FindControl("btnEditItem") as ImageButton).Visible = (string.IsNullOrEmpty(hdfStatus.Value)
                    //    || IsEnquiryEdit(hdfStatus.Value)) && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);
                    //(e.Row.FindControl("btnRemoveItem") as ImageButton).Visible = (string.IsNullOrEmpty(hdfStatus.Value)
                    //    || IsEnquiryEdit(hdfStatus.Value)) && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
        }
        #endregion

        #endregion
        #region Helper Methods
        //private bool IsEnquiryEdit(string status)
        //{
        //    EnqWorkFlowStatus stat;
        //    if (Enum.TryParse(status, out stat))
        //    {
        //        switch (stat)
        //        {
        //            case EnqWorkFlowStatus.EnquiryDrafted:
        //            case EnqWorkFlowStatus.EnquirySubmitted:
        //            case EnqWorkFlowStatus.EnquiryReviewed:
        //            case EnqWorkFlowStatus.EnquiryRequestedformoreInfo:
        //            case EnqWorkFlowStatus.EnquiryMoreInfoSubmitted:
        //            case EnqWorkFlowStatus.EnquiryRequestedtoreviewwithmoreInfo:
        //            case EnqWorkFlowStatus.EnquiryReviewedwithmoreInfo:
        //                return true;
        //                break;
        //            default:
        //                return false;
        //                break;
        //        }
        //    }
        //    else return false;
        //}
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        /// <param name="controlType">Controls to Bind</param>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region ENQUIRY
                    case ControlsEnum.ENQUIRY:
                        //assigning the UI controls with the corresponding Contract ListObject value
                        if (rfqHeadObj != null)
                        {
                            CurrPK = rfqHeadObj.CEH_PK;
                            hdfStatus.Value = rfqHeadObj.CEH_STATUS.ToString();
                            //if (rfqHeadObj.CEH_STATUS == (int)EnqWorkFlowStatus.EnquiryApproved
                            //    || rfqHeadObj.CEH_STATUS == (int)EnqWorkFlowStatus.EnquiryRejected || rfqHeadObj.CEH_STATUS > (int)EnqWorkFlowStatus.EnquiryReviewedwithmoreInfo)
                            //{
                            //    ucrWrkf.ViewType = 0;
                            //    EntryStatus = EntryStatus.VIEWMODE;
                            //    ucrWrkf.ViewAction();
                            //}
                            txtCustomer.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_CUSTOMER_NAME.ToString());
                            custPK = rfqHeadObj.CEH_CUSTOMER;
                            hdfCustomer.Value = rfqHeadObj.CEH_CUSTOMER.ToString();
                            lblEnqTrxNoTxt.Text = ((rfqHeadObj.CEH_NO == null || rfqHeadObj.CEH_NO == "") ? Resources.Messages.DocGenerationNew : rfqHeadObj.CEH_NO);
                            txtEnqDate.Text = rfqHeadObj.CEH_DATE;
                            hdfVersion.Value = rfqHeadObj.CEH_VERSION.ToString();

                            //GetFieldValues(ControlsEnum.FROMPORT);
                            //fromPortPK = rfqHeadObj.CEH_FROM_PORT;
                            //SetFieldValues(ControlsEnum.FROMPORT);
                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_TRANSHIPMENT))
                                transhipmentPK = Convert.ToInt32(rfqHeadObj.CEH_TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);
                            GetFieldValues(ControlsEnum.SHIPBY);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_SHIP_BY))
                                shipByPK = Convert.ToInt32(rfqHeadObj.CEH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);
                            //GetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            //originOfGoodsPK = rfqHeadObj.CEH_ORG_GOODS;
                            //SetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            //GetFieldValues(ControlsEnum.BANKDETAILS);
                            //bankDetailPK = rfqHeadObj.CEH_BANK;
                            //SetFieldValues(ControlsEnum.BANKDETAILS);

                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_SHIPPING_TO))
                                addressPK = Convert.ToInt32(rfqHeadObj.CEH_SHIPPING_TO);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_DEL_TERM))
                                deliveryTermPK = Convert.ToInt32(rfqHeadObj.CEH_DEL_TERM);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_PAYMENT_TERM))
                                paymentTermPK = Convert.ToInt32(rfqHeadObj.CEH_PAYMENT_TERM);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_SPECIAL_TERM))
                                specialCausePK = Convert.ToInt32(rfqHeadObj.CEH_SPECIAL_TERM);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);

                            txtToPort.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_TO_PORT);

                            hdfCurrency.Value = rfqHeadObj.CEH_CURRENCY.ToString();
                            txtCurrency.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_CURRENCY_TEXT);

                            txtShippingAddress.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_SHIPPING_ADDRESS);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_DEL_TERM_TEXT);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_PAYMENT_TERMS);
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_SPECIAL_TERM_TEXT);
                            txtRemarks.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_REMARKS);
                            EnqDtlList = rfqHeadObj.ProductDtl;
                            SetFieldValues(ControlsEnum.SELECTEDITEM);
                            ModifiedDatePnl.Visible = true;
                            LastModifiedTime = rfqHeadObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        }
                        else
                        {
                            //Show Concurrency and bind Listing if Query yield no results
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Error_Concurrent;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Enquiry);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                             + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                        }
                        break;
                    #endregion
                    #region CUSTOMERENQUIRY
                    case ControlsEnum.CUSTOMERENQUIRY:
                        if (rfqHeadObj != null)
                        {
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_TRANSHIPMENT)
                                && ddlTranshipment.Items.FindByValue(rfqHeadObj.CEH_TRANSHIPMENT) != null)
                                ddlTranshipment.SelectedValue = rfqHeadObj.CEH_TRANSHIPMENT;
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_SHIPPING_TO)
                                && ddlCustAddress.Items.FindByValue(rfqHeadObj.CEH_SHIPPING_TO) != null)
                                ddlCustAddress.SelectedValue = rfqHeadObj.CEH_SHIPPING_TO;
                            txtShippingAddress.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_SHIPPING_ADDRESS);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_PAYMENT_TERM)
                                && ddlPaymentTerms.Items.FindByValue(rfqHeadObj.CEH_PAYMENT_TERM) != null)
                                ddlPaymentTerms.SelectedValue = rfqHeadObj.CEH_PAYMENT_TERM;
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_PAYMENT_TERMS);
                            if (!string.IsNullOrEmpty(rfqHeadObj.CEH_DEL_TERM)
                                && ddlDeliveryTerms.Items.FindByValue(rfqHeadObj.CEH_DEL_TERM) != null)
                                ddlDeliveryTerms.SelectedValue = rfqHeadObj.CEH_DEL_TERM;
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_DEL_TERM_TEXT);
                            txtRemarks.Text = HttpUtility.HtmlDecode(rfqHeadObj.CEH_REMARKS);
                        }
                        break;
                    #endregion
                    case ControlsEnum.SELECTEDITEM:
                        if (enqDtlObj != null)
                        {
                            CurrSlNo = enqDtlObj.CED_SL_NO;
                            hdfBrand.Value = enqDtlObj.CED_CUST_ITEM.ToString();
                            txtBrand.Text = enqDtlObj.CIM_BRAND_NAME;
                            hdfBrandCode.Value = enqDtlObj.CIM_BRAND_CODE;
                            hdfProduct.Value = enqDtlObj.CED_ITEM.ToString();
                            txtProduct.Text = HttpUtility.HtmlDecode(enqDtlObj.CIM_ITEM_TEXT);
                            txtQty.Text = Math.Round(enqDtlObj.CED_ENQ_QTY, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            txtBrandQuantity.Text = Math.Round(enqDtlObj.CED_SALE_QTY, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            hdfUOM.Value = enqDtlObj.CED_UOM.ToString();
                            hdfPackingSpec.Value = enqDtlObj.CED_PACKING_SPEC.ToString();
                            txtUOM.Text = HttpUtility.HtmlDecode(enqDtlObj.CIM_UOM_TEXT);
                            if (!string.IsNullOrEmpty(enqDtlObj.CED_EXP_MIN_RATE))
                                txtExpMinRate.Text = Math.Round(Convert.ToDouble(enqDtlObj.CED_EXP_MIN_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            if (!string.IsNullOrEmpty(enqDtlObj.CED_EXP_MAX_RATE))
                                txtExpMaxRate.Text = Math.Round(Convert.ToDouble(enqDtlObj.CED_EXP_MAX_RATE), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            txtPacking.Text = enqDtlObj.CIM_PACKING_SPEC_NAME.ToString();
                            txtPacking.ToolTip = enqDtlObj.CIM_PACKING_SPEC_NAME.ToString();
                            txtPiecesPerCarton.Text = enqDtlObj.APS_TOTAL_PCS.ToString();
                            txtReqByDate.Text = enqDtlObj.CED_REQUIRED_DATE;
                            txtDtlRemark.Text = HttpUtility.HtmlDecode(enqDtlObj.CED_REMARKS);
                            txtBrandUOM.Text = HttpUtility.HtmlDecode(enqDtlObj.CED_SALE_UOM_TEXT);
                            hdfBrandUOMPK.Value = enqDtlObj.CED_SALE_UOM.ToString();
                            hdfBrandUOMConvFactor.Value = enqDtlObj.CED_SALE_UOM_CONV.ToString();
                            txtExpMinRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + HttpUtility.HtmlDecode(enqDtlObj.CED_SALE_UOM_TEXT);
                            txtExpMaxRateUOM.Text = GetLocalResourceObject("Per").ToString() + " " + HttpUtility.HtmlDecode(enqDtlObj.CED_SALE_UOM_TEXT);
                        }
                        break;
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <param name="mode">Save Action</param>
        /// <returns>Object to Save</returns>
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            EnquiryDetails selectedItem;
            try
            {
                switch (mode)
                {
                    #region Save
                    case ActionsEnum.SAVE:
                    case ActionsEnum.WRKFSUBMIT:
                        rfqHeadObj = new EnquiryHeader();
                        rfqHeadObj.CEH_PK = CurrPK;
                        rfqHeadObj.CEH_CUSTOMER = Convert.ToInt32(hdfCustomer.Value);
                        rfqHeadObj.CEH_CUSTOMER_NAME = HttpUtility.HtmlEncode(txtCustomer.Text);
                        rfqHeadObj.CEH_NO = lblEnqTrxNoTxt.Text;
                        rfqHeadObj.CEH_DATE = txtEnqDate.Text;

                        //if (ddlFromPort.SelectedValue != CommonConstants.SELECTVAL)
                        //    rfqHeadObj.CEH_FROM_PORT = Convert.ToInt32(ddlFromPort.SelectedValue);
                        if (ddlTranshipment.SelectedValue != CommonConstants.SELECTVAL)
                            rfqHeadObj.CEH_TRANSHIPMENT = ddlTranshipment.SelectedValue;
                        if (ddlShipBy.SelectedValue != CommonConstants.SELECTVAL)
                            rfqHeadObj.CEH_SHIP_BY = ddlShipBy.SelectedValue;
                        //if (ddlOriginofGoods.SelectedValue != CommonConstants.SELECTVAL)
                        //    rfqHeadObj.CEH_ORG_GOODS = Convert.ToInt32(ddlOriginofGoods.SelectedValue);
                        //if (ddlBankDetails.SelectedValue != CommonConstants.SELECTVAL)
                        //    rfqHeadObj.CEH_BANK = Convert.ToInt32(ddlBankDetails.SelectedValue);
                        rfqHeadObj.CEH_SHIPPING_ADDRESS = HttpUtility.HtmlEncode(txtShippingAddress.Text);
                        if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                            rfqHeadObj.CEH_SHIPPING_TO = ddlCustAddress.SelectedValue;
                        rfqHeadObj.CEH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                        if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                            rfqHeadObj.CEH_DEL_TERM = ddlDeliveryTerms.SelectedValue;
                        rfqHeadObj.CEH_PAYMENT_TERMS = HttpUtility.HtmlEncode(txtPaymentTerms.Text);
                        if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                            rfqHeadObj.CEH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                        rfqHeadObj.CEH_SPECIAL_TERM_TEXT = HttpUtility.HtmlEncode(txtSpecialCause.Text);
                        if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                            rfqHeadObj.CEH_SPECIAL_TERM = ddlSpecialCause.SelectedValue;
                        rfqHeadObj.CEH_TO_PORT = HttpUtility.HtmlEncode(txtToPort.Text);

                        rfqHeadObj.CEH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);
                        rfqHeadObj.CEH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        rfqHeadObj.CEH_CURRENCY_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);

                        rfqHeadObj.CEH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        rfqHeadObj.ProductDtl = EnqDtlList;
                        rfqHeadObj.CEH_VERSION = Convert.ToInt32(hdfVersion.Value);
                        rfqHeadObj.BIZUNIT_PK = currentUser.SBUID;
                        rfqHeadObj.CEH_STATUS = (byte)(string.IsNullOrEmpty(hdfStatus.Value) ? 0 : Convert.ToByte(hdfStatus.Value));
                        rfqHeadObj.CEH_DEPT = currentUser.CurrentDeptPK;
                        rfqHeadObj.USER_PK = currentUser.PKUser;
                        rfqHeadObj.LAST_MOD_DT = LastModifiedTime;
                        rfqHeadObj.ACTIVE = 1;

                        rfqHeadObj.APT_CODE = ApplicationType.CENQ;
                        rfqHeadObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                        rfqHeadObj.WKF_FLAG = 0;
                        if (mode == ActionsEnum.SAVE)
                        {
                            rfqHeadObj.WKF_FLAG = 0;
                        }
                        else if (mode == ActionsEnum.WRKFSUBMIT)
                        {
                            rfqHeadObj.WKF_FLAG = 1;
                        }

                        returnObj = rfqHeadObj;
                        break;
                    #endregion
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {

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
                        break;
                    case ControlsEnum.SELECTEDITEM:
                        grdEnquiryDetails.DataSource = EnqDtlList;
                        grdEnquiryDetails.DataBind();
                        divDetails.Visible = EnqDtlList != null && EnqDtlList.Count > 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.CUSTOMERADDRESS:
                        ddlCustAddress.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlCustAddress.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CAD_NAME");
                            ddlCustAddress.DataTextField = "CAD_NAME";
                            ddlCustAddress.DataValueField = "CAD_PK";
                            ddlCustAddress.DataBind();
                        }
                        ddlCustAddress.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (addressPK > 0 && ddlCustAddress.Items.FindByValue(addressPK.ToString()) != null)
                            ddlCustAddress.SelectedValue = addressPK.ToString();
                        break;
                    //case ControlsEnum.FROMPORT:
                    //    ddlFromPort.Items.Clear();
                    //    if (dtPageData != null)
                    //    {
                    //        ddlFromPort.DataSource = dtPageData;
                    //        ddlFromPort.DataTextField = "CON_NAME";
                    //        ddlFromPort.DataValueField = "CON_PK";
                    //        ddlFromPort.DataBind();
                    //    }
                    //    ddlFromPort.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //    if (fromPortPK > 0)
                    //        ddlFromPort.SelectedValue = fromPortPK.ToString();
                    //    break;
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
                        if (transhipmentPK > 0 && ddlTranshipment.Items.FindByValue(transhipmentPK.ToString()) != null)
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
                        if (shipByPK > 0 && ddlShipBy.Items.FindByValue(shipByPK.ToString()) != null)
                            ddlShipBy.SelectedValue = shipByPK.ToString();
                        break;
                    //case ControlsEnum.ORIGINOFGOODS:
                    //    ddlOriginofGoods.Items.Clear();
                    //    if (dtPageData != null)
                    //    {
                    //        ddlOriginofGoods.DataSource = dtPageData;
                    //        ddlOriginofGoods.DataTextField = "CON_NAME";
                    //        ddlOriginofGoods.DataValueField = "CON_PK";
                    //        ddlOriginofGoods.DataBind();
                    //    }
                    //    ddlOriginofGoods.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //    if (originOfGoodsPK > 0)
                    //        ddlOriginofGoods.SelectedValue = originOfGoodsPK.ToString();
                    //    break;
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
                        if (deliveryTermPK > 0 && ddlDeliveryTerms.Items.FindByValue(deliveryTermPK.ToString()) != null)
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
                        if (paymentTermPK > 0 && ddlPaymentTerms.Items.FindByValue(paymentTermPK.ToString()) != null)
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
                        if (specialCausePK > 0 && ddlSpecialCause.Items.FindByValue(specialCausePK.ToString()) != null)
                            ddlSpecialCause.SelectedValue = specialCausePK.ToString();
                        break;
                    //case ControlsEnum.BANKDETAILS:
                    //    ddlBankDetails.Items.Clear();
                    //    if (dtPageData != null)
                    //    {
                    //        ddlBankDetails.DataSource = dtPageData;
                    //        ddlBankDetails.DataTextField = "CBM_NAME";
                    //        ddlBankDetails.DataValueField = "CBM_PK";
                    //        ddlBankDetails.DataBind();
                    //    }
                    //    ddlBankDetails.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //    if (bankDetailPK > 0)
                    //        ddlBankDetails.SelectedValue = bankDetailPK.ToString();
                    //    break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            commonServiceObj = new CommonService();
            appTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.CENQ, 0, DateTime.Now);
            if (appTypeDetailsList.Count > 0)
            {
                return appTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        private void ResetForm()
        {
            CurrSlNo = 0;
            hdfBrand.Value = string.Empty;
            hdfBrandCode.Value = string.Empty;
            txtBrand.Text = string.Empty;
            hdfProduct.Value = string.Empty;
            txtProduct.Text = string.Empty;
            txtQty.Text = string.Empty;
            hdfUOM.Value = string.Empty;
            hdfPackingSpec.Value = string.Empty;
            txtUOM.Text = string.Empty;
            txtExpMaxRate.Text = string.Empty;
            txtExpMinRate.Text = string.Empty;
            txtPacking.Text = string.Empty;
            hdfPackingPK.Value = string.Empty;
            txtPiecesPerCarton.Text = string.Empty;
            txtDtlRemark.Text = string.Empty;
            //txtReqByDate.Text = string.Empty;
            txtBrandQuantity.Text = string.Empty;
            txtBrandUOM.Text = string.Empty;
            hdfBrandUOMPK.Value = string.Empty;
            hdfBrandUOMConvFactor.Value = string.Empty;
            txtExpMinRateUOM.Text = string.Empty;
            txtExpMaxRateUOM.Text = string.Empty;
        }

        public string GetFormattedNumber(object number)
        {
            string format = "#0.";
            double num = 0;
            for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
            {
                format += "0";
            }
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(format);
        }
        public string GetFormattedCurrency(object number)
        {
            string format = "#0.";
            double num = 0;
            for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
            {
                format += "0";
            }
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(format);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
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
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                //((HiddenField)this.Master.FindControl("hdfPageID")).Value = dtProcess.Rows[0][CommonConstants.F_PAGE].ToString();
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
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAddItem.PreRender += new EventHandler(btnAction_PreRender);
            this.btnClearItem.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnList.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnEnquiry.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnQuotation.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnAddItem.Load += new EventHandler(btnAction_Load);
            this.btnClearItem.Load += new EventHandler(btnAction_Load);
            this.lbnList.Load += new EventHandler(btnAction_Load);
            this.lbnEnquiry.Load += new EventHandler(btnAction_Load);
            this.lbnQuotation.Load += new EventHandler(btnAction_Load);
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
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {

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
                //if (ucrWrkf.RefID > 0)
                //    pnlDelete.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
            ENQUIRY,
            TRXNO,
            SELECTEDITEM,
            PRODUCTSELECTED,
            CUSTOMERADDRESS,
            CUSTOMERCURRENCY,
            //FROMPORT,
            TRANSHIPMENT,
            SHIPBY,
            //ORIGINOFGOODS,
            DELIVERYTERMS,
            PAYMENTTERMS,
            SPECIALCAUSE,
            //BANKDETAILS
            USERCUSTOMER,
            EXCHANGERATE,
            CUSTOMERENQUIRY,
            GETCBMWEIGHT
        }
        #endregion
    }
}





















