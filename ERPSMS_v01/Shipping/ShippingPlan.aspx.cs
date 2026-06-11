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
//using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using BusinessObject.Shipping;
using CustomControls;
using System.IO;
using System.Globalization;


namespace ERPSMS_v01.Shipping
{
    public partial class ShippingPlan : ERP.Store.UI.WorkFlowBasePage
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

        private string AppliedSCBrandPKs
        {
            get
            {
                return this.ViewState[ViewstateStrings.SCBrandPK] != null ? this.ViewState[ViewstateStrings.SCBrandPK].ToString() : string.Empty;
            }
            set
            {
                this.ViewState[ViewstateStrings.SCBrandPK] = value;
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

        private int BizUnitConfigValue
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.BizUnitConfigValue]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BizUnitConfigValue] = value;
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
        /// To maintain keep selected SC's
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
        /// Approved Status
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
        /// Posted Status
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

        private List<BusinessObject.Shipping.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileBLDetailsList] == null ? null : (List<BusinessObject.Shipping.FileDetails>)Session[ERP.Utilities.SessionStrings.FileBLDetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileBLDetailsList] = value;
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


        private int ShippingUploadType
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.ShippingUploadType] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.ShippingUploadType];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.ShippingUploadType] = value;
            }
        }

        private List<BusinessObject.Shipping.ShippingPlanUploadDetails> ShippingUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.ShippingUploadList] == null ? null : (List<BusinessObject.Shipping.ShippingPlanUploadDetails>)ViewState[ViewstateStrings.ShippingUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.ShippingUploadList] = value;
            }
        }

        private int ItemStatus
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ItemStatus]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemStatus] = value;
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
        public List<CustomerBrands> CustomerBrandsList
        {
            get
            {
                return this.ViewState["CustBrandsList"] == null ? null : (List<CustomerBrands>)this.ViewState["CustBrandsList"];
            }
            set
            {
                this.ViewState["CustBrandsList"] = value;
            }
        }

        public DataTable dtShippingBrands
        {
            get
            {
                return this.ViewState["dtShippingBrands"] == null ? null : (DataTable)this.ViewState["dtShippingBrands"];
            }
            set
            {
                this.ViewState["dtShippingBrands"] = value;
            }
        }

        #endregion

        #region Variables

        private List<ADM_COMPANY_MST> admCompanyMstList;
        private ADM_COMPANY_MST admCompanyMstObj;

        BusinessObject.User currentUser;

        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //page related Entity Object
        //Object for service utility
        private ServiceUtility serviceUtilityObj;
        //Object for GON Header
        private SAL_DESPATCH_HDR salDespatchHdrObj;

        //List for binding details to controls  
        //List For Shipping Plan Container types
        private List<ADM_CONST_MST> admConstMstList;
        //List for Selected SC's for Shippinf
        private List<long> SelectedSOListForSP;
        //List for workflow status
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;
        //List for Shipping Plan Number
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;

        List<CustomerBrands> lstCustBrands;

        private List<long> ShippingSCPKs
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.ShippingSCPKs];
            }
            set
            {
                this.ViewState[ViewstateStrings.ShippingSCPKs] = value;
            }

        }
        private string InType
        {
            get
            {
                return this.ViewState["InType"] == null ? null : Convert.ToString(this.ViewState["InType"]);
            }
            set
            {
                this.ViewState["InType"] = value;
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
        private bool IsInvoiceTerms
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsInvoiceTerms] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsInvoiceTerms].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsInvoiceTerms] = value;
            }
        }
        private int InPk
        {
            get
            {
                return this.ViewState["InPk"] == null ? 0 : Convert.ToInt32(this.ViewState["InPk"]);
            }
            set
            {
                this.ViewState["InPk"] = value;
            }
        }
        //Service objects
        private CommonService cm;

        //workflow 
        private string refID;
        private string inboxFlag;

        //dataset for binding details to controls  
        private DataSet dsPageData;
        private DataSet dsShippingDetails;
        private DataSet dsSPDetails;
        private DataSet dsShippingList;
        private DataSet dsShippingPlanHDR;
        private DataTable dtSPWeightDetails;
        DataTable dtPageData;
        private DataTable dtDoList;
        private DataTable dtCustomerSC;
        //private DataTable dtShippingBrands;


        //Business Objects
        ShippingPlanOrder ShippingPlanOrderObj;
        ShippingPlanBO ShippingPlanObj;

        //For Shipping Plan number
        private string ShippingPlanNo;

        private int SPID;

        //For calculating Total Plan Qty
        double TotalPlandQty = 0.0;

        private bool updateDespatch;
        //For Shipping Plan Saving or not
        private bool isSave;

        BusinessObject.Shipping.ShippingPlanUploadDetails shippingUploadObj;

        DataSet dsShippingUploads;
        RadioButton rbtn;
        DataTable dtCompany = new DataTable();
        private DeliveryOrderService deliveryOrderServiceClient;

        private int SOPK = 0;
        private int SODPK = 0;
        private string ItemCode = "";
        private double SODQty = 0;

        private DataTable dtBOIStatus;
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
            try
            {
                //for workflow integration
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    lblCompanyFilter.Visible = GetConfigData().IsMultiplePlant;
                    ddlCompanyFilter.Visible = GetConfigData().IsMultiplePlant;
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    //set the currency and number format
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                    hdfQtyDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithComma.Value += "0";
                    }

                    //for shipping plan no
                    lblSPStatus.Text = Resources.Messages.DocGenerationNew;
                    AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();

                    //get and set the workflow Status
                    GetFieldValues(ControlsEnum.STATUS);
                    SetFieldValues(ControlsEnum.STATUS);

                    //get and set the Container type 
                    GetFieldValues(ControlsEnum.SPCONTAINERTYPE);
                    SetFieldValues(ControlsEnum.SPCONTAINERTYPE);

                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    SelectedInvoicesCrDr = null;


                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "SCD_SEQUENCE";
                    grdUploads.DataKeyNames = itemkeyarray;

                    AppliedSCBrandPKs = null;
                    CustomerBrandsList = null;
                    FileDetailsList = null;
                    ShippingSCPKs = null;
                    ShippingUploadType = Convert.ToInt32(Resources.Constants.ShippingPlanUploadDocType);

                    DateTime PrevMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();

                    txtGeneratedOn.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtETD.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtLoadingDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);

                    //Used for Integration purpose
                    FillProcessID(0);
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
                            commonActions = ActionsEnum.VIEW;
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
                    }

                    //if ShippingPlan Pk >0 then set the the Details otherwise listing 
                    if (CurrPK > 0)
                    {
                        ShippingPlanID = CurrPK;
                        AST_DOC_MODE.Value = ((int)DOCMODE.Submit).ToString();
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                        GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                        GetFieldValues(ControlsEnum.UPLOADEDFILES);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);

                    }
                    else
                    {
                        string[] datakeyarray;
                        datakeyarray = new string[1];
                        datakeyarray[0] = Resources.DataFieldRes.SPPk;
                        grdShippingPlanList.DataKeyNames = datakeyarray;

                        //if Selected SC's then create for shipping otherwise listing 
                        if (SelectedSosForSP != null)
                        {
                            AST_DOC_MODE.Value = GetDOCMODE();
                            lblShippingPlanNo.Text = Resources.Messages.DocGenerationNew;

                            SelectedSOListForSP = SelectedSosForSP;
                            SelectedSosForSP = null;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateDespatch = false;
                            // for cancellation
                            SetCancelRef(CurrPK);
                            //--- new ---
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false)
                            {
                                EntryStatus = EntryStatus.NEWMODE;
                                ucrWrkf.ViewType = 1;
                                //btnSave.Visible = true;
                            }
                            else
                            {
                                SelectedSOListForSP = new List<long>();
                                GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                EntryStatus = EntryStatus.LISTMODE;
                                ucrWrkf.ViewType = 0;
                                //btnSave.Visible = false;
                                //btnEdit.Visible = false;
                                // btnPrint.Visible = false;
                            }
                            //---- end new ----
                        }
                        else
                        {
                            SelectedSOListForSP = new List<long>();
                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            EntryStatus = EntryStatus.LISTMODE;
                            SetCancelRef(CurrPK);
                            //--- new ---
                            ucrWrkf.FillWorkFlowDetails();
                            if (!ucrWrkf.HasActions || ucrWrkf.IsWkfCompleted)
                            {
                                ucrWrkf.ViewType = 0;
                                //btnSave.Visible = false;
                                //btnEdit.Visible = false;
                                //btnPrint.Visible = false;
                            }
                            //---- end new ----
                        }
                    }

                    hdfType.Value = ApplicationType.SPLN;
                    hdfDOType.Value = ApplicationType.DO;
                    ddlCompany.Enabled = GetConfigData().IsMultiplePlant ? false : true;
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
            AdmCompanyMstService admCompanyMstServiceClient;
            //For Common Service
            CommonService CommonServiceClient;
            CommonServiceClient = null;

            //get the userdeatils

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            string getxml;
            //string result;

            try
            {
                //initialize the common service
                CommonServiceClient = new CommonService();
                switch (type)
                {
                    case ControlsEnum.SHIPPINGPLANHDR:
                        //for Lising
                        //
                        int cusID = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        string PlanNo = string.IsNullOrEmpty(txtPlanNo.Text.Trim()) ? string.Empty : txtPlanNo.Text.Trim() + "%";
                        string ScNo = string.IsNullOrEmpty(txtSCno.Text.Trim()) ? string.Empty : txtSCno.Text.Trim() + "%";
                        string CustPoNo = string.IsNullOrEmpty(txtPONumber.Text.Trim()) ? string.Empty : txtPONumber.Text.Trim() + "%";
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
                                CompanyPK = Convert.ToInt32(ddlCompanyFilter.SelectedValue)

                            }, currentUser, 0, Convert.ToInt32(CommonConstants.ACTIVE), Status, cusID, 0, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize, PlanNo, ScNo, CustPoNo, string.Empty);
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
                        //get the SC details list
                        ShippingPlanOrderObj = (ShippingPlanOrder)SetUIValuesToObject(type);
                        getxml = CommonFunctions.XmlSerialize<ShippingPlanOrder>(ShippingPlanOrderObj);
                        dsShippingList = BusinessLogic.Shipping.ShippingPlanBL.GetShippingOrderList(getxml);
                        break;
                    case ControlsEnum.SHIPPINGPLANDETAILS:
                        //get the Shipping Details
                        dsShippingPlanHDR = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanHDR(currentUser, ShippingPlanID, Convert.ToInt32(CommonConstants.ACTIVE));
                        dsShippingDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanDetails(0, ShippingPlanID, Convert.ToInt32(CommonConstants.ACTIVE), AppliedSCBrandPKs);

                        break;
                    case ControlsEnum.PLANNO:
                        //Generate PLANNO No
                        ShippingPlanNo = CommonServiceClient.GetTrxDocNo(ApplicationType.SPLN, 0, currentUser.CurrentDeptPK,
                           Convert.ToDateTime(txtGeneratedOn.Text.Trim()), currentUser.PKUser, true, 0, Convert.ToInt32(ddlCompany.SelectedValue));
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
                        dsSPDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanDetails(0, SPID, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    case ControlsEnum.SPCONTAINERTYPE:
                        //For multiple bizunit
                        ConfigurationSettings();
                        //get the Conatiner type
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, Convert.ToInt16(CommonConstants.ACTIVE), (int)ConstGroupType.ContainerType, null, null, BizUnitConfigValue == 1 ? (int?)null : currentUser.SBUID);
                        break;
                    case ControlsEnum.SHIPPINGAGENT:
                        //get the Shipping Agent
                        int cusPk = hdnCustomerID.Value != string.Empty ? Convert.ToInt32(hdnCustomerID.Value) : 0;
                        dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(0, cusPk, 1, (int)CustomerAddressType.ShippingAgent).Tables[0];
                        break;
                    case ControlsEnum.DOLIST:
                        dtDoList = BusinessLogic.Shipping.ShippingPlanBL.GetSaleOrderHdrByShippingPlanPK(ShippingPlanID);
                        break;
                    #region Company
                    case ControlsEnum.COMPANY:
                        ////gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion

                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:

                        dsShippingUploads = BusinessLogic.Shipping.ShippingUploadsBL.GetShippingUploads(0, ShippingPlanID, Convert.ToByte(DbActiveStatus.ACTIVE), ShippingUploadType);

                        if (dsShippingUploads != null && dsShippingUploads.Tables[0].Rows.Count > 0)
                        {
                            var ShippingUpload = from myRow in dsShippingUploads.Tables[0].AsEnumerable()
                                                 select new ShippingPlanUploadDetails()
                                                 {
                                                     SCD_PK = myRow.Field<int>("SCD_PK"),
                                                     SCD_FILE = myRow.Field<string>("SCD_FILE"),
                                                     SCD_TITLE = myRow.Field<string>("SCD_TITLE"),
                                                     SCD_FILE_PATH = myRow.Field<string>("SCD_FILE_PATH"),
                                                     SCD_DESC = myRow.Field<string>("SCD_DESC"),
                                                     SCD_SEQUENCE = myRow.Field<byte>("SCD_SEQUENCE"),
                                                     SCD_ACTIVE = myRow.Field<byte>("SCD_ACTIVE")
                                                 };
                            if (ShippingUpload != null)
                                ShippingUploadList = ShippingUpload.ToList();
                        }
                        break;
                    #endregion

                    #region CUSTOMERSC
                    case ControlsEnum.CUSTOMERSC:
                        int customerPk = hdnCustomerID.Value != string.Empty ? Convert.ToInt32(hdnCustomerID.Value) : 0;
                        dtCustomerSC = BusinessLogic.Shipping.ShippingPlanBL.GetCustomerSaleOrder(currentUser, 0, Convert.ToInt32(CommonConstants.ACTIVE), customerPk, ShippingPlanID);
                        break;
                    #endregion

                    #region SHIPPINGBRANDS
                    case ControlsEnum.SHIPPINGBRANDS:
                        int scPk = Convert.ToInt32(ddlSalesContract.SelectedValue);
                        dtShippingBrands = BusinessLogic.Shipping.ShippingPlanBL.GetCustomerShippingBrands(currentUser, scPk, ShippingPlanID);
                        break;
                    #endregion

                    #region SHIPPINGPLANWEIGHTDETAILS
                    case ControlsEnum.SHIPPINGPLANWEIGHTDETAILS:
                        dtSPWeightDetails = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanWeightDetails(SOPK, ItemCode, SODQty, SODPK);
                        break;
                    #endregion

                    case ControlsEnum.BOISTATUS:
                        //dtBOIStatus = BusinessLogic.Shipping.ShippingPlanBL.GetSaleOrderHdrByShippingPlanPK(ShippingPlanID);
                        //dtBOIStatus = new DataTable();
                        //dtBOIStatus.Columns.Add("StatusID");
                        //dtBOIStatus.Columns.Add("StatusText");

                        //// Add your real values
                        //dtBOIStatus.Rows.Add("1", "Approved");
                        //dtBOIStatus.Rows.Add("2", "Pending");
                        //dtBOIStatus.Rows.Add("3", "Rejected");

                        //// 👉 Add default item at top
                        //DataRow dr = dtBOIStatus.NewRow();
                        //dr["StatusID"] = "0";        // or empty string: ""
                        //dr["StatusText"] = "-- Select --";
                        //dtBOIStatus.Rows.InsertAt(dr, 0);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CommonServiceClient = null;
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
                    case ControlsEnum.SHIPPINGAGENT:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.CUSTOMERSC:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.SHIPPINGBRANDS:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.NEWSCITEMS:
                        BindGrid(ControlsEnum.NEWSCITEMS);
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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region WORKFLOW STATUS
                case ControlsEnum.STATUS:
                    //fill the Work flow status
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
                #endregion

                #region CONTAINER TYPES
                case ControlsEnum.SPCONTAINERTYPE:
                    //fill the container types
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
                #endregion

                #region SHIPPING AGENT
                case ControlsEnum.SHIPPINGAGENT:
                    //fill the Shipping Agents
                    ddlAgent.Items.Clear();
                    if (dtPageData != null)
                    {
                        ddlAgent.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, Resources.DataFieldRes.AgentName);
                        ddlAgent.DataTextField = Resources.DataFieldRes.AgentName;
                        ddlAgent.DataValueField = Resources.DataFieldRes.AgentPK;
                        ddlAgent.DataBind();
                    }
                    ddlAgent.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    ddlCompanyFilter.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompanyFilter.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                        ddlCompanyFilter.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                        ddlCompanyFilter.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanyFilter.DataBind();
                    }
                    ddlCompanyFilter.Items.Insert(0, new ListItem(Resources.ErpRes.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region CustomerSC
                case ControlsEnum.CUSTOMERSC:
                    ddlSalesContract.Items.Clear();
                    ddlSalesContract.SelectedIndex = -1;
                    ddlSalesContract.SelectedValue = null;
                    ddlSalesContract.ClearSelection();
                    if (dtCustomerSC != null && dtCustomerSC.Rows.Count > 0)
                    {
                        ddlSalesContract.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCustomerSC, Resources.DataFieldRes.SONo);
                        ddlSalesContract.DataTextField = Resources.DataFieldRes.SONo;
                        ddlSalesContract.DataValueField = Resources.DataFieldRes.SaleOrderPK;
                        ddlSalesContract.DataBind();
                    }
                    ddlSalesContract.Items.Insert(0, new ListItem(Resources.ErpRes.Select, ERP.Utilities.CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region SHIPPINGBRANDS
                case ControlsEnum.SHIPPINGBRANDS:
                    ddlBrandName.Items.Clear();
                    ddlBrandName.SelectedIndex = -1;
                    ddlBrandName.SelectedValue = null;
                    ddlBrandName.ClearSelection();
                    if (dtShippingBrands != null && dtShippingBrands.Rows.Count > 0)
                    {
                        ddlBrandName.DataSource = CommonFunctions.HtmlDecodeDataTable(dtShippingBrands, Resources.DataFieldRes.SPBrandName);
                        ddlBrandName.DataTextField = Resources.DataFieldRes.SPBrandName;
                        ddlBrandName.DataValueField = Resources.DataFieldRes.OrderDtlPK;
                        ddlBrandName.DataBind();
                    }
                    ddlBrandName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, ERP.Utilities.CommonConstants.SELECTVAL));
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
                //BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                List<SaleOrderPK> saleOrderPks;
                List<ShippingDetails> ShippingDetails;

                int rowID = 0;
                HiddenField hdfSaleOrderDtlPK;
                HiddenField hdfSaleOrderHdrPK;
                TextBox txtplayNow;
                TextBox txtProductPlanNow;
                HiddenField hdfSONumber;
                HiddenField hdfTotlPcs;
                HiddenField hdnPackingPcs;
                HiddenField hdfProductConvFactor;
                HiddenField hdfProductUOMPK;
                Label lblCBM;
                switch (controlType)
                {
                    #region Shipping Plan
                    case ControlsEnum.SHIPPINGPLANHDR:
                        ShippingPlanObj.SNH_PK = ShippingPlanID;
                        ShippingPlanObj.SNH_NO = ShippingPlanNo;
                        ShippingPlanObj.SNH_DATE = Convert.ToDateTime(txtGeneratedOn.Text.Trim());
                        ShippingPlanObj.SNH_CUSTOMER = Convert.ToInt32(hdnCustomerID.Value);
                        ShippingPlanObj.SNH_SHIP_TO_PORT = txtShipPort.Text.Trim();
                        ShippingPlanObj.SNH_SHIP_AGENT = ddlAgent.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlAgent.SelectedValue) : 0;
                        ShippingPlanObj.SNH_LOADING_DATE = txtLoadingDate.Text.Trim();
                        ShippingPlanObj.SNH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
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
                            ShippingPlanObj.SNH_CTN_QTY = lblTotalCTN.Text.Trim() != "" ? Convert.ToDouble(lblTotalCTN.Text.Replace(",", "").Trim()) : 0.0;
                        }
                        GetTotalPlandQty();
                        ShippingPlanObj.SNH_PLAN_QTY = TotalPlandQty;//
                        GetTotalProdPlanQty();
                        ShippingPlanObj.SNH_TRX_STATUS = Convert.ToInt32(CommonConstants.ACTIVE);
                        ShippingPlanObj.SNH_DEPT = currentUser.CurrentDeptPK;
                        ShippingPlanObj.BIZUNIT_PK = currentUser.SBUID;
                        ShippingPlanObj.WKF_PROCESS = Convert.ToInt32(hdfProcessID.Value);
                        ShippingPlanObj.ACTIVE = Convert.ToInt32(CommonConstants.ACTIVE);
                        ShippingPlanObj.USER_PK = currentUser.PKUser;
                        ShippingPlanObj.LAST_MOD_DT = DateTime.Now;
                        ShippingPlanObj.SNH_BOOKING_NO = txtBookingRefNo.Text.Trim();
                        if (txtClosingDate.Text.Trim() != string.Empty)
                        {
                            if (txtClosingTime.Text.Trim() != string.Empty)
                            {
                                //ShippingPlanObj.SNH_CLOSE_DATE = Convert.ToDateTime(txtClosingDate.Text.Trim()).ToString(Resources.Constants.DateFormatShort) + " " + Convert.ToDateTime(txtClosingTime.Text.Trim()).ToString(Resources.Constants.TimeFormatShort);
                                ShippingPlanObj.SNH_CLOSE_DATE = Convert.ToDateTime(txtClosingDate.Text.Trim()).ToString(Resources.Constants.DateFormatShort) + " " + txtClosingTime.Text.Trim();
                            }
                            else
                            {
                                ShippingPlanObj.SNH_CLOSE_DATE = Convert.ToDateTime(txtClosingDate.Text.Trim()).ToString(Resources.Constants.DateFormatShort);
                            }

                        }

                        //ShippingPlanObj.CVH_IN_TIME = txtInTime.Text.Trim() != string.Empty ? Convert.ToDateTime(txtInTime.Text.Trim()) : DateTime.Now;


                        ShippingPlanObj.SNH_FEEDER_VESSEL = txtFeederVessel.Text.Trim();
                        ShippingPlanObj.SNH_MOTHER_VESSEL = txtMotherVessel.Text.Trim();
                        ShippingPlanObj.SNH_REMARKS = txtRemarks.Text.Trim();
                        ShippingPlanObj.SC_VALIDATION = Convert.ToInt32(hdfRequiredSCValidation.Value.Trim());

                        if (txtETA.Text.Trim() != string.Empty)
                        {
                            ShippingPlanObj.SNH_ETA = txtETA.Text.Trim();
                        }
                        ShippingPlanObj.SNH_CONTAINER_NO = txtContNo.Text.Trim();

                        ShippingDetails = new List<ShippingDetails>();

                        //for Calculate  CTN Qty
                        //CTN_QTY= PlanNow / TotalPcs
                        foreach (GridViewRow grdrow in grdShippingList.Rows)
                        {
                            txtplayNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtPlanNow");
                            if (txtplayNow != null && !string.IsNullOrEmpty(txtplayNow.Text.Trim())
                                && Convert.ToDouble(txtplayNow.Text) > 0)
                            {
                                ShippingDetails sd = new ShippingDetails();
                                lblCBM = (Label)grdShippingList.Rows[rowID].FindControl("lblCBM");
                                double cbm = lblCBM.Text.Trim() == string.Empty ? 0 : Convert.ToDouble(lblCBM.Text.Replace(",", ""));
                                hdfSaleOrderDtlPK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSaleOrderDtlPK");
                                sd.SND_PK = Convert.ToInt32(hdfSaleOrderDtlPK.Value);
                                hdfSaleOrderHdrPK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSaleOrderHdrPK");
                                //sd.SND_PLAN_HDR = Convert.ToInt32(hdfSaleOrderHdrPK.Value);
                                sd.SND_PLAN_QTY = Convert.ToDouble(txtplayNow.Text);
                                hdfTotlPcs = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdnTotalPcs");
                                hdfTotlPcs.Value = hdfTotlPcs.Value == string.Empty ? "1" : hdfTotlPcs.Value;
                                sd.SND_CTN_QTY = Math.Ceiling(Convert.ToDouble(txtplayNow.Text) / Convert.ToDouble(hdfTotlPcs.Value));
                                hdnPackingPcs = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdnPackingPcs");
                                hdnPackingPcs.Value = hdnPackingPcs.Value == string.Empty ? "1" : hdnPackingPcs.Value;
                                sd.SND_CBM = cbm;
                                hdfSONumber = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSONumber");
                                sd.SND_SOD = Convert.ToInt32(hdfSONumber.Value);
                                sd.ACTIVE = Convert.ToInt32(CommonConstants.ACTIVE);
                                sd.SND_MOD_BY = currentUser.PKUser;
                                sd.SND_MOD_DT = DateTime.Now;

                                hdfProductConvFactor = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfProductConvFactor");
                                hdfProductUOMPK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfProductUOMPK");
                                txtProductPlanNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtProductPlanNow");

                                sd.SND_SALE_UOM_CONV = string.IsNullOrEmpty(hdfProductConvFactor.Value) ? 1 : Convert.ToDouble(hdfProductConvFactor.Value);

                                if (!string.IsNullOrEmpty(hdfProductUOMPK.Value))
                                {
                                    sd.SND_SALE_UOM = Convert.ToInt32(hdfProductUOMPK.Value);
                                }
                                if (!string.IsNullOrEmpty(txtProductPlanNow.Text))
                                {
                                    sd.SND_SALE_QTY = Convert.ToDouble(txtProductPlanNow.Text);
                                }
                                ShippingDetails.Add(sd);
                            }
                            rowID++;
                        }

                        //atleast one plan qty >0 allow for saving 
                        isSave = false;
                        foreach (ShippingDetails item in ShippingDetails)
                        {
                            if (item.SND_PLAN_QTY > 0)
                            {
                                isSave = true;
                                break;
                            }
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

        #region   SavePlannedQty
        private void SavePlannedQtyChanges()
        {

            string saveXml;
            int result;
            List<object> lstResult = new List<object>();
            isSave = false;
            if (grdShippingList.Rows.Count > 0)
            {
                if (ValidatePlanNowQuantity())
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckPlanNowQty").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else
                {
                    //Check for Total Plan Qty
                    Label lblTotalPlanNow = ((Label)grdShippingList.FooterRow.FindControl("lblTotalPlanNow"));
                    double TotalPlanNow = lblTotalPlanNow != null ? Convert.ToDouble(lblTotalPlanNow.Text.Replace(",", "")) : 0;
                    if (TotalPlanNow > 0)
                    {
                        //if the Plan No generate in Draft Mode then get the Plan No
                        //If already generated in plan no then that plan no is used
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
                        if (ShippingUploadList != null)
                        {
                            if (ShippingUploadList.Count > 0)
                            {
                                ShippingPlanObj.DocDetails = ShippingUploadList;
                            }
                        }
                        if (isSave)
                        {
                            if (ShippingPlanObj != null)
                            {
                                //convert into XML
                                saveXml = CommonFunctions.XmlSerialize<ShippingPlanBO>(ShippingPlanObj);
                                lstResult = BusinessLogic.Shipping.ShippingPlanBL.SaveShippingPlan(saveXml);
                                result = Convert.ToInt32(lstResult[0]);
                                if (result >= 0) // Success ! re-initialize the page
                                {
                                    string savePath = string.Empty;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    {
                                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                        if (!Directory.Exists(savePath))
                                            Directory.CreateDirectory(savePath);
                                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                    }
                                    else
                                    {
                                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                    }
                                    if (ShippingUploadList != null)
                                    {
                                        foreach (ShippingPlanUploadDetails obj in ShippingUploadList)
                                        {
                                            string filePath = savePath + obj.AttachmentFileName;
                                            FileInfo attachedFileInfo = new FileInfo(filePath);
                                            if (FileDetailsList != null)
                                            {
                                                FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SEQUENCE);
                                                if (fileDetailsObj != null)
                                                {
                                                    fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                                                }
                                            }
                                        }
                                    }
                                }
                                else//fail
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
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
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
                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_PlanNow").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                    }
                }
            }
            else
            {
                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }

        }
        #endregion


        /// <summary>
        /// Set Total cartons
        /// </summary>
        /// <returns></returns>
        private void SetTotalCTN()
        {
            int rowID = 0;
            TextBox txtplayNow;
            TextBox txtProductPlanNow;
            HiddenField hdfTotlPcs;
            HiddenField hdnPackingPcs;
            HiddenField hdfProductConvFactor;
            HiddenField hdfSOH_PK;
            HiddenField hdfSOD_PK;
            Label lblOrderQty;
            Label lblPackedQty;
            Label lblCTNQty;
            Label lblCBM;
            Label lblIGPLCode;
            Label lblNetWeight;
            Label lblGrossWeight;
            double Plan_Qty;
            double ProdPlan_Qty;
            double CTN_Qty;
            double CBM;
            double TotalCTNQty = 0.0;
            double TotalCBM = 0.0;
            double TotalPlanQty = 0.0;
            double TotalProdPlanQty = 0.0;
            double TotalNetWeight = 0.0;
            double TotalGrossWeight = 0.0;
            HiddenField hdfIsPackMat;

            //Calculate CBM and CTN_QTY
            // CTN_Qty = PlanNow/ TotalPcs;
            // CBM = CTN_Qty * PackingPcs;

            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                lblCTNQty = (Label)grdShippingList.Rows[rowID].FindControl("lblCTNQty");
                lblCBM = (Label)grdShippingList.Rows[rowID].FindControl("lblCBM");
                txtplayNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtPlanNow");
                txtProductPlanNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtProductPlanNow");
                hdfSOH_PK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSOH_PK");
                hdfSOD_PK = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfSONumber");
                lblIGPLCode = (Label)grdShippingList.Rows[rowID].FindControl("lblIGPLCode");
                lblNetWeight = (Label)grdShippingList.Rows[rowID].FindControl("lblNetWeight");
                lblGrossWeight = (Label)grdShippingList.Rows[rowID].FindControl("lblGrossWeight");
                hdfIsPackMat = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfIsPackMat");
                if (txtProductPlanNow != null && !string.IsNullOrEmpty(txtProductPlanNow.Text.Trim())
                    && Convert.ToDouble(txtProductPlanNow.Text) > 0)
                {

                    lblOrderQty = (Label)grdShippingList.Rows[rowID].FindControl("lblOrderQty");
                    lblPackedQty = (Label)grdShippingList.Rows[rowID].FindControl("lblPackedQty");
                    hdfProductConvFactor = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdfProductConvFactor");
                    Plan_Qty = txtplayNow.Text != string.Empty ? Convert.ToDouble(txtProductPlanNow.Text) * Convert.ToDouble(hdfProductConvFactor.Value) : 0;
                    TotalPlanQty = TotalPlanQty + Plan_Qty;
                    ProdPlan_Qty = txtProductPlanNow.Text != string.Empty ? Convert.ToDouble(txtProductPlanNow.Text) : 0;
                    TotalProdPlanQty = TotalProdPlanQty + ProdPlan_Qty;
                    hdfTotlPcs = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdnTotalPcs");
                    hdnPackingPcs = (HiddenField)grdShippingList.Rows[rowID].FindControl("hdnPackingPcs");
                    hdfTotlPcs.Value = hdfTotlPcs.Value == string.Empty ? "1" : hdfTotlPcs.Value;
                    CTN_Qty = Math.Ceiling(Plan_Qty / Convert.ToDouble(hdfTotlPcs.Value));

                    if (hdfIsPackMat.Value == "1" || hdfIsPackMat.Value == "2")
                        CTN_Qty = 0;

                    lblCTNQty.Text = lblCTNQty.ToolTip = String.Format("{0:n0}", CTN_Qty); //CTN_Qty.ToString();

                    CBM = (Plan_Qty / Convert.ToDouble(hdfTotlPcs.Value)) * Convert.ToDouble(hdnPackingPcs.Value != string.Empty ? hdnPackingPcs.Value : "0"); //CBM = CTN_Qty * Convert.ToDouble(hdnPackingPcs.Value);
                    lblCBM.Text = lblCBM.ToolTip = String.Format("{0:n4}", CBM);
                    TotalCTNQty = TotalCTNQty + CTN_Qty;
                    TotalCBM = TotalCBM + Convert.ToDouble(String.Format("{0:n4}", CBM));
                    lblOrderQty.Text = lblOrderQty.Text != string.Empty ? lblOrderQty.Text : "0";
                    //lblPackedQty.Text=Math.Ceiling(Convert.ToDouble(lblOrderQty.Text)-Plan_Qty).ToString(); 

                    //For Showing Net weight & Gross weight 
                    SOPK = hdfSOH_PK != null ? Convert.ToInt16(hdfSOH_PK.Value) : 0;
                    SODPK = hdfSOD_PK != null ? Convert.ToInt32(hdfSOD_PK.Value) : 0;
                    ItemCode = lblIGPLCode != null ? (lblIGPLCode.Text) : string.Empty;
                    SODQty = Convert.ToDouble(txtplayNow.Text);//Plan Now (Pcs)
                    GetFieldValues(ControlsEnum.SHIPPINGPLANWEIGHTDETAILS);
                    if (dtSPWeightDetails.Rows.Count > 0)
                    {
                        lblNetWeight.Text = lblNetWeight.ToolTip = String.Format("{0:n3}", Convert.ToDouble(dtSPWeightDetails.Rows[0][Resources.DataFieldRes.SND_QTY_NET_WT]));
                        lblGrossWeight.Text = lblGrossWeight.ToolTip = String.Format("{0:n3}", Convert.ToDouble(dtSPWeightDetails.Rows[0][Resources.DataFieldRes.SND_QTY_GROSS_WT]));

                        TotalNetWeight = TotalNetWeight + Convert.ToDouble(dtSPWeightDetails.Rows[0][Resources.DataFieldRes.SND_QTY_NET_WT]);
                        TotalGrossWeight = TotalGrossWeight + Convert.ToDouble(dtSPWeightDetails.Rows[0][Resources.DataFieldRes.SND_QTY_GROSS_WT]);
                    }
                }
                else
                {
                    lblCBM.Text = lblCBM.ToolTip = String.Format("{0:n4}", 0);
                    lblCTNQty.Text = lblCTNQty.ToolTip = "0";

                    lblNetWeight.Text = lblNetWeight.ToolTip = String.Format("{0:n3}", 0);
                    lblGrossWeight.Text = lblGrossWeight.ToolTip = String.Format("{0:n3}", 0);
                }
                rowID++;
            }
            //set the Total Cartons
            Label lblTotalCTN = ((Label)grdShippingList.FooterRow.FindControl("lblTotalCTN"));
            if (lblTotalCTN != null)
            {
                lblTotalCTN.Text = lblTotalCTN.ToolTip = String.Format("{0:n0}", Math.Ceiling(TotalCTNQty));// Math.Ceiling(TotalCTNQty).ToString();
            }
            //set the Total CBM
            Label lblTotalCBM = ((Label)grdShippingList.FooterRow.FindControl("lblTotalCBM"));
            if (lblTotalCBM != null)
            {
                lblTotalCBM.Text = lblTotalCBM.ToolTip = String.Format("{0:n4}", TotalCBM);
            }

            //Set the Total PlanNow
            Label lblTotalPlanNow = ((Label)grdShippingList.FooterRow.FindControl("lblTotalPlanNow"));
            if (lblTotalPlanNow != null)
            {
                lblTotalPlanNow.Text = lblTotalPlanNow.ToolTip = String.Format("{0:n}", TotalPlanQty);// String.Format("{0:n}", Math.Ceiling(TotalCTNQty));
            }

            Label lblProductTotalPlanNow = ((Label)grdShippingList.FooterRow.FindControl("lblProductTotalPlanNow"));
            if (lblProductTotalPlanNow != null)
            {
                lblProductTotalPlanNow.Text = lblProductTotalPlanNow.ToolTip = String.Format("{0:n3}", TotalProdPlanQty);// String.Format("{0:n}", Math.Ceiling(TotalCTNQty));
            }

            //Set the Total Netweight & GrossWeight
            Label lblTotalNetWeight = ((Label)grdShippingList.FooterRow.FindControl("lblTotalNetWeight"));
            Label lblTotalGrossWeight = ((Label)grdShippingList.FooterRow.FindControl("lblTotalGrossWeight"));
            if (lblTotalNetWeight != null)
            {
                lblTotalNetWeight.Text = lblTotalNetWeight.ToolTip = String.Format("{0:n3}", TotalNetWeight);
            }
            if (lblTotalGrossWeight != null)
            {
                lblTotalGrossWeight.Text = lblTotalGrossWeight.ToolTip = String.Format("{0:n3}", TotalGrossWeight);
            }
        }

        /// <summary>
        /// Get Total Pland Qty
        /// </summary>
        /// <returns></returns>
        private void GetTotalPlandQty()
        {
            int rowID = 0;
            TextBox txtplayNow;
            double Plan_Qty;
            //For calculate total Plan Qty
            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                //Get the Plan Qty
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


        private void GetTotalProdPlanQty()
        {
            int rowID = 0;
            TextBox txtProductPlanNow;
            double ProdPlan_Qty;
            //For calculate total Plan Qty
            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                //Get the Plan Qty
                txtProductPlanNow = (TextBox)grdShippingList.Rows[rowID].FindControl("txtProductPlanNow");
                if (txtProductPlanNow != null && !string.IsNullOrEmpty(txtProductPlanNow.Text.Trim())
                    && Convert.ToDouble(txtProductPlanNow.Text) > 0)
                {
                    ProdPlan_Qty = txtProductPlanNow.Text != string.Empty ? Convert.ToDouble(txtProductPlanNow.Text) : 0;
                    TotalPlandQty = TotalPlandQty + ProdPlan_Qty;
                }
                rowID++;
            }
        }

        /// <summary>
        /// Set Pland Qty
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

        private bool PlanNowQuantityChecking()
        {
            bool checkplan = false;

            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                TextBox txtProductPlanNow = (TextBox)grdrow.FindControl("txtProductPlanNow");
                txtProductPlanNow.Attributes.CssStyle.Remove("color");
            }
            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                Label lblOrderQty = (Label)grdrow.FindControl("lblOrderQty");//order qty
                Label lblPackedQty = (Label)grdrow.FindControl("lblPackedQty");//Planed qty
                HiddenField hdfDespatchQty = (HiddenField)grdrow.FindControl("hdfDespatchQty");
                TextBox txtProductPlanNow = (TextBox)grdrow.FindControl("txtProductPlanNow");

                if ((Convert.ToDouble(lblOrderQty.Text.Trim()) - Convert.ToDouble((String.IsNullOrEmpty(hdfDespatchQty.Value)) ? 0 : Convert.ToDouble(hdfDespatchQty.Value)) - Convert.ToDouble(lblPackedQty.Text.Trim())) < Convert.ToDouble(txtProductPlanNow.Text.Trim()))
                {
                    checkplan = true;
                    txtProductPlanNow.Attributes.CssStyle.Add("color", "Red");
                }

            }

            return checkplan;
        }

        private bool ValidatePlanNowQuantity()
        {
            bool checkresult = false;
            if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                foreach (GridViewRow grdrow in grdShippingList.Rows)
                {
                    TextBox txtProductPlanNow = (TextBox)grdrow.FindControl("txtProductPlanNow");
                    txtProductPlanNow.Attributes.CssStyle.Remove("color");
                }
                foreach (GridViewRow grdrow in grdShippingList.Rows)
                {
                    TextBox txtPlanNow = (TextBox)grdrow.FindControl("txtPlanNow");
                    //Label lblDespatchedQty = (Label)grdrow.FindControl("lblDespatchedQty");
                    HiddenField hdfDespatchQty = (HiddenField)grdrow.FindControl("hdfDespatchQty");

                    TextBox txtProductPlanNow = (TextBox)grdrow.FindControl("txtProductPlanNow");
                    if (hdfDespatchQty != null && txtPlanNow != null)
                    {
                        if (Convert.ToDouble(hdfDespatchQty.Value) > 0 && String.IsNullOrEmpty(txtPlanNow.Text.Trim()))
                        {
                            checkresult = true;
                            txtProductPlanNow.Attributes.CssStyle.Add("color", "Red");
                            return checkresult;
                        }
                        else if (!String.IsNullOrEmpty(txtPlanNow.Text.Trim()) && Convert.ToDouble(txtPlanNow.Text) < Convert.ToDouble(hdfDespatchQty.Value))
                        {
                            checkresult = true;
                            txtProductPlanNow.Attributes.CssStyle.Add("color", "Red");
                            return checkresult;
                        }
                    }
                }
            }
            return checkresult;
        }


        private bool ConformPlanNowQuantity()
        {
            bool Cnfcheckresult = false;
            //int IsPlanNowQtyConform = 0;
            //if (EntryStatus == EntryStatus.ENTRYMODE)
            //{
            foreach (GridViewRow grdrow in grdShippingList.Rows)
            {
                TextBox txtProductPlanNow = (TextBox)grdrow.FindControl("txtProductPlanNow");
                //IsPlanNowQtyConform = Convert.ToInt32(Math.Ceiling((Convert.ToDouble(txtProductPlanNow.Text))));
                var float_number = string.IsNullOrEmpty(txtProductPlanNow.Text) ? Convert.ToDouble(0) : Convert.ToDouble(txtProductPlanNow.Text);
                var result = float_number - Math.Truncate(float_number);
                if (float_number - Math.Truncate(float_number) != 0.0)
                {
                    Cnfcheckresult = true;
                    //hdfIscartYes.Value = CommonConstants.SELECT_VALUE_ONE;
                }
            }
            //}
            return Cnfcheckresult;
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

        private void ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                BizUnitConfigValue = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "SHIPPING")["ACF_VALUE"].ToString());
            }
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

            hdfRequiredSCValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "RequiredSCValidation").ToString();
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
                    #region ShippingPlan Header
                    case ControlsEnum.SHIPPINGPLANDETAILS:
                        if (dsShippingPlanHDR != null && dsShippingPlanHDR.Tables[0].Rows.Count > 0)
                        {
                            //For Edit Mode
                            lblShippingPlanNo.Text = ((dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO] == null || dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO].ToString() == "") ? Resources.Messages.DocGenerationNew : dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO].ToString());
                            hdfShippingPlanNo.Value = (dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO] == null ? string.Empty : dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPNO].ToString());
                            txtGeneratedOn.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            txtETD.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPETD].ToString()).ToString(Resources.Constants.DateFormatShort);
                            txtShipPort.Text = CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPShipToPort].ToString(), 500);
                            lblDeliveryTo.Text = CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPCustomerText].ToString(), 50);
                            lblDeliveryTo.ToolTip = CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPCustomerText].ToString(), 300);
                            hfCustomerName.Value = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPCustomerText].ToString();
                            hdnCustomerID.Value = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPCustomer].ToString();
                            lblSPStatus.Text = CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPStatusText].ToString(), 55);
                            lblSPStatus.ToolTip = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPStatusText].ToString();
                            ddlContainerType.SelectedValue = (dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPContainerType] != null && dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPContainerType].ToString() != string.Empty) ? dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPContainerType].ToString() : CommonConstants.SELECTVAL;


                            //lblSPStatus.Text = CommonFunctions.GetShortString(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPStatusText].ToString(), 35);
                            //lblSPStatus.ToolTip = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPStatusText].ToString();
                            txtLoadingDate.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_LOADING_DATE"].ToString()).ToString(Resources.Constants.DateFormatShort);
                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            ddlAgent.SelectedIndex = Convert.ToInt32(ddlAgent.Items.IndexOf(ddlAgent.Items.FindByValue(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_SHIP_AGENT"].ToString())));
                            LastModifiedTime = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_MOD_DT"].ToString());
                            ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsShippingPlanHDR.Tables[0].Rows[0]["SNH_COMPANY"].ToString())));
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            ModifiedDatePnl.Visible = true;
                            //txtGeneratedOn.Focus();
                            lblShippingPlanNo.Focus();
                            hdfDelstatus.Value = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.SPDeleteStatus].ToString();
                            txtBookingRefNo.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.BookingRefNo].ToString();
                            txtFeederVessel.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.FeederVessel].ToString();
                            txtMotherVessel.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.MotherVessel].ToString();
                            txtRemarks.Text = dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.Remarks].ToString();
                            if (!string.IsNullOrEmpty(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.CloseDate].ToString()))
                            {
                                txtClosingDate.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.CloseDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                                //txtClosingTime.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.CloseDate].ToString()).ToString(Resources.Constants.TimeFormatShort);

                                txtClosingTime.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.CloseDate].ToString()).ToString("HH:mm", CultureInfo.CurrentCulture);

                            }

                            if (!string.IsNullOrEmpty(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.ETA].ToString()))
                            {
                                txtETA.Text = Convert.ToDateTime(dsShippingPlanHDR.Tables[0].Rows[0][Resources.DataFieldRes.ETA].ToString()).ToString(Resources.Constants.DateFormatShort);
                            }
                            txtContNo.Text = dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CONTAINER_NO"].ToString() != null ? dsShippingPlanHDR.Tables[0].Rows[0]["SNH_CONTAINER_NO"].ToString() : "";
                        }
                        else if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            //For New Mode
                            txtShipPort.Text = CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SohToPort].ToString(), 500);
                            lblDeliveryTo.ToolTip = CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SohCustomerName].ToString(), 300);
                            lblDeliveryTo.Text = CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SohCustomerName].ToString(), 50);
                            hfCustomerName.Value = dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SohCustomerName].ToString();
                            hdnCustomerID.Value = dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SaleOrderCustomer].ToString();
                            GetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            SetFieldValues(ControlsEnum.SHIPPINGAGENT);
                            ddlAgent.SelectedIndex = Convert.ToInt32(ddlAgent.Items.IndexOf(ddlAgent.Items.FindByValue(dsShippingList.Tables[0].Rows[0]["SOH_SHIP_AGENT"].ToString())));
                            ddlCompany.SelectedIndex = Convert.ToInt32(ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dsShippingList.Tables[0].Rows[0]["SOH_COMPANY"].ToString())));


                            //txtGeneratedOn.Focus();
                            lblShippingPlanNo.Focus();
                        }
                        break;
                    #endregion

                    case ControlsEnum.SELECTEDDOC:
                        if (shippingUploadObj != null)
                        {
                            CurrSlNo = shippingUploadObj.SCD_SEQUENCE;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = shippingUploadObj.SCD_FILE;
                            anchorFile.HRef = shippingUploadObj.SCD_FILE_PATH;
                            txtTitle.Text = shippingUploadObj.SCD_TITLE;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Attributes.Add("class", "removedownloadClass");
                            }
                            else
                            {
                                anchorFile.Attributes.Add("onclick", "return true;");
                                anchorFile.Attributes.Add("class", "downloadClass");
                            }
                        }
                        else
                        {
                            vrfFileUpload.Enabled = true;
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
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SHIPPINGPLAN LIST
                    case ControlsEnum.SHIPPINGPLANHDR:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdShippingPlanList.DataSource = dsPageData.Tables[1];
                        grdShippingPlanList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion

                    #region SCDETAILS LIST
                    case ControlsEnum.SHIPPINGPLANLIST:
                        if (dsShippingDetails != null && dsShippingDetails.Tables[0].Rows.Count > 0)
                        {
                            hdfShipmentTermValue.Value = dsShippingDetails.Tables[0].Rows[0]["SOH_SHIPMENT_TERM_VALUE"].ToString();
                            hdfShipmentTermPK.Value = dsShippingDetails.Tables[0].Rows[0]["SOH_SHIPMENT_TERM"].ToString();
                            hdfShipmentTerm.Value = dsShippingDetails.Tables[0].Rows[0]["SOH_SHIPMENT_TERM_TEXT"].ToString();
                            grdShippingList.DataSource = dsShippingDetails.Tables[0].DefaultView;
                            grdShippingList.DataBind();
                            SetTotalCTN();
                        }
                        else if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            hdfShipmentTermValue.Value = dsShippingList.Tables[0].Rows[0]["SOH_SHIPMENT_TERM_VALUE"].ToString();
                            hdfShipmentTermPK.Value = dsShippingList.Tables[0].Rows[0]["SOH_SHIPMENT_TERM"].ToString();
                            hdfShipmentTerm.Value = dsShippingList.Tables[0].Rows[0]["SOH_SHIPMENT_TERM_TEXT"].ToString();
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
                    #endregion

                    case ControlsEnum.UPLOADEDFILES:
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            grdUploads.DataSource = ShippingUploadList;
                            grdUploads.DataBind();
                        }
                        else
                        {
                            grdUploads.DataSource = null;
                            grdUploads.DataBind();
                        }
                        break;
                    case ControlsEnum.NEWSCITEMS:
                        grdCustomerShipping.DataSource = CustomerBrandsList;
                        grdCustomerShipping.DataBind();
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
                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(ShippingPlanID, ucrWrkf.ProcessID);
                SetCancelRef(CurrPK);
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
                                //Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
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
            hdfIscartYes.Value = CommonConstants.SELECT_VALUE_ZERO;
            CurrPK = 0;
            ShippingPlanID = 0;
            ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
            txtCustomer.Text = Resources.ErpRes.AutoDefaultValue;
            hdfCustomerID.Value = string.Empty;
            SelectedSosForSP = null;
            Session[ERP.Utilities.SessionStrings.SelectedSosForDO] = null;
            lblLastModifiedHDR.Text = string.Empty;
            ModifiedDatePnl.Visible = false;


            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();

            lblShippingPlanNo.Text = string.Empty;
            hdfShippingPlanNo.Value = string.Empty;
            txtGeneratedOn.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            txtShipPort.Text = string.Empty;
            lblDeliveryTo.Text = string.Empty;
            hdnCustomerID.Value = "0";
            txtETD.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            lblSPStatus.Text = Resources.Messages.DocGenerationNew;
            ddlContainerType.SelectedValue = CommonConstants.SELECTVAL;
            txtLoadingDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //lblShippingAgentTxt.Text = lblShippingAgentTxt.ToolTip = string.Empty;
            //hdfShippingAgent.Value = string.Empty;
            ddlAgent.SelectedValue = CommonConstants.SELECTVAL;
            base.WkfRefID = 0;
            txtSCno.Text = string.Empty;
            txtPONumber.Text = string.Empty;
            txtPlanNo.Text = string.Empty;

            txtBookingRefNo.Text = string.Empty;
            txtClosingDate.Text = string.Empty;
            txtClosingTime.Text = string.Empty;
            txtETA.Text = string.Empty;
            txtFeederVessel.Text = string.Empty;
            txtMotherVessel.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            ShippingUploadList = null;
            FileDetailsList = null;
            UploadResetForm();
            AppliedSCBrandPKs = null;
            CustomerBrandsList = null;
            lblCusDespatchQty.Text = string.Empty;
            lblCusOrderQty.Text = string.Empty;
            ShippingSCPKs = null;
            ddlCompanyFilter.SelectedIndex = 0;
        }

        private void UploadResetForm()
        {
            anchorFile.Visible = false;
            vrfFileUpload.Enabled = true;
            CurrSlNo = 0;
            anchorFile.Attributes.Remove("onclick");
            txtTitle.Text = string.Empty;
            //txtDescription.Text = string.Empty;
        }

        /// <summary>
        /// Funtion used get doc mode for shipping plan number
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            //Get the Shipping plan number
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

        private int ValidateShipping()
        {
            List<ShippingSO> objSOList = new List<ShippingSO>();
            ShippingHeaderSO objSOitem = new ShippingHeaderSO();
            objSOitem.SOList = new List<ShippingSO>();

            //bool pick = true;
            int result = 0;
            ShippingSO objSO = new ShippingSO();
            if (ShippingSCPKs != null)
            {
                foreach (long pk in ShippingSCPKs)
                {
                    objSOList.Add(new ShippingSO { SOH_PK = (int)pk });
                }
            }
            if (Convert.ToInt32(ddlSalesContract.SelectedValue) > 0)
            {
                objSO.SOH_PK = Convert.ToInt32(ddlSalesContract.SelectedValue);
                objSOList.Add(objSO);
            }
            objSOitem.SOList = objSOList.GroupBy(itm => itm.SOH_PK).Select(grp => grp.First()).ToList(); //objSOitem.SOList = objSOList.Distinct().ToList();
            string xmlDoc = CommonFunctions.XmlSerialize<ShippingHeaderSO>(objSOitem);
            result = BusinessLogic.Shipping.ShippingPlanBL.CheckforValidShippingSO(xmlDoc);
            //if (result < 0)
            //{
            //    if (result == -3) //NO SC SELECTED
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("NoItemPickforShipping").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -4) //DIFF CUSTOMER
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Customer").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -5) //DIFF TYPE
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -6) //DIFF CURRENCY
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Currency").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -7) //DIFF TAX / DISCOUNT
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -8) //CUSTOM TAX / DISCOUNT
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -9) //MULTIPLE TAX / DISCOUNT
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else if (result == -10) //DISCOUNT EXISTS
            //    {
            //        pick = false;
            //        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //    else //Default 
            //    {
            //        pick = false;
            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            //    }
            //}
            return result;
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
                string soPK;
                string saveXml;
                bool IsQuantity = false;
                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                int result;
                List<object> lstResult = new List<object>();
                string action;
                isSave = false;

                int selectedItemPK;

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
                    if (((DropDownList)sender).ID == "ddlSalesContract")
                    {
                        commonActions = ActionsEnum.SCBRANDNAME;
                    }
                    else if (((DropDownList)sender).ID == "ddlBrandName")
                    {
                        commonActions = ActionsEnum.DISPLAYBRANDQTY;
                    }
                    else
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtProductPlanNow")
                    {
                        commonActions = ActionsEnum.PLANNOWDETAILS;
                    }
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
                        //For SC Details
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
                            int DelStatus = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfDeleteStatus")).Value);
                            if (DelStatus == 1 || Convert.ToInt32(((HiddenField)gvr.FindControl("hdfApproved")).Value) == 0)
                            {
                                btnEditforCancel.Visible = false;
                            }
                            else
                            {
                                btnEditforCancel.Visible = true;
                            }
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
                            //if (PlanNowQuantityChecking())
                            //{
                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckOrderQuantity").ToString();
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //}

                            //if (PlanNowQuantityChecking())
                            //{
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCartCheckConfirming", "$(document).ready(function(){ShowCartCheckConfirming();});", true);
                            // }

                            //Check for Shipping Plan Details
                            if (grdShippingList.Rows.Count > 0)
                            {
                                if (ValidatePlanNowQuantity())
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckPlanNowQty").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    //Check for Total Plan Qty
                                    Label lblTotalPlanNow = ((Label)grdShippingList.FooterRow.FindControl("lblTotalPlanNow"));
                                    double TotalPlanNow = lblTotalPlanNow != null ? Convert.ToDouble(lblTotalPlanNow.Text.Replace(",", "")) : 0;
                                    if (TotalPlanNow > 0)
                                    {
                                        //if the Plan No generate in Draft Mode then get the Plan No
                                        //If already generated in plan no then that plan no is used
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



                                        if (ShippingUploadList != null)
                                        {
                                            if (ShippingUploadList.Count > 0)
                                            {
                                                ShippingPlanObj.DocDetails = ShippingUploadList;
                                            }
                                        }
                                        if (hdfIscartYes.Value == CommonConstants.SELECT_VALUE_ZERO)
                                        {
                                            if (ConformPlanNowQuantity())
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCartCheckConfirming", "$(document).ready(function(){ShowCartCheckConfirming(1);});", true);
                                            }
                                            else
                                            {
                                                if (isSave)
                                                {
                                                    if (ShippingPlanObj != null)
                                                    {
                                                        //if (BusinessLogic.Shipping.ShippingPlanBL.IsContainerReleaseCartonExist(ShippingPlanObj.SNH_PK))
                                                        //{
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ContainerCaronsExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        //    return;
                                                        //}
                                                        //convert into XML
                                                        saveXml = CommonFunctions.XmlSerialize<ShippingPlanBO>(ShippingPlanObj);
                                                        lstResult = BusinessLogic.Shipping.ShippingPlanBL.SaveShippingPlan(saveXml);
                                                        result = Convert.ToInt32(lstResult[0]);
                                                        if (result >= 0) // Success ! re-initialize the page
                                                        {

                                                            string savePath = string.Empty;
                                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                            {
                                                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                                if (!Directory.Exists(savePath))
                                                                    Directory.CreateDirectory(savePath);
                                                                savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                                            }
                                                            else
                                                            {
                                                                savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                                            }
                                                            if (ShippingUploadList != null)
                                                            {
                                                                foreach (ShippingPlanUploadDetails obj in ShippingUploadList)
                                                                {
                                                                    string filePath = savePath + obj.AttachmentFileName;
                                                                    FileInfo attachedFileInfo = new FileInfo(filePath);
                                                                    if (FileDetailsList != null)
                                                                    {
                                                                        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SEQUENCE);
                                                                        if (fileDetailsObj != null)
                                                                        {
                                                                            fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                                                                        }
                                                                    }
                                                                }
                                                            }


                                                            //Show Save success message and reset shipping Entry
                                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                            EntryStatus = EntryStatus.LISTMODE;
                                                            ResetForm();
                                                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                                            SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);

                                                        }
                                                        else//fail
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
                                                                + "','" + Resources.ErpRes.Information + "');", true);
                                                            }
                                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                                            {
                                                                litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                + "','" + Resources.ErpRes.Information + "');", true);
                                                            }
                                                            else if (result == (int)DbSaveStatus.ITEMEXISTINLOADINGPLAN)
                                                            {
                                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_ExistLoadingPlan;
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                + "','" + Resources.ErpRes.Information + "');", true);
                                                            }
                                                            else if (result == (int)DbSaveStatus.ITEMEXISTINDO)
                                                            {
                                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_ExistDO;
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                + "','" + Resources.ErpRes.Information + "');", true);
                                                            }

                                                            else if (result == (int)DbSaveStatus.CHECKSCQUANTITY)
                                                            {
                                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_NotGreaterthanSCQuantity;
                                                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                                //+ "','" + Resources.ErpRes.Information + "');", true);
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                return;
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

                                            }
                                        }
                                        else
                                        {
                                            if (isSave)
                                            {
                                                if (ShippingPlanObj != null)
                                                {
                                                    //if (BusinessLogic.Shipping.ShippingPlanBL.IsContainerReleaseCartonExist(ShippingPlanObj.SNH_PK))
                                                    //{
                                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ContainerCaronsExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    //    return;
                                                    //}
                                                    //convert into XML
                                                    saveXml = CommonFunctions.XmlSerialize<ShippingPlanBO>(ShippingPlanObj);
                                                    lstResult = BusinessLogic.Shipping.ShippingPlanBL.SaveShippingPlan(saveXml);
                                                    result = Convert.ToInt32(lstResult[0]);
                                                    if (result >= 0) // Success ! re-initialize the page
                                                    {

                                                        string savePath = string.Empty;
                                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                        {
                                                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                            if (!Directory.Exists(savePath))
                                                                Directory.CreateDirectory(savePath);
                                                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                                        }
                                                        else
                                                        {
                                                            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                                        }
                                                        if (ShippingUploadList != null)
                                                        {
                                                            foreach (ShippingPlanUploadDetails obj in ShippingUploadList)
                                                            {
                                                                string filePath = savePath + obj.AttachmentFileName;
                                                                FileInfo attachedFileInfo = new FileInfo(filePath);
                                                                if (FileDetailsList != null)
                                                                {
                                                                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SEQUENCE);
                                                                    if (fileDetailsObj != null)
                                                                    {
                                                                        fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                                                                    }
                                                                }
                                                            }
                                                        }


                                                        //Show Save success message and reset shipping Entry
                                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                        EntryStatus = EntryStatus.LISTMODE;
                                                        ResetForm();
                                                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);

                                                    }
                                                    else//fail
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
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                                        {
                                                            litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.ITEMEXISTINLOADINGPLAN)
                                                        {
                                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_ExistLoadingPlan;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.ITEMEXISTINDO)
                                                        {
                                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_ExistDO;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
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

                                        }
                                        //else
                                        //{

                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCartCheckConfirmingSave", "$(document).ready(function(){ShowCartCheckConfirmingSave();});", true);
                                        //}
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_PlanNow").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                    }
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
                            SelectedSosForSP = null;
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
                        FillProcessID(0);
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
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
                            SelectedSosForSP = null;
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
                                    ItemStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
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

                                GetFieldValues(ControlsEnum.UPLOADEDFILES);
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);

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
                                ItemStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(0);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
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
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
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
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region SP Hdr
                    // Do Action for , when click Radio button in  shipping plan list grid
                    case ActionsEnum.SHOWDETAILS:
                        HiddenField hdfDept;
                        HiddenField shippingType;
                        int selectedPK;
                        int dept;

                        gvr = ((RadioButton)sender).Parent.Parent as ExtGridViewRow;
                        selectedPK = ShippingPlanID = InPk = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfShippingPlanID")).Value);

                        hdfDept = gvr.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        int Status = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfDeleteStatus")).Value);

                        if (Status == 1 || Convert.ToInt32(((HiddenField)gvr.FindControl("hdfApproved")).Value) == 0)
                        {
                            btnEditforCancel.Visible = false;
                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                        }
                        //For Print Button visibility
                        shippingType = gvr.FindControl("hdfShippingType") as HiddenField;
                        if (GetGlobalResourceObject("ConfigurationsRes", "CIPrintNeededInShipping").ToString() == "1" && Convert.ToString(shippingType.Value) == "Export")
                        {
                            pnlListPrint.Visible = true;
                        }
                        else
                        {
                            pnlListPrint.Visible = false;
                        }
                        AppliedSCBrandPKs = null;
                        CustomerBrandsList = null;
                        ShippingSCPKs = null;
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                        break;
                    #endregion

                    #region Tab navigation
                    // Do Action for , when click SaleContract tab
                    case ActionsEnum.DEFAULT:
                        //SetTabURL(commonActions);
                        Response.Redirect(Resources.PageURL.SalesOrderListing, false);
                        break;
                    // Do Action for , when click Shipping Plan tab
                    case ActionsEnum.SHIPPINGPLAN:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm();
                        GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
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
                    // Do Action for , when click Print Docs tab
                    //case ActionsEnum.PRINT:
                    //    if (ShippingPlanID == 0)
                    //    {
                    //        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //    }
                    //    else
                    //    {
                    //        PrinterControl1.ShippingPlanID = ShippingPlanID;
                    //        PrinterControl1.SetCommericalInvoice(ShippingPlanID);
                    //        Session[ERP.Utilities.SessionStrings.SHIPPINGPLANPK] = ShippingPlanID.ToString();
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                    //    }
                    //    break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                        if (ItemStatus == 0)
                        {
                            //deliveryOrderServiceClient = new DeliveryOrderService();
                            //deliveryOrderServiceClient = CommonFunctions.InitiateClient(deliveryOrderServiceClient);
                            //result = Convert.ToInt32(deliveryOrderServiceClient.DeleteShippingPlan(ShippingPlanID));
                            ////result = BusinessLogic.Shipping.ShippingPlanBL.DeleteShippingPlan(ShippingPlanID, LastModifiedTime, null);
                            result = BusinessLogic.Shipping.ShippingPlanBL.DeleteShippingPlan(ShippingPlanID, LastModifiedTime);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.ShippingPlan);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                //btnNew.Focus();
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Delete").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
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
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region dropdownchange
                    // Do Action for , when change Dropdown
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SPLN + "&APPSUBTYPE="+ ddlPrint.SelectedValue), false);
                        break;
                    #endregion

                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
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

                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                            salDespatchHdrObj = CommonFunctions.Initilize<SAL_DESPATCH_HDR>();
                            salDespatchHdrObj.DPH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                            GetUIValuesFromObject(ControlsEnum.SHIPPINGPLANDETAILS);
                            SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);

                            WorkflowCore.CoreService workflowCore1 = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore1.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region DELETESUBMIT
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region SAVESUBMIT
                    // Do Action for , when click Save&Submit button
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        if (hdfIscartYes.Value == CommonConstants.SELECT_VALUE_ZERO)
                        {
                            if (ConformPlanNowQuantity())
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCartCheckConfirming", "$(document).ready(function(){ShowCartCheckConfirming(2);});", true);
                            }
                            else
                            {
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            }
                        }
                        else
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion

                    #region SUBMIT
                    // Do Action for , when click Submit button
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        if (hdfIscartYes.Value == CommonConstants.SELECT_VALUE_ZERO)
                        {
                            if (ConformPlanNowQuantity())
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowCartCheckConfirming", "$(document).ready(function(){ShowCartCheckConfirming(3);});", true);
                            }
                            else
                            {
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                hdfIscartYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            }
                        }
                        else
                        {
                            hdfIscartYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
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
                                if (grdShippingList.Rows.Count > 0)
                                {
                                    //if (PlanNowQuantityChecking())
                                    //{
                                    //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckOrderQuantity").ToString();

                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                    //}
                                    //else
                                    //{

                                    if (ValidatePlanNowQuantity())
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckPlanNowQty").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else
                                    {
                                        Label lblTotalPlanNow = ((Label)grdShippingList.FooterRow.FindControl("lblTotalPlanNow"));
                                        double TotalPlanNow = lblTotalPlanNow != null ? Convert.ToDouble(lblTotalPlanNow.Text.Replace(",", "")) : 0;
                                        if (TotalPlanNow > 0)
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

                                            if (ShippingUploadList != null)
                                            {
                                                if (ShippingUploadList.Count > 0)
                                                {
                                                    ShippingPlanObj.DocDetails = ShippingUploadList;
                                                }
                                            }
                                            if (isSave)
                                            {
                                                if (ShippingPlanObj != null)
                                                {
                                                    //if (BusinessLogic.Shipping.ShippingPlanBL.IsContainerReleaseCartonExist(ShippingPlanObj.SNH_PK))
                                                    //{
                                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ContainerCaronsExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    //    return;
                                                    //}
                                                    saveXml = CommonFunctions.XmlSerialize<ShippingPlanBO>(ShippingPlanObj);
                                                    lstResult = BusinessLogic.Shipping.ShippingPlanBL.SaveShippingPlan(saveXml);
                                                    result = Convert.ToInt32(lstResult[0]);
                                                    if (result >= 0) // Success ! re-initialize the page
                                                    {
                                                        string savePath = string.Empty;
                                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                        {
                                                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                            if (!Directory.Exists(savePath))
                                                                Directory.CreateDirectory(savePath);
                                                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                                        }
                                                        else
                                                        {
                                                            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                                        }
                                                        if (ShippingUploadList != null)
                                                        {
                                                            foreach (ShippingPlanUploadDetails obj in ShippingUploadList)
                                                            {
                                                                string filePath = savePath + obj.AttachmentFileName;
                                                                FileInfo attachedFileInfo = new FileInfo(filePath);
                                                                if (FileDetailsList != null)
                                                                {
                                                                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.SCD_SEQUENCE);
                                                                    if (fileDetailsObj != null)
                                                                    {
                                                                        fileDetailsObj.ShippingFile.SaveAs(attachedFileInfo.FullName);

                                                                    }
                                                                }
                                                            }
                                                        }

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
                                                            litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.EditUsedByAnotherUser;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                                        {
                                                            litErrorMsg.Text = Resources.PageNameRes.ShippingPlan + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.ITEMEXISTINLOADINGPLAN)
                                                        {
                                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_ExistLoadingPlan;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.ITEMEXISTINDO)
                                                        {
                                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_ExistDO;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        else if (result == (int)DbSaveStatus.CHECKSCQUANTITY)
                                                        {
                                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_NotGreaterthanSCQuantity;
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                        else
                                                        {
                                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                                + "','" + Resources.ErpRes.Information + "');", true);
                                                        }
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("msg_atleast_one_item_hav_quantity").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_PlanNow").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                        }
                                    }
                                    //}
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                int reslt = BusinessLogic.Shipping.ShippingPlanBL.ShippingPlanCancelCheck(ShippingPlanID);
                                if (reslt > 0)
                                {
                                    ucrWrkf.ApplicationID = (int)ShippingPlanID;
                                }
                                else
                                {
                                    //if (reslt == (int)DbSaveStatus.CODEEXIST) // Container release cartons exist
                                    //    litErrorMsg.Text = GetLocalResourceObject("Err_ContainerCaronsExist").ToString();
                                    //else 
                                    if (reslt == (int)DbSaveStatus.SQLERROR) // invoice exist
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Cancel_Shipping").ToString();
                                    else
                                        litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyUsed").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    //WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    FillProcessID(0);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    //WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                    SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = ShippingPlanID;
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
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        {
                                            FillProcessID(0);
                                            litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_SubmitSuccess").ToString();
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan, lstResult[1]);
                                        }
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        //Show Save success message and reset shipping Entry

                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ShippingPlan);
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.Invoicing) + "');", true);


                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                               + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                            SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                                        }
                                    }
                                }
                            }

                        }
                        break;
                    #endregion

                    #region REMOVE
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

                    #region TOTAL CARTONS
                    // Do Action for , when click setTotal Button
                    case ActionsEnum.SHOW:
                        SetTotalCTN();
                        SetPlandQty();
                        break;
                    #endregion

                    #region PRINT
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
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPrint]','" + Resources.PageNameRes.ShippingPlan + "','420','200');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ShippingPlanID.ToString() + "&APPTYPE=" + ApplicationType.SPRR + "&APPSUBTYPE=3") + "');", true);
                        }
                        break;
                    #endregion

                    #region Printlisting
                    case ActionsEnum.PRINTLISTING:
                        bool flag = false;
                        foreach (GridViewRow grdrow in grdShippingPlanList.Rows)
                        {
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                int DelStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDeleteStatus")).Value);
                                if (DelStatus == 1 || Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value) == 0)
                                {
                                    btnEditforCancel.Visible = false;
                                }
                                else
                                {
                                    btnEditforCancel.Visible = true;
                                }
                                ShippingPlanID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ShippingPlanID.ToString() + "&APPTYPE=" + ApplicationType.SPRR + "&APPSUBTYPE=3") + "');", true);
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region ADDITEM
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
                                if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                {
                                    shippingUploadObj = ShippingUploadList.SingleOrDefault(itm => itm.SCD_SEQUENCE == CurrSlNo);
                                    if (shippingUploadObj != null)
                                    {
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<FileDetails>();
                                        }

                                        shippingUploadObj.SCD_TYPE = ShippingUploadType;
                                        shippingUploadObj.SCD_TITLE = txtTitle.Text;

                                        if (fupUpload.HasFile)
                                        {
                                            FileInfo tempFileInfoObj;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                            shippingUploadObj.AttachmentFileName = attachmentFileName;
                                            shippingUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            shippingUploadObj.SCD_FILE = fupUpload.FileName;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                            if (fileDetailsObj == null)
                                            {
                                                FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, ShippingFile = HttpContext.Current.Request.Files[0] });
                                            }
                                            else
                                            {
                                                fileDetailsObj.ShippingFile = HttpContext.Current.Request.Files[0];
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (fupUpload.HasFile)
                                {

                                    int slno = 1;
                                    if (ShippingUploadList == null || ShippingUploadList.Count == 0)
                                    {
                                        ShippingUploadList = new List<BusinessObject.Shipping.ShippingPlanUploadDetails>();
                                        slno = 1;
                                    }
                                    else
                                    {
                                        slno = ShippingUploadList.Max(itm => itm.SCD_SEQUENCE);
                                        slno++;
                                    }
                                    if (FileDetailsList == null)
                                    {
                                        FileDetailsList = new List<FileDetails>();
                                    }

                                    shippingUploadObj = new ShippingPlanUploadDetails();
                                    shippingUploadObj.SCD_PK = 0;
                                    shippingUploadObj.SCD_SEQUENCE = (byte)slno;
                                    shippingUploadObj.SCD_TYPE = ShippingUploadType;

                                    shippingUploadObj.SCD_TITLE = txtTitle.Text;

                                    FileInfo tempFileInfoObj;
                                    //string SavePath = string.Empty;
                                    //if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                    //{
                                    //    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                    //}
                                    //else
                                    //{
                                    //    //SavePath = Server.MapPath("../Upload");
                                    //    SavePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                                    //}
                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                    shippingUploadObj.AttachmentFileName = attachmentFileName;
                                    shippingUploadObj.FileExtension = tempFileInfoObj.Extension;
                                    shippingUploadObj.SCD_FILE = fupUpload.FileName;
                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                    {
                                        shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    }
                                    else
                                    {
                                        //SavePath = Server.MapPath("../Upload");
                                        //shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        shippingUploadObj.SCD_FILE_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                    }

                                    // shippingUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                    shippingUploadObj.SCD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    FileDetailsList.Add(new FileDetails() { SlNo = slno, ShippingFile = HttpContext.Current.Request.Files[0] });
                                    ShippingUploadList.Add(shippingUploadObj);

                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            UploadResetForm();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                            //ResetForm(ActionsEnum.ADDITEM);
                        }
                        break;
                    #endregion

                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                if (FileDetailsList != null)
                                    FileDetailsList = FileDetailsList.Where(attdoc => selectedItemPK != attdoc.SlNo).ToList();
                                ShippingUploadList = ShippingUploadList.Where(row => selectedItemPK != row.SCD_SEQUENCE).ToList();
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                //ResetForm(ActionsEnum.ADDITEM);
                                UploadResetForm();
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion

                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (ShippingUploadList != null && ShippingUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                shippingUploadObj = ShippingUploadList.SingleOrDefault(row => selectedItemPK == row.SCD_SEQUENCE);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion

                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.IO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion

                    #region PLANNOWDETAILS
                    case ActionsEnum.PLANNOWDETAILS:

                        if (EntryStatus == EntryStatus.ENTRYMODE)
                        {
                            foreach (GridViewRow grdrow in grdShippingList.Rows)
                            {
                                TextBox txtProductPlanNow1 = (TextBox)grdrow.FindControl("txtProductPlanNow");
                                txtProductPlanNow1.Attributes.CssStyle.Remove("color");
                            }

                            GridViewRow grPlanNow = (GridViewRow)((TextBox)sender).NamingContainer;
                            //Label lblDespatchedQty = (Label)grPlanNow.FindControl("lblDespatchedQty");
                            HiddenField hdfDespatchQty = (HiddenField)grPlanNow.FindControl("hdfDespatchQty");
                            TextBox txtPlanNow = (TextBox)grPlanNow.FindControl("txtPlanNow");
                            TextBox txtProductPlanNow2 = (TextBox)grPlanNow.FindControl("txtProductPlanNow");
                            if (hdfDespatchQty != null && txtPlanNow != null)
                            {
                                if (Convert.ToDouble(hdfDespatchQty.Value) > 0 && String.IsNullOrEmpty(txtPlanNow.Text.Trim()))
                                {
                                    txtProductPlanNow2.Attributes.CssStyle.Add("color", "Red");
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckDespQty").ToString();//Msg_Err_CheckPlanNowQty
                                    IsQuantity = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!String.IsNullOrEmpty(txtPlanNow.Text.Trim()) && Convert.ToDouble(txtPlanNow.Text) < Convert.ToDouble(hdfDespatchQty.Value))
                                {
                                    // txtPlanNow.Text = string.Empty;
                                    txtProductPlanNow2.Attributes.CssStyle.Add("color", "Red");
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckDespQty").ToString();
                                    IsQuantity = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        //if (PlanNowQuantityChecking())
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CheckOrderQuantity").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculatePlanNowQty", "CalculatePlanNowQty();", true);
                        //}
                        //else
                        //{
                        SetTotalCTN();
                        SetPlandQty();
                        //to focus the texbox in  grdShippingList

                        int RowIndex = ((GridViewRow)((TextBox)sender).NamingContainer).RowIndex;
                        TextBox txtProductPlanNow = (TextBox)grdShippingList.Rows[RowIndex].FindControl("txtProductPlanNow");

                        //TextBox txtProductPlanNow = (TextBox)grdShippingList.Rows[RowIndex+1].FindControl("txtProductPlanNow"); (to solve inner exception)

                        //GridViewRow selectedGrdrow = (sender as TextBox).Parent.Parent as GridViewRow;
                        //TextBox txtProductPlanNow = (TextBox)selectedGrdrow.FindControl("txtProductPlanNow");

                        Session["event_controle"] = txtProductPlanNow;
                        if (IsQuantity == true)
                        {
                            TextBox txtProductPlanNowTab = (TextBox)grdShippingList.Rows[RowIndex].FindControl("txtProductPlanNow");
                            Session["event_controle"] = txtProductPlanNowTab;
                            IsQuantity = false;
                        }

                        if (Session["event_controle"] != null)
                        {
                            TextBox controle = (TextBox)Session["event_controle"];

                            controle.Focus();
                        }

                        //}
                        break;
                    #endregion

                    #region ADDSCITEM
                    case ActionsEnum.ADDSCITEM:
                        //Changes made in 'Plan Now' is not showing when new data is added through 'Add SC item'
                        if (hdfIsPlanQtyChanged.Value.ToString() == "1")
                        {
                            SavePlannedQtyChanges();
                            hdfIsPlanQtyChanged.Value = "0";
                        }
                        lblScCustomer.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(hfCustomerName.Value), 23);
                        lblScCustomer.ToolTip = HttpUtility.HtmlDecode(hfCustomerName.Value);
                        lblScShiptoport.Text = CommonFunctions.GetShortString(txtShipPort.Text, 19);
                        lblScShiptoport.ToolTip = txtShipPort.Text;
                        lblCusOrderQty.Text = lblCusDespatchQty.Text = string.Empty;
                        GetFieldValues(ControlsEnum.CUSTOMERSC);
                        SetFieldValues(ControlsEnum.CUSTOMERSC);
                        GetFieldValues(ControlsEnum.SHIPPINGBRANDS);
                        SetFieldValues(ControlsEnum.SHIPPINGBRANDS);
                        SetFieldValues(ControlsEnum.NEWSCITEMS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);

                        break;
                    #endregion

                    #region SCITEMSAPPLY
                    case ActionsEnum.SCITEMSAPPLY:
                        AppliedSCBrandPKs = null;
                        CustomerBrandsList = CustomerBrandsList == null ? new List<CustomerBrands>() : CustomerBrandsList.ToList();
                        if (CustomerBrandsList.Count > 0)
                        {
                            AppliedSCBrandPKs = (string.Join(",", CustomerBrandsList.Select(itm => itm.SOD_PK.ToString()).ToArray().Distinct()));
                        }

                        GetFieldValues(ControlsEnum.SHIPPINGPLANDETAILS);
                        SetFieldValues(ControlsEnum.SHIPPINGPLANLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);

                        break;
                    #endregion

                    #region SCBRANDNAME
                    case ActionsEnum.SCBRANDNAME:
                        lblCusDespatchQty.Text = string.Empty;
                        lblCusOrderQty.Text = string.Empty;
                        GetFieldValues(ControlsEnum.SHIPPINGBRANDS);
                        SetFieldValues(ControlsEnum.SHIPPINGBRANDS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);

                        break;
                    #endregion

                    #region DISPLAYBRANDQTY
                    case ActionsEnum.DISPLAYBRANDQTY:
                        if (Convert.ToInt32(ddlBrandName.SelectedValue) > 0)
                        {
                            var brandQty = (from tbl in dtShippingBrands.AsEnumerable()
                                            where tbl.Field<int>("SOD_PK") == Convert.ToInt32(ddlBrandName.SelectedValue)
                                            select new
                                            {
                                                SOD_QTY = tbl.Field<double>("SOD_QTY"),
                                                SOD_QTY_DISPATCHED = tbl.Field<double>("SOD_QTY_DISPATCHED"),
                                            }).FirstOrDefault();
                            lblCusOrderQty.Text = GetFormattedNumber(brandQty.SOD_QTY);
                            lblCusDespatchQty.Text = GetFormattedNumber(brandQty.SOD_QTY_DISPATCHED);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);
                        }
                        else
                        {
                            lblCusOrderQty.Text = lblCusDespatchQty.Text = string.Empty;
                            litErrorMsg.Text = GetLocalResourceObject("Err_Brand").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region ADDBRANDTOLIST
                    case ActionsEnum.ADDBRANDTOLIST:
                        int valid = 0;
                        valid = ValidateShipping();
                        if (valid > 0)
                        {
                            CustomerBrandsList = CustomerBrandsList == null ? new List<CustomerBrands>() : CustomerBrandsList.ToList();
                            CustomerBrandsList.Add(new CustomerBrands
                            {
                                SOH_PK = Convert.ToInt32(ddlSalesContract.SelectedValue),
                                SOH_NO = ddlSalesContract.SelectedItem.Text,
                                SOD_PK = Convert.ToInt32(ddlBrandName.SelectedValue),
                                SOD_BRAND_NAME = ddlBrandName.SelectedItem.Text,
                                SOD_QTY = (from tbl in dtShippingBrands.AsEnumerable()
                                           where tbl.Field<int>("SOD_PK") == Convert.ToInt32(ddlBrandName.SelectedValue)
                                           select tbl.Field<double>("SOD_QTY")).First<double>(),
                                SOD_QTY_DISPATCHED = (from tbl in dtShippingBrands.AsEnumerable()
                                                      where tbl.Field<int>("SOD_PK") == Convert.ToInt32(ddlBrandName.SelectedValue)
                                                      select tbl.Field<double>("SOD_QTY_DISPATCHED")).First<double>()
                            });

                            var DistinctItems = CustomerBrandsList.GroupBy(itm => new { itm.SOH_PK, itm.SOD_PK }).Select(lst => lst.First());
                            CustomerBrandsList = DistinctItems.ToList();
                            SetFieldValues(ControlsEnum.NEWSCITEMS);
                            ddlBrandName.ClearSelection();
                            lblCusDespatchQty.Text = string.Empty;
                            lblCusOrderQty.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);

                        }
                        else
                        {
                            ddlBrandName.ClearSelection();
                            lblCusDespatchQty.Text = string.Empty;
                            lblCusOrderQty.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);

                            if (valid == -3) //NO SC SELECTED
                            {
                                litErrorMsg.Text = GetLocalResourceObject("NoItemPickforShipping").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -4) //DIFF CUSTOMER
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Customer").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -5) //DIFF TYPE
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -6) //DIFF CURRENCY
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Currency").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -7) //DIFF TAX / DISCOUNT
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -8) //CUSTOM TAX / DISCOUNT
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -9) //MULTIPLE TAX / DISCOUNT
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else if (valid == -10) //DISCOUNT EXISTS
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            else //Default 
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }

                        }
                        break;
                    #endregion

                    #region REMOVESCITEM
                    case ActionsEnum.REMOVESCITEM:
                        GridViewRow scgrdrow;
                        scgrdrow = (sender as Button).Parent.Parent as GridViewRow;
                        HiddenField hdfSodPK = scgrdrow.FindControl("hdfSodPK") as HiddenField;
                        if (hdfSodPK != null)
                        {
                            CustomerBrandsList.RemoveAll(x => x.SOD_PK == Convert.ToInt32(hdfSodPK.Value));
                        }
                        SetFieldValues(ControlsEnum.NEWSCITEMS);
                        ddlBrandName.ClearSelection();
                        lblCusDespatchQty.Text = string.Empty;
                        lblCusOrderQty.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AddnewSCItemPopup", "ShowContainerDiv('[id$=divSCItemDetails]','" + GetLocalResourceObject("SCItemDetails").ToString() + "','700','400');", true);
                        break;
                    #endregion
                    #region PRINTCILISTING
                    case ActionsEnum.PRINTCILISTING:
                        if (IsExportExcel != true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + InPk.ToString() + "&APPTYPE=" + ApplicationType.SHCID + "&APPSUBTYPE=2") + "');", true);
                        }
                        else
                        {
                            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export);
                        }

                        //GridView grd = ((CheckBox)sender).Parent.Parent as GridView;
                        //string InvType = hdfInvType.Value;

                        //if (InType != string.Empty)
                        //{

                        //    if ((InType.ToLower() == "export") || (InType == " Deemed Export"))
                        //    {

                        //        if (IsInvoiceTerms == true)
                        //        {
                        //            if (hdfInvTerm.Value.ToString() == "5")
                        //            {
                        //                if (IsExportExcel != true)
                        //                {

                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + InPk.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=5") + "');", true);
                        //                }
                        //                else
                        //                {
                        //                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.InvoiceTermsMain);
                        //                }

                        //            }
                        //            else if (hdfInvTerm.Value.ToString() == "6")
                        //            {
                        //                if (IsExportExcel != true)
                        //                {

                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + InPk.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=6") + "');", true);
                        //                }
                        //                else
                        //                {
                        //                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.InvoiceTerms1);
                        //                }

                        //            }

                        //            else if (hdfInvTerm.Value.ToString() == "7")
                        //            {
                        //                if (IsExportExcel != true)
                        //                {

                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + InPk.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=7") + "');", true);
                        //                }
                        //                else
                        //                {
                        //                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.InvoiceTerms2);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                if (IsExportExcel != true)
                        //                {

                        //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + InPk.ToString() + "&APPTYPE=" + ApplicationType.CID + "&APPSUBTYPE=2") + "');", true);
                        //                }
                        //                else
                        //                {
                        //                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export);
                        //                }

                        //            }



                        //        }
                        //        else
                        //        {
                        //            if (IsExportExcel != true)
                        //            {

                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2") + "');", true);
                        //            }
                        //            else
                        //            {
                        //                Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export);
                        //            }
                        //        }


                        //    }
                        //    else if (InType.ToLower() == "proforma")
                        //    {
                        //        if (IsExportExcel != true)
                        //        {

                        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=3") + "');", true);
                        //        }
                        //        else
                        //        {
                        //            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma);
                        //        }
                        //    }
                        //}
                        break;
                    #endregion
                    #region PRINTPM
                    case ActionsEnum.PRINTPM:
                        foreach (GridViewRow grdrow in grdShippingPlanList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ShippingPlanID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfShippingPlanID")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ShippingPlanID.ToString() + "&APPTYPE=" + ApplicationType.SPLN + "&APPSUBTYPE=4") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
            int slno;
            try
            {
                if (((GridView)sender).ID == "grdShippingList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        LinkButton lnkSoNo = e.Row.FindControl("lnkSoNo") as LinkButton;
                        //Label lblSONo = e.Row.FindControl("lblSONo") as Label;
                        Label lblSODate = e.Row.FindControl("lblSODate") as Label;
                        Label lblIGPLCode = e.Row.FindControl("lblIGPLCode") as Label;
                        Label lblBrandCode = e.Row.FindControl("lblBrandCode") as Label;
                        Label lblUOM = e.Row.FindControl("lblUOM") as Label;
                        Label lblPackedQty = e.Row.FindControl("lblPackedQty") as Label;
                        Label lblCBM = e.Row.FindControl("lblCBM") as Label;
                        HiddenField hdnPackingPcs = e.Row.FindControl("hdnPackingPcs") as HiddenField;

                        Label lblOrderQty = e.Row.FindControl("lblOrderQty") as Label;
                        Label lblDespatchedQty = e.Row.FindControl("lblDespatchedQty") as Label;
                        Label lblCTNQty = e.Row.FindControl("lblCTNQty") as Label;
                        TextBox txtPlanNow = e.Row.FindControl("txtPlanNow") as TextBox;
                        TextBox txtProductPlanNow = e.Row.FindControl("txtProductPlanNow") as TextBox;
                        HiddenField hdfSaleOrderHdrPK = e.Row.FindControl("hdfSaleOrderHdrPK") as HiddenField;
                        HiddenField hdfSaleOrderDtlPK = e.Row.FindControl("hdfSaleOrderDtlPK") as HiddenField;
                        HiddenField hdfIGPLCode = e.Row.FindControl("hdfIGPLCode") as HiddenField;
                        HiddenField hdfBrandCode = e.Row.FindControl("hdfBrandCode") as HiddenField;
                        HiddenField hdfUOM = e.Row.FindControl("hdfUOM") as HiddenField;
                        HiddenField hdnTotalPcs = e.Row.FindControl("hdnTotalPcs") as HiddenField;
                        HiddenField hdfSONumber = e.Row.FindControl("hdfSONumber") as HiddenField;
                        HiddenField hdfDespatchQty = e.Row.FindControl("hdfDespatchQty") as HiddenField;
                        HiddenField hdfProductConvFactor = e.Row.FindControl("hdfProductConvFactor") as HiddenField;
                        HiddenField hdfProductUOMPK = e.Row.FindControl("hdfProductUOMPK") as HiddenField;
                        if (dsShippingDetails != null && dsShippingDetails.Tables[0].Rows.Count > 0)
                        {

                            hdfSaleOrderHdrPK.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPPlanHDR].ToString();
                            hdfSaleOrderDtlPK.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPK].Equals(DBNull.Value) ? "0" : dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPK].ToString();
                            lnkSoNo.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SONumber].ToString();
                            lnkSoNo.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SONumber].ToString();
                            lnkSoNo.CommandArgument = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SCPK].ToString();

                            ShippingSCPKs = ShippingSCPKs == null ? new List<long>() : ShippingSCPKs;
                            if (!ShippingSCPKs.Contains(Convert.ToInt64(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SCPK]))) //  if (ShippingSCPKs.Where(t => t. == SoIdForDO).Count() == 0)
                            {
                                ShippingSCPKs.Add(Convert.ToInt64(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SCPK]));
                            }

                            //lblSONo.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SONumber].ToString();
                            //lblSONo.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SONumber].ToString();
                            lblSODate.Text = Convert.ToDateTime(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = Convert.ToDateTime(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblIGPLCode.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.ItemCode].ToString();
                            lblIGPLCode.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.ItemName].ToString();
                            //hdfIGPLCode.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();

                            //lblBrandCode.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.BrandName].ToString(), 10);
                            lblBrandCode.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.BrandName].ToString();

                            lblBrandCode.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.BrandName].ToString();
                            //hdfBrandCode.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].CRM_CUST_ITEM_MAP.CIM_PK.ToString();

                            //lblUOM.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Disp_UomCode].ToString();
                            //lblUOM.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Disp_UomCode].ToString();
                            //hdfUOM.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();

                            lblUOM.Text = lblUOM.ToolTip = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_UOM_TEXT].ToString();

                            lblOrderQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.OrderQty].ToString();
                            //lblOrderQty.Text = String.Format("{0:n}", Math.Floor(Convert.ToDecimal(lblOrderQty.Text.Trim())));
                            lblOrderQty.Text = String.Format("{0:n}", Convert.ToDecimal(lblOrderQty.Text.Trim()));
                            lblOrderQty.ToolTip = lblOrderQty.Text;
                            lblDespatchedQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SodQtyDispatched].ToString();
                            //lblDespatchedQty.Text = (Convert.ToDecimal(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPTotalPlanQty]) - Convert.ToDecimal(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SodQtyDispatched])).ToString();
                            lblDespatchedQty.Text = String.Format("{0:n}", Convert.ToDouble(lblDespatchedQty.Text.Trim()));
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            lblCTNQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.CartonsQty].ToString();
                            //lblCTNQty.Text = lblCTNQty.ToolTip = lblCTNQty.Text == string.Empty ? "0" : lblCTNQty.Text;
                            lblCTNQty.Text = lblCTNQty.ToolTip = lblCTNQty.Text == string.Empty ? "0" : String.Format("{0:n0}", Math.Ceiling(Convert.ToDecimal(lblCTNQty.Text.Replace(",", "").Trim())));
                            //lblCTNQty.Text =  lblCTNQty.Text.Trim();// String.Format("{0:n}", Math.Ceiling(Convert.ToDecimal(lblCTNQty.Text.Trim())));


                            hdnPackingPcs.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex]["IPD_CBM"].ToString();
                            lblCBM.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex]["SND_CBM"].ToString();
                            lblCBM.Text = lblCBM.Text == string.Empty ? "0" : lblCBM.Text;
                            lblCBM.Text = String.Format("{0:n4}", Math.Ceiling(Convert.ToDecimal(lblCBM.Text.Trim())));
                            lblCBM.ToolTip = lblCBM.Text;

                            hdnTotalPcs.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.TotalPcs].ToString();
                            hdfSONumber.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDeatilsSOD].ToString();

                            txtPlanNow.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.PlanQty].ToString() != string.Empty ? dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.PlanQty].ToString() : "0";
                            txtPlanNow.Text = Math.Round(decimal.Parse(txtPlanNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            txtPlanNow.Text = Math.Floor(Convert.ToDecimal(txtPlanNow.Text.Trim())).ToString();

                            txtProductPlanNow.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_QTY].Equals(DBNull.Value) ? "0" : dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_QTY].ToString();
                            txtProductPlanNow.Text = String.Format("{0:0.00}", Double.Parse(txtProductPlanNow.Text));
                            //String.Format("{0:n}", Double.Parse(txtProductPlanNow.Text)); 
                            //Fix the bug 4387
                            if (decimal.Parse(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPK].ToString()) > 0)
                            {
                                lblPackedQty.Text = (decimal.Parse(dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPTotalPlanQty].ToString()) - decimal.Parse(txtProductPlanNow.Text)).ToString();
                            }
                            else
                            {
                                lblPackedQty.Text = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPTotalPlanQty].ToString();
                            }
                            lblPackedQty.Text = lblPackedQty.Text == string.Empty ? "0" : lblPackedQty.Text;
                            lblPackedQty.Text = String.Format("{0:n}", Convert.ToDecimal(lblPackedQty.Text.Trim()));
                            lblPackedQty.ToolTip = lblPackedQty.Text;
                            hdfDespatchQty.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDespatchQty].ToString();
                            hdfProductUOMPK.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_UOM].Equals(DBNull.Value) ? string.Empty : dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_UOM].ToString();
                            hdfProductConvFactor.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_UOM_CONV].Equals(DBNull.Value) ? "1" : dsShippingDetails.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_SALE_UOM_CONV].ToString();

                        }

                        if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            //hdfSaleOrderHdrPK.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            //hdfSaleOrderDtlPK.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPlanPK] != null ? dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDetailsPlanPK].ToString() : "0";

                            hdfSaleOrderDtlPK.Value = "0";
                            lnkSoNo.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Order].ToString();
                            lnkSoNo.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Order].ToString();
                            lnkSoNo.CommandArgument = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SCPK].ToString();

                            //lblSONo.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Order].ToString();
                            //lblSONo.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.Order].ToString();
                            lblSODate.Text = Convert.ToDateTime(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderHDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = Convert.ToDateTime(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SaleOrderHDate].ToString()).ToString(Resources.Constants.DateFormatShort);
                            lblIGPLCode.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPIGPLCode].ToString();
                            lblIGPLCode.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPIGPLName].ToString();
                            //hdfIGPLCode.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_ITEM_MST.ITM_PK.ToString();

                            //lblBrandCode.Text = ERP.Utilities.CommonFunctions.GetShortString(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPBrandName].ToString(), 10);
                            lblBrandCode.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPBrandName].ToString();
                            lblBrandCode.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPBrandName].ToString();
                            //hdfBrandCode.Value = salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.CRM_CUST_ITEM_MAP.CIM_PK.ToString();

                            //lblUOM.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPUOMTEXT].ToString();
                            //lblUOM.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPUOMTEXT].ToString();
                            //hdfUOM.Value = dsShippingDetails.Tables[0].Rows[e.Row.RowIndex].INV_UOM_MST.UOM_PK.ToString();

                            lblUOM.Text = lblUOM.ToolTip = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPSaleUOMTEXT].ToString();
                            lblOrderQty.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPOrderQty].ToString();
                            //lblOrderQty.Text = String.Format("{0:n}", Math.Floor(Convert.ToDecimal(lblOrderQty.Text.Trim())));
                            lblOrderQty.Text = String.Format("{0:n}", Convert.ToDecimal(lblOrderQty.Text.Trim()));
                            lblOrderQty.ToolTip = lblOrderQty.Text;

                            lblDespatchedQty.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDQty].ToString();// - (salDespatchDtlList[e.Row.RowIndex].SAL_ORDER_DTL.SOD_QTY_DISPATCHED + Double.Parse(txtDespNow.Text))).ToString();
                            lblDespatchedQty.Text = String.Format("{0:n}", Convert.ToDouble(lblDespatchedQty.Text.Trim()));
                            lblDespatchedQty.ToolTip = lblDespatchedQty.Text;

                            //txtPlanNow.Text = String.Format("{0:N}", decimal.Parse(lblDespatchedQty.Text));
                            //txtPlanNow.Text = Math.Round(decimal.Parse(txtPlanNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();

                            hdnTotalPcs.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPPQty].ToString();
                            hdfSONumber.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.OrderDtlPK].ToString();



                            //lblCTNQty.Text = lblCTNQty.ToolTip = Math.Ceiling(Convert.ToDouble(txtPlanNow.Text != string.Empty ? txtPlanNow.Text.Trim() : "0") / Convert.ToDouble(hdnTotalPcs.Value != string.Empty ? hdnTotalPcs.Value : "1")).ToString();// String.Format("{0:n}", Math.Ceiling(Convert.ToDouble(txtPlanNow.Text != string.Empty ? txtPlanNow.Text.Trim() : "0") / Convert.ToDouble(hdnTotalPcs.Value != string.Empty ? hdnTotalPcs.Value : "1")));
                            lblCTNQty.Text = lblCTNQty.ToolTip = string.Format("{0:n0}", Math.Ceiling(Convert.ToDouble(txtPlanNow.Text != string.Empty ? txtPlanNow.Text.Trim() : "0") / Convert.ToDouble(hdnTotalPcs.Value != string.Empty ? hdnTotalPcs.Value : "1")));


                            lblPackedQty.Text = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDeatilsPlanQty].ToString();
                            lblPackedQty.Text = lblPackedQty.Text == string.Empty ? "0" : lblPackedQty.Text;
                            lblPackedQty.Text = String.Format("{0:n}", Convert.ToDecimal(lblPackedQty.Text.Trim()));
                            lblPackedQty.ToolTip = lblPackedQty.Text;

                            hdnPackingPcs.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex]["IPD_CBM"].ToString();
                            lblCBM.Text = lblCBM.ToolTip = String.Format("{0:n4}", Math.Ceiling(Convert.ToDouble(lblPackedQty.Text != string.Empty ? lblPackedQty.Text.Trim() : "0") * Convert.ToDouble(hdnPackingPcs.Value != string.Empty ? hdnPackingPcs.Value : "0")));

                            decimal sodOrderQty = decimal.Parse(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPOrderQty].ToString());
                            decimal sodPlanQty = decimal.Parse(lblPackedQty.Text);
                            decimal sodDispatchedQty = decimal.Parse(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SPDQty].ToString());
                            decimal planNow = 0, last_planqty = 0, DOQTY = 0;
                            if (dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_PLAN_QTY_LAST].ToString() != "0")
                            {
                                last_planqty = decimal.Parse(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SND_PLAN_QTY_LAST].ToString());

                            }
                            if (dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.DPD_QTY_DESPATCHED_LAST].ToString() != "0")
                            {
                                DOQTY = decimal.Parse(dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.DPD_QTY_DESPATCHED_LAST].ToString());

                            }


                            // decimal planNow = (sodOrderQty - sodPlanQty) > (sodOrderQty - sodDispatchedQty) ? (sodOrderQty - sodDispatchedQty) : (sodOrderQty - sodPlanQty);
                            if (sodOrderQty >= sodPlanQty && sodDispatchedQty == 0)
                            {
                                planNow = sodOrderQty - sodPlanQty;
                            }
                            else if (sodOrderQty >= sodDispatchedQty && sodDispatchedQty > 0)
                            {
                                planNow = sodOrderQty - (DOQTY > 0 ? sodDispatchedQty : (last_planqty + sodDispatchedQty));
                            }
                            //planNow = sodOrderQty - (sodDispatchedQty >= 0 ? sodDispatchedQty : sodPlanQty);

                            //string PlannowSC = Math.Round(planNow, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            string PlannowSC = String.Format("{0:0.00}", planNow);
                            //Math.Round(planNow, Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();

                            PlannowSC = Convert.ToDecimal(PlannowSC.Trim()).ToString();
                            txtProductPlanNow.Text = PlannowSC.Contains('-') ? "0" : PlannowSC;

                            hdfProductUOMPK.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SodSaleUOMPK].ToString();
                            hdfProductConvFactor.Value = dsShippingList.Tables[0].Rows[e.Row.RowIndex][Resources.DataFieldRes.SodSaleUOMConvFactor].ToString();


                            txtPlanNow.Text = (Convert.ToDouble(hdfProductConvFactor.Value) * Convert.ToDouble(txtProductPlanNow.Text)).ToString();

                            // txtPlanNow.Text = planNow.ToString();                          
                            // txtPlanNow.Text = Math.Round(decimal.Parse(txtPlanNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits).ToString();
                            //txtPlanNow.Text = Math.Floor(Convert.ToDecimal(txtPlanNow.Text.Trim())).ToString();
                            //txtProductPlanNow.Text = txtPlanNow.Text = txtPlanNow.Text.Contains('-') ? "0" : txtPlanNow.Text;


                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {

                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (dsShippingList != null && dsShippingList.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[8].Text = GetLocalResourceObject("PlanNow").ToString() + " (" + dsShippingList.Tables[0].Rows[0][Resources.DataFieldRes.SPUOMTEXT] + ")";
                        }
                        else if (dsShippingDetails != null && dsShippingDetails.Tables[0].Rows.Count > 0)
                        {
                            e.Row.Cells[8].Text = GetLocalResourceObject("PlanNow").ToString() + " (" + dsShippingDetails.Tables[0].Rows[0][Resources.DataFieldRes.Disp_UomCode] + ")";
                        }
                    }

                }

                else if (((GridView)sender).ID == "grdShippingPlanList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        RadioButton rbtSelect = e.Row.FindControl("rbtSelect") as RadioButton;
                        //LinkButton lnkDoNumber = e.Row.FindControl("lnkDoNumber") as LinkButton;
                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        Label lblPlantCode = e.Row.FindControl("lblPlantCode") as Label;
                        lblPlantCode.Visible = GetConfigData().IsMultiplePlant;
                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        //if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowBOIStatusInShppingPlan")))
                        //{
                        //    DropDownList ddlStatus = e.Row.FindControl("ddlStatus") as DropDownList;

                        //    GetFieldValues(ControlsEnum.BOISTATUS);
                        //    ddlStatus.DataSource = dtBOIStatus;
                        //    ddlStatus.DataTextField = "StatusText";
                        //    ddlStatus.DataValueField = "StatusID";
                        //    ddlStatus.DataBind();
                        //}
                        switch (appstatus)
                        {
                            case (short)WkfStatusEnum.CANCELLED:
                                rbtSelect.Enabled = false;
                                //lnkDoNumber.Enabled = false;
                                break;
                        }
                    }
                    //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    //    {
                    //        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                    //        //Image imgPosted = e.Row.FindControl("imgPosted") as Image;

                    //        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                    //        //HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                    //        short appstatus = Convert.ToInt16(hdfApproved.Value);
                    //        switch (appstatus)
                    //        {
                    //            case (short)WkfStatusEnum.APPROVED:
                    //                imgApproved.CssClass = GetLocalResourceObject("mark").ToString();
                    //                imgApproved.ToolTip = Resources.Captions.Approved;
                    //                break;
                    //            case (short)WkfStatusEnum.DRAFTED:
                    //                imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                    //                imgApproved.ToolTip = Resources.Captions.Drafted;
                    //                break;
                    //            case (short)WkfStatusEnum.NEW:
                    //                imgApproved.CssClass = GetLocalResourceObject("close").ToString();
                    //                imgApproved.ToolTip = Resources.Captions.Drafted;
                    //                break;
                    //        }

                    //        //if (Convert.ToBoolean(hdfPosted.Value) == true)
                    //        //{
                    //        //    imgPosted.ImageUrl = GetLocalResourceObject("Img_True").ToString();
                    //        //}
                    //        //else
                    //        //{
                    //        //    imgPosted.ImageUrl = GetLocalResourceObject("Img_False").ToString();
                    //        //}
                    //    }
                }
                if (((GridView)sender).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = CommonConstants.SELECT_VALUE_ZERO;
                    }
                }

                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            e.Row.Cells[4].Visible = false;
                            e.Row.Cells[5].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
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
            uclPaging.CurrentPage = 1;
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkShippingPlan.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkList.PreRender += new EventHandler(btnAction_PreRender);
            this.lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAddItem.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnAddnewSCItem.PreRender += new EventHandler(btnAction_PreRender);
            this.btnApplyItems.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            this.lbnSOListing.Load += new EventHandler(btnAction_Load);
            this.lnkShippingPlan.Load += new EventHandler(btnAction_Load);
            this.lnkList.Load += new EventHandler(btnAction_Load);
            this.lnkDetail.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnListPrint.Load += new EventHandler(btnAction_Load);
            this.btnAddItem.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnAddnewSCItem.Load += new EventHandler(btnAction_Load);
            this.btnApplyItems.Load += new EventHandler(btnAction_Load);
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
                //Hide and show the Search Section
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                //Calulate CBM,Cartons and Total Plan Qty in Client Side
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowAdditionalInfoDetails", "$(document).ready(function(){ShowHideAdditionalInfo();});", true);

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
        private void FillProcessID(int Type)
        {
            string path = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            if (Type != 0)
            {
                path = path + "?TYPE=" + Type;
            }

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {

                    base.WkfPageUrl = ucrWrkf.PageUrl = path;
                    PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                }
            }
        }
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
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
            SHIPPINGAGENT,
            COMPANY,
            DOLIST,
            UPLOADEDFILES,
            SELECTEDDOC,
            CUSTOMERSC,
            SHIPPINGBRANDS,
            NEWSCITEMS,
            SHIPPINGPLANWEIGHTDETAILS,
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