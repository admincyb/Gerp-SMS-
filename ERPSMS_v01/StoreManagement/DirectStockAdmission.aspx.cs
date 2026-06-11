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
using BusinessObject.StoreManagement;
using System.IO;

namespace ERPSMS_v01.StoreManagement
{
    public partial class DirectStockAdmission : ERP.Store.UI.WorkFlowBasePage // System.Web.UI.Page //
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

        private double GrnExcessQty
        {
            get
            {
                return this.ViewState["GrnExcessQty"] == null ? 0 : Convert.ToDouble(this.ViewState["GrnExcessQty"].ToString());
            }
            set
            {
                this.ViewState["GrnExcessQty"] = value;
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
            //get
            //{
            //    return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            //}
            //set
            //{
            //    ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            //}
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int companyPK
        {

            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CompanyPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CompanyPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CompanyPK] = value;
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
        /// Property used for to store DirectStockAdmission
        /// </summary>
        private DirectStockAdmissionBO.DirectStockAdmission DirectStockAdmissionProp
        {
            get
            {
                return this.ViewState["DirectStockAdmissionProp"] == null ? new DirectStockAdmissionBO.DirectStockAdmission() : (DirectStockAdmissionBO.DirectStockAdmission)(this.ViewState["DirectStockAdmissionProp"]);
            }
            set
            {
                this.ViewState["DirectStockAdmissionProp"] = value;
            }
        }

        /// <summary>
        /// Property used for to store PedningPO List
        /// </summary>
        private List<DirectStockAdmissionBO.PendingPO> PendingPOList
        {
            get
            {
                return this.ViewState["PendingPOList"] == null ? new List<DirectStockAdmissionBO.PendingPO>() : (List<DirectStockAdmissionBO.PendingPO>)(this.ViewState["PendingPOList"]);
            }
            set
            {
                this.ViewState["PendingPOList"] = value;
            }
        }

        /// <summary>
        /// Property used for to store StockAdmission List
        /// </summary>
        private List<DirectStockAdmissionBO.StockAdmissionItem> StockAdmissionItemList
        {
            get
            {
                return this.ViewState["StockAdmissionItem"] == null ? new List<DirectStockAdmissionBO.StockAdmissionItem>() : (List<DirectStockAdmissionBO.StockAdmissionItem>)(this.ViewState["StockAdmissionItem"]);
            }
            set
            {
                this.ViewState["StockAdmissionItem"] = value;
            }
        }

        ///// <summary>
        ///// To maintain the PageIndex in viewstate
        ///// </summary>
        //private string PageIndex
        //{
        //    get
        //    {
        //        return (string)this.ViewState[ViewstateStrings.PageIndex] ?? "1";
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.PageIndex] = value;
        //    }
        //}

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

        private List<BusinessObject.StoreManagement.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileDSADetailsList] == null ? null : (List<BusinessObject.StoreManagement.FileDetails>)Session[ERP.Utilities.SessionStrings.FileDSADetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileDSADetailsList] = value;
            }
        }

        private List<BusinessObject.StoreManagement.DirectStockAdmissionUploads> DSAUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.DSAUploadList] == null ? null : (List<BusinessObject.StoreManagement.DirectStockAdmissionUploads>)ViewState[ViewstateStrings.DSAUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.DSAUploadList] = value;
            }
        }

        private List<DirectStockAdmissionBO.MaterialReturn> MaterialReturnList
        {
            get
            {
                return ViewState["MaterialReturnList"] == null ? null : (List<DirectStockAdmissionBO.MaterialReturn>)ViewState["MaterialReturnList"];
            }
            set
            {
                ViewState["MaterialReturnList"] = value;
            }
        }

        private DirectStockAdmissionBO.MaterialReturn MaterialReturn
        {
            get
            {
                return ViewState["MaterialReturn"] == null ? null : (DirectStockAdmissionBO.MaterialReturn)ViewState["MaterialReturn"];
            }
            set
            {
                ViewState["MaterialReturn"] = value;
            }
        }

        private List<DirectStockAdmissionBO.BatchDetails> BatchDetailList
        {
            get
            {
                return ViewState["BatchDetailList"] == null ? null : (List<DirectStockAdmissionBO.BatchDetails>)ViewState["BatchDetailList"];
            }
            set
            {
                ViewState["BatchDetailList"] = value;
            }
        }

        private List<DirectStockAdmissionBO.WOHeaderList> ListForReturn
        {
            get
            {
                return ViewState["ListForReturn"] == null ? null : (List<DirectStockAdmissionBO.WOHeaderList>)ViewState["ListForReturn"];
            }
            set
            {
                ViewState["ListForReturn"] = value;
            }
        }

        #endregion

        #region Variables

        private int selWOPK;
        private int BatchPK;
        private int ItemPK;
        private int ReturnWOPK;
        private int ItemTypePK;
        private int processPK;
        private string refID;
        private string inboxFlag;
        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private List<int> WOPKs = new List<int>();
        private Dictionary<int, int> WO_Item_List = new Dictionary<int, int>();

        //IsAfterMulti => For material return(GRN WO) to understand the entry is befor or after Multiple WO.
        int IsAfterMulti = 0;

        User currentUser;
        DataSet dsPageData;
        DataTable dtPageData;
        DataTable dtCompany;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        DataTable dtPos;
        ADM_DOC_ATTACH admDocAttachObj;
        DirectStockAdmissionUploads DSAUploadObj;
        bool isCancelled = false;
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

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDeleteNew.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            //  this.btnCancel.Load += new EventHandler(btnAction_Load);    
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
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
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }
        /// <summary>
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                //Breadcrumb WO
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "1")
                {
                    if (this.GetLocalResourceObject("BreadcrumbWO") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbWO").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("WOTitle").ToString();
                    }
                }
                else
                {
                    if (this.GetLocalResourceObject("Breadcrumb") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("Title").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing(2);});", true);
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing(2);});", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails();});", true);
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
            if (Request.QueryString["Type"] != null)
            {
                path += "&Type=" + Request.QueryString["Type"].ToString();
                hdfMenuType.Value = Request.QueryString["Type"].ToString();
            }
            //if (pid == 12)
            //    path = path + "?PID=" + pid.ToString();
            //if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            //{
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
                //path = Resources.PageURL.DirectStockAdmission.Replace("~", "");
                base.WkfPageUrl = ucrWrkf.PageUrl = path;
                PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
            }
            //}
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
                    hdfDecimalCount.Value = (GetNullableInt(GetLocalResourceObject("QtyDecimal").ToString()) ?? 0).ToString();
                    processID = FillProcessID(0, 1);
                    if (hdfMenuType.Value == "1")
                    {
                        //ListHead.InnerHtml = GetLocalResourceObject("PendingWorkOrderItems").ToString();
                        //aTab1.InnerHtml = GetLocalResourceObject("PendingWorkOrderItems").ToString();
                        lnkPendingList.Text = GetLocalResourceObject("PendingWorkOrderItems").ToString();
                        Label4.Text = GetLocalResourceObject("WoNo").ToString();
                        grdList.Columns[4].HeaderText = GetLocalResourceObject("WoNo").ToString();
                        grdPOs.Columns[1].HeaderText = GetLocalResourceObject("WoNo").ToString();
                        grdPOs.Columns[4].HeaderText = grdStockAdmissionList.Columns[2].HeaderText = GetLocalResourceObject("WoQty").ToString();
                        grdStockAdmissionList.Columns[0].HeaderText = GetLocalResourceObject("WoNo").ToString();
                        lblDepreNo.Text = GetLocalResourceObject("WOGRNNO").ToString();
                        DivChkPendingWO.Visible = true;
                        grdPOs.Columns[7].Visible = false;// grdStockAdmissionList.Columns[4].Visible
                    }
                    else
                    {
                        //ListHead.InnerHtml = GetLocalResourceObject("PendingPurchaseOrderItems").ToString();
                        lnkPendingList.Text = GetLocalResourceObject("PendingPurchaseOrderItems").ToString();
                        DivChkPendingWO.Visible = false;
                    }
                    ucrWrkf.ProcessID = processID;
                    if (ucrWrkf.ProcessID > 0)
                        hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    EntryStatus = EntryStatus.ENTRYMODE;
                    FileDetailsList = null;
                    DSAUploadList = null;
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;
                    BindDropDown(ControlEnums.SEARCHTYPE);
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
                        referenceID = int.Parse(refID);
                        appId = GetApplicationID(referenceID);
                        if (processPK == ucrWrkf.ProcessID)
                        {
                            CurrPK = appId;
                            base.WkfRefID = ucrWrkf.RefID = referenceID;
                        }
                    }
                    //FillProcessId();
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlEnums.COMPANY);
                    SetFieldValues(ControlEnums.COMPANY);
                    GetFieldValues(ControlEnums.SUPPLIERS);
                    SetFieldValues(ControlEnums.SUPPLIERS);

                    GetFieldValues(ControlEnums.ADMISSIONSTORE);
                    SetFieldValues(ControlEnums.ADMISSIONSTORE);
                    GetFieldValues(ControlEnums.SUBCONTRACTSTORE);
                    SetFieldValues(ControlEnums.SUBCONTRACTSTORE);
                    ddlAdmissionStore.SelectedValue = currentUser.CurrentDeptPK.ToString();

                    GetFieldValues(ControlEnums.REJECTIONREASON);
                    SetFieldValues(ControlEnums.REJECTIONREASON);
                    GetFieldValues(ControlEnums.REJECTTO);
                    SetFieldValues(ControlEnums.REJECTTO);
                    GetFieldValues(ControlEnums.POPERCENTAGE);
                    GetFieldValues(ControlEnums.Plant);
                    SetFieldValues(ControlEnums.Plant);
                    if (CurrPK > 0)
                    {
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
                        //EntryStatus = EntryStatus.EDITMODE;
                        GetFieldValues(ControlEnums.POS);
                        SetFieldValues(ControlEnums.POS);
                        GetFieldValues(ControlEnums.DETAILFOREDIT);
                        SetFieldValues(ControlEnums.DETAILFOREDIT);
                        SetFieldValues(ControlEnums.UPLOADEDFILES);
                    }
                    else
                    {
                        ActionHandler(lnkList, EventArgs.Empty);
                    }
                    Session[ERP.Utilities.SessionStrings.RefID] = null;
                    Session[ERP.Utilities.SessionStrings.InboxFlag] = null;

                    if (Request.QueryString["PRefID"] != null)
                    {
                        int pRefId = GetNullableInt(Request.QueryString["PRefID"].ToString()) ?? 0;
                        WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                        DataTable dtApplication = wrkfService.GetApplicationID(pRefId);
                        if (dtApplication != null)
                        {
                            if (dtApplication.Rows.Count > 0)
                            {
                                PO_PK.Value = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                                GetFieldValues(ControlEnums.POSFROMINBOX);
                                SetFieldValues(ControlEnums.POSFROMINBOX);
                                //EntryStatus = EntryStatus.ENTRYMODE;
                                EntryStatus = EntryStatus.NEWMODE;
                            }
                        }
                    }

                    if (Request.QueryString["Type"] != null && Request.QueryString["Type"] == "1")
                    {
                        spnMatReturnList.Visible = true;
                    }
                    else
                    {
                        spnMatReturnList.Visible = false;
                    }
                }
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
            //Session Logout on Department change
            if (!sender.GetType().IsEquivalentTo(typeof(GridView)))//'OnPreRender' event registered in Gridview, so every action on page calling Action Handler
                if (!(this.Master as ERPSMS_2).ValidatePageDept())
                    return;

            bool bIsChecked = false;
            // int selectedPK;
            int result = 0;
            int poDetId;
            // List<int> poIdList = new List<int>();
            List<int> poDetIdList = new List<int>();
            List<DirectStockAdmissionBO.StockAdmissionItem> tempStockAdmissionItemList;
            DirectStockAdmissionBO.DirectStockAdmission DirectStockAdmissionViewState;
            XmlDocument xmlDoc;
            List<object> lstResult = new List<object>();
            bool validFlagStockAdmissionList = false;

            DropDownList ddlWkfAction;
            string action;
            FileInfo tempFileInfoObj;
            int selectedItemPK;
            string savePath = string.Empty;
            int grdListRowDeptId = 0;
            TextBox WrkfComments;
            string IsInventoryLocked = "";
            string LockUptoDate = string.Empty;
            int TabNo = 1;
            try
            {
                //int t = 0;
                //int i = 0 / t;
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
                    if (((DropDownList)sender).ID == "ddlSuppliers")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlAdmissionStore")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlBatchesPopUp")
                    {
                        commonActions = ActionsEnum.BATCHCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlBatch")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(GridView)))
                {
                    if (((GridView)sender).ID == "grdPOs")
                    {
                        commonActions = ActionsEnum.ADDITEM;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "ChkPendingWO")
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                }
                #endregion
                switch (commonActions)
                {
                    #region LIST CANCEL
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        //uclPaging.CurrentPage = 0;
                        //this.PageIndex = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = base.WkfRefID = 0;
                        FillProcessID(0, 1);
                        this.ModifiedDatePnl.Visible = false;
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gdrow = ((RadioButton)sender).NamingContainer as GridViewRow;
                        hdfWOStatus.Value = Convert.ToString(((HiddenField)gdrow.FindControl("hdfStatusList")).Value);
                        if (hdfWOStatus.Value == "2" && hdfMenuType.Value == "1")
                            btnEdit.Visible = false;
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
                        ChkPendingWO.Checked = true;
                        ResetForm(ControlEnums.CLEAR);
                        this.EntryStatus = EntryStatus.NEWMODE;
                        GetFieldValues(ControlEnums.COMPANY);
                        SetFieldValues(ControlEnums.COMPANY);
                        SwitchTab(1);
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        if (ddlSuppliers.SelectedIndex > 0 && ddlAdmissionStore.SelectedIndex > 0)
                        {
                            GetFieldValues(ControlEnums.POS);
                            SetFieldValues(ControlEnums.POS);
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region EDIT DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfStockTransferPk")).Value);
                                grdListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDept")).Value);
                                hdfWOStatus.Value = Convert.ToString(((HiddenField)grdrow.FindControl("hdfStatusList")).Value);
                                if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfStatusList")).Value) == 4)
                                {
                                    hdfIsDSACancelled.Value = "1";
                                }
                                else
                                {
                                    hdfIsDSACancelled.Value = "0";
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(grdListRowDeptId, 1);
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
                                //EntryStatus = EntryStatus.VIEWMODE;

                            }
                            ucrWrkf.ViewAction();

                            GetFieldValues(ControlEnums.SUPPLIERS);
                            SetFieldValues(ControlEnums.SUPPLIERS);
                            GetFieldValues(ControlEnums.POS);
                            SetFieldValues(ControlEnums.POS);
                            GetFieldValues(ControlEnums.DETAILFOREDIT);
                            SetFieldValues(ControlEnums.DETAILFOREDIT);
                            SetFieldValues(ControlEnums.UPLOADEDFILES);
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
                        if (!ConfirmFutureDate(commonActions))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ConfirmFutureDate", "ConfirmFutureDate('" + ActionsEnum.SAVE.ToString() + "');", true);
                            return;
                        }
                        #region Checking :Inventory Transaction Locking
                        IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtAdmissionDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                        if (IsInventoryLocked == "1")
                        {
                            litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        #endregion
                        WKF_FLAG.Value = 0.ToString();
                        DirectStockAdmissionViewState = (DirectStockAdmissionBO.DirectStockAdmission)SetUIValuesToObject(out validFlagStockAdmissionList);
                        if (hdfIsStockExceeded.Value == "1" && hdfMenuType.Value == "1")
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("ReturnQtyExceedsStock").ToString() + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        //if (DirectStockAdmissionViewState.StockAdmissionList.Count == 0 && DirectStockAdmissionViewState.MaterialReturnList.Count == 0)
                        if ((DirectStockAdmissionViewState.StockAdmissionList == null || DirectStockAdmissionViewState.StockAdmissionList.Count == 0) && (DirectStockAdmissionViewState.MaterialReturnList == null || DirectStockAdmissionViewState.MaterialReturnList.Count == 0))
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        //if(hdfMenuType.Value=="1")
                        //{
                        //    if (!IsValidReturnForWO())
                        //    {
                        //        litErrorMsg.Text = (GetLocalResourceObject("Err_NoBatchSelected")).ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.Captions.Information + "');", true);
                        //        return;
                        //    }
                        //}
                        if (validFlagStockAdmissionList == false)
                        {
                            return;
                        }
                        string strTrxNumber = string.Empty;
                        DirectStockAdmissionViewState.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(DirectStockAdmissionViewState);
                        //lstResult = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.SaveStockTransferDtls(xmlDoc.InnerXml);
                        lstResult = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.SaveStockTransferWkf(xmlDoc.InnerXml, out strTrxNumber);

                        result = Convert.ToInt32(lstResult[0]);
                        //result = BusinessLogic.OutStandingDue.OutStandingDueBL.SaveMail(xmlDoc.InnerXml);
                        // result = BL.AdministrationBL.CompoundFormulationBL.SaveCompoundFormulation(compoundFormulation);
                        if (result > 0)
                        {
                            #region ATTACHMENT SAVE
                            if (DSAUploadList != null && DSAUploadList.Count > 0)
                            {
                                savePath = string.Empty;
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

                                foreach (DirectStockAdmissionUploads obj in DSAUploadList)
                                {
                                    string filePath = savePath + obj.AttachmentFileName;
                                    FileInfo attachedFileInfo = new FileInfo(filePath);
                                    if (FileDetailsList != null)
                                    {
                                        FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                        if (fileDetailsObj != null)
                                        {
                                            fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                        }
                                    }
                                }
                            }
                            #endregion
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            if (hdfMenuType.Value == "1")
                            {
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString());

                            }
                            else
                            {
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());

                            }
                            ResetForm(ControlEnums.CLEAR);
                            ResetForm(ControlEnums.CLEARSEARCH);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;

                                }
                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;

                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }

                            else if (result == (int)DbSaveStatus.GRNEXCESS)
                            {
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.GrnExcessQty;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.GrnExcessQty;
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }

                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.AlreadyDeleted;

                                }
                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.AlreadyDeleted;

                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;

                                }
                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;

                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == -10)
                            {
                                //litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.MayAffectStockValueConfirmation;
                                // Include Popup for Yes/No
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmStockValueChange", "ClosePopup();fnConfirmStockValueChange('SAVE');", true);
                                return;
                            }
                            else if (result == -31)
                            {
                                litErrorMsg.Text = string.Format(Resources.Messages.StockTransferAlreadyDone, txtAdmissionDate.Text);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }
                            else if (result == -32)
                            {
                                litErrorMsg.Text = string.Format(Resources.Messages.StockAdjustmentIsAlreadyDone, txtAdmissionDate.Text);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }
                            else if (result == -35)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_RecvedQtyLessIssueQty").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }
                            else if (result == -36)
                            {
                                litErrorMsg.Text = Resources.Messages.CannotModifyHaveReference;//Cannot modify,some of the items were referenced in other pages
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }
                            else if (result == -37)
                            {
                                litErrorMsg.Text = Resources.Messages.MsgRefAdded;//Supplier Ref# already entered
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                                return;
                            }
                            else if (result == (int)DbSaveStatus.CHECKMIEXIST)
                            {
                                litErrorMsg.Text = Resources.Messages.MINotExist;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.DATEOVERLAP)
                            {
                                litErrorMsg.Text = Resources.Messages.GRNDateAndMIDateValidation;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == -38)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ReqQtyMorethanPOQtyConfirmation('"+ commonActions+"');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString());

                                }
                                else
                                {
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (!ConfirmFutureDate(commonActions))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ConfirmFutureDate", "ConfirmFutureDate('" + ActionsEnum.SAVESUBMIT.ToString() + "');", true);
                            return;
                        }
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
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtAdmissionDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                            if (IsInventoryLocked == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            #endregion
                            DirectStockAdmissionViewState = (DirectStockAdmissionBO.DirectStockAdmission)SetUIValuesToObject(out validFlagStockAdmissionList);
                            if (hdfIsStockExceeded.Value == "1" && hdfMenuType.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("ReturnQtyExceedsStock").ToString() + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if (DirectStockAdmissionViewState.StockAdmissionList.Count == 0)
                            {
                                if (!IsValidMaterialReturn(DirectStockAdmissionViewState))
                                {
                                    litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                    return;
                                }
                            }
                            if (hdfMenuType.Value == "1")
                            {
                                if (!(IsValidGRNList(DirectStockAdmissionViewState) || IsValidMaterialReturn(DirectStockAdmissionViewState)))
                                {
                                    litErrorMsg.Text = (GetLocalResourceObject("Err_QtyZero")).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                    return;
                                }
                                int ItemTypeCount = DirectStockAdmissionViewState.StockAdmissionList.Select(x => x.GRD_WIH_ITEM_TYPE).Distinct().Count();
                                if (ItemTypeCount > 1)
                                {
                                    litErrorMsg.Text = (GetLocalResourceObject("Err_SameItem")).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.Captions.Information + "');", true);
                                    return;
                                }
                                //if (!IsValidReturnForWO())
                                //{
                                //    litErrorMsg.Text = (GetLocalResourceObject("Err_NoBatchSelected")).ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                //        + "','" + Resources.Captions.Information + "');", true);
                                //    return;
                                //}
                            }
                            if (validFlagStockAdmissionList == false)
                            {
                                if (DirectStockAdmissionViewState.MaterialReturnList.Count == 0)
                                {
                                    return;
                                }
                            }

                            SaveTransaction(DirectStockAdmissionViewState, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                            #region Commented for new workflow
                            //xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(DirectStockAdmissionViewState);
                            //lstResult = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.SaveStockTransferDtls(xmlDoc.InnerXml);

                            //result = Convert.ToInt32(lstResult[0]);                          
                            //if (result > 0)
                            //{
                            //    #region ATTACHMENT SAVE
                            //    if (DSAUploadList != null && DSAUploadList.Count > 0)
                            //    {
                            //        savePath = string.Empty;
                            //        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                            //        {
                            //            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                            //            if (!Directory.Exists(savePath))
                            //                Directory.CreateDirectory(savePath);
                            //            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                            //        }
                            //        else
                            //        {
                            //            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                            //        }

                            //        foreach (DirectStockAdmissionUploads obj in DSAUploadList)
                            //        {
                            //            string filePath = savePath + obj.AttachmentFileName;
                            //            FileInfo attachedFileInfo = new FileInfo(filePath);
                            //            if (FileDetailsList != null)
                            //            {
                            //                FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                            //                if (fileDetailsObj != null)
                            //                {
                            //                    fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                            //                }
                            //            }
                            //        }
                            //    }
                            //    #endregion
                            //    ucrWrkf.ApplicationID = CurrPK = result;
                            //}
                            //else
                            //{
                            //    #region Validation From SQL 
                            //    if (result == (int)DbSaveStatus.SQLERROR)
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //            + "','" + Resources.Captions.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                            //    {
                            //        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "');", true);
                            //    }
                            //    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            //    {
                            //        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.AlreadyDeleted;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //            + "','" + Resources.Captions.Information + "');", true);
                            //        EntryStatus = EntryStatus.LISTMODE;
                            //    }
                            //    else if (result == (int)DbSaveStatus.CODEEXIST)
                            //    {
                            //        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "','" + "');", true);
                            //    }
                            //    else if (result == -10)
                            //    {
                            //        //litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.MayAffectStockValueConfirmation;
                            //        // Include Popup for Yes/No
                            //        if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            //        {
                            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmStockValueChange", "ClosePopup();fnConfirmStockValueChange('SAVESUBMIT');", true);
                            //        }
                            //        else
                            //        {
                            //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmStockValueChange", "ClosePopup();fnConfirmStockValueChange('SUBMIT');", true);
                            //        }

                            //        return;
                            //    }
                            //    else if (result == -31)
                            //    {
                            //        litErrorMsg.Text = string.Format(Resources.Messages.StockTransferAlreadyDone, txtAdmissionDate.Text);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "','" + "');", true);
                            //        return;
                            //    }
                            //    else if (result == -32)
                            //    {
                            //        litErrorMsg.Text = string.Format(Resources.Messages.StockAdjustmentIsAlreadyDone, txtAdmissionDate.Text);
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "','" + "');", true);
                            //        return;
                            //    }
                            //    else if (result == -35)
                            //    {
                            //        litErrorMsg.Text = GetLocalResourceObject("InsufficientStockQuantity").ToString();
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "','" + "');", true);
                            //        return;
                            //    }
                            //    else if (result == -36)
                            //    {
                            //        litErrorMsg.Text = GetLocalResourceObject("CannotModifyHaveReference").ToString();//Cannot modify,some of the items were referenced in other pages
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "','" + "');", true);
                            //        return;
                            //    }
                            //    else if (result == -37)
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.MsgRefAdded;//Supplier Ref# already entered
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //        + "','" + Resources.Captions.Information + "','" + "');", true);
                            //        return;
                            //    }
                            //    else
                            //    {
                            //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            //        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());
                            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            //            + "','" + Resources.Captions.Information + "');", true);
                            //        return;
                            //    } 
                            //    #endregion
                            //} 
                            #endregion
                        }
                        else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel
                        {
                            #region Cancel Submit Codes
                            #region Checking :Inventory Transaction Locking
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtAdmissionDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                            if (IsInventoryLocked == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            #endregion
                            if (BusinessLogic.StoreManagement.DirectStockAdmissoinBL.ValidationForCancellationDSA(CurrPK))
                            {
                                //ucrWrkf.ApplicationID = CurrPK;
                                isCancelled = true;
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_DSA_Cancel").ToString();
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
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtAdmissionDate.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
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


                        #region Commented for new workflow
                        ///////////////
                        //ucrWrkf.ApplicationID = CurrPK;
                        //if (ucrWrkf.ApplicationID > 0)
                        //{
                        //    ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                        //    //Do WorkFlow if WorkFlow has Actions
                        //    if (ddlWkfAction.Items.Count > 0)
                        //    {
                        //        action = ddlWkfAction.SelectedItem.ToString();
                        //        result = ucrWrkf.DoWorkFlow();
                        //        if (result > 0)
                        //        {
                        //            SetCancelRef(CurrPK);
                        //            ucrWrkf.FillWorkFlowDetails();
                        //            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        //                ucrWrkf.ViewType = 1;
                        //            else
                        //            {
                        //                ucrWrkf.ViewType = 0;
                        //                //EntryStatus = EntryStatus.VIEWMODE;
                        //            }
                        //            ucrWrkf.ViewAction();

                        //            litErrorMsg.Text = GetLocalResourceObject("Msg_SubmitSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                        //            string admissionNo = lstResult.Count > 1 ? (lstResult[1]).ToString() : lblDirectStockAdmissionNo.Text;

                        //            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString(), admissionNo);


                        //            if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                        //            {
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //                        + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                        //            }
                        //            else
                        //            {
                        //                ResetForm(ControlEnums.CLEAR);
                        //                ResetForm(ControlEnums.CLEARSEARCH);
                        //                FillProcessID(0, 1);
                        //                GetFieldValues(ControlEnums.LIST);
                        //                SetFieldValues(ControlEnums.LIST);
                        //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        //            }
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    //Trx not saved
                        //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "');", true);
                        //} 
                        #endregion


                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.DeleteStockTransfer(CurrPK, DirectStockAdmissionProp.LAST_MOD_DT);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            if (hdfMenuType.Value == "1")
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString());
                            else
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                if (hdfMenuType.Value == "1")
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString();

                                else
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString();
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
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " +
                                       GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                }

                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();

                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                }

                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " +
                                   GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                }

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                if (hdfMenuType.Value == "1")
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " +
                                GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                }
                                else
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                if (hdfMenuType.Value == "1")
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString());

                                else
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        if (ValidateMultipleWoSelection(true) == true)
                        {
                            foreach (GridViewRow grdrow in grdPOs.Rows)
                            {
                                CheckBox chk;
                                chk = (CheckBox)grdrow.FindControl("chkSelectPOList");
                                if (chk.Checked)
                                {
                                    //if ((GetNullableInt(((HiddenField)grdrow.FindControl("hdfPOListSelected")).Value)??0) == 1)
                                    //{
                                    //    litErrorMsg.Text = GetLocalResourceObject("Err_RowAlreadySelected").ToString();
                                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    //    return;
                                    //}

                                    // poId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOIdPOList")).Value);
                                    poDetId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPODetIdPOList")).Value);
                                    if (StockAdmissionItemList.Where(x => x.PODetailId == poDetId).Count() == 0)
                                    {
                                        poDetIdList.Add(poDetId);
                                    }
                                }
                            }
                            if (IsSelectedPOValid(poDetIdList))
                            {
                                if (poDetIdList.Count > 0) // (poIdList.Count > 0)
                                {
                                    UpdateStockAdmissionItemListFromGridview(false);
                                    List<DirectStockAdmissionBO.PendingPO> pendingPOTemp = new List<DirectStockAdmissionBO.PendingPO>();
                                    pendingPOTemp = PendingPOList;
                                    pendingPOTemp.Where(x => poDetIdList.Contains(x.PODetId))
                                       .ToList()
                                       .ForEach(l => { l.AddedToStockList = true; l.CheckBoxChecked = true; });
                                    PendingPOList = pendingPOTemp;

                                    List<DirectStockAdmissionBO.PendingPO> lstNewSelectedPOs = PendingPOList
                                        .Where(x => poDetIdList.Contains(x.PODetId)) // .Where(x => poIdList.Contains(x.POId))
                                        .ToList();
                                    int slno = 0;
                                    if (StockAdmissionItemList.Count > 0)
                                    {
                                        slno = StockAdmissionItemList.Max(x => x.SlNo);
                                    }

                                    List<DirectStockAdmissionBO.StockAdmissionItem> tempNewList = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                                    tempNewList = lstNewSelectedPOs
                                        .Select(x => new DirectStockAdmissionBO.StockAdmissionItem
                                        {
                                            Pk = 0,
                                            // SlNo = (slno + 1),
                                            POId = x.POId,
                                            PODetailId = x.PODetId,
                                            PONumber = x.PONumber,
                                            ItemId = x.ItemId,
                                            ItemName = x.ItemName,
                                            UOM = x.UOMId,
                                            POQty = x.POQty,
                                            PendingQty = x.BalanceQtyToRecieve,
                                            Recieved = x.BalanceQtyToRecieve,
                                            Accepted = x.BalanceQtyToRecieve,
                                            PORate = x.PORate,
                                            WIH_ITEM_TYPE = x.WIH_ITEM_TYPE,
                                            WIH_ITEM_TYPE_TEXT = x.WIH_ITEM_TYPE_TEXT,
                                        })
                                        .ToList();

                                    foreach (var item in tempNewList) item.SlNo = ++slno;

                                    tempStockAdmissionItemList = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                                    tempStockAdmissionItemList = StockAdmissionItemList;
                                    tempStockAdmissionItemList.AddRange(tempNewList);
                                    StockAdmissionItemList = tempStockAdmissionItemList;

                                    //List<DirectStockAdmissionBO.DirectStockAdmission> 
                                    //foreach (var item in tempNewList)
                                    //{
                                    //    StockAdmissionItemList.ToList().AddRange Add(item);
                                    //}                            
                                    BindGrid(ControlEnums.STOCKADMISSIONLIST);
                                    BindGrid(ControlEnums.POS);
                                }
                                else if (poDetIdList.Count < 1 && StockAdmissionItemList.Count > 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_RecordsAlreadyAdded").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (poDetIdList.Count < 1 && StockAdmissionItemList.Count < 1)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsSelected").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsSelected").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }

                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_SameItemDiffRate").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "MultipleWoNotAllowed").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region "SAVE_ACTIONPOPUP"
                    case ActionsEnum.SAVE_ACTIONPOPUP:
                        poDetId = GetNullableInt(hdfCurrentPODetIdAddRejectedReason.Value) ?? 0;
                        tempStockAdmissionItemList = StockAdmissionItemList;
                        DirectStockAdmissionBO.StockAdmissionItem tempStockAdmissionItem = tempStockAdmissionItemList
                            .Where(x => x.PODetailId == poDetId)
                            .Single();

                        decimal currentQuantity = GetNullableDecimal(txtDamageQtyPopup.Text) ?? 0;
                        int reason = GetNullableInt(ddlRejectionReasonPopup.SelectedValue).Value;
                        int rejectToStore = GetNullableInt(ddlRejectToDept.SelectedValue).Value;
                        int slNo = GetNullableInt(hdfDamageSlNo.Value) ?? 0;

                        DirectStockAdmissionBO.RejectedDetail rejectedDetails;
                        if (slNo == 0) // New Entry
                        {
                            if (tempStockAdmissionItem.RejectedList == null) tempStockAdmissionItem.RejectedList = new List<DirectStockAdmissionBO.RejectedDetail>();

                            if ((tempStockAdmissionItem.RejectedList.Sum(x => x.Quantity) + currentQuantity) > tempStockAdmissionItem.Rejected)
                            {
                                ShowRejectedPopup();
                                litErrorMsg.Text = GetLocalResourceObject("Err_TotalQuantityExceeds").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            if (tempStockAdmissionItem.RejectedList.Count > 0)
                            {
                                slNo = tempStockAdmissionItem.RejectedList.Max(x => x.SlNo);
                            }
                            ++slNo;

                            rejectedDetails = new DirectStockAdmissionBO.RejectedDetail();
                            rejectedDetails.Pk = 0;
                            rejectedDetails.SlNo = slNo;
                            rejectedDetails.HdrSlNo = tempStockAdmissionItem.SlNo;
                            rejectedDetails.PODetailId = poDetId;
                            rejectedDetails.ItemId = tempStockAdmissionItem.ItemId;
                            rejectedDetails.ItemText = tempStockAdmissionItem.ItemName;
                            rejectedDetails.Quantity = currentQuantity;
                            rejectedDetails.ReasonId = reason;
                            rejectedDetails.ReasonText = ddlRejectionReasonPopup.SelectedItem.Text;
                            rejectedDetails.RejectedToStoreId = rejectToStore;
                            rejectedDetails.RejectedToStoreText = ddlRejectToDept.SelectedItem.Text;
                            tempStockAdmissionItem.RejectedList.Add(rejectedDetails);
                        }
                        else // Updation
                        {
                            rejectedDetails = tempStockAdmissionItem.RejectedList
                                .Where(x => x.SlNo == slNo)
                                .Single();
                            if (((tempStockAdmissionItem.RejectedList.Sum(x => x.Quantity) + currentQuantity) - rejectedDetails.Quantity) > tempStockAdmissionItem.Rejected)
                            {
                                ShowRejectedPopup();
                                litErrorMsg.Text = GetLocalResourceObject("Err_TotalQuantityExceeds").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            rejectedDetails.Quantity = currentQuantity;
                            rejectedDetails.ReasonId = reason;
                            rejectedDetails.ReasonText = ddlRejectionReasonPopup.SelectedItem.Text;
                            rejectedDetails.RejectedToStoreId = rejectToStore;
                            rejectedDetails.RejectedToStoreText = ddlRejectToDept.SelectedItem.Text;
                        }
                        StockAdmissionItemList = tempStockAdmissionItemList;
                        hdfDamageSlNo.Value = string.Empty;
                        BindGrid(ControlEnums.REJECTEDREASONLIST);
                        ShowRejectedPopup();
                        break;
                    #endregion
                    #region "CLEARITEM"
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlEnums.CLEARREJECTCONTROLS);
                        ShowRejectedPopup();
                        break;
                    #endregion
                    #region "ADDITEM"
                    case ActionsEnum.ADDITEM:
                        GridViewPOChangeColour();
                        break;
                    #endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        // ResetForm(ControlEnums.VIEW);
                        ActionHandler(btnEdit, EventArgs.Empty);
                        if (EntryStatus == EntryStatus.EDITMODE) this.EntryStatus = EntryStatus.VIEWMODE;
                        break;
                    #endregion
                    #region Printlisting
                    case ActionsEnum.PRINTLISTING:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfStockTransferPk")).Value);
                                if (CurrPK > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + ApplicationType.DSA) + "');", true);
                                    return;
                                }
                            }
                        }
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        break;
                    #endregion
                    #region ADDITEM
                    case ActionsEnum.ADDITEMUPLOAD:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            BindStockAdmissionItemListFromGridview();

                            if (fupUpload.HasFile)
                            {
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);

                                if (!IsValidExtension(tempFileInfoObj.Extension))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    if (CurrSlNo != 0)
                                    {
                                        if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                        {
                                            DSAUploadObj = DSAUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                            if (DSAUploadObj != null)
                                            {
                                                if (FileDetailsList == null)
                                                {
                                                    FileDetailsList = new List<FileDetails>();
                                                }
                                                if (fupUpload.HasFile)
                                                {

                                                    tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                                    string attachmentFileFormat = tempFileInfoObj.Extension;
                                                    string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    DSAUploadObj.AttachmentFileName = attachmentFileName;
                                                    DSAUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                    DSAUploadObj.DOC_NAME = fupUpload.FileName;
                                                    DSAUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        DSAUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        DSAUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                                    }
                                                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                                    if (fileDetailsObj == null)
                                                    {
                                                        FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, PoFile = HttpContext.Current.Request.Files[0] });
                                                    }
                                                    else
                                                    {
                                                        fileDetailsObj.PoFile = HttpContext.Current.Request.Files[0];
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
                                            if (DSAUploadList == null || DSAUploadList.Count == 0)
                                            {
                                                DSAUploadList = new List<BusinessObject.StoreManagement.DirectStockAdmissionUploads>();
                                                slno = 1;
                                            }
                                            else
                                            {
                                                slno = DSAUploadList.Max(itm => itm.DOC_SEQ_NO);
                                                slno++;
                                            }
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<FileDetails>();
                                            }

                                            DSAUploadObj = new DirectStockAdmissionUploads();
                                            DSAUploadObj.DOC_PK = 0;
                                            DSAUploadObj.DOC_SEQ_NO = slno;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                            DSAUploadObj.AttachmentFileName = attachmentFileName;
                                            DSAUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            DSAUploadObj.DOC_NAME = fupUpload.FileName;
                                            DSAUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                DSAUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                DSAUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }
                                            DSAUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                            DSAUploadList.Add(DSAUploadObj);

                                        }
                                    }
                                }
                            }
                            BindGrid(ControlEnums.UPLOADEDFILES); // SetFieldValues(ControlEnums.UPLOADEDFILES);
                            BindGrid(ControlEnums.STOCKADMISSIONLIST);
                            ResetForm(ControlEnums.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);
                            grdUploads.Focus();
                        }
                        break;
                    #endregion
                    #region REMOVEITEMUPLOAD
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (DSAUploadList != null && DSAUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                DSAUploadList = DSAUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                BindGrid(ControlEnums.UPLOADEDFILES);
                                ResetForm(ControlEnums.ADDITEM);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

                        break;
                    #endregion
                    #region EDITITEMUPLOAD
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (DSAUploadList != null && DSAUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                DSAUploadObj = DSAUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlEnums.SELECTEDDOC);
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideUploadDocDetails(1);});", true);

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
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfStockTransferPk")).Value);
                                grdListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDept")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ucrWrkf.Reset();
                            FillProcessID(grdListRowDeptId, 12);
                            SetUIEditView(commonActions);
                            // EntryStatus = EntryStatus.EDITMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                                ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();

                            GetFieldValues(ControlEnums.SUPPLIERS);
                            SetFieldValues(ControlEnums.SUPPLIERS);
                            GetFieldValues(ControlEnums.POS);
                            SetFieldValues(ControlEnums.POS);
                            GetFieldValues(ControlEnums.DETAILFOREDIT);
                            SetFieldValues(ControlEnums.DETAILFOREDIT);
                            SetFieldValues(ControlEnums.UPLOADEDFILES);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CONVERT
                    case ActionsEnum.CONVERT:
                        GRNConversion conv = new GRNConversion();
                        conv.ReceivedItemList = new List<ConvertItems>();
                        conv.ConvertItemList = new List<ConvertItems>();

                        conv.UserPK = currentUser.PKUser;
                        conv.SBUPK = currentUser.SBUID;
                        conv.ConvertDate = DateTime.Now;

                        foreach (GridViewRow grdrow in grdReceived.Rows)
                        {
                            int ItemPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemPK")).Value);
                            int UOMPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfUOMPK")).Value);
                            int SBDPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSBDPK")).Value);
                            conv.GRNPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGRNPK")).Value);
                            decimal ReceivedQty = Convert.ToDecimal(((Label)grdrow.FindControl("lblReceivedQty")).Text);
                            conv.ReceivedItemList.Add(new ConvertItems()
                            {
                                ItemPK = ItemPK,
                                UomPK = UOMPK,
                                StockPK = SBDPK,
                                Quantity = ReceivedQty
                            });
                        }

                        foreach (GridViewRow grdrow in grdConvert.Rows)
                        {
                            ConvertItems item = new ConvertItems();
                            int ItemPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfConvItemPK")).Value);
                            int UOMPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfConvUOMPK")).Value);
                            decimal ConvQty = Convert.ToDecimal(((TextBox)grdrow.FindControl("txtConvQty")).Text);
                            conv.ConvertItemList.Add(
                                new ConvertItems()
                                {
                                    ItemPK = ItemPK,
                                    UomPK = UOMPK,
                                    Quantity = ConvQty
                                });
                        }

                        //conv.ReceivedItemList = ReceivedList;
                        //conv.ConvertItemList = ConvertList;

                        string strTrxNo = string.Empty;
                        string xmlDocument = CommonFunctions.XmlSerialize<GRNConversion>(conv);
                        result = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.ConvertGRN(xmlDocument);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("ConvertedSucessfully").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                        }
                        else
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETECONVERSION
                    case ActionsEnum.DELETECONVERSION:
                        int GRNPK = 0;
                        foreach (GridViewRow grdrow in grdReceived.Rows)
                        {
                            GRNPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfGRNPK")).Value);
                            break;
                        }
                        result = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.DeleteGRNConversion(GRNPK);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("DeletedSucessfully").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONVERSIONUSING)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ConversionUsing").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDITFORRETURN
                    case ActionsEnum.EDITFORRETURN:


                        if (CurrPK > 0)
                        {
                            MaterialReturnList = DirectStockAdmissionProp.MaterialReturnList;
                            SetFieldValues(ControlEnums.EDITFORRETURN);
                        }
                        else
                        {
                            if (ValidateMultipleWoSelection(false) == true)
                            {
                                bIsChecked = false;
                                int WOPK = 0;
                                int ItemPK = 0;
                                foreach (GridViewRow grdrow in grdPOs.Rows)
                                {
                                    CheckBox chkSelect;
                                    chkSelect = (CheckBox)grdrow.FindControl("chkSelectPOList");
                                    if (chkSelect.Checked)
                                    {
                                        bIsChecked = true;
                                        WOPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOIdPOList")).Value);
                                        ItemPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemId")).Value);
                                        WOPKs.Add(WOPK);
                                        WO_Item_List.Add(WOPK, ItemPK);

                                        DirectStockAdmissionBO.WOHeaderList returnItem = new DirectStockAdmissionBO.WOHeaderList();
                                        returnItem.WOPK = WOPK;
                                        returnItem.WOItemPK = ItemPK;
                                        if (ListForReturn == null)
                                            ListForReturn = new List<DirectStockAdmissionBO.WOHeaderList>();

                                        if (!ListForReturn.Where(w => w.WOPK == WOPK && w.WOItemPK == ItemPK).Any())
                                        {
                                            ListForReturn = new List<DirectStockAdmissionBO.WOHeaderList>();
                                            ListForReturn.Add(returnItem);
                                        }
                                    }
                                }
                                if (bIsChecked)
                                {
                                    GetFieldValues(ControlEnums.EDITFORRETURN);
                                    SetFieldValues(ControlEnums.EDITFORRETURN);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "MultipleWoNotAllowed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                        }
                        TabNo = int.Parse(((LinkButton)sender).CommandArgument.ToString());
                        SwitchTab(TabNo);
                        break;
                    #endregion
                    #region DEFAULT
                    case ActionsEnum.DEFAULT:
                        TabNo = int.Parse(((LinkButton)sender).CommandArgument.ToString());
                        SwitchTab(TabNo);
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        int rowIndex = (((DropDownList)sender).Parent.Parent as GridViewRow).RowIndex;
                        DropDownList _ddlBatch = ((DropDownList)grdMaterialReturn.Rows[rowIndex].FindControl("ddlBatch"));
                        BatchPK = Convert.ToInt32(_ddlBatch.SelectedValue.Split('_')[0]);
                        int WIH_PK = 0;
                        int BOM_PK = 0;
                        if (_ddlBatch.SelectedValue.Contains("_"))
                        {
                            WIH_PK = Convert.ToInt32(_ddlBatch.SelectedValue.Split('_')[1]);
                            BOM_PK = _ddlBatch.SelectedValue.Split('_')[2] == null ? 0 : Convert.ToInt32(_ddlBatch.SelectedValue.Split('_')[2]);
                        }
                        //string BatchSelected = _ddlBatch.SelectedValue; // Convert.ToInt32(((DropDownList)grdMaterialReturn.Rows[rowIndex].FindControl("ddlBatch")).SelectedValue);
                        ItemPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfIssueItemPK")).Value);
                        ItemTypePK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfItemTypePK")).Value);
                        int SlNo = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfMatSlNo")).Value);
                        string Batch = _ddlBatch.SelectedItem.Text;// ((DropDownList)grdMaterialReturn.Rows[rowIndex].FindControl("ddlBatch")).SelectedItem.Text;
                        IsAfterMulti = Convert.ToInt32((grdMaterialReturn.Rows[rowIndex].FindControl("hdfIsAfterMulti") as HiddenField).Value);

                        MaterialReturn = MaterialReturnList.Where(w => w.SlNo == SlNo).FirstOrDefault();
                        GetFieldValues(ControlEnums.FILLBATCHPOPUPDTLS);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                            {
                                dtPageData = dtPageData.Select("SBD_PK=" + BatchPK.ToString() + " AND WIH_PK=" + WIH_PK.ToString() + " AND WIB_PK=" + BOM_PK.ToString()).Any()
                                    ? dtPageData.Select("SBD_PK=" + BatchPK.ToString() + " AND WIH_PK=" + WIH_PK.ToString() + " AND WIB_PK=" + BOM_PK.ToString()).CopyToDataTable()
                                    : dtPageData.Clone();
                            }
                            else if (ItemTypePK == 2) //Product
                            {
                                dtPageData = dtPageData.Select("BID_BIN_CARD=" + BatchPK.ToString() + " AND WIH_PK=" + WIH_PK.ToString() + " AND WIB_PK=" + BOM_PK.ToString()).Any()
                                    ? dtPageData.Select("BID_BIN_CARD=" + BatchPK.ToString() + " AND WIH_PK=" + WIH_PK.ToString() + " AND WIB_PK=" + BOM_PK.ToString()).CopyToDataTable()
                                    : dtPageData.Clone();
                            }
                        }
                        double stock = 0;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            if (double.TryParse(dtPageData.Rows[0]["BALANCE"].ToString(), out stock))
                            {
                                ((Label)grdMaterialReturn.Rows[rowIndex].FindControl("lblReturnStockQty")).Text = GetFormattedNumber(stock);
                                ((TextBox)grdMaterialReturn.Rows[rowIndex].FindControl("lblReturnQty")).Text = GetFormattedNumber(stock);
                            }
                            ((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfBatchUOMPK")).Value = dtPageData.Rows[0]["SBD_UOM"].ToString();
                        }
                        else
                        {
                            ((Label)grdMaterialReturn.Rows[rowIndex].FindControl("lblReturnStockQty")).Text = string.Empty;
                            ((TextBox)grdMaterialReturn.Rows[rowIndex].FindControl("lblReturnQty")).Text = GetFormattedNumber(MaterialReturn.RequiredQty);
                            ((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfBatchUOMPK")).Value = "0";
                            ((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfBatchUOM")).Value = string.Empty;
                        }

                        if (BatchPK > 0 && (MaterialReturn.BatchDetailList == null || MaterialReturn.BatchDetailList.Count == 0))
                        {
                            DirectStockAdmissionBO.BatchDetails batch = new DirectStockAdmissionBO.BatchDetails();
                            if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                                batch.BatchPK = BatchPK;
                            else if (ItemTypePK == 2) //Product
                                batch.BinCardPK = BatchPK;
                            batch.Batch = Batch;
                            batch.WOPK = WIH_PK;
                            batch.BOMPK = BOM_PK;
                            batch.Item = lblItemPopup.Text;
                            batch.ItemPK = ItemPK;
                            batch.StockQty = Convert.ToDecimal(GetFormattedNumber(stock));
                            batch.ActualQty = Convert.ToDecimal(GetFormattedNumber(stock));
                            batch.SlNo = SlNo;
                            batch.UOMPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfBatchUOMPK")).Value);

                            MaterialReturn.GMR_AFTER_MULTI = 1;
                            MaterialReturn.BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();
                            MaterialReturn.ReturnQty = batch.ActualQty;
                            MaterialReturn.BatchDetailList.Add(batch);
                        }
                        else if (BatchPK > 0 && (MaterialReturn.BatchDetailList != null && MaterialReturn.BatchDetailList.Any()))
                        {
                            DirectStockAdmissionBO.BatchDetails batch = new DirectStockAdmissionBO.BatchDetails();
                            if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                                batch.BatchPK = BatchPK;
                            else if (ItemTypePK == 2) //Product
                                batch.BinCardPK = BatchPK;
                            batch.Batch = Batch;
                            batch.WOPK = WIH_PK;
                            batch.BOMPK = BOM_PK;
                            batch.Item = lblItemPopup.Text;
                            batch.ItemPK = ItemPK;
                            batch.StockQty = Convert.ToDecimal(GetFormattedNumber(stock));
                            batch.ActualQty = Convert.ToDecimal(GetFormattedNumber(stock));
                            batch.SlNo = SlNo;
                            batch.UOMPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[rowIndex].FindControl("hdfBatchUOMPK")).Value);

                            List<DirectStockAdmissionBO.BatchDetails> _batchDetailsList = new List<DirectStockAdmissionBO.BatchDetails>();
                            _batchDetailsList.Add(batch);

                            MaterialReturn.GMR_AFTER_MULTI = 1;
                            MaterialReturn.BatchDetailList = _batchDetailsList;
                            MaterialReturn.ReturnQty = batch.ActualQty;
                        }
                        else
                        {
                            MaterialReturn.BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();
                        }
                        RestoreReturnGridValue();
                        break;
                    #endregion
                    #region ADD_ACTION
                    case ActionsEnum.ADD_ACTION:
                        int GridRowIndex = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
                        ItemPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[GridRowIndex].FindControl("hdfIssueItemPK")).Value);
                        ItemTypePK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[GridRowIndex].FindControl("hdfItemTypePK")).Value);
                        int currentWO = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[GridRowIndex].FindControl("hdfMatWOPK")).Value);
                        int currentSlNo = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[GridRowIndex].FindControl("hdfMatSlNo")).Value);
                        decimal returnQty = ((TextBox)grdMaterialReturn.Rows[GridRowIndex].FindControl("lblReturnQty")).Text == "" || ((TextBox)grdMaterialReturn.Rows[GridRowIndex].FindControl("lblReturnQty")).Text == string.Empty ? 0 : Convert.ToDecimal(((TextBox)grdMaterialReturn.Rows[GridRowIndex].FindControl("lblReturnQty")).Text);
                        decimal requiredQty = Convert.ToDecimal(((Label)grdMaterialReturn.Rows[GridRowIndex].FindControl("lblRequiredQty")).Text);
                        DropDownList ddlBatchTemp = (DropDownList)grdMaterialReturn.Rows[GridRowIndex].FindControl("ddlBatch");
                        int batchSelectVal = 0;
                        if (ddlBatchTemp.Items.Count > 0)
                            batchSelectVal = Convert.ToInt32(ddlBatchTemp.SelectedValue.Split('_')[0]);
                        BatchPK = batchSelectVal;
                        string strStock = ((Label)grdMaterialReturn.Rows[GridRowIndex].FindControl("lblReturnStockQty")).Text;
                        decimal StockQty = Convert.ToDecimal(strStock == "" ? "0" : strStock);
                        int BatchUOMPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[GridRowIndex].FindControl("hdfBatchUOMPK")).Value);
                        int CurrentWOPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[GridRowIndex].FindControl("hdfMatWOPK")).Value);
                        ReturnWOPK = CurrentWOPK;

                        //MaterialReturn = MaterialReturnList.Where(w => w.IssueItemPK == ItemPK && w.WOPK == currentWO).FirstOrDefault();
                        MaterialReturn = MaterialReturnList.Where(w => w.SlNo == currentSlNo).FirstOrDefault();

                        BatchDetailList = null;

                        if (MaterialReturn.BatchDetailList == null)
                            MaterialReturn.BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();
                        BatchDetailList = MaterialReturn.BatchDetailList;
                        foreach (DirectStockAdmissionBO.BatchDetails batch in BatchDetailList)
                        {
                            if (!string.IsNullOrEmpty(batch.WOPKs))
                            {
                                List<int> WOIDs = batch.WOPKs.Split(',').Select(int.Parse).ToList();
                                int WOID = batch.WOPK;
                                bool isInList = WOIDs.IndexOf(WOID) != -1;
                                if (!isInList)
                                    batch.HasRowcolor = true;
                            }
                            else
                                batch.HasRowcolor = true;
                        }
                        lblItemPopup.Text = MaterialReturn.IssueItem;
                        hdfSlNo.Value = MaterialReturn.SlNo.ToString();
                        hdfItemPopup.Value = MaterialReturn.IssueItemPK.ToString();
                        hdfItemTypePopup.Value = ItemTypePK.ToString();
                        lblUomNamePopup.Text = MaterialReturn.UOM;
                        hdfUOMPopup.Value = MaterialReturn.UOMPK.ToString();
                        lblTotalQtyPopup.Text = GetFormattedNumber(requiredQty);
                        hdfCurrentWOPK.Value = CurrentWOPK.ToString();
                        lblBalanceQty.Text = GetFormattedNumber((requiredQty - BatchDetailList.Sum(s => s.ActualQty)).ToString());
                        BindGrid(ControlEnums.FILLBATCHGRID);
                        IsAfterMulti = 1;
                        GetFieldValues(ControlEnums.FILLITEMBATCHSPOPUP);
                        SetFieldValues(ControlEnums.FILLITEMBATCHSPOPUP);
                        showPopup();
                        break;
                    #endregion
                    #region CLEARADD
                    case ActionsEnum.CLEARADD:
                        int index = (((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex;
                        int removeSlNo = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[index].FindControl("hdfMatSlNo")).Value);
                        MaterialReturn = MaterialReturnList.Where(w => w.SlNo == removeSlNo).FirstOrDefault();
                        // BatchDetailList = MaterialReturn.BatchDetailList;
                        // BatchDetailList.RemoveRange(1, BatchDetailList.Count - 1);
                        //MaterialReturnList.Where(w => w.SlNo == removeSlNo).FirstOrDefault().BatchDetailList = BatchDetailList;
                        //((Label)grdMaterialReturn.Rows[index].FindControl("lblReturnStockQty")).Text = BatchDetailList.First().StockQty.ToString();

                        MaterialReturnList.Where(w => w.SlNo == removeSlNo).FirstOrDefault().BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();
                        MaterialReturnList.Where(w => w.SlNo == removeSlNo).FirstOrDefault().ReturnQty = 0;
                        ((Label)grdMaterialReturn.Rows[index].FindControl("lblReturnStockQty")).Text = "";
                        ((TextBox)grdMaterialReturn.Rows[index].FindControl("lblReturnQty")).Text = "0.00";
                        IsAfterMulti = Convert.ToInt32((grdMaterialReturn.Rows[index].FindControl("hdfIsAfterMulti") as HiddenField).Value);
                        ReturnWOPK = Convert.ToInt32((grdMaterialReturn.Rows[index].FindControl("hdfMatWOPK") as HiddenField).Value);

                        ItemPK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[index].FindControl("hdfIssueItemPK")).Value);
                        ItemTypePK = Convert.ToInt32(((HiddenField)grdMaterialReturn.Rows[index].FindControl("hdfItemTypePK")).Value);

                        GetFieldValues(ControlEnums.FILLITEMBATCHSPOPUP);
                        DropDownList ddlBatch = grdMaterialReturn.Rows[index].FindControl("ddlBatch") as DropDownList;

                        if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                            ddlBatch.DataValueField = "BATCH_WO_NO";// "SBD_PK";
                        else if (ItemTypePK == 2) //Product
                            ddlBatch.DataValueField = "BATCH_WO_NO";// "BID_BIN_CARD";
                        ddlBatch.DataTextField = "SBD_BATCH_NO";
                        ddlBatch.DataSource = dtPageData;
                        ddlBatch.DataBind();
                        ddlBatch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));

                        // ddlBatch.SelectedValue = BatchDetailList.First().BatchPK.ToString();
                        grdMaterialReturn.Rows[index].Cells[5].Enabled = true; // Enable ddlBatch
                        grdMaterialReturn.Rows[index].Cells[10].Enabled = true; // Enable Quantity Textbox

                        BatchDetailList = null;
                        ImageButton imbClearGridBatch = (ImageButton)grdMaterialReturn.Rows[index].FindControl("imbClearGridBatch") as ImageButton;
                        imbClearGridBatch.Visible = false;
                        break;
                    #endregion
                    #region BATCHCHANGE
                    case ActionsEnum.BATCHCHANGE:
                        if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                        {
                            ItemPK = Convert.ToInt32(hdfItemPopup.Value);
                            BatchPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[0]);
                            if (ddlBatchesPopUp.SelectedValue.Contains("_"))
                                selWOPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[1]);
                            ItemTypePK = Convert.ToInt32(hdfItemTypePopup.Value);
                            GetFieldValues(ControlEnums.FILLBATCHPOPUPDTLS);
                            SetFieldValues(ControlEnums.FILLBATCHPOPUPDTLS);
                        }
                        else
                        {
                            lblStockPopUp.Text = string.Empty;
                            txtQtyPopUp.Text = lblBalanceQty.Text;
                        }
                        showPopup();
                        break;
                    #endregion
                    #region Add New Batches in PoPup
                    case ActionsEnum.ADDBATCHES:
                        showPopup();
                        if (!IsSameBatchExist())
                        {
                            if (currentStockinPopup() == 0)
                            {
                                if (checkRequiredStockinPopup() == 0)
                                {
                                    DirectStockAdmissionBO.BatchDetails batch = new DirectStockAdmissionBO.BatchDetails();
                                    int popBOM_PK = 0;
                                    if (Convert.ToInt32(hdfItemTypePopup.Value) == 1 || Convert.ToInt32(hdfItemTypePopup.Value) == 4) //Material
                                    {
                                        batch.BatchPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[0]);
                                        if (ddlBatchesPopUp.SelectedValue.Contains("_"))
                                        {
                                            batch.WOPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[1]);
                                            popBOM_PK = ddlBatchesPopUp.SelectedValue.Split('_')[2] == null ? 0 : Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[2]);
                                        }
                                    }
                                    else if (Convert.ToInt32(hdfItemTypePopup.Value) == 2) //Product
                                    {
                                        batch.BinCardPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[0]);
                                        if (ddlBatchesPopUp.SelectedValue.Contains("_"))
                                        {
                                            batch.WOPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[1]);
                                            popBOM_PK = ddlBatchesPopUp.SelectedValue.Split('_')[2] == null ? 0 : Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[2]);
                                        }
                                    }
                                    batch.Batch = ddlBatchesPopUp.SelectedItem.Text;
                                    batch.Item = lblItemPopup.Text;
                                    batch.ItemPK = Convert.ToInt32(hdfItemPopup.Value);
                                    batch.StockQty = Convert.ToDecimal(lblStockPopUp.Text);
                                    batch.ActualQty = Convert.ToDecimal(txtQtyPopUp.Text);
                                    batch.SlNo = Convert.ToInt32(hdfSlNo.Value);
                                    batch.UOMPK = Convert.ToInt32(hdfUOMPopup.Value);
                                    batch.WOPKs = hdfBatchWOPKPopup.Value;
                                    batch.BOMPK = popBOM_PK;
                                    batch.WONos = lblBatchWOPopup.ToolTip;
                                    batch.HasRowcolor = hdfIsValidationRequired.Value == "1" ? true : false;

                                    if (BatchDetailList == null)
                                        BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();
                                    BatchDetailList.Add(batch);

                                    lblBalanceQty.Text = GetFormattedNumber((Convert.ToDecimal(lblTotalQtyPopup.Text) - BatchDetailList.Sum(s => s.ActualQty)).ToString());

                                    BindGrid(ControlEnums.FILLBATCHGRID);
                                    ResetForm(ControlEnums.ADDBATCHES);
                                }
                                else
                                {
                                    string msg = string.Empty;
                                    msg = GetLocalResourceObject("MaximumQtyAdded").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorStock", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msg) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorStock", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(this.GetLocalResourceObject("NotEnoughStock").ToString()) + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorStock", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(this.GetLocalResourceObject("SameBatchExist").ToString()) + "','" + Resources.Captions.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region APPLY
                    case ActionsEnum.APPLY:
                        RestoreReturnGridValue();
                        if (MaterialReturn.BatchDetailList == null)
                            MaterialReturn.BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();
                        MaterialReturn.BatchDetailList = BatchDetailList;
                        MaterialReturn.ReturnQty = BatchDetailList.Sum(s => s.ActualQty); //Convert.ToDecimal(lblTotalQtyPopup.Text); //

                        int returnSlNo = MaterialReturn.SlNo;
                        if (MaterialReturnList.Where(w => w.SlNo == returnSlNo).FirstOrDefault().BatchDetailList == null)
                            MaterialReturnList.Where(w => w.SlNo == returnSlNo).FirstOrDefault().BatchDetailList = new List<DirectStockAdmissionBO.BatchDetails>();

                        MaterialReturnList.Where(w => w.SlNo == returnSlNo).FirstOrDefault().BatchDetailList
                            = MaterialReturn.BatchDetailList;
                        //New Code

                        // MaterialReturnList.Where(w => w.SlNo == returnSlNo).FirstOrDefault().IsMultiBatch = BatchDetailList == null ? 0 : BatchDetailList.Count > 1 ? 1 : 0;

                        MaterialReturnList.Where(w => w.SlNo == returnSlNo).FirstOrDefault().ReturnQty = MaterialReturn.ReturnQty; //Convert.ToDecimal(lblTotalQtyPopup.Text);
                        MaterialReturnList.Where(w => w.SlNo == returnSlNo).FirstOrDefault().IsValueChanged = 1;
                        ResetForm(ControlEnums.FILLBATCHPOPUPDTLS);
                        BindGrid(ControlEnums.EDITFORRETURN);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();", true);
                        break;
                    #endregion
                    #region UPDATERETURN
                    case ActionsEnum.UPDATERETURN:
                        RestoreReturnGridValue();
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void RestoreReturnGridValue()
        {
            foreach (GridViewRow row in grdMaterialReturn.Rows)
            {

                ImageButton imbAddGridBatch = (ImageButton)row.FindControl("imbAddGridBatch");
                int WOPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfMatWOPK")).Value);
                int MatSlNo = Convert.ToInt32(((HiddenField)row.FindControl("hdfMatSlNo")).Value);
                string retStrVal = ((TextBox)row.FindControl("lblReturnQty")).Text;
                decimal retVal = 0;
                decimal.TryParse(retStrVal, out retVal);

                if (retVal > 0)
                {
                    MaterialReturnList.Where(w => w.SlNo == MatSlNo).FirstOrDefault().IsValueChanged = 1;
                    MaterialReturnList.Where(w => w.SlNo == MatSlNo).FirstOrDefault().ReturnQty = retVal;
                }
            }
        }
        private void SaveTransaction(DirectStockAdmissionBO.DirectStockAdmission DirectStkAdmission, int workflowFlag)
        {
            XmlDocument xmlDoc;
            List<object> lstResult = new List<object>();
            int result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string action = string.Empty;
            if (DirectStkAdmission == null)
                DirectStkAdmission = new DirectStockAdmissionBO.DirectStockAdmission();

            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            DirectStkAdmission.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            DirectStkAdmission.WKF_APPLICATION = CurrPK;
            DirectStkAdmission.WKF_COMMENTS = wkfDetails.Comments;
            DirectStkAdmission.WKF_TRX_FLAG = workflowFlag;
            DirectStkAdmission.WKF_PROCESS = wkfDetails.ProcessID;
            DirectStkAdmission.WKF_REFERENCE = wkfDetails.ReferenceID;
            DirectStkAdmission.WKF_TASK = wkfDetails.TaskID;
            DirectStkAdmission.WKF_TASK_ACTION = wkfDetails.ActionID;
            DirectStkAdmission.WKF_FLAG = 1;
            DirectStkAdmission.GRH_IS_WORK_ORDER = hdfMenuType.Value == "1" ? 1 : 0;
            action = wkfDetails.ActionText;
            #endregion

            string strTrxNo = string.Empty;
            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(DirectStkAdmission);
            lstResult = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.SaveStockTransferWkf(xmlDoc.InnerXml, out strTrxNo);
            result = Convert.ToInt32(lstResult[0]);
            if (result > 0)
            {
                if (string.IsNullOrEmpty(strTrxNo))
                    strTrxNo = lblDirectStockAdmissionNo.Text.Trim();
                #region ATTACHMENT SAVE
                if (DSAUploadList != null && DSAUploadList.Count > 0)
                {
                    savePath = string.Empty;
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

                    foreach (DirectStockAdmissionUploads obj in DSAUploadList)
                    {
                        string filePath = savePath + obj.AttachmentFileName;
                        FileInfo attachedFileInfo = new FileInfo(filePath);
                        if (FileDetailsList != null)
                        {
                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                            if (fileDetailsObj != null)
                            {
                                fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                            }
                        }
                    }
                }
                #endregion
                if (isCancelled)
                {
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                    if (hdfMenuType.Value == "1")
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString());
                    else
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_SubmitSuccess").ToString();
                    if (hdfMenuType.Value == "1")
                    {
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString(), strTrxNo);
                    }
                    else
                    {
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString(), strTrxNo);
                    }


                }
                //litErrorMsg.Text = GetLocalResourceObject("Msg_SubmitSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                //string admissionNo = lstResult.Count > 1 ? (lstResult[1]).ToString() : lblDirectStockAdmissionNo.Text;               
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
                    if (hdfMenuType.Value == "1")
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                    }
                    else
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.GRNEXCESS)
                {
                    if (hdfMenuType.Value == "1")
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.GrnExcessQty;
                    }
                    else
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.GrnExcessQty;
                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    if (hdfMenuType.Value == "1")
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.AlreadyDeleted;

                    }
                    else
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.AlreadyDeleted;

                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    if (hdfMenuType.Value == "1")
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "WODSAPageName").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;

                    }
                    else
                    {
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;

                    }
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKMIEXIST)
                {
                    litErrorMsg.Text = Resources.Messages.MINotExist;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKEXCHANGERATE)
                {
                    litErrorMsg.Text = Resources.Messages.NoExchangeRate;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }

                else if (result == -10)
                {
                    //litErrorMsg.Text = GetGlobalResourceObject("Messages", "DSAPageName").ToString() + " " + Resources.Messages.MayAffectStockValueConfirmation;
                    // Include Popup for Yes/No
                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmStockValueChange", "ClosePopup();fnConfirmStockValueChange('SAVESUBMIT');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "fnConfirmStockValueChange", "ClosePopup();fnConfirmStockValueChange('SUBMIT');", true);
                    }

                    return;
                }
                else if (result == -31)
                {
                    litErrorMsg.Text = string.Format(Resources.Messages.StockTransferAlreadyDone, txtAdmissionDate.Text);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -32)
                {
                    litErrorMsg.Text = string.Format(Resources.Messages.StockAdjustmentIsAlreadyDone, txtAdmissionDate.Text);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
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
                else if (result == -37)
                {
                    litErrorMsg.Text = Resources.Messages.MsgRefAdded;//Supplier Ref# already entered
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -49)
                {
                    litErrorMsg.Text = GetLocalResourceObject("PRProductInactive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                    return;
                }
                else if (result == -4)
                {
                    litErrorMsg.Text = Resources.Messages.GRNDateAndMIDateValidation;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == -38)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ReqQtyMorethanPOQtyConfirmation();", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    if (hdfMenuType.Value == "1")
                    {
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "WODSAPageName").ToString());

                    }
                    else
                    {
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "DSAPageName").ToString());


                    }
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
            List<DirectStockAdmissionBO.StockAdmissionItem> tempStockAdmissionItemList;
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdDamageDetails")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPkDamageList = row.FindControl("hdfPkDamageList") as HiddenField;
                    //HiddenField hdfItemIdDamageList = row.FindControl("hdfItemIdDamageList") as HiddenField;
                    HiddenField hdfSlNoDamageList = row.FindControl("hdfSlNoDamageList") as HiddenField;
                    hdfDamageSlNo.Value = hdfSlNoDamageList.Value;
                    int pk = (GetNullableInt(hdfPkDamageList.Value) ?? 0);
                    int slNo = (GetNullableInt(hdfSlNoDamageList.Value) ?? 0);
                    int poDetailId = (GetNullableInt(hdfCurrentPODetIdAddRejectedReason.Value) ?? 0);

                    DirectStockAdmissionBO.StockAdmissionItem admissionItem = StockAdmissionItemList
                        .Where(x => x.PODetailId == poDetailId)
                        .Single();

                    DirectStockAdmissionBO.RejectedDetail rejectedDetails = admissionItem.RejectedList
                        .Where(x => x.Pk == pk && x.SlNo == slNo)
                        .Single();

                    lblItemNamePopup.Text = rejectedDetails.ItemText;
                    lblQtyRejectedPopup.Text = String.Format(GetCurrencyFormat(), admissionItem.Rejected);
                    txtDamageQtyPopup.Text = String.Format(GetCurrencyFormat(), rejectedDetails.Quantity);
                    ddlRejectionReasonPopup.SelectedValue = rejectedDetails.ReasonId.ToString();
                    ddlRejectToDept.SelectedValue = rejectedDetails.RejectedToStoreId.ToString();
                    BindGrid(ControlEnums.REJECTEDREASONLIST);
                    ShowRejectedPopup();
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    hdfDamageSlNo.Value = string.Empty;
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPkDamageList = row.FindControl("hdfPkDamageList") as HiddenField;
                    //HiddenField hdfItemIdDamageList = row.FindControl("hdfItemIdDamageList") as HiddenField;
                    HiddenField hdfSlNoDamageList = row.FindControl("hdfSlNoDamageList") as HiddenField;

                    int pk = (GetNullableInt(hdfPkDamageList.Value) ?? 0);
                    int slNo = (GetNullableInt(hdfSlNoDamageList.Value) ?? 0);
                    int poDetailId = (GetNullableInt(hdfCurrentPODetIdAddRejectedReason.Value) ?? 0);

                    tempStockAdmissionItemList = StockAdmissionItemList;

                    DirectStockAdmissionBO.StockAdmissionItem admissionItem = tempStockAdmissionItemList
                        .Where(x => x.PODetailId == poDetailId)
                        .Single();

                    DirectStockAdmissionBO.RejectedDetail rejectedDetails = admissionItem.RejectedList
                        .Where(x => x.Pk == pk && x.SlNo == slNo)
                        .Single();
                    if (rejectedDetails != null)
                    {
                        admissionItem.RejectedList.Remove(rejectedDetails);
                    }

                    StockAdmissionItemList = tempStockAdmissionItemList;
                    BindGrid(ControlEnums.REJECTEDREASONLIST);
                    ShowRejectedPopup();
                }
            }
            else if (senderGridView.ID == "grdStockAdmissionList")
            {

                if (e.CommandName == "RejectedAddClick")
                {
                    if (!UpdateStockAdmissionItemListFromGridview()) return;
                    ResetForm(ControlEnums.CLEARREJECTCONTROLS);
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPkStockAdmissionList = row.FindControl("hdfPkStockAdmissionList") as HiddenField;
                    //HiddenField hdfItemIdDamageList = row.FindControl("hdfItemIdDamageList") as HiddenField;
                    HiddenField hdfPODetIdStockAdmissionList = row.FindControl("hdfPODetIdStockAdmissionList") as HiddenField;
                    int pk = (GetNullableInt(hdfPkStockAdmissionList.Value) ?? 0);
                    int poDetId = (GetNullableInt(hdfPODetIdStockAdmissionList.Value) ?? 0);
                    hdfCurrentPODetIdAddRejectedReason.Value = hdfPODetIdStockAdmissionList.Value;
                    DirectStockAdmissionBO.StockAdmissionItem admissionItem = StockAdmissionItemList
                        .Where(x => x.PODetailId == poDetId)
                        .Single();
                    lblItemNamePopup.Text = admissionItem.ItemName;
                    lblQtyRejectedPopup.Text = String.Format(GetCurrencyFormat(), admissionItem.Rejected);  // admissionItem.Rejected.ToString(GetCurrencyFormat());
                    BindGrid(ControlEnums.REJECTEDREASONLIST);
                    ShowRejectedPopup();
                }
                else if (e.CommandName == "RemoveAdmissionItem")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPkStockAdmissionList = row.FindControl("hdfPkStockAdmissionList") as HiddenField;
                    HiddenField hdfPODetIdStockAdmissionList = row.FindControl("hdfPODetIdStockAdmissionList") as HiddenField;
                    int pk = (GetNullableInt(hdfPkStockAdmissionList.Value) ?? 0);
                    int poDetId = (GetNullableInt(hdfPODetIdStockAdmissionList.Value) ?? 0);
                    tempStockAdmissionItemList = StockAdmissionItemList;
                    DirectStockAdmissionBO.StockAdmissionItem tempAdmissionItem = tempStockAdmissionItemList
                        .Where(x => x.Pk == pk && x.PODetailId == poDetId)
                        .Single();

                    tempStockAdmissionItemList.Remove(tempAdmissionItem);
                    int nextSlNo = tempAdmissionItem.SlNo;
                    for (int i = 0; i < tempStockAdmissionItemList.Count; i++)
                    {
                        if (tempStockAdmissionItemList[i].SlNo > tempAdmissionItem.SlNo)
                        {
                            tempStockAdmissionItemList[i].SlNo = nextSlNo;
                            ++nextSlNo;
                        }
                    }

                    List<DirectStockAdmissionBO.PendingPO> tempPendingPoList = PendingPOList;
                    tempPendingPoList.
                        Where(x => x.PODetId == poDetId)
                       .ToList()
                       .ForEach(l => { l.AddedToStockList = false; l.CheckBoxChecked = false; });

                    PendingPOList = tempPendingPoList;
                    StockAdmissionItemList = tempStockAdmissionItemList;
                    BindGrid(ControlEnums.POS);
                    BindGrid(ControlEnums.STOCKADMISSIONLIST);
                }
            }
            else if (senderGridView.ID == "grdList")
            {
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                int stockPK = Convert.ToInt32((row.FindControl("hdfStockTransferPk") as HiddenField).Value);
                // int ConvertStatus = Convert.ToInt32((row.FindControl("hdfConverted") as HiddenField).Value);
                switch (e.CommandName)
                {
                    case "CONVERT":
                        #region CONVERT
                        var dsResult = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetGRNForConvert(stockPK);

                        grdReceived.DataSource = dsResult.Tables[0];
                        grdReceived.DataBind();

                        grdConvert.DataSource = dsResult.Tables[1];
                        grdConvert.DataBind();

                        //if (ConvertStatus == 1)
                        //{
                        //    btnConvert.Visible = false;
                        //    btnDeleteConvert.Visible = true;
                        //}
                        //else
                        //{
                        //    btnConvert.Visible = true;
                        //    btnDeleteConvert.Visible = false;
                        //}

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divConvert]','" + GetLocalResourceObject("ConversionDetails").ToString() + "','" + "1200" + "','" + "400" + "');", true);
                        #endregion
                        break;
                }
            }
            else if (senderGridView.ID == "grdBatchDetailsPopup")
            {
                if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    int batchPK = Convert.ToInt32((row.FindControl("hdfBatchPopup") as HiddenField).Value);

                    decimal BalanceQty = Convert.ToDecimal(lblBalanceQty.Text);
                    decimal currentQty = Convert.ToDecimal((row.FindControl("lblQtyGridPopup") as Label).Text);
                    lblBalanceQty.Text = GetFormattedNumber(BalanceQty + currentQty);

                    BatchDetailList.Remove(BatchDetailList.Where(w => w.BatchPK == batchPK).FirstOrDefault());
                    BindGrid(ControlEnums.FILLBATCHGRID);
                    showPopup();
                }
            }
        }
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int slno;
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //e.Row.Cells[4].Visible = false;
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
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

                if (((GridView)sender).ID == "grdStockAdmissionList")
                {
                    if (hdfGrnSupplierLocation.Value == "1")
                    {
                        foreach (GridViewRow row in grdStockAdmissionList.Rows)
                        {
                            TextBox TxtLocationAdmissionList = row.FindControl("txtLocationAdmissionList") as TextBox;
                            TxtLocationAdmissionList.Text = ddlSuppliers.SelectedItem.ToString();
                        }
                    }


                }

                if ((sender as GridView).ID == "grdList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button imgConvert = e.Row.FindControl("imgConvert") as Button;
                        int ConversionRequired = Convert.ToInt32((e.Row.FindControl("hdfConversionRequired") as HiddenField).Value);
                        int Converted = Convert.ToInt32((e.Row.FindControl("hdfConverted") as HiddenField).Value);
                        int GRNStatus = Convert.ToInt16(((HiddenField)e.Row.FindControl("hdfStatusList")).Value);

                        if (ConversionRequired == 0 || GRNStatus != 2)
                        {
                            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                            {
                                e.Row.Cells[9].Visible = false;
                            }
                        }

                        if (Converted == 1)
                        {
                            imgConvert.CssClass = "convert-icon";
                            imgConvert.ToolTip = "Convert";
                        }
                        else
                        {
                            imgConvert.CssClass = "not-convert-icon";
                            imgConvert.ToolTip = "Not Convert";
                        }
                    }
                }
                if (((GridView)sender).ID == "grdMaterialReturn")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ItemPK = Convert.ToInt32((e.Row.FindControl("hdfIssueItemPK") as HiddenField).Value);
                        ItemTypePK = Convert.ToInt32((e.Row.FindControl("hdfItemTypePK") as HiddenField).Value);
                        int SlNo = Convert.ToInt32((e.Row.FindControl("hdfMatSlNo") as HiddenField).Value);
                        int ScrapOrBgrade = Convert.ToInt32((e.Row.FindControl("hdfScrapOrBgrade") as HiddenField).Value);
                        ReturnWOPK = Convert.ToInt32((e.Row.FindControl("hdfMatWOPK") as HiddenField).Value);
                        IsAfterMulti = Convert.ToInt32((e.Row.FindControl("hdfIsAfterMulti") as HiddenField).Value);

                        //var dtBatches = BusinessLogic.MaterialManagement.MaterialMaster.GetBatchNoAuto(IssueItemPK, Convert.ToInt32(hdfSubContractStorePK.Value), 0, DateTime.Now, 0, DateTime.Now, 0);
                        GetFieldValues(ControlEnums.FILLITEMBATCHSPOPUP);

                        Label lblReturnStockQty = e.Row.FindControl("lblReturnStockQty") as Label;
                        DropDownList ddlBatch = e.Row.FindControl("ddlBatch") as DropDownList;
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            if (ItemTypePK == 1 || ItemTypePK == 4) //Material || Packing material
                                ddlBatch.DataValueField = "BATCH_WO_NO";// "SBD_PK";
                            else if (ItemTypePK == 2) //Product
                                ddlBatch.DataValueField = "BATCH_WO_NO";// "BID_BIN_CARD";
                            ddlBatch.DataTextField = "SBD_BATCH_NO";
                            ddlBatch.DataSource = dtPageData;
                            ddlBatch.DataBind();
                        }
                        ddlBatch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));

                        ImageButton imbAddGridBatch = e.Row.FindControl("imbAddGridBatch") as ImageButton;
                        ImageButton imbClearGridBatch = e.Row.FindControl("imbClearGridBatch") as ImageButton;
                        if (dtPageData != null && dtPageData.Rows.Count > 1)
                        {
                            imbAddGridBatch.Visible = true;
                            //e.Row.Cells[10].Enabled = false;
                        }
                        else
                        {
                            imbAddGridBatch.Visible = false;
                            //e.Row.Cells[10].Enabled = true;
                        }

                        if (ScrapOrBgrade == 1) //Bgrade
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("BgradeRowColor").ToString());
                        }
                        else if (ScrapOrBgrade == 2)//Scrap
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("ScrapRowColor").ToString());
                        }
                        //if (CurrPK > 0)
                        //{
                        if (MaterialReturnList != null && MaterialReturnList.Where(w => w.SlNo == SlNo).First().BatchDetailList != null &&
                            MaterialReturnList.Where(w => w.SlNo == SlNo).First().BatchDetailList.Count == 1)
                        {
                            DirectStockAdmissionBO.BatchDetails batch = MaterialReturnList.Where(w => w.SlNo == SlNo).First().BatchDetailList.First();
                            string selectedBatch = string.Empty; ;
                            if (IsAfterMulti == 1)
                            {
                                if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                                    selectedBatch = batch.BatchPK.ToString() + "_" + batch.WOPK.ToString() + "_" + batch.BOMPK;
                                else if (ItemTypePK == 2) //Product
                                    selectedBatch = batch.BinCardPK.ToString() + "_" + batch.WOPK.ToString() + "_" + batch.BOMPK;
                            }
                            else
                            {
                                if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                                    selectedBatch = batch.BatchPK.ToString();
                                else if (ItemTypePK == 2) //Product
                                    selectedBatch = batch.BinCardPK.ToString();
                            }
                            ddlBatch.SelectedValue = selectedBatch;


                            //if (ItemTypePK == 1 || ItemTypePK == 4) //Material
                            //    ddlBatch.SelectedValue = batch.BatchPK.ToString() + "_" + batch.WOPK.ToString() + "_" + batch.BOMPK;
                            //else if (ItemTypePK == 2) //Product
                            //    ddlBatch.SelectedValue = batch.BinCardPK.ToString() + "_" + batch.WOPK.ToString() + "_" + batch.BOMPK;

                            lblReturnStockQty.Text = GetFormattedNumber(batch.StockQty.ToString());
                            e.Row.Cells[5].Enabled = true;
                            imbClearGridBatch.Visible = false;
                        }
                        else if (MaterialReturnList != null && MaterialReturnList.Where(w => w.SlNo == SlNo).First().BatchDetailList != null &&
                            MaterialReturnList.Where(w => w.SlNo == SlNo).First().BatchDetailList.Count > 1)
                        {
                            ddlBatch.Items.Clear();
                            ddlBatch.Items.Insert(0, new ListItem(CommonConstants.MULTIBATCH, CommonConstants.MULTIBATCH_VALUE));
                            e.Row.Cells[5].Enabled = false;
                            imbClearGridBatch.Visible = true;
                        }
                        //}

                        //Enable/Disable lblReturnQty by MultiBatch
                        if (ddlBatch.SelectedValue == CommonConstants.MULTIBATCH_VALUE)
                            e.Row.Cells[10].Enabled = false;
                        else
                            e.Row.Cells[10].Enabled = true;
                    }
                }

                if (((GridView)sender).ID == "grdBatchDetailsPopup")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        bool HasRowColor = Convert.ToBoolean((e.Row.FindControl("hdfHasRowColor") as HiddenField).Value);
                        if (HasRowColor)
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("RowRedColor").ToString());
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lbl = (Label)e.Row.FindControl("lblPopupFooterTotal");
                        lbl.Text = GetFormattedNumber(BatchDetailList.Sum(s => s.ActualQty));
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
            int supplierId, admissionStoreId, processId;
            string pageUrl = string.Empty;
            try
            {
                switch (type)
                {
                    #region Company
                    case ControlEnums.COMPANY:
                        //AdmCompanyMstService admCompanyMstServiceClient = new AdmCompanyMstService();
                        //ADM_COMPANY_MST admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        //admCompanyMstObj.CMP_ACTIVE = 1;
                        //serviceUtilityObj = new ServiceUtility();
                        //admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyMappingDetails(companyPK, Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, currentUser.CurrentDeptPK);
                        break;
                    #endregion
                    #region SUPPLIERS
                    case ControlEnums.SUPPLIERS:
                        if (hdfMenuType.Value == "1")
                            dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetPurchaseOrderVendors(currentUser.SBUID, this.CurrPK, (int)VendorRoles.SubContractor);
                        else
                            dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetPurchaseOrderVendors(currentUser.SBUID, this.CurrPK);
                        break;
                    #endregion
                    #region RECIEVINGSTORE
                    case ControlEnums.ADMISSIONSTORE:
                        dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetStockAdmissionStores(currentUser, currentUser.SBUID, 0, 0);
                        break;
                    #endregion
                    #region LIST
                    case ControlEnums.LIST:
                        gridParam = new BusinessObject.GridPrams();

                        gridParam.SearchBy = string.Empty;
                        gridParam.SearchValue = string.Empty;

                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.Fields = GTIService.Constants.DirectStockTransfer.Parameters.GridParmeters;
                        //"[GRH_PK],[GRH_NO],[GRH_DATE],[GRH_STATUS],[GRH_STATUS_TEXT],[REF_ID],[DPT_NAME],[GRH_VENDOR_TEXT],[GRH_PO_NO],[GRH_VND_REF_NO]";
                        gridParam.SortBy = GTIService.Constants.DirectStockTransfer.Fields.GRH_PK_SortBy;
                        gridParam.SortDirection = "DESC";
                        gridParam.FromDate = txtFromDate.Text;
                        gridParam.ToDate = txtToDate.Text;
                        gridParam.FilterStatus = ddlStatus.SelectedValue;
                        //processId = GetNullableInt(hdfProcId.Value) ?? 0;
                        processId = GetNullableInt(hdfProcessID.Value) ?? 0;
                        pageUrl = Resources.PageURL.DirectStockAdmission.Replace("~", ""); // "/StoreManagement/DirectStockAdmission.aspx"; 
                        if (Request.QueryString["Type"] != null)
                        {
                            pageUrl += "&Type=" + Request.QueryString["Type"].ToString();
                        }
                        string adminNo = txtStockAdmissionNo.Text.Trim() == "Select/Type" ? string.Empty : txtStockAdmissionNo.Text.Trim();
                        string poNo = txtPONo.Text.Trim() == "Select/Type" ? string.Empty : txtPONo.Text.Trim();
                        string vendorText = txtVendor.Text.Trim();
                        string VendorPK = vendorText == "Select/Type" || vendorText == "type min 4 characters" || vendorText == string.Empty ? "0" : hdfVendorPKSearch.Value;
                        int cmpPk = string.IsNullOrEmpty(ddlPlantCode.SelectedValue) ? 0 : Convert.ToInt32(ddlPlantCode.SelectedValue);
                        //if (Request.QueryString["Type"] != null)
                        //{
                        //    if (Request.QueryString["Type"] == "1")
                        //    {
                        //        dsPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetStockTransferListByType(gridParam, currentUser, processId, pageUrl, GetNullableInt(hdfDepartmentPkSearch.Value), adminNo, poNo, GetNullableInt(VendorPK), cmpPk);
                        //    }
                        //    else
                        //    {
                        //        dsPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetStockTransferList(gridParam, currentUser, processId, pageUrl, GetNullableInt(hdfDepartmentPkSearch.Value), adminNo, poNo, GetNullableInt(VendorPK), cmpPk);
                        //    }
                        //}
                        //else
                        //{
                        dsPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetStockTransferList(gridParam, currentUser, processId, pageUrl, GetNullableInt(hdfDepartmentPkSearch.Value), adminNo, poNo, GetNullableInt(VendorPK), cmpPk);
                        // }
                        break;
                    #endregion
                    #region POS
                    case ControlEnums.POS:
                        supplierId = admissionStoreId = 0;
                        if (ddlSuppliers.SelectedIndex > 0)
                            supplierId = GetNullableInt(ddlSuppliers.SelectedValue).Value;
                        else
                            return;
                        if (ddlAdmissionStore.SelectedIndex > 0)
                            admissionStoreId = GetNullableInt(ddlAdmissionStore.SelectedValue).Value;
                        else
                            return;
                        int pohPk = 0, grnID = 0; // ToDo:
                        grnID = CurrPK;
                        gridParam = new GridPrams(); // ToDo:
                        gridParam.SearchBy = SearchType.SelectedValue; // GTIService.Constants.DirectStockTransfer.Fields.POH_NO;
                        gridParam.SearchValue = SearchValue.Text.Trim(); //string.Empty;
                        gridParam.Fields = GTIService.Constants.DirectStockTransfer.Parameters.GridParametersPOS;
                        //"POH_PK,POD_PK,POD_ITEM,POD_UOM,POD_GRN_FLAG,POH_NO,ITM_NAME,UOM_CODE,POD_QTY_APPROVED,POD_QTY_RECEIVED,BALANCE_QTY,POH_COMPANY, ITM_CODE";
                        gridParam.FromDate = FromDate.Text.Trim();
                        gridParam.ToDate = ToDate.Text.Trim();// string.Empty;
                        if (hdfMenuType.Value == "1")
                        {
                            gridParam.PendingWO = ChkPendingWO.Checked == true ? 1 : 0;
                        }
                        if (hdfMenuType.Value == "1")
                            dsPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetWOPending(currentUser.SBUID, admissionStoreId, supplierId, gridParam, grnID, pohPk);
                        else
                            dsPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetPurchaseOrderPending(currentUser.SBUID, admissionStoreId, supplierId, gridParam, grnID, pohPk);
                        dtPageData = new DataTable();
                        PendingPOList = dsPageData.Tables[1] //dtPageData
                            .AsEnumerable()
                            .Select(x => new DirectStockAdmissionBO.PendingPO
                            {
                                POId = x.Field<int>(GTIService.Constants.DirectStockTransfer.Fields.POH_PK),
                                PODetId = x.Field<int>(GTIService.Constants.DirectStockTransfer.Fields.POD_PK),
                                PONumber = x.Field<string>(GTIService.Constants.DirectStockTransfer.Fields.POH_NO),
                                // AddedToStockList = Convert.ToBoolean(x.Field<int>(GTIService.Constants.DirectStockTransfer.Fields.POD_GRN_FLAG)),
                                ItemId = x.Field<int>(GTIService.Constants.DirectStockTransfer.Fields.POD_ITEM),
                                //ItemName = x.Field<string>(GTIService.Constants.DirectStockTransfer.Fields.ITM_NAME),
                                ItemName = x.Field<string>(GTIService.Constants.DirectStockTransfer.Fields.ITM_CODE) + "-" + x.Field<string>(GTIService.Constants.DirectStockTransfer.Fields.ITM_NAME),
                                UOMId = x.Field<int>(GTIService.Constants.DirectStockTransfer.Fields.POD_UOM),
                                UOM = x.Field<string>(GTIService.Constants.DirectStockTransfer.Fields.UOM_CODE),
                                POQty = x.Field<decimal>(GTIService.Constants.DirectStockTransfer.Fields.POD_QTY_APPROVED),
                                PreRecievedQty = x.Field<decimal>(GTIService.Constants.DirectStockTransfer.Fields.POD_QTY_RECEIVED),
                                BalanceQtyToRecieve = x.Field<decimal>(GTIService.Constants.DirectStockTransfer.Fields.BALANCE_QTY),
                                //ItemCode = x.Field<string>(GTIService.Constants.DirectStockTransfer.Fields.ITM_CODE)
                                PORate = x.Field<double>(GTIService.Constants.DirectStockTransfer.Fields.POD_RATE),
                                WIH_ITEM_TYPE = x.Field<int>("WIH_ITEM_TYPE"),
                                WIH_ITEM_TYPE_TEXT = x.Field<string>("WIH_ITEM_TYPE_TEXT"),
                                POD_QTY_RETURNED = x.Field<decimal>("POD_QTY_RETURNED"),
                            })
                            .ToList();
                        foreach (var item in PendingPOList)
                        {
                            if (item.BalanceQtyToRecieve < 0) item.BalanceQtyToRecieve = 0;
                        }
                        break;
                    #endregion
                    #region DETAILFOREDIT
                    case ControlEnums.DETAILFOREDIT:
                        string xmlData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectStockAdmissionByPk(CurrPK);
                        //string xmlData = Convert.ToString(dtPageData.Rows[0][0]);
                        DirectStockAdmissionBO.DirectStockAdmission tempDirectStockAdmission = CommonFunctions.XmlDeserialize<DirectStockAdmissionBO.DirectStockAdmission>(xmlData);
                        DirectStockAdmissionProp = tempDirectStockAdmission;
                        SwitchTab(1);
                        break;
                    #endregion
                    #region "REJECTIONREASON"
                    case ControlEnums.REJECTIONREASON:
                        dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDamageTypes(currentUser.SBUID);
                        break;
                    #endregion
                    #region "REJECTTO"
                    case ControlEnums.REJECTTO:
                        dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDamagedItemsStore(currentUser);
                        break;
                    #endregion
                    #region POPERCENTAGE
                    case ControlEnums.POPERCENTAGE:
                        DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("WO GRN EXCESS PERC", string.Empty, currentUser.SBUID);//GRNAdditionalQty
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            hdfOrderPercentage.Value = dt.Rows[0]["ACF_DATA"].ToString();
                        }
                        break;
                    #endregion
                    #region POSFROMINBOX
                    case ControlEnums.POSFROMINBOX:
                        dtPos = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPOVendor(GetNullableInt(PO_PK.Value) ?? 0);
                        break;
                    #endregion
                    #region Plant
                    case ControlEnums.Plant:
                        string searchBy = GTIService.Constants.Common.CommonConstants.CMP_DISPLAY_CODE;
                        dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetDirectGRNAutocomplete(searchBy, string.Empty, currentUser);
                        break;
                    #endregion
                    #region Convert
                    case ControlEnums.CONVERTLIST:
                        break;
                    #endregion
                    #region EDITFORRETURN
                    case ControlEnums.EDITFORRETURN:
                        if (MaterialReturnList == null)
                            MaterialReturnList = new List<DirectStockAdmissionBO.MaterialReturn>();

                        #region Commented
                        //WOPKs = WOPKs.Distinct().ToList();

                        //#region Remove unticked WO
                        //List<int> RemovedWOPKs = new List<int>();
                        //RemovedWOPKs = MaterialReturnList.Select(s => s.WOPK).ToList().Except(WOPKs).Distinct().ToList();
                        //if (RemovedWOPKs.Count() > 0)
                        //    MaterialReturnList.RemoveAll(r => RemovedWOPKs.Contains(r.WOPK));
                        //#endregion

                        //#region new
                        //MaterialReturnList.RemoveAll(r => r.WOPK == WO_Item_List.First().Key && r.WOItemPK == WO_Item_List.First().Value);
                        //#endregion

                        //if (MaterialReturnList != null && MaterialReturnList.Any())
                        //{
                        //    WOPKs = WOPKs.Except(MaterialReturnList.Select(s => s.WOPK).ToList()).ToList();//Get new WO PKs that are not in MaterialReturnList
                        //}
                        #endregion

                        List<DirectStockAdmissionBO.WOHeaderList> ReturnListTemp = new List<DirectStockAdmissionBO.WOHeaderList>();
                        ReturnListTemp = ListForReturn;
                        foreach (DirectStockAdmissionBO.MaterialReturn item in MaterialReturnList)
                        {
                            if (ListForReturn.Where(w => w.WOPK == item.WOPK && w.WOItemPK == item.WOItemPK).Any())
                                ReturnListTemp = new List<DirectStockAdmissionBO.WOHeaderList>();
                        }

                        #region Add WOPKs to list
                        DirectStockAdmissionBO.WOHeader workOrder = new DirectStockAdmissionBO.WOHeader();
                        foreach (DirectStockAdmissionBO.WOHeaderList item in ReturnListTemp)
                        {
                            foreach (GridViewRow row in grdStockAdmissionList.Rows)
                            {
                                int woId = Convert.ToInt32(((HiddenField)row.FindControl("hdfPOIdStockAdmissionList")).Value);
                                int woItemId = Convert.ToInt32(((HiddenField)row.FindControl("hdfItemIdStockAdmissionList")).Value);
                                if (item.WOPK == woId && item.WOItemPK == woItemId)
                                {
                                    decimal receivedQty = Convert.ToDecimal(((TextBox)row.FindControl("txtReceivedAdmissionList")).Text);
                                    //decimal acceptedQty = Convert.ToDecimal(((TextBox)row.FindControl("txtAcceptedAdmissionList")).Text);
                                    item.ReceivedQty = receivedQty;
                                }
                            }
                        }
                        if (workOrder.WOList == null)
                            workOrder.WOList = new List<DirectStockAdmissionBO.WOHeaderList>();
                        workOrder.WOList = ReturnListTemp;
                        #endregion

                        string xmlDocument = CommonFunctions.XmlSerialize<DirectStockAdmissionBO.WOHeader>(workOrder);
                        if (ReturnListTemp.Any())
                        {
                            int MaterialRetturn = 1;//if value 1 get scrap and bgrade
                            string xmlResult = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetWOBOMForReturn(xmlDocument, Convert.ToInt32(ddlSuppliers.SelectedValue), MaterialRetturn);
                            if (xmlResult != "")
                            {
                                DirectStockAdmissionBO.MaterialReturnRoot MatRet = CommonFunctions.XmlDeserialize<DirectStockAdmissionBO.MaterialReturnRoot>(xmlResult);
                                var MatRetList = MatRet.MaterialReturnList;

                                int SlNo = 0;
                                if (MaterialReturnList != null && MaterialReturnList.Any())
                                    SlNo = MaterialReturnList.Max(m => m.SlNo);

                                MatRetList.ForEach(f => { f.SlNo = ++SlNo; });

                                MaterialReturnList = MatRetList;// MaterialReturnList.Concat(MatRetList).ToList();
                            }
                        }
                        break;
                    #endregion
                    #region FILLITEMBATCHSPOPUP
                    //case ControlEnums.FILLITEMBATCHSPOPUP:
                    //    dtPageData = BusinessLogic.MaterialManagement.MaterialMaster.GetBatchNoAuto(ItemPK, Convert.ToInt32(hdfSubContractStorePK.Value), 0, DateTime.Now, 0, DateTime.Now, 0);
                    //    break;
                    #endregion
                    #region FILLBATCHPOPUPDTLS, FILLITEMBATCHSPOPUP
                    case ControlEnums.FILLBATCHPOPUPDTLS:
                    case ControlEnums.FILLITEMBATCHSPOPUP:
                        dtPageData = BusinessLogic.WorkOrder.WorkOrderBL.GetPendingBatches(Convert.ToInt32(ddlSuppliers.SelectedValue), ItemPK, ItemTypePK, CurrPK, 0, ReturnWOPK, IsAfterMulti);
                        break;
                    #endregion
                    #region SUBCONTRACTSTORE
                    case ControlEnums.SUBCONTRACTSTORE:
                        dtPageData = BusinessLogic.StoreManagement.DirectStockAdmissoinBL.GetStockAdmissionStores(currentUser, currentUser.SBUID, 18, 0);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
                    case ControlEnums.ADMISSIONSTORE:
                        BindDropDown(ControlEnums.ADMISSIONSTORE);
                        break;
                    case ControlEnums.LIST:
                        BindGrid(ControlEnums.LIST);
                        break;
                    case ControlEnums.POS:
                        BindGrid(ControlEnums.POS);
                        break;
                    case ControlEnums.DETAILFOREDIT:
                        GetUIValuesFromObject(ControlEnums.DETAILFOREDIT);
                        BindGrid(ControlEnums.STOCKADMISSIONLIST);
                        BindGrid(ControlEnums.POS);//  GridViewPOChangeColour();
                        break;
                    #region "REJECTIONREASON"
                    case ControlEnums.REJECTIONREASON:
                        BindDropDown(ControlEnums.REJECTIONREASON);
                        break;
                    #endregion
                    #region "REJECTTO"
                    case ControlEnums.REJECTTO:
                        BindDropDown(ControlEnums.REJECTTO);
                        break;
                    #endregion
                    #region POSFROMINBOX
                    case ControlEnums.POSFROMINBOX:
                        //txtAdmissionDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        if (dtPos != null && dtPos.Rows.Count > 0)
                        {
                            lblDirectStockAdmissionNo.Text = GetGlobalResourceObject("Messages", "DocGenerationNew").ToString();
                            ddlSuppliers.SelectedValue = dtPos.Rows[0]["POH_VENDOR"].ToString();
                            //ddlCompany.SelectedValue = dtPos.Rows[0]["POH_COMPANY"].ToString();
                            ActionHandler(ddlAdmissionStore, EventArgs.Empty);
                            int pohPk;
                            pohPk = GetNullableInt(dtPos.Rows[0]["POH_PK"].ToString()) ?? 0;
                            foreach (GridViewRow grdrow in grdPOs.Rows)
                            {
                                int poId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOIdPOList")).Value);
                                if (poId == pohPk)
                                {
                                    CheckBox chk;
                                    chk = (CheckBox)grdrow.FindControl("chkSelectPOList");
                                    chk.Checked = true;
                                }
                            }
                            ActionHandler(btnAddToStockAdmissionList, EventArgs.Empty);
                        }

                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlEnums.UPLOADEDFILES:
                        if (DirectStockAdmissionProp != null)
                            GetUIValuesFromObject(ControlEnums.UPLOADEDFILES);
                        BindGrid(ControlEnums.UPLOADEDFILES);
                        break;
                    #endregion
                    case ControlEnums.Plant:
                        BindDropDown(ControlEnums.Plant);
                        break;
                    #region EDITFORRETURN
                    case ControlEnums.EDITFORRETURN:
                        BindGrid(ControlEnums.EDITFORRETURN);
                        break;
                    #endregion
                    #region FILLITEMBATCHSPOPUP
                    case ControlEnums.FILLITEMBATCHSPOPUP:
                        BindDropDown(ControlEnums.FILLITEMBATCHSPOPUP);
                        break;
                    #endregion
                    #region FILLBATCHPOPUPDTLS
                    case ControlEnums.FILLBATCHPOPUPDTLS:
                        GetUIValuesFromObject(ControlEnums.FILLBATCHPOPUPDTLS);
                        break;
                    #endregion
                    #region SUBCONTRACTSTORE
                    case ControlEnums.SUBCONTRACTSTORE:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfSubContractStorePK.Value = dtPageData.Rows[0]["DPT_PK"].ToString();
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
        private bool IsValidReturnForWO()
        {
            bool retFlag = true;
            foreach (GridViewRow row in grdMaterialReturn.Rows)
            {
                int ScarpOrBGrade = Convert.ToInt32(((HiddenField)row.FindControl("hdfScrapOrBgrade")).Value);
                DropDownList ddlBatchTemp = (DropDownList)row.FindControl("ddlBatch");
                int batchSelectVal = 0;
                if (ddlBatchTemp.Items.Count > 0)
                    batchSelectVal = Convert.ToInt32(ddlBatchTemp.SelectedValue);
                string retStrVal = ((TextBox)row.FindControl("lblReturnQty")).Text;
                float retVal = 0;
                float.TryParse(retStrVal, out retVal);
                if (ScarpOrBGrade == 0)//A grade
                {
                    if (retVal > 0 && batchSelectVal == 0)
                    {
                        retFlag = false;
                        break;
                    }
                }

            }

            return retFlag;


        }
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
                    //if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    //{
                    //    ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                    //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlCompany.DataBind();
                    //}
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecodeDataTable(dtCompany, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }
                    break;
                #endregion
                #region SUPPLIERS
                case ControlEnums.SUPPLIERS:
                    ddlSuppliers.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlSuppliers.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.VEN_PK;
                        ddlSuppliers.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.VEN_NAME;
                        ddlSuppliers.DataSource = dtPageData;
                        ddlSuppliers.DataBind();
                    }
                    ddlSuppliers.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlSuppliers.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                #region RECIEVINGSTORE
                case ControlEnums.ADMISSIONSTORE:
                    ddlAdmissionStore.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlAdmissionStore.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DPT_PK;
                        ddlAdmissionStore.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DPT_NAME;
                        ddlAdmissionStore.DataSource = dtPageData;
                        ddlAdmissionStore.DataBind();
                    }
                    ddlAdmissionStore.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlAdmissionStore.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                #region REJECTIONREASON
                case ControlEnums.REJECTIONREASON:
                    ddlRejectionReasonPopup.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlRejectionReasonPopup.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DIT_PK;
                        ddlRejectionReasonPopup.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DIT_NAME;
                        ddlRejectionReasonPopup.DataSource = dtPageData;
                        ddlRejectionReasonPopup.DataBind();
                    }
                    break;
                #endregion
                #region REJECTTO
                case ControlEnums.REJECTTO:
                    ddlRejectToDept.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlRejectToDept.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DPT_PK;
                        ddlRejectToDept.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DPT_NAME;
                        ddlRejectToDept.DataSource = dtPageData;
                        ddlRejectToDept.DataBind();
                    }
                    break;
                #endregion
                #region Plant
                case ControlEnums.Plant:
                    ddlPlantCode.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlPlantCode.DataValueField = GTIService.Constants.Designation.Fields.PK;
                        ddlPlantCode.DataTextField = GTIService.Constants.Designation.Fields.VALUE;
                        ddlPlantCode.DataSource = dtPageData;
                        ddlPlantCode.DataBind();
                    }
                    ddlPlantCode.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlPlantCode.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                case ControlEnums.SEARCHTYPE:
                    if (Request.QueryString["Type"] != null)
                    {
                        if (Request.QueryString["Type"] == "1")
                        {
                            SearchType.Items.Insert(1, new ListItem(Resources.BindValues.WihNo, "WIH_NO"));
                        }
                        else
                        {
                            SearchType.Items.Insert(1, new ListItem(Resources.BindValues.PONumber, "POH_NO"));
                        }
                    }
                    else
                        SearchType.Items.Insert(1, new ListItem(Resources.BindValues.PONumber, "POH_NO"));
                    break;
                #region FILLITEMBATCHSPOPUP
                case ControlEnums.FILLITEMBATCHSPOPUP:
                    ddlBatchesPopUp.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlBatchesPopUp.DataSource = dtPageData;
                        if (ItemTypePK == 1 || ItemTypePK == 4) //Material || Packing material
                            ddlBatchesPopUp.DataValueField = "BATCH_WO_NO";
                        else if (ItemTypePK == 2) //Product
                            ddlBatchesPopUp.DataValueField = "BATCH_WO_NO";
                        ddlBatchesPopUp.DataTextField = "SBD_BATCH_NO";
                        ddlBatchesPopUp.DataBind();
                    }
                    ddlBatchesPopUp.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
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
                #region LIST
                case ControlEnums.LIST:
                    int rowCount = 0;
                    int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    if (dsPageData.Tables[0].Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString());
                    }
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;
                    PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                    grdList.DataSource = dsPageData.Tables[1];
                    grdList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                #endregion
                #region POS
                case ControlEnums.POS:
                    grdPOs.DataSource = PendingPOList;
                    grdPOs.DataBind();
                    break;
                #endregion
                #region STOCKADMISSIONLIST
                case ControlEnums.STOCKADMISSIONLIST:
                    grdStockAdmissionList.DataSource = StockAdmissionItemList;
                    grdStockAdmissionList.DataBind();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "enableDisableAddReasonButton", "enableDisableAddReasonButton();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "FillStoreLocationAutoComplete", "$(document).ready(function(){FillStoreLocationAutoComplete();});", true);
                    break;
                #endregion
                #region REJECTEDREASONLIST
                case ControlEnums.REJECTEDREASONLIST:
                    int poDetId = GetNullableInt(hdfCurrentPODetIdAddRejectedReason.Value) ?? 0;
                    List<DirectStockAdmissionBO.RejectedDetail> tempRejectedDetailsList = StockAdmissionItemList
                         .Where(x => x.PODetailId == poDetId)
                         .Single()
                         .RejectedList;
                    grdDamageDetails.DataSource = tempRejectedDetailsList;
                    grdDamageDetails.DataBind();
                    break;
                #endregion
                #region UPLOADED FILES
                case ControlEnums.UPLOADEDFILES:
                    grdUploads.DataSource = DSAUploadList;
                    grdUploads.DataBind();
                    break;
                #endregion
                #region EDITFORRETURN
                case ControlEnums.EDITFORRETURN:
                    grdMaterialReturn.DataSource = MaterialReturnList;
                    grdMaterialReturn.DataBind();
                    break;
                #endregion
                #region FILLBATCHGRID
                case ControlEnums.FILLBATCHGRID:
                    grdBatchDetailsPopup.DataSource = BatchDetailList;
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

        #region GetUIValuesFromObject
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region DETAILFOREDIT
                    case ControlEnums.DETAILFOREDIT:
                        lblDirectStockAdmissionNo.Text = DirectStockAdmissionProp.GRH_NO == string.Empty ?
                        GetGlobalResourceObject("Messages", "DocGenerationNew").ToString() : DirectStockAdmissionProp.GRH_NO;
                        hdfGRH_VERSION.Value = DirectStockAdmissionProp.GRH_VERSION;
                        // hdfGrnExcessQty.Value = DirectStockAdmissionProp.GRH_EXCESS_PERC.ToString();
                        txtAdmissionDate.Text = DirectStockAdmissionProp.GRH_DATE;
                        ddlSuppliers.SelectedValue = DirectStockAdmissionProp.GRH_VENDOR.ToString();
                        txtSupplierRefNo.Text = DirectStockAdmissionProp.GRH_VND_REF_NO;
                        txtRefDate.Text = DirectStockAdmissionProp.GRH_VND_REF_DATE;
                        ddlAdmissionStore.SelectedValue = DirectStockAdmissionProp.GRH_INV_DEPT.ToString();
                        companyPK = DirectStockAdmissionProp.GRH_COMPANY;
                        GetFieldValues(ControlEnums.COMPANY);
                        SetFieldValues(ControlEnums.COMPANY);
                        ddlCompany.SelectedValue = DirectStockAdmissionProp.GRH_COMPANY.ToString();
                        DateTime dt = Convert.ToDateTime(DirectStockAdmissionProp.LAST_MOD_DT);
                        lblLastModifiedHDR.Text = DirectStockAdmissionProp.LAST_MOD_DT = dt.ToString();
                        ConfirmStockValueChange.Value = Convert.ToString(DirectStockAdmissionProp.STK_VAL_CONFIRM);
                        ActionHandler(ddlAdmissionStore, EventArgs.Empty);

                        List<DirectStockAdmissionBO.StockAdmissionItem> tempStockAdmissionItemList = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                        foreach (var item in DirectStockAdmissionProp.StockAdmissionList)
                        {
                            DirectStockAdmissionBO.StockAdmissionItem admissionItem = new DirectStockAdmissionBO.StockAdmissionItem();
                            admissionItem.Pk = item.GRD_PK;
                            admissionItem.SlNo = item.GRD_SL_NO;
                            admissionItem.ItemId = item.GRD_ITEM;
                            admissionItem.Recieved = item.GRD_QTY_RECEIVED;
                            admissionItem.Accepted = item.GRD_QTY_ACCEPTED;
                            admissionItem.Rejected = item.GRD_QTY_REJECTED;
                            admissionItem.UOM = item.GRD_UOM;
                            admissionItem.POId = item.GRD_PO;
                            admissionItem.PODetailId = item.GRD_PO_DTL;
                            admissionItem.PORate = item.GRD_PO_RATE;
                            admissionItem.BatchNo = item.GRD_VND_REF_NO; // GRD_BATCH_NO;
                            admissionItem.QALotNo = item.GRD_QA_LOT_NO;
                            admissionItem.DOM = item.GRD_DOM == null ? string.Empty : item.GRD_DOM;
                            admissionItem.DOE = item.GRD_DOE == null ? string.Empty : item.GRD_DOE;
                            admissionItem.Location = item.GRD_LOCATION;
                            admissionItem.RealWeight = item.GRD_QTY_REAL;
                            admissionItem.WIH_ITEM_TYPE = Convert.ToInt32(item.GRD_WIH_ITEM_TYPE);

                            //DirectStockAdmissionBO.PendingPO tempPendingPo = new DirectStockAdmissionBO.PendingPO(); // new code sdd
                            //if (PendingPOList.Exists(x => x.POId == admissionItem.POId && x.ItemId == admissionItem.ItemId))
                            //{
                            //    //DirectStockAdmissionBO.PendingPO tempPendingPo = PendingPOList old code
                            //    tempPendingPo = PendingPOList
                            //   .Where(x => x.POId == admissionItem.POId && x.ItemId == admissionItem.ItemId)
                            //   .Single();
                            //}


                            DirectStockAdmissionBO.PendingPO tempPendingPo = PendingPOList
                                 .Where(x => x.POId == admissionItem.POId && x.ItemId == admissionItem.ItemId)
                                 .Single();

                            tempPendingPo.AddedToStockList = true;
                            tempPendingPo.CheckBoxChecked = true;


                            admissionItem.ItemName = tempPendingPo.ItemName;
                            admissionItem.POQty = tempPendingPo.POQty;
                            admissionItem.PendingQty = tempPendingPo.BalanceQtyToRecieve;
                            admissionItem.PODetailId = tempPendingPo.PODetId;
                            admissionItem.PONumber = tempPendingPo.PONumber;
                            //admissionItem.ReturnQty = tempPendingPo.POD_QTY_RETURNED;
                            List<DirectStockAdmissionBO.RejectedDetail> rejectedList = new List<DirectStockAdmissionBO.RejectedDetail>();
                            int dmgSlNo = 0;
                            foreach (var reject in item.DamageList)
                            {
                                DirectStockAdmissionBO.RejectedDetail rejectDetail = new DirectStockAdmissionBO.RejectedDetail();
                                rejectDetail.Pk = reject.GDD_PK;
                                rejectDetail.SlNo = ++dmgSlNo;
                                rejectDetail.HdrSlNo = reject.GDD_SL_NO;
                                rejectDetail.PODetailId = tempPendingPo.PODetId;
                                rejectDetail.ItemId = reject.GDD_ITEM;
                                rejectDetail.ItemText = reject.GDD_ITEM_NAME.IsNullOrEmptyOrWhitespace() ? tempPendingPo.ItemName : reject.GDD_ITEM_NAME;
                                rejectDetail.Quantity = reject.GDD_DMG_QTY;
                                rejectDetail.ReasonId = reject.GDD_DMG_TYPE;
                                rejectDetail.ReasonText = reject.GDD_DMG_TYPE_TEXT;
                                rejectDetail.RejectedToStoreId = reject.GDD_DEPT_STORE;
                                rejectDetail.RejectedToStoreText = reject.GDD_DEPT_STORE_NAME;
                                rejectedList.Add(rejectDetail);
                            }
                            admissionItem.RejectedList = rejectedList;

                            tempStockAdmissionItemList.Add(admissionItem);
                        }

                        StockAdmissionItemList = tempStockAdmissionItemList;
                        break;
                    #endregion

                    #region SELECTED DOC
                    case ControlEnums.SELECTEDDOC:
                        if (DSAUploadObj != null)
                        {
                            CurrSlNo = DSAUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = DSAUploadObj.DOC_NAME;
                            anchorFile.HRef = DSAUploadObj.DOC_PATH;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                            }
                        }
                        break;
                    #endregion
                    # region  FILE_UPLOAD
                    case ControlEnums.UPLOADEDFILES:
                        CurrPK = DirectStockAdmissionProp.GRH_PK;
                        DSAUploadList = DirectStockAdmissionProp.FileList;
                        break;
                    #endregion
                    #region FILLBATCHPOPUPDTLS
                    case ControlEnums.FILLBATCHPOPUPDTLS:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            string BatchField = "SBD_PK";
                            if (ItemTypePK == 1 || ItemTypePK == 4) //Material || Packing material
                                BatchField = "SBD_PK";
                            else if (ItemTypePK == 2) //Product
                                BatchField = "BID_BIN_CARD";
                            dtPageData = dtPageData.Select(BatchField + "=" + BatchPK.ToString() + " AND WIH_PK=" + selWOPK).CopyToDataTable();
                        }
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            double stock = 0;
                            if (double.TryParse(dtPageData.Rows[0]["BALANCE"].ToString(), out stock))
                                lblStockPopUp.Text = GetFormattedNumber(stock);

                            lblStockPopUp.Text = GetFormattedNumber(stock);
                            hdfBatchWOPKPopup.Value = dtPageData.Rows[0]["WIH_PK"].ToString(); //WIH_PK_TEXT
                            lblBatchWOPopup.Text = GetShortString(dtPageData.Rows[0]["WIH_NO"].ToString(), 26); //WIH_NO_TEXT
                            lblBatchWOPopup.ToolTip = dtPageData.Rows[0]["WIH_NO"].ToString(); //WIH_NO_TEXT

                            hdfIsValidationRequired.Value = "1";
                            if (hdfBatchWOPKPopup.Value != string.Empty)
                            {
                                List<int> WOIDs = hdfBatchWOPKPopup.Value.Split(',').Select(int.Parse).ToList();
                                bool isInList = WOIDs.IndexOf(Convert.ToInt32(hdfCurrentWOPK.Value)) != -1;
                                if (isInList)
                                    hdfIsValidationRequired.Value = "0";
                            }

                            double BalStock = 0;
                            double StockPopUp = 0;

                            if (double.TryParse(lblBalanceQty.Text, out BalStock))
                                if (double.TryParse(lblStockPopUp.Text, out StockPopUp))
                                {
                                    if (BalStock > 0 && BalStock > StockPopUp)
                                        txtQtyPopUp.Text = GetFormattedNumber(StockPopUp);
                                    else if (BalStock > 0 && BalStock < StockPopUp)
                                        txtQtyPopUp.Text = GetFormattedNumber(BalStock);
                                    else if (BalStock == StockPopUp)
                                        txtQtyPopUp.Text = GetFormattedNumber(BalStock);
                                }
                            hdfBatchPopupUOM.Value = dtPageData.Rows[0]["SBD_UOM"].ToString();
                        }
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Binding Excepetion" + ex.Message);
            }
        }
        #endregion


        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(out bool validFlag)
        {
            DirectStockAdmissionBO.DirectStockAdmission stockAdmissionHdr = new DirectStockAdmissionBO.DirectStockAdmission();
            stockAdmissionHdr.GRH_PK = CurrPK;
            stockAdmissionHdr.GRH_NO = (lblDirectStockAdmissionNo.Text == GetGlobalResourceObject("Messages", "DocGenerationNew").ToString()) ?
                string.Empty : lblDirectStockAdmissionNo.Text;
            stockAdmissionHdr.GRH_VERSION = hdfGRH_VERSION.Value;
            stockAdmissionHdr.GRH_DATE = txtAdmissionDate.Text;
            stockAdmissionHdr.GRH_VENDOR = GetNullableInt(ddlSuppliers.SelectedValue).Value;
            stockAdmissionHdr.GRH_VND_REF_NO = txtSupplierRefNo.Text.Trim();
            stockAdmissionHdr.GRH_VND_REF_DATE = txtRefDate.Text;
            stockAdmissionHdr.GRH_INV_DEPT = GetNullableInt(ddlAdmissionStore.SelectedValue).Value;
            stockAdmissionHdr.GRH_COMPANY = GetNullableInt(ddlCompany.SelectedValue).Value;
            stockAdmissionHdr.BIZUNIT_PK = currentUser.SBUID;
            stockAdmissionHdr.ACTIVE = 1;
            stockAdmissionHdr.USER_PK = currentUser.PKUser;
            stockAdmissionHdr.LAST_MOD_DT = lblLastModifiedHDR.Text;
            stockAdmissionHdr.WKF_PROCESS = Convert.ToInt32(hdfProcessID.Value);
            stockAdmissionHdr.WKF_FLAG = (GetNullableInt(WKF_FLAG.Value) ?? 0); // == 1 ? true : false;
            // stockAdmissionHdr.APT_CODE = ApplicationType.DSA;
            stockAdmissionHdr.APT_CODE = ApplicationType.GRN;
            stockAdmissionHdr.AST_DOC_MODE = Convert.ToInt16(GetDOCMODE());
            stockAdmissionHdr.STK_VAL_CONFIRM = (GetNullableInt(ConfirmStockValueChange.Value) ?? 0);
            stockAdmissionHdr.GRH_IS_WORK_ORDER = hdfMenuType.Value == "1" ? 1 : 0;
            stockAdmissionHdr.PO_QTY_VALIDATE = hdfPoQtyValidate.Value;
            //stockAdmissionHdr.GRH_EXCESS_PERC = hdfMenuType.Value == "1" ? Convert.ToDouble(GetGlobalResourceObject("ConfigurationsRes", "GRNExcessQty")) : 0;//Extra % concept is implemented in db
            validFlag = UpdateStockAdmissionItemListFromGridview();

            List<DirectStockAdmissionBO.DirectStockAdmissionDetails> stockAdmissionDtlsList = new List<DirectStockAdmissionBO.DirectStockAdmissionDetails>();
            foreach (DirectStockAdmissionBO.StockAdmissionItem item in StockAdmissionItemList)
            {
                DirectStockAdmissionBO.DirectStockAdmissionDetails stockAdmissionDtl = new DirectStockAdmissionBO.DirectStockAdmissionDetails();
                stockAdmissionDtl.GRD_PK = item.Pk;
                stockAdmissionDtl.GRD_NO = stockAdmissionHdr.GRH_NO;
                stockAdmissionDtl.GRD_DATE = stockAdmissionHdr.GRH_DATE;
                stockAdmissionDtl.GRD_VERSION = stockAdmissionHdr.GRH_VERSION;
                stockAdmissionDtl.GRD_SL_NO = item.SlNo;
                stockAdmissionDtl.GRD_ITEM = item.ItemId;
                stockAdmissionDtl.GRD_QTY_RECEIVED = item.Recieved;
                stockAdmissionDtl.GRD_QTY_ACCEPTED = item.Accepted;
                stockAdmissionDtl.GRD_QTY_REJECTED = item.Rejected;
                stockAdmissionDtl.GRD_UOM = item.UOM;
                stockAdmissionDtl.GRD_PO = item.POId;
                stockAdmissionDtl.GRD_PO_DTL = item.PODetailId;
                stockAdmissionDtl.GRD_DEPT = stockAdmissionHdr.GRH_INV_DEPT;
                stockAdmissionDtl.GRD_BIZUNIT = stockAdmissionHdr.BIZUNIT_PK;
                stockAdmissionDtl.GRD_VND_REF_NO = item.BatchNo; // stockAdmissionHdr.GRH_VND_REF_NO;
                stockAdmissionDtl.GRD_QA_LOT_NO = item.QALotNo;
                stockAdmissionDtl.GRD_DOM = item.DOM;
                stockAdmissionDtl.GRD_DOE = item.DOE;
                stockAdmissionDtl.GRD_LOCATION = item.Location;
                stockAdmissionDtl.GRD_QTY_REAL = item.RealWeight;
                stockAdmissionDtl.GRD_WIH_ITEM_TYPE = item.WIH_ITEM_TYPE.ToString();


                if (item.Rejected > 0)
                {
                    List<DirectStockAdmissionBO.DamageItem> damageItemList = new List<DirectStockAdmissionBO.DamageItem>();

                    if (item.Rejected > 0)
                    {
                        if (item.RejectedList == null) throw new ApplicationException(GetLocalResourceObject("Err_RejectedAllocationNotCompleted").ToString());
                        if (item.Rejected != item.RejectedList.Sum(x => x.Quantity)) throw new ApplicationException(GetLocalResourceObject("Err_RejectedAllocationNotCompleted").ToString());
                    }

                    if (item.RejectedList != null)
                    {
                        foreach (var reject in item.RejectedList)
                        {
                            DirectStockAdmissionBO.DamageItem damageItem = new DirectStockAdmissionBO.DamageItem();
                            damageItem.GDD_PK = reject.Pk;
                            damageItem.GDD_SL_NO = reject.HdrSlNo;
                            damageItem.GDD_GRN_DTL = stockAdmissionDtl.GRD_PK;
                            damageItem.GDD_ITEM = stockAdmissionDtl.GRD_ITEM;
                            damageItem.GDD_DMG_TYPE = reject.ReasonId;
                            damageItem.GDD_DMG_QTY = reject.Quantity;
                            damageItem.GDD_DEPT_STORE = reject.RejectedToStoreId;

                            damageItem.GDD_ITEM_NAME = item.ItemName;
                            damageItem.GDD_DMG_TYPE_TEXT = reject.ReasonText;
                            damageItem.GDD_DEPT_STORE_NAME = reject.RejectedToStoreText;
                            damageItemList.Add(damageItem);
                        }
                    }
                    stockAdmissionDtl.DamageList = damageItemList;
                }
                stockAdmissionDtlsList.Add(stockAdmissionDtl);
            }

            stockAdmissionHdr.StockAdmissionList = stockAdmissionDtlsList;
            stockAdmissionHdr.FileList = DSAUploadList; //File Uploads

            bool HasBatchData = false;
            foreach (GridViewRow row in grdMaterialReturn.Rows)
            {
                ImageButton imbAddGridBatch = (ImageButton)row.FindControl("imbAddGridBatch");
                int WOPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfMatWOPK")).Value);
                int SlNo = Convert.ToInt32(((HiddenField)row.FindControl("hdfMatSlNo")).Value);
                int IssueItemPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfIssueItemPK")).Value);
                int UOMPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfMatUOMPK")).Value);
                DropDownList ddlBatchTemp = (DropDownList)row.FindControl("ddlBatch");
                int batchSelectVal = 0;
                if (ddlBatchTemp.Items.Count > 0)
                    batchSelectVal = Convert.ToInt32(ddlBatchTemp.SelectedValue.Split('_')[0]);
                //if (imbAddGridBatch.Visible == false || batchSelectVal > 0)
                // {
                foreach (DirectStockAdmissionBO.MaterialReturn item in MaterialReturnList.Where(w => w.WOPK == WOPK && w.IssueItemPK == IssueItemPK))
                {
                    if (item.GMR_SCRAP_OR_BGRADE == 1 || item.GMR_SCRAP_OR_BGRADE == 2) //
                    {
                        item.ReturnQty = ((TextBox)row.FindControl("lblReturnQty")).Text == "" || ((TextBox)row.FindControl("lblReturnQty")).Text == string.Empty ? 0 : Convert.ToDecimal(((TextBox)row.FindControl("lblReturnQty")).Text);
                    }
                    // if (item.BatchDetailList != null && item.BatchDetailList.Any())
                    // {
                    if (item.GMR_SCRAP_OR_BGRADE == 0)
                    {
                        //item.BatchDetailList.First().ActualQty = item.ReturnQty = ((TextBox)row.FindControl("lblReturnQty")).Text == "" || ((TextBox)row.FindControl("lblReturnQty")).Text == string.Empty ? 0 : Convert.ToDecimal(((TextBox)row.FindControl("lblReturnQty")).Text);
                        item.ReturnQty = ((TextBox)row.FindControl("lblReturnQty")).Text == "" || ((TextBox)row.FindControl("lblReturnQty")).Text == string.Empty ? 0 : Convert.ToDecimal(((TextBox)row.FindControl("lblReturnQty")).Text);
                        HasBatchData = true;
                    }
                    // }
                }
                // }
            }

            //if (HasBatchData)
            //    stockAdmissionHdr.MaterialReturnList = MaterialReturnList;
            if (MaterialReturnList != null && MaterialReturnList.Any())
            {
                hdfIsStockExceeded.Value = "0";
                foreach (DirectStockAdmissionBO.MaterialReturn item in MaterialReturnList)
                {
                    //If One Batch Select 
                    if (item.BatchDetailList != null && item.BatchDetailList.Count() == 1)
                    {
                        item.BatchDetailList[0].ActualQty = item.ReturnQty;
                    }
                    //foreach (DirectStockAdmissionBO.BatchDetails batch in item.BatchDetailList)
                    //{
                    //    if (batch.ActualQty > batch.StockQty)
                    //        hdfIsStockExceeded.Value = "1";
                    //}
                }
                stockAdmissionHdr.MaterialReturnList = MaterialReturnList.Where(w => w.BatchDetailList != null && w.BatchDetailList.Any() || (w.GMR_SCRAP_OR_BGRADE == 1 || w.GMR_SCRAP_OR_BGRADE == 0 || w.GMR_SCRAP_OR_BGRADE == 2 && w.ReturnQty > 0)).ToList();
            }
            // validFlag = UpdateStockAdmissionItemListFromGridview(true, stockAdmissionHdr.MaterialReturnList);
            return stockAdmissionHdr;
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
                #region CLEAR
                case ControlEnums.CLEAR:
                    hdfGRH_VERSION.Value = 1.ToString();
                    lblDirectStockAdmissionNo.Text = GetGlobalResourceObject("Messages", "DocGenerationNew").ToString();
                    txtAdmissionDate.Text = txtSupplierRefNo.Text = txtRefDate.Text = string.Empty;
                    ddlSuppliers.SelectedIndex = 0;
                    ddlAdmissionStore.SelectedValue = currentUser.CurrentDeptPK.ToString();
                    PendingPOList = null;
                    StockAdmissionItemList = null;

                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();
                    CurrPK = 0;
                    PO_PK.Value = "0";
                    BindGrid(ControlEnums.POS);
                    BindGrid(ControlEnums.STOCKADMISSIONLIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    //txtAdmissionDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfIsContFutureDate.Value = "0";
                    ConfirmStockValueChange.Value = "0";
                    FileDetailsList = null;
                    DSAUploadList = null;
                    anchorFile.Visible = false;
                    BindGrid(ControlEnums.UPLOADEDFILES);
                    hdfIsDSACancelled.Value = "0";
                    companyPK = 0;
                    MaterialReturnList = null;
                    BindGrid(ControlEnums.EDITFORRETURN);
                    hdfPoQtyValidate.Value = "1";
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlEnums.CLEARSEARCH:
                    txtStockAdmissionNo.Text = txtPONo.Text = txtDepartment.Text = hdfDepartmentPkSearch.Value = string.Empty;
                    txtVendor.Text = "type min 4 characters";
                    hdfVendorPKSearch.Value = txtFromDate.Text = txtToDate.Text = string.Empty;
                    hdfStockAdmissionNoPkSearch.Value = hdfPOPkSearch.Value = string.Empty;
                    //ddlStatus.SelectedIndex = 0;
                    ddlStatus.SelectedValue = GetGlobalResourceObject("ConfigurationsRes", "StatusFilterNotApproved").ToString();
                    ddlPlantCode.SelectedIndex = 0;
                    companyPK = 0;
                    break;
                #endregion
                #region CLEARREJECTCONTROLS
                case ControlEnums.CLEARREJECTCONTROLS:
                    hdfDamageSlNo.Value = txtDamageQtyPopup.Text = string.Empty;
                    ddlRejectionReasonPopup.SelectedIndex = ddlRejectToDept.SelectedIndex = 0;
                    break;
                #endregion
                case ControlEnums.ADDITEM:
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    companyPK = 0;
                    break;
                case ControlEnums.FILLBATCHPOPUPDTLS:
                    BatchDetailList = null;
                    MaterialReturn = null;
                    hdfItemPopup.Value = "0";
                    hdfItemTypePopup.Value = "0";
                    lblItemPopup.Text = string.Empty;
                    lblUomNamePopup.Text = string.Empty;
                    hdfUOMPopup.Value = "0";
                    hdfSlNo.Value = "0";
                    lblBatchWOPopup.Text = string.Empty;
                    hdfBatchWOPKPopup.Value = "0";
                    BindGrid(ControlEnums.FILLBATCHGRID);
                    break;
                case ControlEnums.ADDBATCHES:
                    ddlBatchesPopUp.SelectedIndex = 0;
                    txtQtyPopUp.Text = string.Empty;
                    lblStockPopUp.Text = string.Empty;
                    lblBatchWOPopup.Text = string.Empty;
                    hdfBatchWOPKPopup.Value = "0";
                    break;
            }
        }
        #endregion

        private void SwitchTab(int TabNo)
        {
            if (TabNo == 1)
            {
                spnPendingList.Attributes["class"] = "tab-active";
                spnMatReturnList.Attributes["class"] = "tab-inactive";
                divPendingItems.Visible = true;
                divMatReturn.Visible = false;
                divGRlist.Visible = true;
            }
            else
            {
                spnPendingList.Attributes["class"] = "tab-inactive";
                spnMatReturnList.Attributes["class"] = "tab-active";
                divPendingItems.Visible = false;
                divMatReturn.Visible = true;
                divGRlist.Visible = false;
            }
        }

        private void showPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupBatches]','" + Resources.Messages.MsgAddBatches + "','850','auto');", true); // 560
        }

        private bool IsSameBatchExist()
        {
            bool IsExist = false;
            if (BatchDetailList != null)
            {
                int popBatchPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[0]);
                int popWOPK = 0;
                if (ddlBatchesPopUp.SelectedValue.Contains("_"))
                    popWOPK = Convert.ToInt32(ddlBatchesPopUp.SelectedValue.Split('_')[1]);
                if (BatchDetailList.Where(w => w.BatchPK == popBatchPK && w.WOPK == popWOPK).Any())
                    IsExist = true;
            }
            return IsExist;
        }

        private int currentStockinPopup()
        {
            int status = 0;
            double balStock = lblStockPopUp.Text == string.Empty ? 0 : Convert.ToDouble(lblStockPopUp.Text);
            double qtytoadd = txtQtyPopUp.Text == string.Empty ? 0 : Convert.ToDouble(txtQtyPopUp.Text);
            if (qtytoadd > balStock)
            {
                status = 1;
            }
            return status;
        }
        /// <summary>
        /// For checking requird stock in popup
        /// </summary>
        /// <returns></returns>
        private int checkRequiredStockinPopup()
        {
            int status = 0;
            double balance = lblBalanceQty.Text == string.Empty ? 0 : Convert.ToDouble(lblBalanceQty.Text);
            double qtytoadd = txtQtyPopUp.Text == string.Empty ? 0 : Convert.ToDouble(txtQtyPopUp.Text);
            if (qtytoadd > balance)
            {
                status = 1;
            }
            return status;
        }

        private void GridViewPOChangeColour()
        {
            foreach (GridViewRow row in grdPOs.Rows)
            {
                if (((HiddenField)row.FindControl("hdfPOListSelected")).Value == "True")
                {
                    string hexColour = GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString();
                    row.BackColor = System.Drawing.ColorTranslator.FromHtml(hexColour);
                }
            }
        }

        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }

        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            return (decimal.TryParse(str, out result) ? (decimal?)result : null);
        }


        private void BindStockAdmissionItemListFromGridview()
        {
            List<DirectStockAdmissionBO.StockAdmissionItem> stockAdmissionList = new List<DirectStockAdmissionBO.StockAdmissionItem>();
            int slno = 0;
            foreach (GridViewRow grdrow in grdStockAdmissionList.Rows)
            {
                int poDetId = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPODetIdStockAdmissionList")).Value).Value;
                // int pk = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPkStockAdmissionList")).Value).Value;
                // int poId = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPOIdStockAdmissionList")).Value).Value;
                // int UOM = GetNullableInt(((HiddenField)grdrow.FindControl("hdfUOM")).Value).Value;
                // HiddenField hdfPOrate = grdrow.FindControl("hdfPOrate") as HiddenField;
                // double PORate = hdfPOrate.Value == string.Empty ? 0 : Convert.ToDouble(hdfPOrate.Value);
                // string poNo = ((HiddenField)grdrow.FindControl("hdfPONumber")).Value;
                // int itemId = GetNullableInt(((HiddenField)grdrow.FindControl("hdfItemIdStockAdmissionList")).Value).Value;
                // string itemName = ((Label)grdrow.FindControl("lblItemAdmissionList")).Text.Trim();
                // decimal PoQty = GetNullableDecimal(((Label)grdrow.FindControl("lblPoQtyAdmissionList")).Text).Value;
                // decimal PendingQty = GetNullableDecimal(((Label)grdrow.FindControl("lblPendingQtyAdmissionList")).Text).Value;
                decimal Received = GetNullableDecimal(((TextBox)grdrow.FindControl("txtReceivedAdmissionList")).Text).Value;
                decimal Accepted = GetNullableDecimal(((HiddenField)grdrow.FindControl("hdftxtAcceptedAdmissionList")).Value).Value;
                decimal Rejected = GetNullableDecimal(((TextBox)grdrow.FindControl("txtRejectedAdmissionList")).Text).Value;
                string QaLotNO = ((TextBox)grdrow.FindControl("txtQaLotNoAdmissionList")).Text.Trim();
                string BatchNo = ((TextBox)grdrow.FindControl("txtBatchNoAdmissionList")).Text.Trim();
                string DOM = ((TextBox)grdrow.FindControl("txtDOMAdmissionList")).Text.Trim();
                string DOE = ((TextBox)grdrow.FindControl("txtDOEAdmissionList")).Text.Trim();
                string Location = ((TextBox)grdrow.FindControl("txtLocationAdmissionList")).Text.Trim();
                decimal RealWeight = GetNullableDecimal(((TextBox)grdrow.FindControl("txtRealQTYAdmissionList")).Text).Value;

                foreach (var item in StockAdmissionItemList.Where(x => x.PODetailId == poDetId))
                {
                    item.Recieved = Received;
                    item.Accepted = Accepted;
                    item.Rejected = Rejected;
                    item.QALotNo = QaLotNO;
                    item.BatchNo = BatchNo;
                    item.DOM = DOM;
                    item.DOE = DOE;
                    item.Location = Location;
                    item.RealWeight = RealWeight;
                }


                //    DirectStockAdmissionBO.StockAdmissionItem stockadditem = new DirectStockAdmissionBO.StockAdmissionItem();
                //    stockadditem.PODetailId = poDetId;
                //    stockadditem.Pk = pk;
                //    stockadditem.POId = poId;
                //    stockadditem.PONumber = poNo;
                //    stockadditem.UOM = UOM;
                //    stockadditem.PORate = PORate;
                //    stockadditem.ItemId = itemId;
                //    stockadditem.ItemName = itemName;
                //    stockadditem.POQty = PoQty;
                //    stockadditem.PendingQty = PendingQty;
                //    stockadditem.Recieved = Received;
                //    stockadditem.Accepted = Accepted;
                //    stockadditem.Rejected = Rejected;
                //    stockadditem.QALotNo = QaLotNO;
                //    stockadditem.BatchNo = BatchNo;
                //    stockadditem.DOM = DOM;
                //    stockadditem.DOE = DOE;
                //    stockadditem.Location = Location;
                //    stockadditem.RealWeight = RealWeight;
                // // stockadditem.RejectedList=StockAdmissionItemList.Select(x=>x.RejectedList).ToList().Where(s=>s.)
                //    stockAdmissionList.Add(stockadditem);               
            }
            //foreach (var item in stockAdmissionList) item.SlNo = ++slno;
            //StockAdmissionItemList = stockAdmissionList;

        }


        private bool UpdateStockAdmissionItemListFromGridview(bool showValidationMessage = true)
        {
            bool validFlag = true;
            System.Text.StringBuilder message = new System.Text.StringBuilder();

            foreach (GridViewRow grdrow in grdStockAdmissionList.Rows)
            {
                int poDetId = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPODetIdStockAdmissionList")).Value).Value;
                int pk = GetNullableInt(((HiddenField)grdrow.FindControl("hdfPkStockAdmissionList")).Value).Value;
                DirectStockAdmissionBO.StockAdmissionItem item = StockAdmissionItemList
                    .Where(x => x.PODetailId == poDetId && x.Pk == pk)
                    .Single();

                string batchNo = ((TextBox)grdrow.FindControl("txtBatchNoAdmissionList")).Text.Trim();
                string qaLotNo = ((TextBox)grdrow.FindControl("txtQaLotNoAdmissionList")).Text.Trim();
                string dom = ((TextBox)grdrow.FindControl("txtDOMAdmissionList")).Text.Trim();
                string doe = ((TextBox)grdrow.FindControl("txtDOEAdmissionList")).Text.Trim();
                string location = ((TextBox)grdrow.FindControl("txtLocationAdmissionList")).Text.Trim();

                decimal recieved, accepted, rejected, realweight;
                recieved = GetNullableDecimal(((TextBox)grdrow.FindControl("txtReceivedAdmissionList")).Text.Trim()) ?? 0;
                accepted = GetNullableDecimal(((TextBox)grdrow.FindControl("txtAcceptedAdmissionList")).Text.Trim()) ?? 0;
                rejected = GetNullableDecimal(((TextBox)grdrow.FindControl("txtRejectedAdmissionList")).Text.Trim()) ?? 0;
                realweight = GetNullableDecimal(((TextBox)grdrow.FindControl("txtRealQTYAdmissionList")).Text.Trim()) ?? 0;
                //rejected = GetNullableDecimal(((HiddenField)grdrow.FindControl("hdfRejectedAdmissionList")).Value.Trim()) ?? 0;
                //rejected = recieved - accepted;
                item.BatchNo = batchNo;
                item.QALotNo = qaLotNo;
                item.Recieved = recieved;
                item.Accepted = accepted;
                item.Rejected = rejected;
                item.DOM = dom;
                item.DOE = doe;
                item.Location = location;
                item.RealWeight = realweight;



                DirectStockAdmissionBO.PendingPO pendingPO = this.PendingPOList
                    .Where(x => x.PODetId == poDetId)
                    .Single();
                //  decimal maxQty = (pendingPO.POQty + (pendingPO.POQty * ((GetNullableDecimal(hdfOrderPercentage.Value) ?? 0) / 100))) - (pendingPO.PreRecievedQty-pendingPO.POD_QTY_RETURNED);
                if (MaterialReturnList == null)
                {
                    if (recieved <= 0)
                    {
                        string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_ReceivedQtyShouldGraterThanZero").ToString()
                            , item.ItemName).ToString());
                        message.Append(str);
                        validFlag = false;
                    }
                }
                if (MaterialReturnList != null)
                {
                    if (recieved <= 0 && MaterialReturnList.Count == 0)
                    {
                        string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_ReceivedQtyShouldGraterThanZero").ToString()
                            , item.ItemName).ToString());
                        message.Append(str);
                        validFlag = false;
                    }
                }
                //if (recieved > maxQty)//pendingPO.BalanceQtyToRecieve)
                //{
                //    string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_ItemExceedsLimitPercentage").ToString(), item.ItemName
                //        , (GetNullableDecimal(hdfOrderPercentage.Value) ?? 0).ToString()));
                //    message.Append(str);
                //    validFlag = false;
                //}
                if (accepted > recieved)
                {
                    string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_AcceptedQtyExceedsReceived").ToString()
                        , item.ItemName));
                    message.Append(str);
                    validFlag = false;
                }
                if (rejected != (recieved - accepted))
                {
                    string str = CommonFunctions.FormatErrorMessage(string.Format(GetLocalResourceObject("Err_InvalidRejectedQty").ToString(), item.ItemName));
                    message.Append(str);
                    validFlag = false;
                }

            }
            //if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false)
            //{
            if (!validFlag & showValidationMessage)
            {
                string msg = message.ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + msg + "','"
                    + Resources.Messages.Information + "');", true);
            }
            //}
            else
            {
                validFlag = true;
            }
            return validFlag;
        }


        /// <summary>
        /// This Methode is Used to Formating Currency fields in HTML 
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyFormat()
        {
            return this.CurrencyFormatString;
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            string format = "#0.";
            for (int i = 0; i < (GetNullableInt(hdfDecimalCount.Value) ?? 0); i++)
            {
                format += "0";
            }

            string s = num.ToString(format);
            return s;
        }

        public static string GetShortString(object evelOrginalString, int limit)
        {
            string orginalString = HttpUtility.HtmlDecode(Convert.ToString(evelOrginalString));
            return (orginalString.Length <= limit) ? orginalString : (orginalString.Substring(0, limit) + "..");
        }

        private void ApplyGridColor(int RowIndex)
        {
            if (RowIndex >= 0)
            {
                grdBatchDetailsPopup.Rows[RowIndex].BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("RowRedColor").ToString());
            }
            else
            {
                for (int j = 0; j <= grdBatchDetailsPopup.Rows.Count - 1; j++)
                {
                    grdBatchDetailsPopup.Rows[j].BackColor = System.Drawing.ColorTranslator.FromHtml("");
                }
            }
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
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.DSA, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        private bool ConfirmFutureDate(ActionsEnum action)
        {
            bool result = true;
            DateTime currDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
            DateTime grnDate = Convert.ToDateTime(txtAdmissionDate.Text);
            if (currDate < grnDate && hdfIsContFutureDate.Value == "0")
            {
                return false;
            }
            return result;
        }

        //#region "FillProcessId"
        ///// <summary>
        ///// Method to Fill Process ID
        ///// </summary>
        //private void FillProcessId()
        //{
        //    int procId = 0;
        //    // set entry screen path
        //    string path = GTIService.Constants.StockTransfer.Fields.ST_Entry_Screen;
        //    WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
        //    // get process details by path and dept PK
        //    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        //    DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
        //    if (dtProcess != null && dtProcess.Rows.Count > 0)
        //    {
        //        // assign procID
        //        procId = int.Parse(dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString());
        //        hdfProcId.Value = procId.ToString();
        //    }
        //}
        //#endregion

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

        private bool ValidateMultipleWoSelection(bool IsAddToList)
        {
            bool flag = true;
            int Count = 0;
            if (hdfMenuType.Value == "1") //if only WO 
            {
                foreach (GridViewRow grdrow in grdPOs.Rows)
                {
                    CheckBox chk;
                    chk = (CheckBox)grdrow.FindControl("chkSelectPOList");
                    if (chk.Checked)
                    {
                        Count++;
                    }
                    if (Count > 1)
                    {
                        flag = false;
                        break;
                    }
                }
                if (IsAddToList)
                {
                    if (StockAdmissionItemList != null && StockAdmissionItemList.Any())
                        flag = false;
                }
            }
            return flag;
        }

        private bool IsValidMaterialReturn(DirectStockAdmissionBO.DirectStockAdmission directStockAdmission)
        {
            bool IsValid = true;
            try
            {
                if (hdfMenuType.Value == "1")
                {
                    if (directStockAdmission != null && directStockAdmission.MaterialReturnList == null)
                        IsValid = false;
                    else if (directStockAdmission != null && directStockAdmission.MaterialReturnList != null)
                    {
                        foreach (DirectStockAdmissionBO.MaterialReturn matRet in directStockAdmission.MaterialReturnList)
                        {
                            if (matRet.BatchDetailList.Sum(s => s.StockQty) > 0 && matRet.BatchDetailList.Sum(s => s.ActualQty) > 0)
                            {
                                IsValid = true;
                                break;
                            }
                            else
                                IsValid = false;
                        }
                    }
                }
                return IsValid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool IsValidGRNList(DirectStockAdmissionBO.DirectStockAdmission directStockAdmission)
        {
            bool IsValid = true;
            try
            {
                if (directStockAdmission.StockAdmissionList != null)
                {
                    if (directStockAdmission.StockAdmissionList.Sum(s => s.GRD_QTY_RECEIVED) == 0)
                        IsValid = false;
                }
                return IsValid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Check the selected PO's have same items with different rate
        /// </summary>
        /// <param name="poDetIdList"></param>
        /// <returns></returns>
        private bool IsSelectedPOValid(List<int> poDetIdList)
        {
            bool validPO = true;
            List<DirectStockAdmissionBO.StockAdmissionItem> tempStockAdmissionItemList;
            if (poDetIdList.Count > 0) // (poIdList.Count > 0)
            {
                List<DirectStockAdmissionBO.PendingPO> pendingPOTemp = new List<DirectStockAdmissionBO.PendingPO>();
                pendingPOTemp = PendingPOList;
                pendingPOTemp.Where(x => poDetIdList.Contains(x.PODetId)).ToList();
                PendingPOList = pendingPOTemp;

                List<DirectStockAdmissionBO.PendingPO> lstNewSelectedPOs = PendingPOList
                    .Where(x => poDetIdList.Contains(x.PODetId))
                    .ToList();
                int slno = 0;
                List<DirectStockAdmissionBO.StockAdmissionItem> StockAdmissionItemListNew = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                foreach (var item in StockAdmissionItemList)
                {
                    StockAdmissionItemListNew.Add(item);
                }
                if (StockAdmissionItemListNew.Count > 0)
                {
                    slno = StockAdmissionItemListNew.Max(x => x.SlNo);
                }
                List<DirectStockAdmissionBO.StockAdmissionItem> tempNewList = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                tempNewList = lstNewSelectedPOs
                    .Select(x => new DirectStockAdmissionBO.StockAdmissionItem
                    {
                        Pk = 0,
                        POId = x.POId,
                        PODetailId = x.PODetId,
                        PONumber = x.PONumber,
                        ItemId = x.ItemId,
                        ItemName = x.ItemName,
                        UOM = x.UOMId,
                        POQty = x.POQty,
                        PendingQty = x.BalanceQtyToRecieve,
                        Recieved = x.BalanceQtyToRecieve,
                        Accepted = x.BalanceQtyToRecieve,
                        PORate = x.PORate,
                        WIH_ITEM_TYPE = x.WIH_ITEM_TYPE,
                        WIH_ITEM_TYPE_TEXT = x.WIH_ITEM_TYPE_TEXT
                    })
                    .ToList();

                foreach (var item in tempNewList) item.SlNo = ++slno;

                tempStockAdmissionItemList = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                tempStockAdmissionItemList = StockAdmissionItemListNew;
                tempStockAdmissionItemList.AddRange(tempNewList);
                StockAdmissionItemListNew = tempStockAdmissionItemList;

                #region Cannot combine PO's have same Item with different Rate
                List<DirectStockAdmissionBO.StockAdmissionItem> StockAdmissionItemListNewTemp = new List<DirectStockAdmissionBO.StockAdmissionItem>();
                StockAdmissionItemListNewTemp = StockAdmissionItemListNew.GroupBy(ac => new
                {
                    ac.ItemId,
                    ac.PORate
                })
                                                                     .Select(ac => new DirectStockAdmissionBO.StockAdmissionItem
                                                                     {
                                                                         ItemId = ac.Key.ItemId,
                                                                         PORate = ac.Key.PORate
                                                                     }).OrderBy(f => f.ItemId).ToList();
                int itemIdTemp = 0, count = 0;
                bool diffRate = false;
                foreach (var item in StockAdmissionItemListNewTemp)
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
        /// ConfigurationSettings
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }

            hdfGrnSupplierLocation.Value = GetGlobalResourceObject("ConfigurationsRes", "GrnSupplierLocation").ToString();
            hdfGrnExcessQty.Value = GetGlobalResourceObject("ConfigurationsRes", "GRNExcessQty").ToString();
        }

        #region Page Control Enum
        enum ControlEnums
        {
            COMPANY,
            LIST,
            // DETAIL,
            SUPPLIERS,
            ADMISSIONSTORE,
            CLEAR,
            POS,
            //ADMISSIONFROMCHANGE,
            CLEARSEARCH,
            DETAILFOREDIT,
            STOCKADMISSIONLIST,
            ADDTOSTOCKADMISSIONLIST,
            REJECTIONREASON,
            REJECTTO,
            REJECTEDREASONLIST,
            CLEARREJECTCONTROLS,
            POPERCENTAGE,
            POSFROMINBOX,
            SELECTEDDOC,
            UPLOADEDFILES,
            ADDITEM,
            Plant,
            SEARCHTYPE,
            CONVERTLIST,
            EDITFORRETURN,
            FILLITEMBATCHSPOPUP,
            FILLBATCHPOPUPDTLS,
            FILLBATCHGRID,
            ADDBATCHES,
            SUBCONTRACTSTORE,
        }
        #endregion
    }
}
