using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject;
using BusinessObject.Common;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.SaleOrder;
using System.Data;
using BusinessObject.CommonManagement;
using System.Text;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using ERPData;
using ERPService;
using System.Threading;

namespace ERPSMS_v01.Sales
{
    public partial class Quotation : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// To Disable Item Tax
        /// </summary>
        private bool EnableItemTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemTax] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.DisableItemTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemTax] = value;
            }
        }

        /// <summary>
        /// To set custom tax config value
        /// </summary>
        private bool IsCustomTaxEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomTaxEnabled] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomTaxEnabled]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomTaxEnabled] = value;
            }
        }
        /// <summary>
        /// To Disable Item Discount
        /// </summary>
        private bool EnableItemDiscount
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemDiscount] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.DisableItemDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemDiscount] = value;
            }
        }
        /// <summary>
        /// Qtn Status
        /// </summary>
        private int QtnStatus
        {
            get
            {
                return Convert.ToInt32(this.ViewState["QtnStatus"]);
            }
            set
            {
                this.ViewState["QtnStatus"] = value;
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
        /// Tax PK
        /// </summary>
        private int TaxPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TaxPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxPK] = value;
            }
        }
        /// <summary>
        /// Vendor PK
        /// </summary>
        private int VendorPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.VendorPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorPK] = value;
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

        ///// <summary>
        ///// To maintain keep QUOTATION Tax Splitting
        ///// </summary>
        //private List<RFQTaxHdr> RFQTaxDtlSplitSession
        //{
        //    get
        //    {
        //        return (List<RFQTaxHdr>)Session[ERP.Utilities.SessionStrings.RFQTaxDtlSplitSession + SelectedResponsePK.ToString()];
        //    }
        //    set
        //    {
        //        Session[ERP.Utilities.SessionStrings.RFQTaxDtlSplitSession + SelectedResponsePK.ToString()] = value;
        //    }
        //}

        ///// <summary>
        ///// To maintain keep QUOTATION Tax Splitting
        ///// </summary>
        //private List<RFQTaxHdr> RFQTaxHdrSplitSession
        //{
        //    get
        //    {
        //        return (List<RFQTaxHdr>)Session[ERP.Utilities.SessionStrings.RFQTaxHdrSplitSession];
        //    }
        //    set
        //    {
        //        Session[ERP.Utilities.SessionStrings.RFQTaxHdrSplitSession] = value;
        //    }
        //}

        /// <summary>
        /// To maintain keep QUOTATION Tax Splitting
        /// </summary>
        private QuotationHeader QuotationHeaderSession
        {
            get
            {
                return (QuotationHeader)Session[ERP.Utilities.SessionStrings.QuotationHeader];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.QuotationHeader] = value;
            }
        }

        /// <summary>
        /// To maintain keep QUOTATION Tax Splitting
        /// </summary>
        private QuotationHeader TempQuotationHeaderSession
        {
            get
            {
                return (QuotationHeader)this.ViewState[ViewstateStrings.TempQuotationHeader];
            }
            set
            {
                this.ViewState[ViewstateStrings.TempQuotationHeader] = value;
            }
        }

        /// <summary>
        /// Response PK
        /// </summary>
        private int SelectedQuotationPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedQuotationPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedQuotationPK] = value;
            }
        }

        private int SelectedCusItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedCusItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCusItemPK] = value;
            }
        }

        private int SelectedItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedItemPK] = value;
            }
        }
        private bool IsHeaderTax
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTax] = value;
            }
        }

        private bool IsEditMode
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEditMode] = value;
            }
        }

        private string SelectedTaxText
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SelectedTaxText];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTaxText] = value;
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
        //page related Entity Object

        private QuotationHeader quotationHeaderObj;
        private QuotationDetails quotationDetailsObj;
        private QuotationTaxHdr quotationTaxHdrObj;
        //private RFQTaxDtl rfqTaxDtlObj;
        List<QuotationDetails> quotationDetailsList;
        //List<RFQTaxDtl> rfqTaxDtlList;
        //RFQTaxSplit rfqDtlSplitObj;
        List<QuotationTaxHdr> quotationTaxHdrList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService commonServiceObj;
        QuotationDetails quotationResponseDtlObj;
        string selectedVendor;
        DataSet dsQuotationHeader;
        DataTable dtQuotationTaxDetails;
        DataSet dsVendor;
        bool hasValidRate;
        private int selectedItem;

        DataTable dtPageData;
        private DataTable dtCustomTaxSet;
        private int custPK;

        private int addressPK;
        private int fromPortPK;
        private int transhipmentPK;
        private int shipByPK;
        private int originOfGoodsPK;

        private int deliveryTermPK;
        private int paymentTermPK;
        private int specialCausePK;
        private int bankDetailPK;

        private string refID;
        private string inboxFlag;
        private BusinessObject.User currentUser;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
		(a1, a2) => a1 - a2,
		(a1, a2) => a1 + a2,
		(a1, a2) => a1 / a2,
		(a1, a2) => a1 * a2,
		(a1, a2) => Math.Pow(a1, a2)
	};

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
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    hdfAppType.Value = BusinessObject.CommonManagement.ApplicationType.CQTN;
                    hdfAppSubType.Value = string.Empty;

                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    QuotationHeaderSession = null;

                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);

                    GetFieldValues(ControlsEnum.TAXSETTINGS);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPageData.Rows)
                        {
                            if (row["ACF_DATA"].ToString().Equals("DISCOUNT"))
                            {
                                if (row["ACF_VALUE"].ToString().Equals("0"))
                                {
                                    EnableItemDiscount = false;
                                }
                            }
                            else if (row["ACF_DATA"].ToString().Equals("TAX"))
                            {
                                if (row["ACF_VALUE"].ToString().Equals("0"))
                                {
                                    EnableItemTax = false;
                                }
                            }
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
                        CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ENQUIRYPK]);
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
                        AST_DOC_MODE.Value = "0";
                        GetFieldValues(ControlsEnum.QUOTATION);
                        SetFieldValues(ControlsEnum.QUOTATIONHEADER);
                        SetFieldValues(ControlsEnum.QUOTATIONDETAIL);
                        if (grdQuotation.Rows.Count > 0)
                        {
                            SetSubTotal();
                        }
                        //GetFieldValues(ControlsEnum.CUSTOMERSELECTED);
                        if (lblQuotationNo.Text.Trim().Equals(string.Empty) || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                        {
                            AST_DOC_MODE.Value = GetDOCMODE();
                            //pnlSaleOrder.Visible = false;
                            //pnlPrint.Visible = false;
                        }
                        //hdfAppType.Value = ApplicationType.CQTN;
                        //hdfAppSubType.Value = string.Empty;  
                        btnRevision.Visible = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(lblQuotationNo.Text)
                            || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.EnquiryListing), false);
                        }
                        else
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.QuotationListing), false);
                        }
                        btnRevision.Visible = false;
                    }
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = null;
                    Session["EnquiryMode"] = null;
                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                    Session["QuotationToEnquiry"] = null;
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
            DataSet dsQuotationTaxDetails;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.QUOTATIONHEADER:
                        //dsQuotationHeader = BusinessLogic.Sales.QuotationBL.GetQuotationHeader(CurrPK);
                        break;
                    ////case ControlsEnum.VENDOR:
                    ////    dsVendor = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetVendorList(CurrPK);
                    ////    break;
                    case ControlsEnum.QUOTATION:
                        ////VendorPK = Convert.ToInt32(ddlVendor.SelectedValue);
                        //quotationHeaderObj = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQResponse(CurrPK, VendorPK);
                        //QuotationHeaderSession = quotationHeaderObj;
                        //if (quotationHeaderObj == null && Convert.ToInt32(CurrPK) != 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "','Quotation.aspx');", true);
                        //}
                        //////
                        quotationHeaderObj = BusinessLogic.Sales.QuotationBL.GetQuotation(CurrPK);
                        QuotationHeaderSession = quotationHeaderObj;
                        if (quotationHeaderObj == null && Convert.ToInt32(CurrPK) != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "','Quotation.aspx');", true);
                        }
                        break;
                    case ControlsEnum.QUOTATIONTAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsQuotationTaxDetails = BusinessLogic.Sales.QuotationBL.GetQuotationTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK));
                            if (dsQuotationTaxDetails != null && dsQuotationTaxDetails.Tables.Count > 0)
                            {
                                dtQuotationTaxDetails = dsQuotationTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Tax == category)
                            {
                                dtQuotationTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtQuotationDate.Text), 0, TaxFilterType.SAL, 1, 0, 1);
                            }
                            else
                            {
                                dtQuotationTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtQuotationDate.Text), 0);
                            }
                        }
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        if (hdfCurrency.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCurrency.Value != string.Empty)
                        {
                            DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtQuotationDate.Text.Trim()));
                            if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                            {
                                hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                            }
                            else
                            {
                                hdfExchangeRate.Value = "1";
                                txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                hdfCurrency.Value = "0";
                                litErrorMsg.Text = GetLocalResourceObject("Err_ExchangeRate").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            hdfExchangeRate.Value = "1";
                            txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                            hdfCurrency.Value = "0";
                        }
                        break;
                    case ControlsEnum.CUSTOMERSELECTED:
                        if (!string.IsNullOrEmpty(hdfCustomer.Value))
                        {
                            dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(addressPK, Convert.ToInt32(hdfCustomer.Value), addressPK > 0 ? 2 : 1);
                        }
                        break;
                    case ControlsEnum.ITEMDETAILS:
                        dsPageData = BusinessLogic.Sales.QuotationBL.GetItemDetails(selectedItem);
                        break;
                    case ControlsEnum.ITEMRATES:
                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetItemRates(selectedItem, 0);
                        dsPageData = BusinessLogic.Sales.QuotationBL.GetItemRates(selectedItem);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        dsPageData = BusinessLogic.Sales.QuotationBL.GetQuoationRevisionHistory(CurrPK);
                        break;

                    case ControlsEnum.CUSTOMERADDRESS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(addressPK, custPK, addressPK > 0 ? 2 : 1, (int)CustomerAddressType.ShippingAddress).Tables[0];
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
                    case ControlsEnum.TAXSETTINGS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break;
                    case ControlsEnum.CUSTOMTAXSETTINGS:
                        IsCustomTaxEnabled = true;
                        dtCustomTaxSet = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CUSTOM TAX SETTINGS", "TAX REQUIRED");
                        if (dtCustomTaxSet != null && dtCustomTaxSet.Rows.Count > 0)
                        {
                            int cfgval = Convert.ToInt32(dtCustomTaxSet.Rows[0]["ACF_VALUE"]);
                            if (cfgval == 0)
                            {
                                IsCustomTaxEnabled = false;
                            }
                        }
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
                    case ControlsEnum.VENDOR:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.QUOTATIONHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.QUOTATIONDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.QUOTATIONHEADERTOP:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.QUOTATIONTAXTYPES:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.QUOTATIONTAXPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.CUSTOMERSELECTED:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.ITEMRATES:
                        BindGrid(controlType);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        BindGrid(controlType);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;

                    case ControlsEnum.CUSTOMERADDRESS:
                        BindDropDown(controlType);
                        txtShippingAddress.Text = string.Empty;
                        break;
                    case ControlsEnum.TRANSHIPMENT:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.SHIPBY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.DELIVERYTERMS:
                        BindDropDown(controlType);
                        txtDeliveryTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        BindDropDown(controlType);
                        txtPaymentTerms.Text = string.Empty;
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        BindDropDown(controlType);
                        txtSpecialCause.Text = string.Empty;
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
            int rowID;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            HiddenField hdfCEDPK;
            HiddenField hdfItemPK;
            HiddenField hdfCusItemPK;
            HiddenField hdfUoM;
            //HiddenField hdfCurrency;
            HiddenField hdfQuotationDtlPK;
            TextBox txtQuantity;
            TextBox txtBrandQuantity;
            TextBox txtRate;
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;
            TextBox txtSubTotal;
            TextBox txtValidFrom;
            TextBox txtValidTo;
            TextBox txtComments;
            Label lblSpecifications;
            HiddenField hdfBrandUOMPK;
            HiddenField hdfBrandUOMConvFactor;
            List<QuotationTaxHdr> quotationTaxHeaderList;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.QUOTATIONHEADER:
                        quotationHeaderObj.CEH_PK = string.IsNullOrEmpty(hdfQuotationPK.Value) ? 0 : Convert.ToInt32(hdfQuotationPK.Value);

                        quotationHeaderObj.CEH_NO = string.IsNullOrEmpty(lblQuotationNo.Text.Trim()) || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew
                        ? string.Empty : lblQuotationNo.Text.Trim();
                        quotationHeaderObj.CEH_CUSTOMER = Convert.ToInt32(hdfCustomer.Value);
                        quotationHeaderObj.CEH_CUSTOMER_NAME = HttpUtility.HtmlEncode(txtCustomer.Text);
                        quotationHeaderObj.CEH_VERSION = 1;
                        quotationHeaderObj.CEH_DATE = string.IsNullOrEmpty(txtQuotationDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtQuotationDate.Text.Trim();
                        quotationHeaderObj.CEH_PK = CurrPK;
                        ////quotationHeaderObj.RRH_VENDOR = Convert.ToInt32(ddlVendor.SelectedValue);
                        ////quotationHeaderObj.RRH_VEN_REF_NO = HttpUtility.HtmlEncode(txtVendorRef.Text.Trim());
                        quotationHeaderObj.CEH_STATUS = 0;
                        quotationHeaderObj.CEH_TOTAL_DISCOUNT = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                        quotationHeaderObj.CEH_TOTAL_TAX = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                        quotationHeaderObj.CEH_TOTAL_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                        quotationHeaderObj.CEH_TOTAL_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                        quotationHeaderObj.CEH_NET_AMOUNT = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                        quotationHeaderObj.CEH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        quotationHeaderObj.CEH_CURRENCY_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                        quotationHeaderObj.CEH_NET_AMOUNT_BC = quotationHeaderObj.CEH_NET_AMOUNT * quotationHeaderObj.CEH_CURRENCY_RATE;

                        //if (!String.IsNullOrEmpty(hdfCustAddress.Value))
                        //    quotationHeaderObj.CEH_SHIPPING_TO = hdfCustAddress.Value;
                        //if (!String.IsNullOrEmpty(txtShippingAddress.Text.Trim()))
                        //    quotationHeaderObj.CEH_SHIPPING_ADDRESS = HttpUtility.HtmlEncode(txtShippingAddress.Text.Trim());

                        //if (!String.IsNullOrEmpty(hdfDeliveryTerms.Value))
                        //    quotationHeaderObj.CEH_DEL_TERM = hdfDeliveryTerms.Value;
                        //if (!String.IsNullOrEmpty(txtDeliveryTerms.Text.Trim()))
                        //    quotationHeaderObj.CEH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text.Trim());

                        //if (!String.IsNullOrEmpty(hdfPaymentTerms.Value))
                        //    quotationHeaderObj.CEH_PAYMENT_TERM = hdfPaymentTerms.Value;
                        //if (!String.IsNullOrEmpty(txtPaymentTerms.Text.Trim()))
                        //    quotationHeaderObj.CEH_PAYMENT_TERMS = HttpUtility.HtmlEncode(txtPaymentTerms.Text.Trim());

                        //if (!String.IsNullOrEmpty(hdfSpecialCause.Value))
                        //    quotationHeaderObj.CEH_SPECIAL_TERM = hdfSpecialCause.Value;
                        //if (!String.IsNullOrEmpty(txtSpecialCause.Text.Trim()))
                        //    quotationHeaderObj.CEH_SPECIAL_TERM_TEXT = HttpUtility.HtmlEncode(txtSpecialCause.Text.Trim());

                        //if (!String.IsNullOrEmpty(hdfShipBy.Value))
                        //    quotationHeaderObj.CEH_SHIP_BY = hdfShipBy.Value;
                        //if (!String.IsNullOrEmpty(txtToPort.Text.Trim()))
                        //    quotationHeaderObj.CEH_TO_PORT = HttpUtility.HtmlEncode(txtToPort.Text.Trim());
                        //if (!String.IsNullOrEmpty(hdfTranshipment.Value))
                        //    quotationHeaderObj.CEH_TRANSHIPMENT = hdfTranshipment.Value;

                        if (ddlTranshipment.SelectedValue != CommonConstants.SELECTVAL)
                            quotationHeaderObj.CEH_TRANSHIPMENT = ddlTranshipment.SelectedValue;
                        if (ddlShipBy.SelectedValue != CommonConstants.SELECTVAL)
                            quotationHeaderObj.CEH_SHIP_BY = ddlShipBy.SelectedValue;
                        quotationHeaderObj.CEH_SHIPPING_ADDRESS = HttpUtility.HtmlEncode(txtShippingAddress.Text);
                        if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                            quotationHeaderObj.CEH_SHIPPING_TO = ddlCustAddress.SelectedValue;
                        quotationHeaderObj.CEH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                        if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                            quotationHeaderObj.CEH_DEL_TERM = ddlDeliveryTerms.SelectedValue;
                        quotationHeaderObj.CEH_PAYMENT_TERMS = HttpUtility.HtmlEncode(txtPaymentTerms.Text);
                        if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                            quotationHeaderObj.CEH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                        quotationHeaderObj.CEH_SPECIAL_TERM_TEXT = HttpUtility.HtmlEncode(txtSpecialCause.Text);
                        if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                            quotationHeaderObj.CEH_SPECIAL_TERM = ddlSpecialCause.SelectedValue;
                        quotationHeaderObj.CEH_TO_PORT = HttpUtility.HtmlEncode(txtToPort.Text);
                        quotationHeaderObj.CEH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);

                        quotationHeaderObj.CEH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        quotationHeaderObj.CEH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        quotationHeaderObj.CEH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        //quotationHeaderObj.CEH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        //quotationHeaderObj.CEH_CRTD_DT = DateTime.Now;
                        //quotationHeaderObj.CEH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        //quotationHeaderObj.CEH_MOD_DT = DateTime.Now;

                        quotationHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        quotationHeaderObj.LAST_MOD_DT = DateTime.Now;
                        List<QuotationDetails> quotationResponsDetailsList = new List<QuotationDetails>();
                        quotationResponsDetailsList = (List<QuotationDetails>)SetUIValuesToObject(ControlsEnum.QUOTATIONDETAIL);
                        if (quotationResponsDetailsList != null && quotationResponsDetailsList.Count > 0)
                        {
                            quotationHeaderObj.QuotationDtl = new List<QuotationDetails>();
                            quotationResponsDetailsList.ForEach(dtl => quotationHeaderObj.QuotationDtl.Add(dtl));
                        }
                        if (QuotationHeaderSession != null)
                        {
                            quotationTaxHeaderList = new List<QuotationTaxHdr>();
                            quotationTaxHeaderList = QuotationHeaderSession.TaxHdr.ToList();
                            if (quotationTaxHeaderList != null && quotationTaxHeaderList.Count > 0)
                            {
                                quotationHeaderObj.TaxHdr = new List<QuotationTaxHdr>();
                                quotationTaxHeaderList.ForEach(dtl => quotationHeaderObj.TaxHdr.Add(dtl));
                            }
                        }

                        quotationHeaderObj.CEH_TOTAL_QTY = 0;//Dummy
                        txtSubTotal = (TextBox)grdQuotation.FooterRow.FindControl("txtSubTotalFooter");
                        quotationHeaderObj.CEH_TOTAL_AMT = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDecimal(txtSubTotal.Text.Trim());
                        quotationHeaderObj.CEH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);

                        quotationHeaderObj.CEH_TRX_STATUS = (byte)DirectOrderStatus.Quotation;
                        quotationHeaderObj.APT_CODE = ApplicationType.CQTN;
                        quotationHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                        quotationHeaderObj.WKF_FLAG = 0;
                        if (commonActions == ActionsEnum.SAVE)
                        {
                            quotationHeaderObj.WKF_FLAG = 0;
                        }
                        else if (commonActions == ActionsEnum.WRKFSUBMIT)
                        {
                            quotationHeaderObj.WKF_FLAG = 1;
                        }
                        retObject = quotationHeaderObj;
                        break;
                    case ControlsEnum.QUOTATIONDETAIL:
                        rowID = 0;
                        quotationResponsDetailsList = new List<QuotationDetails>();
                        foreach (GridViewRow grdrow in grdQuotation.Rows)
                        {
                            quotationDetailsObj = new QuotationDetails();
                            hdfCEDPK = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfCEDPK");
                            quotationDetailsObj.CED_PK = hdfCEDPK == null ? 0 : Convert.ToInt32(hdfCEDPK.Value);
                            hdfQuotationDtlPK = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfQuotationDtlPK");
                            ////quotationDetailsObj.RRD_RFQ_DTL = hdfQuotationDtlPK == null ? 0 : Convert.ToInt32(hdfQuotationDtlPK.Value);
                            quotationDetailsObj.CED_SL_NO = rowID + 1;
                            hdfItemPK = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfItemPK");
                            quotationDetailsObj.CED_ITEM = hdfItemPK == null ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            hdfCusItemPK = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfCusItemPK");
                            quotationDetailsObj.CED_CUST_ITEM = hdfCusItemPK == null ? 0 : Convert.ToInt32(hdfCusItemPK.Value);
                            ////lblSpecifications = (Label)grdQuotation.Rows[rowID].FindControl("lblSpecifications");
                            ////quotationDetailsObj.RRD_ITEM_SPEC = lblSpecifications == null ? string.Empty : string.IsNullOrEmpty(lblSpecifications.Text) ? string.Empty : lblSpecifications.Text.Trim();
                            txtQuantity = (TextBox)grdQuotation.Rows[rowID].FindControl("txtQuantity");
                            txtBrandQuantity = (TextBox)grdQuotation.Rows[rowID].FindControl("txtBrandQuantity");
                            hdfBrandUOMConvFactor = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfBrandUOMConvFactor");
                            hdfBrandUOMPK = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfBrandUOMPK");
                            quotationDetailsObj.CED_ENQ_QTY = txtQuantity == null ? 0 : string.IsNullOrEmpty(txtQuantity.Text) ? 0 : Convert.ToDouble(txtQuantity.Text);
                            hdfUoM = (HiddenField)grdQuotation.Rows[rowID].FindControl("hdfUoM");
                            quotationDetailsObj.CED_UOM = hdfUoM == null ? 0 : Convert.ToInt32(hdfUoM.Value);
                            txtRate = (TextBox)grdQuotation.Rows[rowID].FindControl("txtRate");
                            quotationDetailsObj.CED_RATE = txtRate == null ? 0 : string.IsNullOrEmpty(txtRate.Text.Trim()) ? 0 : Convert.ToDouble(txtRate.Text.Trim());
                            hasValidRate = hasValidRate ? true : quotationDetailsObj.CED_RATE > 0 ? true : false;
                            txtAmount = (TextBox)grdQuotation.Rows[rowID].FindControl("txtAmount");
                            quotationDetailsObj.CED_AMOUNT = txtAmount == null ? 0 : string.IsNullOrEmpty(txtAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAmount.Text.Trim());
                            txtDiscount = (TextBox)grdQuotation.Rows[rowID].FindControl("txtDiscount");
                            quotationDetailsObj.CED_DISCOUNT = txtDiscount == null ? 0 : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtDiscount.Text.Trim());
                            txtTax = (TextBox)grdQuotation.Rows[rowID].FindControl("txtTax");
                            quotationDetailsObj.CED_TAX = txtTax == null ? 0 : string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 : Convert.ToDouble(txtTax.Text.Trim());
                            txtTotal = (TextBox)grdQuotation.Rows[rowID].FindControl("txtTotal");
                            quotationDetailsObj.CED_AMT_NET_TOTAL = txtTotal == null ? 0 : string.IsNullOrEmpty(txtTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtTotal.Text.Trim());
                            txtValidFrom = (TextBox)grdQuotation.Rows[rowID].FindControl("txtValidFrom");
                            quotationDetailsObj.CED_VALID_FROM = txtValidFrom.Text.Trim();
                            txtValidTo = (TextBox)grdQuotation.Rows[rowID].FindControl("txtValidTo");
                            quotationDetailsObj.CED_VALID_TO = txtValidTo.Text.Trim();
                            txtComments = (TextBox)grdQuotation.Rows[rowID].FindControl("txtComments");
                            quotationDetailsObj.CED_COMMENTS = HttpUtility.HtmlEncode(txtComments.Text);
                            quotationDetailsObj.CED_SALE_QTY = txtBrandQuantity == null ? 0 : string.IsNullOrEmpty(txtBrandQuantity.Text) ? 0 : Convert.ToDouble(txtBrandQuantity.Text);
                            quotationDetailsObj.CED_SALE_UOM = Convert.ToInt32(hdfBrandUOMPK.Value);
                            quotationDetailsObj.CED_SALE_UOM_CONV = hdfBrandUOMConvFactor == null ? 1 : string.IsNullOrEmpty(hdfBrandUOMConvFactor.Value) ? 1 : Convert.ToDouble(hdfBrandUOMConvFactor.Value);
                            if (QuotationHeaderSession != null)
                            {
                                quotationTaxHeaderList = new List<QuotationTaxHdr>();
                                QuotationDetails tempResponseDetailsObj = QuotationHeaderSession.QuotationDtl.SingleOrDefault(quotation => quotation.CED_PK == quotationDetailsObj.CED_PK
                                    && quotation.CED_CUST_ITEM == quotationDetailsObj.CED_CUST_ITEM && quotation.CED_ITEM == quotationDetailsObj.CED_ITEM);
                                if (tempResponseDetailsObj != null)
                                {
                                    quotationTaxHeaderList = tempResponseDetailsObj.TaxDtl.ToList();
                                    if (quotationTaxHeaderList != null && quotationTaxHeaderList.Count > 0)
                                    {
                                        quotationTaxHeaderList.ForEach(dtl => dtl.ETD_SL_NO = quotationDetailsObj.CED_SL_NO);
                                        quotationDetailsObj.TaxDtl = new List<QuotationTaxHdr>();
                                        quotationTaxHeaderList.ForEach(dtl => quotationDetailsObj.TaxDtl.Add(dtl));
                                    }
                                }
                            }
                            quotationResponsDetailsList.Add(quotationDetailsObj);
                            rowID++;
                        }

                        retObject = quotationResponsDetailsList;
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
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.QUOTATIONHEADERTOP:
                        if (dsQuotationHeader != null && dsQuotationHeader.Tables[0].Rows.Count > 0)
                        {
                            ////lblRFQNo.Text = dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHNo].ToString();
                            ////hdfRFQNo.Value = HttpUtility.HtmlDecode(dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHNo].ToString());
                            ////lblRFQDate.Text = Convert.ToDateTime(dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHDate]).ToString(Resources.Constants.DateFormatShort);
                            ////hdfRFQDate.Value = dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHDate].ToString();
                        }
                        break;
                    case ControlsEnum.QUOTATIONHEADER:
                        if (quotationHeaderObj != null)
                        {
                            txtCustomer.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_CUSTOMER_TEXT);
                            custPK = quotationHeaderObj.CEH_CUSTOMER;
                            hdfCustomer.Value = quotationHeaderObj.CEH_CUSTOMER.ToString();
                            lblQuotationNo.Text = string.IsNullOrEmpty(quotationHeaderObj.CEH_NO) ?
                                Resources.Messages.DocGenerationNew : quotationHeaderObj.CEH_NO.ToString();
                            lblEnquiryNo.Text = quotationHeaderObj.CEH_REF_NO.ToString();
                            hdfEnquiryNo.Value = quotationHeaderObj.CEH_REF_NO.ToString();
                            lblEnquiryDate.Text = quotationHeaderObj.CEH_REF_DATE.ToString();
                            hdfEnquiryDate.Value = quotationHeaderObj.CEH_REF_DATE.ToString();
                            hdfQuotationPK.Value = quotationHeaderObj.CEH_PK.ToString();
                            txtQuotationDate.Text = quotationHeaderObj.CEH_DATE;
                            QtnStatus = quotationHeaderObj.CEH_STATUS;
                            hdfTrxStatus.Value = quotationHeaderObj.CEH_TRX_STATUS.ToString();
                            ////////txtVendorRef.Text = HttpUtility.HtmlDecode(quotationHeaderObj.RRH_VEN_REF_NO);

                            //txtPaymentTerms.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_PAYMENT_TERM_TEXT);
                            //GetFieldValues(ControlsEnum.CUSTOMERSELECTED);
                            //addressPK = quotationHeaderObj.CEH_SHIPPING_TO;
                            //SetFieldValues(ControlsEnum.CUSTOMERSELECTED);
                            //txtShippingAddress.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SHIPPING_ADDRESS);

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_SHIPPING_TO))
                            //{
                            //    hdfCustAddress.Value = quotationHeaderObj.CEH_SHIPPING_TO;
                            //    txtCustAddress_Txt.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SHIPPING_TO_TEXT);
                            //}
                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_SHIPPING_ADDRESS))
                            //    txtShippingAddress.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SHIPPING_ADDRESS);

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_DEL_TERM))
                            //{
                            //    hdfDeliveryTerms.Value = quotationHeaderObj.CEH_DEL_TERM;
                            //    txtDeliveryTerms_Txt.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_DEL_TERM_NAME);
                            //}
                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_DEL_TERM_TEXT))
                            //    txtDeliveryTerms.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_DEL_TERM_TEXT);

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_PAYMENT_TERM))
                            //{
                            //    hdfPaymentTerms.Value = quotationHeaderObj.CEH_PAYMENT_TERM;
                            //    txtPaymentTerms_Txt.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_PAYMENT_TERM_NAME);
                            //}
                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_PAYMENT_TERMS))
                            //    txtPaymentTerms.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_PAYMENT_TERMS);

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_SPECIAL_TERM))
                            //{
                            //    hdfSpecialCause.Value = quotationHeaderObj.CEH_SPECIAL_TERM;
                            //    txtSpecialCause_Txt.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SPECIAL_TERM_NAME);
                            //}
                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_SPECIAL_TERM_TEXT))
                            //    txtSpecialCause.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SPECIAL_TERM_TEXT);

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_SHIP_BY))
                            //{
                            //    hdfShipBy.Value = quotationHeaderObj.CEH_SHIP_BY;
                            //    txtShipBy.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SHIP_BY_TEXT);
                            //}

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_TO_PORT))
                            //{
                            //    txtToPort.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_TO_PORT);
                            //}

                            //if (!String.IsNullOrEmpty(quotationHeaderObj.CEH_TRANSHIPMENT))
                            //{
                            //    hdfTranshipment.Value = quotationHeaderObj.CEH_TRANSHIPMENT;
                            //    txtTranshipment.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_TRANSHIPMENT_TEXT);
                            //}

                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            if (!string.IsNullOrEmpty(quotationHeaderObj.CEH_TRANSHIPMENT))
                                transhipmentPK = Convert.ToInt32(quotationHeaderObj.CEH_TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);
                            GetFieldValues(ControlsEnum.SHIPBY);
                            if (!string.IsNullOrEmpty(quotationHeaderObj.CEH_SHIP_BY))
                                shipByPK = Convert.ToInt32(quotationHeaderObj.CEH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);

                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(quotationHeaderObj.CEH_SHIPPING_TO))
                                addressPK = Convert.ToInt32(quotationHeaderObj.CEH_SHIPPING_TO);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            if (!string.IsNullOrEmpty(quotationHeaderObj.CEH_DEL_TERM))
                                deliveryTermPK = Convert.ToInt32(quotationHeaderObj.CEH_DEL_TERM);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            if (!string.IsNullOrEmpty(quotationHeaderObj.CEH_PAYMENT_TERM))
                                paymentTermPK = Convert.ToInt32(quotationHeaderObj.CEH_PAYMENT_TERM);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (!string.IsNullOrEmpty(quotationHeaderObj.CEH_SPECIAL_TERM))
                                specialCausePK = Convert.ToInt32(quotationHeaderObj.CEH_SPECIAL_TERM);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);

                            txtToPort.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_TO_PORT);

                            txtShippingAddress.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SHIPPING_ADDRESS);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_DEL_TERM_TEXT);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_PAYMENT_TERMS);
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_SPECIAL_TERM_TEXT);

                            txtRemarks.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_REMARKS);
                            txtHdrDiscount.Text = txtHdrDiscount.ToolTip = quotationHeaderObj.CEH_TOTAL_DISCOUNT.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.Text = txtHdrTax.ToolTip = Math.Round(quotationHeaderObj.CEH_TOTAL_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = quotationHeaderObj.CEH_TOTAL_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = quotationHeaderObj.CEH_TOTAL_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrTotal.Text = txtHdrTotal.ToolTip = quotationHeaderObj.CEH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                            hdfExchangeRate.Value = quotationHeaderObj.CEH_CURRENCY_RATE.ToString();
                            hdfCurrency.Value = quotationHeaderObj.CEH_CURRENCY.ToString();
                            txtCurrency.Text = HttpUtility.HtmlDecode(quotationHeaderObj.CEH_CURRENCY_TEXT);
                            hdfQuotationFlag.Value = quotationHeaderObj.CEH_QUOTATION_FLAG.ToString();

                            if (QtnStatus == 11 || QtnStatus == 17)
                            {
                                //btnSave.Visible = false;
                                //imgHdrDiscount.Visible = false;
                                //imgHdrTax.Visible = false;
                                //btnApply.Visible = false;
                                //imgPopupAdd.Visible = false;


                                txtQuotationDate.Enabled = false;
                                txtCurrency.Enabled = false;
                                ddlShipBy.Enabled = false;
                                ddlTranshipment.Enabled = false;
                                txtShipping.Enabled = false;
                                txtToPort.Enabled = false;
                                txtPriceAdj.Enabled = false;
                                txtDeliveryTerms.Enabled = false;
                                ddlDeliveryTerms.Enabled = false;
                                txtSpecialCause.Enabled = false;
                                ddlPaymentTerms.Enabled = false;
                                txtPaymentTerms.Enabled = false;
                                ddlSpecialCause.Enabled = false;
                                txtSpecialCause.Enabled = false;
                                ddlCustAddress.Enabled = false;
                                txtShippingAddress.Enabled = false;
                                txtRemarks.Enabled = false;
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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.VENDOR:
                    //Bind Vendor dropdown
                    //ddlVendor.Items.Clear();
                    //if (dsVendor != null && dsVendor.Tables[0].Rows.Count > 0)
                    //{
                    //    ddlVendor.DataSource = dsVendor.Tables[0].DataSet;
                    //    ddlVendor.DataTextField = Resources.DataFieldRes.RFQResponseVendorText;
                    //    ddlVendor.DataValueField = Resources.DataFieldRes.RFQResponseVendorPK;
                    //    ddlVendor.DataBind();
                    //}
                    //ddlVendor.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    //if (selectedVendor != null)
                    //{
                    //    ddlVendor.SelectedValue = selectedVendor.ToString();
                    //}
                    break;
                case ControlsEnum.QUOTATIONTAXTYPES:
                    //Bind Tax dropdown
                    ddlPopupTaxType.Items.Clear();
                    if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count > 0)
                    {
                        ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtQuotationTaxDetails, "TAX_HEAD");
                        ddlPopupTaxType.DataTextField = "TAX_HEAD";
                        ddlPopupTaxType.DataValueField = "TAX_PK";
                        ddlPopupTaxType.DataBind();
                    }
                    if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    break;
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
                    if (addressPK > 0)
                        ddlCustAddress.SelectedValue = addressPK.ToString();
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
                    case ControlsEnum.QUOTATIONDETAIL:
                        if (quotationHeaderObj != null)
                        {
                            quotationDetailsList = new List<QuotationDetails>();
                            quotationDetailsList = quotationHeaderObj.QuotationDtl;
                            if (quotationDetailsList != null)
                            {
                                grdQuotation.DataSource = quotationDetailsList;
                                grdQuotation.DataBind();
                            }
                        }
                        break;
                    case ControlsEnum.QUOTATIONTAXPOPUPGRID:

                        if (IsHeaderTax)
                        {

                            quotationTaxHdrList = TempQuotationHeaderSession.TaxHdr.Where(tax => tax.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            quotationDetailsObj = TempQuotationHeaderSession.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK);
                            if (quotationDetailsObj != null)
                            {
                                quotationTaxHdrList = quotationDetailsObj.TaxDtl.Where(tax => tax.ETD_ENQUIRY_DTL == SelectedQuotationPK && tax.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        grdTaxDetails.DataSource = quotationTaxHdrList;
                        grdTaxDetails.DataBind();
                        break;

                    case ControlsEnum.ITEMRATES:
                        grdItemRates.DataSource = dsPageData.Tables[0];
                        grdItemRates.DataBind();
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        grdRevisionHistory.DataSource = dsPageData.Tables[0];
                        grdRevisionHistory.DataBind();
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
                switch (Mode)
                {
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "');", true);
                        break;
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
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.QUOTATIONTAXPOPUPGRID:
                    ////txtPopupAmount.Text = string.Empty;
                    ////txtPopupItemAmount.Text = string.Empty;
                    ////txtPopupOther.Text = string.Empty;
                    TaxPK = 0;
                    ////grdTaxDetails.DataSource = null;
                    ////grdTaxDetails.DataBind();
                    TempQuotationHeaderSession = null;
                    ////hdfTaxFormula.Value = string.Empty;
                    SelectedQuotationPK = 0;
                    SelectedItemPK = 0;
                    SelectedCusItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break;
                case ControlsEnum.QUOTATIONHEADER:
                    CurrPK = 0;
                    QtnStatus = 0;
                    break;
            }



        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            commonServiceObj = new CommonService();
            AppTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.CQTN, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        ////////////////////////////////////////////
        /// 
        /// 
        ///
        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            try
            {
                while (tokenIndex < tokens.Count)
                {
                    string token = tokens[tokenIndex];
                    if (token == "(")
                    {
                        string subExpr = getSubExpression(tokens, ref tokenIndex);
                        operandStack.Push(StringToFormula(subExpr));
                        continue;
                    }
                    if (token == ")")
                    {
                        throw new ArgumentException("Mis-matched parentheses in expression");
                    }
                    //If this is an operator  
                    if (Array.IndexOf(_operators, token) >= 0)
                    {
                        while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                        {
                            string op = operatorStack.Pop();
                            double arg2 = operandStack.Pop();
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                        }
                        operatorStack.Push(token);
                    }
                    else
                    {
                        operandStack.Push(double.Parse(token));
                    }
                    tokenIndex += 1;
                }

                while (operatorStack.Count > 0)
                {
                    string op = operatorStack.Pop();
                    double arg2 = operandStack.Pop();
                    double arg1 = operandStack.Pop();
                    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                }
                return operandStack.Pop();
            }
            catch
            {
                return 0;
            }
        }

        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }

        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }

        private void SetDetailTax(object sender)
        {
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;

            TextBox txtRate;
            TextBox txtQuantity;

            HiddenField hdfCEDPK;
            HiddenField hdfItemPK;
            HiddenField hdfCusItemPK;

            double quantity;
            double rate;
            quantity = 0;
            rate = 0;


            if (sender != null)
            {
                txtRate = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtRate") as TextBox);
                txtQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtBrandQuantity") as TextBox); //"txtQuantity"
                txtAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                if (txtRate != null && txtQuantity != null)
                {
                    if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(txtQuantity.Text, out rate))
                    {
                        if (txtAmount != null)
                        {
                            txtAmount.Text = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                            txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
                            txtTotal = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTotal") as TextBox);

                            hdfCEDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfCEDPK") as HiddenField);
                            hdfCusItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfCusItemPK") as HiddenField);
                            hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            if (hdfCEDPK != null && hdfCusItemPK != null && hdfItemPK != null)
                            {
                                SelectedQuotationPK = string.IsNullOrEmpty(hdfCEDPK.Value) ? 0 : Convert.ToInt32(hdfCEDPK.Value);
                                SelectedCusItemPK = string.IsNullOrEmpty(hdfCusItemPK.Value) ? 0 : Convert.ToInt32(hdfCusItemPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                            }
                        }
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            else
            {
                QuotationHeaderSession = TempQuotationHeaderSession;
                foreach (GridViewRow gvr in grdQuotation.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        hdfCEDPK = (gvr.FindControl("hdfCEDPK") as HiddenField);
                        hdfCusItemPK = (gvr.FindControl("hdfCusItemPK") as HiddenField);
                        hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
                        if (SelectedQuotationPK == Convert.ToInt32(hdfCEDPK.Value) && SelectedItemPK == Convert.ToInt32(hdfItemPK.Value) && SelectedCusItemPK == Convert.ToInt32(hdfCusItemPK.Value))
                        {
                            txtAmount = (gvr.FindControl("txtAmount") as TextBox);
                            txtDiscount = (gvr.FindControl("txtDiscount") as TextBox);
                            txtTax = (gvr.FindControl("txtTax") as TextBox);
                            txtTotal = (gvr.FindControl("txtTotal") as TextBox);
                            if (!SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal))
                                return;
                        }
                    }
                }
            }
            SetSubTotal();
        }
        private bool SetDetailTax(TextBox txtAmount, TextBox txtDiscount, TextBox txtTax, TextBox txtTotal)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            if (txtAmount != null && txtDiscount != null && txtTax != null && txtTotal != null)
            {
                Double.TryParse(txtAmount.Text.Trim(), out amount);
                if (amount >= 0)
                {
                    if (QuotationHeaderSession != null)
                    {
                        quotationHeaderObj = QuotationHeaderSession;
                        quotationDetailsObj = quotationHeaderObj.QuotationDtl.SingleOrDefault(quotation => quotation.CED_PK == SelectedQuotationPK
                            && quotation.CED_CUST_ITEM == SelectedCusItemPK && quotation.CED_ITEM == SelectedItemPK);
                        if (quotationDetailsObj != null)
                        {
                            var discDetail = quotationDetailsObj.TaxDtl.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (QuotationTaxHdr quotationTaxHdrObj in discDetail)
                            {
                                string taxFormula = quotationTaxHdrObj.ETD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    quotationTaxHdrObj.ETD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                            discount = quotationDetailsObj.TaxDtl.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.ETD_TAX_AMT);
                            netAmount = amount - discount;
                            txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = quotationDetailsObj.TaxDtl.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (QuotationTaxHdr quotationTaxHdrObj in taxDetail)
                            {
                                string taxFormula = quotationTaxHdrObj.ETD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                    quotationTaxHdrObj.ETD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                            itmTax = quotationDetailsObj.TaxDtl.ToList().Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.ETD_TAX_AMT);
                            txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                            quotationDetailsObj.CED_AMOUNT = amount;
                            quotationDetailsObj.CED_DISCOUNT = discount;
                            quotationDetailsObj.CED_TAX = itmTax;
                            quotationDetailsObj.CED_AMT_NET_TOTAL = (amount - discount + itmTax);
                            txtTotal.Text = quotationDetailsObj.CED_AMT_NET_TOTAL.ToString(hdfCurrencyFormat.Value);
                            QuotationHeaderSession = quotationHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void SetSubTotal()
        {
            TextBox txtSubTotalFooter;
            txtSubTotalFooter = grdQuotation.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
            if (txtSubTotalFooter != null)
            {
                QuotationHeaderSession.CEH_TOTAL_AMT = Convert.ToDecimal(QuotationHeaderSession.QuotationDtl.Sum(dtl => dtl.CED_AMT_NET_TOTAL));
                txtSubTotalFooter.Text = txtSubTotalFooter.ToolTip = QuotationHeaderSession.CEH_TOTAL_AMT.ToString(hdfCurrencyFormat.Value);
            }
        }
        private bool SetHdrTax()
        {
            //TextBox txtSubTotal;
            double amount;
            double discount;
            double shipping;
            double adjust;
            amount = 0;
            shipping = 0;
            adjust = 0;

            //txtSubTotal = (TextBox)grdQuotation.FooterRow.FindControl("txtSubTotalFooter");
            //Double.TryParse(txtSubTotal.Text.Trim(), out amount);
            //quotationHeaderObj.CEH_TOTAL_AMT = Convert.ToDecimal(amount);
            //if (amount >= 0)
            //{
            if (QuotationHeaderSession != null)
            {
                quotationHeaderObj = QuotationHeaderSession;
                amount = Convert.ToDouble(quotationHeaderObj.CEH_TOTAL_AMT);
                discount = 0;
                var discHeader = quotationHeaderObj.TaxHdr.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Discount));
                foreach (QuotationTaxHdr quotationTaxHdrObj in discHeader)
                {
                    string taxFormula = quotationTaxHdrObj.ETD_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {
                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        quotationTaxHdrObj.ETD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                    }
                }
                discount = quotationHeaderObj.TaxHdr.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.ETD_TAX_AMT);
                quotationHeaderObj.CEH_TOTAL_DISCOUNT = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;

                var taxHeader = quotationHeaderObj.TaxHdr.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Tax));
                foreach (QuotationTaxHdr quotationTaxHdrObj in taxHeader)
                {
                    string taxFormula = quotationTaxHdrObj.ETD_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {
                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        quotationTaxHdrObj.ETD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                    }
                }
                quotationHeaderObj.CEH_TOTAL_TAX = quotationHeaderObj.TaxHdr.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.ETD_TAX_AMT);
                txtHdrTax.Text = txtHdrTax.ToolTip = Math.Round(quotationHeaderObj.CEH_TOTAL_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtShipping.Text, out shipping);
                quotationHeaderObj.CEH_TOTAL_SHIP_CHARGE = shipping;
                double.TryParse(txtPriceAdj.Text, out adjust);
                quotationHeaderObj.CEH_TOTAL_ADJUST = adjust;
                quotationHeaderObj.CEH_NET_AMOUNT = Convert.ToDouble(quotationHeaderObj.CEH_TOTAL_AMT) - quotationHeaderObj.CEH_TOTAL_DISCOUNT + quotationHeaderObj.CEH_TOTAL_TAX
                    + quotationHeaderObj.CEH_TOTAL_SHIP_CHARGE + quotationHeaderObj.CEH_TOTAL_ADJUST;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = quotationHeaderObj.CEH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                QuotationHeaderSession = quotationHeaderObj;
            }
            return true;
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Amount_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
            //    return false;
            //}
        }
        private double CalculateTaxFormula(string taxFormula, double amount)
        {
            double taxAmt;
            taxAmt = 0;
            if (!string.IsNullOrEmpty(taxFormula))
            {
                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return taxAmt;
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
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
                HiddenField hdfCEDPK;
                HiddenField hdfItemPK;
                HiddenField hdfCusItemPK;
                TextBox txtAmount;
                TextBox txtTax;
                TextBox txtDiscount;
                TextBox txtSubTotal;
                string selectedItemPK;
                IEnumerable<QuotationDetails> selectedQuotationDetails;
                double amount;
                double discount;
                int count;
                string action;
                DropDownList ddlWkfAction;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                string addr;
                bool isValidDisc = true;

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
                    if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
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
                    else if (((DropDownList)sender).ID == "ddlCustAddress")
                    {
                        commonActions = ActionsEnum.ADDRESSSELECTED;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtRate" || ((TextBox)sender).ID == "txtBrandQuantity")//"txtQuantity"
                    {
                        commonActions = ActionsEnum.CALCULATEDTLTAX;
                    }
                }
                switch (commonActions)
                {

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
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            hasValidRate = false;
                            quotationHeaderObj = new QuotationHeader();
                            quotationHeaderObj = (QuotationHeader)SetUIValuesToObject(ControlsEnum.QUOTATIONHEADER);
                            if (hasValidRate)
                            {
                                if (quotationHeaderObj != null && quotationHeaderObj.QuotationDtl != null)
                                {
                                    quotationHeaderObj.DRAFT_FLAG = 1;
                                    string xmlDoc = CommonFunctions.XmlSerialize<QuotationHeader>(quotationHeaderObj);//CommonFunctions.ObjectTOXml(quotationHeaderObj);
                                    // save Process Control inspection details
                                    result = BusinessLogic.Sales.QuotationBL.SaveQuotationDetails(xmlDoc);
                                    if (result > 0) // Success !  redirect to listing page
                                    {
                                        // Show Save Message and redired to listing page
                                        //string routeURL = "RFQResponse.aspx";
                                        GetFieldValues(ControlsEnum.QUOTATION);
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Quotation);
                                        if (string.IsNullOrEmpty(lblQuotationNo.Text)
                                            || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.QuotationListing) + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Quotation_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            hasValidRate = false;
                        }
                        break;
                    #endregion
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
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                hasValidRate = false;
                                quotationHeaderObj = new QuotationHeader();
                                quotationHeaderObj = (QuotationHeader)SetUIValuesToObject(ControlsEnum.QUOTATIONHEADER);
                                if (hasValidRate)
                                {
                                    if (quotationHeaderObj != null && quotationHeaderObj.QuotationDtl != null)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<QuotationHeader>(quotationHeaderObj);//CommonFunctions.ObjectTOXml(quotationHeaderObj);
                                        // save Process Control inspection details
                                        result = BusinessLogic.Sales.QuotationBL.SaveQuotationDetails(xmlDoc);
                                        if (result.HasValue && result.Value > 0) // Success ! re-initialize the page
                                        {
                                            //Workflow submission
                                            ucrWrkf.ApplicationID = result.Value;
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Quotation_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Empty_Rate").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;

                            if (ucrWrkf.ApplicationID > 0)
                            {
                                //Workflow submission
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result.HasValue && result.Value > 0)
                                    {
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                            litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                        //Show Save success message and reset Contract Entry
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                                            GetFieldValues(ControlsEnum.QUOTATION);
                                            quotationHeaderObj = QuotationHeaderSession;
                                            object[] args = new object[2];
                                            args[0] = Resources.PageNameRes.Quotation;
                                            args[1] = quotationHeaderObj.CEH_NO;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        }
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;

                                        //Show Save success message and reset Contract Entry

                                        //litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Quotation);
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            if (string.IsNullOrEmpty(lblQuotationNo.Text)
                                                || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.QuotationListing) + "');", true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region RFQTAXDETAILS
                    case ActionsEnum.RFQTAXDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        //if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        //{
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfCEDPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfCEDPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfCusItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfCusItemPK") as HiddenField);
                        if (txtAmount != null && hdfCEDPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                            SelectedQuotationPK = string.IsNullOrEmpty(hdfCEDPK.Value) ? 0 : Convert.ToInt32(hdfCEDPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfCusItemPK.Value) ? 0 : Convert.ToInt32(hdfCusItemPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            if (QuotationHeaderSession != null)
                            {
                                TempQuotationHeaderSession = QuotationHeaderSession;
                                IsHeaderTax = false;
                                SetFieldValues(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                SetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                        TaxPK = 0;

                                        if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtQuotationTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtQuotationTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                            }
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region RFQDISCDETAILS
                    case ActionsEnum.RFQDISCDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        //if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        //{
                        txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        hdfCEDPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfCEDPK") as HiddenField);
                        hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        hdfCusItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfCusItemPK") as HiddenField);
                        if (txtAmount != null && hdfCEDPK != null && txtDiscount != null)
                        {
                            txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                            SelectedQuotationPK = string.IsNullOrEmpty(hdfCEDPK.Value) ? 0 : Convert.ToInt32(hdfCEDPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfCusItemPK.Value) ? 0 : Convert.ToInt32(hdfCusItemPK.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            if (QuotationHeaderSession != null)
                            {
                                TempQuotationHeaderSession = QuotationHeaderSession;
                                IsHeaderTax = false;
                                SetFieldValues(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                SetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                        TaxPK = 0;
                                        if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtQuotationTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtQuotationTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                            }
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region RFQTAXHEADER
                    case ActionsEnum.RFQTAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        //if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        //{
                        if (QuotationHeaderSession != null)
                        {
                            TempQuotationHeaderSession = QuotationHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                            SetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                            txtSubTotal = (TextBox)grdQuotation.FooterRow.FindControl("txtSubTotalFooter");
                            if (txtSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value)
                                    : string.IsNullOrEmpty(txtHdrDiscount.Text) ? Convert.ToDouble(txtSubTotal.Text).ToString(hdfCurrencyFormat.Value)
                                    : (Convert.ToDouble(txtSubTotal.Text.Trim()) - Convert.ToDouble(txtHdrDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                        TaxPK = 0;
                                        if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtQuotationTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtQuotationTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                            }
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region RFQDISCHEADER
                    case ActionsEnum.RFQDISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        //if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        //{
                        if (QuotationHeaderSession != null)
                        {
                            TempQuotationHeaderSession = QuotationHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                            SetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                            txtSubTotal = (TextBox)grdQuotation.FooterRow.FindControl("txtSubTotalFooter");
                            if (txtSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtSubTotal.Text).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                        TaxPK = 0;
                                        if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtQuotationTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtQuotationTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                            }
                        }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        SetDetailTax(null);
                        SetHdrTax();
                        ResetForm(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:
                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempQuotationHeaderSession != null)
                        {
                            quotationHeaderObj = TempQuotationHeaderSession;
                            tempQuotationTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempQuotationTaxSplitObj = quotationHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.ETD_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempQuotationTaxSplitObj = quotationHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.ETD_NAME == txtPopupOther.Text.Trim() && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                quotationDetailsObj = quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                    && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK);

                                if (quotationDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempQuotationTaxSplitObj = quotationDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.ETD_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempQuotationTaxSplitObj = quotationDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.ETD_NAME == txtPopupOther.Text.Trim() && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempQuotationTaxSplitObj == null)
                            {
                                quotationTaxHdrList = new List<QuotationTaxHdr>();

                                quotationTaxHdrObj = new QuotationTaxHdr();
                                try
                                {
                                    quotationTaxHdrObj.ETD_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    quotationTaxHdrObj.ETD_ENQUIRY_DTL = SelectedQuotationPK;
                                    quotationTaxHdrObj.ETD_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        quotationTaxHdrObj.ETD_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    quotationTaxHdrObj.ETD_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    quotationTaxHdrObj.ETD_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    quotationTaxHdrObj.ETD_PK = 0;
                                    //quotationTaxHdrObj.ETD_TAX_CATEGORY_TEXT = "Tax";
                                    quotationTaxHdrObj.ETD_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    quotationTaxHdrObj.ETD_TYPE = 1;
                                    quotationTaxHdrObj.ETD_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (quotationTaxHdrObj.ETD_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(quotationHeaderObj.CEH_TOTAL_AMT);
                                            currentTotal = quotationHeaderObj.TaxHdr.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.ETD_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(quotationTaxHdrObj.ETD_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = quotationTaxHdrObj.ETD_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                quotationTaxHdrList = quotationHeaderObj.TaxHdr.ToList();
                                                quotationTaxHdrList.Add(quotationTaxHdrObj);
                                                quotationHeaderObj.TaxHdr = quotationTaxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            quotationTaxHdrList = quotationHeaderObj.TaxHdr.ToList();
                                            quotationTaxHdrList.Add(quotationTaxHdrObj);
                                            quotationHeaderObj.TaxHdr = quotationTaxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        quotationDetailsObj = quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                            && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK);
                                        if (quotationDetailsObj != null)
                                        {
                                            if (quotationTaxHdrObj.ETD_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = quotationDetailsObj.CED_AMOUNT;
                                                currentTotal = quotationDetailsObj.TaxDtl.Where(quotation => quotation.ETD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.ETD_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(quotationTaxHdrObj.ETD_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = quotationTaxHdrObj.ETD_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    quotationTaxHdrList = quotationDetailsObj.TaxDtl.ToList();
                                                    quotationTaxHdrList.Add(quotationTaxHdrObj);
                                                    quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                                        && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK).TaxDtl = quotationTaxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                quotationTaxHdrList = quotationDetailsObj.TaxDtl.ToList();
                                                quotationTaxHdrList.Add(quotationTaxHdrObj);
                                                quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                                    && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK).TaxDtl = quotationTaxHdrList;
                                            }
                                        }
                                    }
                                    TempQuotationHeaderSession = quotationHeaderObj;
                                    SetFieldValues(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                                }
                            }
                            else
                            {
                                errorTaxAdd = true;
                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                                if (!errorTaxAdd && !errorTaxAmount)
                                {
                                    txtPopupAmount.Text = string.Empty;
                                    txtPopupOther.Text = string.Empty;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                        if (errorTaxAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (TempQuotationHeaderSession != null)
                        {
                            quotationHeaderObj = TempQuotationHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                quotationTaxHdrList = new List<QuotationTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempQuotationTaxSplitObj = quotationHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.ETD_TAX == taxPK && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempQuotationTaxSplitObj = quotationHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.ETD_NAME == hdfTaxName.Value && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempQuotationTaxSplitObj != null)
                                    {
                                        quotationTaxHdrList = quotationHeaderObj.TaxHdr.ToList();
                                        quotationTaxHdrList.Remove(tempQuotationTaxSplitObj);
                                        quotationHeaderObj.TaxHdr = quotationTaxHdrList;
                                    }
                                }
                                else
                                {
                                    quotationDetailsObj = quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                        && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK);
                                    if (quotationDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempQuotationTaxSplitObj = quotationDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.ETD_TAX == taxPK && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempQuotationTaxSplitObj = quotationDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.ETD_NAME == hdfTaxName.Value && rfq.ETD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        quotationDetailsObj = quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                            && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK);
                                        if (quotationDetailsObj != null)
                                        {
                                            quotationTaxHdrList = quotationDetailsObj.TaxDtl.ToList();
                                            quotationTaxHdrList.Remove(tempQuotationTaxSplitObj);
                                            quotationHeaderObj.QuotationDtl.SingleOrDefault(rfq => rfq.CED_PK == SelectedQuotationPK
                                                && rfq.CED_CUST_ITEM == SelectedCusItemPK && rfq.CED_ITEM == SelectedItemPK).TaxDtl = quotationTaxHdrList;
                                        }
                                    }
                                }

                                TempQuotationHeaderSession = quotationHeaderObj;
                                SetFieldValues(ControlsEnum.QUOTATIONTAXPOPUPGRID);

                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                                        TaxPK = 0;
                                        if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtQuotationTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtQuotationTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                        break;
                    #endregion
                    case ActionsEnum.PRINT:
                        SetUIEditView(commonActions);
                        break;
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.QUOTATIONTAXTYPES);
                            TaxPK = 0;
                            if (dtQuotationTaxDetails != null && dtQuotationTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtQuotationTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value);
                                SelectedTaxText = HttpUtility.HtmlEncode(dtQuotationTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                txtPopupAmount.Enabled = false;
                                txtPopupOther.Enabled = false;
                            }
                        }
                        else if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                        {
                            hdfTaxFormula.Value = string.Empty;
                            txtPopupAmount.Text = string.Empty;
                            SelectedTaxText = Resources.Report.Custom;
                            txtPopupOther.Text = string.Empty;
                            txtPopupAmount.Enabled = true;
                            txtPopupOther.Enabled = true;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.QUOTATIONHEADER);
                        if (string.IsNullOrEmpty(lblQuotationNo.Text)
                            || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.EnquiryListing), false);
                        }
                        else
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.QuotationListing), false);
                        }
                        break;
                    #endregion
                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        SetDetailTax(sender);
                        SetHdrTax();
                        ResetForm(ControlsEnum.QUOTATIONTAXPOPUPGRID);
                        break;
                    #endregion
                    #region SALEORDER
                    case ActionsEnum.SALEORDER:
                        if (CurrPK > 0)
                        {
                            Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = CurrPK;
                            if (Session["EnquiryMode"] != null)
                            {
                                if ((EntryStatus)Session["EnquiryMode"] == EntryStatus.VIEWMODE)
                                {
                                    EntryStatus = EntryStatus.VIEWMODE;
                                }
                                else
                                {
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                }
                            }
                            else
                            {
                                EntryStatus = EntryStatus.ENTRYMODE;
                            }
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.SaleContractDetails), false);
                        }
                        //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        //result = BusinessLogic.Sales.QuotationBL.GenerateSaleOrder(CurrPK, Convert.ToInt32(currentUser.PKUser));
                        //if (result > 0)
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("SaleOrderGenerateSuccess").ToString()
                        //                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.CustomerOrderCreation) + "');", true);
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("SaleOrderGenerateFailed").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region Rate History
                    case ActionsEnum.ITEMRATES:
                        selectedItemPK = ((((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField).Value;
                        if (!string.IsNullOrEmpty(selectedItemPK))
                            selectedItem = Convert.ToInt32(selectedItemPK);
                        if (selectedItem > 0)
                        {
                            GetFieldValues(ControlsEnum.ITEMDETAILS);
                            if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                lblItemCodeTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["ITM_CODE"].ToString()), 30);
                                lblItemNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["ITM_NAME"].ToString()), 30);
                                lblItemCodeTxt.ToolTip = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["ITM_CODE"].ToString());
                                lblItemNameTxt.ToolTip = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["ITM_NAME"].ToString());
                                GetFieldValues(ControlsEnum.ITEMRATES);
                                SetFieldValues(ControlsEnum.ITEMRATES);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=ItemRateDialog]','" + GetLocalResourceObject("Rates").ToString() + "','930','500');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Tabs
                    case ActionsEnum.ENQUIRY:
                        if (string.IsNullOrEmpty(lblQuotationNo.Text)
                            || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                        {
                            Session["EnquiryMode"] = EntryStatus.ENTRYMODE;
                            Session["QuotationToEnquiry"] = null;
                            Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK;
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Enquiry), false);
                        }
                        else
                        {
                            Session["EnquiryMode"] = EntryStatus;
                            Session["QuotationToEnquiry"] = "1";
                            Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK;
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.Enquiry), false);
                        }
                        break;
                    #endregion
                    case ActionsEnum.ENQUIRYLIST:
                        ResetForm(ControlsEnum.QUOTATIONHEADER);
                        if (string.IsNullOrEmpty(lblQuotationNo.Text)
                            || lblQuotationNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.EnquiryListing), false);
                        }
                        else
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.QuotationListing), false);
                        }
                        break;
                    #region Revision History
                    case ActionsEnum.REVISIONHISTORY:
                        GetFieldValues(ControlsEnum.REVISIONHISTORY);
                        SetFieldValues(ControlsEnum.REVISIONHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divRevisionHistory]','" + GetLocalResourceObject("RevisionHistory").ToString() + "','400','300');", true);
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
            try
            {
                if ((sender as GridView).ID == "grdQuotation")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[11].Visible = EnableItemDiscount;
                        e.Row.Cells[12].Visible = EnableItemTax;
                        e.Row.Cells[10].Visible = EnableItemDiscount || EnableItemTax;
                        //(e.Row.FindControl("imgDiscount") as ImageButton).Visible = (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);
                        //(e.Row.FindControl("imgTax") as ImageButton).Visible = (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);

                        if (QtnStatus == 11 || QtnStatus == 17)
                        {
                            //(e.Row.FindControl("imgDiscount") as ImageButton).Visible = false;
                            //(e.Row.FindControl("imgTax") as ImageButton).Visible = false;
                            (e.Row.FindControl("txtBrandQuantity") as TextBox).Enabled = false;
                            (e.Row.FindControl("txtRate") as TextBox).Enabled = false;
                            (e.Row.FindControl("txtValidFrom") as TextBox).Enabled = false;
                            (e.Row.FindControl("txtValidTo") as TextBox).Enabled = false;
                            (e.Row.FindControl("txtComments") as TextBox).Enabled = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        e.Row.Cells[11].Visible = EnableItemDiscount;
                        e.Row.Cells[12].Visible = EnableItemTax;
                        e.Row.Cells[10].Visible = EnableItemDiscount || EnableItemTax;
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[11].Visible = EnableItemDiscount;
                        e.Row.Cells[12].Visible = EnableItemTax;
                        e.Row.Cells[10].Visible = EnableItemDiscount || EnableItemTax;
                    }
                }
                else if ((sender as GridView).ID == "grdRevisionHistory")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //(e.Row.FindControl("lnkRevisionPrint") as LinkButton).PostBackUrl = "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "CEH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "CEH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.CQTN + "&APPSUBTYPE=";
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("OnClick", "javascript:return OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "CEH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "CEH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.CQTN + "&APPSUBTYPE=" + "');");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("href", "javascript:void(0);");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Text = DataBinder.Eval(e.Row.DataItem, "CEH_NO").ToString();
                    }
                }
                else if ((sender as GridView).ID == "grdTaxDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("imbTaxRemove") as ImageButton).Visible = (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE);

                        if (QtnStatus == 11 || QtnStatus == 17)
                        {
                            (e.Row.FindControl("imbTaxRemove") as ImageButton).Visible = false;
                        }
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
                //GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                //SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                EntryStatus = EntryStatus.LISTMODE;
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
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaleOrder.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);

            this.lbnList.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnEnquiry.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnQuotation.PreRender += new EventHandler(btnAction_PreRender);

            this.btnApply.PreRender += new EventHandler(btnAction_PreRender);
            this.imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);

            //Load Event
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSaleOrder.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);

            this.lbnList.Load += new EventHandler(btnAction_Load);
            this.lbnEnquiry.Load += new EventHandler(btnAction_Load);
            this.lbnQuotation.Load += new EventHandler(btnAction_Load);

            this.btnApply.Load += new EventHandler(btnAction_Load);
            this.imgPopupAdd.Load += new EventHandler(btnAction_Load);
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
                    //case NavigationEnum.PAGECHANGE:
                    //    uclPaging.CurrentPage = e.CurrentPage;
                    //    break;
                    //case NavigationEnum.FIRST:
                    //    if (e.CurrentPage > 1)
                    //        uclPaging.CurrentPage = 1;
                    //    break;
                    //case NavigationEnum.LAST:
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPaging.CurrentPage = e.TotalPages;
                    //    break;
                    //case NavigationEnum.NEXT:
                    //    // increment the current page index.
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPaging.CurrentPage++;
                    //    break;
                    //case NavigationEnum.PREVIOUS:
                    //    // Decrement the current page index.
                    //    if (e.CurrentPage > 1)
                    //        uclPaging.CurrentPage--;
                    //    break;


                }

                //PageIndex = uclPaging.CurrentPage.ToString();
                // Change Code As per the page
                //GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                //SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                EntryStatus = EntryStatus.LISTMODE;
                //============================
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //    uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            //    uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            //    uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;

            //    uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                if (QuotationHeaderSession != null)
                {
                    hdfHasTax.Value = ((QuotationHeaderSession.TaxHdr == null || QuotationHeaderSession.TaxHdr.Count == 0)
                        && QuotationHeaderSession.QuotationDtl.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0)))
                        ? CommonConstants.SELECT_VALUE_ZERO : CommonConstants.SELECT_VALUE_ONE;
                }
                btnSaleOrder.Visible = QtnStatus == (int)WkfStatusEnqEnum.APPROVED;
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

                //if (QtnStatus == 12 && BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId()))
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSaleOrder1", "ShowSaleOrder();", true);
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
            VENDOR,
            QUOTATION,
            QUOTATIONHEADER,
            QUOTATIONHEADERTOP,
            QUOTATIONDETAIL,
            QUOTATIONTAXTYPES,
            QUOTATIONTAXPOPUPGRID,
            QUOTATIONTAXHEADER,
            EXCHANGERATE,
            CUSTOMERSELECTED,
            ITEMRATES,
            ITEMDETAILS,
            REVISIONHISTORY,
            CUSTOMERADDRESS,
            TRANSHIPMENT,
            SHIPBY,
            DELIVERYTERMS,
            PAYMENTTERMS,
            SPECIALCAUSE,
            TAXSETTINGS,
            CUSTOMTAXSETTINGS

        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        private enum WkfStatusEnqEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }
        #endregion
    }
}