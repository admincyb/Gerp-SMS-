using BusinessLogic.Finance;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Finance
{
    public partial class AssetDisposal : ERP.Store.UI.WorkFlowBasePage
    {
        #region Properties & Variables

        #region Properties
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

        private int Type
        {
            get
            {
                return Convert.ToInt32(this.ViewState["Type"]);
            }
            set
            {
                this.ViewState["Type"] = value;
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
                    String.Format("{{0:n{0}}}", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)
                    : (string)ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] = value;
            }
        }

        /// <summary>
        /// Current Depreciation Transaction Object
        /// </summary>
        private AssetDisposalBO SelectedDisposal
        {
            get
            {
                return (AssetDisposalBO)ViewState["SelectedDisposal"];
            }
            set
            {
                ViewState["SelectedDisposal"] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex] ?? "1";
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
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To maintain the PageSize in viewstate
        /// </summary>
        private int PageSize
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.PageSize] ?? Convert.ToInt32(GetLocalResourceObject("PageSize").ToString()));
            }
            set
            {
                this.ViewState[ViewstateStrings.PageSize] = value;
            }
        }

        /// <summary>
        /// To maintain the SortExpression or Sort By in viewstate
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
        /// To maintain the SortExpression or Then By in viewstate
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
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageProcessID] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.PageProcessID]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PageProcessID] = value;
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


        private int PlantPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PlantPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PlantPK] = value;
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
        /// Expense
        /// </summary>
        private string Expense
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.Expense];
            }
            set
            {
                this.ViewState[ViewstateStrings.Expense] = value;
            }
        }

        /// <summary>
        /// T identify whether the Depreciation is cancelled or not
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

        User currentUser;
        DataTable dtPageData;
        DataTable dtCompany;
        DataTable dtAssetStatus;
        bool assetAddedToList = false;

        private List<ADM_COMPANY_MST> admCompanyMstList;
        private string refID;
        private string inboxFlag;
        private int JournalPK;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        #endregion

        #endregion

        #region Page Events

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        /// <summary>
        /// To Handle Page_Init
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
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnGetAsset.PreRender += new EventHandler(btnAction_PreRender);
            //btnListPrint.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);

            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);

            btnNew.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
            btnGetAsset.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            //btnListPrint.Load += new EventHandler(btnAction_Load);
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

                if (Request.QueryString["Type"] != null)
                {
                    Type = Convert.ToInt32(Request.QueryString["Type"]);
                    hdfDspType.Value = Request.QueryString["Type"];
                }

                switch (Type)
                {
                    //set breadcrum based on type
                    case (int)DepreciationTypeEnum.IFRS:
                        break;
                    case (int)DepreciationTypeEnum.Revenue:
                        lblBreadCrum.Text = GetGlobalResourceObject("Controls", "Depreciation").ToString();
                        break;
                }

                if (IsDeleted)
                {
                    btnSave.Visible = false;
                    btnEditforCancel.Visible = false;
                    btnEdit.Visible = false;
                    btnListPrint.Visible = false;
                    btnPrint.Visible = false;
                }
                else
                {
                    btnListPrint.Visible = true;
                }


                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Grid Events & Helper Methods

        #region RowDataBound Event

        /// <summary>
        /// Row Data Binding event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdDepreTransactionList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

                        if (Type == 2)
                        {
                            imgPosted.Visible = false;
                        }

                        //if (Convert.ToBoolean(Convert.ToInt32(hdfPosted.Value)) == true)
                        //{
                        //    //imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                        //    //imgPosted.ToolTip = Resources.Captions.Posted;

                        //    imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.Posted;
                        //}
                        //else
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.NotPosted;
                        //}
                    }
                }
                else if (((GridView)sender).ID == "grdAssetList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        string Status = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "IS_Extended"));

                        if (Status == "1")
                        {
                            e.Row.Attributes["style"] = "background-color: #F9DEE5";
                        }
                    }

                    if (grdAssetList.Rows.Count > 0)
                    {
                        lblCountAssets.Text = "Asset Count :" + Convert.ToString(grdAssetList.Rows.Count);
                    }
                }
                else if (((GridView)sender).ID == "grdSelectedAssets")
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalAssetDepreAmtFooter = (Label)e.Row.FindControl("lblTotalAssetDepreAmtFooter");
                        if (lblTotalAssetDepreAmtFooter != null)
                        {
                            if (this.SelectedDisposal.Details != null && this.SelectedDisposal.Details.Any())
                            {
                                string _curFormat = "N" + hdfDecimalDigits.Value.Trim();
                                //lblTotalAssetDepreAmtFooter.Text = this.SelectedDepreciation.Details.Sum(x => x.FDD_AMOUNT).ToString(_curFormat);
                                //lblTotalAssetDepreAmtFooter.ToolTip = this.SelectedDepreciation.Details.Sum(x => x.FDD_AMOUNT).ToString(_curFormat);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "CalculateTotal();", true);
                            }
                            else
                            {
                                lblTotalAssetDepreAmtFooter.Text = string.Empty;
                                lblTotalAssetDepreAmtFooter.ToolTip = null;
                            }
                        }
                    }

                    if (grdSelectedAssets.Rows.Count > 0)
                    {
                        lblSelectAsset.Text = "Selected Asset Count :" + Convert.ToString(grdSelectedAssets.Rows.Count);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }

                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlEnums.DEPRETRANLIST);
                SetFieldValues(ControlEnums.DEPRETRANLIST);
                EntryStatus = EntryStatus.LISTMODE;
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }
        #endregion

        #region OnPageIndexChanging Event
        /// <summary>
        /// Page Index Handler for grd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            EntryStatus = EntryStatus.LISTMODE;
        }
        #endregion

        #region OnSorting Event
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;

                EntryStatus = EntryStatus.LISTMODE;
                this.CurrPK = 0;
                GetFieldValues(ControlEnums.DEPRETRANLIST);
                SetFieldValues(ControlEnums.DEPRETRANLIST);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #endregion

        #region Common Methods

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        #region InitializeComponent
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

        #region PageActionHandler
        private void PageActionHandler()
        {
            string prefID;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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
                    if (Request.QueryString["Type"] != null)
                    {
                        Type = Convert.ToInt32(Request.QueryString["Type"]);
                        hdfDspType.Value = Request.QueryString["Type"];
                    }

                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    this.CurrencyFormatString = String.Format("{{0:n{0}}}", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    txtFromDate.Text = DateTime.Now.StartDateOfTheMonth().ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.EndDateOfTheMonth().ToString(Resources.Constants.DateFormatShort);
                    txtDepreMonth.Text = DateTime.Now.ToString(Resources.Constants.DateFormatMonthYear);

                    if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "DepreciationDateWise")) == false)
                    {
                        divDate.Visible = false;
                        divMonth.Visible = true;
                    }
                    else
                    {
                        divDate.Visible = true;
                        divMonth.Visible = false;
                    }

                    GetFieldValues(ControlEnums.COMPANY);
                    SetFieldValues(ControlEnums.COMPANY);

                    GetFieldValues(ControlEnums.ASSETTYPE);
                    SetFieldValues(ControlEnums.ASSETTYPE);

                    GetFieldValues(ControlEnums.LOCATION);
                    SetFieldValues(ControlEnums.LOCATION);

                    GetFieldValues(ControlEnums.PLANT);
                    SetFieldValues(ControlEnums.PLANT); 

                    GetFieldValues(ControlEnums.ASSETSTATUS);
                    SetFieldValues(ControlEnums.ASSETSTATUS);


                    PageIndex = CommonConstants.SELECT_VALUE_ONE;

                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    FillProcessID(1);
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlEnums.DEPREGET);
                        SetFieldValues(ControlEnums.DEPREGET);
                    }
                    else
                    {
                        #region else Region
                        if (!string.IsNullOrEmpty(refID))
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
                            ////start
                            if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                base.WkfRefID = ucrWrkf.RefID;
                                CurrPK = GetApplicationID(ucrWrkf.RefID);
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlEnums.GETEXPENSEPKBYJOURNALPK);
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
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }

                            GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                            SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        }
                        else
                        {
                            txtDepreDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            GetFieldValues(ControlEnums.DEPRETRANLIST);
                            SetFieldValues(ControlEnums.DEPRETRANLIST);
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
        }
        #endregion

        #region Work flow events
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
            if (Approved == 0)
            {
                btnJournalize.Visible = false;
            }
            else
            {
                btnJournalize.Visible = true;
            }
            if (Type == 2)
            {
                btnJournalize.Visible = false;
            }
            else
            {
                btnJournalize.Visible = true;
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
                int retRefID;
                string transNo = string.Empty;
                bool bIsChecked = false;
                int? result = null;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                CommonService CommonServiceClient;
                CommonServiceClient = null;
                string action;

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
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                #endregion

                switch (commonActions)
                {
                    #region List
                    case ActionsEnum.SEARCH:
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        this.PageIndex = "1";
                        this.EntryStatus = BusinessObject.Common.EntryStatus.LISTMODE;
                        this.SelectedDisposal = null;
                        this.CurrPK = 0;
                        this.ModifiedDatePnl.Visible = false;
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                    #endregion
                    #region Item Selected
                    case ActionsEnum.ITEMSELECTED:
                        IsDeleted = false;
                        foreach (GridViewRow grdrow in grdDepreTransactionList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            HiddenField hdfDelStatus;
                            int dept;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;// by Biju
                                this.CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDePreTranID")).Value);

                                #region workflow code
                                hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(CurrPK, PageProcessID);
                                if (Convert.ToInt32(hdfDelStatus.Value) == 1)
                                {
                                    //btnSave.Visible = false;
                                    //btnEditforCancel.Visible = false;
                                    //btnEdit.Visible = false;
                                    //btnListPrint.Visible = false;
                                    //btnPrint.Visible = false;
                                    IsDeleted = true;
                                }
                                //else
                                //{
                                //    btnEditforCancel.Visible = true;
                                //    btnEdit.Visible = true;
                                //    btnListPrint.Visible = true;
                                //    btnPrint.Visible = true;
                                //}
                                #endregion
                            }
                        }
                        if (!bIsChecked)
                        {
                            throw new ApplicationException("Items not selected");
                        }
                        break;
                    #endregion
                    #region Detail & Edit Button
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdDepreTransactionList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDePreTranID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = ((HiddenField)grdrow.FindControl("hdfPosted")).Value == "1" ? true : false;
                                ////
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore1 = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore1.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            /*
                             * Post Button Visibility is handled in btn-Prerender Event
                             * */
                            //if (Approved == 0) btnJournalize.Visible = false;
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                            SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdDepreTransactionList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDePreTranID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = ((HiddenField)grdrow.FindControl("hdfPosted")).Value == "1" ? true : false;
                                ////
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
                                ucrWrkf.ViewAction();
                            }
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                            SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ActionsEnum.NEW);
                        this.EntryStatus = BusinessObject.Common.EntryStatus.NEWMODE;
                        this.CurrPK = 0;
                        this.SelectedDisposal = new AssetDisposalBO();
                        this.SelectedDisposal.DisposalPK = this.CurrPK;

                        #region Work flow Code
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        #endregion
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page,
                            typeof(Page),
                            "Openwindow",
                            "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.ASD) + "');",
                            true);
                        //GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        //SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTotal", "CalculateTotal();", true);
                        break;
                    case ActionsEnum.PRINTLISTING:
                        if (this.CurrPK == 0)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        ScriptManager.RegisterStartupScript(this.Page,
                                typeof(Page),
                                "Openwindow",
                                "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.ASD) + "');",
                                true);
                        break;
                    #endregion
                    #region Launch Popup For Getting Assets
                    case ActionsEnum.GETLIST:
                        DateTime _dummyDate;
                        if (!DateTime.TryParse(txtFromDateEntry.Text.Trim(), out _dummyDate) || !DateTime.TryParse(txtToDateEntry.Text.Trim(), out _dummyDate))
                        {
                            throw new ApplicationException("Select From Date and To Date");
                        }
                        ResetForm(ActionsEnum.GETLIST); // Reset Popup
                        if (this.SelectedDisposal.Details.Count != 0)
                        {
                            ddlPlant.SelectedIndex = ddlPlant.Items.IndexOf(ddlPlant.Items.FindByValue(PlantPK.ToString()));
                            // ddlPlant.SelectedValue =Convert.ToString(ddlPlant.Items.IndexOf(ddlPlant.Items.FindByValue(PlantPK.ToString())));
                            ddlPlant.Enabled = false;
                        }
                        else
                        {
                            ddlPlant.Enabled = true;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPckAssetPopupContainer]','" + Resources.Captions.SelectAssetsForDepreciation + "','900','500');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTotal", "CalculateTotal();", true);
                        break;
                    #endregion
                    #region Asset Search
                    case ActionsEnum.ASSETSEARCH:
                        GetFieldValues(ControlEnums.ASSETLIST);
                        SetFieldValues(ControlEnums.ASSETLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPckAssetPopupContainer]','" + Resources.Captions.SelectAssetsForDepreciation + "','900','500');", true);
                        break;
                    #endregion
                    #region Add To List
                    case ActionsEnum.ADDTOLIST:
                        if (SelectedDisposal.Details != null && SelectedDisposal.Details.Any())
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_CannotAddMultiple").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                            break;
                        }
                        GetFieldValues(ControlEnums.SELECTEDASSETLIST);
                        if (assetAddedToList)
                        {
                            if (txtFromDateEntry.Enabled || txtToDateEntry.Enabled)
                            {
                                txtFromDateEntry.Enabled = false;
                                txtToDateEntry.Enabled = false;
                            }

                            if (txtDepreMonth.Enabled)
                            {
                                txtDepreMonth.Enabled = false;
                            }
                            PlantPK = Convert.ToInt32(ddlPlant.SelectedValue);
                            SetFieldValues(ControlEnums.SELECTEDASSETLIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Msg_AssetAddedToList").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPckAssetPopupContainer]','" + Resources.Captions.SelectAssetsForDepreciation + "','900','500');", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (this.SelectedDisposal.Details == null || this.SelectedDisposal.Details.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            AssetDisposalBO disposalTran = (AssetDisposalBO)SetUIValuesToObject(ControlEnums.SELECTEDDEPRECIATION);
                            disposalTran.WKF_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                            disposalTran.UserPk = currentUser.PKUser;
                            // save Process Control inspection details
                            string xmlDoc = CommonFunctions.XmlSerialize<AssetDisposalBO>(disposalTran);
                            result = DepreciationBL.SaveAssetDisposalWkf(xmlDoc, out retRefID, out transNo);
                            if (result > 0)
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetDisposal);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ActionsEnum.SAVE);
                                GetFieldValues(ControlEnums.DEPRETRANLIST);
                                SetFieldValues(ControlEnums.DEPRETRANLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ActionsEnum.SAVE);
                                    GetFieldValues(ControlEnums.DEPRETRANLIST);
                                    SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("DisposalAmountExceeded").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetDisposal);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Save & Submit
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTotal", "CalculateTotal();", true);
                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        //GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        //SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTotal", "CalculateTotal();", true);
                        break;
                    #endregion
                    #region Workflow Submit
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (this.SelectedDisposal.Details == null || this.SelectedDisposal.Details.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            #region new
                            ucrWrkf.ApplicationID = 0;
                            AssetDisposalBO depreciationTran = (AssetDisposalBO)SetUIValuesToObject(ControlEnums.SELECTEDDEPRECIATION);
                            depreciationTran.WKF_FLAG = 1;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                SaveTransaction(depreciationTran, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT), sender);
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL) //DELETESUBMIT
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.ASD))
                                {
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                                }
                                else
                                {
                                    ResetForm(ActionsEnum.SAVE);
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlEnums.DEPRETRANLIST);
                                    SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                            #endregion
                        }
                        break;
                    #endregion
                    #region Edit For Cancel
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdDepreTransactionList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDePreTranID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = ((HiddenField)grdrow.FindControl("hdfPosted")).Value == "1" ? true : false;
                                ////
                                break;
                            }
                        }
                        if (bIsChecked)
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
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                            SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Delete Submit Popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        this.EntryStatus = BusinessObject.Common.EntryStatus.LISTMODE;
                        ResetForm(ActionsEnum.LIST);
                        ResetForm(ActionsEnum.CLEAR);
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = DepreciationBL.Delete(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetDisposal);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ActionsEnum.SAVE);
                                GetFieldValues(ControlEnums.DEPRETRANLIST);
                                SetFieldValues(ControlEnums.DEPRETRANLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ActionsEnum.SAVE);
                                    GetFieldValues(ControlEnums.DEPRETRANLIST);
                                    SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ActionsEnum.SAVE);
                                    GetFieldValues(ControlEnums.DEPRETRANLIST);
                                    SetFieldValues(ControlEnums.DEPRETRANLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetDisposal);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Remove Item
                    case ActionsEnum.REMOVEITEM:
                        if (grdSelectedAssets.Rows.Count == 1 && this.CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            break;
                        }
                        int _selectedPK = 0;
                        HiddenField hdf = (HiddenField)((Button)sender).Parent.Parent.FindControl("hdfDepreDetailPK");

                        if (hdf != null)
                        {
                            _selectedPK = Convert.ToInt32(hdf.Value.Trim());
                            if (_selectedPK != 0)
                            {
                                result = 0;
                                result = DepreciationBL.DeleteDetails(_selectedPK, LastModifiedTime);
                                if (result > 0) // Success ! re-initialize the page
                                {
                                    //Show Save success message and reset Contract Entry
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Details_Removed_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Asset);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                                    SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                                    if (grdSelectedAssets.Rows.Count == 0)
                                    {
                                        txtFromDateEntry.Enabled = true;
                                        txtToDateEntry.Enabled = true;
                                        txtDepreMonth.Enabled = true;
                                    }
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
                                        litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbDeleteStatus.REFERRED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.UsedInAnotherPlace;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.AlreadyDeleted;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Asset);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                            else
                            {
                                _selectedPK = Convert.ToInt32(((HiddenField)((Button)sender).Parent.Parent.FindControl("hdfAssetPK")).Value.Trim());
                                AssetDisposalDetails det = this.SelectedDisposal.Details.Single(x => x.AssetPK == _selectedPK);
                                if (this.SelectedDisposal.Details.Remove(det))
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Details_Removed_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Asset);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    SetFieldValues(ControlEnums.SELECTEDASSETLIST);
                                    if (grdSelectedAssets.Rows.Count == 0)
                                    {
                                        txtFromDateEntry.Enabled = true;
                                        txtToDateEntry.Enabled = true;
                                        txtDepreMonth.Enabled = true;
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Asset);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                        SetUIValuesToObject(ControlEnums.JOURNALIZE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowTotal", "CalculateTotal();", true);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrJournalize.ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ActionsEnum.LIST);
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ActionsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ActionsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ActionsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ActionsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                        break;
                        #endregion
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            int PType;
            try
            {
                switch (type)
                {
                    #region Depre Tran List
                    case ControlEnums.DEPRETRANLIST:
                        string _pageUrl = Resources.PageURL.Depreciation.Replace("~", "");
                        int DeprPk = String.IsNullOrEmpty(hdfDeprPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfDeprPK.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int Plant = Convert.ToInt32(ddlPlantList.SelectedValue);

                        if (Type == 2)
                        {
                            PType = 2;
                        }
                        else
                        {
                            PType = 1;
                        }

                        dtPageData = DepreciationBL.GetAssetDisposalList(new BusinessObject.GridPrams()
                        {
                            SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.AssetDisposalPK : SortBy,
                            SortDirection = string.IsNullOrEmpty(SortDirection)
                                                    ? Resources.Report.SortDescending
                                                    : SortDirection,
                            ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.AssetDisposalNo
                                                    ? string.Empty
                                                    : string.IsNullOrEmpty(ThenBy)
                                                        ? Resources.DataFieldRes.AssetDisposalNo
                                                        : ThenBy,
                            ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.AssetDisposalNo
                                                    ? string.Empty
                                                    : string.IsNullOrEmpty(ThenDirection)
                                                        ? Resources.Report.SortDescending
                                                        : ThenDirection,
                            FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim())
                                                    ? string.Empty
                                                    : txtFromDate.Text.Trim(),
                            ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim())
                                                    ? string.Empty
                                                    : txtToDate.Text.Trim(),
                            SearchBy = Resources.DataFieldRes.AssetDisposalNo,
                            SearchValue = string.IsNullOrEmpty(txtDepNo.Text.Trim()) ? string.Empty : (txtDepNo.Text.Trim() == "Select/Type" ? string.Empty : txtDepNo.Text.Trim()),
                            PageNumber = Convert.ToInt32(this.PageIndex),
                            PageSize = this.PageSize
                        }, currentUser.PKUser, currentUser.SBUID, _pageUrl, Plant, Status);
                        break;
                    #endregion
                    #region DEPREGET
                    case ControlEnums.DEPREGET:
                        string _pgeUrl = Resources.PageURL.Depreciation.Replace("~", "");

                        int GDeprePk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        dtPageData = DepreciationBL.GetAssetDisposalList(new BusinessObject.GridPrams()
                        {
                            SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.AssetDisposalDate : SortBy,
                            SortDirection = string.IsNullOrEmpty(SortDirection)
                                                    ? Resources.Report.SortDescending
                                                    : SortDirection,
                            ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.AssetDisposalNo
                                                    ? string.Empty
                                                    : string.IsNullOrEmpty(ThenBy)
                                                        ? Resources.DataFieldRes.AssetDisposalNo
                                                        : ThenBy,
                            ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.AssetDisposalNo
                                                    ? string.Empty
                                                    : string.IsNullOrEmpty(ThenDirection)
                                                        ? Resources.Report.SortDescending
                                                        : ThenDirection,
                            FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim())
                                                    ? string.Empty
                                                    : txtFromDate.Text.Trim(),
                            ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim())
                                                    ? string.Empty
                                                    : txtToDate.Text.Trim(),
                            SearchBy = Resources.DataFieldRes.DepreTranPK,
                            SearchValue = GDeprePk.ToString(),
                            PageNumber = 1,
                            PageSize = this.PageSize
                        }, currentUser.PKUser, currentUser.SBUID, _pgeUrl);
                        break;
                    #endregion
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
                    #region Category
                    case ControlEnums.CATEGORY:
                        dtPageData = DepreciationBL.GetCategory(null, (short)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion
                    #region AssetType
                    case ControlEnums.ASSETTYPE:
                        dtPageData = DepreciationBL.GetAssetType(null, (short)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion
                    #region Location
                    case ControlEnums.LOCATION:
                        dtPageData = DepreciationBL.GetLocations(null, (short)DbActiveStatus.ACTIVE, currentUser.SBUID);
                        break;
                    #endregion
                    #region Plant
                    case ControlEnums.PLANT:
                        string splCond = "prd";
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0, splCond);
                        break;
                    #endregion
                    #region SELECTED DEPRECIATION
                    case ControlEnums.SELECTEDDEPRECIATION:
                        this.SelectedDisposal = DepreciationBL.GetDisposalDetails(this.CurrPK);
                        break;
                    #endregion
                    #region ASSETLIST
                    case ControlEnums.ASSETLIST:
                        int assetCategory = Convert.ToInt32(CommonConstants.SELECTVAL); // Asset Category is Removed From UI
                        string AssetCode = txtPopUpAssetCode.Text.Trim();
                        string AssetName = txtPopUpAssetName.Text.Trim();
                        int assetType = Convert.ToInt32(ddlAssetType.SelectedValue);
                        int plant = Convert.ToInt32(ddlPlant.SelectedValue);
                        int CfgType;
                        if (Type == 2)
                        {
                            PType = 2;
                        }
                        else
                        {
                            PType = 1;
                        }
                        DateTime FromDate;
                        DateTime.TryParse(txtFromDateEntry.Text.Trim(), out FromDate);
                        DateTime ToDate;
                        DateTime.TryParse(txtToDateEntry.Text.Trim(), out ToDate);

                        int Location = Convert.ToInt32(ddlLocation.SelectedValue);

                        DateTime PurchaseDateFrom;
                        DateTime PurchaseDateTo;

                        DateTime.TryParse(txtPopUpPurchaseDateFrom.Text.Trim(), out PurchaseDateFrom);
                        DateTime.TryParse(txtPopUpPurchaseDateTo.Text.Trim(), out PurchaseDateTo);

                        DateTime DepreMonth;
                        DateTime.TryParse(txtDepreMonth.Text.Trim(), out DepreMonth);

                        if (Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "DepreciationDateWise")) == false)
                        {
                            CfgType = 1;
                        }

                        else
                        {
                            CfgType = 0;
                        }

                        dtPageData = DepreciationBL.GetAssetListForDisposal(new AssetFilterForDepreParameterBinder
                        {
                            AssetCategory = assetCategory,
                            AssetType = assetType,
                            AssetCode = AssetCode,
                            AssetName = AssetName,
                            FromDate = FromDate,
                            Location = Location,
                            PurchaseDateFrom = PurchaseDateFrom,
                            PurchaseDateTo = PurchaseDateTo,
                            ToDate = ToDate,
                            PType = PType,
                            Plant = plant,
                            DepreMonth = DepreMonth,
                            CfgType = CfgType,
                        });
                        break;
                    #endregion
                    #region SELECTEDASSETLIST
                    case ControlEnums.SELECTEDASSETLIST:
                        assetAddedToList = false;
                        if (this.SelectedDisposal != null)
                        {
                            foreach (GridViewRow row in grdAssetList.Rows)
                            {
                                //CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                                RadioButton chk = (RadioButton)row.FindControl("rbtSelectAsset");
                                if (chk != null && chk.Checked)
                                {
                                    HiddenField hdf = (HiddenField)row.FindControl("hdfAssetPK");
                                    if (hdf != null)
                                    {
                                        #region Getting Values From Grid
                                        int detailPk = 0;
                                        int assetPk = Convert.ToInt32(hdf.Value);
                                        string assetCode = ((Label)row.FindControl("lblAssetCode")).ToolTip;
                                        string assetName = ((Label)row.FindControl("lblAssetName")).ToolTip;
                                        decimal purchaseCost;
                                        Decimal.TryParse(((HiddenField)row.FindControl("hdfPurchaseCost")).Value.Trim(), out purchaseCost);
                                        decimal landCost;
                                        Decimal.TryParse(((HiddenField)row.FindControl("hdfLandCost")).Value.Trim(), out landCost);
                                        string purchaseCurrency = string.Empty;
                                        string currencyText = string.Empty;

                                        DateTime dateofPurchase;
                                        DateTime.TryParse(((Label)row.FindControl("lblAssetPurchaseDate")).Text.Trim(), out dateofPurchase);

                                        double deprePercentage;
                                        Double.TryParse(((Label)row.FindControl("lblCostDeprePercentage")).Text.Trim(), out deprePercentage);

                                        string assetTypeID = ((HiddenField)row.FindControl("hdfAssetType")).Value;
                                        string assetTypeText = ((Label)row.FindControl("lblAssetTypeText")).ToolTip;

                                        decimal balanceDepreAmt = 0;
                                        int active = (int)DbActiveStatus.ACTIVE;
                                        decimal depreAmt;
                                        Decimal.TryParse(((HiddenField)row.FindControl("hdfAssetDepreAmt")).Value.Trim(), out depreAmt);
                                        #endregion

                                        if (!this.SelectedDisposal.Details.Any(x => x.AssetPK == assetPk)) // && depreAmt > 0)
                                        {
                                            this.SelectedDisposal.Details.Add(new AssetDisposalDetails
                                            {
                                                DetailPK = detailPk, // New Entry
                                                AssetPK = assetPk,
                                                amiCostLand = landCost,
                                                amiCostPur = purchaseCost,
                                                amiCurrPur = purchaseCurrency,
                                                amiCurrPur_Text = currencyText,
                                                amiDatePur = dateofPurchase,
                                                amiDeprPerc = deprePercentage,
                                                //asrCategory = assetCategoryID,
                                                //asrCategory_Text = assetCategoryText,
                                                asrType = assetTypeID,
                                                asrType_text = assetTypeText,
                                                asrCode = assetCode,
                                                asrName = assetName,
                                                DetailActive = active,
                                                Amount = depreAmt,
                                                DisposalPK = this.SelectedDisposal.DisposalPK,
                                                BalanceDeprAmt = balanceDepreAmt
                                            });
                                        }
                                        assetAddedToList = true;
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region FIN HEADER
                    case ControlEnums.FINHEADER:
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
                    #region GETEXPENSEPKBYJOURNALPK
                    case ControlEnums.GETEXPENSEPKBYJOURNALPK:
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
                    #region GETASSETSTATUS
                    case ControlEnums.ASSETSTATUS:
                        string splCondition = "xrcSplCond";
                        string searchBy = "Asset";
                        dtAssetStatus = DepreciationBL.GetAssetStatus(searchBy, splCondition);
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
                finTrxServiceClient = null;
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
                    case ControlEnums.DEPRETRANLIST:
                        BindGrid(ControlEnums.DEPRETRANLIST);
                        break;

                    case ControlEnums.PLANT:
                        BindDropDown(ControlEnums.PLANT);
                        break;

                    case ControlEnums.COMPANY:
                        BindDropDown(ControlEnums.COMPANY);
                        break;
                    case ControlEnums.CATEGORY:
                        BindDropDown(ControlEnums.CATEGORY);
                        break;
                    case ControlEnums.ASSETTYPE:
                        BindDropDown(ControlEnums.ASSETTYPE);
                        break;
                    case ControlEnums.LOCATION:
                        BindDropDown(ControlEnums.LOCATION);
                        break;
                    case ControlEnums.SELECTEDASSETLIST:
                        BindGrid(ControlEnums.SELECTEDASSETLIST);
                        break;
                    case ControlEnums.ASSETSTATUS:
                        BindDropDown(ControlEnums.ASSETSTATUS);
                        break;
                        
                    case ControlEnums.SELECTEDDEPRECIATION:
                        if (this.SelectedDisposal == null)
                        {
                            ResetForm(ActionsEnum.LIST);

                            GetFieldValues(ControlEnums.DEPRETRANLIST);
                            SetFieldValues(ControlEnums.DEPRETRANLIST);

                            this.EntryStatus = BusinessObject.Common.EntryStatus.LISTMODE;

                            string errMsgTempale = GetLocalResourceObject("Err_Msg_DepreciationTranNotFound").ToString();
                            throw new ApplicationException(string.Format(errMsgTempale, Resources.PageNameRes.AssetDisposal));
                        }
                        GetUIValuesFromObject(ControlEnums.SELECTEDDEPRECIATION);
                        SetFieldValues(ControlEnums.SELECTEDASSETLIST);
                        break;
                    case ControlEnums.ASSETLIST:
                        BindGrid(ControlEnums.ASSETLIST);
                        break;
                    case ControlEnums.DEPREGET:
                        GetUIValuesFromObject(ControlEnums.DEPREGET);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ActionsEnum controlType)
        {
            switch (controlType)
            {
                case ActionsEnum.NEW:
                    this.CurrPK = 0;
                    txtDepreDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    txtFromDateEntry.Text = DateTime.Now.StartDateOfTheMonth().ToString(Resources.Constants.DateFormatShort);
                    txtToDateEntry.Text = DateTime.Now.EndDateOfTheMonth().ToString(Resources.Constants.DateFormatShort);
                    txtDepreMonth.Text = DateTime.Now.ToString(Resources.Constants.DateFormatMonthYear);
                    txtDescription.Text = String.Empty;
                    lblDepreciationNo.Text = hdfDepreciationNo.Value == string.Empty ? "[NEW]" : hdfDepreciationNo.Value;
                    txtFromDateEntry.Enabled = true;
                    txtToDateEntry.Enabled = true;
                    txtDepreMonth.Enabled = true;
                    lblCountAssets.Text = String.Empty;
                    lblSelectAsset.Text = String.Empty;
                    grdSelectedAssets.DataSource = null;
                    grdSelectedAssets.DataBind();

                    this.SelectedDisposal = null;
                    ModifiedDatePnl.Visible = false;
                    hdfIsCancelled.Value = "0";
                    break;
                ///Reset Select Assets List Popup
                case ActionsEnum.GETLIST:
                    //ddlCategory.SelectedIndex = 0;
                    ddlAssetType.SelectedIndex = 0;
                    ddlLocation.SelectedIndex = 0;
                    ddlPlant.SelectedIndex = 0;
                    ddlPlantList.SelectedIndex = 0;

                    txtPopUpAssetCode.Text = string.Empty;
                    txtPopUpAssetName.Text = string.Empty;

                    lblCountAssets.Text = String.Empty;
                    // lblSelectAsset.Text = String.Empty;

                    txtPopUpPurchaseDateFrom.Text = string.Empty;
                    txtPopUpPurchaseDateTo.Text = string.Empty;
                    lblDepreciationNo.Text = hdfDepreciationNo.Value == string.Empty ? "[NEW]" : hdfDepreciationNo.Value;
                    hdfDeprPK.Value = string.Empty;
                    grdAssetList.DataSource = null;
                    grdAssetList.DataBind();
                    break;
                case ActionsEnum.LIST:
                case ActionsEnum.SAVE:
                    txtFromDate.Text = DateTime.Now.StartDateOfTheMonth().ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.EndDateOfTheMonth().ToString(Resources.Constants.DateFormatShort);
                    this.SelectedDisposal = null;
                    this.CurrPK = 0;
                    foreach (GridViewRow grdrow in grdDepreTransactionList.Rows)
                    {
                        RadioButton rbtn;
                        rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        if (rbtn.Checked)
                        {
                            rbtn.Checked = false;
                            break;
                        }
                    }
                    ModifiedDatePnl.Visible = false;
                    lblCountAssets.Text = String.Empty;
                    lblSelectAsset.Text = String.Empty;
                    break;
                case ActionsEnum.CLEAR:
                    ddlStatus.SelectedIndex = 0;
                    ddlPlantList.SelectedIndex = 0;
                    lblCountAssets.Text = String.Empty;
                    lblSelectAsset.Text = String.Empty;
                    txtDepNo.Text = "Select/Type";
                    break;
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
                    #region SELECTEDDEPRECIATION
                    case ControlEnums.SELECTEDDEPRECIATION:
                        if (this.SelectedDisposal == null)
                        {
                            throw new ApplicationException("Transaction not found");
                        }
                        lblDepreciationNo.Text = this.SelectedDisposal.DisposalNo == string.Empty ? "[NEW]" : this.SelectedDisposal.DisposalNo;
                        txtDepreDate.Text = this.SelectedDisposal.DisposalDate.ToString(Resources.Constants.DateFormatShort);
                        txtFromDateEntry.Text = this.SelectedDisposal.FromDate.ToString(Resources.Constants.DateFormatShort);
                        txtToDateEntry.Text = this.SelectedDisposal.ToDate.ToString(Resources.Constants.DateFormatShort);
                        txtDepreMonth.Text = this.SelectedDisposal.MonthYear.ToString(Resources.Constants.DateFormatMonthYear);
                        txtDescription.Text = this.SelectedDisposal.Description.IsNullOrEmptyOrWhitespace()
                                                ? String.Empty
                                                : this.SelectedDisposal.Description.Trim();
                        this.LastModifiedTime = this.SelectedDisposal.LastModDate;
                        Approved = Convert.ToInt32(this.SelectedDisposal.Status);
                        Posted = Convert.ToBoolean(this.SelectedDisposal.HasJournalEntry);
                        txtToDateEntry.Enabled = false;
                        txtFromDateEntry.Enabled = false;
                        txtDepreMonth.Enabled = false;
                        ModifiedDatePnl.Visible = true;
                        lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        break;
                    #endregion
                    #region DEPREGET
                    case ControlEnums.DEPREGET:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.DepreTranPK]);
                            Approved = Convert.ToInt32(dtPageData.Rows[0][Resources.DataFieldRes.DepreApproved]);
                            Posted = Convert.ToString(dtPageData.Rows[0][Resources.DataFieldRes.DeprePosted]) == "1" ? true : false;
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
                                ucrWrkf.ViewAction();
                            }
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
                            SetFieldValues(ControlEnums.SELECTEDDEPRECIATION);
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

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlEnums controlType)
        {
            Object retObject;
            retObject = null;

            bool bIsChecked = false;

            try
            {
                switch (controlType)
                {
                    #region SELECTEDDEPRECIATION
                    case ControlEnums.SELECTEDDEPRECIATION:
                        if (this.SelectedDisposal == null)
                        {
                            this.SelectedDisposal = new AssetDisposalBO();
                        }

                        #region Server Validations
                        DateTime _dummyDate;

                        if (!DateTime.TryParse(txtDepreDate.Text.Trim(), out _dummyDate))
                        {
                            throw new ApplicationException("Invalid From Transaction Date");
                        }
                        if (!DateTime.TryParse(txtFromDateEntry.Text.Trim(), out _dummyDate))
                        {
                            throw new ApplicationException("Invalid From Date");
                        }
                        if (!DateTime.TryParse(txtToDateEntry.Text.Trim(), out _dummyDate))
                        {
                            throw new ApplicationException("Invalid To Date");
                        }

                        if (!DateTime.TryParse(txtDepreMonth.Text.Trim(), out _dummyDate))
                        {
                            throw new ApplicationException("Invalid To Depre. Month");
                        }
                        #endregion

                        this.SelectedDisposal.DisposalNo = lblDepreciationNo.Text == "[NEW]" ? string.Empty : lblDepreciationNo.Text;
                        this.SelectedDisposal.DisposalDate = Convert.ToDateTime(txtDepreDate.Text.Trim());
                        this.SelectedDisposal.FromDate = Convert.ToDateTime(txtFromDateEntry.Text.Trim());
                        this.SelectedDisposal.ToDate = Convert.ToDateTime(txtToDateEntry.Text.Trim());
                        this.SelectedDisposal.MonthYear = Convert.ToDateTime(txtDepreMonth.Text.Trim());
                        this.SelectedDisposal.Description = txtDescription.Text.Trim();
                        //this.SelectedDisposal.ADH_TYPE = Convert.ToInt16(ddlAssetStatus.SelectedValue);
                        this.SelectedDisposal.TrxType = Convert.ToInt16(ddlAssetStatus.SelectedValue);
                        if (this.CurrPK == 0)
                            this.SelectedDisposal.LastModDate = DateTime.Now;
                        else
                            this.SelectedDisposal.LastModDate = this.LastModifiedTime;

                        //if (Type == 2)
                        //    this.SelectedDisposal.TrxType = 2;
                        //else
                        //    this.SelectedDisposal.TrxType = 1;

                        this.SelectedDisposal.DeptPK = currentUser.CurrentDeptPK;
                        this.SelectedDisposal.BizUnit = currentUser.SBUID;
                        this.SelectedDisposal.Active = ((int)DbActiveStatus.ACTIVE);
                        this.SelectedDisposal.UserPk = currentUser.PKUser;

                        this.SelectedDisposal.TrxCurrencyPK = this.SelectedDisposal.BaseCurrencyPK = currentUser.BaseCurrency <= 0
                                                ? 1 : currentUser.BaseCurrency;
                        this.SelectedDisposal.CompanyPK = ddlPlant.SelectedValue;

                        SetDepreciationAmount();

                        this.SelectedDisposal.AmountBC = this.SelectedDisposal.AmountTC = SelectedDisposal
                                                .Details
                                                .Sum(x => x.Amount);
                        this.SelectedDisposal.ExchgRate = 1;

                        #region Validate Depreciation Amount
                        if (this.SelectedDisposal.Details.Any(x => x.Amount <= 0))
                        {
                            //throw new ApplicationException("Depreciation Amount must be greater than 0");
                        }
                        #endregion

                        #region Check Duplicate
                        if (this.SelectedDisposal.Details.GroupBy(x => x.AssetPK).Any(g => g.Count() > 1))
                        {
                            throw new ApplicationException("Duplicate Items found");
                        }
                        #endregion

                        retObject = this.SelectedDisposal;
                        break;
                    #endregion
                    #region JOURNALIZE
                    case ControlEnums.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////
                            foreach (GridViewRow grdrow in grdDepreTransactionList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDePreTranID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                    break;
                                }
                            }
                            ////Start
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }
                        ////
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                GetFieldValues(ControlEnums.SELECTEDDEPRECIATION);

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

                                Expense = ApplicationType.ASDJ;
                                ucrJournalize.TransactionType = Expense;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = Expense;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = this.SelectedDisposal.DisposalNo;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = this.SelectedDisposal.DisposalDate;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = this.SelectedDisposal.TrxCurrencyPK;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.ASDJ;
                                ucrWrkf.WrkfSubmit -= ActionHandler;
                                ucrWrkf.Reset();
                                ucrWrkf.ViewType = 1;
                                FillProcessID(2);
                                GetFieldValues(ControlEnums.FINHEADER);
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
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                }
                                ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;


                                Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;

                                ucrWrkf.ViewAction();

                                HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                                hdfExchangeRateJV.Value = "";

                                TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                                txtJournalExchangeRate.Text = "";

                                TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                                txtNarration.Text = "";

                                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                WrkfComments.Text = "";

                                Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                                hdfJournalizeWorkFlow.Value = "1";
                                ucrJournalize.CallUserControl();
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Purchase_Expense_Journal").ToString();

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                        #endregion
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
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
                case ControlEnums.DEPRETRANLIST:
                    int rowCount = 0;
                    if (dtPageData.Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dtPageData.Rows[0]["ROW_COUNT"].ToString());
                    }
                    if (this.dtPageData.Rows.Count > 0) this.TotalPages = Convert.ToInt32(dtPageData.AsEnumerable().FirstOrDefault().Field<long>("RowNum"));

                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                      (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                      (rowCount / this.PageSize) + 1;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                    grdDepreTransactionList.DataSource = dtPageData;
                    grdDepreTransactionList.DataBind();

                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                case ControlEnums.SELECTEDASSETLIST:
                    if (this.SelectedDisposal != null)
                    {
                        grdSelectedAssets.DataSource = this.SelectedDisposal.Details;
                    }
                    grdSelectedAssets.DataBind();  // For Empty Row Template
                    break;
                case ControlEnums.ASSETLIST:
                    grdAssetList.DataSource = dtPageData;
                    grdAssetList.DataBind();
                    break;
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
                case ControlEnums.CATEGORY:
                    //if (dtPageData != null && dtPageData.Rows.Count > 0)
                    //{
                    //    ddlCategory.Items.Clear();
                    //    ddlCategory.DataSource = dtPageData;
                    //    ddlCategory.DataTextField = Resources.DataFieldRes.xcaData;
                    //    ddlCategory.DataValueField = Resources.DataFieldRes.xcaPK;
                    //    ddlCategory.DataBind();
                    //    ddlCategory.Items.HtmlDecode();
                    //    ddlCategory.Items.Insert(0, new ListItem { Text = "Select", Value = CommonConstants.SELECTVAL });
                    //    ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.xcaPK].ToString()));
                    //}
                    break;

                case ControlEnums.ASSETTYPE:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlAssetType.Items.Clear();
                        ddlAssetType.DataSource = dtPageData;
                        ddlAssetType.DataTextField = Resources.DataFieldRes.atpName;
                        ddlAssetType.DataValueField = Resources.DataFieldRes.atpPK;
                        ddlAssetType.DataBind();
                        ddlAssetType.Items.HtmlDecode();
                        //ddlAssetType.Items.Insert(0, new ListItem { Text = "Select", Value = CommonConstants.SELECTVAL });
                        ddlAssetType.SelectedIndex = ddlAssetType.Items.IndexOf(ddlAssetType.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.atpPK].ToString()));
                    }
                    break;
                case ControlEnums.PLANT:
                    ddlPlant.Items.Clear();
                    ddlPlantList.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlPlant.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, Resources.DataFieldRes.CMP_NAME);
                        ddlPlant.DataSource = dtPageData;
                        ddlPlant.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_NAME;
                        ddlPlant.DataValueField = Resources.DataFieldRes.CMP_PK;
                        ddlPlant.DataBind();
                        ddlPlant.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.CMP_PK].ToString();

                        ddlPlantList.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, Resources.DataFieldRes.CMP_NAME);
                        ddlPlantList.DataSource = dtPageData;
                        ddlPlantList.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_NAME;
                        ddlPlantList.DataValueField = Resources.DataFieldRes.CMP_PK;
                        ddlPlantList.DataBind();
                        //  ddlPlantList.SelectedValue = dtPageData.Rows[0][Resources.DataFieldRes.CMP_PK].ToString();
                    }
                    ddlPlant.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlPlantList.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlEnums.LOCATION:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlLocation.Items.Clear();
                        ddlLocation.DataSource = dtPageData;
                        ddlLocation.DataTextField = Resources.DataFieldRes.locNameText;
                        ddlLocation.DataValueField = Resources.DataFieldRes.locPK;
                        ddlLocation.DataBind();
                        ddlLocation.Items.HtmlDecode();
                        ddlLocation.Items.Insert(0, new ListItem { Text = "Select", Value = CommonConstants.SELECTVAL });
                        ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.locPK].ToString()));
                    }
                    break;
                case ControlEnums.ASSETSTATUS:
                    ddlAssetStatus.Items.Clear();
                    if (dtAssetStatus != null)
                    {
                        ddlAssetStatus.DataSource = dtAssetStatus;
                        ddlAssetStatus.DataTextField = "xrcData";
                        ddlAssetStatus.DataValueField = "xrcPK";
                        ddlAssetStatus.DataBind();
                    }
                    //ddlAssetStatus.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
            }
        }
        #endregion

        #endregion

        #region Helper Methods
        private void SaveTransaction(AssetDisposalBO objDepreciation, int workflowFlag, object sender)
        {
            try
            {
                int retRefID;
                int? result = 0;
                string transNo = string.Empty;
                string savePath = string.Empty;
                WorkflowDetails wkfDetails = null;
                string TrxNo = string.Empty;
                string action = string.Empty;
                if (objDepreciation == null)
                    objDepreciation = new AssetDisposalBO();

                #region New workflow Submition
                wkfDetails = ucrWrkf.GetWorkflowDetails();
                objDepreciation.UserPk = wkfDetails.UserPK;
                objDepreciation.WKF_APPLICATION = CurrPK;
                objDepreciation.WKF_COMMENTS = wkfDetails.Comments;
                objDepreciation.WKF_TRX_FLAG = workflowFlag;
                objDepreciation.WKF_PROCESS = wkfDetails.ProcessID;
                objDepreciation.WKF_REFERENCE = wkfDetails.ReferenceID;
                objDepreciation.WKF_TASK = wkfDetails.TaskID;
                objDepreciation.WKF_TASK_ACTION = wkfDetails.ActionID;
                action = wkfDetails.ActionText;
                #endregion
                objDepreciation.APT_CODE = ApplicationType.ASD;

                //objDepreciation.CWIPDate = Convert.ToDateTime(txtCWIPDate.Text);
                string xmlDoc = CommonFunctions.XmlSerialize<AssetDisposalBO>(objDepreciation);
                result = DepreciationBL.SaveAssetDisposalWkf(xmlDoc, out retRefID, out transNo);
                if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                {
                    CurrPK = (int)result;
                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                        litErrorMsg.Text = string.Format(Resources.Messages.Msg_Cancelled_Success, Resources.PageNameRes.AssetDisposal);
                    else
                        litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_Submit_Success").ToString(), Resources.PageNameRes.AssetDisposal, transNo);

                    FillProcessID(1);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ActionsEnum.SAVE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm(ActionsEnum.SAVE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnums.DEPRETRANLIST);
                        SetFieldValues(ControlEnums.DEPRETRANLIST);
                    }
                    #endregion
                    ucrWrkf.ApplicationID = result.Value;
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
                        litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.EditUsedByAnotherUser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.CODEEXIST)
                    {
                        litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)
                    {
                        litErrorMsg.Text = Resources.PageNameRes.AssetDisposal + " " + GetLocalResourceObject("DisposalAmountExceeded").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetDisposal);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
        /// Method Used To Set Edited Depreciation Amount to the Selected Assets.
        /// </summary>
        private void SetDepreciationAmount()
        {
            if (this.SelectedDisposal != null & this.SelectedDisposal.Details.Any())
            {
                foreach (GridViewRow row in grdSelectedAssets.Rows)
                {
                    HiddenField hdf = (HiddenField)row.FindControl("hdfAssetPK");
                    if (hdf != null && hdf.Value.Trim() != string.Empty)
                    {
                        int assetPk = Convert.ToInt32(hdf.Value.Trim());
                        TextBox txt = (TextBox)row.FindControl("txtAssetDepreAmt");
                        Decimal amt;
                        Decimal.TryParse(txt.Text.Trim(), out amt);
                        this.SelectedDisposal.Details.SingleOrDefault(x => x.AssetPK == assetPk).Amount = amt;
                    }
                }
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

        #region WorkFlow Methods

        /// <summary>
        /// Method to  Get ApplicationID
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
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
            if (Type > 0)
                path += "&&Type=" + Type.ToString();

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1 || pid == 3 || pid == 11)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
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
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        /// <summary>
        /// Work flow Support method
        /// </summary>
        /// <returns></returns>
        private string GetUrl()
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            if (Type > 0)
                path += "&&Type=" + Type.ToString();
            return path;
        }

        #endregion

        #endregion

        #region Page Control Enum
        enum ControlEnums
        {
            DEPRETRANLIST,
            LOCATION,
            CATEGORY,
            ASSETLIST,
            SELECTEDASSETLIST,
            COMPANY,
            LIST,
            DETAIL,
            SELECTEDDEPRECIATION,
            EXCHANGERATE,
            JOURNALIZE,
            FINHEADER,
            GETEXPENSEPKBYJOURNALPK,
            ASSETTYPE,
            DEPREGET,
            PLANT,
            ASSETSTATUS
        }
        #endregion

        enum DepreciationTypeEnum
        {
            IFRS = 1,
            Revenue = 2
        }
    }
}