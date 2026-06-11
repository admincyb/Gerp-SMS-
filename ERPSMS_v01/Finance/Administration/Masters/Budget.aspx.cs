using BusinessLogic.Finance;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Finance.Administration.Masters
{
    public partial class Budget : ERP.Store.UI.WorkFlowBasePage
    {

        #region Properties & Variables

        #region Properties
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
        private int CurrDtlPk
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrDtlPk] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrDtlPk];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrDtlPk] = value;
            }
        }
        private int CurrDtlSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrDtlSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrDtlSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrDtlSlNo] = value;
            }
        }
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
        private int RowCount
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowCount] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.RowCount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RowCount] = value;
            }
        }
        private int BudgetStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.BudgetStatus] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.BudgetStatus]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BudgetStatus] = value;
            }
        }
        private BudgetBO ObjList
        {
            get
            {
                return (BudgetBO)this.ViewState["ObjList"];
            }
            set
            {
                this.ViewState["ObjList"] = value;
            }
        }
        private bool AmendPage
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.AmendPage] == null ? false : (bool)ViewState[ERP.Utilities.ViewstateStrings.AmendPage];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.AmendPage] = value;
            }
        }
        private bool AmendAdd
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.AmendAdd] == null ? false : (bool)ViewState[ERP.Utilities.ViewstateStrings.AmendAdd];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.AmendAdd] = value;
            }
        }
        private DataSet dtBudgetDetails
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.dtBudgetDetails] == null ? null : (DataSet)ViewState[ERP.Utilities.ViewstateStrings.dtBudgetDetails];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.dtBudgetDetails] = value;
            }
        }
        private bool AmendPagination
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.AmendPagination] == null ? false : (bool)ViewState[ERP.Utilities.ViewstateStrings.AmendPagination];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.AmendPagination] = value;
            }
        }
        private bool IsImport
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.IsImport] == null ? false : (bool)ViewState[ERP.Utilities.ViewstateStrings.IsImport];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.IsImport] = value;
            }
        }
        #endregion

        #region Variables
        User currentUser;
        DataTable dtPageData;
        private List<Month> months = new List<Month>();
        private string uploadPath;
        private string[] excelColumns;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private DataTable dtExcelSchema;
        private DataSet dsImportdata;
        private string landingSheet;
        private StringBuilder sb;
        //private string[] airColums_General = { "Plant", "CostCenter", "AccountNo", "AccountName", "Budget", "Month" };
        private string[] airColums_General = { "Plant", "CostCenter", "CODEA/C", "DESCRIPTION" };
        int TransFlag = 0;
        decimal Total = 0;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvanceSrch", "$(document).ready(function(){ShowHideAdvancedDtlSearch(1);});", true);
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(4);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(0);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.AMEND)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(3);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvanceSrch", "$(document).ready(function(){ShowHideAdvancedDtlSearch(0);});", true);
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
                AmendAdd = false;
                if (AmendPage)
                {
                    AmendPagination = true;
                    SaveBudget(Convert.ToInt32(WorkflowTransactionFlag.SAVE), true);
                    GetFieldValues(ControlEnums.SELBUDGETLIST);
                    AmendPagination = false;
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    GetFieldValues(ControlEnums.SELBUDGETLIST);
                }
                SetFieldValues(ControlEnums.ADDLIST);
                EnableDisableButtons(e.TotalPages);
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
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    GetFieldValues(ControlEnums.PLANT);
                    SetFieldValues(ControlEnums.PLANT);
                    SetFieldValues(ControlEnums.ADVPLANT);
                    GetFieldValues(ControlEnums.FINYEAR);
                    SetFieldValues(ControlEnums.FINYEAR);
                    SetFieldValues(ControlEnums.MONTH);
                    GetFieldValues(ControlEnums.BUDGETLIST);
                    SetFieldValues(ControlEnums.BUDGETLIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    ObjList = new BudgetBO();
                    ObjList.Details = new List<BudgetDetails>();
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    FillProcessID(1);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            try
            {
                switch (type)
                {
                    #region Budget List
                    case ControlEnums.BUDGETLIST:
                        //int BudgetNo = Convert.ToInt32(hdfBudgetNumber.Value);
                        int BudgetNo = 0;
                        int FinYear = Convert.ToInt32(ddlFinYearSrch.SelectedValue);
                        dtPageData = BudgetBL.GetBudgetList(currentUser.PKUser, currentUser.SBUID, BudgetNo, FinYear);
                        break;
                    #endregion
                    #region Financial Year
                    case ControlEnums.FINYEAR:
                        dtPageData = BudgetBL.GetFinYear(currentUser.SBUID);
                        break;
                    #endregion
                    #region Plant
                    case ControlEnums.PLANT:
                        string splCond = "prd";
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0, splCond, CurrPK);
                        break;
                    #endregion
                    #region CHANGE
                    case ControlEnums.ACCNOCHANGE:
                        int CoaPk = hdnBadgetAccNo.Value != string.Empty ? Convert.ToInt32(hdnBadgetAccNo.Value) : 0;
                        dtPageData = BudgetBL.GetCoaParent("%%", "COA_NAME_ONLY", 1, currentUser.SBUID, 0, CoaPk);
                        break;
                    case ControlEnums.ACCNAMECHANGE:
                        CoaPk = hdnBadgetAccName.Value != string.Empty ? Convert.ToInt32(hdnBadgetAccName.Value) : 0;
                        dtPageData = BudgetBL.GetCoaParent("%%", "COA_CODE", 1, currentUser.SBUID, 0, CoaPk);
                        break;
                    #endregion
                    #region SELBUDGET
                    case ControlEnums.SELBUDGET:
                        if (CurrPK > 0)
                        {
                            string List = BudgetBL.GetBudget(CurrPK);
                            if (List != string.Empty)
                            {
                                ObjList = (BudgetBO)CommonFunctions.DeserializeObject(List, new BudgetBO());
                                BudgetStatus = ObjList.BGH_STATUS;
                                ObjList.BGH_BIZUNIT = currentUser.SBUID;
                                ObjList.UserPk = currentUser.PKUser;
                                lblBudgetTotal.Text = ObjList.BGH_AMOUNT.ToString("N2");
                                ObjList.Details = new List<BudgetDetails>();
                            }
                        }
                        else
                        {
                            ObjList = new BudgetBO();
                            ObjList.Details = new List<BudgetDetails>();
                        }
                        break;
                    #endregion
                    #region SELBUDGETLIST
                    case ControlEnums.SELBUDGETLIST:
                        if (CurrPK > 0)
                        {
                            int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                            int SrchPlant = Convert.ToInt32(ddlPlantSrch.SelectedValue);
                            int SrchCostCenter = Convert.ToInt32(hdfCostCenterSrch.Value);
                            if (hdfCostCenterSrch.Value == "-1" || hdfCostCenterSrch.Value == "0")
                            {
                                txtCostCenterSrch.Text = "";
                                hdfCostCenterSrch.Value = "0";
                            }
                            int SrchAccNo = Convert.ToInt32(hdfAccNoSrch.Value);
                            if (hdfAccNoSrch.Value == "-1" || hdfAccNoSrch.Value == "0")
                            {
                                txtAccNoSrch.Text = "";
                                hdfAccNoSrch.Value = "0";
                            }
                            dtBudgetDetails = BudgetBL.GetBudgetDetailsList(CurrPK, Convert.ToInt32(PageIndex), pageSize, SrchPlant, SrchCostCenter, SrchAccNo);
                            if (dtBudgetDetails.Tables.Count > 0 && dtBudgetDetails.Tables[1].Rows.Count > 0)
                            {
                                RowCount = Convert.ToInt32(dtBudgetDetails.Tables[0].Rows[0][0].ToString());
                                decimal budgetTotal = Convert.ToDecimal(dtBudgetDetails.Tables[0].Rows[0][1]);
                                lblBudgetTotal.Text = budgetTotal.ToString("N2");
                                BudgetBO ObjListDtl = new BudgetBO();
                                ObjListDtl = (BudgetBO)CommonFunctions.DeserializeObject(dtBudgetDetails.Tables[1].Rows[0][0].ToString(), new BudgetBO());
                                ObjList.Details = ObjListDtl.Details;
                            }
                            else
                            {
                                ObjList.Details = new List<BudgetDetails>();
                            }
                        }
                        else
                        {
                            ObjList = new BudgetBO();
                            ObjList.Details = new List<BudgetDetails>();
                        }
                        break;
                    #endregion
                    #region HISTORY
                    case ControlEnums.HISTORYHDR:
                        if (CurrPK > 0)
                        {
                            dtPageData = BudgetBL.GetBudgetHistory(CurrPK);
                        }
                        break;
                    case ControlEnums.HISTORYDTL:
                        if (CurrDtlPk > 0)
                        {
                            dtPageData = BudgetBL.GetBudgetDtlHistory(CurrDtlPk);
                        }
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
                    case ControlEnums.BUDGETLIST:
                        BindGrid(controlType);
                        break;
                    case ControlEnums.PLANT:
                        BindDropDown(controlType);
                        break;
                    case ControlEnums.ADVPLANT:
                        BindDropDown(controlType);
                        break;
                    case ControlEnums.FINYEAR:
                        BindDropDown(controlType);
                        break;
                    case ControlEnums.MONTH:
                        BindDropDown(controlType);
                        break;
                    case ControlEnums.ACCNOCHANGE:
                        SetUIEditView(controlType);
                        break;
                    case ControlEnums.ACCNAMECHANGE:
                        SetUIEditView(controlType);
                        break;
                    case ControlEnums.ADDLIST:
                        BindGrid(controlType);
                        break;
                    case ControlEnums.SELBUDGET:
                        SetUIEditView(controlType);
                        break;
                    case ControlEnums.HISTORYHDR:
                        BindGrid(controlType);
                        break;
                    case ControlEnums.HISTORYDTL:
                        BindGrid(controlType);
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
                case ActionsEnum.LIST:
                    CurrPK = 0;
                    CurrDtlSlNo = 0;
                    AmendPage = false;
                    AmendAdd = false;
                    PageIndex = "1";
                    //txtBudgetNumber.Text = "";
                    //hdfBudgetNumber.Value = "0";
                    ddlFinYearSrch.SelectedValue = "-1";
                    ddlFinYear.SelectedValue = "-1";
                    ddlFinYear.Enabled = true;
                    chkForcast.Checked = false;
                    txtCostCenter.Text = "";
                    ddlMonth.SelectedValue = "-1";
                    ddlPlant.SelectedValue = "-1";
                    txtAccountNo.Text = "";
                    txtAccountName.Text = "";
                    txtBudget.Text = "";
                    hdnBadgetCostCenter.Value = "0";
                    hdnBadgetAccNo.Value = "0";
                    hdnBadgetAccName.Value = "0";
                    //hdfBudgetNo.Value = "";
                    //lblBudgetNo.Text = hdfBudgetNo.Value == string.Empty ? "[NEW]" : hdfBudgetNo.Value;
                    ddlPlantSrch.SelectedValue = "-1";
                    txtCostCenterSrch.Text = "";
                    hdfCostCenterSrch.Value = "0";
                    txtAccNoSrch.Text = "";
                    hdfAccNoSrch.Value = "0";
                    AmendPagination = false;
                    chkForcast.Checked = false;
                    chkForcast.Enabled = true;
                    lblchkForcast.Visible = true;
                    chkForcast.Visible = true;
                    lblBudgetTotal.Text = "";
                    hdfCurrBudgetPk.Value = "0";
                    IsImport = false;
                    break;
                case ActionsEnum.NEW:
                    CurrPK = 0;
                    CurrDtlSlNo = 0;
                    AmendPage = false;
                    AmendAdd = false;
                    RowCount = 0;
                    BudgetStatus = 0;
                    //txtBudgetNumber.Text = "";
                    //hdfBudgetNumber.Value = "0";
                    ddlFinYearSrch.SelectedValue = "-1";
                    ddlFinYear.SelectedValue = "-1";
                    ddlFinYear.Enabled = true;
                    chkForcast.Checked = false;
                    txtCostCenter.Text = "";
                    ddlMonth.SelectedValue = "-1";
                    ddlPlant.SelectedValue = "-1";
                    txtAccountNo.Text = "";
                    txtAccountName.Text = "";
                    txtBudget.Text = "";
                    hdnBadgetCostCenter.Value = "0";
                    hdnBadgetAccNo.Value = "0";
                    hdnBadgetAccName.Value = "0";
                    //hdfBudgetNo.Value = "";
                    //lblBudgetNo.Text = hdfBudgetNo.Value == string.Empty ? "[NEW]" : hdfBudgetNo.Value;
                    ddlPlantSrch.SelectedValue = "-1";
                    txtCostCenterSrch.Text = "";
                    hdfCostCenterSrch.Value = "0";
                    txtAccNoSrch.Text = "";
                    hdfAccNoSrch.Value = "0";
                    AmendPagination = false;
                    chkForcast.Checked = false;
                    chkForcast.Enabled = true;
                    lblchkForcast.Visible = true;
                    chkForcast.Visible = true;
                    lblBudgetTotal.Text = "";
                    dtBudgetDetails = new DataSet();
                    hdfCurrBudgetPk.Value = "0";
                    IsImport = false;
                    break;
                case ActionsEnum.ADDLIST:
                    //txtBudgetNumber.Text = "";
                    //hdfBudgetNumber.Value = "0";
                    ddlFinYearSrch.SelectedValue = "-1";
                    txtCostCenter.Text = "";
                    ddlMonth.SelectedValue = "-1";
                    ddlPlant.SelectedValue = "-1";
                    txtAccountNo.Text = "";
                    txtAccountName.Text = "";
                    txtBudget.Text = "";
                    hdnBadgetCostCenter.Value = "0";
                    hdnBadgetAccNo.Value = "0";
                    hdnBadgetAccName.Value = "0";
                    //lblBudgetNo.Text = hdfBudgetNo.Value == string.Empty ? "[NEW]" : hdfBudgetNo.Value;
                    ddlPlantSrch.SelectedValue = "-1";
                    txtCostCenterSrch.Text = "";
                    hdfCostCenterSrch.Value = "0";
                    txtAccNoSrch.Text = "";
                    hdfAccNoSrch.Value = "0";
                    AmendPagination = false;
                    break;
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
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtAmendAmount")
                    {
                        commonActions = ActionsEnum.TEXTCHANGE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.CHANGE;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHANGE;
                }
                #endregion

                switch (commonActions)
                {
                    #region List
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnums.BUDGETLIST);
                        SetFieldValues(ControlEnums.BUDGETLIST);
                        break;
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnums.PLANT);
                        SetFieldValues(ControlEnums.PLANT);
                        SetFieldValues(ControlEnums.ADVPLANT);
                        ResetForm(ActionsEnum.LIST);
                        GetFieldValues(ControlEnums.BUDGETLIST);
                        SetFieldValues(ControlEnums.BUDGETLIST);
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        FillProcessID(1);
                        ResetForm(ActionsEnum.NEW);
                        EntryStatus = EntryStatus.NEWMODE;
                        GetFieldValues(ControlEnums.SELBUDGETLIST);
                        SetFieldValues(ControlEnums.ADDLIST);
                        break;
                    #endregion
                    #region CHANGE
                    case ActionsEnum.ACCNOCHANGE:
                        GetFieldValues(ControlEnums.ACCNOCHANGE);
                        SetFieldValues(ControlEnums.ACCNOCHANGE);
                        break;
                    case ActionsEnum.ACCNAMECHANGE:
                        GetFieldValues(ControlEnums.ACCNAMECHANGE);
                        SetFieldValues(ControlEnums.ACCNAMECHANGE);
                        break;
                    #endregion
                    #region Add List
                    case ActionsEnum.ADDLIST:
                        SetUIEditView(ControlEnums.ADDLIST);
                        SetFieldValues(ControlEnums.ADDLIST);
                        ResetForm(ActionsEnum.ADDLIST);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (ObjList.Details.Count > 0)
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            FillWorkflowProcess();
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_BudgetListNull").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region AMENDSUBMIT
                    case ActionsEnum.AMENDSUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        // FillAmendProcess();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region Save
                    case ActionsEnum.SAVE:
                        if (ObjList.Details.Count > 0)
                        {
                            TransFlag = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                            SaveBudget(TransFlag, false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_BudgetListNull").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region WRKFSUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        if (ObjList.Details.Count > 0)
                        {
                            ucrWrkf.ApplicationID = 0;
                            TransFlag = Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT);
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                AmendAdd = false;
                                SaveBudget(TransFlag, false);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_BudgetListNull").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region BUDGETDTLEDIT
                    case ActionsEnum.BUDGETDTLEDIT:
                        SetUIEditView(ControlEnums.BUDGETDTLEDIT);
                        break;
                    #endregion
                    #region IMPORT
                    case ActionsEnum.IMPORT:
                        if (fupImport.HasFile)
                        {
                            if (ddlFinYear.SelectedValue != "-1")
                            {
                                IsImport = true;
                                PageIndex = "1";
                                string conStr;
                                string filePath = SaveDetails(out conStr, fupImport);
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    ImportToGrid(filePath, conStr);
                                }
                                IsImport = false;
                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Year").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoFile").ToString())
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        EntryStatus = BusinessObject.Common.EntryStatus.LISTMODE;
                        //txtBudgetNumber.Text = "";
                        //hdfBudgetNumber.Value = "0";
                        ddlFinYearSrch.SelectedValue = "-1";
                        GetFieldValues(ControlEnums.BUDGETLIST);
                        SetFieldValues(ControlEnums.BUDGETLIST);
                        break;
                    #endregion
                    #region SEARCHBUDGET
                    case ActionsEnum.SEARCHBUDGET:
                        PageIndex = "1";
                        GetFieldValues(ControlEnums.SELBUDGETLIST);
                        SetFieldValues(ControlEnums.ADDLIST);
                        break;
                    #endregion
                    #region CLEARBUDGET
                    case ActionsEnum.CLEARBUDGET:
                        ddlPlantSrch.SelectedValue = "-1";
                        txtCostCenterSrch.Text = "";
                        hdfCostCenterSrch.Value = "0";
                        txtAccNoSrch.Text = "";
                        hdfAccNoSrch.Value = "0";
                        GetFieldValues(ControlEnums.SELBUDGETLIST);
                        SetFieldValues(ControlEnums.ADDLIST);
                        break;
                    #endregion
                    case ActionsEnum.CHANGE:
                        SetUIEditView(ControlEnums.BUDGETHDR);
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void SaveBudget(int flag, bool PageSave)
        {
            BudgetBO objSaveBudget = new BudgetBO();
            int result = 0;
            WorkflowDetails wkfDetails = null;
            objSaveBudget = new BudgetBO();
            objSaveBudget = ObjList;
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objSaveBudget.WKF_PROCESS = wkfDetails.ProcessID;
            objSaveBudget.WKF_TASK = wkfDetails.TaskID;
            objSaveBudget.WKF_TASK_ACTION = wkfDetails.ActionID;
            objSaveBudget.WKF_TRX_FLAG = flag;
            objSaveBudget.BGH_STATUS = 1;
            litErrorMsg.Text = "";
            if (AmendPage)
            {
                objSaveBudget.BGH_IS_AMEND = 1;
                if (!IsImport)
                {
                    foreach (GridViewRow row in grdSelectedBudget.Rows)
                    {
                        int SlNo = Convert.ToInt32((row.FindControl("hdfBudgetDtlSlNo") as HiddenField).Value);
                        TextBox BudgetAmount = (TextBox)row.FindControl("txtAmendAmount");
                        if (float.Parse(BudgetAmount.Text) > 0)
                        {
                            objSaveBudget.Details.SingleOrDefault(x => x.BDG_SL_NO == SlNo).BDG_AMEND_AMOUNT = float.Parse(BudgetAmount.Text);
                            if (objSaveBudget.Details.SingleOrDefault(x => x.BDG_SL_NO == SlNo).BDG_STATUS_BIT != 2)
                                objSaveBudget.Details.SingleOrDefault(x => x.BDG_SL_NO == SlNo).BDG_STATUS_BIT = 1;
                        }
                    }
                }
            }
            else
                objSaveBudget.BGH_IS_AMEND = 0;
            DataTable dtOut = null;
            result = BudgetBL.SaveBudgetDetails(CommonFunctions.XmlSerialize<BudgetBO>(objSaveBudget), ref dtOut);
            if (result > 0)
            {
                if (!AmendPage)
                {
                    if (flag == 1)
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_Submit_Success").ToString());
                    else
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_Save_Success").ToString());
                }
                else
                {
                    if (!AmendAdd)
                    {
                        if (AmendPagination)
                            litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_AmendPageSave_Success").ToString());
                        else
                        {
                            if (flag == 1)
                                litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_AmendSubmit_Success").ToString());
                            else
                                litErrorMsg.Text = String.Format(GetLocalResourceObject("Msg_AmendSave_Success").ToString());
                        }
                    }
                }
                if (PageSave)
                {
                    EntryStatus = EntryStatus.AMEND;
                }
                else
                {
                    GetFieldValues(ControlEnums.BUDGETLIST);
                    SetFieldValues(ControlEnums.BUDGETLIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm(ActionsEnum.LIST);
                }
                if (!AmendAdd)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "', '" + Resources.Captions.Information + "');", true);
                }
            }
            else
            {
                DbSaveStatus saveStatus = (DbSaveStatus)result;
                switch (saveStatus)
                {
                    case DbSaveStatus.SQLERROR:
                        litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Sql_Error, GetLocalResourceObject("Budget").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.CHECKCOA:
                        sb = new StringBuilder();
                        sb.Append(String.Format(GetLocalResourceObject("CoaNotMatch").ToString(), GetLocalResourceObject("Budget").ToString()));
                        foreach (DataRow item in dtOut.Rows)
                        {

                            if (item[0].ToString() != "")
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            else
                            {
                                sb = new StringBuilder();
                                sb.Append(String.Format(GetLocalResourceObject("CoaBlank").ToString(), GetLocalResourceObject("Budget").ToString()));
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    case DbSaveStatus.CHECKCS:
                        sb = new StringBuilder();
                        sb.Append(String.Format(GetLocalResourceObject("CSNotMatch").ToString(), GetLocalResourceObject("Budget").ToString()));
                        foreach (DataRow item in dtOut.Rows)
                        {
                            if (item[0].ToString() != "")
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            else
                            {
                                sb = new StringBuilder();
                                sb.Append(String.Format(GetLocalResourceObject("CSBlank").ToString(), GetLocalResourceObject("Budget").ToString()));
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    case DbSaveStatus.CHECKPLANT:
                        sb = new StringBuilder();
                        sb.Append(String.Format(GetLocalResourceObject("PlantNotMatch").ToString(), GetLocalResourceObject("Budget").ToString()));
                        foreach (DataRow item in dtOut.Rows)
                        {
                            if (item[0].ToString() != "")
                                sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                            else
                            {
                                sb = new StringBuilder();
                                sb.Append(String.Format(GetLocalResourceObject("PlantBlank").ToString(), GetLocalResourceObject("Budget").ToString()));
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    case DbSaveStatus.CHECKMONTH:
                        sb = new StringBuilder();
                        sb.Append(String.Format(GetLocalResourceObject("MonthNotMatch").ToString(), GetLocalResourceObject("Budget").ToString()));
                        foreach (DataRow item in dtOut.Rows)
                        {
                            sb.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    case DbSaveStatus.CHECKFINYEAR:
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("YearExist").ToString(), GetLocalResourceObject("Budget").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                    case DbSaveStatus.BUDGETEXIST:
                        litErrorMsg.Text = String.Format(GetLocalResourceObject("BudgetExist").ToString(), GetLocalResourceObject("Budget").ToString());
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "', '" + Resources.Captions.Information + "');", true);
                        break;
                }
            }
        }
        private void FillWorkflowProcess()
        {
            ucrWrkf.ViewType = 1;
            ucrWrkf.Visible = true;
            ucrWrkf.RefID = 0;
            ucrWrkf.ViewAction();
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            ucrWrkf.FillWorkFlowDetails();
        }

        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            if (senderGridView.ID == "grdBudgetList")
            {
                CurrPK = Convert.ToInt32((row.FindControl("hdfBudgetPK") as HiddenField).Value);
                switch (e.CommandName)
                {
                    #region AMMEND
                    case "AMMEND":
                        if (CurrPK > 0)
                        {
                            hdfCurrBudgetPk.Value = CurrPK.ToString();
                            EntryStatus = EntryStatus.AMEND;
                            AmendPage = true;
                            FillProcessID((int)POWorkflowType.AMEND);
                            GetFieldValues(ControlEnums.PLANT);
                            SetFieldValues(ControlEnums.ADVPLANT);
                            GetFieldValues(ControlEnums.SELBUDGET);
                            SetFieldValues(ControlEnums.SELBUDGET);
                            GetFieldValues(ControlEnums.SELBUDGETLIST);
                            SetFieldValues(ControlEnums.ADDLIST);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE || EntryStatus == EntryStatus.AMEND) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            ucrWrkf.ViewAction();
                        }
                        break;
                    #endregion
                    #region PERFORMACTION
                    case "PERFORMACTION":
                        if (CurrPK > 0)
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                            GetFieldValues(ControlEnums.SELBUDGET);
                            SetFieldValues(ControlEnums.SELBUDGET);
                            GetFieldValues(ControlEnums.SELBUDGETLIST);
                            SetFieldValues(ControlEnums.ADDLIST);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            ucrWrkf.ViewAction();
                        }
                        break;
                    #endregion
                    #region VIEW
                    case "VIEW":
                        if (CurrPK > 0)
                        {
                            hdfCurrBudgetPk.Value = CurrPK.ToString();
                            EntryStatus = EntryStatus.VIEWMODE;
                            GetFieldValues(ControlEnums.PLANT);
                            SetFieldValues(ControlEnums.ADVPLANT);
                            GetFieldValues(ControlEnums.SELBUDGET);
                            SetFieldValues(ControlEnums.SELBUDGET);
                            GetFieldValues(ControlEnums.SELBUDGETLIST);
                            SetFieldValues(ControlEnums.ADDLIST);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case "DELETEBUDGET":
                        if (CurrPK > 0)
                        {
                            DeleteBudget(CurrPK);
                        }
                        break;
                    #endregion
                    #region HISTORYHDR
                    case "HISTORYHDR":

                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlEnums.HISTORYHDR);
                            SetFieldValues(ControlEnums.HISTORYHDR);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divHistoryDetail]','" + GetLocalResourceObject("HistoryDetail").ToString() + "','" + GetLocalResourceObject("popWidth") + "','" + GetLocalResourceObject("popHeight") + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINT
                    case "PRINT":
                        // string url = GetLocalResourceObject("PurchaseRequestReportURL").ToString() + "?ID=" + pk + "&APPTYPE=" + hdfAppType.Value.ToString() + "&APPSUBTYPE=" + hdfAppSubType.Value.ToString();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OpenPDF", "OpenPDF('" + url + "');", true);       
                        break;
                        #endregion
                }
            }
            else if (senderGridView.ID == "grdSelectedBudget")
            {
                CurrDtlPk = Convert.ToInt32((row.FindControl("hdfBudgetDtlPK") as HiddenField).Value);
                CurrDtlSlNo = Convert.ToInt32((row.FindControl("hdfBudgetDtlSlNo") as HiddenField).Value);
                switch (e.CommandName)
                {
                    #region BUDGETDTLEDIT
                    case "BUDGETDTLEDIT":

                        if (CurrDtlSlNo > 0)
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                            SetUIEditView(ControlEnums.BUDGETDTLEDIT);
                        }
                        break;
                    #endregion
                    #region BUDGETDTLDELET
                    case "BUDGETDTLDELET":
                        if (CurrDtlSlNo > 0)
                        {
                            SetUIEditView(ControlEnums.BUDGETDTLDELET);
                        }
                        break;
                    #endregion
                    #region BUDGETDTLUNDO
                    case "BUDGETDTLUNDO":
                        if (CurrDtlSlNo > 0)
                        {
                            SetUIEditView(ControlEnums.BUDGETDTLUNDO);
                        }
                        break;
                    #endregion
                    #region BUDGETDTLHISTORY
                    case "BUDGETDTLHISTORY":

                        if (CurrDtlPk > 0)
                        {
                            GetFieldValues(ControlEnums.HISTORYDTL);
                            SetFieldValues(ControlEnums.HISTORYDTL);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divHistoryDetail]','" + GetLocalResourceObject("HistoryDetail").ToString() + "','" + GetLocalResourceObject("popWidth") + "','" + GetLocalResourceObject("popHeight") + "');", true);
                        }

                        break;
                        #endregion
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
        private void DeleteBudget(int BudgetPk)
        {
            int delVal = BudgetBL.DeleteBudgetList(BudgetPk);
            switch (delVal)
            {
                case (int)DbDeleteStatus.DELETED:
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlEnums.BUDGETLIST);
                    SetFieldValues(ControlEnums.BUDGETLIST);
                    litErrorMsg.Text = Resources.Messages.DeletedSuccessfully;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("Budget").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                    break;
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdBudgetList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        //ImageButton imbDelete = e.Row.FindControl("imbDelete") as ImageButton;
                        ImageButton imbAmmend = e.Row.FindControl("imbAmmend") as ImageButton;
                        //ImageButton imbEdit = e.Row.FindControl("imbEdit") as ImageButton;
                        ImageButton imbHistory = e.Row.FindControl("imbHistory") as ImageButton;

                        int BudgetStatus = Convert.ToInt32((e.Row.FindControl("hdfBudgetStatus") as HiddenField).Value);
                        int BudgetType = Convert.ToInt32((e.Row.FindControl("hdfBudgetType") as HiddenField).Value);

                        if (BudgetStatus == 2)
                        {
                            //imbEdit.Visible = false;
                            //imbDelete.Visible = false;
                            imbHistory.Visible = true;
                        }
                        else if (BudgetStatus == 0)
                        {
                            imbHistory.Visible = false;
                            imbAmmend.Visible = false;
                            //imbDelete.Visible = true;
                        }

                        if (BudgetType == 1)
                            e.Row.AddCssClass("grid-rowcolor");
                    }
                }
                if (((GridView)sender).ID == "grdSelectedBudget")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        ImageButton imbBudgetDtlHistory = e.Row.FindControl("imbBudgetDtlHistory") as ImageButton;
                        // ImageButton imbDtlEdit = e.Row.FindControl("imbDtlEdit") as ImageButton;
                        ImageButton imbDtlDelete = e.Row.FindControl("imbDtlDelete") as ImageButton;
                        ImageButton imbBudgetDtlUndo = e.Row.FindControl("imbBudgetDtlUndo") as ImageButton;

                        int BudgetDtlPK = Convert.ToInt32((e.Row.FindControl("hdfBudgetDtlPK") as HiddenField).Value);
                        int BudgetDtlSlNo = Convert.ToInt32((e.Row.FindControl("hdfBudgetDtlSlNo") as HiddenField).Value);

                        if (BudgetStatus > 0)
                            imbBudgetDtlHistory.Visible = true;
                        else
                            imbBudgetDtlHistory.Visible = false;

                        if (ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == BudgetDtlSlNo).BDG_STATUS_BIT == 2)
                        {
                            e.Row.AddCssClass("table-firstlevel Disabld");
                            //imbDtlEdit.Visible = false;
                            imbDtlDelete.Visible = false;
                            imbBudgetDtlUndo.Visible = true;
                        }
                        else
                        {
                            e.Row.RemoveCssClass("table-firstlevel Disabld");
                            //imbDtlEdit.Visible = false;
                            imbDtlDelete.Visible = true;
                            imbBudgetDtlUndo.Visible = false;
                        }

                        if (ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == BudgetDtlSlNo).BDG_DEL_STATUS == 1 && ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == BudgetDtlSlNo).BDG_STATUS_BIT != 2)
                        {
                            e.Row.AddCssClass("table-firstlevel Disabld");
                            //imbDtlEdit.Visible = false;
                            imbDtlDelete.Visible = false;
                            imbBudgetDtlUndo.Visible = true;
                        }

                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            grdSelectedBudget.Columns[7].Visible = false;
                            //imbDtlEdit.Visible = false;
                            imbDtlDelete.Visible = false;
                            imbBudgetDtlHistory.Visible = true;
                            imbBudgetDtlUndo.Visible = false;
                        }
                        else if (EntryStatus == EntryStatus.NEWMODE)
                        {
                            grdSelectedBudget.Columns[7].Visible = false;
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE)
                        {
                            grdSelectedBudget.Columns[7].Visible = false;
                        }
                        else
                        {
                            grdSelectedBudget.Columns[7].Visible = true;
                        }
                        Total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, GetGlobalResourceObject("DataFieldRes", "BudgetListAmount").ToString()));
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        e.Row.Cells[6].Text = Total.ToString("N2");
                        e.Row.Cells[6].AddCssClass("textalignRight");
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
                case ControlEnums.BUDGETLIST:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        grdBudgetList.DataSource = dtPageData;
                        grdBudgetList.DataBind();
                    }
                    else
                    {
                        grdBudgetList.DataSource = null;
                        grdBudgetList.DataBind();
                    }
                    break;
                case ControlEnums.ADDLIST:
                    if (ObjList != null && ObjList.Details != null && ObjList.Details.Count > 0)
                    {
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dtBudgetDetails != null && dtBudgetDetails.Tables.Count > 0)
                            RowCount = Convert.ToInt32(dtBudgetDetails.Tables[0].Rows[0]["COUNT"].ToString());
                        else
                            RowCount = ObjList.Details.Count();
                        uclPaging.TotalPages = RowCount == 0 ? 1 : (RowCount <= pageSize) ? 1 :
                              (RowCount % pageSize) == 0 ? (RowCount / pageSize) :
                              (RowCount / pageSize) + 1;
                        //uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? "1" : PageIndex;
                        int skipCount = (Convert.ToInt32(PageIndex) - 1) * pageSize;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtBudgetDetails != null && dtBudgetDetails.Tables.Count > 0)
                            grdSelectedBudget.DataSource = ObjList.Details.ToList();
                        else
                            grdSelectedBudget.DataSource = ObjList.Details.Skip(skipCount).Take(pageSize).ToList();
                        grdSelectedBudget.DataBind();
                        tblBudget.Visible = true;
                        uclPaging.Visible = true;
                        uclPaging.BindPager();

                        //int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        //if (ObjList.Details.Count > 0)
                        //    RowCount = ObjList.Details.Count();
                        //else
                        //    RowCount = 1;
                        //uclPaging.TotalPages = RowCount == 0 ? 1 : (RowCount <= pageSize) ? 1 :
                        //      (RowCount % pageSize) == 0 ? (RowCount / pageSize) :
                        //      (RowCount / pageSize) + 1;
                        //PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        //int skipCount = (Convert.ToInt32(PageIndex) - 1) * pageSize;
                        //uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        //grdSelectedBudget.DataSource = ObjList.Details.Skip(skipCount).Take(pageSize).ToList();
                        //grdSelectedBudget.DataBind();
                        //tblBudget.Visible = true;
                        //uclPaging.Visible = true;
                        //uclPaging.BindPager();
                    }
                    else
                    {
                        grdSelectedBudget.DataSource = null;
                        grdSelectedBudget.DataBind();
                        tblBudget.Visible = false;
                        uclPaging.Visible = false;
                    }
                    break;
                case ControlEnums.HISTORYHDR:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        grdHistoryList.DataSource = dtPageData;
                        grdHistoryList.DataBind();
                    }
                    else
                    {
                        grdHistoryList.DataSource = null;
                        grdHistoryList.DataBind();
                    }
                    break;
                case ControlEnums.HISTORYDTL:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        grdHistoryList.DataSource = dtPageData;
                        grdHistoryList.DataBind();
                    }
                    else
                    {
                        grdHistoryList.DataSource = null;
                        grdHistoryList.DataBind();
                    }
                    break;
            }
        }
        #endregion

        #region EditView
        private void SetUIEditView(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region Change
                case ControlEnums.ACCNOCHANGE:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        txtAccountName.Text = dtPageData.Rows[0]["VALUE"].ToString();
                        hdnBadgetAccName.Value = dtPageData.Rows[0]["PK"].ToString();
                    }
                    break;
                case ControlEnums.ACCNAMECHANGE:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        txtAccountNo.Text = dtPageData.Rows[0]["VALUE"].ToString();
                        hdnBadgetAccNo.Value = dtPageData.Rows[0]["PK"].ToString();
                    }
                    break;
                #endregion

                #region Add List
                case ControlEnums.ADDLIST:
                    PageIndex = "1";
                    if (CurrDtlSlNo == 0)
                    {
                        bool Isexist = false;
                        int slno = 0;
                        if (ObjList.Details.Any(x => x.BDG_PLANT_CODE == ddlPlant.SelectedItem.ToString() && x.BDG_COST_CENTER_NAME == txtCostCenter.Text.ToString() && x.BDG_ACCOUNT_NO == txtAccountNo.Text.ToString() && x.BDG_MONTH_TEXT == ddlMonth.SelectedItem.ToString()))
                        {
                            Isexist = true;
                            slno = ObjList.Details.SingleOrDefault(x => x.BDG_PLANT_CODE == ddlPlant.SelectedItem.ToString() && x.BDG_COST_CENTER_NAME == txtCostCenter.Text.ToString() && x.BDG_ACCOUNT_NO == txtAccountNo.Text.ToString() && x.BDG_MONTH_TEXT == ddlMonth.SelectedItem.ToString()).BDG_SL_NO;
                        }
                        if (Isexist)
                        {
                            if (AmendPage)
                            {
                                ObjList.BGH_IS_AMEND = 1;
                                ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == slno).BDG_AMEND_AMOUNT = float.Parse(txtBudget.Text);
                                ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == slno).BDG_STATUS_BIT = 1;
                            }
                            else
                            {
                                string Msg = String.Format(GetLocalResourceObject("BudgetExist").ToString(), GetLocalResourceObject("Budget").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Msg) + "', '" + Resources.Captions.Information + "');", true);
                                break;
                            }
                        }
                        else
                        {
                            BudgetDetails objBudget = new BudgetDetails();
                            objBudget.BDG_MONTH = Convert.ToInt32(ddlMonth.SelectedValue);
                            objBudget.BDG_MONTH_TEXT = ddlMonth.SelectedItem.ToString();
                            if (dtBudgetDetails != null && dtBudgetDetails.Tables.Count > 0)
                            {
                                objBudget.BDG_SL_NO = Convert.ToInt32(dtBudgetDetails.Tables[0].Rows[0]["COUNT"]) + 1;
                            }
                            else
                            {
                                if (ObjList.Details.Count() == 0)
                                    objBudget.BDG_SL_NO = 1;
                                else
                                    objBudget.BDG_SL_NO = ObjList.Details.Max(x => x.BDG_SL_NO) + 1;
                            }
                            objBudget.BDG_PLANT = Convert.ToInt32(ddlPlant.SelectedValue);
                            objBudget.BDG_PLANT_CODE = ddlPlant.SelectedItem.ToString();
                            objBudget.BDG_COST_CENTER = Convert.ToInt32(hdnBadgetCostCenter.Value);
                            objBudget.BDG_COST_CENTER_NAME = txtCostCenter.Text;
                            objBudget.BDG_COA = Convert.ToInt32(hdnBadgetAccNo.Value);
                            objBudget.BDG_ACCOUNT_NO = txtAccountNo.Text;
                            objBudget.BDG_ACCOUNT_NAME = txtAccountName.Text;
                            objBudget.BDG_AMEND_AMOUNT = 0;
                            if (AmendPage)
                            {
                                ObjList.BGH_IS_AMEND = 1;
                                objBudget.BDG_AMEND_AMOUNT = float.Parse(txtBudget.Text);
                            }
                            else
                            {
                                ObjList.BGH_IS_AMEND = 0;
                                objBudget.BDG_AMOUNT = float.Parse(txtBudget.Text);
                            }
                            objBudget.BDG_VERSION = 0;
                            objBudget.BDG_YEAR = Convert.ToInt32(ddlFinYear.SelectedValue);
                            objBudget.BDG_STATUS_BIT = 0;
                            ObjList.Details.Add(objBudget);
                        }
                        lblBudgetTotal.Text = ObjList.Details.Sum(x => x.BDG_AMOUNT).ToString("N2");
                        if (AmendPage)
                        {
                            AmendAdd = true;
                            SaveBudget(Convert.ToInt32(WorkflowTransactionFlag.SAVE), true);
                            GetFieldValues(ControlEnums.SELBUDGETLIST);
                        }
                    }
                    else
                    {
                        BudgetDetails objBudgetDtl = new BudgetDetails();
                        objBudgetDtl = ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == CurrDtlSlNo);
                        objBudgetDtl.BDG_COST_CENTER = Convert.ToInt32(hdnBadgetCostCenter.Value);
                        objBudgetDtl.BDG_COST_CENTER_NAME = txtCostCenter.Text;
                        objBudgetDtl.BDG_MONTH = Convert.ToInt32(ddlMonth.SelectedValue);
                        objBudgetDtl.BDG_COA = Convert.ToInt32(hdnBadgetAccNo.Value);
                        objBudgetDtl.BDG_ACCOUNT_NO = txtAccountNo.Text;
                        objBudgetDtl.BDG_ACCOUNT_NAME = txtAccountName.Text;
                        objBudgetDtl.BDG_AMOUNT = float.Parse(txtBudget.Text);
                        objBudgetDtl.BDG_PLANT = Convert.ToInt32(ddlPlant.SelectedValue);
                        objBudgetDtl.BDG_PLANT_CODE = ddlPlant.SelectedItem.ToString();
                        objBudgetDtl.BDG_AMOUNT = float.Parse(txtBudget.Text);
                        ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == CurrDtlSlNo).BDG_STATUS_BIT = 1;
                    }
                    break;
                #endregion

                #region EDIT
                case ControlEnums.SELBUDGET:
                    if (ObjList != null)
                    {
                        ddlFinYear.SelectedIndex = ddlFinYear.Items.IndexOf(ddlFinYear.Items.FindByValue(Convert.ToInt32(ObjList.BGH_YEAR).ToString()));
                        ddlFinYear.Enabled = false;
                        //hdfBudgetNo.Value = lblBudgetNo.Text = ObjList.BGH_NO;
                        ObjList.BGH_BIZUNIT = currentUser.SBUID;
                        ObjList.UserPk = currentUser.PKUser;
                        if (Convert.ToInt32(ObjList.BGH_TYPE) == 1)
                        {
                            lblchkForcast.Visible = true;
                            chkForcast.Visible = true;
                            chkForcast.Checked = true;
                            chkForcast.Enabled = false;
                        }
                        else
                        {
                            lblchkForcast.Visible = false;
                            chkForcast.Visible = false;
                            chkForcast.Checked = false;
                        }
                    }
                    break;
                #endregion

                #region BUDGETDTLEDIT
                case ControlEnums.BUDGETDTLEDIT:
                    if (ObjList.Details.Any(x => x.BDG_SL_NO == CurrDtlSlNo))
                    {
                        BudgetDetails objBudgetDtl = new BudgetDetails();
                        objBudgetDtl = ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == CurrDtlSlNo);
                        ddlFinYear.SelectedIndex = ddlFinYear.Items.IndexOf(ddlFinYear.Items.FindByValue(Convert.ToInt32(objBudgetDtl.BDG_YEAR).ToString()));
                        hdnBadgetCostCenter.Value = objBudgetDtl.BDG_COST_CENTER.ToString();
                        txtCostCenter.Text = objBudgetDtl.BDG_COST_CENTER_NAME.ToString();
                        ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(objBudgetDtl.BDG_MONTH_TEXT.ToString()));
                        ddlPlant.SelectedIndex = ddlPlant.Items.IndexOf(ddlPlant.Items.FindByText(objBudgetDtl.BDG_PLANT_CODE.ToString()));
                        txtAccountNo.Text = objBudgetDtl.BDG_ACCOUNT_NO.ToString();
                        txtAccountName.Text = objBudgetDtl.BDG_ACCOUNT_NAME.ToString();
                        hdnBadgetAccNo.Value = objBudgetDtl.BDG_COA.ToString();
                        txtBudget.Text = objBudgetDtl.BDG_AMOUNT.ToString();
                    }
                    break;
                #endregion

                #region BUDGETDTLDELET
                case ControlEnums.BUDGETDTLDELET:
                    if (ObjList.Details.Any(x => x.BDG_SL_NO == CurrDtlSlNo))
                    {
                        ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == CurrDtlSlNo).BDG_STATUS_BIT = 2;
                        BindGrid(ControlEnums.ADDLIST);
                        CurrDtlSlNo = 0;
                    }
                    break;
                #endregion

                #region BUDGETDTLDELET
                case ControlEnums.BUDGETDTLUNDO:
                    if (ObjList.Details.Any(x => x.BDG_SL_NO == CurrDtlSlNo))
                    {
                        ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == CurrDtlSlNo).BDG_STATUS_BIT = 0;
                        ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == CurrDtlSlNo).BDG_DEL_STATUS = 0;
                        BindGrid(ControlEnums.ADDLIST);
                    }
                    break;
                #endregion

                #region BUDGETHDR
                case ControlEnums.BUDGETHDR:
                    if (ObjList != null)
                    {
                        ObjList.BGH_YEAR = Convert.ToInt32(ddlFinYear.SelectedValue);
                        ObjList.BGH_BIZUNIT = currentUser.SBUID;
                        ObjList.UserPk = currentUser.PKUser;
                        ObjList.BGH_TYPE = chkForcast.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                        ObjList.BGH_VERSION = 0;
                        ObjList.BGH_BIZUNIT = currentUser.SBUID;
                    }
                    break;
                    #endregion
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
                case ControlEnums.PLANT:
                    ddlPlant.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlPlant.DataSource = dtPageData;
                        ddlPlant.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                        ddlPlant.DataValueField = Resources.DataFieldRes.CMP_PK;
                        ddlPlant.DataBind();
                    }
                    ddlPlant.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlEnums.ADVPLANT:
                    ddlPlantSrch.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlPlantSrch.DataSource = dtPageData;
                        ddlPlantSrch.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                        ddlPlantSrch.DataValueField = Resources.DataFieldRes.CMP_PK;
                        ddlPlantSrch.DataBind();
                    }
                    ddlPlantSrch.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlEnums.FINYEAR:
                    ddlFinYearSrch.Items.Clear();
                    ddlFinYear.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlFinYearSrch.DataSource = dtPageData;
                        ddlFinYearSrch.DataTextField = Resources.DataFieldRes.FYR_FIN_YEAR;
                        ddlFinYearSrch.DataValueField = Resources.DataFieldRes.FYR_FIN_YEAR;
                        ddlFinYearSrch.DataBind();

                        ddlFinYear.DataSource = dtPageData;
                        ddlFinYear.DataTextField = Resources.DataFieldRes.FYR_FIN_YEAR;
                        ddlFinYear.DataValueField = Resources.DataFieldRes.FYR_FIN_YEAR;
                        ddlFinYear.DataBind();
                    }
                    ddlFinYearSrch.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlFinYear.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlEnums.MONTH:
                    ddlMonth.Items.Clear();
                    months = SetMonth();
                    if (months != null && months.Count > 0)
                    {
                        ddlMonth.DataSource = months;
                        ddlMonth.DataTextField = Resources.DataFieldRes.BudgetMonthName;
                        ddlMonth.DataValueField = Resources.DataFieldRes.BudgetMonthValue;
                        ddlMonth.DataBind();
                    }
                    ddlMonth.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
            }
        }
        #endregion

        #region Helper Methods
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

        #region WorkFlow Methods

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

            if (pid == 2)
                path += "?AMD=1";

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
                ucrWrkf.FillWorkFlowDetails();
            }
        }

        #endregion

        #endregion

        #region Page Control Enum
        enum ControlEnums
        {
            BUDGETLIST,
            FINYEAR,
            MONTH,
            PLANT,
            ADVPLANT,
            ACCNOCHANGE,
            ACCNAMECHANGE,
            ADDLIST,
            SELBUDGET,
            SELBUDGETLIST,
            BUDGETDELETE,
            EDIT,
            BUDGETDTLEDIT,
            BUDGETDTLDELET,
            HISTORYHDR,
            HISTORYDTL,
            IMPORT,
            BUDGETDTLUNDO,
            BUDGETHDR
        }
        #endregion

        #region Months
        public class Month
        {
            public int Value { get; set; }
            public string Name { get; set; }
        }
        private static List<Month> SetMonth()
        {
            List<Month> months = new List<Month>
        {
            new Month { Value = 1, Name = "Jan" },
            new Month { Value = 2, Name = "Feb" },
            new Month { Value = 3, Name = "Mar" },
            new Month { Value = 4, Name = "Apr" },
            new Month { Value = 5, Name = "May" },
            new Month { Value = 6, Name = "Jun" },
            new Month { Value = 7, Name = "Jul" },
            new Month { Value = 8, Name = "Aug" },
            new Month { Value = 9, Name = "Sep" },
            new Month { Value = 10, Name = "Oct" },
            new Month { Value = 11, Name = "Nov" },
            new Month { Value = 12, Name = "Dec" }
        };
            return months;
        }
        #endregion

        #region Amend
        private void FillAmendProcess()
        {
            ucrWrkf.ViewType = 1;
            ucrWrkf.Visible = true;
            ucrWrkf.RefID = 0;
            ucrWrkf.ViewAction();
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            ucrWrkf.FillWorkFlowDetails();
        }
        #endregion

        #region SetUIValuesToObject
        private Object SetUIValuesToObject(ControlEnums controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            BudgetBO objImportHeader;
            try
            {
                switch (controlType)
                {
                    case ControlEnums.IMPORT:
                        objImportHeader = new BudgetBO();
                        objImportHeader.BGH_YEAR = Convert.ToInt32(ddlFinYear.SelectedValue);
                        objImportHeader.BGH_BIZUNIT = currentUser.SBUID;
                        objImportHeader.UserPk = currentUser.PKUser;
                        objImportHeader.BGH_TYPE = chkForcast.Checked ? Convert.ToByte(1) : Convert.ToByte(0);
                        objImportHeader.BGH_VERSION = 0;
                        if (AmendPage)
                            objImportHeader.BGH_IS_AMEND = 1;
                        else
                            objImportHeader.BGH_IS_AMEND = 0;
                        objImportHeader.Details = new List<BudgetDetails>();
                        retObject = objImportHeader;
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
        #endregion

        #region Excel Upload
        /// <summary>
        /// Methode used to save the excel file
        /// </summary>
        private string SaveDetails(out string conStr, FileUpload fupUpload)
        {
            uploadPath = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Budget";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Budget\\";
            }
            else
            {
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Budget";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Budget\\";
            }

            FileInfo tempFileInfoObj;
            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
            string attachmentFileFormat = tempFileInfoObj.Extension;
            string FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
            conStr = CheckValidFileType(attachmentFileFormat);
            if (attachmentFileFormat.ToLower() != ".xls" && attachmentFileFormat.ToLower() != ".xlsx")
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(conStr))
            {
                fupUpload.SaveAs(uploadPath + FileName);
            }
            return uploadPath + FileName;
        }

        /// <summary>
        /// Check File is valid or not , using File Extension , if valid then get the connection string
        /// </summary>
        /// <param name="extn"></param>
        /// <returns>bool : True - valid File, false - Invalid File</returns>
        private string CheckValidFileType(string extn)
        {
            string conStr;
            switch (extn.ToLower())
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.AppSettings["Excel03ConString"];
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.AppSettings["Excel07ConString"];
                    break;
                default: conStr = string.Empty; break;
            }
            return conStr;
        }

        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr)
        {
            try
            {
                StringBuilder sbImport = new StringBuilder();
                string SheetName = CommonConstants.EXELSHEETNAME.ToLower();
                excelColumns = airColums_General;
                conStr = String.Format(conStr, filePath);
                connExcel = new OleDbConnection(conStr);
                cmdExcel = new OleDbCommand();
                oleDbDataAdapter = new OleDbDataAdapter();
                dtExcelSchema = new DataTable();
                dsImportdata = new DataSet();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                {
                    if (dtExcelSchema != null)
                    {
                        landingSheet = string.Empty;
                        foreach (DataRow dr in dtExcelSchema.Rows)
                        {
                            if (dr["TABLE_NAME"].ToString().ToLower() == SheetName.ToLower())
                            {
                                landingSheet = dr["TABLE_NAME"].ToString();
                            }
                        }
                        if (landingSheet == string.Empty)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_ExelsheetName").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, SheetName.Replace("$", ""));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                    }
                    cmdExcel.CommandText = "SELECT * From [" + landingSheet + "]";
                    oleDbDataAdapter.SelectCommand = cmdExcel;
                    oleDbDataAdapter.Fill(dsImportdata, "Landing");
                    connExcel.Close();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    throw new Exception(GetLocalResourceObject("Err_IncorrectFormat").ToString());
                }
                if (dsImportdata != null && dsImportdata.Tables.Count > 0)
                {

                    foreach (DataColumn item in dsImportdata.Tables[0].Columns)
                    {
                        item.ColumnName = item.ColumnName.Replace(" ", "");
                    }
                    var existingColumns = (from p in excelColumns
                                           where this.dsImportdata.Tables[0].Columns.Contains(p)
                                           select p).ToList();
                    int cnt = (from p in excelColumns
                               where this.dsImportdata.Tables[0].Columns.Contains(p)
                               select p).Count();
                    if (cnt != excelColumns.Length)
                    {
                        sb = new StringBuilder();
                        sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                        if (excelColumns != null && excelColumns.Count() > 0)
                        {
                            foreach (string item in excelColumns)
                            {
                                sb.Append("<ul><li>" + item + "</li></ul>");
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                           + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }

                    DataTable dtImportData = dsImportdata.Tables[0];
                    if (!AmendPage)
                    {
                        if (ObjList != null && ObjList.Details != null && ObjList.Details.Count == 0 && ObjList.BGH_PK == 0)
                        {
                            ObjList = new BudgetBO();
                            ObjList = (BudgetBO)SetUIValuesToObject(ControlEnums.IMPORT);
                        }
                        ObjList.BGH_IS_AMEND = 0;
                    }
                    else
                    {
                        ObjList.Details = new List<BudgetDetails>();
                        ObjList.BGH_IS_AMEND = 1;
                    }
                    List<BudgetDetails> objDetList = new List<BudgetDetails>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    int BudGetSlNo = 0;
                    List<string> months = new List<string>
                    {
                        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                    };
                    decimal Budget = 0;
                    float BudgetTotal = 0;
                    int slno = 0;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        for (int j = 0; j < months.Count; j++)
                        {
                            DataRow row = dtImportData.Rows[i];

                            string SlNo = Convert.ToString(dtImportData.Rows[i]["SlNo"]);
                            if (!string.IsNullOrEmpty(SlNo.Trim()))
                            {
                                bool Isexist = false;

                                Budget = row[months[j]] == DBNull.Value ? 0 : Convert.ToDecimal(row[months[j]]);

                                if (ObjList.Details.Any(x => x.BDG_PLANT_CODE == HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["Plant"]).Trim()) && x.BDG_COST_CENTER_NAME == HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["CostCenter"]).Trim()) && x.BDG_ACCOUNT_NO == HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["CODEA/C"]).Trim()) && x.BDG_MONTH_TEXT == HttpUtility.HtmlEncode(months[j])))
                                {
                                    Isexist = true;
                                    slno = ObjList.Details.SingleOrDefault(x => x.BDG_PLANT_CODE == HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["Plant"]).Trim()) && x.BDG_COST_CENTER_NAME == HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["CostCenter"]).Trim()) && x.BDG_ACCOUNT_NO == HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["CODEA/C"]).Trim()) && x.BDG_MONTH_TEXT == HttpUtility.HtmlEncode(Convert.ToString(months[j]))).BDG_SL_NO;
                                }
                                if (Isexist)
                                {
                                    if (AmendPage)
                                        ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == slno).BDG_AMEND_AMOUNT = (float)Budget;
                                    else
                                        ObjList.Details.SingleOrDefault(x => x.BDG_SL_NO == slno).BDG_AMOUNT = (float)Budget;
                                    continue;
                                }
                                BudgetDetails objDetails = new BudgetDetails();

                                if (ObjList.Details.Count > 0)
                                    BudGetSlNo = ObjList.Details.Max(x => x.BDG_SL_NO);

                                objDetails.BDG_SL_NO = BudGetSlNo + 1;
                                objDetails.BDG_YEAR = ObjList.BGH_YEAR;
                                if (importColumns.Contains("Plant"))
                                {
                                    objDetails.BDG_PLANT_CODE = Convert.ToString(dtImportData.Rows[i]["Plant"]).Trim();
                                }
                                if (importColumns.Contains("CostCenter"))
                                {
                                    objDetails.BDG_COST_CENTER_NAME = Convert.ToString(dtImportData.Rows[i]["CostCenter"]).Trim();
                                }
                                if (importColumns.Contains("CODEA/C"))
                                {
                                    objDetails.BDG_ACCOUNT_NO = Convert.ToString(dtImportData.Rows[i]["CODEA/C"]).Trim();
                                }
                                if (importColumns.Contains("DESCRIPTION"))
                                {
                                    objDetails.BDG_ACCOUNT_NAME = Convert.ToString(dtImportData.Rows[i]["DESCRIPTION"]).Trim();
                                }
                                if (importColumns.Contains(months[j]))
                                {
                                    if (AmendPage)
                                        objDetails.BDG_AMEND_AMOUNT = (float)Budget;
                                    else
                                        objDetails.BDG_AMOUNT = (float)Budget;


                                    objDetails.BDG_MONTH_TEXT = HttpUtility.HtmlEncode(months[j]);

                                }
                                if (!string.IsNullOrEmpty(ObjList.BGH_YEAR.ToString()))
                                {
                                    ObjList.Details.Add(objDetails);
                                }

                            }
                            slno = slno + 1;
                        }
                    }
                }
                if (ObjList != null && ObjList.Details != null && ObjList.Details.Count > 0)
                {
                    if (AmendPage)
                    {
                        AmendAdd = true;
                        SaveBudget(Convert.ToInt32(WorkflowTransactionFlag.SAVE), true);
                        GetFieldValues(ControlEnums.SELBUDGETLIST);
                    }
                    SetFieldValues(ControlEnums.ADDLIST);
                    if (ObjList.Details.Count > 0)
                    {
                        float importBudget = ObjList.Details.Sum(x => x.BDG_AMOUNT);
                        lblBudgetTotal.Text = importBudget.ToString("N2");
                    }
                }
            }
            catch (OleDbException ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
            catch (Exception ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        #endregion

        #endregion
    }
}