using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BusinessObject.Common;
using System.Data;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject;
using BusinessObject.CommonManagement;
using ERPData;
using ERPService;
using ERPManager;
using System.Threading;
using System.Web.UI.WebControls;

using ERPSMS_v01.UserControls;
using System.Xml;
using System.IO;
using BusinessObject.Shipping;
using BusinessLogic.Shipping;
using CustomControls;
using System.Text;

namespace ERPSMS_v01.Shipping
{
    public partial class DirectDeliveryOrder : ERP.Store.UI.WorkFlowBasePage
    {
        #region Properties & Variables

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
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return (GetNullableInt(hdfGRH_PK.Value) ?? 0);
            }
            set
            {
                int t = value;
                hdfGRH_PK.Value = t.ToString();
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
        /// Currency Format String
        /// </summary>
        private string CurrencyFormatString
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] == null ?
                    String.Format("{{0:c{0}}}", GetLocalResourceObject("QtyDecimal")) //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)
                    : (string)ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] = value;
            }
        }

        /// <summary>
        /// Property used for to store PedningPO List
        /// </summary>
        private List<PendingSO> PendingSOList
        {
            get
            {
                return this.ViewState["PendingSOList"] == null ? new List<PendingSO>() : (List<PendingSO>)(this.ViewState["PendingSOList"]);
            }
            set
            {
                this.ViewState["PendingSOList"] = value;
            }
        }

        /// <summary>
        /// To maintain keep Asset ServiceOrder Header
        /// </summary>
        private DirectDeliveryOrderHeader DirectDeliveryOrderHeaderSession
        {
            get
            {
                return (DirectDeliveryOrderHeader)ViewState[ViewstateStrings.DirectDeliveryOrderHeader];
            }
            set
            {
                ViewState[ViewstateStrings.DirectDeliveryOrderHeader] = value;
            }
        }

        /// <summary>
        /// Property used for to store Direct DeliveryOrder Details List
        /// </summary>
        private List<DirectDeliveryOrderDetails> DirectDeliveryOrderDetailList
        {
            get
            {
                return this.ViewState["DirectDeliveryOrderDetailList"] == null ? new List<DirectDeliveryOrderDetails>() : (List<DirectDeliveryOrderDetails>)(this.ViewState["DirectDeliveryOrderDetailList"]);
            }
            set
            {
                this.ViewState["DirectDeliveryOrderDetailList"] = value;
            }
        }

        /// <summary>
        /// Property used for to store Direct StockBatchDetailsList
        /// </summary>
        private List<DirectDeliveryOrderStockBatchDetails> StockBatchDetailsList
        {
            get
            {
                return this.ViewState["DirectDeliveryOrderStockBatchDetails"] == null ? new List<DirectDeliveryOrderStockBatchDetails>() : (List<DirectDeliveryOrderStockBatchDetails>)(this.ViewState["DirectDeliveryOrderStockBatchDetails"]);
            }
            set
            {
                this.ViewState["DirectDeliveryOrderStockBatchDetails"] = value;
            }
        }
        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
            }
        }

        /// <summary>
        /// Current Sl No.
        /// </summary>
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
        /// To maintain the Grid Row ItemType in viewstate(for batch popup)
        /// </summary>
        private int GridRowIndex
        {
            get
            {
                return Convert.ToInt32(this.ViewState["GridRowIndex"]);
            }
            set
            {
                this.ViewState["GridRowIndex"] = value;
            }
        }

        /// <summary>
        /// SAL_DESPATCH_DTL PK
        /// </summary>
        private int DPDPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PoDtlPK]);//[DPD_PK]
            }
            set
            {
                this.ViewState[ViewstateStrings.PoDtlPK] = value;
            }
        }

        private int DetailPK
        {
            get
            {
                return this.ViewState["DetailPK"] == null ? 0 : Convert.ToInt32(this.ViewState["DetailPK"]);
            }
            set
            {
                this.ViewState["DetailPK"] = value;
            }
        }
        private int BrandPK
        {
            get
            {
                return this.ViewState["BrandPK"] == null ? 0 : Convert.ToInt32(this.ViewState["BrandPK"]);
            }
            set
            {
                this.ViewState["BrandPK"] = value;
            }
        }
        private decimal CartonCount
        {
            get
            {
                return this.ViewState["CartonCount"] == null ? 0 : Convert.ToDecimal(this.ViewState["CartonCount"]);
            }
            set
            {
                this.ViewState["CartonCount"] = value;
            }
        }
        private decimal OrderTotalPCS
        {
            get
            {
                return this.ViewState["OrderTotalPCS"] == null ? 0 : Convert.ToDecimal(this.ViewState["OrderTotalPCS"]);
            }
            set
            {
                this.ViewState["OrderTotalPCS"] = value;
            }
        }
        private int DphPK
        {
            get
            {
                return this.ViewState["DphPK"] == null ? 0 : Convert.ToInt32(this.ViewState["DphPK"]);
            }
            set
            {
                this.ViewState["DphPK"] = value;
            }
        }
        /// <summary>
        /// CartonsGridviewViewState
        /// </summary>
        private List<CartonsGridview> CartonsGridviewListViewState
        {
            get
            {
                return ViewState["CartonsGridviewListViewState"] == null ? new List<CartonsGridview>() : (List<CartonsGridview>)ViewState["CartonsGridviewListViewState"];
            }
            set
            {
                ViewState["CartonsGridviewListViewState"] = value;
            }
        }
        /// <summary>
        /// CartonsListTemp
        /// </summary>
        private List<CartonDetails> CartonsListMapped
        {
            get
            {
                return ViewState["CartonsListMapped"] == null ? new List<CartonDetails>() : (List<CartonDetails>)ViewState["CartonsListMapped"];
            }
            set
            {
                ViewState["CartonsListMapped"] = value;
            }
        }

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
        /// For Get ConfigValue for show  netweight and gross weight
        /// </summary>
        private bool IsShowNetGrsWeight
        {
            get
            {
                return GetGlobalResourceObject("ConfigurationsRes", "IsShowDONetGrsWeight").ToString() == "1";
            }
        }
        #endregion

        #region Variables

        private int processPK;
        private string refID;
        private string inboxFlag;
        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;

        User currentUser;
        DataSet dsPageData;
        DataTable dtPageData;
        DataTable dtCompany;
        DataTable dtResult;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        DataTable dtSos;
        bool isCancelled = false;
        private int customerPk;
        private int selectedItem = 0;
        private DirectDeliveryOrderHeader objDirectDeliveryOrderHeader;
        private int PageVariable = 0;
        private List<SAL_ORDER_HDR> salesOrderHeaderList;
        private int grdDPHPK;
        private int currentItemId = 0;
        private DateTime transDate;
        private int currentStore = 0;
        SOHeaderBO objSOitem;
        List<SOHeaderListBO> objItemList;
        int FromUOMPK = 0;
        int ToUOMPK = 0;

        #endregion
        #endregion

        #region Page Events

        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            this.btnApplyCartonDetails.PreRender += new EventHandler(btnAction_PreRender);

            this.btnApplyCartonDetails.Load += new EventHandler(btnAction_Load);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDeleteNew.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            //  this.btnCancel.Load += new EventHandler(btnAction_Load);    
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnListPrint.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
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
        /// To Handle Page_Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ModifiedDatePnl.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                hdfIsMultipleClick.Value = "0";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region InitializeComponent
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
        private int FillProcessID(int deptID = 0, int pid = 0, bool SetProcessID = false)
        {
            int processID = 0;
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess;
            if (deptID > 0)
            {
                dtProcess = wrkfService.GetProcessID(path, deptID);
            }
            else
            {
                dtProcess = wrkfService.GetProcessID(path, Session[BusinessObject.Common.SessionStrings.CurDept] != null ? Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()) : 0);
            }
            if (SetProcessID && dtProcess != null && dtProcess.Rows.Count > 0)
            {
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
            }
            else
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    base.WkfPageUrl = ucrWrkf.PageUrl = path;
                    PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                }
            return processID;
        }
        #endregion

        #region PageActionHandler
        private void PageActionHandler()
        {
            int referenceID;
            int processID;
            int appId;
            referenceID = 0;
            processID = 0;
            appId = 0;
            string prefID;
            InitializeComponent();
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    ConfigurationSettings();

                    FillProcessID(0, 1, true);
                    processID = FillProcessID(0, 1);
                    ucrWrkf.ProcessID = processID;
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                       : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    EntryStatus = EntryStatus.ENTRYMODE;

                    string[] datakeyarray;
                    datakeyarray = new string[2];
                    datakeyarray[0] = "DPD_PK";
                    datakeyarray[1] = "DPD_SL_NO";
                    grdDeliveryOrderList.DataKeyNames = datakeyarray;

                    DirectDeliveryOrderHeaderSession = new DirectDeliveryOrderHeader();
                    DirectDeliveryOrderDetailList = new List<DirectDeliveryOrderDetails>();

                    #region Currency,Decimal,Rate,Number Format Settings
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperator.Value = "#" + currencysep + "#0.";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperator.Value += "0";
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

                    #endregion
                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
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
                        referenceID = int.Parse(refID);
                        appId = GetApplicationID(referenceID);
                        if (processPK == ucrWrkf.ProcessID)
                        {
                            CurrPK = appId;
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                    }
                   
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlEnums.COMPANY);
                    SetFieldValues(ControlEnums.COMPANY);
                    GetFieldValues(ControlEnums.DEPARTMENTSTORE);
                    SetFieldValues(ControlEnums.DEPARTMENTSTORE);
                    ddlDeptStore.SelectedValue = currentUser.CurrentDeptPK.ToString();
                    if (CurrPK > 0)
                    {
                        #region MyRegion
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        GetFieldValues(ControlEnums.PendingSOs);
                        SetFieldValues(ControlEnums.PendingSOs);
                        GetFieldValues(ControlEnums.DETAILFOREDIT);
                        SetFieldValues(ControlEnums.DETAILFOREDIT);
                        #endregion
                    }
                    else
                    {
                        ActionHandler(lnkList, EventArgs.Empty);
                    }
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;

                    if (Request.QueryString["PRefID"] != null)//From Inbox to Submit DO (Last task: Sale Order Approval)
                    {
                        int pRefId = GetNullableInt(Request.QueryString["PRefID"].ToString()) ?? 0;
                        DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(pRefId));//Check whether next process is started or not
                        if (dt.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(dt.Rows[0]["appPK"]);
                            FillTransactionData();
                        }
                        else
                        {
                            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                            DataTable dtApplication = wrkfService.GetApplicationID(pRefId);
                            if (dtApplication != null)
                            {
                                if (dtApplication.Rows.Count > 0)
                                {
                                    SO_PK.Value = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                                    GetFieldValues(ControlEnums.SOFROMINBOX);
                                    SetFieldValues(ControlEnums.SOFROMINBOX);
                                    EntryStatus = EntryStatus.NEWMODE;
                                }
                            }
                        }
                    }
                }
                txtCustomer.Focus();
            }
            catch (Exception ex)
            {
                ucrWrkf.ViewType = 0;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            bool bIsChecked = false;
            int result = 0;
            int soDetId;
            List<int> soDetIdList = new List<int>();
            List<DirectDeliveryOrderDetails> tempDirectDeliveryOrderDetailList;
            List<object> lstResult = new List<object>();
            bool validFlagDOlst = false;

            string savePath = string.Empty;
            int grdDeliveryOrderSearchListRowDeptId = 0;
            int selRecordStatus = 0;
            string IsInventoryLocked = "";
            string LockUptoDate = string.Empty;
            string deliveryaddress;
            GridViewRow gvrTemplate;
            string arg;
            GridViewRow grdvwRow;
            GridView grd;
            TextBox WrkfComments;
            int slno = 0;
            StringBuilder detailedValMsg = new StringBuilder();
            try
            {

                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
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
                    if (((ImageButton)sender).ID == "imbSearch") commonActions = ActionsEnum.CHANGE;
                    else commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlSTORE")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlBatchNogrid")
                    {
                        commonActions = ActionsEnum.BATCHCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlBatchesPopUp")
                    {
                        commonActions = ActionsEnum.BATCHCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlUOMGrid")
                    {
                        commonActions = ActionsEnum.RESETQTY;
                    }

                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "chkAutoMode")
                    {
                        if (chkAutoMode.Checked)
                            commonActions = ActionsEnum.AUTOALLOCATECARTON;
                        else
                        {
                            ShowCartonPopup();
                            chkAutoMode.Focus();
                        }
                    }
                }
                //else if (sender.GetType().IsEquivalentTo(typeof(GridView)))
                //{
                //    if (((GridView)sender).ID == "grdPendingSOs")
                //    {
                //        commonActions = ActionsEnum.ADDITEM;
                //    }
                //}
                #endregion
                switch (commonActions)
                {
                    case ActionsEnum.ISSUEAPPLY:
                        if(CartonsListMapped!=null)
                            if (CartonsListMapped.Sum(c => c.DSC_QTY_DESPATCHED) > OrderTotalPCS && hdfOrderPcsConfirm.Value=="0")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "fnConfirmOrderPcs();", true);
                                 return;
                            }
                        DespatchBO Despatch = new DespatchBO();
                        Despatch.DPD_PK = DetailPK;
                        Despatch.DPH_PK = DphPK;
                        Despatch.USER_PK = currentUser.PKUser;
                        Despatch.lstCartonDetail = new List<CartonDetails>();
                        foreach (CartonDetails item in CartonsListMapped)
                        {
                            Despatch.lstCartonDetail.Add(new CartonDetails() { DSC_PK = item.DSC_PK, DSC_CARTON_MST = item.DSC_CARTON_MST, DSC_QTY_DESPATCHED = item.DSC_QTY_DESPATCHED, SL_NO = item.SL_NO });
                        }

                        string xmlDoc = CommonFunctions.XmlSerialize<DespatchBO>(Despatch);
                        DataTable dtOut1 = null;
                        detailedValMsg.Clear();
                        result = DirectDeliveryOrderBL.SaveDOAllocation(xmlDoc);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("Allocation").ToString());
                            ResetForm(ControlEnums.CLEAR);
                            ResetForm(ControlEnums.CLEARSEARCH);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                        }
                        else
                        {
                            #region Error Messages
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }

                            else if (result == -40)//The following stock batch qty is already used in another DO.
                            {
                                detailedValMsg.Append(this.GetLocalResourceObject("AlreadyUsedInAnotherDO").ToString());
                                if (dtOut1 != null)
                                {
                                    foreach (DataRow item in dtOut1.Rows)
                                    {
                                        detailedValMsg.Append("<ul><li>" + item[0].ToString() + " : " + item[1].ToString().Replace("'", "") + "</li></ul>");
                                    }
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + detailedValMsg.ToString()
                                + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            #endregion
                        }

                        break;
                    #region AUTO ALLOCATE CARTON
                    case ActionsEnum.AUTOALLOCATECARTON:
                        if (grdCartons.Rows.Count > 0)
                        {
                            if (hdfCartonListReloadConfirm.Value != 1.ToString())
                            {
                                chkAutoMode.Focus();
                                ShowCartonPopup();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnCartonListReloadConfirm", "fnCartonListReloadConfirm();", true);
                                return;
                            }
                        }

                        dtResult = ContainerReleaseBL.AutoAllocateCartonDODetails(BrandPK, CartonCount, DetailPK);
                        slno = 0;
                        List<CartonDetails> CartonsListTemp1 = new List<CartonDetails>();
                        foreach (DataRow row in dtResult.Rows)
                        {
                            CartonDetails temp = new CartonDetails();
                            temp.DSC_CARTON_MST = Convert.ToInt32(row["BCR_PK"]);
                            temp.DSC_CARTON_NO = Convert.ToString(row["BCR_NO"]);
                            temp.DSC_QTY_DESPATCHED = Convert.ToInt32(row["BCR_TOTAL_QTY"]);
                            temp.DSC_LOCATION_TEXT = (Convert.ToString(row["BCR_LOCATION_TEXT"])).HtmlDecode();
                            temp.SL_NO = ++slno;
                            CartonsListTemp1.Add(temp);
                        }
                        CartonsGridviewListViewState = null;
                        if (CartonsListTemp1 != null && CartonsListTemp1.Count > 0)
                        {
                            CartonsListMapped = CartonsListTemp1;
                            #region Add to CartonsGridviewListViewState
                            List<CartonsGridview> tempCartonsGridview = new List<CartonsGridview>();
                            CartonsGridview temp1 = new CartonsGridview();
                            temp1.BCR_PALLET_NO = GetLocalResourceObject("Carton").ToString();
                            temp1.CartonsCount = CartonsListTemp1.Count;
                            temp1.QtyPcs = Convert.ToInt32(CartonsListTemp1.Sum(x => x.DSC_QTY_DESPATCHED));
                            temp1.BCR_LOCATION_TEXT = CartonsListTemp1[0].DSC_LOCATION_TEXT;
                            tempCartonsGridview.Add(temp1);
                            CartonsGridviewListViewState = tempCartonsGridview;
                        }
                            #endregion
                        BindGrid(ControlEnums.CONTAINERSPLITUP);
                        chkAutoMode.Focus();
                        ShowCartonPopup();
                        hdfCartonListReloadConfirm.Value = 0.ToString();
                        break;
                    #endregion

                    #region CLEAR AUTO ALLOCATION
                    case ActionsEnum.CLEARAUTOALLOCATION:
                        grdCartons.DataSource = null;
                        grdCartons.DataBind();
                        ShowCartonPopup();
                        break;
                    #endregion
                    #region CLEARMAPPING
                    case ActionsEnum.CLEARMAPPING:
                        ResetForm(ControlEnums.SEARCH);
                        ShowCartonPopup();
                        btnClearAlloc.Focus();
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        List<string> lstCartons = new List<string>();
                        // int palleteId = (GetNullableInt(hdfPalleteBinCard.Value) ?? 0) > 0 ? GetNullableInt(hdfPalleteBinCard.Value).Value : 0;
                        string palleteNo = "";
                        //if (palleteId == 0) { palleteNo = txtPalleteBinCard.Text.Trim(); }
                        string cartonPrefix = txtCartonPrefixPopUp.Text.Trim();
                        int cartonFrom = GetNullableInt(txtCartonFromPopUp.Text.Trim()) ?? -1;
                        int cartonTo = GetNullableInt(txtCartonToPopUp.Text.Trim()) ?? -1;
                        int locationId = GetNullableInt(ddlLocationPopUp.SelectedValue) ?? 0;
                        //?  int soPk = (GetNullableInt(hdfCurrentSCID.Value) ?? 0) > 0 ? GetNullableInt(hdfCurrentSCID.Value).Value : 0; //GetNullableInt(hdfCDR_SO_DTL.Value).Value;
                        cartonTo = cartonTo > -1 ? cartonTo : 0;
                        if (cartonFrom > -1)
                        {
                            int strLength = txtCartonFromPopUp.Text.Length;
                            string CodeExp = cartonFrom.ToString().PadLeft(strLength, '0');

                            lstCartons.Add(cartonPrefix + CodeExp);
                            for (int i = cartonFrom + 1; i <= cartonTo; i++)
                            {
                                string CodeExpInner = i.ToString().PadLeft(strLength, '0');
                                lstCartons.Add(cartonPrefix + CodeExpInner);
                            }
                        }
                        string cartonsWithComa = string.Join(",", lstCartons.ToArray());
                        int brandPk = BrandPK;
                        // int currentCDRPk = GetNullableInt(hdfCurrentCDR_PK.Value).Value;
                        dtResult = BusinessLogic.Shipping.ContainerReleaseBL.AddCartonToDOList(cartonsWithComa, cartonPrefix, locationId, BrandPK, DetailPK);

                        int cdrSlNo = Convert.ToInt32(hdfCDR_SL_NO_PopUp.Value);
                        List<CartonDetails> tempList1 = new List<CartonDetails>();


                        //  List<Cartons> tempList = CartonsListTemp;
                        int slNo = CartonsListMapped.Count > 0 ? CartonsListMapped.Max(x => x.SL_NO) : 0;// 0;
                        foreach (DataRow row in dtResult.Rows)
                        {
                            CartonDetails temp = new CartonDetails();
                            temp.DSC_CARTON_MST = Convert.ToInt32(row["BCR_PK"]);
                            temp.DSC_CARTON_NO = Convert.ToString(row["BCR_NO"]);
                            temp.DSC_QTY_DESPATCHED = Convert.ToInt32(row["BCR_TOTAL_QTY"]);
                            temp.DSC_LOCATION_TEXT = (Convert.ToString(row["BCR_LOCATION_TEXT"])).HtmlDecode();

                            temp.SL_NO = ++slNo;
                            if (CartonsListMapped.Exists(x => x.DSC_CARTON_MST == temp.DSC_CARTON_MST)) // Contains(  temp))
                            {
                                btnPlus2.Focus();
                                ShowCartonPopup();
                                string message = string.Empty;
                                message = string.Format(GetLocalResourceObject("Err_CartonAlreadyExistsInList").ToString(), temp.DSC_CARTON_NO);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }

                            tempList1.Add(temp);
                        }
                        if (tempList1.Count < 1)
                        {

                            ShowCartonPopup();
                            string message = GetLocalResourceObject("NoCartonsFound").ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message) + "','" + Resources.Messages.Information + "');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrMessageFocusCtrl('" + CommonFunctions.FormatErrorMessage(message) + "',null,null,null,'" + "txtPalleteBinCard" + "');", true);
                            return;
                        }

                        string message2 = string.Empty; //GetLocalResourceObject("Err_FollowingCartonsMissing").ToString();
                        bool flag = true;
                        foreach (var item in lstCartons)
                            if (!tempList1.Any(x => x.DSC_CARTON_NO.ToUpper().Contains(item.ToUpper()))) // == ))
                            {
                                message2 += item + ", ";
                                flag = false;
                            }
                        if (!flag && hdfSomeCartonMissingConfirm.Value != 1.ToString())
                        {
                            hdfSomeCartonMissingMessage.Value = message2.HtmlEncode();
                            btnPlus2.Focus();
                            ShowCartonPopup();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(message2) + "','" + Resources.Messages.Information + "');", true);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmSomeCartonMissing", "fnConfirmSomeCartonMissing();", true);
                            return;
                        }

                        #region Add the new cartons to  CartonsGridviewViewState
                        List<CartonsGridview> tempCartonsGridview2 = CartonsGridviewListViewState;
                        tempCartonsGridview2 = new List<CartonsGridview>();
                        int slNoGrp = 1;
                        if (tempCartonsGridview2.Count > 0)
                        {
                            slNoGrp = tempCartonsGridview2.Max(x => x.CRC_SL_NO_GRP) + 1;
                        }



                        List<CartonDetails> tempList = CartonsListMapped;
                        tempList.AddRange(tempList1);
                        CartonsListMapped = tempList;

                        tempList1 = tempList.DeepClone();
                        CartonsGridview temp2 = new CartonsGridview();
                        temp2.CRC_SL_NO_GRP = slNoGrp;
                        temp2.CartonsCount = tempList1.Count;
                        temp2.QtyPcs = Convert.ToInt32(tempList1.Sum(x => x.DSC_QTY_DESPATCHED));
                        temp2.BCR_LOCATION_TEXT = tempList1[0].DSC_LOCATION_TEXT;
                        tempCartonsGridview2.Add(temp2);
                        CartonsGridviewListViewState = tempCartonsGridview2;
                        //foreach (var item in tempList1)
                        //{
                        //    item.CRC_SL_NO_GRP = slNoGrp;
                        //}

                        #endregion
                        BindGrid(ControlEnums.CONTAINERSPLITUP);
                        //btnPlus2.Focus();
                        txtCartonPrefixPopUp.Focus();
                        ShowCartonPopup();
                        ResetForm(ControlEnums.SEARCH);
                        chkAutoMode.Checked = false;
                        break;
                    #endregion


                    #region LIST/CANCEL
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = base.WkfRefID = 0;
                        FillProcessID(0, 1);
                        this.ModifiedDatePnl.Visible = false;
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region SHOWSODTL
                    case ActionsEnum.SHOWSODTL:
                        arg = ((Button)sender).CommandArgument;
                        grdvwRow = ((Button)sender).Parent.Parent as GridViewRow;
                        if (grdvwRow != null)
                        {
                            grd = grdvwRow.FindControl("grdSOList") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                salesOrderHeaderList = null;
                            }
                            else
                            {
                                grdDPHPK = Convert.ToInt32(arg);
                                GetFieldValues(ControlEnums.SODETAILS);
                            }
                            grd.Visible = true;
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                string DoNo = ((Label)grdvwRow.FindControl("lblNo")).Text;
                                int dphPK = Convert.ToInt32(((HiddenField)grdvwRow.FindControl("hdfDespatchID")).Value);
                                DataColumn dcdoNo = new DataColumn("DPH_NO");
                                dtPageData.Columns.Add(dcdoNo);
                                DataColumn dcDphPK = new DataColumn("DPH_PK");
                                dtPageData.Columns.Add(dcDphPK);
                                foreach (DataRow dr in dtPageData.Rows)
                                {
                                    dr["DPH_NO"] = DoNo;
                                    dr["DPH_PK"] = dphPK;
                                }

                                grd.DataSource = dtPageData;
                                grd.DataBind();
                            }
                            (grdvwRow.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        this.ModifiedDatePnl.Visible = false;
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlEnums.CLEAR);
                        this.EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        if (hdfCustomerID.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomerID.Value != string.Empty && ddlDeptStore.SelectedIndex > 0)
                        {
                            GetFieldValues(ControlEnums.PendingSOs);
                            SetFieldValues(ControlEnums.PendingSOs);
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnums.CLEAR);
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region Customer Selected
                    case ActionsEnum.CUSTOMERSELECTED:
                        if (hdfCustomerID.Value != CommonConstants.SELECT_VALUE_ZERO && hdfCustomerID.Value != string.Empty)
                        {
                            customerPk = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                            #region DeliveryToAddress Setting
                            GetFieldValues(ControlEnums.CUSTOMER);
                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                deliveryaddress = string.Empty;
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()))
                                {
                                    deliveryaddress += string.IsNullOrEmpty(deliveryaddress) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString());
                                }
                                if (!string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()))
                                {
                                    deliveryaddress += string.IsNullOrEmpty(deliveryaddress) ? HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()) :
                                        string.IsNullOrEmpty(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString().Trim()) ?
                                        string.Empty : ", " + HttpUtility.HtmlDecode(dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString());
                                }
                                txtDeliveryAddress.Text = deliveryaddress;
                            }
                            #endregion
                            GetFieldValues(ControlEnums.PendingSOs);
                            SetFieldValues(ControlEnums.PendingSOs);
                            DirectDeliveryOrderDetailList = null;
                            BindGrid(ControlEnums.DIRECTDELIVERYORDERLIST);
                        }

                        break;
                    #endregion
                    #region EDIT DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdDeliveryOrderSearchList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                grdDeliveryOrderSearchListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDepartmentID")).Value);
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfStatus")).Value) == 4)
                                {
                                    hdfIsDOCancelled.Value = "1";
                                }
                                else
                                {
                                    hdfIsDOCancelled.Value = "0";
                                }
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(grdDeliveryOrderSearchListRowDeptId, 1);
                            SetUIEditView(commonActions);
                            EntryStatus = EntryStatus.EDITMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();

                            GetFieldValues(ControlEnums.CUSTOMER);
                            SetFieldValues(ControlEnums.CUSTOMER);
                            GetFieldValues(ControlEnums.PendingSOs);
                            SetFieldValues(ControlEnums.PendingSOs);
                            GetFieldValues(ControlEnums.DETAILFOREDIT);
                            SetFieldValues(ControlEnums.DETAILFOREDIT);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        #region Checking :Inventory Transaction Locking
                        IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtDODate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                        if (IsInventoryLocked == "1")
                        {
                            litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        #endregion
                        if (DirectDeliveryOrderDetailList == null || DirectDeliveryOrderDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            validFlagDOlst = UpdateDirectDeliveryOrderDetailListFromGridview(true, 1);
                            if (validFlagDOlst == false)
                            {
                                return;
                            }
                            WKF_FLAG.Value = 0.ToString();
                            objDirectDeliveryOrderHeader = new DirectDeliveryOrderHeader();
                            objDirectDeliveryOrderHeader = (DirectDeliveryOrderHeader)SetUIValuesToObject(ControlEnums.DIRECTDELIVERYORDERHDR);
                            if (objDirectDeliveryOrderHeader != null)
                            {
                                if (objDirectDeliveryOrderHeader.DirectDODetailsList != null && objDirectDeliveryOrderHeader.DirectDODetailsList.Count > 0)
                                {
                                    string strTrxNumber = string.Empty;
                                    objDirectDeliveryOrderHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string xmlDocSave = CommonFunctions.XmlSerialize<DirectDeliveryOrderHeader>(objDirectDeliveryOrderHeader);
                                    DataTable dtOut = null;
                                    detailedValMsg.Clear();
                                    lstResult = DirectDeliveryOrderBL.SaveDirectDeliveryOrder(xmlDocSave, ref dtOut);
                                    result = Convert.ToInt32(lstResult[0]);
                                    strTrxNumber = lstResult[1].ToString();
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                                        ResetForm(ControlEnums.CLEAR);
                                        ResetForm(ControlEnums.CLEARSEARCH);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                        GetFieldValues(ControlEnums.LIST);
                                        SetFieldValues(ControlEnums.LIST);
                                    }
                                    else
                                    {
                                        #region Error Messages
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == -30)//NotEnoughStock
                                        {
                                            detailedValMsg.Append(this.GetLocalResourceObject("Msg_NotEnoughStocks").ToString());
                                            if (dtOut != null)
                                            {
                                                foreach (DataRow item in dtOut.Rows)
                                                {
                                                    detailedValMsg.Append("<ul><li>" + item[0].ToString() + " : " + item[1].ToString().Replace("'", "") + "</li></ul>");
                                                }
                                            }
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + detailedValMsg.ToString()
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        else if (result == -31)///Despatch Now Quantity Exceeds. Do you want to continue?
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DespatchNowExceeds", "$(document).ready(function(){ShowDespatchNowQtyExceeds(1);});", true);
                                            return;
                                        }
                                        else if (result == -32)///This is an Invoice entry.So modification is not possible. 
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_Invoice_Exist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == -40)//The following stock batch qty is already used in another DO.
                                        {
                                            detailedValMsg.Append(this.GetLocalResourceObject("AlreadyUsedInAnotherDO").ToString());
                                            if (dtOut != null)
                                            {
                                                foreach (DataRow item in dtOut.Rows)
                                                {
                                                    detailedValMsg.Append("<ul><li>" + item[0].ToString() + " : " + item[1].ToString().Replace("'", "") + "</li></ul>");
                                                }
                                            }
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + detailedValMsg.ToString()
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        else if (result == -42)//Different Type 
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + GetLocalResourceObject("Err_ItemType_differ").ToString()
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        #endregion
                                    }
                                }
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
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        //Show WorkFlow Popup
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    case ActionsEnum.WRKFSUBMIT:
                        ucrWrkf.ApplicationID = 0;
                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                        {
                            WKF_FLAG.Value = 1.ToString();
                            #region Checking :Inventory Transaction Locking
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtDODate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                            if (IsInventoryLocked == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            #endregion
                            if (DirectDeliveryOrderDetailList == null || DirectDeliveryOrderDetailList.Count == 0)
                            {
                                litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            else
                            {
                                validFlagDOlst = UpdateDirectDeliveryOrderDetailListFromGridview(true, 2);
                                if (validFlagDOlst == false)
                                {
                                    return;
                                }
                                objDirectDeliveryOrderHeader = new DirectDeliveryOrderHeader();
                                objDirectDeliveryOrderHeader = (DirectDeliveryOrderHeader)SetUIValuesToObject(ControlEnums.DIRECTDELIVERYORDERHDR);
                                SaveTransaction(objDirectDeliveryOrderHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                            }
                        }
                        else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel
                        {
                            #region Cancel Submit Codes

                            if (BusinessLogic.Shipping.DirectDeliveryOrderBL.ValidationForCancellationDO(CurrPK))
                            {
                                //ucrWrkf.ApplicationID = CurrPK;
                                isCancelled = true;
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_DO_Cancel").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                    FillProcessID(0, 1);
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                WrkfComments.Text = "";
                                EntryStatus = EntryStatus.LISTMODE;

                                this.CurrPK = base.WkfRefID = 0;
                                this.ModifiedDatePnl.Visible = false;
                                GetFieldValues(ControlEnums.LIST);
                                SetFieldValues(ControlEnums.LIST);
                            }
                            #endregion
                        }
                        else
                        {
                            #region Checking :Inventory Transaction Locking
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtDODate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                            if (IsInventoryLocked == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            #endregion
                            //ucrWrkf.ApplicationID = CurrPK;
                            SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }

                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.Shipping.DirectDeliveryOrderBL.DeleteDirectDO(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            #region Error Messages
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            #endregion
                        }
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        List<int> lstItemType=new List<int>();
                        objSOitem = new SOHeaderBO();
                        objSOitem.SOList = new List<SOHeaderListBO>();
                        SOHeaderListBO objSoList;
                        SetAddedPendingSOToDOList();
                        foreach (GridViewRow grdrow in grdPendingSOs.Rows)
                        {
                            CheckBox chk;
                            chk = (CheckBox)grdrow.FindControl("chkSelectSOList");
                            if (chk.Checked)
                            {
                                bIsChecked = true;
                                soDetId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSODetIdPendingList")).Value);//SOD_PK3                                
                                if (DirectDeliveryOrderDetailList.Where(x => x.DPD_SO_DTL == soDetId).Count() == 0)
                                {
                                    soDetIdList.Add(soDetId);
                                }
                                objSoList = new SOHeaderListBO();
                                objSoList.SOH_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOIdPendingList")).Value);//SOH_PK
                                objItemList.Add(objSoList);
                                lstItemType.Add(Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfProductType")).Value));
                            }
                        }
                        if (bIsChecked && CheckAddedType(lstItemType))
                        {
                            objSOitem.SOList = objItemList;
                            string xmlSOHPK = string.Empty;
                            if (objSOitem != null && objSOitem.SOList != null)
                            {
                                if (objSOitem.SOList.Count > 0)
                                {
                                    xmlSOHPK = CommonFunctions.XmlSerialize<SOHeaderBO>(objSOitem);
                                }
                            }
                            result = BusinessLogic.Shipping.DirectDeliveryOrderBL.CheckforValidMultipleSO(xmlSOHPK);      //SPSAL_DESPATCH_DIR_SO_VALDATE                
                            if (result < 0)
                            {
                                #region Validation Messages
                                if (result == -4) //DIFF CUSTOMER
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Customer").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -5) //DIFF TYPE
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Type").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -6) //DIFF CURRENCY                            
                                {

                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Error_Currency").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -7) //DIFF TAX / DISCOUNT / OC 
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiffTaxType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -8) //CUSTOME TAX / DISCOUNT / OC 
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_CustomTaxType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -9) //MULTIPLE TAX / DISCOUNT / OC 
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_HeaderTaxType").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (result == -10) //DISCOUNT EXISTS
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_DiscountExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else //Default 
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                soDetIdList = null;
                                #endregion
                            }
                            else
                            {
                                //if (IsSelectedSOValid(soDetIdList))
                                //{
                                if (soDetIdList.Count > 0)
                                {
                                    UpdateDirectDeliveryOrderDetailListFromGridview(false);
                                    List<PendingSO> pendingSOTemp = new List<PendingSO>();
                                    pendingSOTemp = PendingSOList;
                                    pendingSOTemp.Where(x => soDetIdList.Contains(x.SODetId))
                                       .ToList()
                                       .ForEach(l => { l.AddedToStockList = true; l.CheckBoxChecked = true; });
                                    PendingSOList = pendingSOTemp;

                                    List<PendingSO> lstNewSelectedSOs = PendingSOList
                                        .Where(x => soDetIdList.Contains(x.SODetId)) // .Where(x => poIdList.Contains(x.POId))
                                        .ToList();
                                    slno = 0;
                                    if (DirectDeliveryOrderDetailList.Count > 0)
                                    {
                                        slno = DirectDeliveryOrderDetailList.Max(x => x.DPD_SL_NO);
                                    }

                                    List<DirectDeliveryOrderDetails> tempNewList = new List<DirectDeliveryOrderDetails>();
                                    tempNewList = lstNewSelectedSOs
                                        .Select(x => new DirectDeliveryOrderDetails
                                        {
                                            DPD_PK = 0,
                                            DPD_SALE_ORDER = x.SOId,
                                            DPD_SO_DTL = x.SODetId,
                                            SONumber = x.SONumber,
                                            SODate = x.SODate,
                                            ItemCategoryId = x.ItemCatId,
                                            DPD_ITEM = x.ItemId,
                                            ItemId = x.ItemId,
                                            ItemName = x.ItemName,
                                            DPD_UOM = x.UOMId,
                                            DPD_SALE_UOM = x.UOMId,
                                            DPD_SALE_UOM_CONV = 1,
                                            DPD_BIZUNIT = Convert.ToInt16(currentUser.SBUID),
                                            DPD_SALE_QTY = x.SOQty,
                                            DPD_BALANCE_QTY_TO_DELIVER = x.BalanceQtyToDeliver,
                                            DPD_PRE_DELIVERED_QTY = x.PreDeliveredQty,
                                            DPD_QTY_DESPATCHED = x.BalanceQtyToDeliver,
                                            SORate = x.SORate,
                                            ITM_NEED_BATCH_STK = x.ITM_NEED_BATCH_STK,
                                            DPD_SALE_UOM_TEXT = x.UOM,
                                            ProductType=x.ProductType
                                        })
                                        .ToList();

                                    foreach (var item in tempNewList) item.DPD_SL_NO = ++slno;

                                    tempDirectDeliveryOrderDetailList = new List<DirectDeliveryOrderDetails>();
                                    tempDirectDeliveryOrderDetailList = DirectDeliveryOrderDetailList;
                                    tempDirectDeliveryOrderDetailList.AddRange(tempNewList);
                                    DirectDeliveryOrderDetailList = tempDirectDeliveryOrderDetailList;
                                    BindGrid(ControlEnums.DIRECTDELIVERYORDERLIST);
                                    BindGrid(ControlEnums.PendingSOs);
                                }
                                else if (soDetIdList.Count < 1 && DirectDeliveryOrderDetailList.Count > 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_RecordsAlreadyAdded").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (soDetIdList.Count < 1 && DirectDeliveryOrderDetailList.Count < 1)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsSelected").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsSelected").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                //}
                                //else
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Err_SameItemDiffRate").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                //}
                            }
                        }
                        else if (lstItemType != null && lstItemType.Count > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_ItemType_differ").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsSelected").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region "CLEARITEM"
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlEnums.CLEARREJECTCONTROLS);
                        ShowRejectedPopup();
                        break;
                    #endregion
                    //#region "ADDITEM"
                    //case ActionsEnum.ADDITEM:
                    //    GridViewSOChangeColour();
                    //    break;
                    //#endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        ActionHandler(btnEdit, EventArgs.Empty);
                        if (EntryStatus == EntryStatus.EDITMODE) this.EntryStatus = EntryStatus.VIEWMODE;
                        break;
                    #endregion
                    #region Printlisting
                    case ActionsEnum.PRINTLISTING:
                        foreach (GridViewRow grdrow in grdDeliveryOrderSearchList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                if (CurrPK > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.DOD + "&APPSUBTYPE=6") + "');", true);
                                    return;
                                }
                            }
                        }
                        //if (IsExportExcel)
                        //{
                        //    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + dOPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6");
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + dOPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6") + "');", true);
                        //}
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        break;
                    #endregion
                    #region PRINT (Detail Print)
                    case ActionsEnum.PRINT:
                        if (((Button)sender).ID == "btnPLPrint")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DOD + "&APPSUBTYPE=2") + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.DOD + "&APPSUBTYPE=6") + "');", true);
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
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdDeliveryOrderSearchList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDespatchID")).Value);
                                grdDeliveryOrderSearchListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDepartmentID")).Value);
                                selRecordStatus = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfStatus")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (selRecordStatus != 0 && selRecordStatus != 4)
                            {
                                ucrWrkf.Reset();
                                FillProcessID(grdDeliveryOrderSearchListRowDeptId, 11);
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

                                GetFieldValues(ControlEnums.CUSTOMER);
                                SetFieldValues(ControlEnums.CUSTOMER);
                                GetFieldValues(ControlEnums.PendingSOs);
                                SetFieldValues(ControlEnums.PendingSOs);
                                GetFieldValues(ControlEnums.DETAILFOREDIT);
                                SetFieldValues(ControlEnums.DETAILFOREDIT);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_InvalidRecordForOpn").ToString();//Selected record is invalid for this operation
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region Clear Batches List
                    case ActionsEnum.CLEARADD:
                        if (((ImageButton)sender).ID == "imbClearGridBatch")
                        {
                            GridRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
                            int itemSlNo = Convert.ToInt32(grdDeliveryOrderList.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            int itemId = Convert.ToInt32(((grdDeliveryOrderList.Rows[GridRowIndex].FindControl("hdfItemIdDOList") as HiddenField).Value));
                            DropDownList ddlBatchNogrid = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("ddlBatchNogrid") as DropDownList;
                            Label lblCurrentStock = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("lblCurrentStock") as Label;
                            DropDownList ddlUOMGrid = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("ddlUOMGrid") as DropDownList;
                            SetMultiBatchForGrid(false, true);
                            DataTable dtBatch = new DataTable();
                            dtBatch = DirectDeliveryOrderBL.GetBatchNo(itemId, currentUser.CurrentDeptPK, 0, null, 0);
                            ddlBatchNogrid.Items.Clear();
                            if (dtBatch != null && dtBatch.Rows.Count > 0)
                            {
                                ddlBatchNogrid.DataSource = CommonFunctions.HtmlDecodeDataTable(dtBatch, GTIService.Constants.Shipping.Fields.SBD_BATCH_NO);
                                ddlBatchNogrid.DataTextField = GTIService.Constants.Shipping.Fields.SBD_BATCH_NO;
                                ddlBatchNogrid.DataValueField = GTIService.Constants.Shipping.Fields.SBD_PK;
                                ddlBatchNogrid.DataBind();
                                ddlBatchNogrid.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                if (ddlBatchNogrid.SelectedValue != CommonConstants.SELECTVAL)
                                {
                                    int stockToUomPK = string.IsNullOrEmpty(ddlUOMGrid.SelectedValue) ? 0 : Convert.ToInt32(ddlUOMGrid.SelectedValue);
                                    DataTable dt = BatchDetails(Convert.ToInt32(ddlBatchNogrid.SelectedValue), stockToUomPK);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        lblCurrentStock.Text = GetFormattedNumber(dt.Rows[0]["SBD_STK_TO_UOM"]);
                                    }
                                }
                                else
                                {
                                    lblCurrentStock.Text = "0";
                                }
                            }
                            DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == itemSlNo).DOStockBatch_Details.Clear();
                        }
                        break;
                    #endregion

                    #region BATCHCHANGE (Batch dropDown in Grid)
                    case ActionsEnum.BATCHCHANGE:
                        if (((DropDownList)sender).ID == "ddlBatchNogrid")
                        {
                            gvrTemplate = ((DropDownList)sender).Parent.Parent as GridViewRow;
                            HiddenField hdfcategory = (HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfcategory");
                            HiddenField hdfItem = (HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfItemIdDOList");
                            Label lblCurrentStock = (Label)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("lblCurrentStock");
                            DropDownList ddlUOMGrid = ((DropDownList)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("ddlUOMGrid"));
                            HiddenField hdfPkDOList = (HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfPkDOList");
                            HiddenField hdfDPD_SL_NO = (HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfDPD_SL_NO");
                            DropDownList ddlBatchNogrid = (DropDownList)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("ddlBatchNogrid");
                            DataTable dt = new DataTable();
                            if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                            {
                                double gridBatchSum = 0;
                                int stockToUomPK = string.IsNullOrEmpty(ddlUOMGrid.SelectedValue) ? 0 : Convert.ToInt32(ddlUOMGrid.SelectedValue);
                                dt = BatchDetails(Convert.ToInt32(((DropDownList)sender).SelectedValue), stockToUomPK);
                                for (int i = 0; i <= grdDeliveryOrderList.Rows.Count - 1; i++)
                                {
                                    HiddenField hdfItemRow = (HiddenField)grdDeliveryOrderList.Rows[i].FindControl("hdfItemIdDOList");
                                    DropDownList ddlBatchNogridrow = (DropDownList)grdDeliveryOrderList.Rows[i].FindControl("ddlBatchNogrid");
                                    TextBox txtQtygrid = grdDeliveryOrderList.Rows[i].FindControl("txtDespatchNowQty") as TextBox;
                                    HiddenField hdfDetailPk = (HiddenField)grdDeliveryOrderList.Rows[i].FindControl("hdfDetailPk");
                                    if (hdfItem.Value == hdfItemRow.Value && ((DropDownList)sender).SelectedValue == ddlBatchNogridrow.SelectedValue && gvrTemplate.RowIndex != i)
                                    {
                                        gridBatchSum += txtQtygrid.Text == string.Empty ? 0 : Convert.ToDouble(txtQtygrid.Text);
                                    }
                                    ((TextBox)grdDeliveryOrderList.Rows[i].FindControl("txtDespatchNowQty")).Attributes.CssStyle.Remove("color");
                                }
                                lblCurrentStock.Text = dt.Rows.Count > 0 ? GetFormattedNumber((Convert.ToDouble(dt.Rows[0]["SBD_STK_TO_UOM"]))) : "0";
                                #region Adding Selected Batch Details to StockBatch_Details List
                                List<DirectDeliveryOrderStockBatchDetails> tempStockBatchDetailsList = new List<DirectDeliveryOrderStockBatchDetails>();
                                int dpdSlno = string.IsNullOrEmpty(hdfDPD_SL_NO.Value) ? 0 : Convert.ToInt32(hdfDPD_SL_NO.Value);
                                int dpdPK = string.IsNullOrEmpty(hdfPkDOList.Value) ? 0 : Convert.ToInt32(hdfPkDOList.Value);
                                DirectDeliveryOrderDetails directdeliveryOrderDetailsObj = DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == dpdSlno);

                                slno = 1;
                                if (tempStockBatchDetailsList == null || tempStockBatchDetailsList.Count == 0)
                                {
                                    slno = 1;
                                }
                                DirectDeliveryOrderStockBatchDetails ItemDetailsObj = new DirectDeliveryOrderStockBatchDetails();
                                ItemDetailsObj.SIC_PK = 0;
                                ItemDetailsObj.SIC_SL_NO = dpdSlno;
                                ItemDetailsObj.SIC_ITEM_SL_NO = slno;
                                ItemDetailsObj.SIC_DESPATCH_DTL = dpdPK;
                                ItemDetailsObj.SIC_QTY_CONSUMED = string.IsNullOrEmpty(lblCurrentStock.Text) ? 0 : Convert.ToDecimal(GetFormattedNumber(lblCurrentStock.Text));
                                ItemDetailsObj.SIC_STK_BATCH = Convert.ToInt32(ddlBatchNogrid.SelectedValue);
                                ItemDetailsObj.BatchName = string.IsNullOrEmpty(ddlBatchNogrid.SelectedItem.Text) ? string.Empty : Convert.ToString(ddlBatchNogrid.SelectedItem.Text);
                                ItemDetailsObj.BatchStock = string.IsNullOrEmpty(lblCurrentStock.Text) ? 0 : Convert.ToDecimal(lblCurrentStock.Text);
                                tempStockBatchDetailsList.Add(ItemDetailsObj);
                                StockBatchDetailsList = tempStockBatchDetailsList;
                                DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == dpdSlno).DOStockBatch_Details = StockBatchDetailsList;
                                #endregion
                            }
                            else
                            {
                                lblCurrentStock.Text = string.Empty;
                            }
                            //#region Setting UOM
                            //currentItemId = hdfItem != null ? Convert.ToInt32(hdfItem.Value) : 0;
                            //GetFieldValues(ControlEnums.UOM);
                            //if (dtPageData != null && dtPageData.Rows.Count > 0)
                            //{
                            //    ddlUOMGrid.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, GTIService.Constants.Shipping.Fields.UOM_CODE);
                            //    ddlUOMGrid.DataTextField = GTIService.Constants.Shipping.Fields.UOM_CODE;
                            //    ddlUOMGrid.DataValueField = GTIService.Constants.Shipping.Fields.UOM_PK;
                            //    ddlUOMGrid.DataBind();
                            //    ddlUOMGrid.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                            //    ddlUOMGrid.SelectedValue = (dt != null && dt.Rows.Count > 0) ? dt.Rows[0]["SBD_UOM"].ToString() : "0";
                            //}
                            //#endregion
                        }
                        else if (((DropDownList)sender).ID == "ddlBatchesPopUp")
                        {
                            if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                            {
                                int ToUOMPK = String.IsNullOrEmpty(hdfUomPKPopup.Value) ? 0 : Convert.ToInt32(hdfUomPKPopup.Value);
                                DataTable dtBatch = BatchDetails(Convert.ToInt32(ddlBatchesPopUp.SelectedValue), ToUOMPK);
                                if (dtBatch != null && dtBatch.Rows.Count > 0)
                                {
                                    lblStockPopUp.Text = GetFormattedNumber(Convert.ToDouble(dtBatch.Rows[0]["SBD_STK_TO_UOM"]));
                                    if (!string.IsNullOrEmpty(dtBatch.Rows[0]["SBD_UOM"].ToString()))
                                        hdfBatchPopupUOM.Value = dtBatch.Rows[0]["SBD_UOM"].ToString();
                                }
                            }
                            ShowBatchPopup();
                        }
                        break;
                    #endregion

                    #region BATCH POPUP ACTIONS
                    #region ADD_ACTION (ShowBatchPopup)
                    case ActionsEnum.ADD_ACTION:
                        StockBatchDetailsList = new List<DirectDeliveryOrderStockBatchDetails>();
                        if (((ImageButton)sender).ID == "imbAddGridBatch" || ((ImageButton)sender).ID == "imbViewGridBatch")
                        {
                            GridRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
                            #region Checking UOM is selected or not
                            DropDownList ddlUOMGrid = ((DropDownList)grdDeliveryOrderList.Rows[GridRowIndex].FindControl("ddlUOMGrid"));
                            if (String.IsNullOrEmpty(ddlUOMGrid.SelectedValue) || ddlUOMGrid.SelectedValue == CommonConstants.SELECTVAL)//Checking UOM is selected or not
                            {
                                string str = GetLocalResourceObject("Msg_SelectUOM").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + str + "');", true);
                                return;
                            }
                            #endregion

                            lblStockPopUp.Text = string.Empty;

                            CurrSlNo = Convert.ToInt32(grdDeliveryOrderList.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);

                            DirectDeliveryOrderDetails directdeliveryOrderDetailsObj = DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == CurrSlNo);
                            if (directdeliveryOrderDetailsObj.DOStockBatch_Details != null)
                                StockBatchDetailsList = directdeliveryOrderDetailsObj.DOStockBatch_Details.ToList();

                            SetFieldValues(ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL);

                            HiddenField hdfIsMultiple = ((HiddenField)grdDeliveryOrderList.Rows[GridRowIndex].FindControl("hdfIsMultiple"));
                            Label lblItemDOList = ((Label)grdDeliveryOrderList.Rows[GridRowIndex].FindControl("lblItemDOList"));

                            lblItemNamePopup.Text = lblItemDOList.Text;
                            lblStockPopUp.Text = string.Empty;
                            lblUomNamePopup.Text = ddlUOMGrid.SelectedItem.Text;
                            hdfUomPKPopup.Value = ddlUOMGrid.SelectedValue.ToString();
                            selectedItem = Convert.ToInt32(((HiddenField)grdDeliveryOrderList.Rows[GridRowIndex].FindControl("hdfItemIdDOList")).Value);
                            TextBox txtDespatchNowQty = ((TextBox)grdDeliveryOrderList.Rows[GridRowIndex].FindControl("txtDespatchNowQty"));
                            if ((((HiddenField)grdDeliveryOrderList.Rows[GridRowIndex].FindControl("hdfcategory"))).Value != CommonConstants.SELECTVAL)
                            {
                                #region ddlBatch Popup Binding
                                DataTable dtResult2 = new DataTable();
                                dtResult2 = DirectDeliveryOrderBL.GetBatchNo(selectedItem, currentUser.CurrentDeptPK, 0, null, 0);//get batch DropDown in Popup   
                                ddlBatchesPopUp.Items.Clear();
                                if (dtResult2 != null && dtResult2.Rows.Count > 0)
                                {
                                    ddlBatchesPopUp.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult2, GTIService.Constants.Shipping.Fields.SBD_BATCH_NO);
                                    ddlBatchesPopUp.DataTextField = GTIService.Constants.Shipping.Fields.SBD_BATCH_NO;
                                    ddlBatchesPopUp.DataValueField = GTIService.Constants.Shipping.Fields.SBD_PK;
                                    ddlBatchesPopUp.DataBind();
                                    ddlBatchesPopUp.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                }
                                else
                                {
                                    ddlBatchesPopUp.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                }
                                #endregion
                            }
                        }
                        txtQtyPopUp.Focus();
                        ResetForm(ControlEnums.CLEARBATCHPOPUP);
                        ShowBatchPopup();
                        break;
                    #endregion

                    #region ADDBATCHES (Plus Button Click on Batch PoPup)
                    case ActionsEnum.ADDBATCHES:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            ShowBatchPopup();
                            if (!IsBatchAlreadyinPopupGrid())
                            {
                                if (IsCurrentStockAvailableInPopup())
                                {
                                    StockBatchDetailsList = (List<DirectDeliveryOrderStockBatchDetails>)SetUIValuesToObject(ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL);
                                    SetFieldValues(ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL);
                                    ResetForm(ControlEnums.CLEARBATCHPOPUP);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorStock", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(this.GetLocalResourceObject("InsufficientStockQuantity").ToString()) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorStock", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(this.GetLocalResourceObject("BatchDuplication").ToString()) + "','" + Resources.Captions.Information + "');", true);
                            }

                        }
                        break;
                    #endregion
                    #region APPLY(Apply Batches)
                    case ActionsEnum.APPLY:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            if (DirectDeliveryOrderDetailList != null)
                            {
                                DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == CurrSlNo).DOStockBatch_Details = StockBatchDetailsList;
                            }

                            if (StockBatchDetailsList.Count > 1)
                            {
                                SetMultiBatchForGrid(true, false);
                            }
                            else
                            {
                                SetMultiBatchForGrid(false, false);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();", true);
                        break;
                    #endregion

                    #region DELETE_ACTION (Batch Popup Delete)
                    case ActionsEnum.DELETE_ACTION:
                        if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                        {
                            gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                            if (((ImageButton)sender).ID == "btnDeletepopupBatches")
                            {
                                if (StockBatchDetailsList != null)
                                    StockBatchDetailsList.Remove(StockBatchDetailsList[gvrTemplate.RowIndex]);
                                SetFieldValues(ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL);
                                ResetForm(ControlEnums.CLEARBATCHPOPUP);
                                ShowBatchPopup();
                                ddlBatchesPopUp.Focus();
                            }
                        }
                        break;
                    #endregion
                    #endregion

                    #region RESETQTY (Convert all qty to Selected DO UOM qty)
                    case ActionsEnum.RESETQTY:
                        if (((DropDownList)sender).ID == "ddlUOMGrid")
                        {
                            gvrTemplate = ((DropDownList)sender).Parent.Parent as GridViewRow;
                            Label lblCurrentStock = (Label)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("lblCurrentStock");
                            DropDownList ddlUOMGrid = ((DropDownList)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("ddlUOMGrid"));
                            HiddenField hdfDPDUOMPK = ((HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfDPDUOMPK"));
                            HiddenField hdfDPDCurrentStock = ((HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfDPDCurrentStock"));
                            Label lblSOQtyDOList = ((Label)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("lblSOQtyDOList"));
                            HiddenField hdfSOQty = ((HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfSOQty"));
                            Label lblDespatchedQtyDOList = ((Label)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("lblDespatchedQtyDOList"));
                            HiddenField hdfDespatchedQty = ((HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfDespatchedQty"));
                            TextBox txtDespatchNowQty = ((TextBox)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("txtDespatchNowQty"));
                            HiddenField hdfItem = (HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfItemIdDOList");
                            DropDownList ddlBatchNogrid = (DropDownList)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("ddlBatchNogrid");
                            HiddenField hdfDPD_SL_NO = (HiddenField)grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("hdfDPD_SL_NO");

                            /*Buttons for add view clear multiple batches in grid*/
                            ImageButton imbAddGridBatch = grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("imbAddGridBatch") as ImageButton;
                            ImageButton imbViewGridBatch = grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("imbViewGridBatch") as ImageButton;
                            ImageButton imbClearGridBatch = grdDeliveryOrderList.Rows[gvrTemplate.RowIndex].FindControl("imbClearGridBatch") as ImageButton;


                            //Clear StockBatchDetailList(Popup).Because already added qty are related to previous UOM
                            #region Clearing StockBatchDetails
                            int dpdSlno = string.IsNullOrEmpty(hdfDPD_SL_NO.Value) ? 0 : Convert.ToInt32(hdfDPD_SL_NO.Value);

                            if (DirectDeliveryOrderDetailList != null && DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == dpdSlno).DOStockBatch_Details != null)
                            {
                                DataTable dtBatches = DirectDeliveryOrderBL.GetBatchNo(Convert.ToInt32(hdfItem.Value), currentUser.CurrentDeptPK, 0, null, 0);
                                ddlBatchNogrid.Items.Clear();
                                if (dtBatches != null && dtBatches.Rows.Count > 0)
                                {
                                    ddlBatchNogrid.DataSource = CommonFunctions.HtmlDecodeDataTable(dtBatches, GTIService.Constants.Shipping.Fields.SBD_BATCH_NO);
                                    ddlBatchNogrid.DataTextField = GTIService.Constants.Shipping.Fields.SBD_BATCH_NO;
                                    ddlBatchNogrid.DataValueField = GTIService.Constants.Shipping.Fields.SBD_PK;
                                    ddlBatchNogrid.DataBind();
                                    ddlBatchNogrid.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                    ddlBatchNogrid.SelectedValue = CommonConstants.SELECTVAL;
                                }
                                ddlBatchNogrid.Enabled = true;
                                DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == dpdSlno).DOStockBatch_Details.Clear();
                                imbClearGridBatch.Visible = false;
                            }
                            #endregion

                            DataTable dt = new DataTable();
                            if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                            {
                                int itemPk = string.IsNullOrEmpty(hdfItem.Value) ? 0 : Convert.ToInt32(hdfItem.Value);
                                decimal uomConversionFactor = 0;
                                FromUOMPK = string.IsNullOrEmpty(hdfDPDUOMPK.Value) ? 0 : Convert.ToInt32(hdfDPDUOMPK.Value);//In New mode this is SO UOM. Edit mode saved DO UOm
                                ToUOMPK = string.IsNullOrEmpty(ddlUOMGrid.SelectedValue) ? 0 : Convert.ToInt32(ddlUOMGrid.SelectedValue);
                                uomConversionFactor = BusinessLogic.CommonManagement.CommonBL.GetUOMConversionFactor(itemPk, FromUOMPK, ToUOMPK);
                                if (uomConversionFactor > 0)
                                {
                                    decimal convertedSOQty = Convert.ToDecimal(hdfSOQty.Value) * uomConversionFactor;
                                    decimal convertedDespatchedQty = Convert.ToDecimal(hdfDespatchedQty.Value) * uomConversionFactor;
                                    decimal convertedDespatchNowQty = Convert.ToDecimal(hdfSOQty.Value) * uomConversionFactor;

                                    lblSOQtyDOList.Text = GetFormattedNumber(convertedSOQty);
                                    lblDespatchedQtyDOList.Text = GetFormattedNumber(convertedDespatchedQty);
                                    txtDespatchNowQty.Text = string.Empty;//GetFormattedNumber(convertedDespatchNowQty);

                                    if (!string.IsNullOrEmpty(ddlBatchNogrid.SelectedValue))
                                    {
                                        int batchPk = string.IsNullOrEmpty(ddlBatchNogrid.SelectedValue) ? 0 : Convert.ToInt32(ddlBatchNogrid.SelectedValue);
                                        dt = BatchDetails(batchPk, ToUOMPK);
                                        lblCurrentStock.Text = dt.Rows.Count > 0 ? GetFormattedNumber((Convert.ToDouble(dt.Rows[0]["SBD_STK_TO_UOM"]))) : "0";
                                    }
                                    else
                                    {
                                        transDate = Convert.ToDateTime(txtDODate.Text);
                                        DataTable dtStock = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetCurrentStoreStock(itemPk, currentStore, transDate, ToUOMPK);
                                        if (dtStock != null && dtStock.Rows.Count > 0)
                                        {
                                            lblCurrentStock.Text = GetFormattedNumber(dtStock.Rows[0]["STD_QTY_IN_STOCK_TO_UOM"]);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion


                    case ActionsEnum.QTYCHANGE:
                        foreach (GridViewRow row in grdDeliveryOrderList.Rows)
                        {
                            TextBox txtDespatchNowQty = row.FindControl("txtDespatchNowQty") as TextBox;
                            HiddenField hdfPODetIdDOList = row.FindControl("hdfPODetIdDOList") as HiddenField;
                            HiddenField hdfPOIdDOList = row.FindControl("hdfPOIdDOList") as HiddenField;
                            TextBox txtNetW = row.FindControl("txtNetWeight") as TextBox;
                            TextBox txtGrossW = row.FindControl("txtGrossWeight") as TextBox;
                            DataTable dtDtls = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanWeightDetails(Convert.ToInt32(hdfPOIdDOList.Value), string.Empty, Convert.ToDouble(txtDespatchNowQty.Text), Convert.ToInt32(hdfPODetIdDOList.Value));
                            if (dtDtls != null & dtDtls.Rows.Count > 0)
                            {
                                txtNetW.Text = String.Format("{0:N}", decimal.Parse(dtDtls.Rows[0]["SND_QTY_NET_WT"].ToString()));
                                txtGrossW.Text = String.Format("{0:N}", decimal.Parse(dtDtls.Rows[0]["SND_QTY_GROSS_WT"].ToString()));
                            }
                        }
                        grdDeliveryOrderList.Focus();
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        public void ShowCartonPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ClosePopup();ShowAllocationPopup('" + GetLocalResourceObject("CartonDetails").ToString() + "');", true);
        }
        private void SaveTransaction(DirectDeliveryOrderHeader directDOHeader, int workflowFlag)
        {
            XmlDocument xmlDoc;
            List<object> lstResult = new List<object>();
            int result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string action = string.Empty;
            StringBuilder detailedValMsgWkflow = new StringBuilder();
            if (directDOHeader == null)
                directDOHeader = new DirectDeliveryOrderHeader();

            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            directDOHeader.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            directDOHeader.WKF_APPLICATION = CurrPK;
            directDOHeader.WKF_COMMENTS = wkfDetails.Comments;
            directDOHeader.WKF_TRX_FLAG = workflowFlag;
            directDOHeader.WKF_PROCESS = wkfDetails.ProcessID;
            directDOHeader.WKF_REFERENCE = wkfDetails.ReferenceID;
            directDOHeader.WKF_TASK = wkfDetails.TaskID;
            directDOHeader.WKF_TASK_ACTION = wkfDetails.ActionID;
            directDOHeader.WKF_FLAG = 1;
            action = wkfDetails.ActionText;
            #endregion

            string strTrxNo = string.Empty;
            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(directDOHeader);
            DataTable dtOut = null;
            lstResult = BusinessLogic.Shipping.DirectDeliveryOrderBL.SaveDirectDeliveryOrder(xmlDoc.InnerXml, ref dtOut);
            result = Convert.ToInt32(lstResult[0]);
            strTrxNo = lstResult[1].ToString();
            if (result > 0)
            {
                if (string.IsNullOrEmpty(strTrxNo))
                    strTrxNo = lblDeliveryOrderNo.Text.Trim();

                if (isCancelled)
                {
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_SubmitSuccess").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder, strTrxNo);
                }
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ResetForm(ControlEnums.CLEAR);
                    ResetForm(ControlEnums.CLEARSEARCH);
                    FillProcessID(0, 1);
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                }
                ucrWrkf.ApplicationID = CurrPK = result;
            }
            else
            {
                #region Validation From SQL
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.DirectDeliveryOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == -30)//NotEnoughStock
                {
                    detailedValMsgWkflow.Append(this.GetLocalResourceObject("Msg_NotEnoughStocks").ToString());
                    if (dtOut != null)
                    {
                        foreach (DataRow item in dtOut.Rows)
                        {
                            detailedValMsgWkflow.Append("<ul><li>" + item[0].ToString() + " : " + item[1].ToString().Replace("'", "") + "</li></ul>");
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowMessageFixed('" + detailedValMsgWkflow.ToString()
                    + "','" + Resources.ErpRes.Information + "');", true);
                    return;
                }
                else if (result == -31)///Despatch Now Quantity Exceeds. Do you want to continue?
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DespatchNowExceeds", "ClosePopup();$(document).ready(function(){ShowDespatchNowQtyExceeds(2);});", true);
                    return;
                }
                else if (result == -35)
                {
                    litErrorMsg.Text = GetLocalResourceObject("InsufficientStockQuantity").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -36)
                {
                    litErrorMsg.Text = GetLocalResourceObject("CannotModifyHaveReference").ToString();//Cannot modify,some of the items were referenced in other pages
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -40)//The following stock batch qty is already used in another DO.
                {
                    detailedValMsgWkflow.Append(this.GetLocalResourceObject("AlreadyUsedInAnotherDO").ToString());
                    if (dtOut != null)
                    {
                        foreach (DataRow item in dtOut.Rows)
                        {
                            detailedValMsgWkflow.Append("<ul><li>" + item[0].ToString() + " : " + item[1].ToString().Replace("'", "") + "</li></ul>");
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowMessageFixed('" + detailedValMsgWkflow.ToString()
                    + "','" + Resources.ErpRes.Information + "');", true);
                    return;
                }
                else if (result == -42)//Different Type 
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + GetLocalResourceObject("Err_ItemType_differ").ToString()
                    + "','" + Resources.ErpRes.Information + "');", true);
                    return;
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectDeliveryOrder);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.Captions.Information + "');", true);
                    return;
                }
                #endregion
            }
        }

        #region --- For Grid Actions----
        /// <summary>ON SORTING
        /// Sorting Event Handler for grd AssignProducts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            List<DirectDeliveryOrderDetails> tempDirectDeliveryOrderDetailsList;
            GridView senderGridView = (GridView)sender;

            if (senderGridView.ID == "grdCartons")
            {
                if (e.CommandName == "REMOVEITEM")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfCRC_SL_NO_GRP = row.FindControl("hdfCRC_SL_NO_GRP") as HiddenField;
                    int slNoGrp = GetNullableInt(hdfCRC_SL_NO_GRP.Value).Value;
                    List<CartonDetails> tempCartons = CartonsListMapped;
                    //  tempCartons.RemoveAll(x => x.SL_NO_GRP == slNoGrp);
                    List<CartonsGridview> tempCartonsGridview = CartonsGridviewListViewState;
                    CartonsGridview cGridview = tempCartonsGridview
                        .Where(x => x.CRC_SL_NO_GRP == slNoGrp)
                        .Single();
                    tempCartonsGridview.Remove(cGridview);
                    CartonsGridviewListViewState = tempCartonsGridview;
                    CartonsListMapped = tempCartons;
                    BindGrid(ControlEnums.CONTAINERSPLITUP);
                    ShowCartonPopup();
                    chkAutoMode.Checked = false;
                    txtCartonPrefixPopUp.Focus();
                }

                else if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfCRC_SL_NO_GRP = row.FindControl("hdfCRC_SL_NO_GRP") as HiddenField;
                    int slNoGrp = GetNullableInt(hdfCRC_SL_NO_GRP.Value).Value;
                    StringBuilder sb = new StringBuilder();
                    //foreach (var item in CartonsListMapped
                    //    .Where(x => x.CRC_SL_NO_GRP == slNoGrp)
                    //    )
                    //{
                    //    sb.Append(item.BCR_NO + ", ");
                    //}
                    foreach (var item in CartonsListMapped)
                    {
                        sb.Append(item.DSC_CARTON_NO + ", ");
                    }
                    sb.Remove(sb.Length - 2, 2);
                    //divMsgCartonDtlsPopUpDetail.InnerText = sb.ToString();
                    hdfCartons.Value = sb.ToString();
                    ShowCartonPopup();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnShowCartons", "fnShowCartons();", true);
                    //ShowCartonPopupDetail();
                }
                else if (e.CommandName == "REMOVEITEMALL")
                {
                    List<CartonDetails> tempCartons = CartonsListMapped;
                    tempCartons = new List<CartonDetails>();
                    List<CartonsGridview> tempCartonsGridview = CartonsGridviewListViewState;
                    tempCartonsGridview = new List<CartonsGridview>();
                    CartonsGridviewListViewState = tempCartonsGridview;
                    CartonsListMapped = tempCartons;
                    BindGrid(ControlEnums.CONTAINERSPLITUP);
                    ShowCartonPopup();
                    chkAutoMode.Checked = false;
                    txtCartonPrefixPopUp.Focus();
                }
            }
            else if (senderGridView.ID == "grdSOList")
            {

                if (e.CommandName == "ALLOCATION_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    lblSCNoPopUp.Text = ((Label)row.FindControl("lblSONOList")).Text;
                    lblDONoPopUp.Text = ((HiddenField)row.FindControl("hdfDPHNO")).Value;
                    string ordrPcs = ((HiddenField)row.FindControl("hdfDpdTotalPcs")).Value;
                    OrderTotalPCS = Convert.ToDecimal(string.IsNullOrEmpty(ordrPcs) ? "0" : ordrPcs);
                    lblDOQtyPcs.Text = ((Label)row.FindControl("lblSCDespQty")).Text + " (" + OrderTotalPCS + " "+GetLocalResourceObject("Pcs").ToString()+")";
                    lblBrandNamePopUp.Text = ((Label)row.FindControl("lblItemName")).Text;
                    lblCartonCount.Text = ((Label)row.FindControl("lbNoOfBag")).Text;
                    HiddenField hdfBrand = new System.Web.UI.WebControls.HiddenField();
                    string strBrand = ((HiddenField)row.FindControl("hdfItemPK")).Value;
                    BrandPK = string.IsNullOrEmpty(strBrand) ? 0 : Convert.ToInt32(strBrand);
                    //BrandPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfItemPK")).Value);
                    DphPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfDphPK")).Value);
                    DetailPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfDpdPK")).Value);
                    CartonCount = Convert.ToDecimal(lblCartonCount.Text.Trim());
                    DataTable dtAlcDetails = DirectDeliveryOrderBL.GetAllocateCartonDODetails(DetailPK);
                    CartonsGridviewListViewState = new List<CartonsGridview>();
                    CartonsListMapped = new List<CartonDetails>();
                    int slNo = 0;
                    List<CartonDetails> tempList1 = new List<CartonDetails>();
                    foreach (DataRow dr in dtAlcDetails.Rows)
                    {
                        CartonDetails temp = new CartonDetails();
                        temp.DSC_PK = Convert.ToInt32(dr["DSC_PK"]);
                        temp.DSC_CARTON_MST = Convert.ToInt32(dr["DSC_CARTON_MST"]);
                        temp.DSC_CARTON_NO = Convert.ToString(dr["DSC_CARTON_NO"]);
                        temp.DSC_QTY_DESPATCHED = Convert.ToInt32(dr["DSC_QTY_DESPATCHED"]);
                        temp.SL_NO = ++slNo;
                        tempList1.Add(temp);
                    }
                    List<CartonsGridview> tempCartonsGridview = new List<CartonsGridview>();
                    if (tempList1 != null && tempList1.Count > 0)
                    {
                        CartonsGridview temp1 = new CartonsGridview();
                        temp1.BCR_PALLET_NO = GetLocalResourceObject("Carton").ToString();
                        temp1.CartonsCount = tempList1.Count;
                        temp1.QtyPcs = Convert.ToInt32(tempList1.Sum(x => x.DSC_QTY_DESPATCHED));
                        temp1.BCR_LOCATION_TEXT = tempList1[0].DSC_LOCATION_TEXT;
                        tempCartonsGridview.Add(temp1);
                        CartonsGridviewListViewState = tempCartonsGridview;
                        List<CartonDetails> tempList = CartonsListMapped;
                        tempList.AddRange(tempList1);
                        CartonsListMapped = tempList;
                    }

                    BindGrid(ControlEnums.CONTAINERSPLITUP);
                    BindGrid(ControlEnums.CONTAINERSPLITUP);
                    chkAutoMode.Checked = false;
                    btnApplyCartonDetails.Focus();
                    ShowCartonPopup();
                    txtCartonPrefixPopUp.Focus();

                }
            }
            if (senderGridView.ID == "grdDeliveryOrderList")
            {
                if (e.CommandName == "RemoveDOItem")
                {
                    UpdateDirectDeliveryOrderDetailListFromGridview(false);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPkDOList = row.FindControl("hdfPkDOList") as HiddenField;
                    HiddenField hdfPODetIdDOList = row.FindControl("hdfPODetIdDOList") as HiddenField;
                    int pk = (GetNullableInt(hdfPkDOList.Value) ?? 0);
                    int soDetId = (GetNullableInt(hdfPODetIdDOList.Value) ?? 0);
                    tempDirectDeliveryOrderDetailsList = DirectDeliveryOrderDetailList;
                    DirectDeliveryOrderDetails tempDODetailItem = tempDirectDeliveryOrderDetailsList
                        .Where(x => x.DPD_PK == pk && x.DPD_SO_DTL == soDetId)
                        .Single();

                    tempDirectDeliveryOrderDetailsList.Remove(tempDODetailItem);

                    List<PendingSO> tempPendingSOList = PendingSOList;
                    tempPendingSOList.
                        Where(x => x.SODetId == soDetId)
                       .ToList()
                       .ForEach(l => { l.AddedToStockList = false; l.CheckBoxChecked = false; });

                    PendingSOList = tempPendingSOList;
                    DirectDeliveryOrderDetailList = tempDirectDeliveryOrderDetailsList;
                    BindGrid(ControlEnums.PendingSOs);
                    BindGrid(ControlEnums.DIRECTDELIVERYORDERLIST);
                }
            }

        }
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
            {
                #region grdDeliveryOrderList
                if (((GridView)sender).ID == "grdDeliveryOrderList")
                {
                    DataTable dtBatches;
                    HiddenField hdfcategory = e.Row.FindControl("hdfcategory") as HiddenField;
                    HiddenField hdfIsMultiple = e.Row.FindControl("hdfIsMultiple") as HiddenField;
                    HiddenField hdfBatchGroup = e.Row.FindControl("hdfBatchGroup") as HiddenField;
                    DropDownList ddlBatchNogrid = e.Row.FindControl("ddlBatchNogrid") as DropDownList;
                    HiddenField hdfItemIdDOList = e.Row.FindControl("hdfItemIdDOList") as HiddenField;
                    HiddenField hdfPOIdDOList = e.Row.FindControl("hdfPOIdDOList") as HiddenField;
                    HiddenField hdfPODetIdDOList = e.Row.FindControl("hdfPODetIdDOList") as HiddenField;
                    Label lblCurrentStock = e.Row.FindControl("lblCurrentStock") as Label;
                    Label ltQtyRequired = e.Row.FindControl("ltQtyRequired") as Label;
                    HiddenField hdfDPD_SL_NO = e.Row.FindControl("hdfDPD_SL_NO") as HiddenField;
                    DropDownList ddlUOMGrid = e.Row.FindControl("ddlUOMGrid") as DropDownList;
                    HiddenField hdfItmNeedBatchStkDOList = e.Row.FindControl("hdfItmNeedBatchStkDOList") as HiddenField;
                    HiddenField hdfDPDCurrentStock = e.Row.FindControl("hdfDPDCurrentStock") as HiddenField;
                    /*Buttons for add view clear multiple batches in grid*/
                    ImageButton imbAddGridBatch = e.Row.FindControl("imbAddGridBatch") as ImageButton;
                    ImageButton imbViewGridBatch = e.Row.FindControl("imbViewGridBatch") as ImageButton;
                    ImageButton imbClearGridBatch = e.Row.FindControl("imbClearGridBatch") as ImageButton;
                    TextBox txtDespatchNowQty = e.Row.FindControl("txtDespatchNowQty") as TextBox;
                    HiddenField hdfProductType = e.Row.FindControl("hdfProductType") as HiddenField;
                    imbAddGridBatch.Visible = false;
                    imbViewGridBatch.Visible = false;
                    imbClearGridBatch.Visible = false;

                    currentItemId = hdfItemIdDOList != null ? Convert.ToInt32(hdfItemIdDOList.Value) : 0;
                    currentStore = string.IsNullOrEmpty(ddlDeptStore.SelectedValue) ? 0 : Convert.ToInt32(ddlDeptStore.SelectedValue);
                    transDate = Convert.ToDateTime(txtDODate.Text);
                    int dpdSlno = 0, isHaveMulipleBatch = 0;
                    dpdSlno = Convert.ToInt32(hdfDPD_SL_NO.Value);
                    if (hdfEnableBatch.Value == "0" || hdfItmNeedBatchStkDOList.Value == "0")
                    {
                        #region Setting CurrentStock
                        if (!string.IsNullOrEmpty(hdfDPDCurrentStock.Value) && hdfDPDCurrentStock.Value != "0")
                        {
                            lblCurrentStock.Text = GetFormattedNumber(hdfDPDCurrentStock.Value);
                        }
                        else
                        {
                            GetFieldValues(ControlEnums.CURRENTSTORESTOCK);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                lblCurrentStock.Text = GetFormattedNumber(dtPageData.Rows[0]["STD_QTY_IN_STOCK_TO_UOM"]);
                            }
                        }
                        #endregion
                        ddlBatchNogrid.Enabled = false;
                    }
                    else
                    {
                        #region Batch Region
                        List<DirectDeliveryOrderStockBatchDetails> directDOStockBatchDetList = new List<DirectDeliveryOrderStockBatchDetails>();

                        if (DirectDeliveryOrderDetailList != null)
                        {
                            directDOStockBatchDetList = DirectDeliveryOrderDetailList.SingleOrDefault(c => c.DPD_SL_NO == dpdSlno).DOStockBatch_Details;
                            if (directDOStockBatchDetList != null)
                            {
                                if (directDOStockBatchDetList.Count > 1)
                                    isHaveMulipleBatch = 1;
                            }
                        }
                        if (isHaveMulipleBatch == 1)
                        {
                            imbClearGridBatch.Visible = true;
                            imbAddGridBatch.Visible = true;
                            imbAddGridBatch.ToolTip = this.GetLocalResourceObject("Msg_ModifyBatches").ToString();
                            ddlBatchNogrid.Items.Clear();
                            ddlBatchNogrid.Items.Insert(0, new ListItem(CommonConstants.MULTIBATCH, CommonConstants.MULTIBATCH_VALUE));
                            ddlBatchNogrid.Enabled = false;
                            if (DirectDeliveryOrderHeaderSession.DPH_STATUS == 2)
                            {
                                imbClearGridBatch.Visible = false;
                                imbAddGridBatch.Visible = false;
                                imbViewGridBatch.Visible = true;
                            }
                            //Multiple Batch Stock Setting
                            if (directDOStockBatchDetList != null)
                            {
                                lblCurrentStock.Text = GetFormattedNumber(directDOStockBatchDetList.Sum(f => f.BatchStock).ToString());
                            }

                        }
                        else
                        {
                            int selectedBatchPk = directDOStockBatchDetList != null ? Convert.ToInt32(directDOStockBatchDetList[0].SIC_STK_BATCH) : 0;
                            dtBatches = DirectDeliveryOrderBL.GetBatchNo(Convert.ToInt32(hdfItemIdDOList.Value), currentUser.CurrentDeptPK, selectedBatchPk, null, 0);
                            ddlBatchNogrid.Items.Clear();
                            if (dtBatches != null && dtBatches.Rows.Count > 0)
                            {
                                ddlBatchNogrid.DataSource = CommonFunctions.HtmlDecodeDataTable(dtBatches, GTIService.Constants.Shipping.Fields.SBD_BATCH_NO);
                                ddlBatchNogrid.DataTextField = GTIService.Constants.Shipping.Fields.SBD_BATCH_NO;
                                ddlBatchNogrid.DataValueField = GTIService.Constants.Shipping.Fields.SBD_PK;
                                ddlBatchNogrid.DataBind();
                                ddlBatchNogrid.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                ddlBatchNogrid.SelectedValue = selectedBatchPk.ToString();
                                //if (ddlBatchNogrid.SelectedValue != CommonConstants.SELECTVAL)
                                //{
                                //    DataTable dt = BatchDetails(Convert.ToInt32(ddlBatchNogrid.SelectedValue));
                                //    if (dt != null && dt.Rows.Count > 0)
                                //    {
                                //        lblCurrentStock.Text = GetFormattedNumber(dt.Rows[0]["SBD_QTY_IN_STOCK"]);                                        
                                //    }
                                //}
                                //else
                                //{
                                //    lblCurrentStock.Text = "0";                                    
                                //}
                                lblCurrentStock.Text = directDOStockBatchDetList != null ? GetFormattedNumber(directDOStockBatchDetList[0].BatchStock) : "0";
                            }
                            else
                            {
                                ddlBatchNogrid.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                                lblCurrentStock.Text = "0";
                            }
                            if (CurrPK == 0)
                            {
                            }

                        }
                        if (ddlBatchNogrid.Items.Count > 1)// && (TempObjCompoundingPreparation.CthStatus != 2 || hdfEditFlag.Value == "1"))
                            imbAddGridBatch.Visible = true;
                        #endregion
                    }
                    #region Setting UOM
                    GetFieldValues(ControlEnums.UOM);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlUOMGrid.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, GTIService.Constants.Shipping.Fields.UOM_CODE);
                        ddlUOMGrid.DataTextField = GTIService.Constants.Shipping.Fields.UOM_CODE;
                        ddlUOMGrid.DataValueField = GTIService.Constants.Shipping.Fields.UOM_PK;
                        ddlUOMGrid.DataBind();
                        ddlUOMGrid.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        ddlUOMGrid.SelectedValue = DirectDeliveryOrderDetailList != null ? DirectDeliveryOrderDetailList.SingleOrDefault(c => c.DPD_SL_NO == dpdSlno).DPD_UOM.ToString() : "0";
                    }
                    #endregion
                    #region Net Weight/Gross Weight
                    if (IsShowNetGrsWeight)
                    {
                        grdDeliveryOrderList.Columns[10].Visible = true;
                        grdDeliveryOrderList.Columns[11].Visible = true;
                        TextBox txtNetW = e.Row.FindControl("txtNetWeight") as TextBox;
                        TextBox txtGrossW = e.Row.FindControl("txtGrossWeight") as TextBox;
                        DataTable dtResult = BusinessLogic.Shipping.ShippingPlanBL.GetShippingPlanWeightDetails(Convert.ToInt32(hdfPOIdDOList.Value), string.Empty, Convert.ToDouble(txtDespatchNowQty.Text), Convert.ToInt32(hdfPODetIdDOList.Value));
                        if (dtResult != null & dtResult.Rows.Count > 0)
                        {
                            txtNetW.Text = String.Format("{0:N}", decimal.Parse(dtResult.Rows[0]["SND_QTY_NET_WT"].ToString()));
                            txtGrossW.Text = String.Format("{0:N}", decimal.Parse(dtResult.Rows[0]["SND_QTY_GROSS_WT"].ToString()));
                        }
                    }
                    else
                    {
                        grdDeliveryOrderList.Columns[10].Visible = false;
                        grdDeliveryOrderList.Columns[11].Visible = false;
                    }
                    //For Edit Mode
                    if (EntryStatus != EntryStatus.NEWMODE && DirectDeliveryOrderHeaderSession != null && DirectDeliveryOrderHeaderSession.DirectDODetailsList != null && DirectDeliveryOrderHeaderSession.DirectDODetailsList.Count > 0)
                    {
                        if (IsShowNetGrsWeight)
                        {
                            grdDeliveryOrderList.Columns[10].Visible = true;
                            grdDeliveryOrderList.Columns[11].Visible = true;
                            TextBox txtNetW = e.Row.FindControl("txtNetWeight") as TextBox;
                            TextBox txtGrossW = e.Row.FindControl("txtGrossWeight") as TextBox;
                            txtNetW.Text = String.Format("{0:N}", DirectDeliveryOrderHeaderSession.DirectDODetailsList[e.Row.RowIndex].DPD_NET_WT);
                            txtGrossW.Text = String.Format("{0:N}", DirectDeliveryOrderHeaderSession.DirectDODetailsList[e.Row.RowIndex].DPD_GROSS_WT);
                        }
                        else
                        {
                            grdDeliveryOrderList.Columns[10].Visible = false;
                            grdDeliveryOrderList.Columns[11].Visible = false;
                        }
                    }
                    #endregion

                    if (hdfProductType.Value == "9")
                    {
                        grdDeliveryOrderList.Columns[4].Visible = false;
                        grdDeliveryOrderList.Columns[5].Visible = false;
                        grdDeliveryOrderList.Columns[6].Visible = false;
                    }
                    else
                    {
                        grdDeliveryOrderList.Columns[4].Visible = true;
                        grdDeliveryOrderList.Columns[5].Visible = true;
                        grdDeliveryOrderList.Columns[6].Visible = true;
                    }
                }
                #endregion
                else if (((GridView)sender).ID == "grdPendingSOs")
                {
                    #region grdPendingSOs
                    //selectedRowColor
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfSOPendingListSelected = e.Row.FindControl("hdfSOPendingListSelected") as HiddenField;
                        if (hdfSOPendingListSelected.Value == "True")
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());

                        }
                    }
                    #endregion
                }
            }
            #region grdBatchDetailsPopup
            if (((GridView)sender).ID == "grdBatchDetailsPopup")
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Label lblPopupFooterTotal = (Label)e.Row.FindControl("lblPopupFooterTotal");
                    //lblPopupFooterTotal.Text = GetFormattedNumber(StockBatchDetailsList.Sum(f => Convert.ToDouble(f.SIC_QTY_CONSUMED)));
                }
            }
            #endregion
        }

        #endregion
        #endregion

        #region Custom Pager Control Navigated Event
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndexList = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }
        #endregion

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
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
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlEnums type)
        {
            ServiceUtility serviceUtilityObj;
            BusinessObject.GridPrams gridParam;
            string pageUrl = string.Empty;
            try
            {
                switch (type)
                {
                    #region Company
                    case ControlEnums.COMPANY:
                        AdmCompanyMstService admCompanyMstServiceClient = new AdmCompanyMstService();
                        ADM_COMPANY_MST admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    #region CUSTOMER
                    case ControlEnums.CUSTOMER:
                        dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomer(customerPk, string.Empty, currentUser.SBUID, 2);
                        break;
                    #endregion
                    #region DEPARTMENTSTORE
                    case ControlEnums.DEPARTMENTSTORE:
                        dtPageData = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetDeptStores(currentUser, currentUser.SBUID, 0, 0);
                        break;
                    #endregion
                    #region LIST
                    case ControlEnums.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.FromDate = txtFromDate.Text;
                        gridParam.ToDate = txtToDate.Text;
                        gridParam.FilterStatus = ddlStatus.SelectedValue == "-1" ? string.Empty : ddlStatus.SelectedValue;
                        // processId = GetNullableInt(hdfProcessID.Value) ?? 0;
                        pageUrl = Resources.PageURL.DirectDeliveryOrderURL.Replace("~", "");
                        string doNumber = txtDeliveryOrderNoSearch.Text.Trim() == "Select/Type" ? string.Empty : txtDeliveryOrderNoSearch.Text.Trim();
                        string customerPK = txtCustomerSearch.Text.Trim() == "Select/Type" || txtCustomerSearch.Text.Trim() == string.Empty ? "0" : hdfCustomerPKSearch.Value;
                        dsPageData = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetDirectDOGetList(gridParam, currentUser, doNumber, Convert.ToInt32(customerPK));
                        break;
                    #endregion
                    #region PendingSOs
                    case ControlEnums.PendingSOs:
                        customerPk = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        int sohPk = 0, doPk = 0;
                        string itemName = string.Empty;
                        string sohNo = string.Empty;
                        doPk = CurrPK;
                        if (SearchType.SelectedValue == "SOH_NO")
                        {
                            sohNo = SearchValue.Text.Trim() == "Select/Type" ? string.Empty : SearchValue.Text.Trim();
                        }
                        else if (SearchType.SelectedValue == "ITM_NAME")
                        {
                            itemName = SearchValue.Text.Trim() == "Select/Type" ? string.Empty : SearchValue.Text.Trim();
                        }
                        gridParam = new GridPrams();
                        gridParam.FromDate = FromDate.Text.Trim();
                        gridParam.ToDate = ToDate.Text.Trim();
                        dtPageData = new DataTable();
                        dtPageData = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetPendingSalesOrder(currentUser.SBUID, 0, customerPk, gridParam, doPk, itemName, sohNo, sohPk);//SPSAL_ORDER_DIR_PEND_GET

                        PendingSOList = dtPageData
                            .AsEnumerable()
                            .Select(x => new PendingSO
                            {
                                SOId = x.Field<int>(GTIService.Constants.Shipping.Fields.SOH_PK),
                                SODetId = x.Field<int>(GTIService.Constants.Shipping.Fields.SOD_PK),
                                SONumber = x.Field<string>(GTIService.Constants.Shipping.Fields.SOH_NO),
                                SODate = x.Field<DateTime>(GTIService.Constants.Shipping.Fields.SOH_DATE),
                                ItemCatId = x.Field<int>(GTIService.Constants.Shipping.Fields.ITM_CATEGORY),
                                ItemId = x.Field<int>(GTIService.Constants.Shipping.Fields.SOD_ITEM),
                                //ItemName = x.Field<string>(GTIService.Constants.Shipping.Fields.SOD_ITEM_TEXT),
                                ItemName = x.Field<string>(GTIService.Constants.Shipping.Fields.SOD_ITEM_NAME),
                                UOMId = x.Field<int>(GTIService.Constants.Shipping.Fields.SOD_UOM),
                                UOM = x.Field<string>(GTIService.Constants.Shipping.Fields.UOM_NAME),
                                SOQty = x.Field<decimal>(GTIService.Constants.Shipping.Fields.SOD_QTY),
                                SOQtyApproved = x.Field<decimal>(GTIService.Constants.Shipping.Fields.SOD_QTY_APPROVED),
                                PreDeliveredQty = x.Field<decimal>(GTIService.Constants.Shipping.Fields.SOD_QTY_DISPATCHED),
                                SOQtyAllocated = x.Field<decimal>(GTIService.Constants.Shipping.Fields.SOD_QTY_ALLOCATED),
                                BalanceQtyToDeliver = x.Field<decimal>(GTIService.Constants.Shipping.Fields.SOD_BAL_TO_DISPATCH),
                                SORate = x.Field<decimal>(GTIService.Constants.Shipping.Fields.SOD_RATE),
                                SOH_CURRENCY = x.Field<int>(GTIService.Constants.Shipping.Fields.SOH_CURRENCY),
                                ITM_NEED_BATCH_STK = x.Field<byte>(GTIService.Constants.Shipping.Fields.ITM_NEED_BATCH_STK),
                                ProductType = x.Field<byte>(GTIService.Constants.Shipping.Fields.SOD_ITEM_CAT_VALUE),
                                ProductTypeCode = x.Field<string>(GTIService.Constants.Shipping.Fields.SOD_ITEM_CAT_CODE),
                                ProductTypeText = x.Field<string>(GTIService.Constants.Shipping.Fields.SOD_ITEM_CAT_TEXT),
                            })
                            .ToList();
                        foreach (var item in PendingSOList)
                        {
                            if (item.BalanceQtyToDeliver < 0) item.BalanceQtyToDeliver = 0;
                        }
                        break;
                    #endregion
                    #region DETAILFOREDIT
                    case ControlEnums.DETAILFOREDIT:
                        string xmlData = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetDirectDOByPk(CurrPK);
                        DirectDeliveryOrderHeader tempDirectDO = CommonFunctions.XmlDeserialize<DirectDeliveryOrderHeader>(xmlData);
                        DirectDeliveryOrderHeaderSession = tempDirectDO;
                        break;
                    #endregion
                    #region SODETAILS
                    case ControlEnums.SODETAILS:
                        dtPageData = new DataTable();
                        dtPageData = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetDirectDOSaleOrderDetailList(grdDPHPK);
                        break;
                    #endregion
                    #region CURRENTSTORESTOCK
                    case ControlEnums.CURRENTSTORESTOCK:
                        dtPageData = new DataTable();
                        dtPageData = BusinessLogic.Shipping.DirectDeliveryOrderBL.GetCurrentStoreStock(currentItemId, currentStore, transDate, ToUOMPK);
                        break;
                    #endregion
                    #region UOM
                    case ControlEnums.UOM:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetUOM(currentItemId, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region SOFROMINBOX
                    case ControlEnums.SOFROMINBOX:
                        DataTable dt = DataAccess.SaleOrder.DirectSaleOrderDL.GetDirectDOSaleOrderDetailList(0,GetNullableInt(SO_PK.Value) ?? 0);
                        dtSos = dt.AsEnumerable()
                            .Where(r => r.Field<byte>("SOD_ITEM_CAT_VALUE") == Convert.ToByte(Request.QueryString["CATVAL"]))
                            .CopyToDataTable();
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

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnums.COMPANY:
                        BindDropDown(ControlEnums.COMPANY);
                        break;
                    case ControlEnums.SUPPLIERS:
                        BindDropDown(ControlEnums.SUPPLIERS);
                        break;
                    case ControlEnums.DEPARTMENTSTORE:
                        BindDropDown(ControlEnums.DEPARTMENTSTORE);
                        break;
                    case ControlEnums.LIST:
                        BindGrid(ControlEnums.LIST);
                        break;
                    case ControlEnums.PendingSOs:
                        BindGrid(ControlEnums.PendingSOs);
                        break;
                    case ControlEnums.DETAILFOREDIT:
                        GetUIValuesFromObject(ControlEnums.DETAILFOREDIT);
                        BindGrid(ControlEnums.DIRECTDELIVERYORDERLIST);
                        BindGrid(ControlEnums.PendingSOs);
                        break;
                    case ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL:
                        BindGrid(ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL);
                        break;
                    #region SOFROMINBOX
                    case ControlEnums.SOFROMINBOX:
                        txtDODate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        if (dtSos != null && dtSos.Rows.Count > 0)
                        {
                            lblDeliveryOrderNo.Text = GetGlobalResourceObject("Messages", "DocGenerationNew").ToString();
                            txtCustomer.Text = HttpUtility.HtmlDecode(dtSos.Rows[0]["SOH_CUSTOMER_TEXT"].ToString());
                            hdfCustomerID.Value = dtSos.Rows[0]["SOH_CUSTOMER"].ToString();
                            ActionHandler(btnCustSelected, EventArgs.Empty);
                            int sohPk;
                            sohPk = GetNullableInt(dtSos.Rows[0]["SOH_PK"].ToString()) ?? 0;
                            foreach (GridViewRow grdrow in grdPendingSOs.Rows)
                            {
                                int soId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOIdPendingList")).Value);
                                int ProductType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfProductType")).Value);
                                if (soId == sohPk)
                                {
                                    CheckBox chk;
                                    chk = (CheckBox)grdrow.FindControl("chkSelectSOList");
                                    if (ProductType == Convert.ToInt32(Request.QueryString["CATVAL"]))
                                        chk.Checked = true;
                                }
                            }
                            ActionHandler(btnAddToDOList, EventArgs.Empty);
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
        #endregion

        #region BindDropDown
        /// <summary>
        /// This method is used to Binding DropDoowns
        /// </summary>
        /// <param name="controlType"></param>
        private void BindDropDown(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region COMPANY
                case ControlEnums.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    break;
                #endregion
                #region DEPARTMENTSTORE
                case ControlEnums.DEPARTMENTSTORE:
                    ddlDeptStore.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlDeptStore.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DPT_PK;
                        ddlDeptStore.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DPT_NAME;
                        ddlDeptStore.DataSource = dtPageData;
                        ddlDeptStore.DataBind();
                    }
                    ddlDeptStore.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlDeptStore.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion

            }
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// This method is used to Binding Grids
        /// </summary>
        /// <param name="controlType"></param>
        private void BindGrid(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region CONTAINERSPLITUP
                case ControlEnums.CONTAINERSPLITUP:
                    grdCartons.DataSource = CartonsGridviewListViewState;
                    grdCartons.DataBind();
                    break;
                #endregion
                #region LIST
                case ControlEnums.LIST:
                    int rowCount = 0;
                    int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    if (dsPageData.Tables[0].Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    }
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;
                    PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                    grdDeliveryOrderSearchList.DataSource = dsPageData.Tables[0];
                    grdDeliveryOrderSearchList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                #endregion
                #region PendingSOs
                case ControlEnums.PendingSOs:
                    grdPendingSOs.DataSource = PendingSOList;
                    grdPendingSOs.DataBind();
                    break;
                #endregion
                #region DIRECTDELIVERYORDERLIST
                case ControlEnums.DIRECTDELIVERYORDERLIST:
                    grdDeliveryOrderList.DataSource = DirectDeliveryOrderDetailList;
                    grdDeliveryOrderList.DataBind();
                    break;
                #endregion

                #region DIRECTDELIVERYORDER_ITEMBATCHDTL
                case ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL:
                    grdBatchDetailsPopup.DataSource = StockBatchDetailsList;
                    grdBatchDetailsPopup.DataBind();
                    break;
                #endregion

            }
        }
        #endregion

        #region SetUIEditView
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
        #endregion

        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlEnums controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region ASSETSERVICEORDERHDR
                    case ControlEnums.DIRECTDELIVERYORDERHDR:
                        if (DirectDeliveryOrderHeaderSession != null)
                        {
                            objDirectDeliveryOrderHeader = DirectDeliveryOrderHeaderSession;
                            objDirectDeliveryOrderHeader.DPH_PK = CurrPK;
                            objDirectDeliveryOrderHeader.DPH_TRX_TYPE = "2";
                            objDirectDeliveryOrderHeader.DPH_NO = lblDeliveryOrderNo.Text;
                            objDirectDeliveryOrderHeader.DPH_DATE = Convert.ToDateTime(txtDODate.Text);
                            objDirectDeliveryOrderHeader.DPH_ETD = Convert.ToDateTime(txtETD.Text);
                            objDirectDeliveryOrderHeader.DPH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerID.Value) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                            objDirectDeliveryOrderHeader.DPH_DEPT = string.IsNullOrEmpty(ddlDeptStore.SelectedValue) ? 0 : Convert.ToInt32(ddlDeptStore.SelectedValue);
                            objDirectDeliveryOrderHeader.DPH_CUSTOMER_ADDRESS = HttpUtility.HtmlEncode(txtDeliveryAddress.Text);//GrandTotal 
                            objDirectDeliveryOrderHeader.DPH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                            objDirectDeliveryOrderHeader.DPH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                            objDirectDeliveryOrderHeader.DPH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            objDirectDeliveryOrderHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                            objDirectDeliveryOrderHeader.DPH_CRTD_BY = Convert.ToInt32(currentUser.PKUser);
                            objDirectDeliveryOrderHeader.DPH_CRTD_DT = DateTime.Now;
                            objDirectDeliveryOrderHeader.LAST_MOD_DT = LastModifiedTime;
                            objDirectDeliveryOrderHeader.DPH_TOTAL_QTY = 0;// This field has no relevance.Because there may be different UOM for different sale order
                            objDirectDeliveryOrderHeader.WKF_PROCESS = Convert.ToInt32(hdfProcessID.Value);
                            objDirectDeliveryOrderHeader.WKF_FLAG = (GetNullableInt(WKF_FLAG.Value) ?? 0);
                            objDirectDeliveryOrderHeader.APT_CODE = ApplicationType.DOD;
                            objDirectDeliveryOrderHeader.AST_DOC_MODE = Convert.ToInt16(GetDOCMODE());
                            objDirectDeliveryOrderHeader.IsContinueWithDOQty = string.IsNullOrEmpty(hdfIsContDespatchNowQty.Value) ? 0 : Convert.ToInt16(hdfIsContDespatchNowQty.Value);

                            objDirectDeliveryOrderHeader.DirectDODetailsList = DirectDeliveryOrderDetailList;
                        }
                        retObject = objDirectDeliveryOrderHeader;
                        break;
                    #endregion
                    #region DIRECTDELIVERYORDER_ITEMBATCHDTL
                    case ControlEnums.DIRECTDELIVERYORDER_ITEMBATCHDTL:
                        if (StockBatchDetailsList == null)
                            StockBatchDetailsList = new List<DirectDeliveryOrderStockBatchDetails>();
                        int slno = 1;
                        if (StockBatchDetailsList == null || StockBatchDetailsList.Count == 0)
                        {
                            slno = 1;
                        }
                        else
                        {
                            slno = StockBatchDetailsList.Max(itm => itm.SIC_ITEM_SL_NO);
                            slno++;
                        }
                        DirectDeliveryOrderStockBatchDetails ItemDetailsObj = new DirectDeliveryOrderStockBatchDetails();
                        ItemDetailsObj.SIC_PK = 0;
                        ItemDetailsObj.SIC_SL_NO = CurrSlNo;
                        ItemDetailsObj.SIC_ITEM_SL_NO = slno;
                        ItemDetailsObj.SIC_DESPATCH_DTL = DPDPK;
                        ItemDetailsObj.SIC_QTY_CONSUMED = string.IsNullOrEmpty(txtQtyPopUp.Text) ? 0 : Convert.ToDecimal(GetFormattedNumber(txtQtyPopUp.Text));
                        ItemDetailsObj.SIC_STK_BATCH = Convert.ToInt32(ddlBatchesPopUp.SelectedValue);
                        ItemDetailsObj.BatchName = string.IsNullOrEmpty(ddlBatchesPopUp.SelectedItem.Text) ? string.Empty : Convert.ToString(ddlBatchesPopUp.SelectedItem.Text);
                        ItemDetailsObj.BatchStock = string.IsNullOrEmpty(lblStockPopUp.Text) ? 0 : Convert.ToDecimal(lblStockPopUp.Text);
                        StockBatchDetailsList.Add(ItemDetailsObj);

                        retObject = StockBatchDetailsList;
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
        #endregion
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EDIT
                    case ControlEnums.DETAILFOREDIT:
                        lblDeliveryOrderNo.Text = DirectDeliveryOrderHeaderSession.DPH_NO == string.Empty ? GetGlobalResourceObject("Messages", "DocGenerationNew").ToString() : DirectDeliveryOrderHeaderSession.DPH_NO;
                        hdfDPH_VERSION.Value = DirectDeliveryOrderHeaderSession.DPH_VERSION;
                        txtDODate.Text = DirectDeliveryOrderHeaderSession.DPH_DATE == null ? string.Empty : ((DateTime)DirectDeliveryOrderHeaderSession.DPH_DATE).ToString(Resources.Constants.DateFormatShort);
                        txtETD.Text = DirectDeliveryOrderHeaderSession.DPH_ETD == null ? string.Empty : ((DateTime)DirectDeliveryOrderHeaderSession.DPH_ETD).ToString(Resources.Constants.DateFormatShort);

                        txtCustomer.Text = HttpUtility.HtmlDecode(DirectDeliveryOrderHeaderSession.DPH_CUSTOMER_TEXT);
                        hdfCustomerID.Value = DirectDeliveryOrderHeaderSession.DPH_CUSTOMER.ToString();
                        ddlDeptStore.SelectedValue = DirectDeliveryOrderHeaderSession.DPH_DEPT.ToString();
                        ddlCompany.SelectedValue = DirectDeliveryOrderHeaderSession.DPH_COMPANY.ToString();

                        lblLastModifiedHDR.Text = DirectDeliveryOrderHeaderSession.LAST_MOD_DT == null ? string.Empty : ((DateTime)DirectDeliveryOrderHeaderSession.LAST_MOD_DT).ToString(Resources.Constants.DateFormatShort);
                        LastModifiedTime = DirectDeliveryOrderHeaderSession.LAST_MOD_DT;
                        ActionHandler(btnCustSelected, EventArgs.Empty);
                        txtDeliveryAddress.Text = HttpUtility.HtmlDecode(DirectDeliveryOrderHeaderSession.DPH_CUSTOMER_ADDRESS);
                        List<DirectDeliveryOrderDetails> tempDirectDeliveryOrderDetailsList = new List<DirectDeliveryOrderDetails>();
                        foreach (var item in DirectDeliveryOrderHeaderSession.DirectDODetailsList)
                        {
                            DirectDeliveryOrderDetails doOrderItem = new DirectDeliveryOrderDetails();
                            doOrderItem.DPD_PK = item.DPD_PK;
                            doOrderItem.DPD_SL_NO = item.DPD_SL_NO;
                            doOrderItem.DPD_ITEM = item.DPD_ITEM;
                            doOrderItem.ItemId = item.DPD_ITEM;
                            doOrderItem.DPD_QTY_DESPATCHED = item.DPD_QTY_DESPATCHED;
                            doOrderItem.DPD_QTY_APPROVED = item.DPD_QTY_APPROVED;
                            doOrderItem.DPD_REMARKS = item.DPD_REMARKS;
                            doOrderItem.DPD_SALE_ORDER = item.DPD_SALE_ORDER;
                            doOrderItem.DPD_SO_DTL = item.DPD_SO_DTL;
                            doOrderItem.SORate = item.SORate;
                            doOrderItem.DPD_SL_NO = item.DPD_SL_NO; // GRD_BATCH_NO;
                            doOrderItem.DPD_SALE_QTY = item.DPD_SALE_QTY;
                            doOrderItem.DPD_UOM = item.DPD_UOM;
                            doOrderItem.DPD_SALE_UOM = item.DPD_SALE_UOM;
                            doOrderItem.DPD_SALE_UOM_CONV = item.DPD_SALE_UOM_CONV;
                            doOrderItem.DPD_VERSION = item.DPD_VERSION;
                            doOrderItem.DPD_NO = item.DPD_NO;
                            doOrderItem.DPD_BIZUNIT = item.DPD_BIZUNIT;
                            doOrderItem.DPD_CURRENT_STOCK = item.DPD_CURRENT_STOCK;
                            doOrderItem.DPD_NET_WT = item.DPD_NET_WT;
                            doOrderItem.DPD_GROSS_WT = item.DPD_GROSS_WT;
                            doOrderItem.ProductType = item.ProductType;

                            PendingSO tempPendingSO = PendingSOList
                                 .Where(x => x.SOId == doOrderItem.DPD_SALE_ORDER && x.ItemId == doOrderItem.ItemId)
                                 .Single();
                            tempPendingSO.AddedToStockList = true;
                            tempPendingSO.CheckBoxChecked = true;

                            doOrderItem.ItemName = tempPendingSO.ItemName;
                            doOrderItem.SONumber = tempPendingSO.SONumber;
                            doOrderItem.SODate = tempPendingSO.SODate;
                            doOrderItem.DPD_BALANCE_QTY_TO_DELIVER = tempPendingSO.BalanceQtyToDeliver;
                            doOrderItem.SORate = tempPendingSO.SORate;
                            doOrderItem.DPD_PRE_DELIVERED_QTY = tempPendingSO.PreDeliveredQty;
                            doOrderItem.ITM_NEED_BATCH_STK = tempPendingSO.ITM_NEED_BATCH_STK;
                            doOrderItem.DPD_SALE_UOM_TEXT = tempPendingSO.UOM;

                            doOrderItem.DOStockBatch_Details = item.DOStockBatch_Details;//Adding StockBatchDetails                           

                            tempDirectDeliveryOrderDetailsList.Add(doOrderItem);
                        }

                        DirectDeliveryOrderDetailList = tempDirectDeliveryOrderDetailsList;
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

        #region ConfigurationSettings
        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            // For Setting Flag to identify stock as batchwise or not
            dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("INVENTORY SETTINGS", "EnableStockBatch", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfEnableBatch.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            else
            {
                hdfEnableBatch.Value = "0";
            }
            hdfEnableNetWt.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowDONetGrsWeight").ToString();
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
        }

        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region SEARCH
                case ControlEnums.SEARCH:
                    ddlLocationPopUp.SelectedIndex = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                    txtCartonPrefixPopUp.Text = txtCartonFromPopUp.Text = txtCartonToPopUp.Text = string.Empty;
                    hdfSomeCartonMissingConfirm.Value = CommonConstants.SELECT_ALL_VAL;
                    chkAutoMode.Checked = false;
                    break;
                #endregion
                #region CLEAR
                case ControlEnums.CLEAR:
                    // hdfGRH_VERSION.Value = 1.ToString();
                    hdfOrderPcsConfirm.Value = "0";
                    OrderTotalPCS = 0;
                    lblDeliveryOrderNo.Text = GetGlobalResourceObject("Messages", "DocGenerationNew").ToString();
                    txtDODate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtCustomer.Text = string.Empty;
                    hdfCustomerID.Value = "0";
                    txtDeliveryAddress.Text = string.Empty;
                    PendingSOList = null;
                    DirectDeliveryOrderDetailList = null;

                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();
                    CurrPK = 0;
                    SO_PK.Value = "0";
                    hdfIsDOCancelled.Value = "0";
                    hdfIsContDespatchNowQty.Value = "0";
                    SearchType.SelectedValue = "SOH_NO";
                    BindGrid(ControlEnums.PendingSOs);
                    BindGrid(ControlEnums.DIRECTDELIVERYORDERLIST);
                    EntryStatus = EntryStatus.LISTMODE;

                    break;
                #endregion
                #region CLEARSEARCH
                case ControlEnums.CLEARSEARCH:
                    txtDeliveryOrderNoSearch.Text = hdfDeliveryOrderNoPkSearch.Value = string.Empty;
                    txtCustomerSearch.Text = hdfCustomerPKSearch.Value = txtFromDate.Text = txtToDate.Text = string.Empty;
                    ddlStatus.SelectedValue = CommonConstants.SELECTVAL;
                    break;
                #endregion
                #region CLEARBATCHPOPUP
                case ControlEnums.CLEARBATCHPOPUP:
                    ddlBatchesPopUp.SelectedValue = CommonConstants.SELECTVAL;//"-1"
                    lblStockPopUp.Text = string.Empty;
                    hdfBatchPopupUOM.Value = "0";
                    txtQtyPopUp.Text = string.Empty;
                    break;
                #endregion

            }
        }
        #endregion

        #region Helper Methods
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        //private void GridViewSOChangeColour()
        //{
        //    foreach (GridViewRow row in grdPendingSOs.Rows)
        //    {
        //        if (((HiddenField)row.FindControl("hdfSOPendingListSelected")).Value == "True")
        //        {
        //            string hexColour = GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString();
        //            row.BackColor = System.Drawing.ColorTranslator.FromHtml(hexColour);
        //        }
        //    }
        //}
        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            return (decimal.TryParse(str, out result) ? (decimal?)result : null);
        }
        private bool UpdateDirectDeliveryOrderDetailListFromGridview(bool showValidationMessage = true, int transFlag = 0)
        {
            bool validFlag = true;
            System.Text.StringBuilder message = new System.Text.StringBuilder();

            foreach (GridViewRow grdrow in grdDeliveryOrderList.Rows)
            {
                DropDownList ddlBatchNogrid = grdrow.FindControl("ddlBatchNogrid") as DropDownList;
                DropDownList ddlUOMGrid = grdrow.FindControl("ddlUOMGrid") as DropDownList;
                HiddenField hdfItmNeedBatchStkDOList = grdrow.FindControl("hdfItmNeedBatchStkDOList") as HiddenField;
                Label lblCurrentStock = grdrow.FindControl("lblCurrentStock") as Label;
                int soDetId = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPODetIdDOList")).Value).Value;
                int pk = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPkDOList")).Value).Value;
                DirectDeliveryOrderDetails item = DirectDeliveryOrderDetailList.Where(x => x.DPD_SO_DTL == soDetId && x.DPD_PK == pk).Single();

                decimal despatchNow;
                despatchNow = Convert.ToDecimal(GetFormattedNumber(((TextBox)grdrow.FindControl("txtDespatchNowQty")).Text.Trim()));
                item.DPD_QTY_DESPATCHED = despatchNow;
                item.DPD_UOM = (String.IsNullOrEmpty(ddlUOMGrid.SelectedValue) || ddlUOMGrid.SelectedValue == CommonConstants.SELECTVAL) ? 0 : Convert.ToInt32(ddlUOMGrid.SelectedValue);

                PendingSO pendingSO = this.PendingSOList.Where(x => x.SODetId == soDetId).Single();
                ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Remove("color");
                if (despatchNow <= 0)
                {
                    string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_QtyGreaterThanZero").ToString(), item.ItemName.Replace("'", " ")).ToString());
                    message.Append(str);
                    validFlag = false;
                    ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Add("color", "Red");
                }
                if (hdfEnableBatch.Value == "1" && ddlBatchNogrid.SelectedValue == CommonConstants.SELECTVAL)//Checking Batch is selected or not
                {
                    string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Msg_SelectBatch").ToString(), item.ItemName.Replace("'", " ")).ToString());
                    message.Append(str);
                    validFlag = false;
                    ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Add("color", "Red");
                    break;
                }
                if (String.IsNullOrEmpty(ddlUOMGrid.SelectedValue) || ddlUOMGrid.SelectedValue == CommonConstants.SELECTVAL)//Checking UOM is selected or not
                {
                    string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_Select_UOM").ToString(), item.ItemName.Replace("'", " ")).ToString());
                    message.Append(str);
                    validFlag = false;
                    ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Add("color", "Red");
                }
                decimal NetWt, GrossWt;
                NetWt = Convert.ToDecimal(GetFormattedNumber(((TextBox)grdrow.FindControl("txtNetWeight")).Text.Trim()));
                GrossWt = Convert.ToDecimal(GetFormattedNumber(((TextBox)grdrow.FindControl("txtGrossWeight")).Text.Trim()));
                item.DPD_NET_WT = NetWt;
                item.DPD_GROSS_WT = GrossWt;
                #region Stock Validation
                if (hdfEnableBatch.Value == "0" || hdfItmNeedBatchStkDOList.Value == "0") //No Batch
                {
                    #region Commented because this should be checked in Save SP. Reason: it may have different UOM
                    //if (!string.IsNullOrEmpty(lblCurrentStock.Text))
                    //{
                    //    if (despatchNow > Convert.ToDecimal(lblCurrentStock.Text))
                    //    {
                    //        string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_InsufficientStockQty").ToString(), item.ItemName).ToString());
                    //        message.Append(str);
                    //        validFlag = false;
                    //        ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Add("color", "Red");
                    //    }
                    //} 
                    #endregion
                }
                else
                {
                    #region Have Batch
                    if (item.DOStockBatch_Details != null && item.DOStockBatch_Details.Count > 1)//Multiple Batch
                    {
                        if (item.DPD_QTY_DESPATCHED != item.DOStockBatch_Details.Sum(x => Convert.ToDecimal(x.SIC_QTY_CONSUMED)))//Despatch now qty does not match with total batch qty
                        {
                            string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_AllocAndTotQtyMismatch").ToString(), item.ItemName.Replace("'", " ")).ToString());
                            message.Append(str);
                            validFlag = false;
                            ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Add("color", "Red");
                        }
                    }
                    else
                    {
                        if (item.DOStockBatch_Details != null && item.DOStockBatch_Details.Count > 0)
                            item.DOStockBatch_Details[0].SIC_QTY_CONSUMED = item.DPD_QTY_DESPATCHED;//Setting Desp.Now TextBox Qty to stockBatchDetails
                        #region Commented because this should be checked in Save SP. Reason: it may have different UOM
                        //if (!string.IsNullOrEmpty(lblCurrentStock.Text))
                        //{
                        //    if (item.DPD_QTY_DESPATCHED > Convert.ToDecimal(lblCurrentStock.Text))
                        //    {
                        //        string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_InsufficientStockQty").ToString(), item.ItemName).ToString());
                        //        message.Append(str);
                        //        validFlag = false;
                        //        ((TextBox)grdrow.FindControl("txtDespatchNowQty")).Attributes.CssStyle.Add("color", "Red");
                        //    }
                        //} 
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            if (!validFlag & showValidationMessage)
            {
                string msg = message.ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + msg + "','" + Resources.Messages.Information + "');", true);
            }
            else
            {
                validFlag = true;
            }
            return validFlag;
        }
        /// <summary>
        /// multiBatches for Grid 
        /// </summary>
        private void SetMultiBatchForGrid(bool status, bool removeall)
        {
            ImageButton imbAddGridBatch = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("imbAddGridBatch") as ImageButton;
            ImageButton imbClearGridBatch = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("imbClearGridBatch") as ImageButton;
            DropDownList ddlBatchNogrid = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("ddlBatchNogrid") as DropDownList;
            HiddenField hdfIsMultiple = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("hdfIsMultiple") as HiddenField;
            HiddenField hdfDPD_SL_NO = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("hdfDPD_SL_NO") as HiddenField;
            Label lblCurrentStock = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("lblCurrentStock") as Label;
            TextBox txtDespatchNowQty = grdDeliveryOrderList.Rows[GridRowIndex].FindControl("txtDespatchNowQty") as TextBox;
            //is status true set as multi batch add otherwise set clear
            if (status)
            {
                imbClearGridBatch.Visible = true;
                imbAddGridBatch.ToolTip = this.GetLocalResourceObject("Msg_ModifyBatches").ToString();
                ddlBatchNogrid.Items.Clear();
                ddlBatchNogrid.Items.Insert(0, new ListItem(CommonConstants.MULTIBATCH, CommonConstants.MULTIBATCH_VALUE));
                ddlBatchNogrid.Enabled = false;
                hdfIsMultiple.Value = "1";
                lblCurrentStock.Visible = false;
            }
            else
            {
                imbClearGridBatch.Visible = false;
                imbAddGridBatch.ToolTip = this.GetLocalResourceObject("Msg_addBatches").ToString();
                ddlBatchNogrid.Enabled = true;
                hdfIsMultiple.Value = "0";
                lblCurrentStock.Visible = true;
            }
            if (txtDespatchNowQty != null)
            {
                int dpdSlno = string.IsNullOrEmpty(hdfDPD_SL_NO.Value) ? 0 : Convert.ToInt32(hdfDPD_SL_NO.Value);
                DirectDeliveryOrderDetails directDODetailsObj = DirectDeliveryOrderDetailList.SingleOrDefault(itm => itm.DPD_SL_NO == dpdSlno);
                if (directDODetailsObj.DOStockBatch_Details != null)
                {
                    decimal totalAddedQty = directDODetailsObj.DOStockBatch_Details.Sum(x => Convert.ToDecimal(x.SIC_QTY_CONSUMED));
                    txtDespatchNowQty.Text = GetFormattedNumber(totalAddedQty);
                }
            }
        }
        private void SetAddedPendingSOToDOList()
        {
            objItemList = new List<SOHeaderListBO>();
            foreach (DirectDeliveryOrderDetails item in DirectDeliveryOrderDetailList)
            {
                SOHeaderListBO objSoList = new SOHeaderListBO();
                objSoList.SOH_PK = Convert.ToInt32(item.DPD_SALE_ORDER);//SOH_PK
                objItemList.Add(objSoList);
            }
        }
        /// <summary>
        /// This Methode is Used to Formating Currency fields in HTML 
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyFormat()
        {
            return this.CurrencyFormatString;
        }

        public void ShowRejectedPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=DivRejectedDtlsPopUp]','"
                + GetLocalResourceObject("RejectedDetails").ToString() + "','500','450');", true);
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.DOD, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "dll";
            if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            {
                BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            }
            bool flag = true;
            string[] extensionList = BlockedExtensions.Split(',');
            for (int i = 0; i < extensionList.Length; i++)
                if (("." + extensionList[i]) == extension)
                {
                    flag = false;
                    break;
                }
            return flag;
        }

        /// <summary>
        /// Check the selected SO's have same items with different rate
        /// </summary>
        /// <param name="soDetIdList"></param>
        /// <returns></returns>
        private bool IsSelectedSOValid(List<int> soDetIdList)
        {
            bool validPO = true;
            List<DirectDeliveryOrderDetails> tempDirectDeliveryOrderDetailList;
            if (soDetIdList.Count > 0)
            {
                List<PendingSO> pendingSOTemp = new List<PendingSO>();
                pendingSOTemp = PendingSOList;
                pendingSOTemp.Where(x => soDetIdList.Contains(x.SODetId)).ToList();
                PendingSOList = pendingSOTemp;

                List<PendingSO> lstNewSelectedPOs = PendingSOList
                    .Where(x => soDetIdList.Contains(x.SODetId))
                    .ToList();
                int slno = 0;
                List<DirectDeliveryOrderDetails> DirectDeliveryOrderItemListNew = new List<DirectDeliveryOrderDetails>();
                foreach (var item in DirectDeliveryOrderDetailList)
                {
                    DirectDeliveryOrderItemListNew.Add(item);
                }
                if (DirectDeliveryOrderItemListNew.Count > 0)
                {
                    slno = DirectDeliveryOrderItemListNew.Max(x => x.DPD_SL_NO);
                }
                List<DirectDeliveryOrderDetails> tempNewList = new List<DirectDeliveryOrderDetails>();
                tempNewList = lstNewSelectedPOs
                    .Select(x => new DirectDeliveryOrderDetails
                    {
                        DPD_PK = 0,
                        DPD_SALE_ORDER = x.SOId,
                        DPD_SO_DTL = x.SODetId,
                        SONumber = x.SONumber,
                        DPD_ITEM = x.ItemId,
                        ItemId = x.ItemId,
                        ItemName = x.ItemName,
                        DPD_UOM = x.UOMId,
                        DPD_SALE_QTY = x.SOQty,
                        DPD_BALANCE_QTY_TO_DELIVER = x.BalanceQtyToDeliver,
                        DPD_QTY_DESPATCHED = x.BalanceQtyToDeliver,
                        SORate = x.SORate,
                        ITM_NEED_BATCH_STK = x.ITM_NEED_BATCH_STK
                    })
                    .ToList();

                foreach (var item in tempNewList) item.DPD_SL_NO = ++slno;

                tempDirectDeliveryOrderDetailList = new List<DirectDeliveryOrderDetails>();
                tempDirectDeliveryOrderDetailList = DirectDeliveryOrderItemListNew;
                tempDirectDeliveryOrderDetailList.AddRange(tempNewList);
                DirectDeliveryOrderItemListNew = tempDirectDeliveryOrderDetailList;

                #region Cannot combine PO's have same Item with different Rate
                List<DirectDeliveryOrderDetails> DirectDeliveryOrderItemListNewTemp = new List<DirectDeliveryOrderDetails>();
                DirectDeliveryOrderItemListNewTemp = DirectDeliveryOrderItemListNew.GroupBy(ac => new
                {
                    ac.ItemId,
                    ac.SORate
                })
                  .Select(ac => new DirectDeliveryOrderDetails
                  {
                      ItemId = ac.Key.ItemId,
                      SORate = ac.Key.SORate
                  }).OrderBy(f => f.ItemId).ToList();
                int itemIdTemp = 0, count = 0;
                bool diffRate = false;
                foreach (var item in DirectDeliveryOrderItemListNewTemp)
                {
                    if (itemIdTemp != 0)
                    {
                        if (item.ItemId == itemIdTemp)
                        {
                            itemIdTemp = item.ItemId;
                            diffRate = true;
                            break;
                        }
                        else
                        {
                            itemIdTemp = item.ItemId;
                        }
                    }
                    else
                    {
                        itemIdTemp = item.ItemId;
                    }
                }
                if (diffRate)
                {
                    validPO = false;
                }
                #endregion
            }
            return validPO;
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
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }

        /// <summary>
        /// Get Batch Details 
        /// </summary>
        /// <param name="batchPK">Batch PK</param>
        /// <returns></returns>
        private DataTable BatchDetails(int batchPK, int ToUOM)
        {
            dtPageData = DirectDeliveryOrderBL.GetBatchDetails(batchPK, ToUOM);
            return dtPageData;
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
            return num.ToString(hdfDecimalFormatWithSeperator.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        /// <summary>
        /// function for show popup
        /// </summary>
        private void ShowBatchPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupBatches]','" + Resources.Messages.MsgAddBatches + "','650','auto');", true); // 560
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalBatchQty", "$(document).ready(function(){CalculateTotalBatchQty();});", true);
        }
        /// <summary>
        /// Calculate balance Latex qty for Diaplay
        /// </summary>
        private void CalculateBatchPopupBalance()
        {
            if (StockBatchDetailsList != null)
            {
                double totalAddedQty = StockBatchDetailsList.Sum(x => Convert.ToDouble(x.SIC_QTY_CONSUMED));
                totalAddedQty = Convert.ToDouble(GetFormattedNumber(totalAddedQty));
            }
        }
        /// <summary>
        /// Checking batch is already added  
        /// </summary>
        /// <returns></returns>
        private bool IsBatchAlreadyinPopupGrid()
        {
            bool isBatchAdded = false;
            for (int i = 0; i <= grdBatchDetailsPopup.Rows.Count - 1; i++)
            {
                HiddenField hdfSIC_STK_BATCH = grdBatchDetailsPopup.Rows[i].FindControl("hdfSIC_STK_BATCH") as HiddenField;
                if (hdfSIC_STK_BATCH.Value == ddlBatchesPopUp.SelectedValue)
                {
                    isBatchAdded = true;
                    break;
                }
            }
            return isBatchAdded;
        }
        /// <summary>
        /// for checking current stock in popup
        /// </summary>
        /// <returns></returns>
        private bool IsCurrentStockAvailableInPopup()
        {
            bool isStockAvailable = true;
            double currStock = lblStockPopUp.Text == string.Empty ? 0 : Convert.ToDouble(lblStockPopUp.Text);
            double qtytoadd = txtQtyPopUp.Text == string.Empty ? 0 : Convert.ToDouble(txtQtyPopUp.Text);
            if (qtytoadd > currStock)
            {
                isStockAvailable = false;
            }
            return isStockAvailable;
        }

        private void FillTransactionData()
        {
            EntryStatus = EntryStatus.EDITMODE;
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
            }
            GetFieldValues(ControlEnums.PendingSOs);
            SetFieldValues(ControlEnums.PendingSOs);
            GetFieldValues(ControlEnums.DETAILFOREDIT);
            SetFieldValues(ControlEnums.DETAILFOREDIT);
        }

        private bool CheckAddedType(List<int> lst)
        {
            bool result = true;
            if (lst.Distinct().Count() == 1)
            {
                foreach (GridViewRow grdRow in grdDeliveryOrderList.Rows)
                {
                    HiddenField hdfProductType = grdRow.FindControl("hdfProductType") as HiddenField;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        if (lst[i] != Convert.ToInt32(hdfProductType.Value))
                            result = false;
                    }
                }
            }
            else
                result = false;
            return result;
        }
        #endregion

        #region Page Control Enum
        enum ControlEnums
        {
            COMPANY,
            LIST,
            SUPPLIERS,
            DEPARTMENTSTORE,
            CLEAR,
            PendingSOs,
            CLEARSEARCH,
            CLEARBATCHPOPUP,
            DETAILFOREDIT,
            DIRECTDELIVERYORDERHDR,
            DIRECTDELIVERYORDERDTL,
            DIRECTDELIVERYORDER_ITEMBATCHDTL,
            DIRECTDELIVERYORDERLIST,
            ADDTODIRECTDELIVERYORDERLIST,
            REJECTIONREASON,
            REJECTTO,
            REJECTEDREASONLIST,
            CLEARREJECTCONTROLS,
            POPERCENTAGE,
            SOFROMINBOX,
            SELECTEDDOC,
            ADDITEM,
            CUSTOMER,
            FILLBATCHES,
            EDIT,
            SODETAILS,
            CURRENTSTORESTOCK,
            UOM,
            UOMCONVERSIONFACTOR,
            CONTAINERSPLITUP,
            SEARCH

        }
        #endregion
    }
}