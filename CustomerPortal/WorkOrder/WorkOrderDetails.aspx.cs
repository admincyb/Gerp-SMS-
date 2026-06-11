using BusinessLogic.CommonManagement;
using BusinessLogic.SubDepartmentManagement;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Sales;
using BusinessObject.WorkOrder;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerPortal.WorkOrder
{
    public partial class WorkOrderDetails : ERP.Store.UI.WorkFlowBasePage//ERP.Store.UI.MyBasePage //
    {
        #region Variables and Properties
        #region Properties
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

        private WorkOrderBO WorkOrderHeader
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.WorkOrderHeaderSession] == null ? null : (WorkOrderBO)ViewState[ERP.Utilities.ViewstateStrings.WorkOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.ViewstateStrings.WorkOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.ViewstateStrings.WorkOrderHeaderSession);
                ViewState.Add(ERP.Utilities.ViewstateStrings.WorkOrderHeaderSession, value);
            }
        }

        private WorkOrderBO TempWorkOrderHeader
        {
            get
            {
                return ViewState[ERP.Utilities.SessionStrings.TempWorkOrderHeaderSession] == null ? null : (WorkOrderBO)ViewState[ERP.Utilities.SessionStrings.TempWorkOrderHeaderSession];
            }
            set
            {
                if (ViewState[ERP.Utilities.SessionStrings.TempWorkOrderHeaderSession] != null)
                    ViewState.Remove(ERP.Utilities.SessionStrings.TempWorkOrderHeaderSession);
                ViewState.Add(ERP.Utilities.SessionStrings.TempWorkOrderHeaderSession, value);
            }
        }

        //private List<BusinessObject.WorkOrder.WorkOrderDetails> WorkOrderDetailList
        //{
        //    get
        //    {
        //        return ViewState[ViewstateStrings.WorkOrderDetailList] == null ? null : (List<BusinessObject.WorkOrder.WorkOrderDetails>)ViewState[ViewstateStrings.WorkOrderDetailList];
        //    }
        //    set
        //    {
        //        ViewState[ViewstateStrings.WorkOrderDetailList] = value;
        //    }
        //}

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
        /// Serail No. of the WO Item
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
        /// To maintain selected SC Detail PK
        /// </summary>
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

        /// <summary>
        /// To maintain the selected WO PK
        /// for Detail Tax
        /// </summary>
        private int SelectedWOItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["SelectedWOItemPK"]);
            }
            set
            {
                this.ViewState["SelectedWOItemPK"] = value;
            }
        }

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
        private bool IsTaxInSBU
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxInSBU] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxInSBU].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxInSBU] = value;
            }
        }
        /// <summary>
        /// To identify whether Header/Detail Tax
        /// </summary>
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
        ///  Text of the Selected Tax
        /// </summary>
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
        /// To Disable Item Tax
        /// </summary>
        private int EnableItemTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemTax] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.DisableItemTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemTax] = value;
            }
        }

        private bool IsTaxForOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherCharge] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherCharge].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherCharge] = value;
            }
        }

        /// <summary>
        /// To Disable Item Discount
        /// </summary>
        private int EnableItemDiscount
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemDiscount] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.DisableItemDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemDiscount] = value;
            }
        }

        /// <summary>
        /// Whether the Tqax is in edit mode
        /// </summary>
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

        private int ProcessID
        {
            get
            {
                return this.ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["ProcessID"].ToString());
            }
            set
            {
                this.ViewState["ProcessID"] = value;
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

        private List<WorkOrderUploads> WOUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.WOUploadList] == null ? null : (List<WorkOrderUploads>)ViewState[ViewstateStrings.WOUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.WOUploadList] = value;
            }
        }

        private List<BusinessObject.Sales.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FileSODetailsList] == null ? null : (List<BusinessObject.Sales.FileDetails>)Session[ERP.Utilities.SessionStrings.FileSODetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FileSODetailsList] = value;
            }
        }

        private int CurrDocSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrDocSlNo] = value;
            }
        }

        private int UserStatus
        {
            get { return ViewState["UserStatus"] == null ? 0 : (int)ViewState["UserStatus"]; }
            set { ViewState["UserStatus"] = value; }
        }

        #endregion
        private BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        DataSet dsWorkOrderTaxDetails;
        DataTable dtWorkOrderTaxDetails;
        List<WorkOrderTaxHdr> WOTaxHdrList;

        bool isCancelled = false;
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations
            = {
                  (a1, a2) => a1 - a2,
                  (a1, a2) => a1 + a2,
                  (a1, a2) => a1 / a2,
                  (a1, a2) => a1 * a2,
                  (a1, a2) => Math.Pow(a1, a2)
              };

        private WorkOrderBO workOrderHeaderObj;
        private BusinessObject.WorkOrder.WorkOrderDetails workOrderDetailsObj;
        List<BusinessObject.WorkOrder.WorkOrderDetails> workOrderDetailsList;
        private List<BillOfMaterials> BillOfMaterialList;

        private DataTable dtCustomTaxSet;
        DataTable dtPageData;
        WorkOrderUploads woUploadObj;
        private DataTable dtResult;
        private DataTable dtBOM;

        private int processPK;
        private string refID;
        private string inboxFlag;

        #endregion

        #region PageLevel Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
            uclPaging.CurrentPage = 1;

            //btnNew.PreRender += new EventHandler(btnAction_PreRender);
            //btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnSave.PreRender += new EventHandler(btnAction_PreRender);
            //btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            //btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            //btnPrint.PreRender += new EventHandler(btnAction_PreRender);

            //btnNew.Load += new EventHandler(btnAction_Load);
            //btnSubmit.Load += new EventHandler(btnAction_Load);
            //btnSave.Load += new EventHandler(btnAction_Load);
            //btnDelete.Load += new EventHandler(btnAction_Load);
            //btnCancel.Load += new EventHandler(btnAction_Load);
            //btnPrint.Load += new EventHandler(btnAction_Load);
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            //(sender as Control).Visible = true;
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
                GetFieldValues(ControlsEnum.WOLIST);
                SetFieldValues(ControlsEnum.WOLIST);
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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                }
                EnableDisablControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            string prefID;
            int referenceID;
            int appId;
            int preferenceID;
            try
            {
                ucrWrkf.ViewType = 1;
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    this.DataBind();
                    EntryStatus = EntryStatus.LISTMODE;

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithComma.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    hdfRateDecimalDigits.Value = rateDecimalDigits.ToString();
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    FileDetailsList = null;
                    WOUploadList = null;

                    //hide/ show detail tax/ discount
                    GetFieldValues(ControlsEnum.TAXSETTINGS);

                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);

                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtPageData.Rows)
                        {
                            if (row["ACF_DATA"].ToString().Equals("DISCOUNT"))
                            {
                                EnableItemDiscount = Convert.ToInt32(row["ACF_VALUE"]);
                            }
                            else if (row["ACF_DATA"].ToString().Equals("TAX"))
                            {
                                EnableItemTax = Convert.ToInt32(row["ACF_VALUE"]);
                            }
                        }
                    }

                    btnNew.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessID());

                    #region Process & Workflow                   
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                      : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        ucrWrkf.RefID = int.Parse(refID);
                        base.WkfRefID = ucrWrkf.RefID;
                        CurrPK = GetApplicationID(ucrWrkf.RefID);

                        ucrWrkf.ViewType = 1;
                        EntryStatus = EntryStatus.VIEWMODE;

                        ucrWrkf.FillWorkFlowDetails();
                        btnSave.Visible = false;

                        if (CurrPK > 0)
                        {
                            ModifiedDatePnl.Visible = true;
                            AssignLocalBreadCrumb();
                            GetFieldValues(ControlsEnum.OPERATION_WORK);
                            SetFieldValues(ControlsEnum.OPERATION_WORK);
                            GetFieldValues(ControlsEnum.BOMTYPE);
                            SetFieldValues(ControlsEnum.BOMTYPE);
                            GetFieldValues(ControlsEnum.STORELOCATION);
                            SetFieldValues(ControlsEnum.STORELOCATION);
                            GetFieldValues(ControlsEnum.DELIVERYTO);
                            SetFieldValues(ControlsEnum.DELIVERYTO);
                            GetFieldValues(ControlsEnum.EDITITEM);
                            SetFieldValues(ControlsEnum.EDITITEM);
                            SetFieldValues(ControlsEnum.WOITEM);
                        }
                    }
                    else
                    {

                    }

                    #endregion

                    if (EntryStatus == EntryStatus.LISTMODE)
                    {
                        GetFieldValues(ControlsEnum.WOLIST);
                        SetFieldValues(ControlsEnum.WOLIST);
                        AssignLocalBreadCrumb();
                    }

                    if (CurrPK == 0)
                    {
                        ModifiedDatePnl.Visible = false;
                        lblWorkOrderNo.Text = Resources.Messages.DocGenerationNew;

                        WorkOrderHeader = new WorkOrderBO()
                        {
                            TaxHdr = new List<WorkOrderTaxHdr>(),
                            WorkOrderDetailList = new List<BusinessObject.WorkOrder.WorkOrderDetails>()
                        };
                        TempWorkOrderHeader = new WorkOrderBO()
                        {
                            TaxHdr = new List<WorkOrderTaxHdr>(),
                            WorkOrderDetailList = new List<BusinessObject.WorkOrder.WorkOrderDetails>()
                        };
                    }

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitComponents();", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID()
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
                ProcessID = ucrWrkf.ProcessID;
                ucrWrkf.FillWorkFlowDetails();
            }
            return ProcessID;
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

        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                double SubTotal = 0;
                FileInfo tempFileInfoObj;
                int selectedItemPK = 0;
                TextBox WrkfComments;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                bool isValidDiscAm = true;
                int result = 0;

                WorkOrderTaxHdr tempWorkOrderTaxHdrObj = null;
                WorkOrderTaxHdr workOrderTaxHdrObj;
                List<WorkOrderTaxHdr> workOrderTaxHdrList;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHECKEDCHANGED;
                }
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                    if (((DropDownList)sender).ID == "ddlItemType")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                switch (commonActions)
                {
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.WOLIST);
                        SetFieldValues(ControlsEnum.WOLIST);
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.WOLIST);
                        SetFieldValues(ControlsEnum.WOLIST);
                        break;
                    #endregion

                    #region SELECTEDINDEXCHANGED

                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        ShowHideControl(Convert.ToInt32(ddlItemType.SelectedValue));
                        break;

                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErpRes.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else //valid
                        {
                            SetUIValuesToObject(ControlsEnum.SAVE);
                            string xmlDoc = CommonFunctions.XmlSerialize<WorkOrderBO>(WorkOrderHeader);
                            if (WorkOrderHeader.WorkOrderDetailList == null || WorkOrderHeader.WorkOrderDetailList.Count == 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_NoItemInList + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }

                            result = BusinessLogic.WorkOrder.WorkOrderBL.SaveWorkOrder(xmlDoc);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.WorkOrderDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                ResetForm(ControlsEnum.WORKORDER);
                                GetFieldValues(ControlsEnum.WOLIST);
                                SetFieldValues(ControlsEnum.WOLIST);
                                EntryStatus = EntryStatus.LISTMODE;
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
                                    litErrorMsg.Text = Resources.PageNameRes.WorkOrderDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.WorkOrderDetails + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.WorkOrderDetails + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EnquiryListing) + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CustomerBrand);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ActivateWorkOrderDetails();
                        AssignLocalBreadCrumb();
                        GetFieldValues(ControlsEnum.ITEMTYPE);
                        SetFieldValues(ControlsEnum.ITEMTYPE);
                        GetFieldValues(ControlsEnum.OPERATION_WORK);
                        SetFieldValues(ControlsEnum.OPERATION_WORK);
                        GetFieldValues(ControlsEnum.BOMTYPE);
                        SetFieldValues(ControlsEnum.BOMTYPE);
                        GetFieldValues(ControlsEnum.STORELOCATION);
                        SetFieldValues(ControlsEnum.STORELOCATION);
                        GetFieldValues(ControlsEnum.DELIVERYTO);
                        SetFieldValues(ControlsEnum.DELIVERYTO);
                        SetFieldValues(ControlsEnum.WOITEM);
                        break;
                    #endregion

                    #region EDIT

                    case ActionsEnum.EDITITEM:
                        ModifiedDatePnl.Visible = true;
                        SetUIEditView(commonActions);
                        AssignLocalBreadCrumb();
                        GetFieldValues(ControlsEnum.OPERATION_WORK);
                        SetFieldValues(ControlsEnum.OPERATION_WORK);
                        GetFieldValues(ControlsEnum.BOMTYPE);
                        SetFieldValues(ControlsEnum.BOMTYPE);
                        GetFieldValues(ControlsEnum.STORELOCATION);
                        SetFieldValues(ControlsEnum.STORELOCATION);
                        GetFieldValues(ControlsEnum.DELIVERYTO);
                        SetFieldValues(ControlsEnum.DELIVERYTO);
                        CurrPK = int.Parse(((ImageButton)sender).CommandArgument.ToString());
                        GetFieldValues(ControlsEnum.EDITITEM);
                        SetFieldValues(ControlsEnum.EDITITEM);
                        SetFieldValues(ControlsEnum.WOITEM);
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                            ucrWrkf.ViewAction();
                        }

                        GridViewRow grvEditRow = (((ImageButton)sender).Parent.Parent as GridViewRow);
                        HiddenField hdfUserStatus = (HiddenField)grvEditRow.FindControl("hdfUserStatus");
                        UserStatus = Convert.ToInt32(hdfUserStatus.Value);
                        break;

                    #endregion

                    #region VIEW

                    case ActionsEnum.VIEW:
                        EntryStatus = EntryStatus.VIEWMODE;
                        ModifiedDatePnl.Visible = true;
                        SetUIEditView(commonActions);
                        AssignLocalBreadCrumb();
                        GetFieldValues(ControlsEnum.OPERATION_WORK);
                        SetFieldValues(ControlsEnum.OPERATION_WORK);
                        GetFieldValues(ControlsEnum.BOMTYPE);
                        SetFieldValues(ControlsEnum.BOMTYPE);
                        GetFieldValues(ControlsEnum.STORELOCATION);
                        SetFieldValues(ControlsEnum.STORELOCATION);
                        GetFieldValues(ControlsEnum.DELIVERYTO);
                        SetFieldValues(ControlsEnum.DELIVERYTO);
                        CurrPK = int.Parse(((ImageButton)sender).CommandArgument.ToString());
                        GetFieldValues(ControlsEnum.EDITITEM);
                        SetFieldValues(ControlsEnum.EDITITEM);
                        SetFieldValues(ControlsEnum.WOITEM);
                        //EnableDisablControls();
                        break;

                    #endregion

                    #region ADDWORKORDERDETAIL
                    case ActionsEnum.ADDWORKORDERDETAIL:
                        if (IsSameItemExist(Convert.ToInt32(ddlWOItemType.SelectedValue), Convert.ToInt32(hdfWorkOrderItem.Value)))
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SameItemExist").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (WorkOrderHeader == null)
                                WorkOrderHeader = new WorkOrderBO();
                            BusinessObject.WorkOrder.WorkOrderDetails objWoItems = (BusinessObject.WorkOrder.WorkOrderDetails)SetUIValuesToObject(ControlsEnum.ADDWORKORDERDETAIL);
                            int slNo = 1;
                            if (WorkOrderHeader.WorkOrderDetailList == null || WorkOrderHeader.WorkOrderDetailList.Count == 0)
                                WorkOrderHeader.WorkOrderDetailList = new List<BusinessObject.WorkOrder.WorkOrderDetails>();
                            else
                                slNo = WorkOrderHeader.WorkOrderDetailList.Max(sl => sl.SlNo) + 1;
                            objWoItems.SlNo = slNo;
                            GetFieldValues(ControlsEnum.BOM);
                            objWoItems.BillOfMaterialList = BillOfMaterialList;

                            WorkOrderHeader.WorkOrderDetailList.Add(objWoItems);
                            BindGrid(ControlsEnum.ADDWORKORDERDETAIL);
                            SetFieldValues(ControlsEnum.BOM);
                            SetFieldValues(ControlsEnum.WOITEM);
                            SetSubTotal();
                            SetHdrTax();
                            ResetForm(ControlsEnum.WORKORDERDETAIL);
                        }
                        break;
                    #endregion

                    #region ADDBOM

                    case ActionsEnum.ADDBOM:
                        SetUIValuesToObject(ControlsEnum.ADDBOM);
                        BindGrid(ControlsEnum.BOM);
                        break;

                    #endregion

                    #region REMOVEWODETAIL
                    case ActionsEnum.REMOVEWORKORDERDETAIL:
                        int WODtlPK = int.Parse(((ImageButton)sender).CommandArgument.ToString());

                        GridViewRow grvWORow = (((ImageButton)sender).Parent.Parent as GridViewRow);
                        HiddenField hdfWODtlItemPK = (HiddenField)grvWORow.FindControl("hdfWODtlItemPK");
                        HiddenField hdfWOdtlOperOrWorkPK = (HiddenField)grvWORow.FindControl("hdfWOdtlOperOrWorkPK");

                        WorkOrderHeader.WorkOrderDetailList.RemoveAll(r => r.ItemPK == Convert.ToInt32(hdfWODtlItemPK.Value)
                        && r.OperationPK == Convert.ToInt32(hdfWOdtlOperOrWorkPK.Value));

                        BindGrid(ControlsEnum.ADDWORKORDERDETAIL);
                        BindGrid(ControlsEnum.BOM);
                        SetSubTotal();
                        break;
                    #endregion

                    #region REMOVEBOMITEM
                    case ActionsEnum.REMOVEBOMITEM:
                        int MaterialPK = int.Parse(((ImageButton)sender).CommandArgument.ToString());
                        GridViewRow grvRow = (((ImageButton)sender).Parent.Parent as GridViewRow);
                        HiddenField hdfBOMDtlWOItem = (HiddenField)grvRow.FindControl("hdfBOMDtlWOItem");
                        HiddenField hdfBOMOperOrWork = (HiddenField)grvRow.FindControl("hdfBOMOperOrWork");
                        BusinessObject.WorkOrder.WorkOrderDetails details = WorkOrderHeader.WorkOrderDetailList
                            .SingleOrDefault(w => w.ItemPK == Convert.ToInt32(hdfBOMDtlWOItem.Value) && w.OperationPK == Convert.ToInt32(hdfBOMOperOrWork.Value));
                        if (details.BillOfMaterialList != null && details.BillOfMaterialList.Count > 0)
                            details.BillOfMaterialList.RemoveAll(r => r.MaterialPK == MaterialPK);

                        // details.BillOfMaterialList.Remove(details.BillOfMaterialList.Where(w=>))
                        // // UpdateBillOfMaterial();

                        //// BillOfMaterialList.Remove(BillOfMaterialList.Where(w => w.SlNo == BOMItemPK).FirstOrDefault());
                        BindGrid(ControlsEnum.BOM);
                        break;
                    #endregion

                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WRKFSubmit
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
                            isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                SetUIValuesToObject(ControlsEnum.SAVE);
                                if (hdfExchangeRate.Value != "-1")
                                {
                                    WorkOrderHeader.WKF_FLAG = 1;
                                    if (WorkOrderHeader.NetAmount > 0)
                                    {
                                        if (WorkOrderHeader != null && WorkOrderHeader.WorkOrderDetailList != null)
                                        {
                                            SaveTransaction(WorkOrderHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SubTotal").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            //|| (Request.QueryString[QueryStrings.PageType] != null &&
                            //Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.WO))
                                {
                                    // ucrWrkf.ApplicationID = CurrPK;
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_WO_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                    {
                                        //if (PIType == "21")
                                        //    FillProcessID(21);
                                        //else
                                        //    FillProcessID(1);
                                        FillProcessID();
                                    }
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.WORKORDER);
                                    GetFieldValues(ControlsEnum.WOLIST);
                                    SetFieldValues(ControlsEnum.WOLIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            //ucrWrkf.ApplicationID = CurrPK;

                            //if (ucrWrkf.ApplicationID > 0)
                            //{
                            //    ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                            //    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            //    //Do WorkFlow if WorkFlow has Actions
                            //    if (ddlWkfAction.Items.Count > 0)
                            //    {
                            //        action = ddlWkfAction.SelectedItem.ToString();
                            //        result = ucrWrkf.DoWorkFlow();

                            //    }
                            //}
                        }
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        this.btnNew.Focus();
                        GetFieldValues(ControlsEnum.WOLIST);
                        SetFieldValues(ControlsEnum.WOLIST);
                        ResetForm(ControlsEnum.WORKORDER);
                        EntryStatus = EntryStatus.LISTMODE;
                        AssignLocalBreadCrumb();
                        break;
                    #endregion

                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        dvPerc.Visible = false;
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (WorkOrderHeader != null)
                        {
                            ResetForm(ControlsEnum.WORKORDERHEADER);
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);

                            if (!string.IsNullOrEmpty(txtSubTotal.Text))
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtSubTotal.Text).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtWorkOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','645','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region SHIPPINGHEADER
                    case ActionsEnum.SHIPPINGHEADER:
                        dvPerc.Visible = false;
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (WorkOrderHeader != null)
                        {
                            ResetForm(ControlsEnum.WORKORDERHEADER);
                            TempWorkOrderHeader = WorkOrderHeader;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            //lblSubTotal = grdBOM.FooterRow == null ? null : (Label)grdBOM.FooterRow.FindControl("lblSubTotalFooter");
                            SubTotal = txtSubTotal.Text == string.Empty ? 0 : Convert.ToDouble(txtSubTotal.Text);
                            if (SubTotal > 0) //(lblSubTotal != null)
                            {
                                double taxable = 0;// Convert.ToDouble(WorkOrderHeaderSession.SOH_TOTAL_AMT) - WorkOrderHeaderSession.SOH_TOTAL_DISCOUNT;
                                txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtWorkOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = Resources.Report.Freight;
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
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("Shipping").ToString() + "','645','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:
                        if (hdfSubContractor.Value == null || hdfSubContractor.Value == "0")
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SubContractor").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                            hdfTaxFormula.Value = string.Empty;
                            if (WorkOrderHeader != null)
                            {
                                ResetForm(ControlsEnum.WORKORDERHEADER);
                                TempWorkOrderHeader = WorkOrderHeader;
                                IsHeaderTax = true;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                GetFieldValues(ControlsEnum.TAXTYPES);
                                SetFieldValues(ControlsEnum.TAXTYPES);
                                double taxable = 0;

                                if (EnableItemTax != 2)
                                {
                                    //taxable = Convert.ToDouble(WorkOrderHeaderSession.SOH_TOTAL_AMT) - WorkOrderHeaderSession.SOH_TOTAL_DISCOUNT;
                                    //Add Other Charges Based on configuration
                                    if (IsTaxForOtherCharge)
                                    {
                                        taxable += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : Convert.ToDouble(txtShipping.Text);
                                    }
                                }
                                else
                                {
                                    ResetForm(ControlsEnum.TAXCHECKBOX);
                                    if (chkSubTotal.Checked)
                                        taxable += txtSubTotal.Text != string.Empty ? Convert.ToDouble(txtSubTotal.Text) : 0;//SubTotal;
                                    if (chkDiscount.Checked)
                                        taxable = txtHdrDiscount.Text != string.Empty ? taxable - Convert.ToDouble(txtHdrDiscount.Text) : taxable;
                                    if (chkOtherCharges.Checked)
                                        taxable += txtShipping.Text != string.Empty ? Convert.ToDouble(txtShipping.Text) : 0;
                                }

                                txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtWorkOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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
                                if (EnableItemTax == 2)
                                    divTaxApplicableAmount.Attributes.Add("style", "display:block;");
                                else
                                    divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','645','300');", true);
                            }
                        }
                        break;

                    #endregion

                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempWorkOrderHeader != null)
                        {
                            workOrderHeaderObj = TempWorkOrderHeader;
                            tempWorkOrderTaxHdrObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempWorkOrderTaxHdrObj = workOrderHeaderObj.TaxHdr == null ? null :
                                        workOrderHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.TaxPK == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && ctr.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempWorkOrderTaxHdrObj = workOrderHeaderObj.TaxHdr == null ? null :
                                        workOrderHeaderObj.TaxHdr.SingleOrDefault(t => t.TaxName == txtPopupOther.Text.Trim() && t.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                if (CurrSlNo > 0)
                                {
                                    workOrderDetailsObj = workOrderHeaderObj.WorkOrderDetailList.SingleOrDefault(row => CurrSlNo == row.SlNo);
                                }
                                else
                                {
                                    workOrderDetailsObj = workOrderHeaderObj.WorkOrderDetailList == null ? null :
                                     workOrderHeaderObj.WorkOrderDetailList.LastOrDefault(wod => wod.WorkOrderDetailPK == SelectedDtlPK
                                     && wod.ItemPK == SelectedWOItemPK && wod.ItemPK == selectedItemPK);
                                }

                                if (workOrderDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempWorkOrderTaxHdrObj = workOrderDetailsObj.Tax == null ? null :
                                            workOrderDetailsObj.Tax.SingleOrDefault(rfq => rfq.TaxPK == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempWorkOrderTaxHdrObj = workOrderDetailsObj.Tax == null ? null :
                                            workOrderDetailsObj.Tax.SingleOrDefault(rfq => rfq.TaxName == txtPopupOther.Text.Trim() && rfq.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempWorkOrderTaxHdrObj == null)
                            {
                                workOrderTaxHdrList = new List<WorkOrderTaxHdr>();

                                workOrderTaxHdrObj = new WorkOrderTaxHdr();
                                try
                                {
                                    workOrderTaxHdrObj.TaxAmount = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    workOrderTaxHdrObj.WorkOrderPK = SelectedDtlPK;
                                    workOrderTaxHdrObj.SlNo = 1;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        workOrderTaxHdrObj.TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    workOrderTaxHdrObj.DiscountPercentage = txtTaxPerc.Text == "" || Convert.ToDouble(txtTaxPerc.Text) < 0 ? 0 : Convert.ToDouble(txtTaxPerc.Text);
                                    workOrderTaxHdrObj.TaxName = HttpUtility.HtmlEncode(SelectedTaxText);
                                    //workOrderTaxHdrObj.SLT_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    workOrderTaxHdrObj.TaxHeaderPK = 0;
                                    workOrderTaxHdrObj.TaxCategoryPK = Convert.ToInt32(hdfTaxCategory.Value);
                                    workOrderTaxHdrObj.TaxTypePK = Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0 ? 1 : 2;//1 => Defined Tax, 2=> Custom Tax.
                                    workOrderTaxHdrObj.TaxFormula = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (workOrderTaxHdrObj.TaxCategoryPK == (int)TaxType.Tax && EnableItemTax == 2)
                                        {
                                            workOrderTaxHdrObj.HasSubTotal = chkSubTotal.Checked ? 1 : 0;
                                            workOrderTaxHdrObj.HasDiscount = chkDiscount.Checked ? 1 : 0;
                                            workOrderTaxHdrObj.HasOtherCharge = chkOtherCharges.Checked ? 1 : 0;
                                        }
                                        if (workOrderTaxHdrObj.TaxCategoryPK == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(workOrderHeaderObj.TotalAmount);
                                            currentTotal = workOrderHeaderObj.TaxHdr == null ? 0 :
                                                workOrderHeaderObj.TaxHdr.Where(htx => htx.TaxCategoryPK == ((int)TaxType.Discount)).Sum(disc => disc.TaxAmount);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(workOrderTaxHdrObj.TaxFormula, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = workOrderTaxHdrObj.TaxAmount;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                //if (Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Amend)
                                                //{
                                                //    if (workOrderHeaderObj.SOH_AMT_INVOICED > 0)
                                                //    {
                                                //        if (workOrderHeaderObj.SOH_TOTAL_DISCOUNT > (currentTotal + taxAmt))
                                                //        {
                                                //            isValidDiscAm = false;
                                                //        }
                                                //    }
                                                //}
                                                if (isValidDiscAm == true)
                                                {
                                                    workOrderTaxHdrList = workOrderHeaderObj.TaxHdr.ToList();
                                                    workOrderTaxHdrList.Add(workOrderTaxHdrObj);
                                                    workOrderHeaderObj.TaxHdr = workOrderTaxHdrList;
                                                }
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            workOrderTaxHdrList = workOrderHeaderObj.TaxHdr.ToList();
                                            workOrderTaxHdrList.Add(workOrderTaxHdrObj);
                                            workOrderHeaderObj.TaxHdr = workOrderTaxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        if (CurrSlNo > 0)
                                        {
                                            workOrderDetailsObj = workOrderHeaderObj.WorkOrderDetailList.SingleOrDefault(row => CurrSlNo == row.SlNo);
                                        }
                                        else
                                        {
                                            workOrderDetailsObj = workOrderHeaderObj.WorkOrderDetailList == null ? null :
                                                workOrderHeaderObj.WorkOrderDetailList.LastOrDefault(item => item.WorkOrderPK == SelectedDtlPK
                                                && item.ItemPK == SelectedWOItemPK);

                                        }
                                        if (workOrderDetailsObj != null)
                                        {
                                            if (workOrderTaxHdrObj.TaxCategoryPK == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = workOrderDetailsObj.Amount;
                                                currentTotal = workOrderDetailsObj.Tax == null ? 0 :
                                                    workOrderDetailsObj.Tax.Where(dtx => dtx.TaxCategoryPK == ((int)TaxType.Discount)).Sum(disc => disc.TaxAmount);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(workOrderTaxHdrObj.TaxFormula, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = workOrderTaxHdrObj.TaxAmount;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    workOrderTaxHdrList = workOrderDetailsObj.Tax == null ? new List<WorkOrderTaxHdr>() : workOrderDetailsObj.Tax.ToList();
                                                    workOrderTaxHdrList.Add(workOrderTaxHdrObj);
                                                    if (CurrSlNo > 0)
                                                    {
                                                        workOrderHeaderObj.WorkOrderDetailList.LastOrDefault(row => CurrSlNo == row.SlNo).Tax = workOrderTaxHdrList;
                                                    }
                                                    else
                                                    {
                                                        workOrderHeaderObj.WorkOrderDetailList.LastOrDefault(rfq => rfq.WorkOrderPK == SelectedDtlPK
                                                            && rfq.ItemPK == SelectedWOItemPK).Tax = workOrderTaxHdrList;
                                                    }
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                workOrderTaxHdrList = workOrderDetailsObj.Tax == null ? new List<WorkOrderTaxHdr>() : workOrderDetailsObj.Tax.ToList();
                                                workOrderTaxHdrList.Add(workOrderTaxHdrObj);
                                                if (CurrSlNo > 0)
                                                {
                                                    workOrderHeaderObj.WorkOrderDetailList.SingleOrDefault(row => CurrSlNo == row.SlNo).Tax = workOrderTaxHdrList;
                                                }
                                                else
                                                {
                                                    workOrderHeaderObj.WorkOrderDetailList.LastOrDefault(rfq => rfq.WorkOrderPK == SelectedDtlPK
                                                        && rfq.ItemPK == SelectedWOItemPK).Tax = workOrderTaxHdrList;
                                                }
                                            }
                                        }
                                    }
                                    TempWorkOrderHeader = workOrderHeaderObj;
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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("Shipping").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','645','300');", true);

                        if (errorTaxAdd)
                        {
                            if (hdfTaxCategory.Value == "1")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (hdfTaxCategory.Value == "2")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Shipping_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (hdfTaxCategory.Value == "3")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Disc_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        if (!isValidDiscAm)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount_Amend").ToString()) + "','" + Resources.ErpRes.Information + "');", true);

                        break;
                    #endregion

                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (TempWorkOrderHeader != null)
                        {
                            workOrderHeaderObj = TempWorkOrderHeader;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                workOrderTaxHdrList = new List<WorkOrderTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempWorkOrderTaxHdrObj = workOrderHeaderObj.TaxHdr == null ? null :
                                            workOrderHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.TaxPK == taxPK && rfq.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempWorkOrderTaxHdrObj = workOrderHeaderObj.TaxHdr == null ? null :
                                                workOrderHeaderObj.TaxHdr.LastOrDefault(rfq => rfq.TaxName == hdfTaxName.Value && rfq.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempWorkOrderTaxHdrObj != null)
                                    {
                                        workOrderTaxHdrList = workOrderHeaderObj.TaxHdr.ToList();
                                        workOrderTaxHdrList.Remove(tempWorkOrderTaxHdrObj);
                                        workOrderHeaderObj.TaxHdr = workOrderTaxHdrList;
                                    }
                                }
                                else
                                {
                                    //if (CurrSlNo > 0)
                                    //{
                                    //    workOrderDetailsObj = workOrderHeaderObj.WorkOrderDetailList == null ? null :
                                    //       workOrderHeaderObj.WorkOrderDetailList.SingleOrDefault(row => CurrSlNo == row.SlNo);
                                    //}
                                    //else
                                    //{
                                    //    workOrderDetailsObj = workOrderHeaderObj.WorkOrderDetailList == null ? null :
                                    //     workOrderHeaderObj.WorkOrderDetailList.LastOrDefault(rfq => rfq.WorkOrderPK == SelectedDtlPK
                                    //     && rfq.ItemPK == SelectedWOItemPK && rfq.ItemPK == selectedItemPK);

                                    //}
                                    //if (workOrderDetailsObj != null)
                                    //{
                                    //    if (taxPK > 0)
                                    //    {
                                    //        tempWorkOrderTaxHdrObj = workOrderDetailsObj.TaxDtl == null ? null :
                                    //            workOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_TAX == taxPK && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    //    }
                                    //    else
                                    //    {
                                    //        if (hdfTaxName != null)
                                    //        {
                                    //            tempWorkOrderTaxHdrObj = workOrderDetailsObj.TaxDtl == null ? null :
                                    //                workOrderDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.SLT_NAME == hdfTaxName.Value && rfq.SLT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    //        }
                                    //    }
                                    //    if (CurrSlNo > 0)
                                    //    {
                                    //        workOrderDetailsObj = workOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);
                                    //    }
                                    //    else
                                    //    {
                                    //        workOrderDetailsObj = workOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                    //            && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK);
                                    //    }

                                    //    if (workOrderDetailsObj != null && workOrderDetailsObj.TaxDtl != null)
                                    //    {
                                    //        workOrderTaxHdrList = workOrderDetailsObj.TaxDtl.ToList();
                                    //        workOrderTaxHdrList.Remove(tempWorkOrderTaxHdrObj);
                                    //        if (CurrSlNo > 0)
                                    //        {
                                    //            workOrderHeaderObj.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO).TaxDtl = workOrderTaxHdrList;
                                    //        }
                                    //        else
                                    //        {
                                    //            workOrderHeaderObj.SaleContractDetails.LastOrDefault(rfq => rfq.SOD_PK == SelectedDtlPK
                                    //                && rfq.SOD_CUST_ITEM == SelectedCusItemPK && rfq.SOD_ITEM == SelectedItemPK).TaxDtl = workOrderTaxHdrList;
                                    //        }
                                    //    }
                                    //}
                                }

                                TempWorkOrderHeader = workOrderHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

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
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtWorkOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("Shipping").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','645','300');", true);
                        break;
                    #endregion

                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        if (IsHeaderTax)
                        {
                            WorkOrderHeader = TempWorkOrderHeader;
                            SetSubTotal();
                            SetHdrTax();
                        }
                        else
                        {
                            //SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            //SelectedWOItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
                            if (CurrSlNo > 0)
                            {
                                workOrderDetailsObj = TempWorkOrderHeader.WorkOrderDetailList.SingleOrDefault(row => CurrSlNo == row.SlNo);
                            }
                            else
                            {
                                workOrderDetailsObj = TempWorkOrderHeader.WorkOrderDetailList.LastOrDefault(wdt => wdt.WorkOrderDetailPK == SelectedDtlPK
                                && wdt.WorkOrderPK == SelectedWOItemPK);
                            }

                            if (workOrderDetailsObj != null)
                            {
                                CurrSlNo = workOrderDetailsObj.SlNo;
                            }
                            SetDetailTax(TempWorkOrderHeader);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion

                    #region TAXPOPUP CHECK CHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        if (((CheckBox)sender).ID == "chkSubTotal" || ((CheckBox)sender).ID == "chkDiscount" || ((CheckBox)sender).ID == "chkOtherCharges")
                        {
                            hdfTaxFormula.Value = string.Empty;
                            double taxable = 0;
                            if (chkSubTotal.Checked)
                            {
                                //lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                                //taxable += lblSubTotal.Text != string.Empty ? Convert.ToDouble(lblSubTotal.Text) : 0;
                                SubTotal = txtSubTotal.Text == string.Empty ? 0 : Convert.ToDouble(txtSubTotal.Text);
                                taxable += SubTotal;
                            }
                            if (chkDiscount.Checked)
                            {
                                taxable = txtHdrDiscount.Text != string.Empty ? taxable - Convert.ToDouble(txtHdrDiscount.Text) : taxable;
                            }
                            if (chkOtherCharges.Checked)
                            {
                                taxable += txtShipping.Text != string.Empty ? Convert.ToDouble(txtShipping.Text) : 0;
                            }
                            txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    TaxPK = 0;
                                    if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count == 1)
                                    {
                                        string taxFormula = dtWorkOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                        hdfTaxFormula.Value = taxFormula;
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                        txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                        SelectedTaxText = HttpUtility.HtmlDecode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
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

                            //SetHdrTax();
                            //SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            if (EnableItemTax == 2)
                                divTaxApplicableAmount.Attributes.Add("style", "display:block;");

                            else
                                divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','645','300');", true);
                        }
                        break;
                    #endregion

                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            dvPerc.Visible = false;
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtWorkOrderTaxDetails.Rows[0]["TAX_FORMULA"].ToString();

                                if (taxFormula == "0")
                                {
                                    hdfTaxFormula.Value = "0";
                                    SelectedTaxText = HttpUtility.HtmlEncode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    hdfTaxFormula.Value = taxFormula;
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                    txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                    SelectedTaxText = HttpUtility.HtmlEncode(dtWorkOrderTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
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
                            txtPopupOther.Text = Resources.Controls.Discount;
                            if (hdfTaxCategory.Value == ((int)TaxType.Discount).ToString())
                                dvPerc.Visible = true;
                            else
                                dvPerc.Visible = false;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Shipping) ? GetLocalResourceObject("Shipping").ToString() : (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString())) + "','645','300');", true);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','645','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','645','300');", true);
                        break;
                    #endregion

                    #region ATTACHDOCS
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
                            if (!string.IsNullOrEmpty(fupUpload.PostedFile.FileName))
                            {
                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                if (!IsValidExtension(tempFileInfoObj.Extension))
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    if (CurrDocSlNo != 0)
                                    {
                                        if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                        {
                                            woUploadObj = WOUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrDocSlNo);
                                            if (woUploadObj != null)
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
                                                    woUploadObj.AttachmentFileName = attachmentFileName;
                                                    woUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                    woUploadObj.DOC_NAME = fupUpload.FileName;
                                                    woUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                    {
                                                        woUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                    }
                                                    else
                                                    {
                                                        woUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;

                                                    }
                                                    FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrDocSlNo);
                                                    if (fileDetailsObj == null)
                                                    {
                                                        FileDetailsList.Add(new FileDetails() { SlNo = CurrDocSlNo, SoFile = HttpContext.Current.Request.Files[0] });
                                                    }
                                                    else
                                                    {
                                                        fileDetailsObj.SoFile = HttpContext.Current.Request.Files[0];
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
                                            if (WOUploadList == null || WOUploadList.Count == 0)
                                            {
                                                WOUploadList = new List<WorkOrderUploads>();
                                                slno = 1;
                                            }
                                            else
                                            {
                                                slno = WOUploadList.Max(itm => itm.DOC_SEQ_NO);
                                                slno++;
                                            }
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<FileDetails>();
                                            }

                                            woUploadObj = new WorkOrderUploads();
                                            woUploadObj.DOC_PK = 0;
                                            woUploadObj.DOC_SEQ_NO = slno;
                                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                            string attachmentFileFormat = tempFileInfoObj.Extension;
                                            string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                            woUploadObj.AttachmentFileName = attachmentFileName;
                                            woUploadObj.FileExtension = tempFileInfoObj.Extension;
                                            woUploadObj.DOC_NAME = fupUpload.FileName;
                                            woUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                            {
                                                woUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                            }
                                            else
                                            {
                                                woUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                            }

                                            woUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                            FileDetailsList.Add(new FileDetails() { SlNo = slno, SoFile = HttpContext.Current.Request.Files[0] });
                                            WOUploadList.Add(woUploadObj);
                                        }
                                    }
                                }
                            }
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            ResetForm(ControlsEnum.ADDITEM);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalcTotal", "CalculateTotal();", true);
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (WOUploadList != null && WOUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                WOUploadList = WOUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                if (FileDetailsList != null)
                                    FileDetailsList = FileDetailsList.Where(attdoc => selectedItemPK != attdoc.SlNo).ToList();
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEMUPLOAD:
                        if (WOUploadList != null && WOUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                woUploadObj = WOUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                        #endregion
                        #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region For Grid Actions

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((sender as GridView).ID == "grdWorkOrder")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        ImageButton imbEdit = e.Row.FindControl("imbEdit") as ImageButton;
                        ImageButton imbView = e.Row.FindControl("imbView") as ImageButton;
                        ImageButton imbPrint = e.Row.FindControl("imbPrint") as ImageButton;

                        int UserStatus = Convert.ToInt32((e.Row.FindControl("hdfUserStatus") as HiddenField).Value);
                        int poStatus = Convert.ToInt32((e.Row.FindControl("hdfWOStatus") as HiddenField).Value);

                        if (UserStatus == 1)
                        {
                            imbEdit.Visible = true;
                            imbView.Visible = false;
                        }
                        else if (UserStatus == 0)
                        {
                            imbEdit.Visible = false;
                        }
                        else if (UserStatus == 2)
                        {
                            imbEdit.Visible = true;
                            imbView.Visible = false;
                        }

                        if (poStatus == 4)
                        {
                            imbEdit.Visible = false;
                            imbView.Visible = true;
                        }

                        imgApproved.CssClass = GetLocalResourceObject("NotIssued").ToString();
                        imgApproved.ToolTip = GetLocalResourceObject("NotIssuedText").ToString(); //Resources.Captions.Pending;
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
                    if (SortDirection == Resources.ErpRes.SortAscending)
                        SortDirection = Resources.ErpRes.SortDescending;
                    else
                        SortDirection = Resources.ErpRes.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.ErpRes.SortAscending;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Method used to Handle all Command actions of gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {

        }

        #endregion

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
                string pageUrl = string.Empty;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (type)
                {
                    #region BOM
                    case ControlsEnum.BOM:
                        string XmlResult = BusinessLogic.MaterialManagement.MaterialMaster.
                            GetOrderItemByTypeItemQty(Convert.ToInt32(ddlWOItemType.SelectedValue), Convert.ToInt32(hdfWorkOrderItem.Value),
                            Convert.ToDecimal(txtWOQty.Text), Convert.ToInt32(ddlOperOrWork.SelectedValue));
                        if (XmlResult != string.Empty)
                        {
                            BillOfMaterialRoot root = CommonFunctions.XmlDeserialize<BillOfMaterialRoot>(XmlResult);
                            BillOfMaterialList = root.BillOfMaterialList;
                            //if (BillOfMaterialList == null)
                            //    BillOfMaterialList = root.BillOfMaterialList;
                            //else
                            //{
                            //    root.BillOfMaterialList.ForEach(f =>
                            //    {
                            //        BillOfMaterialList.Add(f);
                            //    });
                            //}

                        }
                        break;
                    #endregion

                    #region ITEMTYPE

                    case ControlsEnum.ITEMTYPE:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "WO ITEM TYPE");
                        break;

                    #endregion

                    #region ITEMCATEGORY

                    case ControlsEnum.ITEMCATEGORY:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PRODUCT SUB TYPE");
                        break;

                    #endregion

                    #region TAXTYPES
                    case ControlsEnum.TAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsWorkOrderTaxDetails = BusinessLogic.Sales.QuotationBL.GetQuotationTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK));
                            if (dsWorkOrderTaxDetails != null && dsWorkOrderTaxDetails.Tables.Count > 0)
                            {
                                dtWorkOrderTaxDetails = dsWorkOrderTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Shipping == category)//shipping is not under sales or Purchase
                            {
                                dtWorkOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtWODate.Text), 0, TaxFilterType.SAL, 1);
                            }
                            else
                            {
                                if ((int)TaxType.Discount != category)
                                {
                                    dtWorkOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtWODate.Text), 0, TaxFilterType.SAL, 1, 0, 1);
                                }
                                else
                                {
                                    dtWorkOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtWODate.Text), 0);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region CUSTOMTAXSETTINGS
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
                    #endregion

                    #region TAXSETTINGS
                    case ControlsEnum.TAXSETTINGS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break;
                    #endregion

                    #region OPERATION_WORK
                    case ControlsEnum.OPERATION_WORK:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, "WO OPERATION");
                        break;
                    #endregion

                    #region BOMTYPE

                    case ControlsEnum.BOMTYPE:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, "BOM TYPE");
                        break;

                    #endregion

                    #region STORELOCATION

                    case ControlsEnum.STORELOCATION:
                        dtResult = SubDepartmentMaster.GetInventoryStoresByConfig_New(currentUser.SBUID, 3, 0);
                        break;

                    #endregion

                    #region STORELOCATION

                    case ControlsEnum.DELIVERYTO:
                        dtResult = SubDepartmentMaster.GetGeneralStoresByConfig(0, 2, currentUser, currentUser.SBUID, 0);
                        break;

                    #endregion

                    #region WOLIST

                    case ControlsEnum.WOLIST:
                        int WOPK = 0, WOItem = 0, SubContrPK = 0, status = 0;

                        GridPrams gridParamObj = new GridPrams();
                        gridParamObj.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        gridParamObj.PageSize = grdWorkOrder.PageSize;
                        gridParamObj.SearchBy = "";
                        gridParamObj.SearchValue = Convert.ToInt32(hdfWONo.Value) > 0 ? txtWONo.Text : "%%";
                        gridParamObj.SortBy = SortBy = SortBy == null ? GetLocalResourceObject("SortBy").ToString() : SortBy;//"MAS_FROM_DT"
                        gridParamObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        gridParamObj.FromDate = txtFromDate.Text;
                        gridParamObj.ToDate = txtToDate.Text;
                        gridParamObj.BizUnit = currentUser.SBUID;
                        gridParamObj.UserPK = currentUser.PKUser;

                        pageUrl = Resources.PageURL.WorkOrderDetails;
                        SubContrPK = Convert.ToInt32(hdfSubContr.Value);
                        WOItem = Convert.ToInt32(hdfWOItem.Value);
                        WOPK = Convert.ToInt32(hdfWONo.Value);
                        status = Convert.ToInt32(ddlStatus.SelectedValue);

                        DataSet dsWorkOrder = new DataSet();
                        dsWorkOrder = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderList(gridParamObj, SubContrPK, WOPK, WOItem, status, pageUrl);
                        gridParamObj.TotalRecords = Convert.ToInt32(dsWorkOrder.Tables[0].Rows[0][0]);
                        TotalPages = (gridParamObj.TotalRecords == 0) ? 1 : (gridParamObj.TotalRecords <= gridParamObj.PageSize) ? 1 : (gridParamObj.TotalRecords % gridParamObj.PageSize) == 0 ? (gridParamObj.TotalRecords / gridParamObj.PageSize) : (gridParamObj.TotalRecords / gridParamObj.PageSize) + 1;
                        dtResult = dsWorkOrder.Tables[1];
                        break;

                    #endregion

                    #region EDITITEM

                    case ControlsEnum.EDITITEM:
                        string xmlResult = BusinessLogic.WorkOrder.WorkOrderBL.GetWorkOrderByPK(CurrPK);
                        WorkOrderHeader = CommonFunctions.XmlDeserialize<WorkOrderBO>(xmlResult);
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
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ITEMTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.ITEMCATEGORY:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.BOM:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(ControlsEnum.TAXPOPUPGRID);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        //if (workOrderHeaderObj != null)
                        //    GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.OPERATION_WORK:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.BOMTYPE:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.STORELOCATION:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.DELIVERYTO:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.WOITEM:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.WOLIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.EDITITEM:
                        GetUIValuesFromObject(ControlsEnum.EDITITEM);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Metods

        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(WorkOrderBO objWorkorder, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objWorkorder == null)
                objWorkorder = new WorkOrderBO();
            #region Transaction Log and Application Code

            //objWorkorder.ATL_APP_TYPE = ApplicationType.PI;
            //objWorkorder.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
            //                   : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objWorkorder.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            objWorkorder.WKF_APPLICATION = CurrPK;
            objWorkorder.WKF_COMMENTS = wkfDetails.Comments;
            objWorkorder.WKF_TRX_FLAG = workflowFlag;
            objWorkorder.WKF_PROCESS = wkfDetails.ProcessID;
            objWorkorder.WKF_REFERENCE = wkfDetails.ReferenceID;
            objWorkorder.WKF_TASK = wkfDetails.TaskID;
            objWorkorder.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            //if (isCancelled)
            //    objWorkorder.ATL_ACTION = (byte)LogAction.CANCEL;
            //else
            //    objWorkorder.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<WorkOrderBO>(objWorkorder);
            string woNumber = string.Empty;
            result = BusinessLogic.WorkOrder.WorkOrderBL.SaveWorkOrderWkf(xmlDoc, out woNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                #region File Upload
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
                {
                    #region File Uploads
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
                    #endregion
                }
                #endregion
                if (result.HasValue && result.Value > 0)
                {
                    if (isCancelled)
                    {
                        //if (PIType == "21")
                        //    FillProcessID(21);
                        //else
                        //    FillProcessID(1);

                        FillProcessID();

                        litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                    }
                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                    TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                    WrkfComments.Text = "";
                    if (string.IsNullOrEmpty(woNumber))
                        woNumber = lblWorkOrderNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.WorkOrderDetails;
                    args[1] = woNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    //Show Save Message and redired to listing page                                        
                    //litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.WorkOrderDetails);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ControlsEnum.WORKORDER);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm(ControlsEnum.WORKORDER);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.WOLIST);
                        SetFieldValues(ControlsEnum.WOLIST);
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
                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }

        }
        #endregion

        private void ShowHideControl(int type)
        {
            try
            {
                switch (type)
                {
                    case 1://Material
                        trCustomer.Visible = false;
                        tdWOItem.Visible = true;
                        tdBrand.Visible = false;
                        tdProduct.Visible = false;
                        break;
                    case 2://Products
                        trCustomer.Visible = false;
                        tdWOItem.Visible = false;
                        tdBrand.Visible = false;
                        tdProduct.Visible = true;
                        GetFieldValues(ControlsEnum.ITEMCATEGORY);
                        SetFieldValues(ControlsEnum.ITEMCATEGORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitProducts();", true);
                        break;
                    case 3://Brands
                        trCustomer.Visible = true;
                        tdWOItem.Visible = false;
                        tdBrand.Visible = true;
                        tdProduct.Visible = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "InitCustomerBrands();", true);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EDIT

                    case ControlsEnum.EDITITEM:
                        lblWorkOrderNo.Text = WorkOrderHeader.WorkOrderNo;
                        txtWODate.Text = WorkOrderHeader.WorkOrderDate.ToString(Resources.ErpRes.DateFormat);
                        txtRefNo.Text = WorkOrderHeader.WorkOrderRefNo;
                        txtSubContractor.Text = WorkOrderHeader.Vendor;
                        hdfSubContractor.Value = WorkOrderHeader.VendorPK.ToString();
                        hdfSubContractorItem.Value = WorkOrderHeader.VendorPK.ToString();
                        if (WorkOrderHeader.DepartmentPK > 0)
                            ddlLocation.SelectedValue = WorkOrderHeader.DepartmentPK.ToString();
                        if (WorkOrderHeader.DepartmentTo > 0)
                            ddlDeliveryTo.SelectedValue = WorkOrderHeader.DepartmentTo.ToString();

                        grdWODetails.DataSource = WorkOrderHeader.WorkOrderDetailList;
                        grdWODetails.DataBind();
                        grdBOM.DataSource = GetBillOfMaterials();
                        grdBOM.DataBind();

                        txtSubTotal.Text = WorkOrderHeader.TotalAmount.ToString();
                        txtHdrDiscount.Text = WorkOrderHeader.TotalDiscount.ToString();
                        txtShipping.Text = WorkOrderHeader.TotalShipCharge.ToString();
                        txtHdrTax.Text = WorkOrderHeader.TotalTax.ToString();
                        txtPriceAdj.Text = WorkOrderHeader.TotalAdjust.ToString();
                        txtHdrTotal.Text = WorkOrderHeader.NetAmount.ToString();
                        LastModifiedTime = WorkOrderHeader.ModifiedDate;
                        lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        break;

                    #endregion

                    #region  FILE_UPLOAD
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = workOrderHeaderObj.WorkOrderPK;
                        WOUploadList = workOrderHeaderObj.FileList;
                        break;
                    case ControlsEnum.SELECTEDDOC:
                        if (woUploadObj != null)
                        {
                            CurrDocSlNo = woUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = woUploadObj.DOC_NAME;
                            anchorFile.HRef = woUploadObj.DOC_PATH;
                            anchorFile.Attributes.Remove("onclick");
                            anchorFile.Style.Add("cursor", "pointer");
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrDocSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                                anchorFile.Style.Add("cursor", "default");
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>       
        private Object SetUIValuesToObject(ControlsEnum setType)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            BusinessObject.WorkOrder.WorkOrderDetails wod;
            object retObject;
            retObject = null;
            try
            {
                switch (setType)
                {
                    case ControlsEnum.SAVE:
                        if (WorkOrderHeader == null)
                            WorkOrderHeader = new WorkOrderBO();
                        if (CurrPK > 0)
                            WorkOrderHeader.WorkOrderPK = CurrPK;
                        WorkOrderHeader.BizUnitPK = currentUser.SBUID;
                        WorkOrderHeader.CreatedDate = DateTime.Now;
                        WorkOrderHeader.CreatedUserPK = currentUser.PKUser;
                        WorkOrderHeader.VendorPK = Convert.ToInt32(hdfSubContractor.Value);
                        WorkOrderHeader.WorkOrderDate = Convert.ToDateTime(txtWODate.Text);
                        WorkOrderHeader.WorkOrderNo = lblWorkOrderNo.Text;
                        WorkOrderHeader.WorkOrderRefNo = txtRefNo.Text;
                        WorkOrderHeader.ModifiedDate = LastModifiedTime;
                        if (ddlDeliveryTo.SelectedValue != string.Empty && Convert.ToInt32(ddlDeliveryTo.SelectedValue) > 0)
                            WorkOrderHeader.DepartmentTo = Convert.ToInt32(ddlDeliveryTo.SelectedValue);
                        if (ddlLocation.SelectedValue != string.Empty && Convert.ToInt32(ddlLocation.SelectedValue) > 0)
                            WorkOrderHeader.DepartmentPK = Convert.ToInt32(ddlLocation.SelectedValue);
                        WorkOrderHeader.TotalQuantity = WorkOrderHeader.WorkOrderDetailList.Sum(s => s.Quantity);
                        WorkOrderHeader.NetAmount = txtHdrTotal.Text == string.Empty ? 0 : Convert.ToDouble(txtHdrTotal.Text);
                        break;
                    case ControlsEnum.ADDWORKORDERDETAIL:
                        wod = new BusinessObject.WorkOrder.WorkOrderDetails();

                        wod.OperationPK = Convert.ToInt32(ddlOperOrWork.SelectedValue);
                        wod.Operation = ddlOperOrWork.SelectedItem.Text;
                        wod.WorkOrderNo = lblWorkOrderNo.Text;
                        wod.ItemTypePK = Convert.ToInt32(ddlWOItemType.SelectedValue);
                        wod.ItemType = ddlWOItemType.SelectedItem.Text;
                        wod.ItemPK = Convert.ToInt32(hdfWorkOrderItem.Value);
                        wod.Item = txtWorkOrderItem.Text;
                        wod.UOMPK = Convert.ToInt32(hdfItemUOM.Value);
                        wod.UOM = txtUOM.Text;
                        wod.Rate = Convert.ToDouble(txtWORate.Text);
                        wod.Quantity = Convert.ToDouble(txtWOQty.Text);
                        wod.Amount = Convert.ToDouble(txtWOAmt.Text);
                        wod.RequiredDate = Convert.ToDateTime(txtWODtlDate.Text);
                        wod.Remarks = txtWODtlRemarks.Text;
                        retObject = wod;
                        break;
                    case ControlsEnum.ADDBOM:
                        if (WorkOrderHeader != null)
                        {
                            BillOfMaterials bom = new BillOfMaterials();
                            int woItem = Convert.ToInt32(ddlBOMWOItem.SelectedValue);
                            //wod = new BusinessObject.WorkOrder.WorkOrderDetails();
                            //wod = WorkOrderHeader.WorkOrderDetailList.FindLast(w => w.ItemPK == woItem);

                            bom.ActualQuantity = Convert.ToInt32(txtBOMQty.Text);
                            bom.Quantity = Convert.ToInt32(txtBOMQty.Text);
                            bom.OperationPK = Convert.ToInt32(ddlBOMOperWork.SelectedValue);
                            bom.Operation = ddlBOMOperWork.SelectedItem.Text;
                            bom.ItemPK = Convert.ToInt32(ddlBOMWOItem.SelectedValue);
                            bom.Item = ddlBOMWOItem.SelectedItem.Text;
                            bom.BOMTypePK = Convert.ToInt32(ddlBOMType.SelectedValue);
                            bom.BOMType = ddlBOMType.SelectedItem.Text;
                            bom.CategoryPK = Convert.ToInt32(hdfBOMCategory.Value);
                            bom.Category = txtBOMCategory.Text;
                            bom.MaterialPK = Convert.ToInt32(hdfBOMItem.Value);
                            bom.Material = txtBOMItem.Text;
                            bom.UOMPK = Convert.ToInt32(hdfBOMUOM.Value);
                            bom.UOM = txtBOMUOM.Text;

                            if (WorkOrderHeader.WorkOrderDetailList == null || WorkOrderHeader.WorkOrderDetailList.Count == 0)
                                WorkOrderHeader.WorkOrderDetailList = new List<BusinessObject.WorkOrder.WorkOrderDetails>();
                            else
                                WorkOrderHeader.WorkOrderDetailList.FindLast(w => w.ItemPK == woItem).BillOfMaterialList.Add(bom);
                        }
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
        /// Is Same Item Exist in WODetailList
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        private bool IsSameItemExist(int itemTypePK, int itemPK)
        {
            bool result = false;
            if (WorkOrderHeader == null && WorkOrderHeader.WorkOrderDetailList != null)
                result = false;
            else
                result = WorkOrderHeader.WorkOrderDetailList.Count(w => w.ItemPK == itemPK && w.ItemTypePK == itemTypePK) > 0;
            return result;

        }

        private List<BillOfMaterials> GetBillOfMaterials()
        {
            List<BillOfMaterials> lstResult = new List<BillOfMaterials>();
            int slNo = 1;
            if (WorkOrderHeader != null && WorkOrderHeader.WorkOrderDetailList != null)
            {
                WorkOrderHeader.WorkOrderDetailList.ForEach(wo =>
                {
                    if (wo.BillOfMaterialList != null)
                    {
                        wo.BillOfMaterialList.ForEach(bil =>
                        {
                            bil.SlNo = slNo++;
                            lstResult.Add(bil);
                        });

                    }
                });
            }
            return lstResult;

        }
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.ADDWORKORDERDETAIL:
                        if (WorkOrderHeader != null)
                            grdWODetails.DataSource = WorkOrderHeader.WorkOrderDetailList;
                        else
                            grdWODetails.DataSource = null;
                        grdWODetails.DataBind();
                        break;
                    case ControlsEnum.BOM:
                        grdBOM.DataSource = GetBillOfMaterials();// BillOfMaterialList;// dtBOM;
                        grdBOM.DataBind();
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax && WorkOrderHeader != null)
                        {
                            WOTaxHdrList = TempWorkOrderHeader.TaxHdr == null ? new List<WorkOrderTaxHdr>() :
                                TempWorkOrderHeader.TaxHdr.Where(tax => tax.TaxCategoryPK == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            //if (CurrSlNo > 0)
                            //{
                            //    contractDetails = TempWorkOrderHeader.SaleContractDetails == null ? null :
                            //        TempWorkOrderHeader.SaleContractDetails.SingleOrDefault(row => CurrSlNo == row.SOD_SL_NO);

                            //}
                            //else
                            //{
                            //    contractDetails = TempWorkOrderHeader.SaleContractDetails == null ? null :
                            //        TempWorkOrderHeader.SaleContractDetails.LastOrDefault(dtl => dtl.SOD_PK == SelectedDtlPK
                            //        && dtl.SOD_CUST_ITEM == SelectedCusItemPK && dtl.SOD_ITEM == SelectedItemPK);
                            //}

                            //if (contractDetails != null)
                            //{
                            //    contractTaxHdrList = contractDetails.TaxDtl == null ? new List<SaleOrderTaxHdr>() :
                            //        contractDetails.TaxDtl.Where(tax => Convert.ToInt32(tax.SLT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            //}
                            //else
                            //    contractTaxHdrList = new List<SaleOrderTaxHdr>();
                        }
                        if (hdfTaxCategory.Value == ((int)TaxType.Discount).ToString())
                        {
                            grdTaxDetails.Columns[2].Visible = true; // show the discount percentage column when the popup is against Discount
                        }
                        else
                        {
                            grdTaxDetails.Columns[2].Visible = false;// hide the discount percentage column when the popup is not against Discount
                        }
                        grdTaxDetails.DataSource = WOTaxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        grdUploads.DataSource = WOUploadList;
                        grdUploads.DataBind();
                        break;
                    case ControlsEnum.WOLIST:
                        TotalPages = TotalPages;
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? "1" : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdWorkOrder.DataSource = dtResult;
                        grdWorkOrder.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
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
                    #region ITEMTYPE

                    case ControlsEnum.ITEMTYPE:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlWOItemType.DataSource = dtResult;
                            ddlWOItemType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlWOItemType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlWOItemType.DataBind();
                            ddlItemType.DataSource = dtResult;
                            ddlItemType.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlItemType.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlItemType.DataBind();
                        }
                        //ddlWOItemType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        //ddlWOItemType.SelectedIndex = 0;
                        break;

                    #endregion

                    #region ITEMCATEGORY

                    case ControlsEnum.ITEMCATEGORY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlProduct.DataSource = dtResult;
                            ddlProduct.DataTextField = Resources.DataFieldRes.cfgData;
                            ddlProduct.DataValueField = Resources.DataFieldRes.cfgValue;
                            ddlProduct.DataBind();
                        }
                        ddlProduct.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    #endregion

                    #region TAXTYPES

                    case ControlsEnum.TAXTYPES:
                        ddlPopupTaxType.Items.Clear();
                        if (dtWorkOrderTaxDetails != null && dtWorkOrderTaxDetails.Rows.Count > 0)
                        {
                            ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtWorkOrderTaxDetails, "TAX_DISP_NAME");
                            ddlPopupTaxType.DataTextField = "TAX_DISP_NAME";
                            ddlPopupTaxType.DataValueField = "TAX_PK";
                            ddlPopupTaxType.DataBind();
                        }

                        if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                            ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                        break;

                    #endregion

                    #region OPERATION_WORK

                    case ControlsEnum.OPERATION_WORK:
                        ddlOperOrWork.DataSource = dtResult;
                        ddlOperOrWork.DataTextField = "CFG_DATA";
                        ddlOperOrWork.DataValueField = "CFG_VALUE";
                        ddlOperOrWork.DataBind();
                        ddlOperOrWork.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                        ddlBOMOperWork.DataSource = dtResult;
                        ddlBOMOperWork.DataTextField = "CFG_DATA";
                        ddlBOMOperWork.DataValueField = "CFG_VALUE";
                        ddlBOMOperWork.DataBind();
                        ddlBOMOperWork.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;

                    #endregion

                    case ControlsEnum.BOMTYPE:
                        ddlBOMType.DataSource = dtResult;
                        ddlBOMType.DataTextField = "CFG_DATA";
                        ddlBOMType.DataValueField = "CFG_VALUE";
                        ddlBOMType.DataBind();
                        ddlBOMType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.STORELOCATION:
                        ddlLocation.DataSource = dtResult;
                        ddlLocation.DataTextField = "DPT_NAME";
                        ddlLocation.DataValueField = "DPT_PK";
                        ddlLocation.DataBind();
                        //ddlLocation.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.DELIVERYTO:
                        ddlDeliveryTo.DataSource = dtResult;
                        ddlDeliveryTo.DataTextField = "DPT_NAME";
                        ddlDeliveryTo.DataValueField = "DPT_PK";
                        ddlDeliveryTo.DataBind();
                        ddlDeliveryTo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        break;
                    case ControlsEnum.WOITEM:
                        if (WorkOrderHeader != null && WorkOrderHeader.WorkOrderDetailList != null)
                        {
                            ddlBOMWOItem.DataSource = WorkOrderHeader.WorkOrderDetailList;
                            ddlBOMWOItem.DataTextField = "Item";
                            ddlBOMWOItem.DataValueField = "ItemPK";
                            ddlBOMWOItem.DataBind();
                            ddlBOMWOItem.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        }
                        else
                        {
                            ddlBOMWOItem.DataSource = null;
                            if (ddlBOMWOItem.Items.Count == 0)
                                ddlBOMWOItem.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnableDisablControls()
        {
            try
            {
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    btnAddItem.Visible = false;
                    imbBOMAdd.Visible = false;
                    ddlBOMOperWork.Enabled = false;
                    ddlBOMType.Enabled = false;
                    ddlBOMWOItem.Enabled = false;
                    ddlLocation.Enabled = false;
                    ddlOperOrWork.Enabled = false;
                    ddlPopupTaxType.Enabled = false;
                    //ddlWOItemType.Enabled = false;
                    ddlDeliveryTo.Enabled = false;
                    btnSubmit.Visible = false;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                }
                else
                {
                    btnAddItem.Visible = true;
                    imbBOMAdd.Visible = true;
                    ddlBOMOperWork.Enabled = true;
                    ddlBOMType.Enabled = true;
                    ddlBOMWOItem.Enabled = true;
                    ddlLocation.Enabled = true;
                    ddlOperOrWork.Enabled = true;
                    ddlPopupTaxType.Enabled = true;
                    //ddlWOItemType.Enabled = true;
                    ddlDeliveryTo.Enabled = true;
                    btnSubmit.Visible = true;
                    btnSave.Visible = true;
                    btnDelete.Visible = true;
                }

                if ((UserStatus == 1 || UserStatus == 0) && EntryStatus != EntryStatus.VIEWMODE) //drafted or new
                {
                    btnSave.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                }
                string viewMode = ((int)EntryStatus).ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ViewMode", "ViewMode("+ viewMode + ");", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetSubTotal()
        {
            try
            {
                double SubTotal = 0;

                if (WorkOrderHeader != null && WorkOrderHeader.WorkOrderDetailList != null && WorkOrderHeader.WorkOrderDetailList.Any())
                {
                    SubTotal = WorkOrderHeader.WorkOrderDetailList.Sum(s => s.Amount);
                    WorkOrderHeader.TotalAmount = SubTotal;
                }

                txtSubTotal.Text = txtSubTotal.ToolTip = SubTotal.ToString(hdfCurrencyFormat.Value);
            }
            catch (Exception ex)
            {

                throw ex;
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

            if (WorkOrderHeader != null)
            {
                workOrderHeaderObj = WorkOrderHeader;
                amount = Convert.ToDouble(workOrderHeaderObj.TotalAmount);

                #region Discount
                discount = 0;
                if (workOrderHeaderObj.TaxHdr != null)
                {
                    var discHeader = workOrderHeaderObj.TaxHdr.Where(hdr => hdr.TaxCategoryPK == ((int)TaxType.Discount));
                    foreach (WorkOrderTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.TaxFormula;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.TaxAmount = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    discount = workOrderHeaderObj.TaxHdr.Where(quotation => quotation.TaxCategoryPK == ((int)TaxType.Discount)).Sum(quotation => quotation.TaxAmount);
                }
                workOrderHeaderObj.TotalDiscount = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;
                #endregion

                #region Shipping Charge
                if (workOrderHeaderObj.TaxHdr != null)
                {
                    var shippingHeader = workOrderHeaderObj.TaxHdr.Where(quotation => quotation.TaxCategoryPK == ((int)TaxType.Shipping));
                    foreach (WorkOrderTaxHdr taxHdrObj in shippingHeader)
                    {
                        string taxFormula = taxHdrObj.TaxFormula;
                        if (!string.IsNullOrEmpty(taxFormula) && taxFormula != "0")
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.TaxAmount = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    shipping = workOrderHeaderObj.TotalShipCharge = workOrderHeaderObj.TaxHdr.Where(quotation => quotation.TaxCategoryPK == ((int)TaxType.Shipping)).Sum(quotation => quotation.TaxAmount);
                }
                txtShipping.Text = txtShipping.ToolTip = shipping.ToString(hdfCurrencyFormat.Value);
                workOrderHeaderObj.TotalShipCharge = shipping;
                #endregion

                #region Tax
                if (EnableItemTax != 2)
                {
                    //Add Other Charges Based on configuration
                    if (IsTaxForOtherCharge)
                    {
                        amount += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : Convert.ToDouble(txtShipping.Text);
                    }
                }

                if (workOrderHeaderObj.TaxHdr != null)
                {
                    var taxHeader = workOrderHeaderObj.TaxHdr.Where(quotation => quotation.TaxCategoryPK == ((int)TaxType.Tax));
                    foreach (WorkOrderTaxHdr taxHdrObj in taxHeader)
                    {
                        if (EnableItemTax == 2)
                        {
                            amount = 0;
                            if (taxHdrObj.HasSubTotal == 1)
                                amount += Convert.ToDouble(workOrderHeaderObj.TotalAmount);
                            if (taxHdrObj.HasDiscount == 1)
                                amount = txtHdrDiscount.Text != string.Empty ? amount - Convert.ToDouble(txtHdrDiscount.Text) : amount;
                            if (taxHdrObj.HasOtherCharge == 1)
                                amount += txtShipping.Text != string.Empty ? Convert.ToDouble(txtShipping.Text) : 0;
                        }
                        string taxFormula = taxHdrObj.TaxFormula;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            //Commented for RBPL rounding issue
                            //taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            taxHdrObj.TaxAmount = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }

                    if (EnableItemTax != 1)
                        workOrderHeaderObj.TotalTax = workOrderHeaderObj.TaxHdr.Where(quotation => quotation.TaxCategoryPK == ((int)TaxType.Tax)).Sum(quotation => quotation.TaxAmount);
                    else
                        workOrderHeaderObj.TotalTax = 0;
                }
                #endregion

                txtHdrTax.Text = txtHdrTax.ToolTip = workOrderHeaderObj.TotalTax.ToString(hdfCurrencyFormat.Value);
                //double.TryParse(txtShipping.Text, out shipping);

                double.TryParse(txtPriceAdj.Text, out adjust);
                workOrderHeaderObj.TotalAdjust = adjust;
                workOrderHeaderObj.NetAmount = Convert.ToDouble(workOrderHeaderObj.TotalAmount) - workOrderHeaderObj.TotalDiscount + workOrderHeaderObj.TotalTax
                    + workOrderHeaderObj.TotalShipCharge + workOrderHeaderObj.TotalAdjust;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = workOrderHeaderObj.NetAmount.ToString(hdfCurrencyFormat.Value);
                WorkOrderHeader = workOrderHeaderObj;
                TempWorkOrderHeader = workOrderHeaderObj;
                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalcTotal", "CalculateTotal();", true);
            }
            return true;
        }

        private void SetDetailTax(WorkOrderBO workOrderHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;
            //if (rdbPackingSpec.Checked == true)
            //{
            //    if (double.TryParse(txtspecprice.Text, out quantity) && double.TryParse(txtspecqty.Text, out rate))
            //    {
            //        if (txtAmount != null)
            //        {
            //            txtAmount.Text = GetFormattedCurrency(rate * quantity);

            //            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
            //            //SelectedCusItemPK = string.IsNullOrEmpty(hdfPrdPackSpec.Value) ? 0 : Convert.ToInt32(hdfPrdPackSpec.Value);
            //            SelectedCusItemPK = 0;
            //            SelectedItemPK = string.IsNullOrEmpty(hdfPrdPackSpec.Value) ? 0 : Convert.ToInt32(hdfPrdPackSpec.Value);
            //            SetItemTax(workOrderHdr);
            //        }
            //    }
            //    else
            //    {
            //        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            //    }
            //}
            //else
            //{
            //    if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(txtBrandQuantity.Text, out rate))
            //    {
            //        if (txtAmount != null)
            //        {
            //            txtAmount.Text = GetFormattedCurrency(rate * quantity);

            //            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
            //            SelectedCusItemPK = string.IsNullOrEmpty(hdfBrand.Value) ? 0 : Convert.ToInt32(hdfBrand.Value);
            //            SelectedItemPK = string.IsNullOrEmpty(hdfProduct.Value) ? 0 : Convert.ToInt32(hdfProduct.Value);
            //            SetItemTax(workOrderHdr);
            //        }
            //    }
            //    else
            //    {
            //        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            //    }
            //}
        }

        private bool SetItemTax(WorkOrderBO workOrderHdr)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            //Double.TryParse(txtAmount.Text.Trim(), out amount);
            if (amount >= 0)
            {
                if (true)//(saleContractHdr != null)
                {
                    //saleOrderHeaderObj = saleContractHdr;
                    //if (CurrSlNo > 0)
                    //{

                    //    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                    //        && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK && crt.SOD_SL_NO == CurrSlNo);
                    //}
                    //else
                    //{
                    //    saleOrderDetailsObj = saleOrderHeaderObj.SaleContractDetails.LastOrDefault(crt => crt.SOD_PK == SelectedDtlPK
                    //        && crt.SOD_CUST_ITEM == SelectedCusItemPK && crt.SOD_ITEM == SelectedItemPK);
                    //}
                    //if (saleOrderDetailsObj != null)
                    //{
                    //    if (saleOrderDetailsObj.TaxDtl != null)
                    //    {
                    //        var discDetail = saleOrderDetailsObj.TaxDtl.Where(quotation => quotation.SLT_TAX_CATEGORY == ((int)TaxType.Discount));
                    //        foreach (SaleOrderTaxHdr taxHdrObj in discDetail)
                    //        {
                    //            string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                    //            if (!string.IsNullOrEmpty(taxFormula))
                    //            {
                    //                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                    //                taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    //            }
                    //        }
                    //        discount = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SLT_TAX_AMT);
                    //    }
                    //    netAmount = amount - discount;
                    //    txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                    //    if (saleOrderDetailsObj.TaxDtl != null)
                    //    {
                    //        var taxDetail = saleOrderDetailsObj.TaxDtl.Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                    //        foreach (SaleOrderTaxHdr taxHdrObj in taxDetail)
                    //        {
                    //            string taxFormula = taxHdrObj.SLT_TAX_FORMULA;
                    //            if (!string.IsNullOrEmpty(taxFormula))
                    //            {
                    //                taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                    //                taxHdrObj.SLT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    //            }
                    //        }
                    //        itmTax = saleOrderDetailsObj.TaxDtl.ToList().Where(ctr => ctr.SLT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.SLT_TAX_AMT);
                    //    }
                    //    txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                    //    saleOrderDetailsObj.SOD_AMOUNT = amount;
                    //    saleOrderDetailsObj.SOD_DISCOUNT = EnableItemDiscount == 0 ? 0 : discount;
                    //    saleOrderDetailsObj.SOD_TAX = EnableItemTax == 0 ? 0 : itmTax;
                    //    saleOrderDetailsObj.SOD_NET_AMOUNT = (amount - (EnableItemDiscount == 0 ? 0 : discount) + (EnableItemTax == 0 ? 0 : itmTax));
                    //    txtTotal.Text = saleOrderDetailsObj.SOD_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
                    //    //TempWorkOrderHeader = saleOrderHeaderObj;
                    //}
                }
                return true;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                return false;
            }
        }

        private void ConfigurationSettings()
        {
            try
            {
                DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
                if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
                {
                    hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
                }

                DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "TAX");
                if (dt != null && dt.Rows.Count > 0)
                    IsTaxInSBU = dt.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;

                IsTaxForOtherCharge = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales")));
            }
            catch (Exception ex)
            {

                throw ex;
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

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
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

        private void ActivateWorkOrderDetails()
        {
            PageAction_Entry.Visible = true;
        }

        /// <summary>
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                string breadCrumb = string.Empty;
                if (EntryStatus == EntryStatus.NEWMODE)
                {
                    if (this.GetLocalResourceObject("Breadcrumb") != null)
                    {
                        breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                    }
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    if (this.GetLocalResourceObject("BreadcrumbList") != null)
                    {
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.WORKORDERHEADER:
                    TempWorkOrderHeader = WorkOrderHeader;
                    CurrSlNo = 0;
                    //hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;                    
                    dvPerc.Visible = false;
                    break;
                case ControlsEnum.WORKORDERDETAIL:
                    txtWorkOrderItem.Text = string.Empty;
                    hdfWorkOrderItem.Value = string.Empty;
                    ddlWOItemType.SelectedIndex = 0;
                    txtUOM.Text = string.Empty;
                    hdfItemUOM.Value = string.Empty;
                    txtWORate.Text = string.Empty;
                    txtWOQty.Text = string.Empty;
                    txtWOAmt.Text = string.Empty;
                    txtWODtlDate.Text = string.Empty;
                    txtWODtlRemarks.Text = string.Empty;
                    break;
                case ControlsEnum.WORKORDER:
                    CurrPK = 0;
                    ModifiedDatePnl.Visible = false;
                    lblWorkOrderNo.Text = Resources.Messages.DocGenerationNew;
                    txtSubContractor.Text = string.Empty;
                    hdfSubContractor.Value = "0";
                    hdfSubContractorItem.Value = "0";
                    txtRefNo.Text = string.Empty;
                    ddlOperOrWork.SelectedIndex = 0;
                    txtWorkOrderItem.Text = string.Empty;
                    hdfWorkOrderItem.Value = "0";
                    txtUOM.Text = string.Empty;
                    hdfItemUOM.Value = "0";
                    txtWORate.Text = string.Empty;
                    txtWOQty.Text = string.Empty;
                    txtWOAmt.Text = string.Empty;
                    txtWODtlRemarks.Text = string.Empty;
                    //WorkOrderHeader = null;
                    WorkOrderHeader = new WorkOrderBO()
                    {
                        TaxHdr = new List<WorkOrderTaxHdr>(),
                        WorkOrderDetailList = new List<BusinessObject.WorkOrder.WorkOrderDetails>()
                    };
                    ddlBOMOperWork.SelectedIndex = 0;
                    ddlBOMWOItem.SelectedIndex = 0;
                    ddlBOMType.SelectedIndex = 0;
                    txtBOMCategory.Text = string.Empty;
                    hdfBOMCategory.Value = "0";
                    txtBOMItem.Text = string.Empty;
                    hdfBOMItem.Value = "0";
                    txtBOMUOM.Text = string.Empty;
                    hdfBOMUOM.Value = "0";
                    txtBOMQty.Text = string.Empty;
                    grdWODetails.DataSource = null;
                    grdWODetails.DataBind();
                    grdBOM.DataSource = null;
                    grdBOM.DataBind();
                    grdTaxDetails.DataSource = null;
                    grdTaxDetails.DataBind();
                    txtSubTotal.Text = string.Empty;
                    txtHdrDiscount.Text = string.Empty;
                    txtShipping.Text = string.Empty;
                    txtHdrTax.Text = string.Empty;
                    txtPriceAdj.Text = string.Empty;
                    txtHdrTotal.Text = string.Empty;
                    UserStatus = 0;
                    break;
                case ControlsEnum.CLEAR:
                    txtSubContr.Text = string.Empty;
                    hdfSubContr.Value = "0";
                    txtFromDate.Text = string.Empty;
                    hdfFromDate.Value = string.Empty;
                    txtToDate.Text = string.Empty;
                    hdfToDate.Value = string.Empty;
                    txtWOItem.Text = string.Empty;
                    hdfWOItem.Value = "0";
                    txtWONo.Text = string.Empty;
                    hdfWONo.Value = "0";
                    break;
            }
        }

        public enum ControlsEnum
        {
            WORKORDERHEADER,
            WORKORDERDETAIL,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXCHECKBOX,
            CUSTOMTAXSETTINGS,
            TAXSETTINGS,
            UPLOADEDFILES,
            ADDITEM,
            SELECTEDDOC,
            OPERATION_WORK,
            BOMTYPE,
            STORELOCATION,
            ADDWORKORDERDETAIL,
            BOM,
            SAVE,
            WOITEM,
            WORKORDER,
            ADDBOM,
            WOLIST,
            EDITITEM,
            DELIVERYTO,
            CLEAR,
            ITEMCATEGORY,
            ITEMTYPE
        }

        #endregion
    }

    //public enum ControlsEnum
    //{
    //    PENDINGORDERSLIST,
    //    PRODUCTPROPERTY,
    //    PROPERTY,
    //    ORDERNUMBER,
    //    PLANHEADER,
    //    PRODUCTPLANGROUP,
    //    EDIT,
    //    PRODUCTPLANGROUPLIST,
    //    SBU,
    //    PLANLISTING,
    //    CLEARLIST,
    //    PLANDETAILS,
    //    ORDERDETAILS,
    //    LINELIST,
    //    LINEDETAILS,
    //    LINEGRID,
    //    SIZE,
    //    CUSTOMER,
    //    ALLOCATIONDETAILS,
    //    SUMMARYDETAILS,
    //    SUMMARYPLANTLIST,
    //    SUMMARYLINELIST,
    //    SUMMARYGROUPLIST,
    //    PLANVERSIONS,
    //    RELEASESC,
    //    BINDETAILS,
    //    LINEWISESUMMARY,
    //    SCDETAILS,
    //    SCENARIODETAILS,
    //    MACHINEFILL,
    //    LINEFORMERDETAILS,
    //    GROUPLINES,
    //    PLANGROUPLINES,
    //    PLANCALCULATION,
    //    SELECTALLPAGEITEMS
    //}
    class Common
    {
        public const string CON_NAME = "CON_NAME";
        public const string CON_CODE = "CON_CODE";
        public const string CON_PK = "CON_PK";
        public const string F_CUS_PK = "CUS_PK";
        public const string F_CUS_CODE = "CUS_CODE";
        public const string F_BZU_CODE = "BZU_CODE";
        public const string F_BZU_PK = "BZU_PK";
        public const string F_PIG_PK = "PIG_PK";
        public const string F_PIG_NAME = "PIG_NAME";
        public const string F_SOH_NO = "SOH_NO";
        public const string F_SOD_PK = "SOD_PK";
        public const string F_SOH_PK = "SOH_PK";

    }
}