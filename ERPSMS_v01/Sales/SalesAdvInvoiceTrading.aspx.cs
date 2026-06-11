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
using BusinessObject;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessLogic.CommonManagement;
using BusinessObject.SaleOrder;

namespace ERPSMS_v01.Sales
{
    public partial class SalesAdvInvoiceTrading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Sale Order TYPE
        /// </summary>
        private int SaleOrderType
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SaleOrderType];
            }
            set
            {
                this.ViewState[ViewstateStrings.SaleOrderType] = value;
            }
        }


        /// <summary>
        /// Is continue
        /// </summary>
        private bool Iscont
        {
            get
            {
                return this.ViewState[ViewstateStrings.Iscont] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.Iscont]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Iscont] = value;
            }
        }
        /// <summary>
        /// Is Allow All Bizunit Currency
        /// </summary>
        private bool IsBizUnitCur
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsBizUnitCur] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsBizUnitCur]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsBizUnitCur] = value;
            }
        }
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrSOPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrSOPK] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
            }
        }

        

        private int RPTTYPE
        {
            get
            {
                return this.ViewState["RPTTYPE"] == null ? 1 : Convert.ToInt32(this.ViewState["RPTTYPE"]);
            }
            set
            {
                this.ViewState["RPTTYPE"] = value;
            }
        }
       

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
        private int InvoiceId
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.InvoiceId];
            }
            set
            {
                this.ViewState[ViewstateStrings.InvoiceId] = value;
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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

        /// <summary>
        /// To maintain keep PoHeader List
        /// </summary>
        private string ProcessidDummy
        {
            get
            {
                return (string)Session[ERP.Utilities.SessionStrings.ProcessidDummy];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.ProcessidDummy] = value;
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
        /// Status
        /// </summary>
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
        /// Customer Pk
        /// </summary>
        private int CustomerID
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerID] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerID];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerID] = value;
            }
        }
        /// <summary>
        /// Currency
        /// </summary>
        private int Currency
        {
            get
            {
                return this.ViewState[ViewstateStrings.Currency] == null ? 0 : (int)this.ViewState[ViewstateStrings.Currency];
            }
            set
            {
                this.ViewState[ViewstateStrings.Currency] = value;
            }
        }
        /// <summary>
        /// Invoice Type
        /// </summary>
        private int InvType
        {
            get
            {
                return this.ViewState[ViewstateStrings.InvType] == null ? 0 : (int)this.ViewState[ViewstateStrings.InvType];
            }
            set
            {
                this.ViewState[ViewstateStrings.InvType] = value;
            }
        }    
       
        /// <summary>
        /// Tax calculation is enabled or disabled for advance invoice .
        /// </summary>
        private bool IsAdvInvHasTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAdvInvHasTax] == null ? false : (bool)this.ViewState[ViewstateStrings.IsAdvInvHasTax];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAdvInvHasTax] = value;
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

        /// <summary>
        /// To keep pending SO list
        /// </summary>
        private DataTable dtPendingSOList
        {
            get
            {
                return this.ViewState[ViewstateStrings.OtherDetailList] == null ? new DataTable() : (DataTable)this.ViewState[ViewstateStrings.OtherDetailList];
            }
            set
            {
                this.ViewState[ViewstateStrings.OtherDetailList] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexSO
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndexList];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndexList] = value;
            }
        }

        /// <summary>
        /// To maintain keep SO Invoice Header Tax Splitting
        /// </summary>
        private DirectSOInvoiceHeader SOInvoiceHeaderSession
        {
            get
            {
                return (DirectSOInvoiceHeader)Session[ERP.Utilities.SessionStrings.DirectSOInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.DirectSOInvoiceHeaderSession] = value;
            }
        }
        /// <summary>
        /// To maintain the Then Direction in viewstate
        /// </summary>
        private string ThenDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenDirection] = value;
            }
        }
        /// <summary>
        /// Is invoice cancelled or not
        /// </summary>
        private bool IsDeleted
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDeleted] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDeleted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDeleted] = value;
            }
        }
        #endregion
        #region Variables       
        private ActionsEnum commonActions;     
        private ServiceUtility serviceUtilityObj;       
        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private DirectSOInvoiceHeader invoiceHeaderObj;       
        private DirectSOHeaderBO SoHeaderObj;

        DataTable dtSOData;     
        private int salesOrderPK = 0;
        User currentUser;
        DataTable dtCompany = new DataTable();
        DataSet dsCustomerTypes = new DataSet();
        DataSet dsCustomerDetailsByType;
        private int CustomerTypeSelectedPk;
        private string CustomerSavedBranchId;
        private string CustomerSavedTaxId;
        private int custPK;
        private long InvoicePk = 0;
        DataTable dtAmountDetails;
        private DataTable dtInvoiceType;
        private DataTable dtInvoiceList;
        private DataSet dsPageData;     
        DataTable dtTaxSettings;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        bool isCancelled = false;
        bool ischanged = false;
        private string refID;
        private string inboxFlag;
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
            string prefID;
            try
            {
                ddlPlantName.Visible = lblPlantName.Visible = GetConfigData().IsMultiplePlant;
                hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
                hdfDecimalFormat.Value = "#0.";
                hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                {
                    hdfDecimalFormat.Value += "0";
                }
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormat.Value = "#0.";
                hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                    hdfCurrencyFormatWithSeperator.Value += "0";
                }
                hdfRateFormat.Value = "#0.";
                int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                for (int i = 0; i < rateDecimalDigits; i++)
                {
                    hdfRateFormat.Value += "0";
                }

                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }


                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.COMPANYNAME);
                    SetFieldValues(ControlsEnum.COMPANYNAME);
                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);

                    ischanged = false;
                    hdfJournalizeWorkFlow.Value = "0";
                    Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;

                    Department deptCurrency = BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDeptDetailsByID(
                        Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                    Session[BusinessObject.Common.SessionStrings.CurDeptCountry] = deptCurrency.BaseCountry;

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();                  
                    if (IsAdvInvHasTax)
                    {
                        ddlInvoiceType.Enabled = true;
                    }
                    else
                    {
                        ddlInvoiceType.Enabled = false;
                    }                   

                    //hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    ////start
                    //if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    //{
                    //    CurrSOPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERPK]);
                    //    Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                    //    ////start
                    //    EntryStatus = EntryStatus.NEWMODE;
                    //    ////
                    //    if (IsAdvInvHasTax)
                    //        ddlInvoiceType.Enabled = true;
                    //}
                    //else
                    //{
                    //    ddlInvoiceType.Enabled = false;
                    //}

                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    ////
                    //Used for Integration purpose
                    //   FillProcessID(1);
                    int processId = 1;
                   
                    int.TryParse(pid, out processId);
                   
                    if (ProcessidDummy == "3")
                    { processId = 3; }
                    if (processId == 3)
                        FillProcessID(3);
                    else
                        FillProcessID(1);
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
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
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("3") || pid.Equals("11"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfInvDelStatus.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }                        
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    if (CurrPK > 0)
                    {
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;                       
                        GetFieldValues(ControlsEnum.INVOICETYPE);
                        SetFieldValues(ControlsEnum.INVOICETYPE);                       
                        hdfIVHPK.Value = CurrPK.ToString();                     
                        GetFieldValues(ControlsEnum.SOINVHEADER);
                        SetFieldValues(ControlsEnum.SOINVHEADER);
                        SetFieldValues(ControlsEnum.SOINVOICELIST);
                        GetFieldValues(ControlsEnum.PEDINGSOLIST);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        ddlInvoiceType.Enabled = false;
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                    SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                    SetBranchIDEnableDisable();

                    #region Fill workflow details with invoice type
                    if (!IsAdvInvHasTax && CurrPK <= 0)
                    {
                        if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
                        {
                            if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(3);
                            else
                                FillProcessID(1);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.NEWMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                        }
                    }
                    #endregion
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
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            int? Status = null;
            int TotalRecords = 0;           
            string xmlDocSO = string.Empty;
            try
            {
                switch (type)
                {
                    #region PEDING SO LIST
                    case ControlsEnum.PEDINGSOLIST:
                        TotalPages = 0;
                        custPK = 0;
                        TotalRecords = 0;
                        int.TryParse(hdfCustomer.Value, out custPK);
                        int.TryParse(hdfSOPK.Value, out salesOrderPK);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 0;//string.IsNullOrEmpty(PageIndexSO) ? 1 : Convert.ToInt32(PageIndexSO);
                        serviceUtilityObj.PageSize = 0;// Convert.ToInt32(GetLocalResourceObject("PageSize_SOList"));
                        dtPendingSOList = BusinessLogic.Sales.SalesInvoiceBL.GetPendingSalesOrderList(custPK, salesOrderPK, CurrPK, string.Empty, currentUser.SBUID, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);
                        //if (dtPendingSOList != null && dtPendingSOList.Rows.Count > 0)
                        //{
                        //    //set Total Page Count
                        //    TotalRecords = dtPendingSOList.Rows.Count > 0 ? Convert.ToInt32(dtPendingSOList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                        //    TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                        //                (TotalRecords % serviceUtilityObj.PageSize) == 0 ? (TotalRecords / serviceUtilityObj.PageSize) :
                        //                (TotalRecords / serviceUtilityObj.PageSize) + 1;
                        //}
                        break;
                    #endregion
                    #region SOINVHEADER
                    case ControlsEnum.SOINVHEADER:
                        xmlDocSO = string.Empty;
                        if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                        {
                            xmlDocSO = CommonFunctions.XmlSerialize<DirectSOHeaderBO>(SoHeaderObj);
                        }
                        //CustAllAdv = SaleOrderType == 1 ? 0 : 1;//SaleOrderType = 1 : Domestic
                        invoiceHeaderObj = BusinessLogic.Sales.SalesInvoiceBL.GetDirectSalesInvoiceHeaderMUL(xmlDocSO, !string.IsNullOrEmpty(xmlDocSO) ? 0 : CurrPK);
                        if (SOInvoiceHeaderSession == null)
                            SOInvoiceHeaderSession = invoiceHeaderObj.DeepClone();
                        else if (invoiceHeaderObj != null && SOInvoiceHeaderSession != null)
                        {
                            List<int> objSoList = new List<int>();
                            if (SOInvoiceHeaderSession.SOMappingDetails != null)
                                objSoList = SOInvoiceHeaderSession.SOMappingDetails.Select(r => r.ICM_SO_HDR).Distinct().ToList();
                            List<DirectSOInvoiceMappingDetails> objMpgList = invoiceHeaderObj.SOMappingDetails.Where(r => !objSoList.Contains(r.ICM_SO_HDR)).ToList();
                            if (objMpgList != null && objMpgList.Count > 0)
                            {
                                objMpgList.ForEach(dtl =>
                                {
                                    SOInvoiceHeaderSession.SOMappingDetails.Add(dtl);
                                });
                            }
                        }
                        break;
                    #endregion
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        TotalRecords = 0;
                        int cusID = String.IsNullOrEmpty(hdfCustomerSearchID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerSearchID.Value);
                        if (hdfCustomerSearchID.Value != null && hdfCustomerSearchID.Value != "0" && hdfCustomerSearchID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerSearchID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomerSearch.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int InvType = Convert.ToInt32(ddlSaleOrderType.SelectedValue);
                        string customer = string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()) ? string.Empty : (txtCustomerSearch.Text.Trim() == Resources.ErpRes.AutoDefaultValue ? string.Empty : txtCustomerSearch.Text.Trim());
                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetDirectSalesInvoiceList(
                                new BusinessObject.GridPrams()
                                {
                                    SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SalesInvoiceDate : SortBy,
                                    SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                    ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy,
                                    ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                    FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                    ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                    SearchBy = "ICH_NO",
                                    SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == Resources.ErpRes.AutoDefaultValue ? string.Empty : txtInvoiceNumber.Text.Trim()),
                                    PageNumber = string.IsNullOrEmpty(PageIndex) ? 1 : Convert.ToInt32(PageIndex),
                                    PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"))
                                }, currentUser, cusID, InvPk, CurrSOPK, HttpUtility.HtmlEncode(customer), InvType, txtSCno.Text
                                 , Resources.PageURL.TradingAdvInv.Replace("~", "")
                                 , string.Empty
                                 , Status
                                 , 0
                                 , string.Empty
                                 , 0
                                 , (byte)SalesInvoiceCategory.Advanced
                             );
                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtCustomerSearch.Text.Trim()) ? string.Empty : (txtCustomerSearch.Text.Trim() == Resources.ErpRes.AutoDefaultValue ? string.Empty : txtCustomerSearch.Text.Trim());
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();
                            int pagsize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                            TotalRecords = dtInvoiceList.Rows.Count > 0 ? Convert.ToInt32(dtInvoiceList.Rows[0]["ROW_COUNT"].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= pagsize) ? 1 :
                                        (TotalRecords % pagsize) == 0 ? (TotalRecords / pagsize) :
                                        (TotalRecords / pagsize) + 1;
                        }
                        break;
                    #endregion                   
                    #region Get Exchange Rate
                    case ControlsEnum.EXCHANGERATE:
                        //Get Exchange Rate
                        salesInvoiceServiceClient = new SalesInvoiceService();
                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        double ExchgRate = salesInvoiceServiceClient.GetConversionFactor(
                                                             invoiceHeaderObj.ICH_CURRENCY, invoiceHeaderObj.ICH_BASE_CURR,
                                                             Convert.ToDateTime(invoiceHeaderObj.ICH_DATE), (IsBizUnitCur == true ? currentUser.SBUID : 0));
                        hdfExchangeRate.Value = ExchgRate.ToString();
                        break;
                    #endregion                   
                                    
                    #region INVOICETYPE
                    case ControlsEnum.INVOICETYPE:
                        dtInvoiceType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE TYPE", "Advance");
                        break;
                    #endregion                                   
                    #region SALES_INVOICE_TYPE
                    case ControlsEnum.SALES_INVOICE_TYPE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.SALES_INVOICE_TYPE;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
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
                        //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region COMPANY NAME
                    case ControlsEnum.COMPANYNAME:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //  dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break; 
                    #endregion
                    #region SO TYPE
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE TYPE", "Regular");
                        break; 
                    #endregion
                    #region CustomerTypes
                    case ControlsEnum.CUSTOMERTYPES:
                        int.TryParse(hdfCustomer.Value, out custPK);
                        if (custPK > 0)
                            dsCustomerTypes = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(0, custPK, 1, null);
                        break;
                    #endregion
                    #region GetCustomerDetailsByCustomerType
                    case ControlsEnum.GETCUSTOMERDETAILSBYTYPE:
                        int CAD_PK = 0;
                        int.TryParse(ddlCustomerType.SelectedValue, out CAD_PK);
                        int CustomerPK = 0;
                        //int.TryParse(hdfCurrCustomerPK.Value, out CustomerPK);
                        dsCustomerDetailsByType = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(CAD_PK, CustomerPK, 2, 0);
                        break;
                    #endregion
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();                       
                        dtAmountDetails = BusinessLogic.Sales.SalesInvoiceBL.GetInvCusReceivedAmntDetails(Convert.ToInt16(InvoicePk)).Tables[0];
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
                salesInvoiceServiceClient = null;
                admCompanyMstServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
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
                    #region PEDING SO LIST
                    case ControlsEnum.PEDINGSOLIST:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region SO INV HEADER
                    case ControlsEnum.SOINVHEADER:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region COMPANY NAME
                    case ControlsEnum.COMPANYNAME:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANYNAME);
                        break; 
                    #endregion
                    #region INVOICE LIST
                    case ControlsEnum.INVOICELIST:
                        BindGrid(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region PO Invoice List
                    case ControlsEnum.SOINVOICELIST:
                        BindGrid(ControlsEnum.SOINVOICELIST);
                        break;
                    #endregion
                   
                    #region InvoiceType
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(ControlsEnum.INVOICETYPE);
                        break;
                    #endregion
                    #region SO TYPE
                    case ControlsEnum.SOTYPE:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region CUSTOMER TYPES
                    case ControlsEnum.CUSTOMERTYPES:
                        BindDropDown(ControlsEnum.CUSTOMERTYPES);
                        break; 
                    #endregion
                    #region GET CUSTOMER DETAILS BY TYPE
                    case ControlsEnum.GETCUSTOMERDETAILSBYTYPE:
                        if (ddlCustomerType.SelectedValue == CommonConstants.SELECTVAL)
                        {

                            txtTypeID.Text = "";
                            txtTaxID.Text = "";
                        }
                        else
                        {
                            if (dsCustomerDetailsByType != null & dsCustomerDetailsByType.Tables[0].Rows.Count > 0)
                            {
                                txtTypeID.Text = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE_NAME"].ToString());
                                txtTaxID.Text = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_CUSTOMER_GST"].ToString());

                                hdfCustomerTypeId.Value = Convert.ToString(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]);
                                if (Convert.ToInt32(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]) == (int)CustomerContactTypeEnum.Branch)
                                {
                                    txtTypeID.Enabled = true;
                                    vrfBranchCode.Enabled = true;
                                    txtTypeID.CssClass = "input-small";
                                }
                                else
                                {
                                    txtTypeID.Enabled = false;
                                    txtTypeID.Text = "00000";
                                    vrfBranchCode.Enabled = false;
                                    txtTypeID.CssClass = "input-small input-disabled";
                                }

                            }
                            else
                            {
                                txtTypeID.Text = "";
                                txtTaxID.Text = "";
                            }
                        }
                        break; 
                    #endregion
                    #region AMOUNT DETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region UPDATE GRID VAL TO OBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        SetUIValuesToObject(controlType);
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
                LinkButton lbnBalAmt;
                //Label lblBalAmt;
                decimal balamt;
                balamt = 0;
                AlertBO alertBoObj;
                bool InvalidReceiptItem = false;
                int invType = 1;
                HiddenField hdfSONumber;
                TextBox txtPayNow;
                TextBox txtOthercharges;
                HiddenField hdfSOTax;
                HiddenField hdfSODiscount;
                HiddenField hdfAdjustPerInvAmt;
                double taxAmnt;
                double discAmnt;
                double adjAmnt;
                double otherAmnt;
                switch (controlType)
                {
                    #region Invoice Header
                    case ControlsEnum.FINANCEINVOICEHDR:
                        if (SOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = SOInvoiceHeaderSession;
                            invoiceHeaderObj.ICH_PK = CurrPK;
                            invoiceHeaderObj.ICH_NO = lblDispInvoiceNo.Text.Trim();
                            invoiceHeaderObj.ICH_TYPE = ddlInvoiceType.SelectedValue;
                            invoiceHeaderObj.ICH_DATE = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now.ToString() : txtInvdate.Text.Trim();
                            invoiceHeaderObj.ICH_REFERENCE = !string.IsNullOrEmpty(hdfInvoiceReference.Value) ? hdfInvoiceReference.Value : string.Empty;
                            invoiceHeaderObj.ICH_CUSTOMER = hdfCustomer.Value;                                               
                            invoiceHeaderObj.ICH_DATE_PAY_BY = string.IsNullOrEmpty(txtPaybydate.Text.Trim()) ? DateTime.Now.ToString() : txtPaybydate.Text.Trim();
                            invoiceHeaderObj.ICH_AMOUNT_TC = string.IsNullOrEmpty(txtInvoiceAmt.Text) ? 0 : Convert.ToDouble(txtInvoiceAmt.Text);
                            invoiceHeaderObj.ICH_DISCOUNT_TC = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDouble(txtDiscount.Text);
                            invoiceHeaderObj.ICH_TAX_TC = string.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDouble(txtTaxAmount.Text);
                            invoiceHeaderObj.ICH_NET_VALUE_TC = invoiceHeaderObj.ICH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(txtNetAmount.Text);
                            invoiceHeaderObj.ICH_AMOUNT_ADV_DED_TC = 0;
                            invoiceHeaderObj.ICH_BASE_CURR = currentUser.BaseCurrency;
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            invoiceHeaderObj.ICH_EXCHG_RATE = double.Parse(hdfExchangeRate.Value);
                            invoiceHeaderObj.ICH_AMOUNT_NET_BC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(invoiceHeaderObj.ICH_EXCHG_RATE) * Convert.ToDouble(txtNetAmount.Text);
                            invoiceHeaderObj.ICH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            invoiceHeaderObj.ICH_ACTIVE = "1";
                            invoiceHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            invoiceHeaderObj.LAST_MOD_DT = LastModifiedTime;
                            invoiceHeaderObj.ICH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            invoiceHeaderObj.ICH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            invoiceHeaderObj.ICH_AMOUNT_RCVD_TC = 0;
                            invoiceHeaderObj.ICH_AMOUNT_DN_TC = 0;
                            invoiceHeaderObj.ICH_AMOUNT_CN_TC = 0;
                            invoiceHeaderObj.ICH_CATEGORY = (byte)SalesInvoiceCategory.Advanced;                
                            invoiceHeaderObj.ICH_CUSTOMER = hdfCustomer.Value;                           
                            invoiceHeaderObj.ICH_SHIP_CHARGE = Convert.ToDouble(hdfOCFooter.Value);
                            invoiceHeaderObj.ICH_AMOUNT_ADJUST = 0;                          
                            invoiceHeaderObj.ICH_ORG_GOODS = null;                           
                            invoiceHeaderObj.ICH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);                           
                            invoiceHeaderObj.ICH_BRANCH = ddlCustomerType.SelectedValue;
                            invoiceHeaderObj.ICH_BRANCH_TYPE = hdfCustomerTypeId.Value;//HO/Branch(4/5)
                            invoiceHeaderObj.ICH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtTypeID.Text);
                            invoiceHeaderObj.ICH_TAX_ID = HttpUtility.HtmlEncode(txtTaxID.Text);
                            invoiceHeaderObj.ICH_IS_OPENING = 0;
                            invoiceHeaderObj.AST_CODE = ApplicationType.DSI;
                            invoiceHeaderObj.OrderDetail = null;//for adv invoice no need to save item details
                            invoiceHeaderObj.SOMappingDetails = (List<DirectSOInvoiceMappingDetails>)SetUIValuesToObject(ControlsEnum.FINANCEINVOICETRXMPG);
                        }
                        retObject = invoiceHeaderObj;
                        break;
                    #endregion

                    #region Invoice Trx Mpg
                    case ControlsEnum.FINANCEINVOICETRXMPG:
                        List<DirectSOInvoiceMappingDetails> objMappingDetailsList = new List<DirectSOInvoiceMappingDetails>();
                        foreach (GridViewRow grdrow in grdSalesList.Rows)
                        {
                            taxAmnt = 0;
                            discAmnt = 0;
                            adjAmnt = 0;
                            otherAmnt = 0;
                            DirectSOInvoiceMappingDetails objMappingDetails = new DirectSOInvoiceMappingDetails();
                            txtOthercharges = (TextBox)grdrow.FindControl("txtOthercharges");
                            hdfSOTax = (HiddenField)grdrow.FindControl("hdfSOTax");
                            hdfSODiscount = (HiddenField)grdrow.FindControl("hdfSODiscount");
                            hdfAdjustPerInvAmt = (HiddenField)grdrow.FindControl("hdfAdjustPerInvAmt");
                            hdfSONumber = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfSONumber").ToString());
                            objMappingDetails.ICM_PK = CurrMpgPK;
                            objMappingDetails.ICM_INVOICE_HDR = CurrPK;
                            objMappingDetails.ICM_SO_HDR = hdfSONumber == null ? 0 : Convert.ToInt32(hdfSONumber.Value);
                            txtPayNow = (TextBox)grdrow.FindControl(GetLocalResourceObject("txtPayNow").ToString());
                            objMappingDetails.ICM_AMOUNT = string.IsNullOrEmpty(txtPayNow.Text.Trim()) ? 0 : Convert.ToDouble(txtPayNow.Text.Trim());
                            objMappingDetails.ICM_ACTIVE = 1;
                            if (txtOthercharges != null)
                                double.TryParse(txtOthercharges.Text, out otherAmnt);
                            objMappingDetails.ICM_OTHER_AMOUNT = otherAmnt;
                            if (hdfSOTax != null)
                                double.TryParse(hdfSOTax.Value, out taxAmnt);
                            objMappingDetails.ICM_TAX_AMOUNT = taxAmnt;
                            if (hdfSODiscount != null)
                                double.TryParse(hdfSODiscount.Value, out discAmnt);
                            objMappingDetails.ICM_DISCOUNT_AMOUNT = discAmnt;
                            if (hdfAdjustPerInvAmt != null)
                                double.TryParse(hdfAdjustPerInvAmt.Value, out adjAmnt);
                            objMappingDetails.ICM_ADJUST_AMOUNT = adjAmnt;
                            objMappingDetailsList.Add(objMappingDetails);
                        }
                        retObject = objMappingDetailsList;
                        break;
                    #endregion
                    
                    #region UPDATE GRID VAL TO OBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.SOMappingDetails != null)
                        {
                            foreach (GridViewRow grdrow in grdSalesList.Rows)
                            {
                                taxAmnt = 0;
                                discAmnt = 0;
                                adjAmnt = 0;
                                otherAmnt = 0;
                                hdfSONumber = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfSONumber").ToString());
                                txtPayNow = (TextBox)grdrow.FindControl(GetLocalResourceObject("txtPayNow").ToString());
                                txtOthercharges = (TextBox)grdrow.FindControl("txtOthercharges");
                                DirectSOInvoiceMappingDetails objMapping = SOInvoiceHeaderSession.SOMappingDetails.SingleOrDefault(r => r.ICM_SO_HDR == Convert.ToInt32(hdfSONumber.Value));
                                hdfSOTax = (HiddenField)grdrow.FindControl("hdfSOTax");
                                hdfSODiscount = (HiddenField)grdrow.FindControl("hdfSODiscount");
                                hdfAdjustPerInvAmt = (HiddenField)grdrow.FindControl("hdfAdjustPerInvAmt");
                                objMapping.ICM_INVOICE_HDR = CurrPK;
                                objMapping.ICM_SO_HDR = hdfSONumber == null ? 0 : Convert.ToInt32(hdfSONumber.Value);
                                objMapping.ICM_AMOUNT = string.IsNullOrEmpty(txtPayNow.Text.Trim()) ? 0 : Convert.ToDouble(txtPayNow.Text.Trim());
                                objMapping.ICM_ACTIVE = 1;
                                if (txtOthercharges != null)
                                    double.TryParse(txtOthercharges.Text, out otherAmnt);
                                objMapping.ICM_OTHER_AMOUNT = otherAmnt;
                                if (hdfSOTax != null)
                                    double.TryParse(hdfSOTax.Value, out taxAmnt);
                                objMapping.ICM_TAX_AMOUNT = taxAmnt;
                                if (hdfSODiscount != null)
                                    double.TryParse(hdfSODiscount.Value, out discAmnt);
                                objMapping.ICM_DISCOUNT_AMOUNT = discAmnt;
                                if (hdfAdjustPerInvAmt != null)
                                    double.TryParse(hdfAdjustPerInvAmt.Value, out adjAmnt);
                                objMapping.ICM_ADJUST_AMOUNT = adjAmnt;
                            }
                        }
                        retObject = SOInvoiceHeaderSession;
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


        #region Set BranchID Enable/Disable
        private void SetBranchIDEnableDisable()
        {
            if (ddlCustomerType.SelectedValue != "")
            {
                GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
            }

        }
        #endregion


        /// <summary>
        /// Is Matcing Tax
        /// </summary>
        /// <param name="TaxList"></param>
        /// <param name="tax"></param>
        /// <returns></returns>
        private bool IsMatcingTax(List<decimal> TaxList, decimal tax)
        {

            bool flag = true;
            bool BaseType = true;
            bool CurType = true;
            if (TaxList != null)
            {
                BaseType = tax > 0 ? true : false;
                foreach (long var in TaxList)
                {
                    CurType = var > 0 ? true : false;
                    if (BaseType != CurType)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// Is Same Customer
        /// </summary>
        /// <param name="Customers"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCustomer(List<long> Customers, long pk)
        {

            bool flag = true;
            if (Customers != null)
                foreach (long ven in Customers)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

        /// <summary>
        /// Is Same InvoiceTypes
        /// </summary>
        /// <param name="Customers"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameInvTypes(List<long> InvTypes, long pk)
        {

            bool flag = true;
            if (InvTypes != null)
                foreach (long ven in InvTypes)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

        /// <summary>
        /// Is Same Currency
        /// </summary>
        /// <param name="Currencies"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCurrency(List<long> Currencies, long pk)
        {

            bool flag = true;
            if (Currencies != null)
                foreach (long ven in Currencies)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
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
                    #region SO INV HEADER
                    case ControlsEnum.SOINVHEADER:
                        invoiceHeaderObj = SOInvoiceHeaderSession;
                        if (invoiceHeaderObj != null)
                        {
                            CurrPK = invoiceHeaderObj.ICH_PK;
                            lblDispInvoiceNo.Text = string.IsNullOrEmpty(invoiceHeaderObj.ICH_NO) ? Resources.ErpRes.Draft : invoiceHeaderObj.ICH_NO;
                            txtCustomer.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CUSTOMER_TEXT);
                            hdfCustomer.Value = invoiceHeaderObj.ICH_CUSTOMER.ToString();
                            lblInvoiceAmt.Text = GetLocalResourceObject("InvoiceAmt").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CURRENCY_TEXT) + ")";
                            lblDiscount.Text = GetLocalResourceObject("Discount").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CURRENCY_TEXT) + ")";
                            lblTaxAmount.Text = GetLocalResourceObject("TaxAmount").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CURRENCY_TEXT) + ")";
                            lblNetAmount.Text = GetLocalResourceObject("NetAmount").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_CURRENCY_TEXT) + ")";
                            txtPaybydate.Text = !string.IsNullOrEmpty(invoiceHeaderObj.ICH_DATE_PAY_BY) ? Convert.ToDateTime(invoiceHeaderObj.ICH_DATE_PAY_BY).ToString(Resources.Constants.DateFormatShort) : string.Empty;
                            if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_DATE_PAY_BY))
                                hdfPaybydate.Value = invoiceHeaderObj.ICH_DATE_PAY_BY;
                            SaleOrderType = Convert.ToInt32(invoiceHeaderObj.ICH_TYPE);
                            if (Convert.ToInt32(invoiceHeaderObj.ICH_TYPE) == Convert.ToInt32(SalesInvoiceType.Export))
                            {
                                SaleOrderType = Convert.ToInt32(SalesInvoiceType.Proforma);
                            }
                            else
                            {
                                SaleOrderType = Convert.ToInt32(invoiceHeaderObj.ICH_TYPE);
                                if (!IsAdvInvHasTax)
                                    SaleOrderType = Convert.ToInt32(SalesInvoiceType.Proforma);
                            }
                            txtInvoiceAmt.Text = GetFormattedCurrency(invoiceHeaderObj.ICH_AMOUNT_TC.ToString());
                            txtDiscount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(invoiceHeaderObj.ICH_DISCOUNT_TC.ToString()) : "0";
                            txtInvdate.Text = Convert.ToDateTime(invoiceHeaderObj.ICH_DATE).ToString(Resources.Constants.DateFormatShort);
                            hdfInvdate.Value = invoiceHeaderObj.ICH_DATE.ToString();
                            txtTaxAmount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(invoiceHeaderObj.ICH_TAX_TC.ToString()) : "0";
                            txtNetAmount.Text = GetFormattedCurrency(invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString());
                            txtRemarks.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_REMARKS);
                            LastModifiedTime = invoiceHeaderObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            hdfInvRcvdAmt.Value = invoiceHeaderObj.ICH_AMOUNT_RCVD_TC.ToString();
                            hdfIsJournalize.Value = invoiceHeaderObj.ICH_HAS_JRNL_ENTRY.ToString();
                            Approved = invoiceHeaderObj.ICH_STATUS;
                            //hdfInvDelStatus.Value = invoiceHeaderObj.ICH_DEL_STATUS.ToString();
                            hdfInvoiceReference.Value = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.ICH_REFERENCE);
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            ddlInvoiceType.SelectedValue = invoiceHeaderObj.ICH_TYPE;
                            ddlCompany.SelectedValue = invoiceHeaderObj.ICH_COMPANY.ToString();
                            txtCurrr.Text = invoiceHeaderObj.ICH_CURRENCY_TEXT;                                         

                            CustomerTypeSelectedPk = string.IsNullOrEmpty(invoiceHeaderObj.ICH_BRANCH.ToString()) ? 0 : Convert.ToInt32(invoiceHeaderObj.ICH_BRANCH);
                            CustomerSavedBranchId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_BRANCH_TEXT) ? "" : invoiceHeaderObj.ICH_BRANCH_TEXT;
                            CustomerSavedTaxId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_TAX_ID) ? "" : invoiceHeaderObj.ICH_TAX_ID;
                            GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                            SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                            SetBranchIDEnableDisable();
                            if (CurrPK > 0)
                            {
                                //In Edit Mode For Showing Saving Data
                                if (CustomerTypeSelectedPk > 0 && ddlCustomerType.Items.FindByValue(CustomerTypeSelectedPk.ToString()) != null)
                                {
                                    ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
                                    txtTypeID.Text = CustomerSavedBranchId.ToString();
                                    txtTaxID.Text = ERP.Utilities.CommonFunctions.GetDecodedString(CustomerSavedTaxId.ToString());
                                }
                                else
                                {
                                    txtTypeID.Text = "";
                                    txtTaxID.Text = "";
                                }
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
                #region INVOICE TYPE
                case ControlsEnum.INVOICETYPE:
                    ddlInvoiceType.Items.Clear();
                    if (dtInvoiceType != null && dtInvoiceType.Rows.Count > 0)
                    {
                        ddlInvoiceType.DataSource = dtInvoiceType;
                        ddlInvoiceType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlInvoiceType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlInvoiceType.DataBind();
                    }
                    ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break; 
                #endregion
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
                #region COMPANY NAME
                case ControlsEnum.COMPANYNAME:
                    ddlPlantName.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlPlantName.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                        ddlPlantName.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                        ddlPlantName.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlPlantName.DataBind();
                    }
                    ddlPlantName.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break; 
                #endregion
                #region SOTYPE
                case ControlsEnum.SOTYPE:
                    ddlSaleOrderType.Items.Clear();
                    if (dtSOData != null)
                    {
                        ddlSaleOrderType.DataSource = dtSOData;
                        ddlSaleOrderType.DataTextField = "CFG_DATA";
                        ddlSaleOrderType.DataValueField = "CFG_VALUE";
                        ddlSaleOrderType.DataBind();
                    }
                    ddlSaleOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Customer Types
                case ControlsEnum.CUSTOMERTYPES:
                    ddlCustomerType.Items.Clear();
                    if (dsCustomerTypes != null && dsCustomerTypes.Tables.Count > 0 && dsCustomerTypes.Tables[0].Rows.Count > 0)
                    {
                        ddlCustomerType.DataSource = CommonFunctions.HtmlDecodeDataTable(dsCustomerTypes.Tables[0], "CAD_NAME");
                        ddlCustomerType.DataTextField = "CAD_NAME";
                        ddlCustomerType.DataValueField = "CAD_PK";
                        ddlCustomerType.DataBind();
                        SetBranchIDEnableDisable();
                    }
                    ddlCustomerType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (CustomerTypeSelectedPk > 0 && ddlCustomerType.Items.FindByValue(CustomerTypeSelectedPk.ToString()) != null)
                        ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
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
                    #region PEDING SO LIST
                    case ControlsEnum.PEDINGSOLIST:
                        //uclSOPaging.TotalPages = TotalPages;
                        PageIndexSO = string.IsNullOrEmpty(PageIndexSO) ? CommonConstants.SELECT_VALUE_ONE : PageIndexSO;
                        //uclSOPaging.CurrentPage = Convert.ToInt32(PageIndexSO);
                        if (dtPendingSOList != null && dtPendingSOList.Rows.Count > 0)
                            grdSoList.DataSource = dtPendingSOList;
                        else
                            grdSoList.DataSource = null;
                        grdSoList.DataBind();
                        //uclSOPaging.Visible = true;
                        //uclSOPaging.BindPager();
                        break;
                    #endregion
                    #region Invoice Hdr
                    case ControlsEnum.INVOICELIST:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtInvoiceList != null)
                        {
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdSalesInvoiceList.PageIndex = Convert.ToInt32(PageIndex);
                            grdSalesInvoiceList.DataSource = dtInvoiceList.DefaultView;
                            grdSalesInvoiceList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                            ////For Setting/Resetting Colour of a selected InvoiceNo
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            ////End
                        }
                        else
                        {
                            grdSalesInvoiceList.DataSource = null;
                            grdSalesInvoiceList.DataBind();
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    #region SO Invoice List
                    case ControlsEnum.SOINVOICELIST:
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.SOMappingDetails != null && SOInvoiceHeaderSession.SOMappingDetails.Count > 0)
                            grdSalesList.DataSource = SOInvoiceHeaderSession.SOMappingDetails.ToList();
                        else
                            grdSalesList.DataSource = null;
                        grdSalesList.DataBind();

                        if (ddlInvoiceType.SelectedValue != "-1" && Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            grdSalesList.Columns[10].Visible = true;
                        }
                        else
                        {
                            grdSalesList.Columns[10].Visible = false;
                        }
                        if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) && IsAdvInvHasTax)
                        {
                            grdSalesList.Columns[14].Visible = true;
                            grdSalesList.Columns[15].Visible = true;
                            DivtaxFooter.Visible = true;
                            DivDiscFooter.Visible = true;
                        }
                        else
                        {
                            grdSalesList.Columns[14].Visible = false;
                            grdSalesList.Columns[15].Visible = false;
                            DivtaxFooter.Visible = false;
                            DivDiscFooter.Visible = false;
                        }

                        break;
                    #endregion
                    #region AMOUNT DETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        if (dtAmountDetails != null && dtAmountDetails.Rows.Count > 0)
                            grdPaidAmntSplitup.DataSource = dtAmountDetails;
                        else
                            grdPaidAmntSplitup.DataSource = null;
                        grdPaidAmntSplitup.DataBind();                        
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
                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                SetCancelRef(CurrPK);
                ucrWrkf.FillWorkFlowDetails();
                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                {
                    ucrWrkf.ViewType = 1;
                    //btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.ViewType = 0;
                    //EntryStatus = EntryStatus.VIEWMODE;
                    //btnSave.Visible = false;
                }
                ucrWrkf.ViewAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method used to Reset Form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            IsDeleted = false;
            SOInvoiceHeaderSession = null;
            txtInvoiceNumber.Text = "Select/Type";
            txtCustomerSearch.Text = "Select/Type";
            hdfIVHPK.Value = "";
            hdfCustomerSearchID.Value = "";
            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            //txtFromDate.Text = string.Empty;
            //hdfFromDate.Value = string.Empty;
            //txtToDate.Text = string.Empty;
            //hdfToDate.Value = string.Empty; 
            txtSCno.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
           
            ModifiedDatePnl.Visible = false;         
            base.WkfRefID = 0;
            ddlInvoiceType.Enabled = false;
            //hdfTaxSettings.Value = string.Empty;
            ddlSaleOrderType.SelectedIndex = 0;
            ddlPlantName.SelectedIndex = 0;
            hdfIsPendingSOVisible.Value = "0";
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region SO INV HEADER
                case ControlsEnum.SOINVHEADER:
                    CurrPK = 0;
                    IsDeleted = false;
                    SOInvoiceHeaderSession = null;
                    lblDispInvoiceNo.Text = Resources.ErpRes.Draft;
                    hdfInvoiceNo.Value = string.Empty;
                    txtCustomer.Text = string.Empty;
                    hdfCustomer.Value = string.Empty;
                    txtSONumber.Text = string.Empty;
                    hdfSOPK.Value = string.Empty;
                    txtInvdate.Text = string.Empty;
                    txtPaybydate.Text = string.Empty;
                    ddlInvoiceType.ClearSelection();
                    txtCurrr.Text = string.Empty;
                    ddlCustomerType.ClearSelection();
                    txtTypeID.Text = string.Empty;
                    txtTaxID.Text = string.Empty;
                    txtInvoiceAmt.Text = string.Empty;
                    txtNetAmount.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    grdSoList.DataSource = null;
                    grdSoList.DataBind();
                    grdSalesList.DataSource = null;
                    grdSalesList.DataBind();
                    hdfIsPendingSOVisible.Value = "0";

                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();
                    break;
                #endregion
                #region RESETRECEIPT
                case ControlsEnum.RESETRECEIPT:
                    Currency = 0;
                    CustomerID = 0;
                    InvType = 0;
                    hdfSelectedItemPk.Value = "0";
                    break;
                #endregion
            }
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
        public string GetFormattedCurrencyWithSeperator(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
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
            
            try
            {               
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                GridViewRow gridRow;
                bool bIsChecked = false;               
                long? result;              
                TextBox WrkfComments;
                decimal TotalAmount = 0;
                decimal NetAmount = 0;
                Label lblTotalPayNowFooter;               
                HiddenField hdfDept;               
                int dept;
                int invType = 1;
                string soPK;
                CheckBox chkInvselect;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlInvoiceType")
                    {
                        commonActions = ActionsEnum.SALESINVOICETYPECHANGED;
                    }                   
                    if (((DropDownList)sender).ID == "ddlCustomerType")
                    {
                        commonActions = ActionsEnum.CUSTOMERTYPECHANGING;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                switch (commonActions)
                {
                    #region DTL SEARCH/CUSTOMERCHANGE
                    case ActionsEnum.DTLSEARCH:
                    case ActionsEnum.CUSTOMERCHANGE:
                        //uclSOPaging.CurrentPage = 1;
                        SOInvoiceHeaderSession = null;
                        PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.PEDINGSOLIST);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        hdfIsPendingSOVisible.Value = "1";
                        GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtSONumber.Text = string.Empty;
                        hdfSOPK.Value = string.Empty;
                        //uclSOPaging.CurrentPage = 1;
                        PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.PEDINGSOLIST);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        hdfIsPendingSOVisible.Value = "1";
                        break;
                    #endregion
                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        SoHeaderObj = new DirectSOHeaderBO();
                        SoHeaderObj.SOList = new List<DirectSOHeaderListBO>();
                        List<DirectSOHeaderListBO> objItemList = new List<DirectSOHeaderListBO>();
                        DirectSOHeaderListBO objSoList;
                        HiddenField hdfSCPK;
                        HiddenField hdfSCCustomer;
                        HiddenField hdfSCType;
                        HiddenField hdfSCCurrency;
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.SOMappingDetails != null)
                        {
                            SOInvoiceHeaderSession.SOMappingDetails.ForEach(dtl =>
                            {
                                objSoList = new DirectSOHeaderListBO();
                                objSoList.SOH_PK = dtl.ICM_SO_HDR;
                                objSoList.SOH_CUSTOMER = Convert.ToInt32(SOInvoiceHeaderSession.ICH_CUSTOMER);
                                objSoList.SOH_TYPE = Convert.ToInt32(SOInvoiceHeaderSession.ICH_TYPE);
                                objSoList.SOH_CURRENCY = SOInvoiceHeaderSession.ICH_CURRENCY;
                                objItemList.Add(objSoList);
                            });
                        }
                        foreach (GridViewRow grdrow in grdSoList.Rows)
                        {
                            CheckBox chkSCselect = (CheckBox)grdrow.FindControl("chkSoSelect");
                            if (chkSCselect.Checked)
                            {
                                hdfSCPK = (HiddenField)grdrow.FindControl("hdfSCPK");
                                hdfSCCustomer = (HiddenField)grdrow.FindControl("hdfSCCustomerPK");
                                hdfSCType = (HiddenField)grdrow.FindControl("hdfSCType");
                                hdfSCCurrency = (HiddenField)grdrow.FindControl("hdfSCCurrency");
                                objSoList = new DirectSOHeaderListBO();
                                objSoList.SOH_PK = Convert.ToInt32(hdfSCPK.Value);
                                objSoList.SOH_CUSTOMER = Convert.ToInt32(hdfSCCustomer.Value);
                                objSoList.SOH_TYPE = Convert.ToInt32(hdfSCType.Value);
                                objSoList.SOH_CURRENCY = Convert.ToInt32(hdfSCCurrency.Value);
                                if (objItemList != null && (objItemList.Where(r => r.SOH_CUSTOMER != Convert.ToInt32(hdfSCCustomer.Value)).Count() > 0
                                                            || objItemList.Where(r => r.SOH_TYPE != Convert.ToInt32(hdfSCType.Value)).Count() > 0
                                                            || objItemList.Where(r => r.SOH_CURRENCY != Convert.ToInt32(hdfSCCurrency.Value)).Count() > 0
                                                            )
                                    )
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_SC").ToString()) + "');", true);
                                    return;
                                }
                                if (objItemList != null && objItemList.Where(r => r.SOH_PK == Convert.ToInt32(hdfSCPK.Value)).Count() <= 0)
                                    objItemList.Add(objSoList);
                            }
                        }
                        if (objItemList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordsSelected").ToString()) + "');", true);
                            return;
                        }
                        SoHeaderObj.SOList = objItemList;
                        GetFieldValues(ControlsEnum.INVOICETYPE);
                        SetFieldValues(ControlsEnum.INVOICETYPE);

                        GetFieldValues(ControlsEnum.SOINVHEADER);
                        SetFieldValues(ControlsEnum.SOINVHEADER);
                        SetFieldValues(ControlsEnum.SOINVOICELIST);
                        SetBranchIDEnableDisable();
                        if (SoHeaderObj.SOList[0].SOH_TYPE == Convert.ToByte(SalesInvoiceType.Export))
                        {
                            ddlInvoiceType.SelectedValue = Convert.ToByte(SalesInvoiceType.Proforma).ToString();
                        }
                        else
                        {
                            ddlInvoiceType.SelectedValue = SoHeaderObj.SOList[0].SOH_TYPE.ToString();
                            if (!IsAdvInvHasTax)
                                ddlInvoiceType.SelectedValue = Convert.ToByte(SalesInvoiceType.Proforma).ToString();
                        }
                        #region Fill workflow details with invoice type
                        if (CurrPK <= 0)//!IsAdvInvHasTax && 
                        {
                            if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
                            {
                                if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(3);
                                else
                                    FillProcessID(1);
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.NEWMODE && ucrWrkf.HasPageTaskPermission)
                                    ucrWrkf.ViewType = 1;
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                }
                            }
                        }
                        #endregion
                        hdfIsPendingSOVisible.Value = "0";
                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            bool cont = false;

                            if (ddlCustomerType.Items.Count <= 0)
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                return;
                            }
                            if (grdSalesList.Rows.Count <= 0)
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_ErrSave_Invoice").ToString()) + "');", true);
                                return;
                            }


                            foreach (GridViewRow grdPOrow in grdSalesList.Rows)
                            {
                                Label lblTotalAmountSO = (Label)grdPOrow.FindControl("lblTotalAmount");
                                Label lblInvoicedSO = (Label)grdPOrow.FindControl("lblInvoiced");
                                TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");
                                Label lblPInvoicedSO = (Label)grdPOrow.FindControl("lblProformaInvoiced");

                                TextBox txtOthercharges = (TextBox)grdPOrow.FindControl("txtOthercharges");
                                Label lblOtherAmount = (Label)grdPOrow.FindControl("lblOtherAmount");
                                HiddenField hdfOtherchargeOLD = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");
                                HiddenField hdfPayNow = (HiddenField)grdPOrow.FindControl("hdfPayNow");
                                if (hdfOtherchargeOLD.Value == "") { hdfOtherchargeOLD.Value = "0"; }
                                if (hdfPayNow.Value == "") { hdfPayNow.Value = "0"; }
                                if (lblTotalAmountSO.Text == "") { lblTotalAmountSO.Text = "0"; }
                                if (lblInvoicedSO.Text == "") { lblInvoicedSO.Text = "0"; }
                                if (txtPayNow.Text == "") { txtPayNow.Text = "0"; }
                                if (txtOthercharges.Text == "") { txtOthercharges.Text = "0"; }

                                if (ItemStatus == 0)
                                {
                                    if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic))
                                    {
                                        if (Convert.ToDecimal(lblTotalAmountSO.Text) >= Convert.ToDecimal(lblInvoicedSO.Text) + Convert.ToDecimal(txtPayNow.Text))
                                        {
                                            if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherchargeOLD.Value))
                                            {
                                                if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherchargeOLD.Value))) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                {
                                                    cont = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lblPInvoicedSO.Text))
                                            if (Convert.ToDecimal(txtPayNow.Text) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblPInvoicedSO.Text)))
                                            {
                                                cont = true;
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_5").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic))
                                    {
                                        if (Convert.ToDecimal(lblTotalAmountSO.Text) >= (Convert.ToDecimal(lblInvoicedSO.Text) - Convert.ToDecimal(hdfPayNow.Value)) + Convert.ToDecimal(txtPayNow.Text))
                                        {
                                            if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherchargeOLD.Value))
                                            {
                                                if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text) - Convert.ToDecimal(hdfPayNow.Value))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherchargeOLD.Value))) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                {
                                                    cont = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lblPInvoicedSO.Text))
                                            if (Convert.ToDecimal(txtPayNow.Text) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblPInvoicedSO.Text)))
                                            {
                                                cont = true;
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_4").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                    }
                                }
                            }
                            if (cont == true)
                            {
                                if (hdfIsJournalize.Value == "True")
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Journalize;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    if (grdSalesList.Rows.Count > 0)
                                    {
                                        ///////
                                        byte invoiceType = Convert.ToByte(ddlInvoiceType.SelectedValue);
                                        bool isValidEntry = true;
                                        foreach (GridViewRow grdRow in grdSalesList.Rows)
                                        {
                                            Label lblTotal = (Label)grdRow.FindControl("lblTotalAmount");
                                            TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNow");
                                            HiddenField hdfInvoiced = (HiddenField)grdRow.FindControl("hdfInvoiced");
                                            if (lblTotal != null && !string.IsNullOrEmpty(lblTotal.Text)
                                                && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text)
                                                && hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                            {
                                                if (invoiceType != (byte)SalesInvoiceType.Proforma)
                                                {
                                                    if (hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                                    {
                                                        if (Convert.ToDecimal(txtPayNow.Text.Trim().Replace(",", "")) > Convert.ToDecimal(lblTotal.Text.Trim().Replace(",", "")) - Convert.ToDecimal(Math.Round(decimal.Parse(hdfInvoiced.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)))
                                                        {
                                                            isValidEntry = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        ///////
                                        if (isValidEntry)
                                        {
                                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                            lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                                            if (NetAmount == TotalAmount)
                                            {
                                                invoiceHeaderObj = new DirectSOInvoiceHeader();
                                                invoiceHeaderObj = (DirectSOInvoiceHeader)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);
                                                invoiceHeaderObj.WKF_FLAG = 0;
                                                invoiceHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                                string xmlDoc = CommonFunctions.XmlSerialize<DirectSOInvoiceHeader>(invoiceHeaderObj);
                                                // save Process Control inspection details
                                                string invNumber = string.Empty;
                                                result = BusinessLogic.Sales.SalesInvoiceBL.SaveDirectSalesInvoiceHeader(xmlDoc, out invNumber);
                                                if (result > 0)
                                                {
                                                    if (string.IsNullOrEmpty(lblDispInvoiceNo.Text.Trim())
                                                        || lblDispInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                                    {
                                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                                    }
                                                    else
                                                    {
                                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                        object[] args = new object[2];
                                                        args[0] = Resources.PageNameRes.AdvanceInvoice;
                                                        args[1] = lblDispInvoiceNo.Text.Trim();
                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                                    }

                                                    //litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                    //litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Invoice);
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                    EntryStatus = EntryStatus.LISTMODE;
                                                    ResetForm();
                                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                                    btnNew.Focus();
                                                }
                                                else
                                                {
                                                    if (result == (int)DbSaveStatus.SQLERROR)
                                                    {
                                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                            + "','" + Resources.ErpRes.Information + "');", true);

                                                    }
                                                    else if (result == (int)DbSaveStatus.REFERRED)//need checking for receipt created or not
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReceiptCreated").ToString()) + "','" + Resources.Messages.Information + "');", true);

                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid1").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.SOINVHEADER);
                        EntryStatus = EntryStatus.NEWMODE;
                        FillProcessID(1);
                        TotalPages = 0;
                        //uclSOPaging.CurrentPage = 1;
                        hdfIsPendingSOVisible.Value = "0";
                        PageIndexSO = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.INVOICETYPE);
                        SetFieldValues(ControlsEnum.INVOICETYPE);
                        GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        if (IsAdvInvHasTax)                       
                            ddlInvoiceType.Enabled = true;                      
                        else                       
                            ddlInvoiceType.Enabled = false;                       
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #endregion
                    #region EDIT/VIEW/INVOICEDETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.INVOICEDETAIL:
                        bIsChecked = false;
                        gridRow = null;
                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                gridRow = grdrow;
                                break;
                            }
                        }
                        if (bIsChecked && gridRow != null)
                        {
                            ResetForm(ControlsEnum.SOINVHEADER);
                            CurrPK = Convert.ToInt32(((HiddenField)gridRow.FindControl("hdfInvoiceID")).Value);
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)gridRow.FindControl("hdfCustomerPK")).Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)gridRow.FindControl("lblCustomer")).Text;
                            Approved = Convert.ToInt32(((HiddenField)gridRow.FindControl("hdfApproved")).Value);
                            HiddenField hdfPosted = (HiddenField)gridRow.FindControl("hdfPosted");
                            invType = Convert.ToInt32(((HiddenField)gridRow.FindControl("hdfInvType")).Value);
                            if (hdfPosted != null)
                                Posted = Convert.ToBoolean(hdfPosted.Value);

                            HiddenField hdfStatus = (HiddenField)gridRow.FindControl("hdfStatus");
                            ItemStatus = Convert.ToInt16(hdfStatus.Value);
                            RPTTYPE = invType;
                            IsDeleted = Convert.ToInt32(((HiddenField)gridRow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                            if (IsDeleted)
                            {
                                btnSave.Visible = false;
                                hdfInvDelStatus.Value = "1";
                            }
                            else
                            {
                                btnSave.Visible = true;
                                hdfInvDelStatus.Value = "0";
                            }

                            hdfDept = gridRow.FindControl("hdfDept") as HiddenField;
                            if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }

                            if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(3);
                            else
                                FillProcessID(1);

                            WorkflowCore.CoreService workflowCoreObj = new WorkflowCore.CoreService();
                            base.WkfRefID = workflowCoreObj.GetRefID(CurrPK, PageProcessID);

                            SetUIEditView(commonActions);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;

                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVOICELIST);

                            GetFieldValues(ControlsEnum.PEDINGSOLIST);
                            SetFieldValues(ControlsEnum.PEDINGSOLIST);
                           
                            //hdfIVHPK.Value = CurrPK.ToString();
                                           
                            //SetFieldValues(ControlsEnum.SOINVOICELIST); 
                            hdfIsPendingSOVisible.Value = "0";
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region Remove
                    case ActionsEnum.REMOVE:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        gridRow = ((Button)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfScPk = (HiddenField)gridRow.FindControl("hdfSONumber");
                        int SOPk = 0;
                        int.TryParse(hdfScPk.Value, out SOPk);
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.SOMappingDetails != null)
                        {
                            DirectSOInvoiceMappingDetails objMappingDtl = SOInvoiceHeaderSession.SOMappingDetails.SingleOrDefault(r => r.ICM_SO_HDR == SOPk);
                            SOInvoiceHeaderSession.SOMappingDetails.Remove(objMappingDtl);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVOICELIST);
                            SetBranchIDEnableDisable();
                            #region Fill workflow details with invoice type
                            if (!IsAdvInvHasTax && CurrPK <= 0)
                            {
                                if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
                                {
                                    if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                        FillProcessID(3);
                                    else
                                        FillProcessID(1);
                                    SetCancelRef(CurrPK);
                                    ucrWrkf.FillWorkFlowDetails();
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.NEWMODE && ucrWrkf.HasPageTaskPermission)
                                        ucrWrkf.ViewType = 1;
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                    }
                                }
                            }
                            #endregion
                        }

                        break;
                    #endregion                                 
                    #region Delete
                    case ActionsEnum.DELETE:
                        result = 0;
                        result = BusinessLogic.Sales.SalesInvoiceBL.DeleteDirectSalesInvoiceDetails(currentUser.PKUser.ToString(), CurrPK, LastModifiedTime, null, ApplicationType.DSI);
                        if (result > 0) // Success ! re-initialize the page
                        {
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.AdvanceInvoice);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm();
                            GetFieldValues(ControlsEnum.INVOICELIST);
                            SetFieldValues(ControlsEnum.INVOICELIST);
                            btnNew.Focus();
                        }
                        else
                        {
                            if (result == (int)DbDeleteStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AdvanceInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.INVOICELIST);
                                SetFieldValues(ControlsEnum.INVOICELIST);
                            }
                            else if (result == (int)DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AdvanceInvoice + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AdvanceInvoice + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.INVOICELIST);
                                SetFieldValues(ControlsEnum.INVOICELIST);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AdvanceInvoice);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion                    
                    #region Invoice List
                    case ActionsEnum.INVOICELIST:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        if (grdSalesList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                            if (NetAmount == TotalAmount)
                            {
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                //Show WorkFlow Popup                       
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        if (grdSalesList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                            if (NetAmount == TotalAmount)
                            {
                                //Show WorkFlow Popup                       
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {

                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (hdfIsJournalize.Value == "True")
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Journalize;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    bool cont = false;
                                    if (ddlCustomerType.Items.Count <= 0)
                                    {

                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                        return;
                                    }
                                    foreach (GridViewRow grdPOrow in grdSalesList.Rows)
                                    {
                                        Label lblTotalAmountSO = (Label)grdPOrow.FindControl("lblTotalAmount");
                                        Label lblInvoicedSO = (Label)grdPOrow.FindControl("lblInvoiced");
                                        Label lblPInvoicedSO = (Label)grdPOrow.FindControl("lblProformaInvoiced");
                                        TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");

                                        TextBox txtOthercharges = (TextBox)grdPOrow.FindControl("txtOthercharges");
                                        Label lblOtherAmount = (Label)grdPOrow.FindControl("lblOtherAmount");
                                        HiddenField hdfOtherchargeOLD = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");
                                        if (hdfOtherchargeOLD.Value == "") { hdfOtherchargeOLD.Value = "0"; }
                                        if (lblTotalAmountSO.Text == "") { lblTotalAmountSO.Text = "0"; }
                                        if (lblInvoicedSO.Text == "") { lblInvoicedSO.Text = "0"; }
                                        if (txtPayNow.Text == "") { txtPayNow.Text = "0"; }
                                        if (txtOthercharges.Text == "") { txtOthercharges.Text = "0"; }

                                        if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic))
                                        {
                                            if (Convert.ToDecimal(lblTotalAmountSO.Text) >= Convert.ToDecimal(lblInvoicedSO.Text) + Convert.ToDecimal(txtPayNow.Text))
                                            {
                                                if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherchargeOLD.Value))
                                                {
                                                    if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherchargeOLD.Value))) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                    {
                                                        cont = true;
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                     "ClosePopup();", true);
                                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                        break;
                                                    }

                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                     "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                     "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lblPInvoicedSO.Text))
                                                if (Convert.ToDecimal(txtPayNow.Text) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblPInvoicedSO.Text)))
                                                {
                                                    cont = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                      "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_5").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                        }

                                    }
                                    if (cont == true)
                                    {
                                        if (grdSalesList.Rows.Count > 0)
                                        {
                                            byte invoiceType = Convert.ToByte(ddlInvoiceType.SelectedValue);
                                            bool isValidEntry = true;
                                            foreach (GridViewRow grdRow in grdSalesList.Rows)
                                            {
                                                Label lblTotal = (Label)grdRow.FindControl("lblTotalAmount");
                                                TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNow");
                                                HiddenField hdfInvoiced = (HiddenField)grdRow.FindControl("hdfInvoiced");
                                                if (lblTotal != null && !string.IsNullOrEmpty(lblTotal.Text)
                                                    && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text)
                                                    && hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                                {
                                                    if (invoiceType != (byte)SalesInvoiceType.Proforma)
                                                    {
                                                        if (hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                                        {
                                                            if (Convert.ToDecimal(txtPayNow.Text.Trim().Replace(",", "")) > Convert.ToDecimal(lblTotal.Text.Trim().Replace(",", "")) - Convert.ToDecimal(Math.Round(decimal.Parse(hdfInvoiced.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)))
                                                            {
                                                                isValidEntry = false;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                            ///////
                                            if (isValidEntry)
                                            {
                                                NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                                TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                                lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                                                //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                                lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                                                if (NetAmount == TotalAmount)
                                                {
                                                    invoiceHeaderObj = new DirectSOInvoiceHeader();
                                                    invoiceHeaderObj = (DirectSOInvoiceHeader)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);
                                                    invoiceHeaderObj.WKF_FLAG = 1;
                                                    if (hdfExchangeRate.Value != "-1")
                                                    {
                                                        if (invoiceHeaderObj != null)
                                                        {
                                                            invoiceHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                                            SaveTransaction(invoiceHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));

                                                        }
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                        "ClosePopup();", true);

                                                        litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid1").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.DSI))
                                {
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                    btnNew.Focus();
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region PRINT/PRINTLISTING
                    case ActionsEnum.PRINT:                        
                        if (CurrPK > 0)
                        {
                            if (ddlInvoiceType.SelectedValue != null && Convert.ToInt32(ddlInvoiceType.SelectedIndex) > 0)
                            {
                                RPTTYPE = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
                            }
                            if (RPTTYPE == 2 || RPTTYPE == 3)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSIJ +
                                       "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.DSIJ + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                    CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=11") + "');", true);
                            }
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString()
                            // + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.PRINTLISTING:
                        if (CurrPK == 0)
                        {
                            foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                            {
                                chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                                if (chkInvselect.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);

                                    if (bIsChecked)
                                    {
                                        HiddenField hdfInvType = (HiddenField)grdrow.FindControl("hdfInvType");

                                        if (hdfInvType.Value == "2" || hdfInvType.Value == "3")
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DSIJ +
                                                   "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.DSIJ + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=11") + "');", true);
                                        }
                                        CurrPK = 0;
                                    }
                                    return;
                                }
                            }

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        }
                        break;
                    #endregion                    
                    #region Reset
                    case ActionsEnum.RESET:
                        Currency = 0;
                        CustomerID = 0;
                        InvType = 0;                       
                        hdfSelectedItemPk.Value = "0";
                        break;
                    #endregion
                    #region SALESINVOICETYPECHANGED
                    case ActionsEnum.SALESINVOICETYPECHANGED:
                        Label lblTotalAmount;
                        lblTotalAmount = null;
                        Label lblInvoiced;
                        lblInvoiced = null;
                        Label lblProformaInvoiced;
                        lblProformaInvoiced = null;
                        Label lblBalancetoInvoice;
                        lblBalancetoInvoice = null;
                        TextBox txtPayNow1;
                        txtPayNow1 = null;
                        if ((!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue) 
                            && ddlInvoiceType.SelectedValue == CommonConstants.SELECTVAL)
                            || SOInvoiceHeaderSession == null
                            || SOInvoiceHeaderSession.SOMappingDetails == null
                            || SOInvoiceHeaderSession.SOMappingDetails.Count == 0)
                        {
                            return;
                        }

                        if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Proforma))
                        {
                            if (!IsAdvInvHasTax)
                                ischanged = true;

                            grdSalesList.HeaderRow.Cells[9].Visible = true;
                            grdSalesList.FooterRow.Cells[9].Visible = true;
                            foreach (GridViewRow grdRow in grdSalesList.Rows)
                            {
                                grdRow.Cells[9].Visible = true;
                                lblTotalAmount = grdRow.FindControl("lblTotalAmount") as Label;
                                lblInvoiced = grdRow.FindControl("lblInvoiced") as Label;
                                lblProformaInvoiced = grdRow.FindControl("lblProformaInvoiced") as Label;
                                lblBalancetoInvoice = grdRow.FindControl("lblBalancetoInvoice") as Label;
                                txtPayNow1 = grdRow.FindControl("txtPayNow") as TextBox;
                                if (lblTotalAmount != null && lblInvoiced != null && lblProformaInvoiced != null && lblBalancetoInvoice != null && txtPayNow1 != null)
                                {
                                    double total = string.IsNullOrEmpty(lblTotalAmount.Text.Trim()) ? 0 : Convert.ToDouble(lblTotalAmount.Text.Trim().Replace(",", ""));
                                    double invoiced = string.IsNullOrEmpty(lblInvoiced.Text.Trim()) ? 0 : Convert.ToDouble(lblInvoiced.Text.Trim().Replace(",", ""));
                                    double pInvoiced = string.IsNullOrEmpty(lblProformaInvoiced.Text.Trim()) ? 0 : Convert.ToDouble(lblProformaInvoiced.Text.Trim().Replace(",", ""));
                                    double maxAllowedPInvoiced = invoiced > pInvoiced ? invoiced : pInvoiced;
                                    txtPayNow1.Text = GetFormattedCurrency(((total - maxAllowedPInvoiced) >= 0 ? (total - maxAllowedPInvoiced) : 0).ToString());
                                    lblBalancetoInvoice.Text = lblBalancetoInvoice.ToolTip = String.Format("{0:c}", ((total - maxAllowedPInvoiced) >= 0 ? (total - maxAllowedPInvoiced) : 0));
                                }
                            }                          
                            SetFieldValues(ControlsEnum.SOINVOICELIST);
                        }
                        else
                        {
                            ischanged = true;
                            grdSalesList.HeaderRow.Cells[9].Visible = false;
                            grdSalesList.FooterRow.Cells[9].Visible = false;
                            foreach (GridViewRow grdRow in grdSalesList.Rows)
                            {
                                grdRow.Cells[9].Visible = false;
                                lblTotalAmount = grdRow.FindControl("lblTotalAmount") as Label;
                                lblInvoiced = grdRow.FindControl("lblInvoiced") as Label;
                                lblBalancetoInvoice = grdRow.FindControl("lblBalancetoInvoice") as Label;
                                txtPayNow1 = grdRow.FindControl("txtPayNow") as TextBox;
                                if (lblTotalAmount != null && lblInvoiced != null && lblBalancetoInvoice != null && txtPayNow1 != null)
                                {
                                    double total = string.IsNullOrEmpty(lblTotalAmount.Text.Trim()) ? 0 : Convert.ToDouble(lblTotalAmount.Text.Trim().Replace(",", ""));
                                    double invoiced = string.IsNullOrEmpty(lblInvoiced.Text.Trim()) ? 0 : Convert.ToDouble(lblInvoiced.Text.Trim().Replace(",", ""));
                                    txtPayNow1.Text = GetFormattedCurrency(((total - invoiced) >= 0 ? total - invoiced : 0).ToString());
                                    lblBalancetoInvoice.Text = lblBalancetoInvoice.ToolTip = String.Format("{0:c}", ((total - invoiced) >= 0 ? total - invoiced : 0));
                                }
                            }                          
                            SetFieldValues(ControlsEnum.SOINVOICELIST);
                        }

                        #region Change workflow details with invoice type
                        if (CurrPK <= 0)//!IsAdvInvHasTax &&
                        {
                            if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
                            {
                                if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(3);
                                else
                                    FillProcessID(1);
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.NEWMODE && ucrWrkf.HasPageTaskPermission)
                                    ucrWrkf.ViewType = 1;
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                }
                            }
                        }
                        #endregion

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                if (!string.IsNullOrEmpty(hdfStatus.Value) && Convert.ToInt32(hdfStatus.Value) == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DraftedInv").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                RPTTYPE = invType;
                                IsDeleted = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                                if (IsDeleted)
                                {
                                    btnSave.Visible = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                }

                                if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(3);
                                else
                                    FillProcessID(1);

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;                          
                            hdfIVHPK.Value = CurrPK.ToString();                            
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVOICELIST);
                            GetFieldValues(ControlsEnum.PEDINGSOLIST);
                            SetFieldValues(ControlsEnum.PEDINGSOLIST);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region CUSTOMERTYPECHANGING
                    case ActionsEnum.CUSTOMERTYPECHANGING:
                        GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetBranchIDEnableDisable();
                        break;
                    #endregion
                    #region TAXSPLITUPPOPUP
                    case ActionsEnum.TAXDETAILSSPLITUP:
                        int SOH_Pk = int.Parse(((LinkButton)sender).CommandArgument.ToString());
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.SOMappingDetails != null)
                        {
                            DirectSOInvoiceMappingDetails objMapping = SOInvoiceHeaderSession.SOMappingDetails.SingleOrDefault(r => r.ICM_SO_HDR == SOH_Pk);
                            if (objMapping != null && objMapping.SOTaxList != null)
                                grdTaxSplitupDetails.DataSource = objMapping.SOTaxList.ToList();
                            else
                                grdTaxSplitupDetails.DataSource = null;
                            grdTaxSplitupDetails.DataBind();
                        }
                        else
                        {
                            grdTaxSplitupDetails.DataSource = null;
                            grdTaxSplitupDetails.DataBind();
                        }                        
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTaxSplitupDetails]','" + GetLocalResourceObject("TaxDetails").ToString() + "','400','200');", true);

                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.SOD + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region AMOUNTDETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoiceID");
                        long.TryParse(hdfInvoiceID.Value, out InvoicePk);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','200');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
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

        private void SaveTransaction(DirectSOInvoiceHeader invoiceHeaderObj, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (invoiceHeaderObj == null)
                invoiceHeaderObj = new DirectSOInvoiceHeader();
            #region Transaction Log and Application Code
            invoiceHeaderObj.ATL_APP_TYPE = ApplicationType.DSI;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            invoiceHeaderObj.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            invoiceHeaderObj.WKF_APPLICATION = CurrPK;
            invoiceHeaderObj.WKF_COMMENTS = wkfDetails.Comments;
            invoiceHeaderObj.WKF_TRX_FLAG = workflowFlag;
            invoiceHeaderObj.WKF_PROCESS = wkfDetails.ProcessID;
            invoiceHeaderObj.WKF_REFERENCE = wkfDetails.ReferenceID;
            invoiceHeaderObj.WKF_TASK = wkfDetails.TaskID;
            invoiceHeaderObj.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                invoiceHeaderObj.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                invoiceHeaderObj.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<DirectSOInvoiceHeader>(invoiceHeaderObj);
            string invoiceNumber = string.Empty;
            result = BusinessLogic.Sales.SalesInvoiceBL.SaveDirectSalesInvoiceHeader(xmlDoc, out invoiceNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                if (isCancelled)
                {
                    FillProcessID(1);
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                WrkfComments.Text = "";
                if (string.IsNullOrEmpty(invoiceNumber))
                    invoiceNumber = lblDispInvoiceNo.Text.Trim();
                object[] args = new object[2];
                args[0] = Resources.PageNameRes.AdvanceInvoice;
                args[1] = invoiceNumber;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AdvanceInvoice);
                #region Inbox or Listing Page Redirection
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.INVOICELIST);
                    SetFieldValues(ControlsEnum.INVOICELIST);
                    btnNew.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                }
                #endregion

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
                    litErrorMsg.Text = Resources.PageNameRes.AdvanceInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AdvanceInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AdvanceInvoice);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
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
                GetFieldValues(ControlsEnum.INVOICELIST);
                SetFieldValues(ControlsEnum.INVOICELIST);
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
                #region Grid Data Row
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    #region grdSoList
                    if (((GridView)sender).ID == "grdSoList")
                    {
                        HiddenField hdfSCPK = e.Row.FindControl("hdfSCPK") as HiddenField;
                        CheckBox chkSoSelect = e.Row.FindControl("chkSoSelect") as CheckBox;
                        if (SOInvoiceHeaderSession != null && SOInvoiceHeaderSession.SOMappingDetails.Where(r => r.ICM_SO_HDR == Convert.ToInt32(hdfSCPK.Value)).Count() > 0)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());
                            chkSoSelect.Checked = true;
                            chkSoSelect.Enabled = false;
                        }
                    } 
                    #endregion
                }
                #endregion
                #region Grid Header Row
                if (e.Row.RowType == DataControlRowType.Header)
                {

                }
                #endregion
                #region Grid Footer Row
                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
                #endregion              
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
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
           
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
          
            btnDeleteOK.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            
            btnListPrint.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
           
            btnDeleteOK.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);
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
                GetFieldValues(ControlsEnum.INVOICELIST);
                SetFieldValues(ControlsEnum.INVOICELIST);
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
                hdfInvCategory.Value = Convert.ToByte((byte)SalesInvoiceCategory.Advanced).ToString();
                if (grdSalesList.Rows.Count > 0 || CurrPK > 0)
                    txtCustomer.Enabled = false;
                else
                    txtCustomer.Enabled = true;
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
                if (IsDeleted)
                {
                    btnSave.Visible = false;
                    btnDeleteNew.Visible = false;
                    hdfInvDelStatus.Value = "1";
                    //btnSaveSubmit.Visible = false;
                    //btnSubmit.Visible = false;
                }
                else
                    hdfInvDelStatus.Value = "0";
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveLink", "$(document).ready(function () { RemoveBalAmntHyperLink();});", true);

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
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CURRENCY");
            if (dt != null && dt.Rows.Count > 0)
            {
                IsBizUnitCur = dt.Rows[0]["ACF_VALUE"].ToString() == "1" ? false : true;
            }

            #region Advance Invoice Tax Settings
            //DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            //if (dtTax != null && dtTax.Rows.Count > 0)
            //{
            //    IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
            //}
            dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
            if (dtTaxSettings != null && dtTaxSettings.Rows.Count > 0)
            {
                DataRow drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                if (drTaxSettings != null)
                {
                    IsAdvInvHasTax = drTaxSettings["ACF_VALUE"].ToString() == "1" ? true : false;
                    hdfTaxSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                }
                drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "POSTING REQUIRED");
                if (drTaxSettings != null)
                {
                    hdfPostingSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                }
            }
            #endregion

            hdfIsTaxForOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales").ToString();
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            //if (Request.QueryString[QueryStrings.PID] == null)
            //{
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
            //}
            //else
            //{
            //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            //    else
            //        path = Request.Url.AbsolutePath.ToLower();
            //}

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
                        //base.WkfPageUrl = path;
                    }
                    base.WkfPageUrl = path;
                }
            }
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                //DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
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
            INVOICELIST,         
            SOINVOICELIST,           
            FINANCEINVOICEHDR,
            FINANCEINVOICETRXMPG,
            PICKFORRECEIPT,
            PICKFORCRDRNOTE,
            EXCHANGERATE,
            JOURNALIZE,
            WRKFSUBMIT,
            INVOICETYPE,
            SALES_INVOICE_TYPE,
            COMPANY,
            COMPANYNAME,
            SOTYPE,
            CUSTOMERTYPES,
            GETCUSTOMERDETAILSBYTYPE,
            TAXSPLITUP,
            AMOUNTDETAILS,
            RESETRECEIPT,
            PEDINGSOLIST,
            SOINVHEADER,
            UPDATEGRIDVALTOOBJECT


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


        #region CustomerContactTypes
        public enum CustomerContactTypeEnum
        {
            HeadOffice = 4,
            Branch = 5
        }
        #endregion        

    }
}