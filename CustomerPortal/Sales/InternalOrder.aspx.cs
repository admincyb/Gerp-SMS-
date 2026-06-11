using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Sales;
using ERP.Utilities;
using ERPData;
using ERPService;
using ERPManager;

namespace CustomerPortal.Sales
{
    public partial class InternalOrder : ERP.Store.UI.WorkFlowBasePage
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
        /// To maintain keep Carton Decimal
        /// </summary>
        private int CartonDecimal
        {
            get
            {
                return this.ViewState[ViewstateStrings.CartonDecimal] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CartonDecimal].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.CartonDecimal] = value;
            }
        }

        /// <summary>
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        private SaleContractBO SaleOrderHeaderSession
        {
            get
            {
                return (SaleContractBO)Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SaleOrderHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep Sale Order Tax Splitting
        /// </summary>
        private SaleContractBO TempSaleOrderHeaderSession
        {
            get
            {
                return (SaleContractBO)Session[ERP.Utilities.SessionStrings.TempSaleOrderHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.TempSaleOrderHeaderSession] = value;
            }
        }
        private int SelectedDtlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedDtlPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedDtlPK] = value;
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
        private int ShowLotNo
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowLotNo"]);
            }
            set
            {
                this.ViewState["ShowLotNo"] = value;
            }
        }
        private int ShowLotSize
        {
            get
            {
                return Convert.ToInt32(this.ViewState["ShowLotSize"]);
            }
            set
            {
                this.ViewState["ShowLotSize"] = value;
            }
        }
        private int IsInternalOrderPrint
        {
            get
            {
                return Convert.ToInt32(this.ViewState["IsInternalOrderPrint"]);
            }
            set
            {
                this.ViewState["IsInternalOrderPrint"] = value;
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
        private SaleContractBO saleOrderHeaderObj;
        private SaleContractDetailsBO saleOrderDetailsObj;

        List<SaleContractDetailsBO> saleOrderDetailsList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService commonServiceObj;
        DataSet dsSaleOrderTaxDetails;
        DataTable dtSaleOrderTaxDetails;
        DataTable dtPageData;
        //DataTable dtGrid;
        private int copyContractPK;
        private int notifyPartyPK;
        private int consigneePK;
        private int agentPK;
        private int fromPortPK;
        private int originOfGoodsPK;
        private int bankDetailPK;
        private double exchangeRate;
        private int soTypePK;
        private int cmpPK;
        private int custPK;
        private int addressPK;
        private int addressActive;

        private int transhipmentPK;
        private int shipByPK;
        private int deliveryTermPK;
        private int paymentTermPK;
        private int specialCausePK;
        private int inspectionPK;
        private int exportDocPK;
        private int custprodPK;
        DateTime bookingDate;
        //private int artPK;
        private int artWorkPK;

        private string refID;
        private bool hasPreviousTrxDiff;
        private string inboxFlag;
        private int processPK;
        private BusinessObject.User currentUser;

        List<SaleOrderTaxHdr> contractTaxHdrList;
        SaleContractDetailsBO contractDetails;

        private ADM_COMPANY_MST admCompanyMstObj;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        DataTable dtCompany;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations
            = {
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
            string prefID;
            int cusPK;
            int referenceID;
            int preferenceID;
            int appId;

            DateDefaultEnum defaultDate;
            int defaultAddMonths;

            referenceID = 0;
            preferenceID = 0;
            appId = 0;
            try
            {
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                hdfDecimalFormatWithComma.Value = "#" + currencysep + "#0.";
                hdfDecimalFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                {
                    hdfDecimalFormat.Value += "0";
                    hdfDecimalFormatWithComma.Value += "0";
                }
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                }
                hdfRateFormat.Value = "#0.";
                int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                for (int i = 0; i < rateDecimalDigits; i++)
                {
                    hdfRateFormat.Value += "0";
                }
                                
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

                if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                {
                    lblCompany.Visible = true;
                    ddlCompany.Visible = true;
                }
                else
                {
                    lblCompany.Visible = false;
                    ddlCompany.Visible = false;
                }

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    ShowLotNo = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowLotNo"));
                    ShowLotSize = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowLotSize"));

                    if (GetGlobalResourceObject("ConfigurationsRes", "SCCaseMarkShow").ToString() == "1")
                    {
                        DivCaseMark.Visible = true;
                    }
                    else
                    {
                        DivCaseMark.Visible = false;
                    }
                    txtCurrency.Enabled = false;
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SOD_SL_NO";
                    grdItemDetails.DataKeyNames = itemkeyarray;

                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    Session[ERP.Utilities.SessionStrings.QuotationHeader] = null;
                    TempSaleOrderHeaderSession = SaleOrderHeaderSession = null;

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
                    lblItemDiscount.Visible = txtDiscount.Visible = imgDiscount.Visible = EnableItemDiscount;
                    lblItemTax.Visible = txtTax.Visible = imgTax.Visible = EnableItemTax;

                    FillProcessID();
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    ReferanceID = string.IsNullOrEmpty(refID)
                                  ? string.IsNullOrEmpty(prefID)
                                      ? 0
                                      : int.Parse(prefID)
                                  : int.Parse(refID);
                    
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
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
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        preferenceID = int.Parse(prefID);
                        appId = GetApplicationID(preferenceID);
                        CurrPK = appId;
                        Session[ERP.Utilities.SessionStrings.PRefID] = null;
                        Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    }
                    if (Session[ERP.Utilities.SessionStrings.QUOTATIONPK] != null || Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    {
                        if (Session[ERP.Utilities.SessionStrings.QUOTATIONPK] != null)
                            CurrQuotationPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.QUOTATIONPK]);
                        if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                            CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERPK]);
                    }
                    if (CurrPK > 0 || CurrQuotationPK > 0)
                    {
                        GetFieldValues(ControlsEnum.SALEORDER);
                        if (saleOrderHeaderObj != null)
                            CurrPK = saleOrderHeaderObj.SOH_PK;
                        if (Session[ERP.Utilities.SessionStrings.SaleOrderMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.SaleOrderMode];
                        }
                        else
                            EntryStatus = EntryStatus.ENTRYMODE;
                        SetFieldValues(ControlsEnum.SALEORDERHEADER);
                        SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                        if (referenceID == 0)
                        {
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            ucrWrkf.RefID = preferenceID > 0 ? preferenceID : workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            if (ucrWrkf.RefID > 0)
                            {
                                GetFieldValues(ControlsEnum.REFIDSTATUS);
                                base.WkfRefID = (preferenceID > 0 || hasPreviousTrxDiff) ? 0 : ucrWrkf.RefID;
                            }                            
                        }
                    }
                    else
                    {
                        lblSaleOrderNo.Text = Resources.Messages.DocGenerationNew;

                        defaultDate = DateDefaultEnum.CurrentDate;
                        defaultAddMonths = 0;
                        Enum.TryParse(GetLocalResourceObject("DefaultDayBooking").ToString(), out defaultDate);
                        int.TryParse(GetLocalResourceObject("DefaultAddMonthsBooking").ToString(), out defaultAddMonths);
                        txtBookingDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ? defaultAddMonths + 1 : defaultAddMonths)
                            .AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                            defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);

                        defaultDate = DateDefaultEnum.CurrentDate;
                        defaultAddMonths = 0;
                        Enum.TryParse(GetLocalResourceObject("DefaultDayReqdBy").ToString(), out defaultDate);
                        int.TryParse(GetLocalResourceObject("DefaultAddMonthsReqdBy").ToString(), out defaultAddMonths);
                        txtReqByDate.Text = txtShipmentDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ?
                            defaultAddMonths + 1 : defaultAddMonths).AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                            defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);

                        txtSaleOrderDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        if (Session[ERP.Utilities.SessionStrings.SaleOrderMode] != null)
                        {
                            EntryStatus = (EntryStatus)Session[ERP.Utilities.SessionStrings.SaleOrderMode];
                        }
                        else
                            EntryStatus = EntryStatus.NEWMODE;

                        if (Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK] != null)
                        {
                            copyContractPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERCOPYPK]);
                            GetFieldValues(ControlsEnum.COPYCONTRACT);
                            SetFieldValues(ControlsEnum.COPYCONTRACT);
                            SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                        }
                        else
                        {
                            TempSaleOrderHeaderSession = SaleOrderHeaderSession = new SaleContractBO()
                            {
                                TaxHdr = new List<SaleOrderTaxHdr>(),
                                SaleContractDetails = new List<SaleContractDetailsBO>()
                            };

                            GetFieldValues(ControlsEnum.SOTYPE);
                            SetFieldValues(ControlsEnum.SOTYPE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlSaleOrderType.Items.FindByValue(dtPageData.Rows[0]["CFG_VALUE"].ToString()) != null)
                                ddlSaleOrderType.SelectedValue = dtPageData.Rows[0]["CFG_VALUE"].ToString();

                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            
                            GetFieldValues(ControlsEnum.SHIPBY);
                            SetFieldValues(ControlsEnum.SHIPBY);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlShipBy.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                ddlShipBy.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            //GetFieldValues(ControlsEnum.FROMPORT);
                            //SetFieldValues(ControlsEnum.FROMPORT);
                            //if (dtPageData != null && dtPageData.Rows.Count > 0
                            //     && ddlFromPort.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                            //    ddlFromPort.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlTranshipment.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                ddlTranshipment.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();

                            GetFieldValues(ControlsEnum.BANKDETAILS);
                            SetFieldValues(ControlsEnum.BANKDETAILS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlBankDetails.Items.FindByValue(dtPageData.Rows[0]["CBM_PK"].ToString()) != null)
                                ddlBankDetails.SelectedValue = dtPageData.Rows[0]["CBM_PK"].ToString();

                            GetFieldValues(ControlsEnum.INSPECTION);
                            SetFieldValues(ControlsEnum.INSPECTION);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlInspection.Items.FindByValue(dtPageData.Rows[0]["CFG_VALUE"].ToString()) != null)
                                ddlInspection.SelectedValue = dtPageData.Rows[0]["CFG_VALUE"].ToString();
                           
                            GetFieldValues(ControlsEnum.EXPORTDOC);
                            SetFieldValues(ControlsEnum.EXPORTDOC);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlExportDoc.Items.FindByValue(dtPageData.Rows[0]["CFG_VALUE"].ToString()) != null)
                                ddlExportDoc.SelectedValue = dtPageData.Rows[0]["CFG_VALUE"].ToString();
                            
                            GetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            SetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0
                                 && ddlOriginofGoods.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                ddlOriginofGoods.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();
                        }

                        txtCustomer.Focus();
                    }
                    AST_DOC_MODE.Value = "0";
                    if (lblSaleOrderNo.Text.Trim().Equals(string.Empty) || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                    {
                        AST_DOC_MODE.Value = GetDOCMODE();
                    }

                    ucrWrkf.FillWorkFlowDetails();
                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                        ucrWrkf.ViewType = 1;
                    else
                        ucrWrkf.ViewType = 0;

                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                    IsInternalOrderPrint = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsShowInternalOrderPrint"));
                    if (IsInternalOrderPrint == 1)
                        btnPrintIO.Visible = true;
                    else
                        btnPrintIO.Visible = false;
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

        /// <summary>
        /// Set Configuration settings
        /// </summary>
        private void ConfigurationSettings()
        {           
            #region Carton decimal settings
            DataTable dtCartonConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CARTON SETTINGS", "CARTON DECIMAL");
            if (dtCartonConfig != null && dtCartonConfig.Rows.Count > 0)
            {
                CartonDecimal = Convert.ToInt32(dtCartonConfig.Rows[0]["ACF_VALUE"].ToString());
            }
            #endregion
            IsInternalOrderPrint = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsShowInternalOrderPrint"));
            if (IsInternalOrderPrint==1)
                btnPrintIO.Visible = true;
            else
                btnPrintIO.Visible = false;

            if (GetGlobalResourceObject("ConfigurationsRes", "SCCaseMarkShow").ToString() == "1")
            {
                grdItemDetails.Columns[16].Visible = true;
            }
            else
            {
                grdItemDetails.Columns[16].Visible = false;
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
            AdmCompanyMstService admCompanyMstServiceClient;
            CommonService CommonServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.SALEORDER:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(CurrQuotationPK, CurrPK);
                        TempSaleOrderHeaderSession = SaleOrderHeaderSession = saleOrderHeaderObj;
                        if (saleOrderHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "','Quotation.aspx');", true);
                        }
                        break;
                    case ControlsEnum.COPYCONTRACT:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(0, copyContractPK);
                        break;
                    case ControlsEnum.CUSTOMERCONTRACT:
                        saleOrderHeaderObj = BusinessLogic.Sales.SaleOrderBL.GetSaleContractHeader(0, 0, custPK);
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
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    case ControlsEnum.BANKDETAILS:
                        dtPageData = BusinessLogic.Sales.Enquiry.GetBankDetails(0, 1, currentUser.SBUID, (int)CashBankType.Bank);
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
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, deliveryTermPK > 0 ? 2 : 1);
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, 1);
                        break;
                    case ControlsEnum.DELIVERYTERMSBYPK:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(deliveryTermPK, custPK, (int)CustomerTermType.DeliveryTerms, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.PAYMENTTERMS:
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1);
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, 1);
                        break;
                    case ControlsEnum.PAYMENTTERMSBYPK:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, custPK, (int)CustomerTermType.PaymentTerms, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.SPECIALCAUSE:
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, specialCausePK > 0 ? 2 : 1);
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, 1);
                        break;
                    case ControlsEnum.SPECIALCAUSEBYPK:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(specialCausePK, custPK, (int)CustomerTermType.SpecialCause, (int)DbActiveStatus.HASPK);
                        break;
                    case ControlsEnum.USERCUSTOMER:
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
                        break;
                    case ControlsEnum.CUSTOMER:
                        dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomer(custPK, string.Empty, currentUser.SBUID, 2);
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtSaleOrderDate.Text.Trim()));
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
                    case ControlsEnum.CUSTOMERPRODUCT:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetBrandDetails(custprodPK);
                        //dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerProduct(custprodPK, productPK, custPK, string.Empty, string.Empty, currentUser.SBUID, custprodPK > 0 ? 2 : 1);
                        break;
                    case ControlsEnum.PACKINGSPEC:
                        dtPageData = BusinessLogic.Inventory.PackingMasterBL.GetPackingMappingList(artWorkPK, artWorkPK > 0 ? Convert.ToInt32(DbActiveStatus.HASPK)
                            : Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0, 0, custprodPK, string.Empty, true);
                        break;
                    case ControlsEnum.BRANDRATE:
                        dtPageData = BusinessLogic.BrandRates.BrandRatesBL.GetBrandRate(custprodPK, 0, 0, bookingDate);
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(addressPK, custPK, addressActive == 2 ? 2 : 1, (int)CustomerAddressType.ShippingAddress).Tables[0];
                        break;
                    //case ControlsEnum.ARTWORK:
                    //    dtGrid = BusinessLogic.Sales.CustomerProduct.GetArtWork(artPK, custprodPK, artPK > 0 ? 2 : 1, string.Empty);
                    //    break;
                    case ControlsEnum.INSPECTION:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO INSP TYPE");
                        break;
                    case ControlsEnum.EXPORTDOC:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO EXP DOC");
                        break;
                    case ControlsEnum.CONTRACTTERMS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.SaleContract, 1, 1, currentUser.SBUID);
                        break;
                    case ControlsEnum.TAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsSaleOrderTaxDetails = BusinessLogic.Sales.QuotationBL.GetQuotationTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK));
                            if (dsSaleOrderTaxDetails != null && dsSaleOrderTaxDetails.Tables.Count > 0)
                            {
                                dtSaleOrderTaxDetails = dsSaleOrderTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Tax == category)
                            {
                                dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL);
                            }
                            else
                            {
                                dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0);
                            }
                        }
                        break;
                    case ControlsEnum.TAXSETTINGS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break;
                    case ControlsEnum.REFIDSTATUS:
                        CommonServiceClient = new CommonService();
                        hasPreviousTrxDiff = CommonServiceClient.GetHasPreviousTrxDiffProcess(ucrWrkf.RefID);
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
                    case ControlsEnum.COPYCONTRACT:
                        if (saleOrderHeaderObj != null)
                        {
                            saleOrderHeaderObj.SOH_PK = 0;
                            saleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                                 || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew
                                ? string.Empty : lblSaleOrderNo.Text.Trim();
                            saleOrderHeaderObj.SOH_DATE = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ?
                                DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtSaleOrderDate.Text.Trim();
                            saleOrderHeaderObj.SOH_REFERENCE = null;
                            saleOrderHeaderObj.SOH_REFERENCE_DATE = null;
                            if (!string.IsNullOrEmpty(txtBookingDate.Text))
                                saleOrderHeaderObj.SOH_BOOKING_DATE = txtBookingDate.Text;
                            if (!string.IsNullOrEmpty(txtShipmentDate.Text))
                                saleOrderHeaderObj.SOH_DELIVERY_DATE = txtShipmentDate.Text;
                            if (saleOrderHeaderObj.SaleContractDetails != null)
                            {
                                saleOrderHeaderObj.SaleContractDetails.ForEach(dtl =>
                                {
                                    dtl.SOD_PK = 0;
                                    dtl.SOD_REQUIRED_BY = txtReqByDate.Text;
                                    dtl.TaxDtl.ForEach(dtx =>
                                    {
                                        dtx.SLT_PK = 0;
                                    });
                                });
                                saleOrderHeaderObj.TaxHdr.ForEach(tax =>
                                {
                                    tax.SLT_PK = 0;
                                });
                            }
                            TempSaleOrderHeaderSession = SaleOrderHeaderSession = saleOrderHeaderObj;
                        }
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.CUSTOMERCONTRACT:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.SALEORDERDETAIL:
                        txtCustomer.Enabled = (!IsCustomerUser) && saleOrderHeaderObj.SaleContractDetails.Count == 0;
                        BindGrid(controlType);
                        if (saleOrderHeaderObj.SaleContractDetails.Sum(itm => itm.APS_TOTAL_PCS) > 0)
                        {
                            //lblTotalCBM.Text = lblTotalCBM.ToolTip = saleOrderHeaderObj.SaleContractDetails.Sum(itm =>
                            //    Math.Round((itm.SOD_QTY / itm.APS_TOTAL_PCS) * itm.CBM, 4)).ToString();

                            double val = 0;
                            foreach (var item in saleOrderDetailsList)
                            {
                                val += item.APS_TOTAL_PCS == 0 ? 0 : item.SOD_QTY / item.APS_TOTAL_PCS * item.CBM;
                            }
                            lblTotalCBM.Text = lblTotalCBM.ToolTip = val.ToString();

                        }
                        else
                        {
                            lblTotalCBM.Text = "0.0000";
                        }
                        break;
                    case ControlsEnum.PACKINGSPEC:
                        BindDropDownList(ControlsEnum.PACKINGSPEC);
                        break;
                    case ControlsEnum.CUSTOMERADDRESS:
                        BindDropDownList(controlType);
                        txtShippingAddress.Text = string.Empty;
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
                    case ControlsEnum.COMPANY:
                        BindDropDownList(ControlsEnum.COMPANY);
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
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(ControlsEnum.TAXPOPUPGRID);
                        break;
                    case ControlsEnum.TAXTYPES:
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
            string artWorkDesc;
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //Label lblSubTotal;
            try
            {
                switch (controlType)
                {
                    #region SaleContract Header
                    case ControlsEnum.SALEORDERHEADER:
                        if (SaleOrderHeaderSession != null)
                        {
                            saleOrderHeaderObj = SaleOrderHeaderSession;
                            saleOrderHeaderObj.SOH_PK = CurrPK;
                            saleOrderHeaderObj.SOH_NO = string.IsNullOrEmpty(lblSaleOrderNo.Text.Trim())
                                 || lblSaleOrderNo.Text.Trim() == Resources.Messages.DocGenerationNew
                                ? string.Empty : lblSaleOrderNo.Text.Trim();
                            saleOrderHeaderObj.SOH_DATE = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ?
                                DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtSaleOrderDate.Text.Trim();
                            saleOrderHeaderObj.SOH_CUSTOMER = Convert.ToInt32(hdfCustomer.Value);
                            saleOrderHeaderObj.SOH_CUSTOMER_NAME = HttpUtility.HtmlEncode(txtCustomer.Text);

                            if (hdfCusAddress.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS = HttpUtility.HtmlEncode(hdfCusAddress.Value);
                            if (hdfCusCountry.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY = hdfCusCountry.Value.Trim();
                            if (hdfCusCountryText.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfCusCountryText.Value);
                            if (hdfCusEmail.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_EMAIL = HttpUtility.HtmlEncode(hdfCusEmail.Value);
                            if (hdfCusFax.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_FAX = HttpUtility.HtmlEncode(hdfCusFax.Value);
                            if (hdfCusMobile.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_MOBILE = HttpUtility.HtmlEncode(hdfCusMobile.Value);
                            if (hdfCusPhone.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_PHONE = HttpUtility.HtmlEncode(hdfCusPhone.Value);
                            if (hdfCusZip.Value.Trim() != string.Empty)
                                saleOrderHeaderObj.SOH_CUSTOMER_ZIP = HttpUtility.HtmlEncode(hdfCusZip.Value);

                            if (saleOrderHeaderObj.SOH_QUOTATION > 0)
                            {
                                if (!string.IsNullOrEmpty(txtRefNo.Text))
                                    saleOrderHeaderObj.SOH_REF_NO = HttpUtility.HtmlEncode(txtRefNo.Text);
                                if (!string.IsNullOrEmpty(txtRefDate.Text))
                                    saleOrderHeaderObj.SOH_REF_DATE = txtRefDate.Text;
                            }
                            if (!string.IsNullOrEmpty(txtPONo.Text))
                                saleOrderHeaderObj.SOH_REFERENCE = HttpUtility.HtmlEncode(txtPONo.Text);
                            if (!string.IsNullOrEmpty(txtPODate.Text))
                                saleOrderHeaderObj.SOH_REFERENCE_DATE = txtPODate.Text;
                            if (!string.IsNullOrEmpty(txtBookingDate.Text))
                                saleOrderHeaderObj.SOH_BOOKING_DATE = txtBookingDate.Text;
                            saleOrderHeaderObj.SOH_TYPE = Convert.ToInt32(ddlSaleOrderType.SelectedValue);

                            saleOrderHeaderObj.SOH_CURRENCY = string.IsNullOrEmpty(hdfCurrency.Value) ? 0 : Convert.ToInt32(hdfCurrency.Value);
                            saleOrderHeaderObj.SOH_CURRENCY_BC = currentUser.BaseCurrency;
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            saleOrderHeaderObj.SOH_CURRENCY_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);

                            if (ddlShipBy.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_SHIP_BY = ddlShipBy.SelectedValue;
                            if (!string.IsNullOrEmpty(txtShipmentDate.Text))
                                saleOrderHeaderObj.SOH_DELIVERY_DATE = txtShipmentDate.Text;
                            if (!string.IsNullOrEmpty(txtShipmentDateText.Text))
                                saleOrderHeaderObj.SOH_SHIPMENT_DESC = txtShipmentDateText.Text;

                            //if (ddlFromPort.SelectedValue != CommonConstants.SELECTVAL)
                            //    saleOrderHeaderObj.SOH_FROM_PORT = ddlFromPort.SelectedValue;

                            saleOrderHeaderObj.SOH_FROM_PORT = hdfFromPortID.Value.ToString();
                            saleOrderHeaderObj.SOH_TO_PORT = HttpUtility.HtmlEncode(txtToPort.Text);

                            if (ddlTranshipment.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_TRANSHIPMENT = ddlTranshipment.SelectedValue;
                            saleOrderHeaderObj.SOH_FINAL_DESTINATION = HttpUtility.HtmlEncode(txtPortofDischarge.Text);

                            if (ddlConsigneeDetails.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_CONSIGNEE = ddlConsigneeDetails.SelectedValue;
                                if (hdfCNEName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_NAME = HttpUtility.HtmlEncode(hdfCNEName.Value);
                                if (hdfCNEAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS = HttpUtility.HtmlEncode(hdfCNEAddress.Value);
                                if (hdfCNECountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY = hdfCNECountry.Value.Trim();
                                if (hdfCNECountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfCNECountryText.Value);
                                if (hdfCNEEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL = HttpUtility.HtmlEncode(hdfCNEEmail.Value);
                                if (hdfCNEFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_FAX = hdfCNEFax.Value;
                                if (hdfCNEMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE = hdfCNEMobile.Value;
                                if (hdfCNEPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_PHONE = hdfCNEPhone.Value;
                                if (hdfCNEZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_CONSIGNEE_ZIP = hdfCNEZip.Value;
                            }

                            if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_SHIPPING_TO = ddlCustAddress.SelectedValue;
                                if (hdfShpName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_NAME = HttpUtility.HtmlEncode(hdfShpName.Value);
                                if (hdfShpAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_ADDRESS = HttpUtility.HtmlEncode(hdfShpAddress.Value);
                                if (hdfShpCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_COUNTRY = hdfShpCountry.Value.Trim();
                                if (hdfShpCountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfShpCountryText.Value);
                                if (hdfShpEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_EMAIL = HttpUtility.HtmlEncode(hdfShpEmail.Value);
                                if (hdfShpFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_FAX = hdfShpFax.Value;
                                if (hdfShpMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_MOBILE = hdfShpMobile.Value;
                                if (hdfShpPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_PHONE = hdfShpPhone.Value;
                                if (hdfShpZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIPPING_ZIP = hdfShpZip.Value;
                            }

                            if (ddlNotifyParty.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_NOTIFY_PARTY = ddlNotifyParty.SelectedValue;
                                if (hdfNPName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME = HttpUtility.HtmlEncode(hdfNPName.Value);
                                if (hdfNPAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS = HttpUtility.HtmlEncode(hdfNPAddress.Value);
                                if (hdfNPCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY = hdfNPCountry.Value.Trim();
                                if (hdfNPCountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfNPCountryText.Value);
                                if (hdfNPEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL = HttpUtility.HtmlEncode(hdfNPEmail.Value);
                                if (hdfNPFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX = hdfNPFax.Value;
                                if (hdfNPMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE = hdfNPMobile.Value;
                                if (hdfNPPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE = hdfNPPhone.Value;
                                if (hdfNPZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP = hdfNPZip.Value;
                            }

                            if (ddlAgent.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                saleOrderHeaderObj.SOH_SHIP_AGENT = ddlAgent.SelectedValue;
                                if (hdfAgentName.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_NAME = HttpUtility.HtmlEncode(hdfAgentName.Value);
                                if (hdfAgentAddress.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS = HttpUtility.HtmlEncode(hdfAgentAddress.Value);
                                if (hdfAgentCountry.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY = hdfAgentCountry.Value.Trim();
                                if (hdfAgentCountryText.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT = HttpUtility.HtmlEncode(hdfAgentCountryText.Value);
                                if (hdfAgentEmail.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL = HttpUtility.HtmlEncode(hdfAgentEmail.Value);
                                if (hdfAgentFax.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_FAX = hdfAgentFax.Value;
                                if (hdfAgentMobile.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE = hdfAgentMobile.Value;
                                if (hdfAgentPhone.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE = hdfAgentPhone.Value;
                                if (hdfAgentZip.Value.Trim() != string.Empty)
                                    saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP = hdfAgentZip.Value;
                            }

                            saleOrderHeaderObj.SOH_SHIP_INT_TO = HttpUtility.HtmlEncode(txtShppingIntimationto.Text);
                            saleOrderHeaderObj.SOH_FAX = HttpUtility.HtmlEncode(txtShppingIntimationtoFax.Text);
                            saleOrderHeaderObj.SOH_CONTAINER_SIZE = HttpUtility.HtmlEncode(txtCaseMark.Text);

                            saleOrderHeaderObj.SOH_DEL_TERM_TEXT = HttpUtility.HtmlEncode(txtDeliveryTerms.Text);
                            if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_DEL_TERM = ddlDeliveryTerms.SelectedValue;
                            saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT = HttpUtility.HtmlEncode(txtPaymentTerms.Text);
                            if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                            saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT = HttpUtility.HtmlEncode(txtSpecialCause.Text);
                            if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_SPECIAL_TERM = ddlSpecialCause.SelectedValue;

                            if (ddlBankDetails.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_BANK = ddlBankDetails.SelectedValue;
                            saleOrderHeaderObj.SOH_NEED_ADV_PYMT = chkNeedAdvPay.Checked;

                            if (ddlInspection.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_INSP_TYPE = ddlInspection.SelectedValue;
                            if (ddlExportDoc.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_EXP_DOC = ddlExportDoc.SelectedValue;
                            saleOrderHeaderObj.SOH_PACKING_INSTRN = HttpUtility.HtmlEncode(txtPackingInstruction.Text);
                            if (ddlOriginofGoods.SelectedValue != CommonConstants.SELECTVAL)
                                saleOrderHeaderObj.SOH_ORG_GOODS = ddlOriginofGoods.SelectedValue;
                            saleOrderHeaderObj.SOH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);

                            //lblSubTotal = (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            //saleOrderHeaderObj.SOH_TOTAL_AMT = lblSubTotal == null ? 0 : string.IsNullOrEmpty(lblSubTotal.Text.Trim()) ? 0 : Convert.ToDecimal(lblSubTotal.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_DISCOUNT = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_TAX = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                            saleOrderHeaderObj.SOH_TOTAL_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                            saleOrderHeaderObj.SOH_NET_AMOUNT = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                            saleOrderHeaderObj.SOH_NET_AMOUNT_BC = saleOrderHeaderObj.SOH_NET_AMOUNT * saleOrderHeaderObj.SOH_CURRENCY_RATE;

                            saleOrderHeaderObj.SOH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            saleOrderHeaderObj.SOH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            saleOrderHeaderObj.ACTIVE = saleOrderHeaderObj.SOH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            saleOrderHeaderObj.USER_PK = currentUser.PKUser;
                            saleOrderHeaderObj.LAST_MOD_DT = LastModifiedTime;
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
                    #endregion
                    #region Sale Contract Details
                    case ControlsEnum.SALEORDERDETAIL:
                        if (CurrSlNo != 0)
                        {
                            saleOrderDetailsObj = saleOrderDetailsList.SingleOrDefault(itm => itm.SOD_SL_NO == CurrSlNo);
                            if (saleOrderDetailsObj != null)
                            {
                                saleOrderDetailsObj.SOD_PK = Convert.ToInt32(hdfDetailPK.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtBrand.Text);
                                saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text);
                                saleOrderDetailsObj.APS_NAME = HttpUtility.HtmlEncode(txtPacking.Text);
                                saleOrderDetailsObj.SOD_PACKING_SPEC = Convert.ToInt32(hdfPackingSpec.Value);
                                saleOrderDetailsObj.PACKING_TEXT = HttpUtility.HtmlEncode(hdfPackingText.Value);
                                saleOrderDetailsObj.CBM = Convert.ToInt32(hdfCBM.Value);
                                if (ddlArtWork.SelectedValue != CommonConstants.SELECTVAL)
                                {
                                    saleOrderDetailsObj.SOD_ART_WORK = ddlArtWork.SelectedValue;

                                    saleOrderDetailsObj.PC_ART_WORK = lnkArtWorkPC.InnerText;
                                    saleOrderDetailsObj.PC_DOC_PATH = hdfArtWorkPC.Value;
                                    saleOrderDetailsObj.IB_ART_WORK = lnkArtWorkIB.InnerText;
                                    saleOrderDetailsObj.IB_DOC_PATH = hdfArtWorkIB.Value;
                                    saleOrderDetailsObj.IC_ART_WORK = lnkArtWorkIC.InnerText;
                                    saleOrderDetailsObj.IC_DOC_PATH = hdfArtWorkIC.Value;
                                    saleOrderDetailsObj.ZB_ART_WORK = lnkArtWorkZB.InnerText;
                                    saleOrderDetailsObj.ZB_DOC_PATH = hdfArtWorkZB.Value;
                                    saleOrderDetailsObj.MC_ART_WORK = lnkArtWorkMC.InnerText;
                                    saleOrderDetailsObj.MC_DOC_PATH = hdfArtWorkMC.Value;
                                    saleOrderDetailsObj.SC_ART_WORK = lnkArtWorkSC.InnerText;
                                    saleOrderDetailsObj.SC_DOC_PATH = hdfArtWorkSC.Value;

                                    artWorkDesc = string.Empty;
                                    artWorkDesc = saleOrderDetailsObj.PC_ART_WORK;
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.ZB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.ZB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.MC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.MC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.SC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.SC_ART_WORK);
                                    saleOrderDetailsObj.SOD_ART_WORK_DESC = artWorkDesc;
                                }
                                else
                                {
                                    saleOrderDetailsObj.SOD_ART_WORK = null;
                                    saleOrderDetailsObj.PC_ART_WORK = null;
                                    saleOrderDetailsObj.PC_DOC_PATH = null;
                                    saleOrderDetailsObj.IB_ART_WORK = null;
                                    saleOrderDetailsObj.IB_DOC_PATH = null;
                                    saleOrderDetailsObj.IC_ART_WORK = null;
                                    saleOrderDetailsObj.IC_DOC_PATH = null;
                                    saleOrderDetailsObj.ZB_ART_WORK = null;
                                    saleOrderDetailsObj.ZB_DOC_PATH = null;
                                    saleOrderDetailsObj.MC_ART_WORK = null;
                                    saleOrderDetailsObj.MC_DOC_PATH = null;
                                    saleOrderDetailsObj.SC_ART_WORK = null;
                                    saleOrderDetailsObj.SC_DOC_PATH = null;
                                    saleOrderDetailsObj.SOD_ART_WORK_DESC = null;
                                }
                                saleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                saleOrderDetailsObj.SOD_UOM = Convert.ToInt32(hdfUOM.Value);
                                saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                saleOrderDetailsObj.SOD_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                saleOrderDetailsObj.SOD_DISCOUNT = Math.Round(Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_LOT_NO = HttpUtility.HtmlEncode(txtLotNo.Text);

                                saleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfProduct.Value);
                                saleOrderDetailsObj.SOD_ITEM_CODE = txtProduct.Text;
                                saleOrderDetailsObj.SOD_ITEM_TEXT =  HttpUtility.HtmlEncode(hdfProductName.Value);
                                saleOrderDetailsObj.APS_TOTAL_PCS = Convert.ToDouble(txtTotalPiecesCtn.Text);
                                if (!string.IsNullOrEmpty(txtReqByDate.Text))
                                    saleOrderDetailsObj.SOD_REQUIRED_BY = txtReqByDate.Text;
                                saleOrderDetailsObj.SOD_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_TAX = Math.Round(Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_LOT_SIZE = HttpUtility.HtmlEncode(txtLotSize.Text);
                                saleOrderDetailsObj.SOD_CASE_MARK = HttpUtility.HtmlEncode(txtCaseMark.Text);

                                saleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                saleOrderDetailsObj.SOD_REMARKS2 = HttpUtility.HtmlEncode(txtDtlRemark2.Text);
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (saleOrderDetailsList == null || saleOrderDetailsList.Count == 0)
                            {
                                saleOrderDetailsList = new List<SaleContractDetailsBO>();
                                slno = 1;
                            }
                            else
                            {
                                slno = saleOrderDetailsList.Max(itm => itm.SOD_SL_NO);
                                slno++;
                            }
                            if (saleOrderDetailsList.SingleOrDefault(itm => itm.SOD_CUST_ITEM == Convert.ToInt32(hdfBrand.Value)) == null)
                            {
                                saleOrderDetailsObj = new SaleContractDetailsBO();
                                saleOrderDetailsObj.SOD_SL_NO = slno;
                                saleOrderDetailsObj.SOD_CUST_ITEM = Convert.ToInt32(hdfBrand.Value);
                                saleOrderDetailsObj.SOD_CUST_ITEM_TEXT = HttpUtility.HtmlEncode(txtBrand.Text);
                                saleOrderDetailsObj.SOD_CUST_ITEM_CODE = HttpUtility.HtmlEncode(txtBrandCode.Text);
                                saleOrderDetailsObj.APS_NAME = HttpUtility.HtmlEncode(txtPacking.Text);
                                saleOrderDetailsObj.SOD_PACKING_SPEC = Convert.ToInt32(hdfPackingSpec.Value);
                                saleOrderDetailsObj.PACKING_TEXT = HttpUtility.HtmlEncode(hdfPackingText.Value);
                                saleOrderDetailsObj.CBM = Convert.ToInt32(hdfCBM.Value);
                                if (ddlArtWork.SelectedValue != CommonConstants.SELECTVAL)
                                {
                                    saleOrderDetailsObj.SOD_ART_WORK = ddlArtWork.SelectedValue;

                                    saleOrderDetailsObj.PC_ART_WORK = lnkArtWorkPC.InnerText;
                                    saleOrderDetailsObj.PC_DOC_PATH = hdfArtWorkPC.Value;
                                    saleOrderDetailsObj.IB_ART_WORK = lnkArtWorkIB.InnerText;
                                    saleOrderDetailsObj.IB_DOC_PATH = hdfArtWorkIB.Value;
                                    saleOrderDetailsObj.IC_ART_WORK = lnkArtWorkIC.InnerText;
                                    saleOrderDetailsObj.IC_DOC_PATH = hdfArtWorkIC.Value;
                                    saleOrderDetailsObj.ZB_ART_WORK = lnkArtWorkZB.InnerText;
                                    saleOrderDetailsObj.ZB_DOC_PATH = hdfArtWorkZB.Value;
                                    saleOrderDetailsObj.MC_ART_WORK = lnkArtWorkMC.InnerText;
                                    saleOrderDetailsObj.MC_DOC_PATH = hdfArtWorkMC.Value;
                                    saleOrderDetailsObj.SC_ART_WORK = lnkArtWorkSC.InnerText;
                                    saleOrderDetailsObj.SC_DOC_PATH = hdfArtWorkSC.Value;

                                    artWorkDesc = string.Empty;
                                    artWorkDesc = saleOrderDetailsObj.PC_ART_WORK;
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.IC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.IC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.ZB_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.ZB_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.MC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.MC_ART_WORK);
                                    artWorkDesc = string.Concat(artWorkDesc, string.IsNullOrEmpty(artWorkDesc) || string.IsNullOrEmpty(saleOrderDetailsObj.SC_ART_WORK) ? string.Empty : ", ", saleOrderDetailsObj.SC_ART_WORK);
                                    saleOrderDetailsObj.SOD_ART_WORK_DESC = artWorkDesc;
                                }
                                saleOrderDetailsObj.SOD_QTY = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                saleOrderDetailsObj.SOD_UOM = Convert.ToInt32(hdfUOM.Value);
                                saleOrderDetailsObj.SOD_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                saleOrderDetailsObj.SOD_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                saleOrderDetailsObj.SOD_DISCOUNT = Math.Round(string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_LOT_NO = HttpUtility.HtmlEncode(txtLotNo.Text);

                                saleOrderDetailsObj.SOD_ITEM = Convert.ToInt32(hdfProduct.Value);
                                saleOrderDetailsObj.SOD_ITEM_CODE = txtProduct.Text;
                                saleOrderDetailsObj.SOD_ITEM_TEXT =  HttpUtility.HtmlEncode(hdfProductName.Value);
                                saleOrderDetailsObj.APS_TOTAL_PCS = Convert.ToDouble(txtTotalPiecesCtn.Text);
                                if (!string.IsNullOrEmpty(txtReqByDate.Text))
                                    saleOrderDetailsObj.SOD_REQUIRED_BY = txtReqByDate.Text;
                                saleOrderDetailsObj.SOD_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_TAX = Math.Round(string.IsNullOrEmpty(txtTax.Text.Trim()) ? 0 :
                                    Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                saleOrderDetailsObj.SOD_LOT_SIZE = HttpUtility.HtmlEncode(txtLotSize.Text);
                                saleOrderDetailsObj.SOD_CASE_MARK = HttpUtility.HtmlEncode(txtCaseMark.Text);

                                saleOrderDetailsObj.SOD_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                saleOrderDetailsObj.SOD_REMARKS2 = HttpUtility.HtmlEncode(txtDtlRemark2.Text);

                                saleOrderDetailsList.Add(saleOrderDetailsObj);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('"
                                    + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Same_Brand").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                return retObject;
                            }
                        }
                        retObject = saleOrderDetailsList;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            string addr;
            DateDefaultEnum defaultDate;
            int defaultAddMonths;
            try
            {
                switch (controlType)
                {
                    #region Contract
                    case ControlsEnum.SALEORDERHEADER:
                    case ControlsEnum.COPYCONTRACT:
                        if (saleOrderHeaderObj != null)
                        {
                            custPK = saleOrderHeaderObj.SOH_CUSTOMER;
                            hdfCustomer.Value = saleOrderHeaderObj.SOH_CUSTOMER.ToString();
                            txtCustomer.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_TEXT);

                            txtBuyerAddress.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS)
                                + (string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT) ? "" : (", " +
                                HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT)));
                            hdfCusAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ADDRESS);
                            hdfCusCountry.Value = saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY;
                            hdfCusCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_COUNTRY_TEXT);
                            hdfCusZip.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_ZIP);
                            hdfCusPhone.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_PHONE);
                            hdfCusMobile.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_MOBILE);
                            hdfCusFax.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_FAX);
                            hdfCusEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CUSTOMER_EMAIL);

                            if (controlType == ControlsEnum.SALEORDERHEADER)
                            {
                                CurrPK = saleOrderHeaderObj.SOH_PK;
                                lblSaleOrderNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_NO) ?
                                    Resources.Messages.DocGenerationNew : saleOrderHeaderObj.SOH_NO;
                                txtSaleOrderDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_DATE) ?
                                    DateTime.Now.ToString(Resources.ErpRes.DateFormat) : saleOrderHeaderObj.SOH_DATE;

                                txtPONo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REFERENCE) ? string.Empty : saleOrderHeaderObj.SOH_REFERENCE;
                                txtPODate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REFERENCE_DATE) ? string.Empty : saleOrderHeaderObj.SOH_REFERENCE_DATE;
                                
                                lblCustomerName.Text = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CUSTOMER_NAME) ? string.Empty : CommonFunctions.GetShortString(saleOrderHeaderObj.SOH_CUSTOMER_NAME,30);
                                lblCustomerName.ToolTip = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CUSTOMER_NAME) ? string.Empty : saleOrderHeaderObj.SOH_CUSTOMER_NAME;

                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BOOKING_DATE))
                                    txtBookingDate.Text = saleOrderHeaderObj.SOH_BOOKING_DATE;
                                else
                                {
                                    defaultDate = DateDefaultEnum.CurrentDate;
                                    defaultAddMonths = 0;
                                    Enum.TryParse(GetLocalResourceObject("DefaultDayBooking").ToString(), out defaultDate);
                                    int.TryParse(GetLocalResourceObject("DefaultAddMonthsBooking").ToString(), out defaultAddMonths);
                                    txtBookingDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ? defaultAddMonths + 1 : defaultAddMonths)
                                        .AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                                        defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
                                }
                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DELIVERY_DATE))
                                    txtShipmentDate.Text = saleOrderHeaderObj.SOH_DELIVERY_DATE;
                                else
                                {
                                    defaultDate = DateDefaultEnum.CurrentDate;
                                    defaultAddMonths = 0;
                                    Enum.TryParse(GetLocalResourceObject("DefaultDayReqdBy").ToString(), out defaultDate);
                                    int.TryParse(GetLocalResourceObject("DefaultAddMonthsReqdBy").ToString(), out defaultAddMonths);
                                    txtShipmentDate.Text = DateTime.Now.AddMonths(defaultDate == DateDefaultEnum.LastDate ?
                                        defaultAddMonths + 1 : defaultAddMonths).AddDays(defaultDate == DateDefaultEnum.CurrentDate ? 0 :
                                        defaultDate == DateDefaultEnum.FirstDate ? 1 - DateTime.Now.Day : -DateTime.Now.Day).ToString(Resources.ErpRes.DateFormat);
                                }
                                if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPMENT_DESC))
                                    txtShipmentDateText.Text = saleOrderHeaderObj.SOH_SHIPMENT_DESC;
                            }
                            GetFieldValues(ControlsEnum.SOTYPE);
                            soTypePK = saleOrderHeaderObj.SOH_TYPE;
                            SetFieldValues(ControlsEnum.SOTYPE);

                            GetFieldValues(ControlsEnum.COMPANY);
                            cmpPK = saleOrderHeaderObj.SOH_COMPANY;
                            SetFieldValues(ControlsEnum.COMPANY);

                            hdfCurrency.Value = saleOrderHeaderObj.SOH_CURRENCY.ToString();
                            txtCurrency.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CURRENCY_TEXT);

                            trQuotationReference.Visible = saleOrderHeaderObj.SOH_QUOTATION > 0;
                            txtRefNo.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_NO) ? string.Empty : saleOrderHeaderObj.SOH_REF_NO;
                            txtRefDate.Text = String.IsNullOrEmpty(saleOrderHeaderObj.SOH_REF_DATE) ? string.Empty : saleOrderHeaderObj.SOH_REF_DATE;

                            GetFieldValues(ControlsEnum.SHIPBY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_BY))
                                shipByPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIP_BY);
                            SetFieldValues(ControlsEnum.SHIPBY);

                            //GetFieldValues(ControlsEnum.FROMPORT);
                            //if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FROM_PORT))
                            //    fromPortPK = Convert.ToInt32(saleOrderHeaderObj.SOH_FROM_PORT);
                            //SetFieldValues(ControlsEnum.FROMPORT);

                            hdfFromPortID.Value = saleOrderHeaderObj.SOH_FROM_PORT == null ? "0" : saleOrderHeaderObj.SOH_FROM_PORT.ToString();
                            txtFromPort.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FROM_PORT_TEXT);

                            txtToPort.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TO_PORT);
                           


                            GetFieldValues(ControlsEnum.TRANSHIPMENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_TRANSHIPMENT))
                                transhipmentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_TRANSHIPMENT);
                            SetFieldValues(ControlsEnum.TRANSHIPMENT);

                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE))
                                consigneePK = Convert.ToInt32(saleOrderHeaderObj.SOH_CONSIGNEE);
                            SetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_NAME))
                                hdfCNEName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY))
                                hdfCNECountry.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY);
                            txtConsigneeDetails.Text = hdfCNEAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT))
                                hdfCNECountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL))
                                hdfCNEEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_FAX))
                                hdfCNEFax.Value = saleOrderHeaderObj.SOH_CONSIGNEE_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE))
                                hdfCNEMobile.Value = saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_PHONE))
                                hdfCNEPhone.Value = saleOrderHeaderObj.SOH_CONSIGNEE_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP))
                                hdfCNEZip.Value = saleOrderHeaderObj.SOH_CONSIGNEE_ZIP;

                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_TO))
                                addressPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIPPING_TO);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_NAME))
                                hdfShpName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY))
                                hdfShpCountry.Value = saleOrderHeaderObj.SOH_SHIPPING_COUNTRY;
                            txtShippingAddress.Text = hdfShpAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT))
                                hdfShpCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_EMAIL))
                                hdfShpEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_FAX))
                                hdfShpFax.Value = saleOrderHeaderObj.SOH_SHIPPING_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_MOBILE))
                                hdfShpMobile.Value = saleOrderHeaderObj.SOH_SHIPPING_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_PHONE))
                                hdfShpPhone.Value = saleOrderHeaderObj.SOH_SHIPPING_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_ZIP))
                                hdfShpZip.Value = saleOrderHeaderObj.SOH_SHIPPING_ZIP;

                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY))
                                notifyPartyPK = Convert.ToInt32(saleOrderHeaderObj.SOH_NOTIFY_PARTY);
                            SetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME))
                            {
                                hdfNPName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME);
                            }
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY))
                            {
                                hdfNPCountry.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY;
                            }
                            txtNotifyParty.Text = hdfNPAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT))
                                hdfNPCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL))
                                hdfNPEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX))
                                hdfNPFax.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE))
                                hdfNPMobile.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE))
                                hdfNPPhone.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP))
                                hdfNPZip.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_ZIP;

                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT))
                                agentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIP_AGENT);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_NAME))
                                hdfAgentName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY))
                                hdfAgentCountry.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY;
                            hdfAgentAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT))
                                hdfAgentCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL))
                                hdfAgentEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_FAX))
                                hdfAgentFax.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE))
                                hdfAgentMobile.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE))
                                hdfAgentPhone.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP))
                                hdfAgentZip.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP;

                            txtShppingIntimationto.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_INT_TO);
                            txtShppingIntimationtoFax.Text = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FAX) ? string.Empty : HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FAX);
                            txtContainerSize.Text = string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONTAINER_SIZE) ? string.Empty : HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONTAINER_SIZE);

                            //GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM))
                                deliveryTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_DEL_TERM);
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_TEXT);

                            //GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM))
                                paymentTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_PAYMENT_TERM);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT);

                            //GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM))
                                specialCausePK = Convert.ToInt32(saleOrderHeaderObj.SOH_SPECIAL_TERM);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT);

                            GetFieldValues(ControlsEnum.BANKDETAILS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BANK))
                                bankDetailPK = Convert.ToInt32(saleOrderHeaderObj.SOH_BANK);
                            SetFieldValues(ControlsEnum.BANKDETAILS);

                            GetFieldValues(ControlsEnum.INSPECTION);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_INSP_TYPE))
                                inspectionPK = Convert.ToInt32(saleOrderHeaderObj.SOH_INSP_TYPE);
                            SetFieldValues(ControlsEnum.INSPECTION);

                            GetFieldValues(ControlsEnum.EXPORTDOC);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_EXP_DOC))
                                exportDocPK = Convert.ToInt32(saleOrderHeaderObj.SOH_EXP_DOC);
                            SetFieldValues(ControlsEnum.EXPORTDOC);
                            txtPackingInstruction.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PACKING_INSTRN);

                            GetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_ORG_GOODS))
                                originOfGoodsPK = Convert.ToInt32(saleOrderHeaderObj.SOH_ORG_GOODS);
                            SetFieldValues(ControlsEnum.ORIGINOFGOODS);
                            if (CurrPK == 0)
                            {
                                EntryStatus = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.NEWMODE : EntryStatus;
                                if (dtPageData != null && dtPageData.Rows.Count > 0
                                && ddlOriginofGoods.Items.FindByValue(dtPageData.Rows[0]["CON_PK"].ToString()) != null)
                                    ddlOriginofGoods.SelectedValue = dtPageData.Rows[0]["CON_PK"].ToString();
                                txtPortofDischarge.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_TO_PORT);
                            }
                            else
                            {
                                chkNeedAdvPay.Checked = saleOrderHeaderObj.SOH_NEED_ADV_PYMT;
                                txtPortofDischarge.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FINAL_DESTINATION); ModifiedDatePnl.Visible = true;

                                LastModifiedTime = saleOrderHeaderObj.LAST_MOD_DT;
                                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            }

                            txtRemarks.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_REMARKS);

                            txtHdrDiscount.Text = saleOrderHeaderObj.SOH_TOTAL_DISCOUNT.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.Text = saleOrderHeaderObj.SOH_TOTAL_TAX.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = saleOrderHeaderObj.SOH_TOTAL_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrTotal.Text = saleOrderHeaderObj.SOH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);

                            txtHdrDiscount.ToolTip = saleOrderHeaderObj.SOH_TOTAL_DISCOUNT.ToString("c");
                            txtHdrTax.ToolTip = saleOrderHeaderObj.SOH_TOTAL_TAX.ToString("c");
                            txtShipping.ToolTip = saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE.ToString("c");
                            txtPriceAdj.ToolTip = saleOrderHeaderObj.SOH_TOTAL_ADJUST.ToString("c");
                            txtHdrTotal.ToolTip = saleOrderHeaderObj.SOH_NET_AMOUNT.ToString("c");

                            if (saleOrderHeaderObj.SOH_STATUS == (int)WorkFlowStatusEnum.Reviewed || saleOrderHeaderObj.SOH_STATUS == (int)WorkFlowStatusEnum.SendBackForApprove)
                            {
                                GetFieldValues(ControlsEnum.CONTRACTTERMS);
                                lnkTerms.Visible = true;
                                ltrTerms.Text = (dtPageData != null && dtPageData.Rows.Count > 0) ? dtPageData.Rows[0]["CON_DESC"].ToString() : string.Empty;
                                if (ucrWrkf.ViewType == 0)
                                {
                                    vrfInspection.Enabled = false;
                                    vrfExportDoc.Enabled = false;
                                }
                            }
                            else
                            {
                                lnkTerms.Visible = false;
                                vrfInspection.Enabled = false;
                                vrfExportDoc.Enabled = false;
                            }
                        }
                        break;
                    #endregion
                    #region Customer Contract
                    case ControlsEnum.CUSTOMERCONTRACT:
                        if (saleOrderHeaderObj != null)
                        {
                            //------ Customer Contract Details ------
                            //Set default Ship By
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_BY)
                                && ddlShipBy.Items.FindByValue(saleOrderHeaderObj.SOH_SHIP_BY) != null)
                                ddlShipBy.SelectedValue = saleOrderHeaderObj.SOH_SHIP_BY;
                            //Set default From Port
                            //if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FROM_PORT)
                            //    && ddlFromPort.Items.FindByValue(saleOrderHeaderObj.SOH_FROM_PORT) != null)
                            //    ddlFromPort.SelectedValue = saleOrderHeaderObj.SOH_FROM_PORT;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_FROM_PORT))
                            {
                                hdfFromPortID.Value = saleOrderHeaderObj.SOH_FROM_PORT;
                                txtFromPort.Text = saleOrderHeaderObj.SOH_FROM_PORT_TEXT;
                            }
                            //Set default Transhipment
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_TRANSHIPMENT)
                                && ddlTranshipment.Items.FindByValue(saleOrderHeaderObj.SOH_TRANSHIPMENT) != null)
                                ddlTranshipment.SelectedValue = saleOrderHeaderObj.SOH_TRANSHIPMENT;

                            txtShppingIntimationto.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_INT_TO);
                            txtShppingIntimationtoFax.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_FAX);
                            txtContainerSize.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONTAINER_SIZE);
                            //------ Bind Address and Details ------
                            GetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE))
                                consigneePK = Convert.ToInt32(saleOrderHeaderObj.SOH_CONSIGNEE);
                            SetFieldValues(ControlsEnum.CONSIGNEE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_NAME))
                                hdfCNEName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY))
                                hdfCNECountry.Value = saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY;
                            txtConsigneeDetails.Text = hdfCNEAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT))
                                hdfCNECountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL))
                                hdfCNEEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_CONSIGNEE_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_FAX))
                                hdfCNEFax.Value = saleOrderHeaderObj.SOH_CONSIGNEE_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE))
                                hdfCNEMobile.Value = saleOrderHeaderObj.SOH_CONSIGNEE_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_PHONE))
                                hdfCNEPhone.Value = saleOrderHeaderObj.SOH_CONSIGNEE_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_CONSIGNEE_ZIP))
                                hdfCNEZip.Value = saleOrderHeaderObj.SOH_CONSIGNEE_ZIP;

                            GetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY))
                                notifyPartyPK = Convert.ToInt32(saleOrderHeaderObj.SOH_NOTIFY_PARTY);
                            SetFieldValues(ControlsEnum.NOTIFYPARTY);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME))
                                hdfNPName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY))
                                hdfNPCountry.Value = saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY;
                            txtNotifyParty.Text = hdfNPAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT))
                                hdfNPCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_NOTIFY_PARTY_COUNTRY_TEXT);
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

                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_TO))
                                addressPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIPPING_TO);
                            SetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_NAME))
                                hdfShpName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY))
                                hdfShpCountry.Value = saleOrderHeaderObj.SOH_SHIPPING_COUNTRY;
                            txtShippingAddress.Text = hdfShpAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT))
                                hdfShpCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_EMAIL))
                                hdfShpEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIPPING_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_FAX))
                                hdfShpFax.Value = saleOrderHeaderObj.SOH_SHIPPING_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_MOBILE))
                                hdfShpMobile.Value = saleOrderHeaderObj.SOH_SHIPPING_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_PHONE))
                                hdfShpPhone.Value = saleOrderHeaderObj.SOH_SHIPPING_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIPPING_ZIP))
                                hdfShpZip.Value = saleOrderHeaderObj.SOH_SHIPPING_ZIP;

                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT))
                                agentPK = Convert.ToInt32(saleOrderHeaderObj.SOH_SHIP_AGENT);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_NAME))
                                hdfAgentName.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_NAME);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY))
                                hdfAgentCountry.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY;
                            hdfAgentAddress.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_ADDRESS);

                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT))
                                hdfAgentCountryText.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_COUNTRY_TEXT);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL))
                                hdfAgentEmail.Value = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SHIP_AGENT_EMAIL);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_FAX))
                                hdfAgentFax.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_FAX;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE))
                                hdfAgentMobile.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_MOBILE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE))
                                hdfAgentPhone.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_PHONE;
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP))
                                hdfAgentZip.Value = saleOrderHeaderObj.SOH_SHIP_AGENT_ZIP;

                            //GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_DEL_TERM))
                                deliveryTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_DEL_TERM);
                            GetFieldValues(ControlsEnum.DELIVERYTERMS);
                            SetFieldValues(ControlsEnum.DELIVERYTERMS);
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_DEL_TERM_TEXT);

                            //GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_PAYMENT_TERM))
                                paymentTermPK = Convert.ToInt32(saleOrderHeaderObj.SOH_PAYMENT_TERM);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PAYMENT_TERM_TEXT);

                            //GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_SPECIAL_TERM))
                                specialCausePK = Convert.ToInt32(saleOrderHeaderObj.SOH_SPECIAL_TERM);
                            GetFieldValues(ControlsEnum.SPECIALCAUSE);
                            SetFieldValues(ControlsEnum.SPECIALCAUSE);
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_SPECIAL_TERM_TEXT);

                            //Set default bank
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_BANK)
                                && ddlBankDetails.Items.FindByValue(saleOrderHeaderObj.SOH_BANK) != null)
                                ddlBankDetails.SelectedValue = saleOrderHeaderObj.SOH_BANK;

                            //Set default inspection
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_INSP_TYPE)
                                && ddlInspection.Items.FindByValue(saleOrderHeaderObj.SOH_INSP_TYPE) != null)
                                ddlInspection.SelectedValue = saleOrderHeaderObj.SOH_INSP_TYPE;
                            //Set default Export docs
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_EXP_DOC)
                                && ddlExportDoc.Items.FindByValue(saleOrderHeaderObj.SOH_EXP_DOC) != null)
                                ddlExportDoc.SelectedValue = saleOrderHeaderObj.SOH_EXP_DOC;

                            txtPackingInstruction.Text = HttpUtility.HtmlDecode(saleOrderHeaderObj.SOH_PACKING_INSTRN);
                            //set default origin of goods
                            if (!string.IsNullOrEmpty(saleOrderHeaderObj.SOH_ORG_GOODS)
                                && ddlOriginofGoods.Items.FindByValue(saleOrderHeaderObj.SOH_ORG_GOODS) != null)
                                ddlOriginofGoods.SelectedValue = saleOrderHeaderObj.SOH_ORG_GOODS;
                        }
                        break;
                    #endregion
                    #region Line Item Details
                    case ControlsEnum.SELECTEDITEM:
                        if (saleOrderDetailsObj != null)
                        {
                            custprodPK = saleOrderDetailsObj.SOD_CUST_ITEM;
                            hdfDetailPK.Value = saleOrderDetailsObj.SOD_PK.ToString();
                            CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                            hdfBrand.Value = saleOrderDetailsObj.SOD_CUST_ITEM.ToString();
                            txtBrand.Text = saleOrderDetailsObj.SOD_CUST_ITEM_TEXT;
                            txtBrandCode.Text = saleOrderDetailsObj.SOD_CUST_ITEM_CODE;
                            txtPacking.Text = saleOrderDetailsObj.CIM_PACKING_SPEC_NAME;
                            hdfPackingSpec.Value = saleOrderDetailsObj.SOD_PACKING_SPEC.ToString();
                            //hdfPackingText.Value = saleOrderDetailsObj.PACKING_TEXT.ToString();
                            hdfCBM.Value = saleOrderDetailsObj.CBM.ToString();
                            if(!string.IsNullOrEmpty(saleOrderDetailsObj.PACKING_TEXT))
                            {
                                hdfPackingText.Value = saleOrderDetailsObj.PACKING_TEXT.ToString();
                            }
                            GetFieldValues(ControlsEnum.PACKINGSPEC);
                            artWorkPK = string.IsNullOrEmpty(saleOrderDetailsObj.SOD_ART_WORK) ? 0 : Convert.ToInt32(saleOrderDetailsObj.SOD_ART_WORK);
                            SetFieldValues(ControlsEnum.PACKINGSPEC);

                            if (!string.IsNullOrEmpty(saleOrderDetailsObj.SOD_ART_WORK))
                            {
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.PC_ART_WORK))
                                {
                                    lnkArtWorkPC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.PC_DOC_PATH))
                                        lnkArtWorkPC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkPC.Disabled = false;
                                        lnkArtWorkPC.HRef = saleOrderDetailsObj.PC_DOC_PATH;
                                    }
                                    lnkArtWorkPC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.PC_ART_WORK);
                                    hdfArtWorkPC.Value = saleOrderDetailsObj.PC_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.IB_ART_WORK))
                                {
                                    lnkArtWorkIB.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.IB_DOC_PATH))
                                        lnkArtWorkIB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIB.Disabled = false;
                                        lnkArtWorkIB.HRef = saleOrderDetailsObj.IB_DOC_PATH;
                                    }
                                    lnkArtWorkIB.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.IB_ART_WORK);
                                    hdfArtWorkIB.Value = saleOrderDetailsObj.IB_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.IC_ART_WORK))
                                {
                                    lnkArtWorkIC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.IC_DOC_PATH))
                                        lnkArtWorkIC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIC.Disabled = false;
                                        lnkArtWorkIC.HRef = saleOrderDetailsObj.IC_DOC_PATH;
                                    }
                                    lnkArtWorkIC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.IC_ART_WORK);
                                    hdfArtWorkIC.Value = saleOrderDetailsObj.IC_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.ZB_ART_WORK))
                                {
                                    lnkArtWorkZB.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.ZB_DOC_PATH))
                                        lnkArtWorkZB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkZB.Disabled = false;
                                        lnkArtWorkZB.HRef = saleOrderDetailsObj.ZB_DOC_PATH;
                                    }
                                    lnkArtWorkZB.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.ZB_ART_WORK);
                                    hdfArtWorkZB.Value = saleOrderDetailsObj.ZB_DOC_PATH;
                                }
                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.MC_ART_WORK))
                                {
                                    lnkArtWorkMC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.MC_DOC_PATH))
                                        lnkArtWorkMC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkMC.Disabled = false;
                                        lnkArtWorkMC.HRef = saleOrderDetailsObj.MC_DOC_PATH;
                                    }
                                    lnkArtWorkMC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.MC_ART_WORK);
                                    hdfArtWorkMC.Value = saleOrderDetailsObj.MC_DOC_PATH;
                                }

                                if (!string.IsNullOrEmpty(saleOrderDetailsObj.SC_ART_WORK))
                                {
                                    lnkArtWorkSC.Visible = true;
                                    if (string.IsNullOrEmpty(saleOrderDetailsObj.SC_DOC_PATH))
                                        lnkArtWorkSC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkSC.Disabled = false;
                                        lnkArtWorkSC.HRef = saleOrderDetailsObj.SC_DOC_PATH;
                                    }
                                    lnkArtWorkSC.InnerText = HttpUtility.HtmlDecode(saleOrderDetailsObj.SC_ART_WORK);
                                    hdfArtWorkSC.Value = saleOrderDetailsObj.SC_DOC_PATH;
                                }
                            }

                            txtQty.Text = GetFormattedNumber(saleOrderDetailsObj.SOD_QTY);
                            hdfUOM.Value = saleOrderDetailsObj.SOD_UOM.ToString();
                            txtUOM.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_UOM_TEXT);
                            txtRate.Text = GetFormattedRate(saleOrderDetailsObj.SOD_RATE);
                            txtDiscount.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_DISCOUNT);
                            txtLotNo.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_LOT_NO);

                            hdfProduct.Value = saleOrderDetailsObj.SOD_ITEM.ToString();
                            txtProduct.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_ITEM_CODE);
                            hdfProductName.Value = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_ITEM_TEXT);
                            txtTotalPiecesCtn.Text = Math.Floor(saleOrderDetailsObj.APS_TOTAL_PCS > 1 ? saleOrderDetailsObj.APS_TOTAL_PCS : 1).ToString();
                            txtReqByDate.Text = saleOrderDetailsObj.SOD_REQUIRED_BY;
                            hdfReqByDate.Value = Convert.ToDateTime(saleOrderDetailsObj.SOD_REQUIRED_BY).ToString();
                            txtAmount.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_AMOUNT);
                            txtTax.Text = GetFormattedCurrency(saleOrderDetailsObj.SOD_TAX);
                            txtLotSize.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_LOT_SIZE);
                            txtCaseMark.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_CASE_MARK);

                            txtDtlRemark.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_REMARKS);
                            txtDtlRemark2.Text = HttpUtility.HtmlDecode(saleOrderDetailsObj.SOD_REMARKS2);
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
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.PACKINGSPEC:
                        ddlArtWork.Items.Clear();
                        if (dtPageData != null)
                        {
                            ddlArtWork.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "PIM_ART_WORK");
                            ddlArtWork.DataTextField = "PIM_ART_WORK";
                            ddlArtWork.DataValueField = "PIM_PK";
                            ddlArtWork.DataBind();
                        }
                        ddlArtWork.Items.Insert(0, new ListItem(Resources.ErpRes.New, CommonConstants.SELECTVAL));
                        if (artWorkPK > 0 && ddlArtWork.Items.FindByValue(artWorkPK.ToString()) != null)
                            ddlArtWork.SelectedValue = artWorkPK.ToString();
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
                        if (addressPK > 0 && ddlCustAddress.Items.FindByValue(addressPK.ToString()) != null)
                            ddlCustAddress.SelectedValue = addressPK.ToString();
                        break;
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
                        if (notifyPartyPK > 0 && ddlNotifyParty.Items.FindByValue(notifyPartyPK.ToString()) != null)
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
                        if (consigneePK > 0 && ddlConsigneeDetails.Items.FindByValue(consigneePK.ToString()) != null)
                            ddlConsigneeDetails.SelectedValue = consigneePK.ToString();
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
                        //ddlFromPort.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        //if (fromPortPK > 0 && ddlFromPort.Items.FindByValue(fromPortPK.ToString()) != null)
                        //    ddlFromPort.SelectedValue = fromPortPK.ToString();
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
                        if (originOfGoodsPK > 0 && ddlOriginofGoods.Items.FindByValue(originOfGoodsPK.ToString()) != null)
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
                        if (bankDetailPK > 0 && ddlBankDetails.Items.FindByValue(bankDetailPK.ToString()) != null)
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
                        if (soTypePK > 0 && ddlSaleOrderType.Items.FindByValue(soTypePK.ToString()) != null)
                            ddlSaleOrderType.SelectedValue = soTypePK.ToString();
                        break;
                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                        {
                            ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                            ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                            ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                            ddlCompany.DataBind();
                        }
                        if (cmpPK > 0 && ddlCompany.Items.FindByValue(cmpPK.ToString()) != null)
                            ddlCompany.SelectedValue = cmpPK.ToString();
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
                        if (agentPK > 0 && ddlAgent.Items.FindByValue(agentPK.ToString()) != null)
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
                        if (inspectionPK > 0 && ddlInspection.Items.FindByValue(inspectionPK.ToString()) != null)
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
                        if (exportDocPK > 0 && ddlExportDoc.Items.FindByValue(exportDocPK.ToString()) != null)
                            ddlExportDoc.SelectedValue = exportDocPK.ToString();
                        break;
                    case ControlsEnum.TAXTYPES:
                        //Bind Tax dropdown
                        ddlPopupTaxType.Items.Clear();
                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count > 0)
                        {
                            ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSaleOrderTaxDetails, "TAX_HEAD");
                            ddlPopupTaxType.DataTextField = "TAX_HEAD";
                            ddlPopupTaxType.DataValueField = "TAX_PK";
                            ddlPopupTaxType.DataBind();
                        }
                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
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
                            saleOrderDetailsList = new List<SaleContractDetailsBO>();
                            saleOrderDetailsList = saleOrderHeaderObj.SaleContractDetails;
                            if (saleOrderDetailsList != null)
                            {
                                grdItemDetails.DataSource = saleOrderDetailsList;
                                grdItemDetails.DataBind();
                            }
                        }
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {
                            contractTaxHdrList = TempSaleOrderHeaderSession.TaxHdr == null ? new List<SaleOrderTaxHdr>() :
                                TempSaleOrderHeaderSession.TaxHdr.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            contractDetails = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(dtl => dtl.SOD_PK == SelectedDtlPK
                                && dtl.SOD_CUST_ITEM == SelectedCusItemPK && dtl.SOD_ITEM == SelectedItemPK);
                            if (contractDetails != null)
                            {
                                contractTaxHdrList = contractDetails.TaxDtl == null ? new List<SaleOrderTaxHdr>() :
                                    contractDetails.TaxDtl.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                contractTaxHdrList = new List<SaleOrderTaxHdr>();
                        }
                        grdTaxDetails.DataSource = contractTaxHdrList;
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
        /// Validate for invalid entry in Autocomplete fields
        /// </summary>
        /// <param name="mode"></param>
        /// <returns>Validation Status</returns>
        private bool ValidateForm(ActionsEnum mode)
        {
            bool flag;
            flag = true;
            switch (mode)
            {
                case ActionsEnum.SAVE:
                case ActionsEnum.WRKFSUBMIT:

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
                    Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrQuotationPK;
                    CurrQuotationPK = 0;
                    break;
                case ControlsEnum.SALEORDERDETAIL:
                    TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                    CurrSlNo = 0;
                    hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfBrand.Value = string.Empty;
                    txtBrand.Text = string.Empty;
                    ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                    break;
                case ControlsEnum.CUSTOMERCONTRACT:
                    TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                    ResetDdl(ddlShipBy);
                    //ResetDdl(ddlFromPort);
                    ResetDdl(ddlTranshipment);

                    txtShppingIntimationto.Text = string.Empty;
                    txtShppingIntimationtoFax.Text = string.Empty;
                    txtContainerSize.Text = string.Empty;

                    ClearDdl(ControlsEnum.CONSIGNEE);
                    hdfCNEName.Value = hdfCNECountry.Value = txtConsigneeDetails.Text = hdfCNEAddress.Value = hdfCNECountryText.Value =
                        hdfCNEEmail.Value = hdfCNEFax.Value = hdfCNEMobile.Value = hdfCNEPhone.Value = hdfCNEZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.NOTIFYPARTY);
                    hdfNPName.Value = hdfNPCountry.Value = txtNotifyParty.Text = hdfNPAddress.Value = hdfNPCountryText.Value =
                        hdfNPEmail.Value = hdfNPFax.Value = hdfNPMobile.Value = hdfNPPhone.Value = hdfNPZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.CUSTOMERADDRESS);
                    hdfShpName.Value = hdfShpCountry.Value = txtShippingAddress.Text = hdfShpAddress.Value = hdfShpCountryText.Value =
                        hdfShpEmail.Value = hdfShpFax.Value = hdfShpMobile.Value = hdfShpPhone.Value = hdfShpZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.SHIPPINGAGENT);
                    hdfAgentName.Value = hdfAgentCountry.Value = hdfAgentAddress.Value = hdfAgentCountryText.Value = hdfAgentEmail.Value =
                        hdfAgentFax.Value = hdfAgentMobile.Value = hdfAgentPhone.Value = hdfAgentZip.Value = string.Empty;

                    ClearDdl(ControlsEnum.DELIVERYTERMS);
                    txtDeliveryTerms.Text = string.Empty;
                    ClearDdl(ControlsEnum.PAYMENTTERMS);
                    txtPaymentTerms.Text = string.Empty;
                    ClearDdl(ControlsEnum.SPECIALCAUSE);
                    txtSpecialCause.Text = string.Empty;

                    ResetDdl(ddlBankDetails);
                    ResetDdl(ddlInspection);
                    ResetDdl(ddlExportDoc);
                    txtPackingInstruction.Text = string.Empty;
                    ResetDdl(ddlOriginofGoods);
                    break;
                case ControlsEnum.CUSTOMER:
                    txtBuyerAddress.Text = hdfCusAddress.Value = hdfCusCountry.Value = hdfCusCountryText.Value = hdfCusZip.Value =
                                    hdfCusPhone.Value = hdfCusMobile.Value = hdfCusFax.Value = hdfCusEmail.Value = string.Empty;
                    hdfCurrency.Value = txtCurrency.Text = txtPortofDischarge.Text = txtToPort.Text = hdfExchangeRate.Value = string.Empty;
                    break;
                case ControlsEnum.CUSTOMERPRODUCT:
                    txtBrandCode.Text = string.Empty;
                    txtPacking.Text = string.Empty;
                    hdfPackingSpec.Value = string.Empty;
                    hdfPackingText.Value = string.Empty;
                    //hdfArtWork.Value = string.Empty;
                    hdfCBM.Value = string.Empty;
                    ClearDdl(ControlsEnum.PACKINGSPEC);
                    ResetForm(ControlsEnum.PACKINGSPEC);
                    txtQty.Text = string.Empty;
                    hdfUOM.Value = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtRate.Text = string.Empty;
                    txtDiscount.Text = string.Empty;
                    txtLotNo.Text = string.Empty;

                    hdfProduct.Value = string.Empty;
                    txtProduct.Text = string.Empty;
                    hdfProductName.Value = string.Empty;
                    txtTotalPiecesCtn.Text = string.Empty;
                    //txtReqByDate.Text = string.Empty;
                    //hdfReqByDate.Value = string.Empty;
                    txtAmount.Text = string.Empty;
                    txtTax.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtCaseMark.Text = string.Empty;
                    txtDtlRemark.Text = string.Empty;
                    txtDtlRemark2.Text = string.Empty;
                    break;
                case ControlsEnum.PACKINGSPEC:
                    lnkArtWorkPC.HRef = "#";
                    lnkArtWorkIB.HRef = "#";
                    lnkArtWorkIC.HRef = "#";
                    lnkArtWorkZB.HRef = "#";
                    lnkArtWorkMC.HRef = "#";
                    lnkArtWorkSC.HRef = "#";

                    lnkArtWorkPC.Visible = false;
                    lnkArtWorkIB.Visible = false;
                    lnkArtWorkIC.Visible = false;
                    lnkArtWorkZB.Visible = false;
                    lnkArtWorkMC.Visible = false;
                    lnkArtWorkSC.Visible = false;

                    lnkArtWorkPC.Disabled = true;
                    lnkArtWorkIB.Disabled = true;
                    lnkArtWorkIC.Disabled = true;
                    lnkArtWorkZB.Disabled = true;
                    lnkArtWorkMC.Disabled = true;
                    lnkArtWorkSC.Disabled = true;

                    hdfArtWorkPC.Value = string.Empty;
                    hdfArtWorkIB.Value = string.Empty;
                    hdfArtWorkIC.Value = string.Empty;
                    hdfArtWorkZB.Value = string.Empty;
                    hdfArtWorkMC.Value = string.Empty;
                    hdfArtWorkSC.Value = string.Empty;
                    break;
                case ControlsEnum.TAXPOPUPGRID:
                    TaxPK = 0;
                    SelectedDtlPK = 0;
                    SelectedItemPK = 0;
                    SelectedCusItemPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break;
            }
        }
        /// <summary>
        /// Reset Ddl Selected Value
        /// </summary>
        /// <param name="ddl"></param>
        private void ResetDdl(DropDownList ddl)
        {
            if (ddl != null)
            {
                ddl.ClearSelection();
                if (ddl.Items.Count > 1)
                    ddl.Items[1].Selected = true;
            }
        }
        /// <summary>
        /// Clear Ddl Values
        /// </summary>
        /// <param name="control"></param>
        private void ClearDdl(ControlsEnum control)
        {
            dtPageData = null;
            BindDropDownList(control);
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

        private void SetDetailTax(SaleContractBO saleContractHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(txtQty.Text, out rate))
            {
                if (txtAmount != null)
                {
                    txtAmount.Text = GetFormattedCurrency(rate * quantity);

                    SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                    SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                    SetItemTax(saleContractHdr);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        private bool SetItemTax(SaleContractBO saleContractHdr)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            Double.TryParse(txtAmount.Text.Trim(), out amount);
            if (amount >= 0)
            {
                if (saleContractHdr != null)
                {
                    saleOrderHeaderObj = saleContractHdr;
                    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                        && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK);
                    if (saleOrderDetailsObj != null)
                    {
                        if (saleOrderDetailsObj.TaxDtl != null)
                        {
                            var discDetail = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (SaleOrderTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            discount = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (saleOrderDetailsObj.TaxDtl != null)
                        {
                            var taxDetail = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (SaleOrderTaxHdr taxHdrObj in taxDetail)
                            {
                                string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                    taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            itmTax = saleOrderDetailsObj.TaxDtl.ToList().Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.SLT_TAX_AMT);
                        }
                        txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        saleOrderDetailsObj.SOD_AMOUNT = amount;
                        saleOrderDetailsObj.SOD_DISCOUNT = discount;
                        saleOrderDetailsObj.SOD_TAX = itmTax;
                        saleOrderDetailsObj.SOD_NET_AMOUNT = (amount - discount + itmTax);
                        txtTotal.Text = saleOrderDetailsObj.SOD_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                        //TempSaleOrderHeaderSession = saleOrderHeaderObj;
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
        private void SetSubTotal()
        {
            Label lblSubTotalFooter;
            SaleOrderHeaderSession.SOH_TOTAL_AMT = Convert.ToDouble(SaleOrderHeaderSession.SaleContractDetails.Sum(dtl => dtl.SOD_NET_AMOUNT));
            if (grdItemDetails.FooterRow != null)
            {
                lblSubTotalFooter = grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label;
                if (lblSubTotalFooter != null)
                {
                    lblSubTotalFooter.Text = SaleOrderHeaderSession.SOH_TOTAL_AMT.ToString("c");
                }
            }
        }
        private bool SetHdrTax()
        {
            double amount;
            double discount;
            double shipping;
            double adjust;
            amount = 0;
            shipping = 0;
            adjust = 0;

            if (SaleOrderHeaderSession != null)
            {
                saleOrderHeaderObj = SaleOrderHeaderSession;
                amount = Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT);
                discount = 0;
                if (saleOrderHeaderObj.TaxHdr != null)
                {
                    var discHeader = saleOrderHeaderObj.TaxHdr.Where(hdr => hdr.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (SaleOrderTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    discount = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.SLT_TAX_AMT);
                }
                saleOrderHeaderObj.SOH_TOTAL_DISCOUNT = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;
                if (saleOrderHeaderObj.TaxHdr != null)
                {
                    var taxHeader = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (SaleOrderTaxHdr taxHdrObj in taxHeader)
                    {
                        string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    saleOrderHeaderObj.SOH_TOTAL_TAX = saleOrderHeaderObj.TaxHdr.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SLT_TAX_AMT);
                }
                txtHdrTax.Text = txtHdrTax.ToolTip = saleOrderHeaderObj.SOH_TOTAL_TAX.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtShipping.Text, out shipping);
                saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE = shipping;
                double.TryParse(txtPriceAdj.Text, out adjust);
                saleOrderHeaderObj.SOH_TOTAL_ADJUST = adjust;
                saleOrderHeaderObj.SOH_NET_AMOUNT = Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT) - saleOrderHeaderObj.SOH_TOTAL_DISCOUNT + saleOrderHeaderObj.SOH_TOTAL_TAX
                    + saleOrderHeaderObj.SOH_TOTAL_SHIP_CHARGE + saleOrderHeaderObj.SOH_TOTAL_ADJUST;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = saleOrderHeaderObj.SOH_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                TempSaleOrderHeaderSession = SaleOrderHeaderSession = saleOrderHeaderObj;
            }
            return true;
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
        public string GetFormattedNumberWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithComma.Value);
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
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            path = path + (Request.Url.Query.IndexOf('&') > 0 ? Request.Url.Query.Substring(0, Request.Url.Query.IndexOf('&')).ToLower()
                : Request.Url.Query.ToLower());
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
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
            try
            {
                //Session Logout on Department change
                if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                    return;

                int? result;
                string action;
                DropDownList ddlWkfAction;
                string addr;

                double qty;
                double rate;
                Label lblSubTotal;
                SaleOrderTaxHdr tempSaleOrderTaxHdrObj = null;
                SaleOrderTaxHdr saleOrderTaxHdrObj;
                List<SaleOrderTaxHdr> saleOrderTaxHdrList;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                int brandArtPK;

                IsInternalOrderPrint = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsShowInternalOrderPrint"));
                if (IsInternalOrderPrint == 1)
                    btnPrintIO.Visible = true;
                else
                    btnPrintIO.Visible = false;

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
                    if (((DropDownList)sender).ID == "ddlArtWork")
                    {
                        commonActions = ActionsEnum.ARTWORKSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlNotifyParty")
                    {
                        commonActions = ActionsEnum.NOTIFYPARTYSELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlConsigneeDetails")
                    {
                        commonActions = ActionsEnum.CONSIGNEESELECTED;
                    }
                    else if (((DropDownList)sender).ID == "ddlAgent")
                    {
                        commonActions = ActionsEnum.AGENTSELECTED;
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
                    else if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                }
                if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtRate")
                    {
                        commonActions = ActionsEnum.CALCULATEDTLTAX;
                    }
                }
                switch (commonActions)
                {
                    #region Customer Selected
                    case ActionsEnum.CUSTOMERSELECTED:
                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                            GetFieldValues(ControlsEnum.CUSTOMER);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                txtBuyerAddress.Text = hdfCusAddress.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                hdfCusCountry.Value = dsPageData.Tables[0].Rows[0]["CUS_COUNTRY"].ToString();
                                hdfCusCountryText.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                hdfCusZip.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ZIP"].ToString());
                                hdfCusPhone.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_PHONE"].ToString());
                                hdfCusMobile.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_MOBILE"].ToString());
                                hdfCusFax.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_FAX"].ToString());
                                hdfCusEmail.Value = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_EMAIL"].ToString());

                                txtPortofDischarge.Text = txtToPort.Text = HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_SHIP_TO_PORT"].ToString());

                                hdfCurrency.Value = dsPageData.Tables[0].Rows[0]["CUR_PK"].ToString();
                                txtCurrency.Text = string.Format(Resources.ErpRes.NameCodeFormat, dsPageData.Tables[0].Rows[0]["CUR_CODE"].ToString()
                                    , dsPageData.Tables[0].Rows[0]["CUR_NAME"].ToString());
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                if (exchangeRate > 0)
                                {
                                    hdfExchangeRate.Value = exchangeRate.ToString();
                                }
                                else
                                {
                                    txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                                    hdfCurrency.Value = "0";
                                }
                            }
                            else
                            {
                                ResetForm(ControlsEnum.CUSTOMER);
                            }
                            ResetForm(ControlsEnum.CUSTOMERCONTRACT);
                            GetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                            SetFieldValues(ControlsEnum.CUSTOMERCONTRACT);
                            txtPONo.Focus();
                        }
                        else
                        {
                            txtCustomer.Focus();
                            ResetForm(ControlsEnum.CUSTOMER);
                            ResetForm(ControlsEnum.CUSTOMERCONTRACT);
                        }
                        SaleOrderHeaderSession.SaleContractDetails = new List<SaleContractDetailsBO>();
                        saleOrderHeaderObj = SaleOrderHeaderSession;
                        SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        break;
                    #endregion
                    #region Exchange Rate
                    case ActionsEnum.EXCHANGERATE:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        if (exchangeRate > 0)
                        {
                            hdfExchangeRate.Value = exchangeRate.ToString();
                        }
                        else
                        {
                            txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                            hdfCurrency.Value = "0";
                            litErrorMsg.Text = GetLocalResourceObject("Err_ExchangeRate").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Brand Change
                    case ActionsEnum.PRODUCTSELECTED:
                        if (hdfCustomer.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomer.Value != string.Empty)
                        {
                            custPK = Convert.ToInt32(hdfCustomer.Value);
                        }
                        if (hdfBrand.Value != CommonConstants.SELECT_VALUE_ZERO && hdfBrand.Value != string.Empty)
                        {
                            custprodPK = Convert.ToInt32(hdfBrand.Value);
                            GetFieldValues(ControlsEnum.CUSTOMERPRODUCT);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                txtBrandCode.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CIM_BRAND_CODE"].ToString());
                                txtPacking.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CIM_PACKING_SPEC_NAME"].ToString());
                                hdfPackingSpec.Value = dtPageData.Rows[0]["APS_PK"].ToString();
                                hdfPackingText.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["PACKING_TEXT"].ToString());
                                //hdfArtWork.Value = dtPageData.Rows[0]["PIM_PK"].ToString();
                                //chkNewArtWork.Checked = string.IsNullOrEmpty(hdfArtWork.Value);
                                hdfCBM.Value = dtPageData.Rows[0]["CBM"].Equals(DBNull.Value) ? CommonConstants.SELECT_VALUE_ZERO : dtPageData.Rows[0]["CBM"].ToString();
                                if (!dtPageData.Rows[0]["PIM_PK"].Equals(DBNull.Value))
                                {
                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["PC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkPC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["PC_DOC_PATH"].ToString()))
                                            lnkArtWorkPC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkPC.Disabled = false;
                                            lnkArtWorkPC.HRef = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkPC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["PC_ART_WORK"].ToString());
                                        hdfArtWorkPC.Value = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkPC.HRef = "#";
                                        lnkArtWorkPC.Visible = false;
                                        lnkArtWorkPC.Disabled = true;
                                        hdfArtWorkPC.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IB_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkIB.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["IB_DOC_PATH"].ToString()))
                                            lnkArtWorkIB.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkIB.Disabled = false;
                                            lnkArtWorkIB.HRef = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkIB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IB_ART_WORK"].ToString());
                                        hdfArtWorkIB.Value = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkIB.HRef = "#";
                                        lnkArtWorkIB.Visible = false;
                                        lnkArtWorkIB.Disabled = true;
                                        hdfArtWorkIB.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkIC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["IC_DOC_PATH"].ToString()))
                                            lnkArtWorkIC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkIC.Disabled = false;
                                            lnkArtWorkIC.HRef = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkIC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IC_ART_WORK"].ToString());
                                        hdfArtWorkIC.Value = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkIC.HRef = "#";
                                        lnkArtWorkIC.Visible = false;
                                        lnkArtWorkIC.Disabled = true;
                                        hdfArtWorkIC.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkZB.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_DOC_PATH"].ToString()))
                                            lnkArtWorkZB.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkZB.Disabled = false;
                                            lnkArtWorkZB.HRef = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkZB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ZB_ART_WORK"].ToString());
                                        hdfArtWorkZB.Value = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkZB.HRef = "#";
                                        lnkArtWorkZB.Visible = false;
                                        lnkArtWorkZB.Disabled = true;
                                        hdfArtWorkZB.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["MC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkMC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["MC_DOC_PATH"].ToString()))
                                            lnkArtWorkMC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkMC.Disabled = false;
                                            lnkArtWorkMC.HRef = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkMC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["MC_ART_WORK"].ToString());
                                        hdfArtWorkMC.Value = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();

                                    }
                                    else
                                    {
                                        lnkArtWorkMC.HRef = "#";
                                        lnkArtWorkMC.Visible = false;
                                        lnkArtWorkMC.Disabled = true;
                                        hdfArtWorkMC.Value = string.Empty;
                                    }

                                    if (!string.IsNullOrEmpty(dtPageData.Rows[0]["SC_ART_WORK"].ToString()))
                                    {
                                        lnkArtWorkSC.Visible = true;
                                        if (string.IsNullOrEmpty(dtPageData.Rows[0]["SC_DOC_PATH"].ToString()))
                                            lnkArtWorkSC.Disabled = true;
                                        else
                                        {
                                            lnkArtWorkSC.Disabled = false;
                                            lnkArtWorkSC.HRef = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                        }
                                        lnkArtWorkSC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["SC_ART_WORK"].ToString());
                                        hdfArtWorkSC.Value = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                    }
                                    else
                                    {
                                        lnkArtWorkSC.HRef = "#";
                                        lnkArtWorkSC.Visible = false;
                                        lnkArtWorkSC.Disabled = true;
                                        hdfArtWorkSC.Value = string.Empty;
                                    }
                                }
                                else
                                    ResetForm(ControlsEnum.PACKINGSPEC);
                                txtQty.Text = GetFormattedNumber(dtPageData.Rows[0]["CIM_LAST_ORDR_QTY"].Equals(DBNull.Value) ? CommonConstants.SELECT_VALUE_ZERO : dtPageData.Rows[0]["CIM_LAST_ORDR_QTY"].ToString());
                                hdfUOM.Value = dtPageData.Rows[0]["UOM_PK"].ToString();
                                txtUOM.Text = dtPageData.Rows[0]["UOM_CODE"].ToString();

                                hdfProduct.Value = dtPageData.Rows[0]["ITM_PK"].ToString();
                                txtProduct.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ITM_CODE"].ToString());
                                hdfProductName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ITM_NAME"].ToString());
                                double totalPieces = Convert.ToDouble(dtPageData.Rows[0]["APS_TOTAL_PCS"].Equals(DBNull.Value)
                                    ? CommonConstants.SELECT_VALUE_ONE : dtPageData.Rows[0]["APS_TOTAL_PCS"].ToString());
                                totalPieces = totalPieces > 1 ? totalPieces : 1;
                                txtTotalPiecesCtn.Text = Math.Floor(totalPieces).ToString();

                                brandArtPK = dtPageData.Rows[0]["PIM_PK"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dtPageData.Rows[0]["PIM_PK"]);
                                GetFieldValues(ControlsEnum.PACKINGSPEC);
                                artWorkPK = brandArtPK;
                                SetFieldValues(ControlsEnum.PACKINGSPEC);

                                if (!string.IsNullOrEmpty(txtBookingDate.Text))
                                {
                                    custprodPK = Convert.ToInt32(hdfBrand.Value);
                                    bookingDate = Convert.ToDateTime(txtBookingDate.Text);
                                    GetFieldValues(ControlsEnum.BRANDRATE);
                                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                                    {
                                        txtRate.Text = GetFormattedRate(dtPageData.Rows[0]["BRD_RATE"].ToString());
                                        if (double.TryParse(txtQty.Text, out qty) && double.TryParse(txtRate.Text, out rate))
                                        {
                                            txtAmount.Text = GetFormattedCurrency(qty * rate);
                                        }
                                        else
                                            txtAmount.Text = GetFormattedCurrency(0);
                                    }
                                    else
                                    {
                                        txtRate.Text = GetFormattedRate(0);
                                        txtAmount.Text = GetFormattedCurrency(0);
                                    }
                                }
                                else
                                {
                                    txtRate.Text = GetFormattedRate(0);
                                    txtAmount.Text = GetFormattedCurrency(0);
                                }
                            }
                            else
                                ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                        }
                        else
                            ResetForm(ControlsEnum.CUSTOMERPRODUCT);
                        break;
                    #endregion
                    #region Booking Date Change
                    case ActionsEnum.BOOKINGDATECHANGE:
                        if (hdfBrand.Value != CommonConstants.SELECT_VALUE_ZERO && hdfBrand.Value != string.Empty
                            && !string.IsNullOrEmpty(txtBookingDate.Text))
                        {
                            custprodPK = Convert.ToInt32(hdfBrand.Value);
                            bookingDate = Convert.ToDateTime(txtBookingDate.Text);
                            GetFieldValues(ControlsEnum.BRANDRATE);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                txtRate.Text = GetFormattedRate(dtPageData.Rows[0]["BRD_RATE"].ToString());
                                if (double.TryParse(txtQty.Text, out qty) && double.TryParse(txtRate.Text, out rate))
                                {
                                    txtAmount.Text = GetFormattedCurrency(qty * rate);
                                }
                                else
                                    txtAmount.Text = GetFormattedCurrency(0);
                            }
                            else
                            {
                                txtRate.Text = GetFormattedRate(0);
                                txtAmount.Text = GetFormattedCurrency(0);
                            }
                        }
                        else
                        {
                            txtRate.Text = GetFormattedRate(0);
                            txtAmount.Text = GetFormattedCurrency(0);
                        }
                        break;
                    #endregion
                    #region Add Item
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            SaleOrderHeaderSession = TempSaleOrderHeaderSession;
                            saleOrderDetailsList = SaleOrderHeaderSession.SaleContractDetails;
                            saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                            if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                            {
                                SaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                                SetDetailTax(TempSaleOrderHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                saleOrderHeaderObj = SaleOrderHeaderSession;
                                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                                ResetForm(ControlsEnum.SALEORDERDETAIL);
                            }
                        }
                        break;
                    #endregion
                    #region Remove Item
                    case ActionsEnum.REMOVEITEM:
                        if (SaleOrderHeaderSession.SaleContractDetails != null && SaleOrderHeaderSession.SaleContractDetails.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                SaleOrderHeaderSession.SaleContractDetails = SaleOrderHeaderSession.SaleContractDetails.Where(row => CurrSlNo != row.SOD_SL_NO).ToList();
                                SetDetailTax(SaleOrderHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                saleOrderHeaderObj = SaleOrderHeaderSession;
                                SetFieldValues(ControlsEnum.SALEORDERDETAIL);
                            }
                        }
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        break;
                    #endregion
                    #region Edit Item
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        if (SaleOrderHeaderSession.SaleContractDetails != null && SaleOrderHeaderSession.SaleContractDetails.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                saleOrderDetailsObj = SaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                            }
                        }
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        break;
                    #endregion
                    #region Clear Item
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.SALEORDERDETAIL);
                        break;
                    #endregion
                    #region Atrwork Selected
                    case ActionsEnum.ARTWORKSELECTED:
                        artWorkPK = Convert.ToInt32(ddlArtWork.SelectedValue);
                        if (artWorkPK > 0)
                        {
                            GetFieldValues(ControlsEnum.PACKINGSPEC);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["PC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkPC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["PC_DOC_PATH"].ToString()))
                                        lnkArtWorkPC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkPC.Disabled = false;
                                        lnkArtWorkPC.HRef = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkPC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["PC_ART_WORK"].ToString());
                                    hdfArtWorkPC.Value = dtPageData.Rows[0]["PC_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IB_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkIB.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["IB_DOC_PATH"].ToString()))
                                        lnkArtWorkIB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIB.Disabled = false;
                                        lnkArtWorkIB.HRef = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkIB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IB_ART_WORK"].ToString());
                                    hdfArtWorkIB.Value = dtPageData.Rows[0]["IB_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["IC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkIC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["IC_DOC_PATH"].ToString()))
                                        lnkArtWorkIC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkIC.Disabled = false;
                                        lnkArtWorkIC.HRef = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkIC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["IC_ART_WORK"].ToString());
                                    hdfArtWorkIC.Value = dtPageData.Rows[0]["IC_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkZB.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["ZB_DOC_PATH"].ToString()))
                                        lnkArtWorkZB.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkZB.Disabled = false;
                                        lnkArtWorkZB.HRef = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkZB.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["ZB_ART_WORK"].ToString());
                                    hdfArtWorkZB.Value = dtPageData.Rows[0]["ZB_DOC_PATH"].ToString();
                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["MC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkMC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["MC_DOC_PATH"].ToString()))
                                        lnkArtWorkMC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkMC.Disabled = false;
                                        lnkArtWorkMC.HRef = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkMC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["MC_ART_WORK"].ToString());
                                    hdfArtWorkMC.Value = dtPageData.Rows[0]["MC_DOC_PATH"].ToString();

                                }

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["SC_ART_WORK"].ToString()))
                                {
                                    lnkArtWorkSC.Visible = true;
                                    if (string.IsNullOrEmpty(dtPageData.Rows[0]["SC_DOC_PATH"].ToString()))
                                        lnkArtWorkSC.Disabled = true;
                                    else
                                    {
                                        lnkArtWorkSC.Disabled = false;
                                        lnkArtWorkSC.HRef = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                    }
                                    lnkArtWorkSC.InnerText = HttpUtility.HtmlDecode(dtPageData.Rows[0]["SC_ART_WORK"].ToString());
                                    hdfArtWorkSC.Value = dtPageData.Rows[0]["SC_DOC_PATH"].ToString();
                                }
                            }
                            else
                                ResetForm(ControlsEnum.PACKINGSPEC);
                        }
                        else
                            ResetForm(ControlsEnum.PACKINGSPEC);
                        break;
                    #endregion
                    #region Notify Party
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
                    #endregion
                    #region Consignee
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
                    #endregion
                    #region Agent
                    case ActionsEnum.AGENTSELECTED:
                        agentPK = Convert.ToInt32(ddlAgent.SelectedValue);
                        if (agentPK > 0)
                        {
                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                addr = string.Empty;
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                    hdfAgentName.Value = dtPageData.Rows[0]["CAD_NAME"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                    hdfAgentCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

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
                                hdfAgentAddress.Value = addr;

                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                    hdfAgentCountryText.Value = dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                    hdfAgentEmail.Value = dtPageData.Rows[0]["CAD_EMAIL"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                    hdfAgentFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                    hdfAgentMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                    hdfAgentPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                                if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                    hdfAgentZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                            }
                            else
                            {
                                hdfAgentName.Value = string.Empty;
                                hdfAgentAddress.Value = string.Empty;
                                hdfAgentCountry.Value = string.Empty;
                                hdfAgentCountryText.Value = string.Empty;
                                hdfAgentEmail.Value = string.Empty;
                                hdfAgentFax.Value = string.Empty;
                                hdfAgentMobile.Value = string.Empty;
                                hdfAgentPhone.Value = string.Empty;
                                hdfAgentZip.Value = string.Empty;
                            }
                        }
                        else
                        {
                            hdfAgentName.Value = string.Empty;
                            hdfAgentAddress.Value = string.Empty;
                            hdfAgentCountry.Value = string.Empty;
                            hdfAgentCountryText.Value = string.Empty;
                            hdfAgentEmail.Value = string.Empty;
                            hdfAgentFax.Value = string.Empty;
                            hdfAgentMobile.Value = string.Empty;
                            hdfAgentPhone.Value = string.Empty;
                            hdfAgentZip.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Shipping Address
                    case ActionsEnum.ADDRESSSELECTED:
                        if (ddlCustAddress.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            addressPK = Convert.ToInt32(ddlCustAddress.SelectedValue);
                            addressActive = 2;
                            GetFieldValues(ControlsEnum.CUSTOMERADDRESS);
                        }
                        if (addressPK > 0 && dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfShpAddress.Value = txtShippingAddress.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_ADDRESS"].ToString()) +
                                (string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_CITY"].ToString()) ? string.Empty
                                    : ", " + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_CITY"].ToString())) +
                                    (string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString()) ? string.Empty
                                    : ", " + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_STATE_TEXT"].ToString())) +
                                    (string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()) ? string.Empty
                                    : ", " + HttpUtility.HtmlDecode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()));

                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_NAME"].ToString()))
                                hdfShpName.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_NAME"].ToString());
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY"].ToString()))
                                hdfShpCountry.Value = dtPageData.Rows[0]["CAD_COUNTRY"].ToString();

                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString()))
                                hdfShpCountryText.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_COUNTRY_TEXT"].ToString());
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_EMAIL"].ToString()))
                                hdfShpEmail.Value = HttpUtility.HtmlEncode(dtPageData.Rows[0]["CAD_EMAIL"].ToString());
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_FAX"].ToString()))
                                hdfShpFax.Value = dtPageData.Rows[0]["CAD_FAX"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_MOBILE"].ToString()))
                                hdfShpMobile.Value = dtPageData.Rows[0]["CAD_MOBILE"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_PHONE"].ToString()))
                                hdfShpPhone.Value = dtPageData.Rows[0]["CAD_PHONE"].ToString();
                            if (!string.IsNullOrEmpty(dtPageData.Rows[0]["CAD_ZIP"].ToString()))
                                hdfShpZip.Value = dtPageData.Rows[0]["CAD_ZIP"].ToString();
                        }
                        else
                        {
                            hdfShpAddress.Value = txtShippingAddress.Text = hdfShpName.Value = hdfShpCountry.Value = hdfShpCountryText.Value =
                                hdfShpEmail.Value = hdfShpFax.Value = hdfShpMobile.Value = hdfShpPhone.Value = hdfShpZip.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region Delivery Terms
                    case ActionsEnum.DELTERMSELECTED:
                        if (ddlDeliveryTerms.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            deliveryTermPK = Convert.ToInt32(ddlDeliveryTerms.SelectedValue);
                            GetFieldValues(ControlsEnum.DELIVERYTERMSBYPK);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtDeliveryTerms.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtDeliveryTerms.Text = string.Empty;
                        break;
                    #endregion
                    #region Payment Terms
                    case ActionsEnum.PAYTERMSELECTED:
                        if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            paymentTermPK = Convert.ToInt32(ddlPaymentTerms.SelectedValue);
                            GetFieldValues(ControlsEnum.PAYMENTTERMSBYPK);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtPaymentTerms.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtPaymentTerms.Text = string.Empty;
                        break;
                    #endregion
                    #region Special Cause
                    case ActionsEnum.SPECAUSESELECTED:
                        if (ddlSpecialCause.SelectedValue != CommonConstants.SELECTVAL)
                        {
                            specialCausePK = Convert.ToInt32(ddlSpecialCause.SelectedValue);
                            GetFieldValues(ControlsEnum.SPECIALCAUSEBYPK);
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtSpecialCause.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["DTL_NAME_TEXT"].ToString());
                        }
                        else
                            txtSpecialCause.Text = string.Empty;
                        break;
                    #endregion
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (SaleOrderHeaderSession.SaleContractDetails == null || SaleOrderHeaderSession.SaleContractDetails.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            saleOrderHeaderObj = new SaleContractBO();
                            saleOrderHeaderObj = (SaleContractBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);

                            if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleContractDetails != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<SaleContractBO>(saleOrderHeaderObj);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
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
                                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InternalOrderListing + (Request.Url.Query.IndexOf('&') > 0 ?
                                        Request.Url.Query.Substring(0, Request.Url.Query.IndexOf('&')).ToLower() : Request.Url.Query.ToLower())) + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion                    
                    #region SAVESUBMIT popup
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT Popup
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WorkFlow Submit
                    case ActionsEnum.WRKFSUBMIT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                saleOrderHeaderObj = new SaleContractBO();
                                saleOrderHeaderObj = (SaleContractBO)SetUIValuesToObject(ControlsEnum.SALEORDERHEADER);
                                if (saleOrderHeaderObj != null && saleOrderHeaderObj.SaleContractDetails != null)
                                {
                                    string xmlDoc = CommonFunctions.XmlSerialize<SaleContractBO>(saleOrderHeaderObj);//CommonFunctions.ObjectTOXml(saleOrderHeaderObj);
                                    // save Process Control inspection details
                                    result = BusinessLogic.Sales.SaleOrderBL.SaveSaleOrderDetails(xmlDoc);
                                    if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                                    {
                                        Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                                        Session[ERP.Utilities.SessionStrings.SaleOrderMode] = null;
                                        ucrWrkf.ApplicationID = result.Value;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
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
                                    if (result.HasValue && result.Value > 0)
                                    {
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        //Show Save success message and reset Contract Entry
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Ack_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes","InternalOrder").ToString());


                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InternalOrderListing + (Request.Url.Query.IndexOf('&') > 0 ?
                                            Request.Url.Query.Substring(0, Request.Url.Query.IndexOf('&')).ToLower() : Request.Url.Query.ToLower())) + "');", true);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINTIO:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region TAXDETAILS
                    case ActionsEnum.TAXDETAILS:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;

                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                            saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                    TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                    && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);
                            if (saleOrderDetailsObj == null)
                            {
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                            }

                            if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (TempSaleOrderHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;

                                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
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
                        break;
                    #endregion
                    #region DISCDETAILS
                    case ActionsEnum.DISCDETAILS:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            saleOrderDetailsList = TempSaleOrderHeaderSession.SaleContractDetails;
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);

                            saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails == null ? null :
                                    TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                    && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);
                            if (saleOrderDetailsObj == null)
                            {
                                saleOrderDetailsList = (List<SaleContractDetailsBO>)SetUIValuesToObject(ControlsEnum.SALEORDERDETAIL);
                                TempSaleOrderHeaderSession.SaleContractDetails = saleOrderDetailsList;
                            }
                            if (saleOrderDetailsList != null && saleOrderDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                                if (TempSaleOrderHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;
                                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
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
                        break;
                    #endregion
                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (SaleOrderHeaderSession != null)
                        {
                            TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                double taxable = Convert.ToDouble(SaleOrderHeaderSession.SOH_TOTAL_AMT) - SaleOrderHeaderSession.SOH_TOTAL_DISCOUNT;
                                txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
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
                        break;
                    #endregion
                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (SaleOrderHeaderSession != null)
                        {
                            TempSaleOrderHeaderSession = SaleOrderHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(lblSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(lblSubTotal.Text).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
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
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        if (IsHeaderTax)
                        {
                            SaleOrderHeaderSession = TempSaleOrderHeaderSession;
                            SetSubTotal();
                            SetHdrTax();
                        }
                        else
                        {
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
                            saleOrderDetailsObj = TempSaleOrderHeaderSession.SaleContractDetails.SingleOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                            && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK);
                            if (saleOrderDetailsObj != null)
                            {
                                CurrSlNo = saleOrderDetailsObj.SOD_SL_NO;
                            }
                            SetDetailTax(TempSaleOrderHeaderSession);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        //SelectedDtlPK
                        //SelectedCusItemPK
                        //SelectedItemPK


                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempSaleOrderHeaderSession != null)
                        {
                            saleOrderHeaderObj = TempSaleOrderHeaderSession;
                            tempSaleOrderTaxHdrObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                        saleOrderHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.SLT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && ctr.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                        saleOrderHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.SLT_NAME == txtPopupOther.Text.Trim() && ctr.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                    saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(ctr => ctr.SOD_PK == SelectedDtlPK
                                    && ctr.SOD_CUST_ITEM == SelectedCusItemPK && ctr.SOD_ITEM == SelectedItemPK);

                                if (saleOrderDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                            saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                            saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == txtPopupOther.Text.Trim() && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempSaleOrderTaxHdrObj == null)
                            {
                                saleOrderTaxHdrList = new List<SaleOrderTaxHdr>();

                                saleOrderTaxHdrObj = new SaleOrderTaxHdr();
                                try
                                {
                                    saleOrderTaxHdrObj.SLT_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    saleOrderTaxHdrObj.SLT_SO_DTL = SelectedDtlPK;
                                    saleOrderTaxHdrObj.SLT_SL_NO = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        saleOrderTaxHdrObj.SLT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    saleOrderTaxHdrObj.SLT_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    saleOrderTaxHdrObj.SLT_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    saleOrderTaxHdrObj.SLT_PK = 0;
                                    //quotationTaxHdrObj.SLT_TAX_CATEGORY_TEXT = "Tax";
                                    saleOrderTaxHdrObj.SLT_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    saleOrderTaxHdrObj.SLT_TYPE = 1;
                                    saleOrderTaxHdrObj.SLT_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (saleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(saleOrderHeaderObj.SOH_TOTAL_AMT);
                                            currentTotal = saleOrderHeaderObj.TaxHdr == null ? 0 :
                                                saleOrderHeaderObj.TaxHdr.Where(htx => htx.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SLT_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(saleOrderTaxHdrObj.SLT_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = saleOrderTaxHdrObj.SLT_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                                                saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                                saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                                            saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                            saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                            saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(item => item.SOD_PK == SelectedDtlPK
                                            && item.SOD_CUST_ITEM == SelectedCusItemPK && item.SOD_ITEM == SelectedItemPK);
                                        if (saleOrderDetailsObj != null)
                                        {
                                            if (saleOrderTaxHdrObj.SLT_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = saleOrderDetailsObj.SOD_AMOUNT;
                                                currentTotal = saleOrderDetailsObj.TaxDtl == null ? 0 :
                                                    saleOrderDetailsObj.TaxDtl.Where(dtx => dtx.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SLT_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(saleOrderTaxHdrObj.SLT_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = saleOrderTaxHdrObj.SLT_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl == null ? new List<SaleOrderTaxHdr>() : saleOrderDetailsObj.TaxDtl.ToList();
                                                    saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                                    saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                        && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl == null ? new List<SaleOrderTaxHdr>() : saleOrderDetailsObj.TaxDtl.ToList();
                                                saleOrderTaxHdrList.Add(saleOrderTaxHdrObj);
                                                saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                    && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;
                                            }
                                        }
                                    }
                                    TempSaleOrderHeaderSession = saleOrderHeaderObj;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
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
                        if (TempSaleOrderHeaderSession != null)
                        {
                            saleOrderHeaderObj = TempSaleOrderHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                saleOrderTaxHdrList = new List<SaleOrderTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                            saleOrderHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempSaleOrderTaxHdrObj = saleOrderHeaderObj.TaxHdr == null ? null :
                                                saleOrderHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempSaleOrderTaxHdrObj != null)
                                    {
                                        saleOrderTaxHdrList = saleOrderHeaderObj.TaxHdr.ToList();
                                        saleOrderTaxHdrList.Remove(tempSaleOrderTaxHdrObj);
                                        saleOrderHeaderObj.TaxHdr = saleOrderTaxHdrList;
                                    }
                                }
                                else
                                {
                                    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails == null ? null :
                                        saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                        && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK);
                                    if (saleOrderDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                                saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempSaleOrderTaxHdrObj = saleOrderDetailsObj.TaxDtl == null ? null :
                                                    saleOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                            && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK);
                                        if (saleOrderDetailsObj != null && saleOrderDetailsObj.TaxDtl != null)
                                        {
                                            saleOrderTaxHdrList = saleOrderDetailsObj.TaxDtl.ToList();
                                            saleOrderTaxHdrList.Remove(tempSaleOrderTaxHdrObj);
                                            saleOrderHeaderObj.SaleContractDetails.SingleOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                                && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = saleOrderTaxHdrList;
                                        }
                                    }
                                }

                                TempSaleOrderHeaderSession = saleOrderHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

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
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtSaleOrderTaxDetails != null && dtSaleOrderTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtSaleOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                SelectedTaxText = HttpUtility.HtmlEncode(dtSaleOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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
                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        SetDetailTax(TempSaleOrderHeaderSession);
                        //SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.SALEORDERHEADER);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_GoBack", "GoBack();", true);
                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = ApplicationType.SO;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblSaleOrderNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtSaleOrderDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtSaleOrderDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        ucrAlert.TypePartyName = txtCustomer.Text.Trim();
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
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
            SaleContractDetailsBO scItem;

            Label lblItemCartonsOrBags;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkPC;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkIB;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkIC;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkZB;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkMC;
            System.Web.UI.HtmlControls.HtmlAnchor lnkLstArtWorkSC;

            Label lblItemTotalQty;
            Label lblItemTotalCarton;
            Label lblItemTotalAmount;
            Label lblDiscountTotal;
            Label lblTaxTotal;
            Label lblSubTotalFooter;

            try
            {
                if ((sender as GridView).ID == "grdItemDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        e.Row.Cells[10].Visible = Convert.ToBoolean(ShowLotNo);
                        e.Row.Cells[11].Visible = Convert.ToBoolean(ShowLotSize);

                        e.Row.Cells[7].Visible = EnableItemDiscount;
                        e.Row.Cells[8].Visible = EnableItemTax;

                        lblItemCartonsOrBags = e.Row.FindControl("lblItemCartonsOrBags") as Label;
                        lnkLstArtWorkPC = e.Row.FindControl("lnkLstArtWorkPC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkIB = e.Row.FindControl("lnkLstArtWorkIB") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkIC = e.Row.FindControl("lnkLstArtWorkIC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkZB = e.Row.FindControl("lnkLstArtWorkZB") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkMC = e.Row.FindControl("lnkLstArtWorkMC") as System.Web.UI.HtmlControls.HtmlAnchor;
                        lnkLstArtWorkSC = e.Row.FindControl("lnkLstArtWorkSC") as System.Web.UI.HtmlControls.HtmlAnchor;

                        scItem = (SaleContractDetailsBO)e.Row.DataItem;
                        if (scItem != null)
                        {
                            if (lblItemCartonsOrBags != null)
                            {
                                if (scItem.SOD_IS_PACK_MAT != 1 && scItem.SOD_IS_PACK_MAT != 2)
                                {

                                    if (CartonDecimal > 0)
                                        lblItemCartonsOrBags.Text = GetFormattedNumberWithComma(Math.Round((scItem.SOD_QTY / scItem.APS_TOTAL_PCS), CartonDecimal));
                                    else
                                        lblItemCartonsOrBags.Text = Math.Ceiling(scItem.SOD_QTY / scItem.APS_TOTAL_PCS).ToString();
                                }
                                else
                                {
                                    lblItemCartonsOrBags.Text = "-";
                                }
                                lblItemCartonsOrBags.ToolTip = HttpUtility.HtmlDecode(scItem.PACKING_TEXT);
                            }
                            if (lnkLstArtWorkPC != null)
                            {
                                lnkLstArtWorkPC.Visible = !string.IsNullOrEmpty(scItem.PC_ART_WORK);
                                if (string.IsNullOrEmpty(scItem.PC_DOC_PATH))
                                    lnkLstArtWorkPC.Disabled = true;
                                else
                                    lnkLstArtWorkPC.HRef = scItem.PC_DOC_PATH;
                                lnkLstArtWorkPC.InnerText = lnkLstArtWorkPC.Title = scItem.PC_ART_WORK;

                            }
                            if (lnkLstArtWorkIB != null)
                            {
                                lnkLstArtWorkIB.Visible = !string.IsNullOrEmpty(scItem.IB_ART_WORK);
                                if (string.IsNullOrEmpty(scItem.IB_DOC_PATH))
                                    lnkLstArtWorkIB.Disabled = true;
                                else
                                    lnkLstArtWorkIB.HRef = scItem.IB_DOC_PATH;
                                lnkLstArtWorkIB.InnerText = lnkLstArtWorkIB.Title = scItem.IB_ART_WORK;
                            }
                            if (lnkLstArtWorkIC != null)
                            {
                                lnkLstArtWorkIC.Visible = !string.IsNullOrEmpty(scItem.IC_ART_WORK);
                                if (string.IsNullOrEmpty(scItem.IC_DOC_PATH))
                                    lnkLstArtWorkIC.Disabled = true;
                                else
                                    lnkLstArtWorkIC.HRef = scItem.IC_DOC_PATH;
                                lnkLstArtWorkIC.InnerText = lnkLstArtWorkIC.Title = scItem.IC_ART_WORK;
                            }
                            if (lnkLstArtWorkZB != null)
                            {
                                lnkLstArtWorkZB.Visible = !string.IsNullOrEmpty(scItem.ZB_ART_WORK);
                                if (string.IsNullOrEmpty(scItem.ZB_DOC_PATH))
                                    lnkLstArtWorkZB.Disabled = true;
                                else
                                    lnkLstArtWorkZB.HRef = scItem.ZB_DOC_PATH;
                                lnkLstArtWorkZB.InnerText = lnkLstArtWorkZB.Title = scItem.ZB_ART_WORK;
                            }
                            if (lnkLstArtWorkMC != null)
                            {
                                lnkLstArtWorkMC.Visible = !string.IsNullOrEmpty(scItem.MC_ART_WORK);
                                if (string.IsNullOrEmpty(scItem.MC_DOC_PATH))
                                    lnkLstArtWorkMC.Disabled = true;
                                else
                                    lnkLstArtWorkMC.HRef = scItem.MC_DOC_PATH;
                                lnkLstArtWorkMC.InnerText = lnkLstArtWorkMC.Title = scItem.MC_ART_WORK;
                            }
                            if (lnkLstArtWorkSC != null)
                            {
                                lnkLstArtWorkSC.Visible = !string.IsNullOrEmpty(scItem.SC_ART_WORK);
                                if (string.IsNullOrEmpty(scItem.SC_DOC_PATH))
                                    lnkLstArtWorkSC.Disabled = true;
                                else
                                    lnkLstArtWorkSC.HRef = scItem.SC_DOC_PATH;
                                lnkLstArtWorkSC.InnerText = lnkLstArtWorkSC.Title = scItem.SC_ART_WORK;
                            }
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer && saleOrderDetailsList != null)
                    {
                        e.Row.Cells[7].Visible = EnableItemDiscount;
                        e.Row.Cells[8].Visible = EnableItemTax;

                        e.Row.Cells[10].Visible = Convert.ToBoolean(ShowLotNo);
                        e.Row.Cells[11].Visible = Convert.ToBoolean(ShowLotSize);

                        lblItemTotalQty = e.Row.FindControl("lblItemTotalQty") as Label;
                        lblItemTotalCarton = e.Row.FindControl("lblItemTotalCarton") as Label;
                        lblItemTotalAmount = e.Row.FindControl("lblItemTotalAmount") as Label;
                        lblDiscountTotal = e.Row.FindControl("lblDiscountTotal") as Label;
                        lblTaxTotal = e.Row.FindControl("lblTaxTotal") as Label;
                        lblSubTotalFooter = e.Row.FindControl("lblSubTotalFooter") as Label;

                        if (lblItemTotalQty != null)
                        {
                           // lblItemTotalQty.Text = lblItemTotalQty.ToolTip = saleOrderDetailsList.Sum(itm => itm.SOD_QTY).ToString("N");
                            lblItemTotalQty.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithComma(saleOrderDetailsList.Sum(itm => itm.SOD_QTY));
                        }
                        if (lblItemTotalCarton != null)
                        {
                            if (saleOrderDetailsList.Sum(itm => itm.APS_TOTAL_PCS) > 0)
                            {
                                if (CartonDecimal > 0)
                                    lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = GetFormattedNumberWithComma(Math.Ceiling(Math.Round(saleOrderDetailsList.Sum(itm => Math.Round((itm.SOD_QTY / itm.APS_TOTAL_PCS), CartonDecimal)), CartonDecimal)));
                                else
                                {
                                    //lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = saleOrderDetailsList.Sum(itm => Math.Ceiling(itm.SOD_QTY / itm.APS_TOTAL_PCS)).ToString();
                                    //lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = saleOrderDetailsList.Sum(itm => Math.Ceiling(itm.SOD_QTY / itm.APS_TOTAL_PCS)).ToString();
                                    double val = 0;
                                    foreach (var item in saleOrderDetailsList)
                                    {
                                        val += item.APS_TOTAL_PCS == 0 ? 0 : item.SOD_QTY / item.APS_TOTAL_PCS;
                                    }
                                    lblItemTotalCarton.Text = lblItemTotalCarton.ToolTip = val.ToString();
                                }
                            }
                            else
                            {
                                lblItemTotalCarton.Text = "-";
                            }

                        }
                        if (lblItemTotalAmount != null)
                        {
                            lblItemTotalAmount.Text = lblItemTotalAmount.ToolTip = saleOrderDetailsList.Sum(itm => itm.SOD_AMOUNT).ToString("N");
                        }
                        if (lblDiscountTotal != null)
                        {
                            lblDiscountTotal.Text = lblDiscountTotal.ToolTip = saleOrderDetailsList.Sum(itm => itm.SOD_DISCOUNT).ToString("N");
                        }
                        if (lblTaxTotal != null)
                        {
                            lblTaxTotal.Text = lblTaxTotal.ToolTip = saleOrderDetailsList.Sum(itm => itm.SOD_TAX).ToString("N");
                        }
                        if (lblSubTotalFooter != null)
                        {
                            lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = saleOrderDetailsList.Sum(itm => itm.SOD_NET_AMOUNT).ToString("N");
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[7].Visible = EnableItemDiscount;
                        e.Row.Cells[8].Visible = EnableItemTax;

                        e.Row.Cells[10].Visible = Convert.ToBoolean(ShowLotNo);
                        e.Row.Cells[11].Visible = Convert.ToBoolean(ShowLotSize);
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
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrintIO.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnPrintIO.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
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
                    IsInternalOrderPrint = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsShowInternalOrderPrint"));
                    if (IsInternalOrderPrint == 1)
                        btnPrintIO.Visible = true;
                    else
                        btnPrintIO.Visible = false;
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
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
            CUSTOMER,
            //ARTWORK,
            INSPECTION,
            EXPORTDOC,
            CONTRACTTERMS,
            TAXPOPUPGRID,
            EXCHANGERATE,
            CUSTOMERADDRESS,
            CUSTOMERCONTRACT,
            COPYCONTRACT,
            BRANDRATE,
            CUSTOMERPRODUCT,
            SELECTEDITEM,
            TAXTYPES,
            TAXSETTINGS,
            PACKINGSPEC,
            REFIDSTATUS,
            DELIVERYTERMSBYPK,
            PAYMENTTERMSBYPK,
            SPECIALCAUSEBYPK,
            COMPANY
        }
        private enum WorkFlowStatusEnum
        {
            Reviewed = 4,
            SendBackForApprove = 11
        }

        #endregion
    }
}