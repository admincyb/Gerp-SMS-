using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject;
using ERPManager;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.PurchaseOrderManagement;
using System.Xml;
using System.Text;
using System.Threading;
using BusinessObject.CommonManagement;
using ERPData;
using ERPService;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class RFQResponse : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
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
        private int CompanyPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CustomerPK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CustomerPK] = value;
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
        ///// To maintain keep RFQ Tax Splitting
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
        ///// To maintain keep RFQ Tax Splitting
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
        /// To maintain keep RFQ Tax Splitting
        /// </summary>
        private RFQResponseHeader RFQResponseHeaderSession
        {
            get
            {
                return (RFQResponseHeader)Session[ERP.Utilities.SessionStrings.RFQResponseHeader];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.RFQResponseHeader] = value;
            }
        }

        /// <summary>
        /// To maintain keep RFQ Tax Splitting
        /// </summary>
        private RFQResponseHeader TempRFQResponseHeaderSession
        {
            get
            {
                return (RFQResponseHeader)this.ViewState[ViewstateStrings.TempRFQResponseHeader];
                //return (RFQResponseHeader)Session[ERP.Utilities.SessionStrings.TempRFQResponseHeader];
            }
            set
            {
                this.ViewState[ViewstateStrings.TempRFQResponseHeader] = value;
                //Session[ERP.Utilities.SessionStrings.TempRFQResponseHeader] = value;
            }
        }

        /// <summary>
        /// Response PK
        /// </summary>
        private int SelectedResponsePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedResponsePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedResponsePK] = value;
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


        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        DataTable dtCurrentCompany;

        private RFQResponseHeader rfqResponseHeaderObj;
        private RFQResponseDetails rfqResponseDetailsObj;
        private RFQTaxHdr rfqTaxHdrObj;
        //private RFQTaxDtl rfqTaxDtlObj;
        List<RFQResponseDetails> rfqResponseDetailsList;
        //List<RFQTaxDtl> rfqTaxDtlList;
        //RFQTaxSplit rfqDtlSplitObj;
        List<RFQTaxHdr> rfqTaxHdrList;
        RFQResponseDetails rfqResponseDtlObj;
        string selectedVendor;
        DataSet dsRFQHeader;
        DataTable dtRFQTaxDetails;
        DataSet dsVendor;

        bool hasValidRate;

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
                if (!IsPostBack)
                {
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalFormat.Value = "#0.";                    
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfRateDigits.Value = "#0.";
                    int rate = Session[ERP.Utilities.SessionStrings.RateDecimalDigit] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit].ToString()) : 3;
                    for (int i = 0; i < rate; i++)
                    {
                        hdfRateDigits.Value += "0";
                    }

                    Session[ERP.Utilities.SessionStrings.RFQResponseHeader] = null;
                    if (Session[ERP.Utilities.SessionStrings.RFQPK] != null)
                    {
                        CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RFQPK]);
                        GetFieldValues(ControlsEnum.RFQHEADER);
                        SetFieldValues(ControlsEnum.RFQHEADERTOP);
                        GetFieldValues(ControlsEnum.VENDOR);
                        if (Session[ERP.Utilities.SessionStrings.RFQVendor] == null)
                        {
                            ddlVendor.Enabled = true;
                        }
                        else
                        {
                            selectedVendor = Session[ERP.Utilities.SessionStrings.RFQVendor].ToString();
                            ddlVendor.Enabled = false;
                        }
                        SetFieldValues(ControlsEnum.VENDOR);
                        GetFieldValues(ControlsEnum.RFQ);
                        SetFieldValues(ControlsEnum.RFQHEADER);
                        SetFieldValues(ControlsEnum.RFQDETAIL);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        SetSubTotal();
                    }
                    else
                    {
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQListing), false);
                    }
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
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
            DataSet dsRFQTaxDetails;
            AdmCompanyMstService admCompanyMstServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.RFQHEADER:
                        dsRFQHeader = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQHeader(CurrPK, Convert.ToByte(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;
                    case ControlsEnum.VENDOR:
                        dsVendor = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetVendorList(CurrPK);
                        break;
                    case ControlsEnum.RFQ:
                        VendorPK = Convert.ToInt32(ddlVendor.SelectedValue);
                        rfqResponseHeaderObj = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQResponse(CurrPK, VendorPK);
                        RFQResponseHeaderSession = rfqResponseHeaderObj;
                        if (rfqResponseHeaderObj == null && Convert.ToInt32(CurrPK) != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "','RFQResponse.aspx');", true);
                        }
                        break;
                    case ControlsEnum.RFQTAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsRFQTaxDetails = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK), 0);
                            if (dsRFQTaxDetails != null && dsRFQTaxDetails.Tables.Count > 0)
                            {
                                dtRFQTaxDetails = dsRFQTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Tax == category)
                            {
                                dtRFQTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtResponseDate.Text), 0, TaxFilterType.PUR);
                            }
                            else
                            {
                                dtRFQTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtResponseDate.Text), 0);
                            }
                        }
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtResponseDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "1";
                        }
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //dtCurrentCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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
                    case ControlsEnum.RFQHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.RFQDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.RFQHEADERTOP:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.RFQTAXTYPES:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.RFQTAXPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
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
            HiddenField hdfRRDPK;
            HiddenField hdfItemPK;
            HiddenField hdfUoM;
            //HiddenField hdfCurrency;
            HiddenField hdfRFQDtlPK;
            Label lblQuantity;
            TextBox txtRate;
            TextBox txtAmount;
            TextBox txtDiscount;
            TextBox txtTax;
            TextBox txtTotal;
            TextBox txtSubTotal;
            Label lblSpecifications;
            List<RFQTaxHdr> rfqTaxHeaderList;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.RFQHEADER:
                        rfqResponseHeaderObj.RRH_PK = string.IsNullOrEmpty(hdfResponsePK.Value) ? 0 : Convert.ToInt32(hdfResponsePK.Value);
                        rfqResponseHeaderObj.RRH_NO = rfqResponseHeaderObj.RRH_PK.ToString();
                        rfqResponseHeaderObj.RRH_VERSION = 1;
                        rfqResponseHeaderObj.RRH_DATE = string.IsNullOrEmpty(txtResponseDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtResponseDate.Text.Trim();
                        rfqResponseHeaderObj.RRH_RFQ_HDR = CurrPK;
                        rfqResponseHeaderObj.RRH_VENDOR = Convert.ToInt32(ddlVendor.SelectedValue);
                        rfqResponseHeaderObj.RRH_VEN_REF_NO = HttpUtility.HtmlEncode(txtVendorRef.Text.Trim());
                        rfqResponseHeaderObj.RRH_STATUS = 0;
                        rfqResponseHeaderObj.RRH_AMT_DISC = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                        rfqResponseHeaderObj.RRH_AMT_TAX = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                        rfqResponseHeaderObj.RRH_AMT_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                        rfqResponseHeaderObj.RRH_AMT_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                        rfqResponseHeaderObj.RRH_AMT_NET_TOTAL = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                        rfqResponseHeaderObj.RRH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        rfqResponseHeaderObj.RRH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                        rfqResponseHeaderObj.RRH_AMT_NET_TOTAL_BC = rfqResponseHeaderObj.RRH_AMT_NET_TOTAL * rfqResponseHeaderObj.RRH_EXCHG_RATE;
                        rfqResponseHeaderObj.RRH_PAYMENT_TERMS = string.IsNullOrEmpty(txtPaymentTerms.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtPaymentTerms.Text.Trim());
                        rfqResponseHeaderObj.RRH_DELIVERY_TERMS = string.IsNullOrEmpty(txtDeliveryTerms.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtDeliveryTerms.Text.Trim());
                        rfqResponseHeaderObj.RRH_OTHER_DETAILS = string.IsNullOrEmpty(txtOtherDetails.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtOtherDetails.Text.Trim());
                        rfqResponseHeaderObj.RRH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        rfqResponseHeaderObj.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                        rfqResponseHeaderObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        rfqResponseHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        rfqResponseHeaderObj.LAST_MOD_DT = DateTime.Now;

                        rfqResponseHeaderObj.RRH_COMPANY = ddlCompany.SelectedItem.Value;
                        CompanyPK = Convert.ToInt32(rfqResponseHeaderObj.RRH_COMPANY);

                        List<RFQResponseDetails> rfqResponsDetailsList = new List<RFQResponseDetails>();
                        rfqResponsDetailsList = (List<RFQResponseDetails>)SetUIValuesToObject(ControlsEnum.RFQDETAIL);
                        if (rfqResponsDetailsList != null && rfqResponsDetailsList.Count > 0)
                        {
                            rfqResponseHeaderObj.ResponseDtl = new List<RFQResponseDetails>();
                            rfqResponsDetailsList.ForEach(dtl => rfqResponseHeaderObj.ResponseDtl.Add(dtl));
                        }
                        if (RFQResponseHeaderSession != null)
                        {
                            rfqTaxHeaderList = new List<RFQTaxHdr>();
                            rfqTaxHeaderList = RFQResponseHeaderSession.TaxHdr.ToList();
                            if (rfqTaxHeaderList != null && rfqTaxHeaderList.Count > 0)
                            {
                                rfqResponseHeaderObj.TaxHdr = new List<RFQTaxHdr>();
                                rfqTaxHeaderList.ForEach(dtl => rfqResponseHeaderObj.TaxHdr.Add(dtl));
                            }
                        }

                        rfqResponseHeaderObj.RRH_TOTAL_QTY = 0;//Dummy
                        txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                        rfqResponseHeaderObj.RRH_AMT_SUB_TOTAL = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());
                        //rfqResponseHeaderObj.RRH_CURRENCY = string.IsNullOrEmpty(hdfHdrCurrency.Value)?0:Convert.ToInt32(hdfHdrCurrency.Value);
                        rfqResponseHeaderObj.RRH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);
                        retObject = rfqResponseHeaderObj;
                        break;
                    case ControlsEnum.RFQDETAIL:
                        rowID = 0;
                        rfqResponsDetailsList = new List<RFQResponseDetails>();
                        foreach (GridViewRow grdrow in grdRFQResponse.Rows)
                        {
                            rfqResponseDetailsObj = new RFQResponseDetails();
                            hdfRRDPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfRRDPK");
                            rfqResponseDetailsObj.RRD_PK = hdfRRDPK == null ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                            hdfRFQDtlPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfRFQDtlPK");
                            rfqResponseDetailsObj.RRD_RFQ_DTL = hdfRFQDtlPK == null ? 0 : Convert.ToInt32(hdfRFQDtlPK.Value);
                            rfqResponseDetailsObj.RRD_SL_NO = rowID + 1;
                            hdfItemPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfItemPK");
                            rfqResponseDetailsObj.RRD_ITEM = hdfItemPK == null ? 0 : Convert.ToInt32(hdfItemPK.Value);
                            lblSpecifications = (Label)grdRFQResponse.Rows[rowID].FindControl("lblSpecifications");
                            rfqResponseDetailsObj.RRD_ITEM_SPEC = lblSpecifications == null ? string.Empty : string.IsNullOrEmpty(lblSpecifications.Text) ? string.Empty : lblSpecifications.Text.Trim();
                            lblQuantity = (Label)grdRFQResponse.Rows[rowID].FindControl("lblQuantity");
                            rfqResponseDetailsObj.RRD_QTY_REQUESTED = lblQuantity == null ? 0 : string.IsNullOrEmpty(lblQuantity.Text) ? 0 : Convert.ToDouble(lblQuantity.Text.Trim().Replace(",", ""));
                            hdfUoM = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfUoM");
                            rfqResponseDetailsObj.RRD_UOM = hdfUoM == null ? 0 : Convert.ToInt32(hdfUoM.Value);
                            txtRate = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtRate");
                            rfqResponseDetailsObj.RRD_RATE = txtRate == null ? 0 : string.IsNullOrEmpty(txtRate.Text.Trim()) ? 0 : Convert.ToDouble(txtRate.Text.Trim());
                            hasValidRate = rfqResponseDetailsObj.RRD_RATE > 0 ? true : false;
                            txtAmount = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtAmount");
                            rfqResponseDetailsObj.RRD_AMOUNT = txtAmount == null ? 0 : string.IsNullOrEmpty(txtAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtAmount.Text.Trim());
                            txtDiscount = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtDiscount");
                            rfqResponseDetailsObj.RRD_AMT_DISC = txtDiscount == null ? 0 : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtDiscount.Text.Trim());
                            txtTax = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtTax");
                            rfqResponseDetailsObj.RRD_AMT_TAX = txtTax == null ? 0 : string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 : Convert.ToDouble(txtTax.Text.Trim());
                            txtTotal = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtTotal");
                            rfqResponseDetailsObj.RRD_AMT_NET_TOTAL = txtTotal == null ? 0 : string.IsNullOrEmpty(txtTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtTotal.Text.Trim());
                            //hdfCurrency = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfCurrency");
                            //rfqResponseDetailsObj.RRD_CURRENCY =  hdfCurrency == null ? 0 : Convert.ToInt32(hdfCurrency.Value);
                            //rfqResponseDetailsObj.RRD_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);
                            if (RFQResponseHeaderSession != null)
                            {
                                rfqTaxHeaderList = new List<RFQTaxHdr>();
                                RFQResponseDetails tempResponseDetailsObj = RFQResponseHeaderSession.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == rfqResponseDetailsObj.RRD_PK && rfq.RRD_ITEM == rfqResponseDetailsObj.RRD_ITEM);
                                if (tempResponseDetailsObj != null)
                                {
                                    rfqTaxHeaderList = tempResponseDetailsObj.TaxDtl.ToList();
                                    if (rfqTaxHeaderList != null && rfqTaxHeaderList.Count > 0)
                                    {
                                        rfqTaxHeaderList.ForEach(dtl => dtl.RTD_SL_NO = rfqResponseDetailsObj.RRD_SL_NO);
                                        rfqResponseDetailsObj.TaxDtl = new List<RFQTaxHdr>();
                                        rfqTaxHeaderList.ForEach(dtl => rfqResponseDetailsObj.TaxDtl.Add(dtl));
                                    }
                                }
                            }
                            //hdfHdrCurrency.Value=rfqResponseDetailsObj.RRD_CURRENCY.ToString();
                            rfqResponsDetailsList.Add(rfqResponseDetailsObj);
                            //txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtTotal");
                            //rfqResponseHeaderObj.RRH_AMT_SUB_TOTAL = txtSubTotal == null ? 0 : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtSubTotal.Text.Trim());
                            rowID++;
                        }

                        retObject = rfqResponsDetailsList;
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
                    case ControlsEnum.RFQHEADERTOP:
                        if (dsRFQHeader != null && dsRFQHeader.Tables[0].Rows.Count > 0)
                        {
                            lblRFQNo.Text = dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHNo].ToString();
                            hdfRFQNo.Value = HttpUtility.HtmlDecode(dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHNo].ToString());
                            lblRFQDate.Text = Convert.ToDateTime(dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHDate]).ToString(Resources.Constants.DateFormatShort);
                            hdfRFQDate.Value = dsRFQHeader.Tables[0].Rows[0][Resources.DataFieldRes.RFHDate].ToString();
                            hdfCompanyPk.Value = dsRFQHeader.Tables[0].Rows[0][GetLocalResourceObject("CompanyPk").ToString()].ToString() != string.Empty ?
                                dsRFQHeader.Tables[0].Rows[0][GetLocalResourceObject("CompanyPk").ToString()].ToString() :
                                currentUser.SBUID.ToString();

                        }
                        break;
                    case ControlsEnum.RFQHEADER:
                        if (rfqResponseHeaderObj != null)
                        {
                            hdfResponsePK.Value = rfqResponseHeaderObj.RRH_PK.ToString();
                            txtResponseDate.Text = rfqResponseHeaderObj.RRH_DATE;
                            txtVendorRef.Text = HttpUtility.HtmlDecode(rfqResponseHeaderObj.RRH_VEN_REF_NO);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(rfqResponseHeaderObj.RRH_PAYMENT_TERMS);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(rfqResponseHeaderObj.RRH_DELIVERY_TERMS);
                            txtOtherDetails.Text = HttpUtility.HtmlDecode(rfqResponseHeaderObj.RRH_OTHER_DETAILS);
                            txtHdrDiscount.Text = rfqResponseHeaderObj.RRH_AMT_DISC.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.Text = rfqResponseHeaderObj.RRH_AMT_TAX.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = rfqResponseHeaderObj.RRH_AMT_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = rfqResponseHeaderObj.RRH_AMT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrTotal.Text = rfqResponseHeaderObj.RRH_AMT_NET_TOTAL.ToString(hdfCurrencyFormat.Value);
                            //txtHdrTotal.Text = Math.Round(decimal.Parse(txtHdrTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtPriceAdj.Text = Math.Round(decimal.Parse(txtPriceAdj.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtHdrTotal.Text = Math.Round(decimal.Parse(txtHdrTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            hdfExchangeRate.Value = rfqResponseHeaderObj.RRH_EXCHG_RATE.ToString();
                            hdfCurrency.Value = rfqResponseHeaderObj.RRH_CURRENCY.ToString();
                            txtCurrency.Text = rfqResponseHeaderObj.RRH_CURRENCY_TEXT;
                            if (rfqResponseHeaderObj.RRH_COMPANY != null)
                            {
                                hdfCompanyPk.Value = rfqResponseHeaderObj.RRH_COMPANY.ToString();
                                GetFieldValues(ControlsEnum.COMPANY);
                                SetFieldValues(ControlsEnum.COMPANY);
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
                    ddlVendor.Items.Clear();
                    if (dsVendor != null && dsVendor.Tables[0].Rows.Count > 0)
                    {
                        ddlVendor.DataSource = dsVendor.Tables[0].DataSet;
                        ddlVendor.DataTextField = Resources.DataFieldRes.RFQResponseVendorText;
                        ddlVendor.DataValueField = Resources.DataFieldRes.RFQResponseVendorPK;
                        ddlVendor.DataBind();
                    }
                    ddlVendor.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    if (selectedVendor != null)
                    {
                        ddlVendor.SelectedValue = selectedVendor.ToString();
                    }
                    break;
                case ControlsEnum.RFQTAXTYPES:
                    //Bind Tax dropdown
                    ddlPopupTaxType.Items.Clear();
                    if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count > 0)
                    {
                        ddlPopupTaxType.DataSource = dtRFQTaxDetails.DataSet;
                        ddlPopupTaxType.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlPopupTaxType.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlPopupTaxType.DataBind();
                    }
                    ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    break;
                default:
                    break;
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPK.ToString()));
                    }
                    break;
                #endregion
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
                    case ControlsEnum.RFQDETAIL:
                        if (rfqResponseHeaderObj != null)
                        {
                            rfqResponseDetailsList = new List<RFQResponseDetails>();
                            rfqResponseDetailsList = rfqResponseHeaderObj.ResponseDtl;
                            if (rfqResponseDetailsList != null)
                            {
                                ConfigurationSettings();
                                grdRFQResponse.DataSource = rfqResponseDetailsList;
                                grdRFQResponse.DataBind();
                            }
                        }
                        break;
                    case ControlsEnum.RFQTAXPOPUPGRID:

                        if (IsHeaderTax)
                        {
                            rfqTaxHdrList = TempRFQResponseHeaderSession.TaxHdr.Where(tax => tax.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            rfqResponseDtlObj = TempRFQResponseHeaderSession.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                            if (rfqResponseDtlObj != null)
                            {
                                rfqTaxHdrList = rfqResponseDtlObj.TaxDtl.Where(tax => tax.RTD_RESP_DTL == SelectedResponsePK && tax.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        grdTaxDetails.DataSource = rfqTaxHdrList;
                        grdTaxDetails.DataBind();
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.RFQTAXPOPUPGRID:
                    txtPopupAmount.Text = string.Empty;
                    txtPopupItemAmount.Text = string.Empty;
                    txtPopupOther.Text = string.Empty;
                    TaxPK = 0;
                    grdTaxDetails.DataSource = null;
                    grdTaxDetails.DataBind();
                    TempRFQResponseHeaderSession = null;
                    hdfTaxFormula.Value = string.Empty;
                    SelectedResponsePK = 0;
                    SelectedItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break;
                case ControlsEnum.RFQHEADER:
                    CurrPK = 0;
                    CompanyPK = 0;
                    break;
            }
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateDigits.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public bool GetTaxConfiguration()
        {
            bool istax = false;
            istax = hdfIsTax.Value == "1" ? true : false;
            return istax;
        }

        public bool GetDiscountConfiguration()
        {
            bool isDiscount = false;
            isDiscount = hdfIsDiscount.Value == "1" ? true : false;
            return isDiscount;
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
            Label lblQuantity;

            HiddenField hdfRRDPK;
            HiddenField hdfItemPK;

            double quantity;
            double rate;
            quantity = 0;
            rate = 0;


            if (sender != null)
            {
                txtRate = sender as TextBox;
                lblQuantity = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblQuantity") as Label);
                txtAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                if (txtRate != null && lblQuantity != null)
                {
                    double.TryParse(txtRate.Text, out rate);
                    double.TryParse(lblQuantity.Text, out quantity);
                    //if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(lblQuantity.Text, out rate) && quantity > 0 && rate > 0)
                    //{
                        if (txtAmount != null)
                        {
                            txtAmount.Text = Math.Round((rate * quantity), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                            txtAmount.Text = Math.Round(decimal.Parse(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                            //txtAmount.Text = String.Format("{0:c}", txtAmount.Text);


                            txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
                            txtTotal = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTotal") as TextBox);

                            hdfRRDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfRRDPK") as HiddenField);
                            hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            if (hdfRRDPK != null && hdfItemPK != null)
                            {
                                SelectedResponsePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                SetDetailTax(txtAmount, txtDiscount, txtTax, txtTotal);
                            }
                        }
                   // }
                    //else
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                    //}
                }
            }
            else
            {
                RFQResponseHeaderSession = TempRFQResponseHeaderSession;
                foreach (GridViewRow gvr in grdRFQResponse.Rows)
                {
                    if (gvr.RowType == DataControlRowType.DataRow)
                    {
                        hdfRRDPK = (gvr.FindControl("hdfRRDPK") as HiddenField);
                        hdfItemPK = (gvr.FindControl("hdfItemPK") as HiddenField);
                        if (SelectedResponsePK == Convert.ToInt32(hdfRRDPK.Value) && SelectedItemPK == Convert.ToInt32(hdfItemPK.Value))
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
                    if (RFQResponseHeaderSession != null)
                    {
                        rfqResponseHeaderObj = RFQResponseHeaderSession;
                        rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                        if (rfqResponseDetailsObj != null)
                        {
                            var discDetail = rfqResponseDetailsObj.TaxDtl.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (RFQTaxHdr rfqTaxHdrObj in discDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            discount = rfqResponseDetailsObj.TaxDtl.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.RTD_TAX_AMT);
                            netAmount = amount - discount;
                            txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);

                            var taxDetail = rfqResponseDetailsObj.TaxDtl.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (RFQTaxHdr rfqTaxHdrObj in taxDetail)
                            {
                                string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                    rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            itmTax = rfqResponseDetailsObj.TaxDtl.ToList().Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.RTD_TAX_AMT);
                            txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                            rfqResponseDetailsObj.RRD_AMOUNT = amount;
                            rfqResponseDetailsObj.RRD_AMT_DISC = discount;
                            rfqResponseDetailsObj.RRD_AMT_TAX = itmTax;
                            rfqResponseDetailsObj.RRD_AMT_NET_TOTAL = (amount - discount + itmTax);
                            txtTotal.Text = rfqResponseDetailsObj.RRD_AMT_NET_TOTAL.ToString(hdfCurrencyFormat.Value);
                            //txtTotal.Text = Math.Round(decimal.Parse(txtTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);


                            RFQResponseHeaderSession = rfqResponseHeaderObj;
                        }
                    }
                    return true;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
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
            txtSubTotalFooter = grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
            if (txtSubTotalFooter != null)
            {
                RFQResponseHeaderSession.RRH_AMT_SUB_TOTAL = RFQResponseHeaderSession.ResponseDtl.Sum(dtl => dtl.RRD_AMT_NET_TOTAL);
                txtSubTotalFooter.Text = RFQResponseHeaderSession.RRH_AMT_SUB_TOTAL.ToString(hdfCurrencyFormat.Value);
                txtSubTotalFooter.ToolTip = RFQResponseHeaderSession.RRH_AMT_SUB_TOTAL.ToString(hdfCurrencyFormat.Value);
                //txtSubTotalFooter.Text = String.Format("{0:000}", decimal.Parse(txtSubTotalFooter.Text));

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

            //txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
            //Double.TryParse(txtSubTotal.Text.Trim(), out amount);
            //rfqResponseHeaderObj.RRH_AMT_SUB_TOTAL = amount;
            //if (amount >= 0)
            //{
            if (RFQResponseHeaderSession != null)
            {
                rfqResponseHeaderObj = RFQResponseHeaderSession;
                amount = rfqResponseHeaderObj.RRH_AMT_SUB_TOTAL;
                discount = 0;
                var discHeader = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount));
                foreach (RFQTaxHdr rfqTaxHdrObj in discHeader)
                {
                    string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {
                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    }
                }
                discount = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.RTD_TAX_AMT);
                rfqResponseHeaderObj.RRH_AMT_DISC = discount;
                txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;

                var taxHeader = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax));
                foreach (RFQTaxHdr rfqTaxHdrObj in taxHeader)
                {
                    string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                    if (!string.IsNullOrEmpty(taxFormula))
                    {
                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    }
                }
                rfqResponseHeaderObj.RRH_AMT_TAX = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.RTD_TAX_AMT);
                txtHdrTax.Text = rfqResponseHeaderObj.RRH_AMT_TAX.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtShipping.Text, out shipping);
                rfqResponseHeaderObj.RRH_AMT_SHIP_CHARGE = shipping;
                double.TryParse(txtPriceAdj.Text, out adjust);
                rfqResponseHeaderObj.RRH_AMT_ADJUST = adjust;
                rfqResponseHeaderObj.RRH_AMT_NET_TOTAL = rfqResponseHeaderObj.RRH_AMT_SUB_TOTAL - rfqResponseHeaderObj.RRH_AMT_DISC + rfqResponseHeaderObj.RRH_AMT_TAX
                    + rfqResponseHeaderObj.RRH_AMT_SHIP_CHARGE + rfqResponseHeaderObj.RRH_AMT_ADJUST;
                txtHdrTotal.Text = rfqResponseHeaderObj.RRH_AMT_NET_TOTAL.ToString(hdfCurrencyFormat.Value);
                RFQResponseHeaderSession = rfqResponseHeaderObj;
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
        private bool IsValidDiscount(int type, int itm)
        {
            double curDisc = Convert.ToDouble(txtPopupAmount.Text.Trim());
            // double totDisc=
            return false;
        }

        private void ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("PURCHASE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfIsTax.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString();
                hdfIsDiscount.Value = dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString();
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                string itemAmount;
                //List<RFQTaxSplit> tempRfqTaxDtlSplit;
                //RFQTaxSplit tempRFQTaxSplitObj;
                List<RFQTaxHdr> tempRfqTaxHdrSplit;
                RFQTaxHdr tempRFQTaxSplitObj = null;
                HiddenField hdfRRDPK;
                HiddenField hdfItemPK;
                TextBox txtAmount;
                TextBox txtTax;
                TextBox txtDiscount;
                TextBox txtSubTotal;

                double amount;
                double discount;
                int count;

                bool isValidDisc = true;
                double totalAmt = 0;
                double currentTotal = 0;
                double taxAmt = 0;

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
                    if (((DropDownList)sender).ID == "ddlVendor")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlCompany")
                    {
                        commonActions = ActionsEnum.CMPYSELECTEDINDEXCHANGED;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtRate")
                    {
                        commonActions = ActionsEnum.CALCULATEDTLTAX;
                    }
                }
                //if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                //{
                //    if (((TextBox)sender).ID == "txtHdrDiscount" || ((TextBox)sender).ID == "txtSubTotalFooter")
                //    {
                //        commonActions = ActionsEnum.CALCULATEHDRTAX;
                //    }
                //}
                switch (commonActions)
                {
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
                            rfqResponseHeaderObj = new RFQResponseHeader();
                            rfqResponseHeaderObj = (RFQResponseHeader)SetUIValuesToObject(ControlsEnum.RFQHEADER);
                            //if (hasValidRate)
                            //{
                                if (rfqResponseHeaderObj != null && rfqResponseHeaderObj.ResponseDtl != null)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<RFQResponseHeader>(rfqResponseHeaderObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                    // save Process Control inspection details
                                    result = BusinessLogic.PurchaseOrderManagement.RequestForQuote.SaveRFQResponseDetails(xmlDoc);
                                    if (result > 0) // Success !  redirect to listing page
                                    {
                                        // Show Save Message and redired to listing page
                                        //string routeURL = "RFQResponse.aspx";
                                        GetFieldValues(ControlsEnum.RFQ);
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQResponse);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.RFQResponse) + "');", true);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Response_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Empty_Rate").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                            //}
                            hasValidRate = false;
                        }
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        txtCurrency.Text = string.Empty;
                        hdfCurrency.Value = string.Empty;
                        if (Convert.ToInt32(ddlVendor.SelectedValue) >= 0)
                        {
                            GetFieldValues(ControlsEnum.RFQ);
                            SetFieldValues(ControlsEnum.RFQHEADER);
                            SetFieldValues(ControlsEnum.RFQDETAIL);
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        SetSubTotal();
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.CMPYSELECTEDINDEXCHANGED:
                        CompanyPK = Convert.ToInt32(ddlCompany.SelectedItem.Value);
                        break;
                    #endregion
                    #region RFQTAXDETAILS
                    case ActionsEnum.RFQTAXDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        {
                            txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                            txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            hdfRRDPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfRRDPK") as HiddenField);
                            hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            if (txtAmount != null && hdfRRDPK != null && txtDiscount != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text.Trim()).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                                SelectedResponsePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                if (RFQResponseHeaderSession != null)
                                {
                                    TempRFQResponseHeaderSession = RFQResponseHeaderSession;
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);
                                    GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                    SetFieldValues(ControlsEnum.RFQTAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                            TaxPK = 0;
                                            if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtRFQTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                                SelectedTaxText = dtRFQTaxDetails.Rows[0]["TAX_HEAD"].ToString();
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
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region RFQDISCDETAILS
                    case ActionsEnum.RFQDISCDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        {
                            txtAmount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                            txtDiscount = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                            hdfRRDPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfRRDPK") as HiddenField);
                            hdfItemPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                            if (txtAmount != null && hdfRRDPK != null && txtDiscount != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text.Trim()).ToString(hdfCurrencyFormat.Value);
                                SelectedResponsePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                                if (RFQResponseHeaderSession != null)
                                {
                                    TempRFQResponseHeaderSession = RFQResponseHeaderSession;
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);
                                    GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                    SetFieldValues(ControlsEnum.RFQTAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                            TaxPK = 0;
                                            if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtRFQTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                                SelectedTaxText = dtRFQTaxDetails.Rows[0]["TAX_HEAD"].ToString();
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
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region RFQTAXHEADER
                    case ActionsEnum.RFQTAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        {
                            if (RFQResponseHeaderSession != null)
                            {
                                TempRFQResponseHeaderSession = RFQResponseHeaderSession;
                                IsHeaderTax = true;
                                SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                SetFieldValues(ControlsEnum.RFQTAXTYPES);
                                txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                                if (txtSubTotal != null)
                                {
                                    txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? Convert.ToDouble(txtSubTotal.Text.Trim()).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtSubTotal.Text.Trim()) - Convert.ToDouble(txtHdrDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                            TaxPK = 0;
                                            if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtRFQTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                                SelectedTaxText = dtRFQTaxDetails.Rows[0]["TAX_HEAD"].ToString();
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
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region RFQDISCHEADER
                    case ActionsEnum.RFQDISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (ddlVendor.Items.Count > 1 && Convert.ToInt32(ddlVendor.SelectedValue) > 0)
                        {
                            if (RFQResponseHeaderSession != null)
                            {
                                TempRFQResponseHeaderSession = RFQResponseHeaderSession;
                                IsHeaderTax = true;
                                SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                SetFieldValues(ControlsEnum.RFQTAXTYPES);
                                txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                                if (txtSubTotal != null)
                                {
                                    txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtSubTotal.Text.Trim()).ToString(hdfCurrencyFormat.Value);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                            TaxPK = 0;
                                            if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtRFQTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                                SelectedTaxText = dtRFQTaxDetails.Rows[0]["TAX_HEAD"].ToString();
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
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        //int category = 1;
                        //int.TryParse(hdfTaxCategory.Value, out category);
                        //if (TempRFQResponseHeaderSession != null)
                        //{
                        //    RFQResponseHeaderSession = TempRFQResponseHeaderSession;
                        //    rfqResponseHeaderObj = RFQResponseHeaderSession;
                        //    double taxAmount;
                        //    if (IsHeaderTax)
                        //    {
                        //        taxAmount = rfqResponseHeaderObj.TaxHdr.ToList().Where(rfq => rfq.RTD_TAX_CATEGORY == category).Sum(rfq => rfq.RTD_TAX_AMT);
                        //        if (category == ((int)TaxType.Discount))
                        //            txtHdrDiscount.Text = taxAmount.ToString();
                        //        else
                        //            txtHdrTax.Text = taxAmount.ToString();
                        //    }
                        //    else
                        //    {
                        //        rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                        //        if (rfqResponseDetailsObj != null)
                        //        {
                        //            taxAmount = rfqResponseDetailsObj.TaxDtl.ToList().Where(rfq => rfq.RTD_TAX_CATEGORY == category).Sum(rfq => rfq.RTD_TAX_AMT);
                        //            int rowID = 0;
                        //            foreach (GridViewRow grdrow in grdRFQResponse.Rows)
                        //            {
                        //                hdfRRDPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfRRDPK");
                        //                hdfItemPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfItemPK");
                        //                if (hdfRRDPK != null && hdfItemPK != null && Convert.ToInt32(hdfRRDPK.Value) == SelectedResponsePK && Convert.ToInt32(hdfItemPK.Value) == SelectedItemPK)
                        //                {
                        //                    if (category == ((int)TaxType.Discount))
                        //                        txtTax = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtDiscount");
                        //                    else
                        //                        txtTax = (TextBox)grdRFQResponse.Rows[rowID].FindControl("txtTax");
                        //                    if (txtTax != null)
                        //                    {
                        //                        txtTax.Text = taxAmount.ToString();
                        //                    }
                        //                }
                        //                rowID++;
                        //            }
                        //        }
                        //    }
                        //}
                        SetDetailTax(null);
                        SetHdrTax();
                        ResetForm(ControlsEnum.RFQTAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:
                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempRFQResponseHeaderSession != null)
                        {
                            rfqResponseHeaderObj = TempRFQResponseHeaderSession;
                            tempRFQTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempRFQTaxSplitObj = rfqResponseHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.RTD_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempRFQTaxSplitObj = rfqResponseHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.RTD_NAME == txtPopupOther.Text.Trim() && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                                if (rfqResponseDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempRFQTaxSplitObj = rfqResponseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.RTD_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempRFQTaxSplitObj = rfqResponseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.RTD_NAME == txtPopupOther.Text.Trim() && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempRFQTaxSplitObj == null)
                            {
                                rfqTaxHdrList = new List<RFQTaxHdr>();
                                rfqTaxHdrObj = new RFQTaxHdr();
                                try
                                {
                                    if (Convert.ToDouble(txtPopupItemAmount.Text.Trim()) < Convert.ToDouble(txtPopupAmount.Text.Trim()))
                                    {
                                        errorTaxAmount = true;
                                    }
                                    rfqTaxHdrObj.RTD_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    rfqTaxHdrObj.RTD_RESP_DTL = SelectedResponsePK;
                                    rfqTaxHdrObj.RTD_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        rfqTaxHdrObj.RTD_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    rfqTaxHdrObj.RTD_TAX_TEXT = SelectedTaxText;
                                    rfqTaxHdrObj.RTD_NAME = txtPopupOther.Text.Trim();
                                    rfqTaxHdrObj.RTD_PK = 0;
                                    //rfqTaxHdrObj.RTD_TAX_CATEGORY_TEXT = "Tax";
                                    rfqTaxHdrObj.RTD_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    rfqTaxHdrObj.RTD_TYPE = 1;
                                    rfqTaxHdrObj.RTD_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (rfqTaxHdrObj.RTD_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(rfqResponseHeaderObj.RRH_AMT_NET_TOTAL);
                                            currentTotal = rfqResponseHeaderObj.TaxHdr.Where(quotation => quotation.RTD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.RTD_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(rfqTaxHdrObj.RTD_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = rfqTaxHdrObj.RTD_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                rfqTaxHdrList = rfqResponseHeaderObj.TaxHdr.ToList();
                                                rfqTaxHdrList.Add(rfqTaxHdrObj);
                                                rfqResponseHeaderObj.TaxHdr = rfqTaxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            rfqTaxHdrList = rfqResponseHeaderObj.TaxHdr.ToList();
                                            rfqTaxHdrList.Add(rfqTaxHdrObj);
                                            rfqResponseHeaderObj.TaxHdr = rfqTaxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                                        if (rfqResponseDetailsObj != null)
                                        {
                                            if (rfqTaxHdrObj.RTD_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = rfqResponseDetailsObj.RRD_AMOUNT;
                                                currentTotal = rfqResponseDetailsObj.TaxDtl.Where(quotation => quotation.RTD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.RTD_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(rfqTaxHdrObj.RTD_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = rfqTaxHdrObj.RTD_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    rfqTaxHdrList = rfqResponseDetailsObj.TaxDtl.ToList();
                                                    rfqTaxHdrList.Add(rfqTaxHdrObj);
                                                    rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK).TaxDtl = rfqTaxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                rfqTaxHdrList = rfqResponseDetailsObj.TaxDtl.ToList();
                                                rfqTaxHdrList.Add(rfqTaxHdrObj);
                                                rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK).TaxDtl = rfqTaxHdrList;
                                            }
                                        }
                                    }
                                    TempRFQResponseHeaderSession = rfqResponseHeaderObj;
                                    SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);
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
                                    //txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                                txtPopupAmount.Text = string.Empty;
                                txtPopupOther.Text = string.Empty;
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        if (errorTaxAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Tax_Add").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Invalid_Tax_Amount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (TempRFQResponseHeaderSession != null)
                        {
                            rfqResponseHeaderObj = TempRFQResponseHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                rfqTaxHdrList = new List<RFQTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempRFQTaxSplitObj = rfqResponseHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.RTD_TAX == taxPK && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempRFQTaxSplitObj = rfqResponseHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.RTD_NAME == hdfTaxName.Value && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempRFQTaxSplitObj != null)
                                    {
                                        rfqTaxHdrList = rfqResponseHeaderObj.TaxHdr.ToList();
                                        rfqTaxHdrList.Remove(tempRFQTaxSplitObj);
                                        rfqResponseHeaderObj.TaxHdr = rfqTaxHdrList;
                                    }
                                }
                                else
                                {
                                    rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                                    if (rfqResponseDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempRFQTaxSplitObj = rfqResponseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.RTD_TAX == taxPK && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempRFQTaxSplitObj = rfqResponseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.RTD_NAME == hdfTaxName.Value && rfq.RTD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                                        if (rfqResponseDetailsObj != null)
                                        {
                                            rfqTaxHdrList = rfqResponseDetailsObj.TaxDtl.ToList();
                                            rfqTaxHdrList.Remove(tempRFQTaxSplitObj);
                                            rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK).TaxDtl = rfqTaxHdrList;
                                        }
                                    }
                                }

                                TempRFQResponseHeaderSession = rfqResponseHeaderObj;
                                SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);

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
                                    TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    GetFieldValues(ControlsEnum.RFQTAXTYPES);
                                    TaxPK = 0;
                                    if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count == 1)
                                    {
                                        string taxFormula = dtRFQTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                        hdfTaxFormula.Value = taxFormula;
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                        txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                        SelectedTaxText = dtRFQTaxDetails.Rows[0]["TAX_HEAD"].ToString();
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    }
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.RFQTAXTYPES);
                            TaxPK = 0;
                            if (dtRFQTaxDetails != null && dtRFQTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtRFQTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                SelectedTaxText = dtRFQTaxDetails.Rows[0]["TAX_HEAD"].ToString();
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
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.RFQHEADER);
                        Response.Redirect(Resources.PageURL.RFQListing);
                        break;
                    #endregion
                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        //amount = 0;
                        //count = 0;
                        //try
                        //{
                        //    txtAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAmount") as TextBox);
                        //    amount = Convert.ToDouble(txtAmount.Text.Trim());
                        //}
                        //catch
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        //if (amount >= 0)
                        //{
                        //    if (RFQResponseHeaderSession != null)
                        //    {
                        //        rfqResponseHeaderObj = RFQResponseHeaderSession;
                        //        hdfRRDPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfRRDPK") as HiddenField);
                        //        hdfItemPK = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField);
                        //        if (hdfRRDPK != null)
                        //        {
                        //            SelectedResponsePK = string.IsNullOrEmpty(hdfRRDPK.Value) ? 0 : Convert.ToInt32(hdfRRDPK.Value);
                        //            SelectedItemPK = string.IsNullOrEmpty(hdfItemPK.Value) ? 0 : Convert.ToInt32(hdfItemPK.Value);
                        //            rfqResponseDetailsObj = rfqResponseHeaderObj.ResponseDtl.SingleOrDefault(rfq => rfq.RRD_PK == SelectedResponsePK && rfq.RRD_ITEM == SelectedItemPK);
                        //            if (rfqResponseDetailsObj != null)
                        //            {
                        //                discount = 0;
                        //                var discDetail = rfqResponseDetailsObj.TaxDtl.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount));
                        //                foreach (RFQTaxHdr rfqTaxHdrObj in discDetail)
                        //                {
                        //                    string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                        //                    if (!string.IsNullOrEmpty(taxFormula))
                        //                    {
                        //                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        //                        rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), 2);
                        //                    }
                        //                }
                        //                discount = rfqResponseDetailsObj.TaxDtl.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.RTD_TAX_AMT);
                        //                amount = amount - discount;
                        //                txtDiscount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtDiscount") as TextBox);
                        //                txtDiscount.Text = discount.ToString();


                        //                var taxDetail = rfqResponseDetailsObj.TaxDtl.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax));
                        //                foreach (RFQTaxHdr rfqTaxHdrObj in taxDetail)
                        //                {
                        //                    string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                        //                    if (!string.IsNullOrEmpty(taxFormula))
                        //                    {
                        //                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        //                        rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), 2);
                        //                    }
                        //                }
                        //                txtTax = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtTax") as TextBox);
                        //                txtTax.Text = rfqResponseDetailsObj.TaxDtl.ToList().Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.RTD_TAX_AMT).ToString();
                        //                RFQResponseHeaderSession = rfqResponseHeaderObj;
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Rate_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        SetDetailTax(sender);
                        SetHdrTax();
                        ResetForm(ControlsEnum.RFQTAXPOPUPGRID);
                        break;
                    #endregion
                    #region CALCULATEHDRTAX
                    case ActionsEnum.CALCULATEHDRTAX:
                        //amount = 0;
                        //count = 0;
                        //try
                        //{
                        //    txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                        //    amount = Convert.ToDouble(txtSubTotal.Text.Trim());
                        //}
                        //catch
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Invalid_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        //if (amount >= 0)
                        //{
                        //    if (RFQResponseHeaderSession != null)
                        //    {
                        //        rfqResponseHeaderObj = RFQResponseHeaderSession;
                        //        discount = 0;
                        //        var discHeader = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount));
                        //        foreach (RFQTaxHdr rfqTaxHdrObj in discHeader)
                        //        {
                        //            string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                        //            if (!string.IsNullOrEmpty(taxFormula))
                        //            {
                        //                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        //                rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), 2);
                        //            }
                        //        }
                        //        discount = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.RTD_TAX_AMT);
                        //        amount = amount - discount;
                        //        txtHdrDiscount.Text = discount.ToString();

                        //        var taxHeader = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax));
                        //        foreach (RFQTaxHdr rfqTaxHdrObj in taxHeader)
                        //        {
                        //            string taxFormula = rfqTaxHdrObj.RTD_TAX_FORMULA;
                        //            if (!string.IsNullOrEmpty(taxFormula))
                        //            {
                        //                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                        //                rfqTaxHdrObj.RTD_TAX_AMT = Math.Round(StringToFormula(taxFormula), 2);
                        //            }
                        //        }
                        //        txtHdrTax.Text = rfqResponseHeaderObj.TaxHdr.Where(rfq => rfq.RTD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.RTD_TAX_AMT).ToString();
                        //        RFQResponseHeaderSession = rfqResponseHeaderObj;
                        //    }
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Amount_Greater_Discount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        SetHdrTax();
                        break;
                    #endregion
                    #region Tabs
                    case ActionsEnum.RFQSEARCH:
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQSearch), false);
                        break;
                    case ActionsEnum.RFQREQUEST:
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RequestForQuote), false);
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
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {

                }
                if (e.Row.RowType == DataControlRowType.Header)
                {

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {


                }

                if (((GridView)sender).ID == "grdRFQResponse")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Label lblQuantity = e.Row.FindControl("lblQuantity") as Label;
                        TextBox txtRate = e.Row.FindControl("txtRate") as TextBox;

                        TextBox txtAmount = e.Row.FindControl("txtAmount") as TextBox;
                        TextBox txtDiscount = e.Row.FindControl("txtDiscount") as TextBox;
                        TextBox txtTax = e.Row.FindControl("txtTax") as TextBox;
                        TextBox txtTotal = e.Row.FindControl("txtTotal") as TextBox;

                        if (rfqResponseDetailsList != null && rfqResponseDetailsList.Count > 0)
                        {
                            //lblQuantity.Text = String.Format("{0:c}", decimal.Parse(lblQuantity.Text));
                            //lblQuantity.ToolTip = lblQuantity.Text;
                            //txtAmount.Text = Math.Round(decimal.Parse(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtDiscount.Text = Math.Round(decimal.Parse(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtTax.Text = Math.Round(decimal.Parse(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtTotal.Text = Math.Round(decimal.Parse(txtTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
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

            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
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
                if (RFQResponseHeaderSession != null)
                {
                    hdfHasTax.Value = ((RFQResponseHeaderSession.TaxHdr == null || RFQResponseHeaderSession.TaxHdr.Count == 0)
                        && RFQResponseHeaderSession.ResponseDtl.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0)))
                        ? CommonConstants.SELECT_VALUE_ZERO : CommonConstants.SELECT_VALUE_ONE;
                }

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
            VENDOR,
            RFQ,
            RFQHEADER,
            RFQHEADERTOP,
            RFQDETAIL,
            RFQTAXTYPES,
            RFQTAXPOPUPGRID,
            RFQTAXHEADER,
            EXCHANGERATE,
            COMPANY

        }

        #endregion


    }
}

