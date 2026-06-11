using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.POInvoicing;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using BusinessObject.AlertManagement;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.Finance;
using System.IO;
using ERP.Store.UI;

namespace ERPSMS_v01.Finance
{
    public partial class ClosingStock : WorkFlowBasePage 
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
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
        /// Current PK
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
        /// Process ID of the Page
        /// </summary>
        private int WkfStatus
        {
            get
            {
                return this.ViewState["WkfStatus"] == null ? Convert.ToByte(0) : Convert.ToByte(this.ViewState["WkfStatus"]);
            }
            set
            {
                this.ViewState["WkfStatus"] = value;
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
        #region Variables
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private StockHeader StockHeaderObj;
        AdmCompanyMstService admCompanyMstServiceClient;
        TextBox WrkfComments;
        DropDownList ddlWkfAction;
        private string action;
        private string refID;
        private string inboxFlag;
        private DataSet dsPageData;
        private DataTable dtClosingStockList;
        private DataTable dtClosingStockHistory;
        private DataTable dtConfigData;
        private StockHeader objStockHdr;
        private List<StockItems> objStockItemsList;
        private DataTable dtPageData;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private int ItemCategoryPk;
        private int JournalPK;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2) 
	#endregion
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
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    hdfJournalizeWorkFlow.Value = "0";
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                }
                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    txtStockDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.EXCHANGERATE);

                    txtMonth.Text = string.Empty;
                    txtStockNo.Text = string.Empty;
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfJournalizeWorkFlow.Value = "0";
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
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }


                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();


                    FillProcessID(1);
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
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
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;                        
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKGET);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKGET);
                    }
                    else
                    {
                        #region else Region
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
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                                if (pid.Equals("11"))
                                    hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETJOURNALBYPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                }
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
                            GetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                            SetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                        }
                        else
                        {
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = "LSH_PK";
                            grdClosingStock.DataKeyNames = datakeyarray;
                            GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                            SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        #endregion
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

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsPurStkVisible.Value = GetGlobalResourceObject("ConfigurationsRes", "IsClosingStockAmtsVisible").ToString();
            GetFieldValues(ControlsEnum.INVENTTYPECONFIG);
            SetFieldValues(ControlsEnum.INVENTTYPECONFIG);
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
            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region  CLOSINGSTOCKLIST
                    case ControlsEnum.CLOSINGSTOCKLIST:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
                        serviceUtilityObj.TotalRecords = 0;
                        DateTime? searchdate = null;
                        if (!string.IsNullOrEmpty(txtMonth.Text))
                        {
                            var today = DateTime.Parse(txtMonth.Text.ToString());
                            var CurrentMonth = new DateTime(today.Year, today.Month, 1);
                            searchdate = CurrentMonth.AddMonths(1).AddDays(-1);
                        }
                        dtClosingStockList = BusinessLogic.Finance.ClosingStockBL.GetClosingStockList(serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize, searchdate, txtStockNo.Text, Convert.ToInt32(ddlStatus.SelectedValue), currentUser);
                        serviceUtilityObj.TotalRecords = dtClosingStockList.Rows.Count > 0 ? Convert.ToInt32(dtClosingStockList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region  CLOSING STOCK GET
                    case ControlsEnum.CLOSINGSTOCKGET:
                        int GClosingStkPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 1;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
                        serviceUtilityObj.TotalRecords = 0;
                        DateTime? srchdate = null;
                        //if (!string.IsNullOrEmpty(txtMonth.Text))
                        //{
                        //    var today = DateTime.Parse(txtMonth.Text.ToString());
                        //    var CurrentMonth = new DateTime(today.Year, today.Month, 1);
                        //    searchdate = CurrentMonth.AddMonths(1).AddDays(-1);
                        //}
                        dtClosingStockList = BusinessLogic.Finance.ClosingStockBL.GetClosingStockList(serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize, srchdate, txtStockNo.Text, 3, currentUser, GClosingStkPk);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region  CLOSINGSTOCKDETAILS
                    case ControlsEnum.CLOSINGSTOCKDETAILS:
                        objStockItemsList = new List<StockItems>();
                        objStockHdr = null;
                        objStockHdr = BusinessLogic.Finance.ClosingStockBL.GetStockItemsList(DateTime.Parse(txtAsOnDate.Text), currentUser.SBUID, 0);
                        objStockItemsList = objStockHdr.StockItemList.ToList();
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        DateTime ClosingStockDate = DateTime.Now;
                        DateTime.TryParse(txtStockDate.Text, out ClosingStockDate);
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(currentUser.BaseCurrency, currentUser.BaseCurrency, ClosingStockDate);
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) > 0)
                            {
                                hdfExchangeRate.Value = Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]).ToString();
                            }
                        }
                        break;
                    #endregion
                    #region  CLOSINGSTOCKHDR
                    case ControlsEnum.CLOSINGSTOCKHDR:
                        objStockItemsList = new List<StockItems>();
                        objStockHdr = null;
                        objStockHdr = BusinessLogic.Finance.ClosingStockBL.GetClosingStockByPk(CurrPK, currentUser.SBUID, (byte)DbActiveStatus.HASPK);
                        break;
                    #endregion
                    #region  CLOSINGSTOCKHISTORY
                    case ControlsEnum.CLOSINGSTOCKHISTORY:
                        dtClosingStockHistory = new DataTable();
                        dtClosingStockHistory = BusinessLogic.Finance.ClosingStockBL.GetClosingStockHistory(ItemCategoryPk);
                        break;
                    #endregion
                    #region  INVENTORY TYPE CONFIGUARATION
                    case ControlsEnum.INVENTTYPECONFIG:
                        dtConfigData = new DataTable();
                        dtConfigData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("INVENTORY SYSTEM TYPE", string.Empty, currentUser.SBUID);
                        break;
                    #endregion
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
                    #region GET JOURNAL BY PK
                    case ControlsEnum.GETJOURNALBYPK:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JournalPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
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
                finTrxServiceClient = null;
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
                    #region CLOSING STOCK LIST
                    case ControlsEnum.CLOSINGSTOCKLIST:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region CLOSINGS TOCK DETAILS
                    case ControlsEnum.CLOSINGSTOCKDETAILS:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region CLOSING STOCK HDR
                    case ControlsEnum.CLOSINGSTOCKHDR:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region CLOSING STOCK HISTORY
                    case ControlsEnum.CLOSINGSTOCKHISTORY:
                        BindGrid(controlType);
                        break;
                    #endregion
                    #region INVENT TYPE CONFIG
                    case ControlsEnum.INVENTTYPECONFIG:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region CLOSINGSTOCKGET
                    case ControlsEnum.CLOSINGSTOCKGET:
                        GetUIValuesFromObject(ControlsEnum.CLOSINGSTOCKGET);
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
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            //string BlockedExtensions = "dll";
            //if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            //{
            //    BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            //}
            bool flag = true;
            //string[] extensionList = BlockedExtensions.Split(',');
            //for (int i = 0; i < extensionList.Length; i++)
            //    if (("." + extensionList[i]) == extension)
            //    {
            //        flag = false;
            //        break;
            //    }
            return flag;
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        //
        private int FillProcessId()
        {
            int procId = 0;
            string path;
            path = "/Finance/FCReverse.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
            }
            return procId;
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region CLOSINGSTOCKHDR
                    case ControlsEnum.CLOSINGSTOCKHDR:
                        StockHeaderObj.LSH_PK = CurrPK;
                        StockHeaderObj.LSH_NO = lblClosingStockNo.Text;
                        StockHeaderObj.LSH_DATE = DateTime.Parse(txtStockDate.Text.ToString()).ToString();
                        var today = DateTime.Parse(txtAsOnDate.Text.ToString());
                        var CurrentMonth = new DateTime(today.Year, today.Month, 1);
                        DateTime AsOnDate = CurrentMonth.AddMonths(1).AddDays(-1);
                        StockHeaderObj.LSH_AS_ON_DATE = AsOnDate.ToString();
                        StockHeaderObj.LSH_DESC = HttpUtility.HtmlEncode(txtRemarks.Text);
                        StockHeaderObj.LSH_BASE_CURR = currentUser.BaseCurrency;
                        StockHeaderObj.LSH_TRX_CURR = currentUser.BaseCurrency;
                        StockHeaderObj.LSH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                        StockHeaderObj.LSH_DEPT = currentUser.CurrentDeptPK;
                        StockHeaderObj.LSH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                        StockHeaderObj.LSH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        StockHeaderObj.LSH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        StockHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        StockHeaderObj.LAST_MOD_DT = LastModifiedTime;

                        StockHeaderObj.StockItemList = new List<StockItems>();
                        List<StockItems> detailsList = new List<StockItems>();
                        StockItems objDetail;
                        foreach (GridViewRow inRow in grdItemDetails.Rows)
                        {
                            if (!String.IsNullOrEmpty((inRow.FindControl("txtClosingNow") as TextBox).Text))
                            {
                                objDetail = new StockItems();
                                HiddenField hdfDetlPk = (HiddenField)inRow.FindControl("hdfDetlPk");
                                HiddenField hdfItemCatPk = (HiddenField)inRow.FindControl("hdfItemCatPk");
                                Label lblPurchaseAmount = (Label)inRow.FindControl("lblPurchaseAmount");
                                Label lblStockAmount = (Label)inRow.FindControl("lblStockAmount");
                                TextBox txtClosingNow = (TextBox)inRow.FindControl("txtClosingNow");
                                //if (Convert.ToDouble(txtClosingNow.Text) > 0)
                                //{
                                objDetail.LSD_PK = string.IsNullOrEmpty(hdfDetlPk.Value) ? 0 : Convert.ToInt32(hdfDetlPk.Value);
                                objDetail.LSD_ITEM_CAT = string.IsNullOrEmpty(hdfItemCatPk.Value) ? 0 : Convert.ToInt32(hdfItemCatPk.Value);
                                objDetail.LSD_PURCHASE = double.Parse(lblPurchaseAmount.Text);
                                objDetail.LSD_STOCK = double.Parse(lblStockAmount.Text);
                                objDetail.LSD_CLOSING = double.Parse(txtClosingNow.Text);
                                objDetail.LSD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                detailsList.Add(objDetail);
                                //}
                            }
                        }
                        StockHeaderObj.StockItemList = detailsList;
                        retObject = StockHeaderObj;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        if (Approved == 2)
                        {
                            GetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                            if (objStockHdr != null)
                            {
                                Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = null;

                                //Journalize New sessions start
                                Session[ERP.Utilities.SessionStrings.DrControls] = null;
                                Session[ERP.Utilities.SessionStrings.CrControls] = null;
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                                Session[ERP.Utilities.SessionStrings.AccountType] = null;
                                Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                                Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                                //Journalize New sessions End

                                //Invoice = ApplicationType.SIJ;
                                ucrJournalize.TransactionType = ApplicationType.CLSTJ;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = ApplicationType.CLSTJ;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = objStockHdr.LSH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = objStockHdr.LSH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = objStockHdr.LSH_TRX_CURR;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = objStockHdr.LSH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.CLSTJ;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                FillProcessID(2);
                                GetFieldValues(ControlsEnum.FINHEADER);
                                //EntryStatus = EntryStatus.ENTRYMODE;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                {
                                    ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                    base.WkfRefID = ucrWrkf.RefID;
                                }
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                {
                                    ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                    ucrWrkf.ViewType = 1;
                                    // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";
                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                hdfJournalizeWorkFlow.Value = "1";
                                ucrJournalize.CallUserControl();

                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Closing_Stk_Journal").ToString();

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_PickClosingStk_Msg").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                }
                return retObject;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region CLOSING STOCK HDR
                    case ControlsEnum.CLOSINGSTOCKHDR:
                        if (objStockHdr != null)
                        {
                            txtStockDate.Text = DateTime.Parse(objStockHdr.LSH_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtAsOnDate.Text = DateTime.Parse(objStockHdr.LSH_AS_ON_DATE).ToString(Resources.Constants.DateFormatMonthYear);
                            txtRemarks.Text = HttpUtility.HtmlDecode(objStockHdr.LSH_DESC);
                            hdfExchangeRate.Value = objStockHdr.LSH_EXCHG_RATE.ToString();
                            ddlCompany.SelectedValue = objStockHdr.LSH_COMPANY.ToString();
                            lblClosingStockNo.Text = string.IsNullOrEmpty(objStockHdr.LSH_NO) ? Resources.ErpRes.Draft : objStockHdr.LSH_NO;
                            if (CurrPK > 0)
                            {
                                LastModifiedTime = objStockHdr.LAST_MOD_DT;
                                lblLastModifiedHDR.Text = objStockHdr.LAST_MOD_DT.ToString();
                            }
                            Approved = Convert.ToInt32(objStockHdr.LSH_STATUS);
                            hdfIsJournalize.Value = objStockHdr.LSH_HAS_JRNL_ENTRY.ToString();
                            objStockItemsList = new List<StockItems>();
                            objStockItemsList = objStockHdr.StockItemList.ToList();
                            SetFieldValues(ControlsEnum.CLOSINGSTOCKDETAILS);
                        }
                        break;
                    #endregion
                    #region INVENT TYPE CONFIG
                    case ControlsEnum.INVENTTYPECONFIG:
                        if (dtConfigData != null && dtConfigData.Rows.Count > 0)
                        {
                            hdfInventTypeConfig.Value = dtConfigData.Rows[0]["ACF_VALUE"].ToString();
                        }
                        break;
                    #endregion
                    #region CLOSING STOCK GET
                    case ControlsEnum.CLOSINGSTOCKGET:
                        if (dtClosingStockList != null && dtClosingStockList.Rows.Count > 0)
                        {
                            hdfIsCancelled.Value = "0";
                            setvisibility(ActionsEnum.VIEW);
                            CurrPK = Convert.ToInt32(dtClosingStockList.Rows[0]["LSH_PK"]);
                            Approved = Convert.ToInt32(dtClosingStockList.Rows[0]["LSH_STATUS"]);
                            Posted = Convert.ToBoolean(Convert.ToInt32(dtClosingStockList.Rows[0]["LSH_HAS_JRNL_ENTRY"]));
                            hdfIsCancelled.Value = Convert.ToString(dtClosingStockList.Rows[0]["LSH_DEL_STATUS"]);
                            SetUIEditView(ActionsEnum.VIEW);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                            SetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);


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
                    #region CLOSING STOCK LIST
                    case ControlsEnum.CLOSINGSTOCKLIST:
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                        grdClosingStock.PageIndex = Convert.ToInt32(PageIndex);
                        grdClosingStock.DataSource = dtClosingStockList;
                        grdClosingStock.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion
                    #region CLOSINGSTOCK DETAILS
                    case ControlsEnum.CLOSINGSTOCKDETAILS:
                        grdItemDetails.DataSource = objStockItemsList;
                        grdItemDetails.DataBind();
                        if (hdfIsPurStkVisible.Value != "1")
                        {
                            grdItemDetails.Columns[2].Visible = false;
                            grdItemDetails.Columns[3].Visible = false;
                        }
                        break;
                    #endregion
                    #region CLOSING STOCK HISTORY
                    case ControlsEnum.CLOSINGSTOCKHISTORY:
                        grdStockHistory.DataSource = dtClosingStockHistory;
                        grdStockHistory.DataBind();
                        if (hdfIsPurStkVisible.Value != "1")
                        {
                            grdStockHistory.Columns[3].Visible = false;
                            grdStockHistory.Columns[4].Visible = false;
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
                #region NEW
                case ControlsEnum.NEW:
                    CurrPK = 0;
                    grdItemDetails.DataSource = null;
                    grdItemDetails.DataBind();
                    lblClosingStockNo.Text = Resources.Messages.DocGenerationNew.ToString();
                    txtStockDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    var today = DateTime.Today;
                    var CurrentMonth = new DateTime(today.Year, today.Month, 1);
                    DateTime AsOnDate = CurrentMonth.AddDays(-1);
                    txtAsOnDate.Text = AsOnDate.ToString(Resources.Constants.DateFormatMonthYear);
                    lblLastModifiedHDR.Text = string.Empty;
                    ddlStatus.SelectedIndex = 0;
                    break;
                #endregion
                #region LIST
                case ControlsEnum.LIST:
                    CurrPK = 0;
                    ModifiedDatePnl.Visible = false;
                    LastModifiedTime = DateTime.Now;
                    lblLastModifiedHDR.Text = string.Empty;
                    ddlStatus.SelectedIndex = 0;
                    base.WkfRefID = ucrWrkf.RefID = 0;
                    break;
                #endregion
                #region clearAdvSearch
                case ControlsEnum.clearAdvSearch:
                    txtMonth.Text = string.Empty;
                    txtStockNo.Text = string.Empty;
                    ddlStatus.SelectedIndex = 0;
                    break;
                #endregion

            }
        }


        private void setvisibility(ActionsEnum ActionsEnum)
        {
            switch (ActionsEnum)
            {
                #region EDIT
                case ActionsEnum.DETAILS:
                case ActionsEnum.EDIT:
                    txtAsOnDate.Enabled = false;
                    txtAsOnDate.CssClass = "input-small input-disabled";
                    break;
                #endregion
                #region VIEW
                case ActionsEnum.VIEW:
                    txtAsOnDate.Enabled = false;
                    txtAsOnDate.CssClass = "input-small input-disabled";
                    break;
                #endregion
                #region New
                case ActionsEnum.NEW:
                    txtAsOnDate.Enabled = true;
                    txtAsOnDate.CssClass = "input-small";
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
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetFormattedExchangeRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
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
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
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
                    }
                    base.WkfPageUrl = path;
                }
            }
        }

        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
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
                bool bIsChecked = false;
                string savePath = string.Empty;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtAsOnDate")
                    {
                        commonActions = ActionsEnum.ASONDATECHANGE;
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
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        uclPaging.CurrentPage = 1;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.clearAdvSearch);
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.NEW);
                        setvisibility(ActionsEnum.NEW);
                        SetUIEditView(commonActions);
                        EntryStatus = EntryStatus.NEWMODE;
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKDETAILS);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKDETAILS);
                        FillProcessID(1);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ucrWrkf.ApplicationID = 0;
                        ucrWrkf.ViewType = 1;
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();

                        break;
                    #endregion
                    #region LIST
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        ResetForm(ControlsEnum.LIST);
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm(ControlsEnum.NEW);
                        ResetForm(ControlsEnum.clearAdvSearch);
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region AS ON DATE CHANGE
                    case ActionsEnum.ASONDATECHANGE:
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKDETAILS);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKDETAILS);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (hdfIsJournalize.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Messages.Msg_Journalize) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            string TrxNo = string.Empty;
                            StockHeaderObj = new StockHeader();
                            StockHeaderObj = (StockHeader)SetUIValuesToObject(ControlsEnum.CLOSINGSTOCKHDR);
                            if (StockHeaderObj != null && StockHeaderObj.StockItemList != null)
                            {
                                if (StockHeaderObj.StockItemList.Count > 0)
                                {
                                    StockHeaderObj.WKF_FLAG = 0;
                                    string xmlDoc = CommonFunctions.XmlSerialize<StockHeader>(StockHeaderObj);
                                    // save Process Control inspection details
                                    result = BusinessLogic.Finance.ClosingStockBL.SaveClosingStockDetails(xmlDoc, ref TrxNo);
                                    if (result > 0) // Success !  redirect to listing page
                                    {
                                        #region Update dummy entry while modify closing stock after approval
                                        if (Approved == (int)DbStatus.APPROVED) // Checking invoice stataus wheteher invoice approved or not
                                        {
                                            FinTrxService finTrxServiceClient;
                                            finTrxServiceClient = new FinTrxService();
                                            string refType = string.Empty;
                                            refType = ApplicationType.CLSTJ;
                                            long DummyResult = StockHeaderObj.LSH_PK;
                                            bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)StockHeaderObj.LSH_PK, 0);
                                            if (IsDummyEntry == true)
                                            {
                                                DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)StockHeaderObj.LSH_PK, 0);
                                            }
                                            if (DummyResult > 0)
                                            {
                                                finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                            }
                                            finTrxServiceClient = null;
                                        }
                                        #endregion

                                        // Show Save Message and redired to listing page                                        
                                        //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ClosingStock);

                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ClosingStock);
                                        //object[] args = new object[2];
                                        //args[0] = Resources.PageNameRes.ClosingStock;
                                        //args[1] = TrxNo;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);

                                        EntryStatus = EntryStatus.LISTMODE;
                                        ResetForm(ControlsEnum.clearAdvSearch);
                                        ResetForm(ControlsEnum.LIST);
                                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Amount_Zero").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }


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
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            string StockNo = string.Empty;


                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                string TrxNo = string.Empty;
                                StockHeaderObj = new StockHeader();
                                StockHeaderObj = (StockHeader)SetUIValuesToObject(ControlsEnum.CLOSINGSTOCKHDR);
                                if (StockHeaderObj != null && StockHeaderObj.StockItemList != null)
                                {
                                    if (StockHeaderObj.StockItemList.Count > 0)
                                    {
                                        StockHeaderObj.WKF_FLAG = 1;
                                        string xmlDoc = CommonFunctions.XmlSerialize<StockHeader>(StockHeaderObj);
                                        // save Process Control inspection details
                                        result = BusinessLogic.Finance.ClosingStockBL.SaveClosingStockDetails(xmlDoc, ref TrxNo);
                                        if (result > 0) // Success !  redirect to listing page
                                        {
                                            // Show Save Message and redired to listing page                                        
                                            //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ClosingStock);
                                            ucrWrkf.ApplicationID = result.Value;
                                            lblClosingStockNo.Text = TrxNo;
                                            //litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                            //object[] args = new object[2];
                                            //args[0] = Resources.PageNameRes.ClosingStock;
                                            //args[1] = TrxNo;
                                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            //    + "','" + Resources.ErpRes.Information + "');", true);

                                            //EntryStatus = EntryStatus.LISTMODE;
                                            //ResetForm(ControlsEnum.clearAdvSearch);
                                            //ResetForm(ControlsEnum.LIST);
                                            //GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                            //SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Amount_Zero").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }


                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            //|| (Request.QueryString[QueryStrings.PageType] != null &&
                            //Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.CLST))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_ClosingStock_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.clearAdvSearch);
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                    SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);

                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result.HasValue && result.Value > 0)
                                    {

                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        {
                                            FillProcessID(1);
                                            litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                                        }
                                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                        WrkfComments.Text = "";
                                        //Show Save success message and reset Contract Entry
                                        StockNo = lblClosingStockNo.Text.Trim();
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.ClosingStock;
                                        args[1] = StockNo;

                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        // Show Save Message and redired to listing page                                      
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {

                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.clearAdvSearch);
                                            ResetForm(ControlsEnum.LIST);
                                            GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                            SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.clearAdvSearch);
                                            ResetForm(ControlsEnum.LIST);
                                            GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                            SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region EDIT/VIEW/DETAILS
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.DETAILS:
                        hdfIsCancelled.Value = "0";
                        setvisibility(commonActions);
                        foreach (GridViewRow grdrow in grdClosingStock.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHdrPk")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfStatus")).Value);
                                Posted = Convert.ToBoolean(Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPosted")).Value));
                                hdfIsCancelled.Value = ((HiddenField)grdrow.FindControl("hdfDelStatus")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                            SetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Finance.ClosingStockBL.DeleteClosingStock(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ClosingStock);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.LIST);
                                GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.ClosingStock + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                    SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ClosingStock + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ClosingStock + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                    SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ClosingStock);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region CLOSING STOCK HISTORY
                    case ActionsEnum.CLOSINGSTOCKHISTORY:
                        GridViewRow gvrstkItms = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfItemCatPk = gvrstkItms.FindControl("hdfItemCatPk") as HiddenField;
                        Label lblCategotyName = gvrstkItms.FindControl("lblCategotyName") as Label;
                        lblItemCategotyName.Text = HttpUtility.HtmlDecode(lblCategotyName.Text);
                        int.TryParse(hdfItemCatPk.Value, out ItemCategoryPk);
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKHISTORY);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKHISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divStockHistory]','" + GetLocalResourceObject("ClosingStockHistory").ToString() + "','780','300');", true);

                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        hdfJournalizeWorkFlow.Value = "0";
                        //finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        //finInvoiceCusHdrObj.ICH_PK = CurrPK;
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.LIST);
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        //if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        //{
                        //    string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        //    if (Transaction == "SAVE")
                        //    {
                        //        salesInvoiceServiceClient = new SalesInvoiceService();
                        //        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        //        result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                        //    }
                        //}

                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.CLOSINGSTOCKLIST);
                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        //}
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        //if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        //{
                        //    string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                        //    if (Transaction == "DELETE")
                        //    {
                        //        salesInvoiceServiceClient = new SalesInvoiceService();
                        //        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        //        result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                        //    }
                        //}
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        hdfIsCancelled.Value = "0";
                        foreach (GridViewRow grvRow in grdClosingStock.Rows)
                        {
                            HiddenField hdfDept;
                            int dept;
                            RadioButton rbtSelect = (RadioButton)grvRow.FindControl("rbtSelect");
                            if (rbtSelect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grvRow.FindControl("hdfHdrPk")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grvRow.FindControl("hdfStatus")).Value);
                                Posted = Convert.ToBoolean(Convert.ToInt32(((HiddenField)grvRow.FindControl("hdfPosted")).Value));
                                hdfIsCancelled.Value = ((HiddenField)grvRow.FindControl("hdfDelStatus")).Value;
                                hdfDept = grvRow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }

                        if (bIsChecked)
                        {
                            if (hdfIsCancelled.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Alrdy_Cancel").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if (Approved == (int)DbStatus.DRAFTED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Draft_Cancel").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if (!Posted)
                            {
                                FillProcessID(11);
                                SetUIEditView(commonActions);
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                    ucrWrkf.ViewType = 1;
                                else
                                    ucrWrkf.ViewType = 0;
                                ucrWrkf.ViewAction();
                                ModifiedDatePnl.Visible = true;
                                GetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                                SetFieldValues(ControlsEnum.CLOSINGSTOCKHDR);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Journalized").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT Popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
            SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
            EntryStatus = EntryStatus.LISTMODE;
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
            uclPaging.CurrentPage = 1;
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteClosingStk.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            //lnkList.PreRender += new EventHandler(btnAction_PreRender);
            //lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);
            btnDeleteClosingStk.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            //btnPrint.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            //lnkList.Load += new EventHandler(btnAction_Load);
            //lnkDetail.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);

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
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;


                }

                PageIndex = uclPaging.CurrentPage.ToString();
                // Change Code As per the page
                GetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
                SetFieldValues(ControlsEnum.CLOSINGSTOCKLIST);
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
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                if (hdfInventTypeConfig.Value != "2")
                    btnJournalize.Visible = false;
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
            EXCHANGERATE,
            JOURNALIZE,
            FINHEADER,
            GETJOURNALBYPK,
            COMPANY,
            NEW,
            clearAdvSearch,
            CLOSINGSTOCKLIST,
            LIST,
            CLOSINGSTOCKDETAILS,
            CLOSINGSTOCKHDR,
            CLOSINGSTOCKHDRBYPK,
            CLOSINGSTOCKHISTORY,
            INVENTTYPECONFIG,
            CLOSINGSTOCKGET
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

    }
}