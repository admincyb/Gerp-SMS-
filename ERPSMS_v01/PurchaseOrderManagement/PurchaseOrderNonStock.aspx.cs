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
using ERPService;
using ERPData;
using BusinessLogic.PurchaseOrderManagement;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderNonStock : ERP.Store.UI.WorkFlowBasePage
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

        //private List<PTaxDetails> TaxHdr
        //{
        //    get
        //    {
        //        return ViewState["TaxHdr"] == null ? new List<PTaxDetails>() : (List<PTaxDetails>)ViewState["TaxHdr"];
        //    }
        //    set
        //    {
        //        ViewState["TaxHdr"] = value;
        //    }
        //}

        //private List<PTaxDetails> TaxDetails
        //{
        //    get
        //    {
        //        return ViewState["TaxDetails"] == null ? new List<PTaxDetails>() : (List<PTaxDetails>)ViewState["TaxDetails"];
        //    }
        //    set
        //    {
        //        ViewState["TaxDetails"] = value;
        //    }
        //}
        /// <summary>
        /// Is Tax Add
        /// </summary>
        private int isTaxAdd
        {
            get
            {
                return this.ViewState["isTaxAdd"] == null ? 0 : Convert.ToInt32(this.ViewState["isTaxAdd"]);
            }
            set
            {
                this.ViewState["isTaxAdd"] = value;
            }
        }
        /// <summary>
        /// Is Discount Add
        /// </summary>
        private int isDiscountAdd
        {
            get
            {
                return this.ViewState["isDiscountAdd"] == null ? 0 : Convert.ToInt32(this.ViewState["isDiscountAdd"]);
            }
            set
            {
                this.ViewState["isDiscountAdd"] = value;
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

        private string refID;
        private string inboxFlag;

        private POHeader POHeaderlist;
        private static int slno = 1;
        private static List<PTaxDetails> TaxHdr = new List<PTaxDetails>();
        private static List<PTaxDetails> TaxDetails = new List<PTaxDetails>();

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private CommonService commonServiceObj;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private RFQResponseHeader rfqResponseHeaderObj;
        private PONonStock purchaseOrderNonStockObj;
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
        DataTable dtPOType;
        DataSet dsExchangeRate;
        DataTable dtPOHistory;
        private int venPk;
        private int servicePk;

        private List<INV_UOM_MST> listUom;
        private List<PUR_VENDOR_MST> listVendor;
        private List<INV_ITEM_VENDOR_MAP> listVendorItems;
        private INV_ITEM_MST ObjItemDetails;

        bool hasValidRate;
        bool isCancelled = false;
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
		(a1, a2) => a1 - a2,
		(a1, a2) => a1 + a2,
		(a1, a2) => a1 / a2,
		(a1, a2) => a1 * a2,
		(a1, a2) => Math.Pow(a1, a2)
	};

        private static DataTable dsPODetails;
        private DataTable dtCompany;

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
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    dsPODetails = null;
                    slno = 1;
                    TaxHdr = new List<PTaxDetails>();
                    TaxDetails = new List<PTaxDetails>();
                    divCalc.Visible = false;
                    lblRFQNo.Text = Resources.Messages.DocGenerationNew;
                    txtResponseDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtRFQDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtReqByDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfDecimalFormat.Value = "#0.";
                    //for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    //{
                    //    hdfDecimalFormat.Value += "0";
                    //}
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperator.Value += "0";
                    }
                    hdfNumberDigits.Value = NoDecimalDigitsP2P.ToString();
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperator.Value += "0";
                    }
                    hdfRateDigits.Value = "#0.";
                    int rate = Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P].ToString()) : 4;
                    for (int i = 0; i < rate; i++)
                    {
                        hdfRateDigits.Value += "0";
                    }
                    vreRate.DecimalDigits = rate;
                    GetFieldValues(ControlsEnum.POTYPE);
                    SetFieldValues(ControlsEnum.POTYPE);

                    GetFieldValues(ControlsEnum.VENDOR);
                    SetFieldValues(ControlsEnum.VENDOR);
                    EntryStatus = EntryStatus.ENTRYMODE;

                    grdRFQResponse.DataSource = null;
                    grdRFQResponse.DataBind();


                    FillProcessID((int)POWorkflowType.PO);

                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                   : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        // base.WkfRefID = 
                        ucrWrkf.RefID = int.Parse(refID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }

                    if (CurrPK > 0)
                    {
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        //base.WkfRefID = 
                        ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();

                        EntryStatus = EntryStatus.ENTRYMODE;
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        ucrWrkf.ViewAction();

                        FillPODetails(CurrPK);
                        btnRevision.Visible = true;
                    }


                    if (Request.QueryString["POID"] != null)
                    {
                        FillPODetails(Convert.ToInt32(Request.QueryString["POID"].ToString()));
                    }
                    if (Request.QueryString["Status"] != null)
                    {
                        EntryStatus = EntryStatus.VIEWMODE;
                    }
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
            CommonService cm;
            AdmCompanyMstService admCompanyMstServiceClient;
            ServiceUtility serviceUtilityObj;

            DataSet dsRFQTaxDetails;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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
                        //To get company related to current SBU
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    case ControlsEnum.RFQHEADER:
                        dsRFQHeader = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQHeader(CurrPK, Convert.ToByte(DbActiveStatus.HASPK), currentUser.SBUID);
                        break;
                    case ControlsEnum.VENDOR:
                        dsVendor = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetVendorList(CurrPK);
                        break;
                    case ControlsEnum.RFQ:
                        VendorPK = hdfVendor.Value != "" ? Convert.ToInt32(hdfVendor.Value) : 0;
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
                                if (GetGlobalResourceObject("ConfigurationsRes", "IsTaxNotDueForMaterialPO").ToString() == "1")//Should show all purchase related taxes in both  Service & Normal PO.    
                                {
                                    dtRFQTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtResponseDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Include, 1);
                                }
                                else
                                {
                                    dtRFQTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtResponseDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Only, 1);
                                }
                            }
                            else
                            {
                                dtRFQTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtResponseDate.Text), 0);
                            }
                        }
                        break;
                    case ControlsEnum.EXCHANGERATE:
                        dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrencyTxt.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtResponseDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "1";
                        }
                        break;
                    case ControlsEnum.UOM:
                        cm = new CommonService();
                        int Pk = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;
                        if (Pk > 0)
                        {
                            listUom = cm.GetUOM(Pk);
                        }
                        break;
                    case ControlsEnum.VENDORDETAILS:
                        cm = new CommonService();
                        listVendor = cm.GetVendor(venPk);
                        break;
                    case ControlsEnum.POTYPE:
                        dtPOType = PurchaseOrderGenerateBL.GetPurchaseType(currentUser, GetLocalResourceObject("POTYPE").ToString(), Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    case ControlsEnum.VENDORITEMDETAILS:
                        cm = new CommonService();
                        listVendorItems = cm.GetVendorItems(venPk, servicePk);
                        break;

                    case ControlsEnum.ITEMDETAILS:
                        cm = new CommonService();
                        ObjItemDetails = cm.GetItemDetails(servicePk);
                        break;
                    case ControlsEnum.REVISIONHISTORY:
                        dtPOHistory = PurchaseOrderGenerateBL.GetPONonStockHistory(CurrPK);
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
                    case ControlsEnum.COMPANY:
                        BindDropDown(controlType);
                        break;
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
                    case ControlsEnum.UOM:
                        if (listUom != null && listUom.Count > 0)
                        {
                            txtUOM.Text = listUom[0].UOM_CODE;
                            hdfUOM.Value = listUom[0].UOM_PK.ToString();
                        }
                        else
                        {
                            txtUOM.Text = string.Empty;
                            hdfUOM.Value = "0";
                        }
                        break;
                    case ControlsEnum.VENDORDETAILS:
                        if (listVendor != null && listVendor.Count > 0)
                        {
                            txtCurrencyTxt.Text = listVendor[0].ADM_CURRENCY_MST.CUR_CODE;
                            hdfCurrencyTxt.Value = listVendor[0].VEN_CURRENCY.ToString();
                            ddlType.SelectedIndex = Convert.ToInt32(ddlType.Items.IndexOf(ddlType.Items.FindByValue(listVendor[0].VEN_PO_TYPE.ToString())));

                        }
                        break;
                    case ControlsEnum.POTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.VENDORITEMDETAILS:
                        if (listVendorItems != null && listVendorItems.Count > 0)
                        {
                            txtRate.Text = GetFormattedRate(listVendorItems[0].ITV_PRICE);
                        }
                        else
                        {
                            txtRate.Text = string.Empty;
                        }
                        break;

                    case ControlsEnum.ITEMDETAILS:
                        if (ObjItemDetails != null)
                        {
                            txtDtlRemark.Text = ObjItemDetails.ITM_DESC;
                        }
                        else
                        {
                            txtDtlRemark.Text = string.Empty;
                        }
                        break;

                    case ControlsEnum.REVISIONHISTORY:
                        BindGrid(controlType);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
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
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("SERVICE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                isTaxAdd = string.IsNullOrEmpty(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString()) ? 1 : Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString());
                isDiscountAdd = string.IsNullOrEmpty(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString()) ? 1 : Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString());

                imgTax.Visible = isTaxAdd == 1 ? true : false;
                imgDiscount.Visible = isDiscountAdd == 1 ? true : false;

                //Hide/Show line item wise tax & discount text boxes along with the corresponding image button
                lblItemTax.Visible = isTaxAdd == 1 ? true : false;
                txtItemTax.Visible = isTaxAdd == 1 ? true : false;
                lblItemDiscount.Visible = isDiscountAdd == 1 ? true : false;
                txtItemDiscount.Visible = isDiscountAdd == 1 ? true : false;
            }

            //Enable/Disable company dropdown list  based on Config.
            if (GetGlobalResourceObject("ConfigurationsRes", "IsCompanyDisable").ToString() == "1")
            {
                ddlCompany.Enabled = false;
            }

            hdfShowTransactionPort.Value = GetGlobalResourceObject("ConfigurationsRes", "ShowTransactionPort").ToString();  

        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int type)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            // base.WkfPageUrl = path + "?TYPE=1"; 
            path += "?TYPE=" + type.ToString();
            base.WkfPageUrl = path;

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by page and user dept,
            DataTable dtProcess;
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept]));
            }
            else
            {
                dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            }

            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // pass proc Id to wrkflw user control and fill action details 
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
            }
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }

        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        private void FillWorkFlowDetails(string refID)
        {
            int processID;
            WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
            //*************************************RefID is used to get the details of application and its state in Workflow**********************************
            if (refID != "0")
            {

                //***************************Assigning Ref ID To Hidden Field*********************************************************************************
                ReferenceID.Value = refID;
                if (ReferenceID.Value == "0")//**************Means its is a fresh application Need to do the workflow from base*******************************
                {
                    processID = Convert.ToInt32(ProcessID.Value);
                    //********************************Call Workflow TO get the Intitial Task And Action By Providing the the ProcessID************************
                    //Assign the Task And Action According to Process Intial Task Given By Work Flow
                    DataTable dtTask = obj.GetInitialTaskAction(processID); ;
                    if (dtTask.Rows.Count > 0)
                    {
                        TaskID.Value = dtTask.Rows[0]["Kmsp"].ToString();
                        TaskName.Value = dtTask.Rows[0]["KmsNme"].ToString();
                        ApplicationID.Value = Request.QueryString["POID"] != null ? Request.QueryString["POID"].ToString() : "0";
                        FillActions(dtTask);
                    }

                }
                else//*******************************************************Means its in Workflow and application once Saved*********************************
                {


                    //*********************************************************Request Workflow to Get the Application Satus By Providing the RefID************
                    DataTable dtAppStatus = obj.GetAppStatus(Convert.ToInt32(refID), ((BusinessObject.User)HttpContext.Current.User.Identity).PKUser);
                    if (dtAppStatus.Rows.Count > 0)
                    {
                        DataRow drow = dtAppStatus.Rows[0];
                        TaskID.Value = drow["KdsSkt"].ToString();
                        TaskName.Value = drow["KmsNme"].ToString();
                        FillActions(dtAppStatus);
                        ApplicationID.Value = drow["FmrPpaDi"].ToString();
                        ReferenceID.Value = Request.QueryString["RefID"].ToString();

                    }
                    //used to fill the completed task
                    else
                    {
                        DataTable dtAppID = obj.GetApplicationID(Convert.ToInt32(refID));
                        if (dtAppID.Rows.Count > 0)
                        {
                            DataRow drow = dtAppID.Rows[0];
                            ApplicationID.Value = drow["FmrPpaDi"].ToString();
                        }
                    }

                    FillPODetails(Convert.ToInt32(ApplicationID.Value));



                }
            }
            //*******************************Automatically Assigned the referenec No as 0******************************************************
            else
            {
                processID = Convert.ToInt32(ProcessID.Value);
                //********************************Call Workflow TO get the Intitial Task And Action By Providing the the ProcessID************************
                //Assign the Task And Action According to Process Intial Task Given By Work Flow
                DataTable dtTask = obj.GetInitialTaskAction(processID); ;
                if (dtTask.Rows.Count > 0)
                {
                    TaskID.Value = dtTask.Rows[0]["Kmsp"].ToString();
                    TaskName.Value = dtTask.Rows[0]["KmsNme"].ToString();
                    ApplicationID.Value = Request.QueryString["POID"] != null ? Request.QueryString["POID"].ToString() : "0";
                    FillActions(dtTask);
                }
            }
        }

        //Summary
        //CreatedBy Vineeth Babu
        //CreatedOn 01-April-2011
        //Method Used to fill the Action Drop Down Using the Datatable Provided
        private void FillActions(DataTable dtTask)
        {
            //WRKFACT_ID.DataTextField = "NdtNme";
            //WRKFACT_ID.DataValueField = "Ndtp";
            //WRKFACT_ID.DataSource = dtTask;
            //WRKFACT_ID.DataBind();
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType, ActionsEnum Acton)
        {
            Object retObject;
            retObject = null;
            int rowID;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            TextBox txtSubTotal;
            try
            {
                switch (controlType)
                {
                    #region PONONSTOCKHEADER
                    case ControlsEnum.PONONSTOCK:
                        rowID = 0;
                        hasValidRate = true;
                        purchaseOrderNonStockObj.POH_TYPE = ddlType.SelectedValue;
                        purchaseOrderNonStockObj.POH_SHIPPING = "1";
                        purchaseOrderNonStockObj.POH_BILLING = "1";
                        purchaseOrderNonStockObj.POH_EXCHG_RATE = "1";
                        purchaseOrderNonStockObj.BizUnitPk = currentUser.SBUID.ToString();
                        purchaseOrderNonStockObj.UserPk = currentUser.PKUser.ToString();
                        purchaseOrderNonStockObj.APT_CODE = ApplicationType.PO;
                        purchaseOrderNonStockObj.AST_DOC_MODE = GetDOCMODE();
                        if (Acton == ActionsEnum.SAVE)
                            purchaseOrderNonStockObj.WKF_FLAG = 0;
                        else if (Acton == ActionsEnum.WRKFSUBMIT)
                            purchaseOrderNonStockObj.WKF_FLAG = 1;
                        purchaseOrderNonStockObj.POH_VENDOR = Convert.ToInt32(hdfVendor.Value);
                        purchaseOrderNonStockObj.POH_PK = CurrPK;
                        purchaseOrderNonStockObj.POH_CRTD_BY = currentUser.SBUID;
                        purchaseOrderNonStockObj.POH_CURRENCY = Convert.ToInt32(hdfCurrencyTxt.Value);
                        purchaseOrderNonStockObj.POH_CURRENCY_BC = Convert.ToInt32(hdfCurrencyTxt.Value);
                        purchaseOrderNonStockObj.POH_TOTAL_VALUE_BC = txtHdrTotal.Text.Trim();
                        purchaseOrderNonStockObj.POH_CONTRACT_REF_NO = txtVendorRef.Text.Trim();
                        purchaseOrderNonStockObj.POH_DATE = txtRFQDate.Text.Trim();
                        purchaseOrderNonStockObj.POH_DEPT = currentUser.CurrentDeptPK;
                        txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                        purchaseOrderNonStockObj.POH_SUB_TOTAL = txtSubTotal == null ? "0" : string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? "0" : txtSubTotal.Text.Trim();
                        purchaseOrderNonStockObj.POH_DISC_AMT = txtHdrDiscount.Text.Trim();
                        purchaseOrderNonStockObj.POH_SHIP_CHARGE = txtShipping.Text.Trim();
                        purchaseOrderNonStockObj.POH_ADD_TAX_AMT = txtHdrTax.Text.Trim();
                        purchaseOrderNonStockObj.POH_PRICE_ADJUST = txtPriceAdj.Text.Trim();
                        purchaseOrderNonStockObj.POH_TOTAL_VALUE = txtHdrTotal.Text.Trim();
                        purchaseOrderNonStockObj.POH_GROUP = (byte)POGroup.Services;
                        purchaseOrderNonStockObj.POH_TERMS = txtPaymentTerms.Text;
                        purchaseOrderNonStockObj.POH_COMMENTS = txtOtherDetails.Text;
                        purchaseOrderNonStockObj.POH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                        if (!string.IsNullOrEmpty(txtFromPort.Text.Trim()) && txtFromPort.Text.Trim() != Resources.ErpRes.AutoDefaultValue.ToString())
                        {
                            purchaseOrderNonStockObj.POH_FROM_PORT_TEXT = HttpUtility.HtmlEncode(txtFromPort.Text);
                            int fromPortId = 0;
                            int.TryParse(hdfFromPortID.Value, out fromPortId);
                            purchaseOrderNonStockObj.POH_FROM_PORT = fromPortId > 0 ? fromPortId.ToString() : null;
                        }
                        else
                        {
                            purchaseOrderNonStockObj.POH_FROM_PORT_TEXT = null;
                            purchaseOrderNonStockObj.POH_FROM_PORT = null;
                        }
                        int toPortId = 0;
                        int.TryParse(hdfToPortID.Value, out toPortId);
                        purchaseOrderNonStockObj.POH_TO_PORT = toPortId > 0 ? toPortId.ToString() : null;

                        //if amendment, then save Purchase Order History
                        if (hdfIsAmend.Value == CommonConstants.SELECT_VALUE_ONE)
                        {
                            purchaseOrderNonStockObj.POH_IS_AMEND = 1;
                        }
                        else
                        {
                            purchaseOrderNonStockObj.POH_IS_AMEND = 0;
                        }


                        List<PurchaseOrderLists> PurchaseOrderList = new List<PurchaseOrderLists>();
                        PurchaseOrderLists objPurchaseOrderLists = new PurchaseOrderLists();
                        List<PurchaseOrderDetails> PODetails = new List<PurchaseOrderDetails>();
                        List<PTaxHeader> PTaxHeaderDetails = new List<PTaxHeader>();
                        PTaxHeader objPTaxHeader;
                        PurchaseOrderDetails objPurchaseOrderDetails;
                        foreach (GridViewRow grdrow in grdRFQResponse.Rows)
                        {

                            objPurchaseOrderDetails = new PurchaseOrderDetails();
                            HiddenField hdfPOItemPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfItemPK");
                            int iPK = Convert.ToInt32(hdfPOItemPK.Value);
                            Label lblPOItem = (Label)grdRFQResponse.Rows[rowID].FindControl("lblItem");
                            HiddenField hdfDtlPK = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfDtlPK");
                            objPurchaseOrderDetails.POD_PK = hdfDtlPK.Value != "" ? Convert.ToInt32(hdfDtlPK.Value) : 0;
                            objPurchaseOrderDetails.POD_PO = CurrPK;
                            objPurchaseOrderDetails.POD_SL_NO = (rowID + 1).ToString();
                            objPurchaseOrderDetails.POD_ITEM = Convert.ToInt32(hdfPOItemPK.Value);
                            objPurchaseOrderDetails.ITM_TEXT = lblPOItem.ToolTip;
                            Label lblPOQuantity = (Label)grdRFQResponse.Rows[rowID].FindControl("lblQuantity");
                            objPurchaseOrderDetails.POD_QTY_REQUESTED = lblPOQuantity.ToolTip.Replace(",", "");
                            HiddenField hdfPOUoM = (HiddenField)grdRFQResponse.Rows[rowID].FindControl("hdfUoM");
                            Label lblPOUoM = (Label)grdRFQResponse.Rows[rowID].FindControl("lblUoM");
                            objPurchaseOrderDetails.POD_UOM = hdfPOUoM.Value;
                            objPurchaseOrderDetails.UOM_CODE = lblPOUoM.ToolTip;
                            Label lblPOrate = (Label)grdRFQResponse.Rows[rowID].FindControl("lblrate");
                            objPurchaseOrderDetails.POD_RATE = lblPOrate.ToolTip;
                            objPurchaseOrderDetails.POD_TAX_PERC = "0.00";
                            Label lblTax = (Label)grdRFQResponse.Rows[rowID].FindControl("lblTax");
                            objPurchaseOrderDetails.POD_TAX = lblTax.ToolTip.Replace(",", "");
                            objPurchaseOrderDetails.POD_DISC_PERC = "0.00";
                            Label lblDiscount = (Label)grdRFQResponse.Rows[rowID].FindControl("lblDiscount");
                            objPurchaseOrderDetails.POD_DISC_AMT = lblDiscount.ToolTip.Replace(",", "");
                            Label lblPOAmount = (Label)grdRFQResponse.Rows[rowID].FindControl("lblAmount");
                            objPurchaseOrderDetails.POD_AMT_VALUE = lblPOAmount.ToolTip.Replace(",", "");
                            Label lblTotal = (Label)grdRFQResponse.Rows[rowID].FindControl("lblTotal");
                            objPurchaseOrderDetails.POD_AMOUNT = lblTotal.ToolTip.Replace(",", "");
                            objPurchaseOrderDetails.POD_CONV_FACT = "1";
                            objPurchaseOrderDetails.POD_DEPT = currentUser.CurrentDeptPK;
                            Label lblComment = (Label)grdRFQResponse.Rows[rowID].FindControl("lblComment");
                            Label lblRDate = (Label)grdRFQResponse.Rows[rowID].FindControl("lblRDate");
                            objPurchaseOrderDetails.POD_REQD_DATE = lblRDate.Text.Trim();
                            objPurchaseOrderDetails.POD_REMARKS = lblComment.ToolTip;
                            List<PTaxDetails> TaxDtlList = new List<PTaxDetails>();
                            TaxDtlList = TaxDetails.Where(c => c.ItemPK == iPK).ToList();
                            objPurchaseOrderDetails.TaxDetails = TaxDtlList;
                            PODetails.Add(objPurchaseOrderDetails);
                            rowID++;
                        }
                        objPurchaseOrderLists.PODetails = PODetails;
                        foreach (PTaxDetails item in TaxHdr)
                        {
                            objPTaxHeader = new PTaxHeader();
                            objPTaxHeader.PTH_NAME = item.POT_NAME;
                            objPTaxHeader.PTH_PK = item.POT_PK.ToString();
                            objPTaxHeader.PTH_TAX = item.POT_TAX.ToString();
                            objPTaxHeader.PTH_TAX_AMT = item.POT_TAX_AMT;
                            objPTaxHeader.PTH_TAX_CATEGORY = item.POT_TAX_CATEGORY;
                            objPTaxHeader.PTH_TYPE = item.POT_TYPE.ToString();
                            PTaxHeaderDetails.Add(objPTaxHeader);
                        }
                        objPurchaseOrderLists.TaxHdr = PTaxHeaderDetails;
                        PurchaseOrderList.Add(objPurchaseOrderLists);
                        purchaseOrderNonStockObj.PurchaseOrderList = PurchaseOrderList;
                        retObject = purchaseOrderNonStockObj;
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

        private void FillPODetails(int poID)
        {
            CurrPK = poID;
            POHeaderlist = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGenerateBL.GetPurchaseOrderNonStock(poID);
            if (POHeaderlist != null)
            {
                ClearDetails();
                lblRFQNo.Text = !string.IsNullOrEmpty(POHeaderlist.POH_NO) ? POHeaderlist.POH_NO : Resources.Messages.DocGenerationNew;
                txtRFQDate.Text = POHeaderlist.POH_DATE;
                txtVendor.Text = POHeaderlist.VEN_NAME;
                hdfVendor.Value = POHeaderlist.POH_VENDOR.ToString();
                txtCurrencyTxt.Text = POHeaderlist.POH_CURRENCY_TEXT;
                hdfCurrencyTxt.Value = POHeaderlist.POH_CURRENCY.ToString();
                txtVendorRef.Text = POHeaderlist.POH_CONTRACT_REF_NO;
                ddlType.SelectedIndex = Convert.ToInt32(ddlType.Items.IndexOf(ddlType.Items.FindByValue(POHeaderlist.POH_TYPE)));
                ddlCompany.SelectedValue = POHeaderlist.POH_COMPANY.ToString();
                hdfIsCancelled.Value = POHeaderlist.POH_DEL_STATUS;
                setItemTable();

                int count = 1;
                foreach (PurchaseOrderDetails item in POHeaderlist.PurchaseOrderList[0].PODetails)
                {
                    DataRow dr = dsPODetails.NewRow();
                    dr["SL_NO"] = count;
                    dr["POD_PK"] = item.POD_PK;
                    dr["POD_ITEM"] = item.POD_ITEM;
                    dr["ITM_CODE"] = item.ITM_CODE;
                    dr["ITM_TEXT"] = item.ITM_TEXT;
                    dr["POD_UOM"] = item.POD_UOM;
                    dr["UOM_CODE"] = item.UOM_CODE;
                    dr["POD_RATE"] = GetFormattedRate(item.POD_RATE);
                    dr["POD_QTY_REQUESTED"] = item.POD_QTY_REQUESTED;
                    dr["POD_AMT_VALUE"] = item.POD_AMT_VALUE;
                    dr["POD_DISC_AMT"] = item.POD_DISC_AMT;
                    dr["POD_TAX"] = item.POD_TAX;
                    dr["POD_AMOUNT"] = item.POD_AMOUNT;
                    dr["POD_REMARKS"] = item.POD_REMARKS;
                    dr["POD_REQD_DATE"] = item.POD_REQD_DATE;
                    dr["POD_QTY_INVOICED"] = item.POD_QTY_INVOICED;
                    dsPODetails.Rows.Add(dr);
                    count++;
                    foreach (PTaxDetails itemtax in item.TaxDetails)
                    {
                        itemtax.ItemPK = item.POD_ITEM;
                        itemtax.Applied = true;
                    }
                    TaxDetails = TaxDetails.Union(item.TaxDetails).ToList();
                }
                dsPODetails.AcceptChanges();
                BindGrid(ControlsEnum.RFQDETAIL);
                slno = count;
                //PTaxHeader PTH;
                //foreach (PTaxDetails item in POHeader.PurchaseOrderList[0].TaxDetails)
                //{
                //    PTH = new PTaxHeader();
                //    PTH.PTH_PK = item.POT_PK.ToString();
                //    PTH.PTH_TAX = item.POT_TAX.ToString();
                //    PTH.PTH_TAX_AMT = item.POT_TAX_AMT;
                //    PTH.PTH_NAME = item.POT_NAME;
                //    PTH.PTH_TYPE = item.POT_TYPE.ToString();
                //    PTH.PTH_TAX_CATEGORY = item.POT_TAX_CATEGORY;
                //    TaxHdr.Add(PTH);
                //}
                TaxHdr = POHeaderlist.PurchaseOrderList[0].TaxDetails;
                TaxHdr.ForEach(x => x.Applied = true);
                txtHdrDiscount.Text = GetFormattedCurrency(POHeaderlist.POH_DISC_AMT);
                txtShipping.Text = GetFormattedCurrency(POHeaderlist.POH_SHIP_CHARGE);
                txtHdrTax.Text = GetFormattedCurrency(POHeaderlist.POH_ADD_TAX_AMT);
                txtPriceAdj.Text = GetFormattedCurrency(POHeaderlist.POH_PRICE_ADJUST);
                txtHdrTotal.Text = GetFormattedCurrency(POHeaderlist.POH_TOTAL_VALUE);
                txtPaymentTerms.Text = POHeaderlist.POH_TERMS;
                txtOtherDetails.Text = POHeaderlist.POH_COMMENTS;
                hdfPOStatus.Value = POHeaderlist.POH_STATUS;
                if (POHeaderlist.POH_STATUS == "2")//Approve
                {
                    btnAmend.Visible = true;
                    btnCancelSubmit.Visible = true;
                }
                else if (POHeaderlist.POH_STATUS == "1")//Submit
                {
                    btnAmend.Visible = false;
                    btnCancelSubmit.Visible = true;
                }
                else
                {
                    btnAmend.Visible = false;
                    btnCancelSubmit.Visible = false;
                }

                if (Convert.ToInt32(POHeaderlist.POH_STATUS) > 100)
                {
                    FillAmendProcess();
                }

                txtFromPort.Text = HttpUtility.HtmlDecode(POHeaderlist.POH_FROM_PORT_TEXT);
                hdfFromPortID.Value = Convert.ToString(POHeaderlist.POH_FROM_PORT);
                txtToPort.Text = HttpUtility.HtmlDecode(POHeaderlist.POH_TO_PORT_TEXT);
                hdfToPortID.Value = Convert.ToString(POHeaderlist.POH_TO_PORT);
            }
        }

        private string GetDOCMODE()
        {
            commonServiceObj = new CommonService();
            AppTypeDetailsList = commonServiceObj.GetReportParameters(ApplicationType.PO, (int)AppSubTypePO.NONSTOCK, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        private void FillAmendProcess()
        {
            ucrWrkf.ViewType = 1;
            ucrWrkf.Visible = true;
            ucrWrkf.RefID = 0;
            FillProcessID((int)POWorkflowType.AMEND);
            ucrWrkf.ViewAction();
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            btnAmend.Visible = false;
            EntryStatus = EntryStatus.ENTRYMODE;
            hdfIsAmend.Value = CommonConstants.SELECT_VALUE_ONE;
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
                            //  ddlCompany.SelectedValue = rfqResponseHeaderObj.po .ICH_COMPANY.ToString();
                            //txtHdrTotal.Text = Math.Round(decimal.Parse(txtHdrTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtPriceAdj.Text = Math.Round(decimal.Parse(txtPriceAdj.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            //txtHdrTotal.Text = Math.Round(decimal.Parse(txtHdrTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            hdfExchangeRate.Value = rfqResponseHeaderObj.RRH_EXCHG_RATE.ToString();
                            //hdfCurrencyTxt.Value = rfqResponseHeaderObj.RRH_CURRENCY.ToString();
                            //txtCurrencyTxt.Text = rfqResponseHeaderObj.RRH_CURRENCY_TEXT;
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
                    //if (selectedVendor != null)
                    //{
                    //    ddlVendor.SelectedValue = selectedVendor.ToString();
                    //}
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
                case ControlsEnum.POTYPE:
                    ddlType.Items.Clear();
                    if (dtPOType != null && dtPOType.Rows.Count > 0)
                    {
                        ddlType.DataSource = dtPOType;
                        ddlType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlType.DataBind();
                    }
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
                    //To set company related to current SBU 
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    break;
                #endregion
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
                    case ControlsEnum.RFQDETAIL:
                        if (dsPODetails.Rows.Count > 0)
                        {
                            dsPODetails.DefaultView.Sort = "SL_NO ASC";
                            grdRFQResponse.DataSource = dsPODetails.DefaultView;
                            grdRFQResponse.DataBind();
                            divCalc.Visible = true;
                        }
                        else
                        {
                            grdRFQResponse.DataSource = null;
                            grdRFQResponse.DataBind();
                        }
                        CalcSubTotal();
                        break;
                    case ControlsEnum.RFQTAXPOPUPGRID:

                        if (IsHeaderTax)
                        {
                            //grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                            grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && c.Delete != true);
                            grdTaxDetails.DataBind();
                        }
                        else
                        {
                            if (hdfBrand.Value != string.Empty && hdfBrand.Value != "0")
                            {
                                //grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && c.ItemPK == Convert.ToInt32(hdfBrand.Value));
                                grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && c.ItemPK == Convert.ToInt32(hdfBrand.Value) && c.Delete != true);
                                grdTaxDetails.DataBind();
                            }
                            else
                            {
                                grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && c.ItemPK == Convert.ToInt32(hdfBrand.Value) && c.Delete != true);
                                grdTaxDetails.DataBind();
                            }
                        }
                        break;

                    case ControlsEnum.REVISIONHISTORY:
                        grdRevisionHistory.DataSource = dtPOHistory;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClearDetails()
        {
            hdfDetailPK.Value = "0";
            hdfDSlno.Value = "0";
            hdfBrand.Value = "0";
            txtBrand.Text = string.Empty;
            txtUOM.Text = string.Empty;
            hdfUOM.Value = "1";
            txtRate.Text = string.Empty;
            txtQty.Text = string.Empty;
            txtItemAmount.Text = string.Empty;
            txtItemDiscount.Text = string.Empty;
            txtItemTax.Text = string.Empty;
            txtItemAmount.Text = string.Empty;
            txtDtlRemark.Text = string.Empty;
            txtReqByDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
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

                    break;
            }



        }


        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedNumberWithSeperator(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithSeperator.Value);
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
        public string GetFormattedCurrencyWithSeperator(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
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

            //TextBox txtSubTotal;
            double amount;
            double discount;
            double tax;
            amount = 0;
            int iPK = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;


            amount = txtItemAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtItemAmount.Text);
            discount = 0;
            var discDetails = TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Discount) && rfq.ItemPK == iPK);
            foreach (PTaxDetails potTaxHdrObj in discDetails)
            {
                string taxFormula = potTaxHdrObj.POT_TAX_FORMULA;
                if (!string.IsNullOrEmpty(taxFormula))
                {
                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    potTaxHdrObj.POT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                }
            }

            discount = TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Discount) && rfq.ItemPK == iPK).Sum(rfq => rfq.POT_TAX_AMT);



            txtItemDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
            amount = amount - discount;

            var tDetails = TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Tax) && rfq.ItemPK == iPK);
            foreach (PTaxDetails potTaxHdrObj in tDetails)
            {
                string taxFormula = potTaxHdrObj.POT_TAX_FORMULA;
                if (!string.IsNullOrEmpty(taxFormula))
                {
                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    potTaxHdrObj.POT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                }
            }

            tax = TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Tax) && rfq.ItemPK == iPK).Sum(rfq => rfq.POT_TAX_AMT);
            txtItemTax.Text = tax.ToString(hdfCurrencyFormat.Value);


            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails5", "$(document).ready(function(){ShowHideItemDetails(1);});", true);


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
            //TextBox txtSubTotalFooter;
            //txtSubTotalFooter = grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
            //if (txtSubTotalFooter != null)
            //{
            //    RFQResponseHeaderSession.RRH_AMT_SUB_TOTAL = RFQResponseHeaderSession.ResponseDtl.Sum(dtl => dtl.RRD_AMT_NET_TOTAL);
            //    txtSubTotalFooter.Text = RFQResponseHeaderSession.RRH_AMT_SUB_TOTAL.ToString(hdfCurrencyFormat.Value);
            //    txtSubTotalFooter.ToolTip = RFQResponseHeaderSession.RRH_AMT_SUB_TOTAL.ToString(hdfCurrencyFormat.Value);
            //    //txtSubTotalFooter.Text = String.Format("{0:000}", decimal.Parse(txtSubTotalFooter.Text));

            //}
        }

        private void CalcSubTotal()
        {
            int rowID = 0;
            double total = 0.0;
            foreach (GridViewRow grdrow in grdRFQResponse.Rows)
            {
                Label lblTotal = (Label)grdRFQResponse.Rows[rowID].FindControl("lblTotal");
                Label lblDiscount = (Label)grdRFQResponse.Rows[rowID].FindControl("lblDiscount");
                Label lblTax = (Label)grdRFQResponse.Rows[rowID].FindControl("lblTax");
                if (lblTotal != null)
                {
                    total = total + (Convert.ToDouble(lblTotal.Text.Replace(",", "")));
                }
                rowID++;
            }

            TextBox txtSubTotalFooter;
            txtSubTotalFooter = grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter") as TextBox;
            if (txtSubTotalFooter != null)
            {
                txtSubTotalFooter.Text = GetFormattedCurrency(total.ToString());
            }

            txtHdrDiscount.Text = txtHdrDiscount.Text.Trim() != string.Empty ? GetFormattedCurrency(txtHdrDiscount.Text.Trim()) : GetFormattedCurrency("0");
            txtHdrTax.Text = txtHdrTax.Text.Trim() != string.Empty ? GetFormattedCurrency(txtHdrTax.Text.Trim()) : GetFormattedCurrency("0");
            txtShipping.Text = txtShipping.Text.Trim() != string.Empty ? GetFormattedCurrency(txtShipping.Text.Trim()) : GetFormattedCurrency("0");
            txtPriceAdj.Text = txtPriceAdj.Text.Trim() != string.Empty ? GetFormattedCurrency(txtPriceAdj.Text.Trim()) : GetFormattedCurrency("0");
            total = (total - Convert.ToDouble(txtHdrDiscount.Text.Trim())) + Convert.ToDouble(txtHdrTax.Text.Trim()) + Convert.ToDouble(txtPriceAdj.Text.Trim());
            txtHdrTotal.Text = GetFormattedCurrency(total.ToString());

        }
        private bool SetHdrTax()
        {
            //TextBox txtSubTotal;
            double amount;
            double discount;
            double tax;
            double adjust;
            double othercharge = 0;
            amount = 0;
            adjust = 0;
            //double taxwithOthercharge = 0;


            if (grdRFQResponse.FooterRow != null)
            {
                TextBox txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                if (txtSubTotal != null)
                    amount = txtSubTotal.Text == string.Empty ? 0 : Convert.ToDouble(txtSubTotal.Text);
            }


            //***********Other Charge************
            var shippingHeader = TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Shipping));
            foreach (PTaxDetails rfqShippingHdrObj in shippingHeader)
            {
                string taxFormula = rfqShippingHdrObj.POT_TAX_FORMULA;
                if (!string.IsNullOrEmpty(taxFormula))
                {
                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    rfqShippingHdrObj.POT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                }
            }
            othercharge = TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(rfq => rfq.POT_TAX_AMT);
            txtShipping.Text = othercharge.ToString(hdfCurrencyFormat.Value);


            //***********Discount************

            discount = 0;
            var discHeader = TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Discount));
            foreach (PTaxDetails potTaxHdrObj in discHeader)
            {
                string taxFormula = potTaxHdrObj.POT_TAX_FORMULA;
                if (!string.IsNullOrEmpty(taxFormula))
                {
                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    potTaxHdrObj.POT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                }
            }
            discount = TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(rfq => rfq.POT_TAX_AMT);
            txtHdrDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
            amount = amount - discount;

            //***********Tax************            

            var taxHeader = TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Tax));
            foreach (PTaxDetails rfqTaxHdrObj in taxHeader)
            {
                string taxFormula = rfqTaxHdrObj.POT_TAX_FORMULA;
                if (!string.IsNullOrEmpty(taxFormula))
                {
                    if (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase").ToString() == "1")
                    {
                        taxFormula = taxFormula.Replace("#SUBTOTAL#", (amount + othercharge).ToString());
                    }
                    else
                    {
                        taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    }
                    rfqTaxHdrObj.POT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                }
            }
            tax = TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(rfq => rfq.POT_TAX_AMT);
            txtHdrTax.Text = tax.ToString(hdfCurrencyFormat.Value);


            adjust = txtPriceAdj.Text == string.Empty ? 0 : Convert.ToDouble(txtPriceAdj.Text);
            amount = (amount + tax) + adjust + othercharge;
            txtHdrTotal.Text = (amount).ToString(hdfCurrencyFormat.Value);

            return true;

        }
        private bool IsValidDiscount(int type, int itm)
        {
            double curDisc = Convert.ToDouble(txtPopupAmount.Text.Trim());
            return false;
        }

        private void SetItemDetails(GridViewRow grdItemRow)
        {

            HiddenField hdfDtlPK = (HiddenField)grdItemRow.FindControl("hdfDtlPK");
            HiddenField hdfPOItemPK = (HiddenField)grdItemRow.FindControl("hdfItemPK");
            Label lblPOItem = (Label)grdItemRow.FindControl("lblItem");
            Label lblPOQuantity = (Label)grdItemRow.FindControl("lblQuantity");
            HiddenField hdfPOUoM = (HiddenField)grdItemRow.FindControl("hdfUoM");
            Label lblPOUoM = (Label)grdItemRow.FindControl("lblUoM");
            Label lblPOrate = (Label)grdItemRow.FindControl("lblrate");
            Label lblPOAmount = (Label)grdItemRow.FindControl("lblAmount");
            Label lblComment = (Label)grdItemRow.FindControl("lblComment");
            Label lblRDate = (Label)grdItemRow.FindControl("lblRDate");
            Label lblDiscount = (Label)grdItemRow.FindControl("lblDiscount");
            Label lblTax = (Label)grdItemRow.FindControl("lblTax");
            HiddenField hdfInvoicedQty = (HiddenField)grdItemRow.FindControl("hdfInvoicedQty");

            hdfDetailPK.Value = hdfDtlPK.Value;
            hdfBrand.Value = hdfPOItemPK.Value;
            txtBrand.Text = lblPOItem.ToolTip;
            txtUOM.Text = lblPOUoM.ToolTip;
            hdfUOM.Value = hdfPOUoM.Value;
            txtQty.Text = lblPOQuantity.ToolTip.Replace(",", "");
            txtRate.Text = lblPOrate.ToolTip;
            txtReqByDate.Text = lblRDate.ToolTip;
            txtItemAmount.Text = lblPOAmount.ToolTip.Replace(",", "");
            txtItemDiscount.Text = lblDiscount.ToolTip.Replace(",", "");
            txtItemTax.Text = lblTax.ToolTip.Replace(",", "");
            txtDtlRemark.Text = lblComment.ToolTip;
            hdfItemInvoicedQty.Value = hdfInvoicedQty.Value;
        }

        private bool checkItems(int ItemPk)
        {
            bool exists = false;
            if (dsPODetails != null)
            {
                for (int i = 0; i < dsPODetails.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dsPODetails.Rows[i]["POD_ITEM"].ToString()) == ItemPk)
                    {
                        exists = true;
                        break;
                    }
                }
            }
            return exists;
        }


        private void setItemTable()
        {
            if (dsPODetails == null)
            {
                dsPODetails = new DataTable();
                dsPODetails.Columns.Add("SL_NO", typeof(string));
                dsPODetails.Columns.Add("POD_PK", typeof(string));
                dsPODetails.Columns.Add("POD_ITEM", typeof(string));
                dsPODetails.Columns.Add("ITM_CODE", typeof(string));
                dsPODetails.Columns.Add("ITM_TEXT", typeof(string));
                dsPODetails.Columns.Add("POD_UOM", typeof(string));
                dsPODetails.Columns.Add("UOM_CODE", typeof(string));
                dsPODetails.Columns.Add("POD_RATE", typeof(string));
                dsPODetails.Columns.Add("POD_QTY_REQUESTED", typeof(string));
                dsPODetails.Columns.Add("POD_AMT_VALUE", typeof(string));
                dsPODetails.Columns.Add("POD_DISC_AMT", typeof(string));
                dsPODetails.Columns.Add("POD_TAX", typeof(string));
                dsPODetails.Columns.Add("POD_AMOUNT", typeof(string));
                dsPODetails.Columns.Add("POD_REMARKS", typeof(string));
                dsPODetails.Columns.Add("POD_REQD_DATE", typeof(string));
                dsPODetails.Columns.Add("POD_QTY_INVOICED", typeof(string));
                dsPODetails.AcceptChanges();
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
                DropDownList ddlWkfAction;
                string action;
                int? result;
                TextBox txtAmount;
                TextBox txtTax;
                TextBox txtDiscount;
                TextBox txtSubTotal;

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
                    #region save
                    case ActionsEnum.SAVE:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        if (dsExchangeRate == null || dsExchangeRate.Tables[0].Rows.Count <= 0 || Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) < 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.Messages.NoExchangeRate + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (grdRFQResponse.Rows.Count > 0)
                            {
                                hasValidRate = false;
                                purchaseOrderNonStockObj = new PONonStock();
                                purchaseOrderNonStockObj = (PONonStock)SetUIValuesToObject(ControlsEnum.PONONSTOCK, commonActions);
                                if (hasValidRate)
                                {
                                    if (purchaseOrderNonStockObj != null)
                                    {
                                        purchaseOrderNonStockObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                        string xmlDoc = CommonFunctions.XmlSerialize<PONonStock>(purchaseOrderNonStockObj);
                                        string transNumber = string.Empty;
                                        result = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOServiceDetailsWkf(xmlDoc, out transNumber);//SPPUR_ORDER_WKF_SAVE
                                        if (result > 0) // Success !  redirect to listing page
                                        {                                        
                                            #region Show Save Message and redired to listing page
                                            string crdrNo = string.Empty;
                                            if (string.IsNullOrEmpty(lblRFQNo.Text.Trim()) || lblRFQNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                crdrNo = lblRFQNo.Text.Trim();
                                                object[] args = new object[2];
                                                args[0] = Resources.PageNameRes.PurchaseOrder;
                                                args[1] = crdrNo;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                            }
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PONonStock) + "');", true);
                                            #endregion
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Response_Save").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Empty_Rate").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                hasValidRate = false;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Empty_Items").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        txtCurrencyTxt.Text = string.Empty;
                        hdfCurrencyTxt.Value = string.Empty;
                        venPk = hdfVendor.Value != string.Empty ? Convert.ToInt32(hdfVendor.Value) : 0;
                        servicePk = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;
                        GetFieldValues(ControlsEnum.VENDORDETAILS);
                        SetFieldValues(ControlsEnum.VENDORDETAILS);
                        break;
                    #endregion

                    #region POTAXDETAILS
                    case ActionsEnum.RFQTAXDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (hdfVendor.Value != string.Empty && hdfVendor.Value != "0")
                        {

                            txtAmount = txtItemAmount;
                            txtDiscount = txtItemDiscount;
                            if (txtAmount != null && txtDiscount != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text.Trim()).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                                IsHeaderTax = false;
                                //int taxCategory1 = 1;
                                //int.TryParse(hdfTaxCategory.Value, out taxCategory1);
                                //if (TaxHdr!=null)
                                //{
                                //    foreach (var item in TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory1).ToList())
                                //    {
                                //        item.Applied = true;
                                //        item.Delete = false;
                                //    } 
                                //}
                                //if (TaxDetails!=null)
                                //{
                                //    foreach (var item in TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory1).ToList())
                                //    {
                                //        item.Applied = true;
                                //        item.Delete = false;
                                //    } 
                                //}
                                ////TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == category)
                                ////.ToList().ForEach(x => x.Delete = false);
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

                                int taxCategory = 1;
                                int.TryParse(hdfTaxCategory.Value, out taxCategory);
                                //TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory)
                                //.ToList().RemoveAll(x => x.Applied == false);
                                foreach (PTaxDetails item in TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory).ToList())
                                {
                                    if (item.Applied != true)
                                    {
                                        TaxDetails.Remove(item);
                                    }
                                }
                                if (IsHeaderTax)
                                {
                                    grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                else
                                {
                                    grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && c.ItemPK == Convert.ToInt32(hdfBrand.Value));
                                    grdTaxDetails.DataBind();
                                }

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                                //}
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails3", "$(document).ready(function(){ShowHideItemDetails(1);});", true);

                        break;
                    #endregion

                    #region PODISCDETAILS
                    case ActionsEnum.RFQDISCDETAILS:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (hdfVendor.Value != string.Empty && hdfVendor.Value != "0")
                        {

                            txtAmount = txtItemAmount;
                            txtDiscount = txtItemDiscount;
                            if (txtAmount != null && txtDiscount != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text.Trim()).ToString(hdfCurrencyFormat.Value);
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
                                int taxCategory = 1;
                                int.TryParse(hdfTaxCategory.Value, out taxCategory);
                                //TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory)
                                //.ToList().RemoveAll(x => x.Applied == false);
                                foreach (PTaxDetails item in TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory).ToList())
                                {
                                    if (item.Applied != true)
                                    {
                                        TaxDetails.Remove(item);
                                    }
                                }
                                if (IsHeaderTax)
                                {
                                    grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                else
                                {
                                    grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && c.ItemPK == Convert.ToInt32(hdfBrand.Value));
                                    grdTaxDetails.DataBind();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                                //}
                            }


                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails4", "$(document).ready(function(){ShowHideItemDetails(1);});", true);

                        break;
                    #endregion

                    #region POTAXHEADER
                    case ActionsEnum.RFQTAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (hdfVendor.Value != string.Empty && hdfVendor.Value != "0")
                        {

                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.RFQTAXTYPES);
                            SetFieldValues(ControlsEnum.RFQTAXTYPES);
                            txtSubTotal = (TextBox)grdRFQResponse.FooterRow.FindControl("txtSubTotalFooter");
                            if (txtSubTotal != null)
                            {
                                string totalAmount = ((double)0).ToString(hdfCurrencyFormat.Value);
                                totalAmount = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? Convert.ToDouble(txtSubTotal.Text.Trim()).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtSubTotal.Text.Trim()) - Convert.ToDouble(txtHdrDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                                if (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase").ToString() == "1")
                                {
                                    totalAmount = Convert.ToDouble(Convert.ToDouble(totalAmount) + Convert.ToDouble((string.IsNullOrEmpty(txtShipping.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtShipping.Text.Trim()).ToString(hdfCurrencyFormat.Value)))).ToString(hdfCurrencyFormat.Value);
                                }

                                // txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? Convert.ToDouble(txtSubTotal.Text.Trim()).ToString(hdfCurrencyFormat.Value) : (Convert.ToDouble(txtSubTotal.Text.Trim()) - Convert.ToDouble(txtHdrDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);
                                txtPopupItemAmount.Text = totalAmount;
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
                                ////////////////////////////////////////////////////
                                int taxCategory = 1;
                                int.TryParse(hdfTaxCategory.Value, out taxCategory);
                                //TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory)
                                //.ToList().RemoveAll(x => x.Applied == false);
                                foreach (PTaxDetails item in TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory).ToList())
                                {
                                    if (item.Applied != true)
                                    {
                                        TaxHdr.Remove(item);
                                    }
                                }
                                if (IsHeaderTax)
                                {
                                    grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                else
                                {
                                    grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                ////////////////////////////////////////////////////
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            }
                            //}
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region PODISCHEADER
                    case ActionsEnum.RFQDISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (hdfVendor.Value != string.Empty && hdfVendor.Value != "0")
                        {

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
                                ////////////////////////////////////////////////////
                                int taxCategory = 1;
                                int.TryParse(hdfTaxCategory.Value, out taxCategory);
                                //TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory)
                                //.ToList().RemoveAll(x => x.Applied == false);
                                foreach (PTaxDetails item in TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory).ToList())
                                {
                                    if (item.Applied != true)
                                    {
                                        TaxHdr.Remove(item);
                                    }
                                }
                                if (IsHeaderTax)
                                {
                                    grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                else
                                {
                                    grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                ////////////////////////////////////////////////////

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                            //}
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region POOTHERCHARGEHEADER
                    case ActionsEnum.OTHERCHARGEHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (hdfVendor.Value != string.Empty && hdfVendor.Value != "0")
                        {

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
                                ////////////////////////////////////////////////////
                                int taxCategory = 1;
                                int.TryParse(hdfTaxCategory.Value, out taxCategory);
                                //TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory)
                                //.ToList().RemoveAll(x => x.Applied == false);
                                foreach (PTaxDetails item in TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == taxCategory).ToList())
                                {
                                    if (item.Applied != true)
                                    {
                                        TaxHdr.Remove(item);
                                    }
                                }
                                if (IsHeaderTax)
                                {
                                    grdTaxDetails.DataSource = TaxHdr.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                else
                                {
                                    grdTaxDetails.DataSource = TaxDetails.Where(c => c.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    grdTaxDetails.DataBind();
                                }
                                ////////////////////////////////////////////////////

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("OtherChargeDetails").ToString() + "','600','300');", true);
                            }
                            //}
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Vendor").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);

                        double taxAmount;
                        if (IsHeaderTax)
                        {
                            TaxHdr.RemoveAll(x => x.Delete == true);
                            //foreach (var item in TaxHdr)
                            //    if (item.Delete==true) TaxHdr.Remove(item);

                            TaxHdr.Where(rfq => rfq.POT_TAX_CATEGORY == category)
                                .ToList().ForEach(x => x.Applied = true);

                            taxAmount = TaxHdr.ToList().Where(rfq => rfq.POT_TAX_CATEGORY == category).Sum(rfq => rfq.POT_TAX_AMT);
                            if (category == ((int)TaxType.Discount))
                                txtHdrDiscount.Text = taxAmount.ToString(hdfCurrencyFormat.Value);
                            else
                                txtHdrTax.Text = taxAmount.ToString(hdfCurrencyFormat.Value);
                        }
                        else
                        {
                            int iPK = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;

                            TaxDetails.RemoveAll(x => x.Delete == true);
                            //foreach (var item in TaxDetails)
                            //    if (item.Delete == true) TaxDetails.Remove(item);
                            TaxDetails.Where(rfq => rfq.POT_TAX_CATEGORY == category && rfq.ItemPK == iPK)
                               .ToList().ForEach(x => x.Applied = true);

                            taxAmount = TaxDetails.ToList().Where(rfq => rfq.POT_TAX_CATEGORY == category && rfq.ItemPK == iPK).Sum(rfq => rfq.POT_TAX_AMT);

                            if (category == ((int)TaxType.Discount))
                                txtTax = txtItemDiscount;
                            else
                                txtTax = txtItemTax;
                            if (txtTax != null)
                            {
                                txtTax.Text = taxAmount.ToString(hdfCurrencyFormat.Value);
                            }

                        }

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
                        int tcategory = 1;
                        int.TryParse(hdfTaxCategory.Value, out tcategory);

                        if (IsHeaderTax)
                        {
                            PTaxDetails TDetailsItems = new PTaxDetails();
                            TDetailsItems.POT_NAME = txtPopupOther.Text.Trim();
                            TDetailsItems.POT_PK = 0;
                            TDetailsItems.POT_SL_NO = slno;
                            TDetailsItems.POT_PO_DTL = "0";
                            TDetailsItems.POT_PO = CurrPK;
                            TDetailsItems.POT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            TDetailsItems.POT_TAX_CATEGORY = tcategory;
                            TDetailsItems.POT_TAX_CATEGORY_TEXT = ddlPopupTaxType.SelectedItem.Text;
                            TDetailsItems.POT_TYPE = ddlPopupTaxType.SelectedValue == "-1" ? 2 : 1;
                            TDetailsItems.POT_TAX_FORMULA = hdfTaxFormula.Value;
                            TDetailsItems.POT_TAX_TEXT = ddlPopupTaxType.SelectedItem.Text;
                            TDetailsItems.POT_TAX_AMT = Convert.ToDouble(txtPopupAmount.Text);
                            TDetailsItems.IsHeader = 1;
                            TDetailsItems.ItemPK = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;
                            TaxHdr.Add(TDetailsItems);
                        }
                        else
                        {
                            int itPk = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;
                            //delete old item pk 
                            List<PTaxDetails> oldDetails = TaxDetails.Where(c => c.ItemPK == itPk && c.POT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue)).ToList();
                            foreach (PTaxDetails item in oldDetails)
                            {
                                TaxDetails.Remove(item);
                            }

                            PTaxDetails TDetailsItems = new PTaxDetails();
                            TDetailsItems.POT_NAME = txtPopupOther.Text.Trim();
                            TDetailsItems.POT_PK = 0;
                            TDetailsItems.POT_SL_NO = hdfDSlno.Value != "0" && hdfDSlno.Value != "" ? Convert.ToInt32(hdfDSlno.Value) : slno;
                            TDetailsItems.POT_PO_DTL = hdfDetailPK.Value;
                            TDetailsItems.POT_PO = CurrPK;
                            TDetailsItems.POT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            TDetailsItems.POT_TAX_CATEGORY = tcategory;
                            TDetailsItems.POT_TAX_CATEGORY_TEXT = ddlPopupTaxType.SelectedItem.Text;
                            TDetailsItems.POT_TYPE = ddlPopupTaxType.SelectedValue == "-1" ? 2 : 1;
                            TDetailsItems.POT_TAX_FORMULA = hdfTaxFormula.Value;
                            TDetailsItems.POT_TAX_TEXT = ddlPopupTaxType.SelectedItem.Text;
                            TDetailsItems.POT_TAX_AMT = Convert.ToDouble(txtPopupAmount.Text);
                            TDetailsItems.IsHeader = 0;
                            TDetailsItems.ItemPK = itPk;
                            TaxDetails.Add(TDetailsItems);
                        }
                        if (TaxHdr.Count > 0 || TaxDetails.Count > 0)
                        {
                            //TempRFQResponseHeaderSession = rfqResponseHeaderObj;
                            SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);

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
                        //}
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        if (errorTaxAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Tax_Add").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Invalid_Tax_Amount").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:

                        HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                        HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                        if (hdfTaxPK != null)
                        {
                            int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                            rfqTaxHdrList = new List<RFQTaxHdr>();
                            PTaxDetails objPTaxDetails = null;
                            if (IsHeaderTax)
                            {
                                if (taxPK > 0)
                                {
                                    objPTaxDetails = TaxHdr.SingleOrDefault(rfq => rfq.POT_PK == taxPK && rfq.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    if (hdfTaxName != null)
                                    {
                                        objPTaxDetails = TaxHdr.SingleOrDefault(rfq => rfq.POT_NAME == hdfTaxName.Value && rfq.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                                if (objPTaxDetails != null)
                                {
                                    // TaxHdr.Remove(objPTaxDetails);
                                    foreach (var item in TaxHdr)
                                    {
                                        if (item == objPTaxDetails)
                                        {
                                            item.Delete = true;
                                        }
                                    }
                                }
                            }
                            else
                            {

                                if (taxPK > 0)
                                {
                                    objPTaxDetails = TaxDetails.SingleOrDefault(rfq => rfq.POT_PK == taxPK && rfq.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && rfq.ItemPK == Convert.ToInt32(hdfBrand.Value));
                                }
                                else
                                {
                                    if (hdfTaxName != null)
                                    {
                                        objPTaxDetails = TaxDetails.SingleOrDefault(rfq => rfq.POT_NAME == hdfTaxName.Value && rfq.POT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value) && rfq.ItemPK == Convert.ToInt32(hdfBrand.Value));
                                    }
                                }
                                if (objPTaxDetails != null)
                                {
                                    // TaxDetails.Remove(objPTaxDetails);
                                    foreach (var item in TaxDetails)
                                    {
                                        if (item == objPTaxDetails)
                                        {
                                            item.Delete = true;
                                        }
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails10", "$(document).ready(function(){ShowHideItemDetails(1);});", true);


                            }

                            //TempRFQResponseHeaderSession = rfqResponseHeaderObj;
                            SetFieldValues(ControlsEnum.RFQTAXPOPUPGRID);

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
                        //}
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
                        Response.Redirect(Resources.PageURL.PONonStock);
                        break;
                    #endregion

                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        SetDetailTax(sender);
                        SetHdrTax();
                        ResetForm(ControlsEnum.RFQTAXPOPUPGRID);
                        break;
                    #endregion

                    #region CALCULATEHDRTAX
                    case ActionsEnum.CALCULATEHDRTAX:
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

                    #region ITEMSELECTED
                    case ActionsEnum.PRODUCTSELECTED:
                        GetFieldValues(ControlsEnum.UOM);
                        SetFieldValues(ControlsEnum.UOM);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        if (checkItems(Convert.ToInt32(hdfBrand.Value)))
                        {
                            txtBrand.Text = Resources.Messages.AutoDefaultValue;
                            hdfBrand.Value = "0";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_ITEM").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        venPk = hdfVendor.Value != string.Empty ? Convert.ToInt32(hdfVendor.Value) : 0;
                        servicePk = hdfBrand.Value != string.Empty ? Convert.ToInt32(hdfBrand.Value) : 0;
                        GetFieldValues(ControlsEnum.VENDORITEMDETAILS);
                        SetFieldValues(ControlsEnum.VENDORITEMDETAILS);
                        //Description in service master should come as comment (default)
                        GetFieldValues(ControlsEnum.ITEMDETAILS);
                        SetFieldValues(ControlsEnum.ITEMDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails1", "$(document).ready(function(){ShowHideItemDetails(1);});", true);
                        break;
                    #endregion

                    #region ADDITEMS
                    case ActionsEnum.ADDITEM:
                        if (!string.IsNullOrEmpty(hdfItemInvoicedQty.Value))
                        {
                            if (Convert.ToDouble(hdfItemInvoicedQty.Value) > 0)
                            {
                                if (Convert.ToDouble(hdfItemInvoicedQty.Value) > Convert.ToDouble(txtQty.Text))
                                {
                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_Qty_Amend").ToString(), (Convert.ToDouble(hdfItemInvoicedQty.Value)).ToString(hdfDecimalFormat.Value));
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails7", "$(document).ready(function(){ShowHideItemDetails(1);});", true);
                                    return;
                                }
                            }
                        }

                        int isEdit = hdfEdit.Value == string.Empty ? -1 : Convert.ToInt32(hdfEdit.Value);
                        if (isEdit > -1)
                        {
                            if (dsPODetails.Rows.Count > 0)
                            {
                                dsPODetails.Rows[isEdit].BeginEdit();
                                dsPODetails.Rows[isEdit].Delete();
                                dsPODetails.AcceptChanges();
                            }
                        }

                        if (!checkItems(Convert.ToInt32(hdfBrand.Value)))
                        {

                            hdfEdit.Value = string.Empty;
                            DataRow dr;
                            setItemTable();

                            SetDetailTax(null);


                            dr = dsPODetails.NewRow();
                            dr["SL_NO"] = hdfDSlno.Value != "0" ? hdfDSlno.Value : slno.ToString();
                            dr["POD_PK"] = hdfDetailPK.Value;
                            dr["POD_ITEM"] = hdfBrand.Value;
                            dr["ITM_CODE"] = txtBrand.Text;
                            dr["ITM_TEXT"] = txtBrand.Text;
                            dr["POD_UOM"] = hdfUOM.Value;
                            dr["UOM_CODE"] = txtUOM.Text;
                            dr["POD_RATE"] = txtRate.Text;
                            dr["POD_QTY_REQUESTED"] = txtQty.Text;
                            dr["POD_AMT_VALUE"] = txtItemAmount.Text != string.Empty ? txtItemAmount.Text : "0";
                            dr["POD_DISC_AMT"] = txtItemDiscount.Text != string.Empty ? txtItemDiscount.Text : "0";
                            dr["POD_TAX"] = txtItemTax.Text != string.Empty ? txtItemTax.Text : "0";
                            dr["POD_AMOUNT"] = ((Convert.ToDouble(dr["POD_AMT_VALUE"]) - Convert.ToDouble(dr["POD_DISC_AMT"])) + Convert.ToDouble(dr["POD_TAX"])).ToString(hdfCurrencyFormat.Value);
                            dr["POD_REMARKS"] = txtDtlRemark.Text.Trim();
                            dr["POD_REQD_DATE"] = txtReqByDate.Text.Trim();
                            dr["POD_QTY_INVOICED"] = string.IsNullOrEmpty(hdfItemInvoicedQty.Value) ? "0" : hdfItemInvoicedQty.Value;
                            dsPODetails.Rows.Add(dr);
                            dsPODetails.AcceptChanges();
                            BindGrid(ControlsEnum.RFQDETAIL);
                            SetHdrTax();
                            ClearDetails();
                            slno++;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_ITEM").ToString() + "','" + Resources.ErpRes.Information + "');", true);

                        }
                        break;
                    #endregion

                    #region CLEARDEATILS
                    case ActionsEnum.CLEARITEM:
                        ClearDetails();
                        break;
                    #endregion

                    #region DELETEGRID
                    case ActionsEnum.DELETEGRID:
                        GridViewRow grdrow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        if (dsPODetails.Rows.Count > 0)
                        {
                            dsPODetails.Rows[grdrow.RowIndex].BeginEdit();
                            dsPODetails.Rows[grdrow.RowIndex].Delete();
                            dsPODetails.AcceptChanges();
                        }
                        dsPODetails.AcceptChanges();
                        BindGrid(ControlsEnum.RFQDETAIL);
                        SetHdrTax();
                        break;
                    #endregion

                    #region EDITGRID
                    case ActionsEnum.EDITGRID:
                        ClearDetails();
                        GridViewRow grdeditrow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        hdfEdit.Value = grdeditrow.RowIndex.ToString();
                        SetItemDetails(grdeditrow);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails2", "$(document).ready(function(){ShowHideItemDetails(1);});", true);

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
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        //Show WorkFlow Popup
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WRKSUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        if (dsExchangeRate == null || dsExchangeRate.Tables[0].Rows.Count <= 0 || Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) < 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + Resources.Messages.NoExchangeRate + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (grdRFQResponse.Rows.Count > 0)
                                {
                                    hasValidRate = false;
                                    purchaseOrderNonStockObj = new PONonStock();
                                    purchaseOrderNonStockObj = (PONonStock)SetUIValuesToObject(ControlsEnum.PONONSTOCK, commonActions);
                                    purchaseOrderNonStockObj.WKF_FLAG = 1;
                                    if (hasValidRate)
                                    {
                                        if (purchaseOrderNonStockObj != null)
                                        {                                           
                                            SaveTransaction(purchaseOrderNonStockObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Err_Empty_Rate").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    hasValidRate = false;
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Err_Empty_Items").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.CheckforPOCancellation((int)CurrPK))
                                {
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_PO_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            
                        }

                        break;
                    #endregion

                    #region SHOW
                    case ActionsEnum.SHOW:
                        SetDetailTax(null);
                        SetHdrTax();
                        break;
                    #endregion

                    #region AMEND
                    case ActionsEnum.AMEND:
                        FillAmendProcess();
                        break;
                    #endregion

                    #region CANCEL SUBMIT
                    case ActionsEnum.DELETESUBMIT:
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.Visible = true;
                        ucrWrkf.RefID = 0;
                        FillProcessID((int)POWorkflowType.CANCEL);
                        ucrWrkf.ViewAction();
                        WorkflowCore.CoreService wrkflwCore = new WorkflowCore.CoreService();
                        ucrWrkf.RefID = wrkflwCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);

                        break;
                    #endregion

                    #region REVISIONHISTORY
                    case ActionsEnum.REVISIONHISTORY:
                        GetFieldValues(ControlsEnum.REVISIONHISTORY);
                        SetFieldValues(ControlsEnum.REVISIONHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowRevisionHistory", "ShowContainerDiv('[id$=divRevisionHistory]','" + GetLocalResourceObject("RevisionHistory").ToString() + "','500','300');", true);
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

                        Label lblAmount = e.Row.FindControl("lblAmount") as Label;
                        Label lblDiscount = e.Row.FindControl("lblDiscount") as Label;
                        Label lblTax = e.Row.FindControl("lblTax") as Label;
                        Label lblTotal = e.Row.FindControl("lblTotal") as Label;


                        decimal rowDis = Convert.ToDecimal(lblDiscount.Text.Replace(",", ""));
                        decimal rowtax = Convert.ToDecimal(lblTax.Text.Replace(",", ""));
                        decimal rowamt = Convert.ToDecimal(lblAmount.Text.Replace(",", ""));
                        decimal rowtot = (rowamt - rowDis) + rowtax;

                        //lblTotal.Text = GetFormattedCurrency(rowtot);
                        //lblTotal.ToolTip = GetFormattedCurrency(rowtot);
                        lblTotal.Text = GetFormattedCurrencyWithSeperator(rowtot);
                        lblTotal.ToolTip = GetFormattedCurrencyWithSeperator(rowtot);


                        //if (rfqResponseDetailsList != null && rfqResponseDetailsList.Count > 0)
                        //{

                        //    //lblQuantity.Text = String.Format("{0:c}", decimal.Parse(lblQuantity.Text));
                        //    //lblQuantity.ToolTip = lblQuantity.Text;
                        //    //txtAmount.Text = Math.Round(decimal.Parse(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //    //txtDiscount.Text = Math.Round(decimal.Parse(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //    //txtTax.Text = Math.Round(decimal.Parse(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //    //txtTotal.Text = Math.Round(decimal.Parse(txtTotal.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //}

                    }
                }
                else if ((sender as GridView).ID == "grdRevisionHistory")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("OnClick", "javascript:return OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + DataBinder.Eval(e.Row.DataItem, "POH_PK").ToString() + "&RevID=" + DataBinder.Eval(e.Row.DataItem, "POH_VERSION").ToString() + "&APPTYPE=" + BusinessObject.CommonManagement.ApplicationType.PO + "&APPSUBTYPE=" + ((int)AppSubTypePO.NONSTOCK).ToString() + "');");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Attributes.Add("href", "javascript:void(0);");
                        (e.Row.FindControl("lnkRevisionPrint") as LinkButton).Text = DataBinder.Eval(e.Row.DataItem, "POH_NO").ToString();
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
            // uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAmend.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.btnAmend.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
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

                if (grdRFQResponse.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideItemDetails6", "$(document).ready(function(){ShowHideItemDetails(1);});", true);
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
            UOM,
            PONONSTOCK,
            VENDORDETAILS,
            POTYPE,
            VENDORITEMDETAILS,
            COMPANY,
            ITEMDETAILS,
            REVISIONHISTORY
        }

        #endregion

        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(PONonStock purchaseOrderNonStockObj, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (purchaseOrderNonStockObj == null)
                purchaseOrderNonStockObj = new PONonStock();
            #region Application Code      
            purchaseOrderNonStockObj.APT_CODE = ApplicationType.PO;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            purchaseOrderNonStockObj.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            purchaseOrderNonStockObj.WKF_APPLICATION = CurrPK;
            purchaseOrderNonStockObj.WKF_COMMENTS = wkfDetails.Comments;
            purchaseOrderNonStockObj.WKF_TRX_FLAG = workflowFlag;
            purchaseOrderNonStockObj.WKF_PROCESS = wkfDetails.ProcessID;
            purchaseOrderNonStockObj.WKF_REFERENCE = wkfDetails.ReferenceID;
            purchaseOrderNonStockObj.WKF_TASK = wkfDetails.TaskID;
            purchaseOrderNonStockObj.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
          
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<PONonStock>(purchaseOrderNonStockObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
            string transNumber = string.Empty;
          
            result = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOServiceDetailsWkf(xmlDoc, out transNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                if (result.HasValue && result.Value > 0)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                    if (isCancelled)
                    {
                        FillProcessID(1);
                        litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                    }
                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                    TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                    WrkfComments.Text = "";
                    if (string.IsNullOrEmpty(transNumber))
                        transNumber = lblRFQNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.PurchaseOrder;
                    args[1] = transNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    // Show Save Message and redired to listing page   
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PurchaseOrder);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {                        
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.PONonStock) + "');", true);
                    }
                    #endregion
                }
                ucrWrkf.ApplicationID = result.Value;
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ClosePopup();ShowDuplicateVendorInvNoContinue(2);});", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.PurchaseOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.PurchaseOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PurchaseOrder);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }

        }
        #endregion

    }
}

