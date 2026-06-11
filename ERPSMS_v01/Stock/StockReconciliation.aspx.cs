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
using BusinessObject.Stock;

using ERPSMS_v01.UserControls;
using System.Xml;
using BusinessObject.StoreManagement;
using System.IO;
using BusinessLogic.Stock;

namespace ERPSMS_v01.Stock
{
    public partial class StockReconciliation : ERP.Store.UI.WorkFlowBasePage
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
        /// Stock initialization Pk
        /// </summary>
        private int StockInitPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.StockInitPk] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.StockInitPk]));
            }
            set
            {
                this.ViewState[ViewstateStrings.StockInitPk] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
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
        /// Managing tab visibility 
        /// </summary>
        private int DisplayTab
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisplayTab] == null ? 1 : (int)(this.ViewState[ViewstateStrings.DisplayTab]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisplayTab] = value;
            }
        }



        #endregion

        private BusinessObject.User currentUser;
        private DataTable dtResult; 
        private ActionsEnum commonActions; 
        ReconcileEnum reclEnum;
        #endregion

        #region PageEvents
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e); 
        }

        #region Page_PreRender
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true); 
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs('" + DisplayTab + "');", true); 
        }
        #endregion
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

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                InitializeComponent();
                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.STOCKTAKEINITIALIZATION);
                    SetFieldValues(ControlsEnum.STOCKTAKEINITIALIZATION);
                    ActionHandler(lbnLocationChange, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
                int? result = null;
                #region Getting Command Action
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

                #endregion
                switch (commonActions)
                {
                    case ActionsEnum.LOCATIONCHANGE:
                        reclEnum = ReconcileEnum.LocationChange;
                        DisplayTab = (int)reclEnum;
                        GetFieldValues(ControlsEnum.STOCKRECONCILIATION);
                        BindGrid(reclEnum);
                        break;
                    case ActionsEnum.STORECHANGE:
                        reclEnum = ReconcileEnum.StoreChange;
                        DisplayTab = (int)reclEnum;
                        GetFieldValues(ControlsEnum.STOCKRECONCILIATION);
                        BindGrid(reclEnum);
                        break;
                    case ActionsEnum.ADDITION:
                        reclEnum = ReconcileEnum.Addition;
                        DisplayTab = (int)reclEnum;
                        GetFieldValues(ControlsEnum.STOCKRECONCILIATION);
                        BindGrid(reclEnum);
                        break;
                    case ActionsEnum.DELETION:
                        reclEnum = ReconcileEnum.Deletion;
                        DisplayTab = (int)reclEnum;
                        GetFieldValues(ControlsEnum.STOCKRECONCILIATION);
                        BindGrid(reclEnum);
                        break;
                    case ActionsEnum.QTYCHANGE:
                        reclEnum = ReconcileEnum.QtyChange;
                        DisplayTab = (int)reclEnum;
                        GetFieldValues(ControlsEnum.STOCKRECONCILIATION);
                        BindGrid(reclEnum);
                        break;
                    case ActionsEnum.RECONSILE:

                        break;
                    case ActionsEnum.PRINT: 
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + StockInitPk + "&APPTYPE=" + ApplicationType.BSRC + "&APPSUBTYPE=" + "&DISPLAYTYPE=" + DisplayTab) + "');", true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
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
                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EnableDisableButtons(e.TotalPages, "uclPaging");
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            //  EntryStatus = EntryStatus.LISTMODE;
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
            dtResult = null;
            try
            {
                switch (type)
                {
                    #region STOCK TAKE INITIALIZATION
                    case ControlsEnum.STOCKTAKEINITIALIZATION:
                        dtResult = StockBL.GetActiveRecord(currentUser.SBUID);
                        break;
                    #endregion
                    #region STOCK RECONCILIATION
                    case ControlsEnum.STOCKRECONCILIATION:
                        dtResult = StockBL.GetStockReconciliation(StockInitPk, (int)reclEnum);
                        break;
                    #endregion
                    default:
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
                    #region STOCKTAKEINITIALIZATION
                    case ControlsEnum.STOCKTAKEINITIALIZATION:
                        GetUIValuesFromObject(ControlsEnum.STOCKTAKEINITIALIZATION);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                default:
                    break;
            }
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ReconcileEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ReconcileEnum.LocationChange:
                        grdLocationChange.DataSource = dtResult;
                        grdLocationChange.DataBind();
                        break;
                    case ReconcileEnum.StoreChange:
                        grdStoreChange.DataSource = dtResult;
                        grdStoreChange.DataBind();
                        break;
                    case ReconcileEnum.QtyChange:
                        grdQtyChange.DataSource = dtResult;
                        grdQtyChange.DataBind();
                        break;
                    case ReconcileEnum.Addition:
                        grdAddition.DataSource = dtResult;
                        grdAddition.DataBind();
                        break;
                    case ReconcileEnum.Deletion:
                        grdDeletion.DataSource = dtResult;
                        grdDeletion.DataBind();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        #region "GetUIValuesFromObject"
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region STOCKTAKEINITIALIZATION
                    case ControlsEnum.STOCKTAKEINITIALIZATION:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtDate.Text = Convert.ToDateTime(dtResult.Rows[0]["BIT_DATE"]).ToString(CommonConstants.DATEFORMAT);
                            StockInitPk = Convert.ToInt32(dtResult.Rows[0]["BIT_PK"]);
                        }
                        break;
                    #endregion
                    default:
                        break;
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
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            switch (controlType)
            {
                default:
                    break;
            }
            return returnObject;
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                default:
                    break;
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

        #region UtitlityMethods
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
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            CLEAR,
            LIST,
            SAVE,
            STOCKTAKEINITIALIZATION,
            STOCKRECONCILIATION
        }
        #endregion
        #region TabEnum
        public enum TabEnum
        {
            PRODUCTDETAILS = 1,
            SUBTYPEPRODUCTS = 2,
            SUBGRADEPRODUCTS = 3,
            PACKINGMATERIALS = 4
        }
        #endregion
        #region TabEnum
        public enum ReconcileEnum
        {
            LocationChange = 1,
            StoreChange = 2,
            QtyChange = 3,
            Addition = 4,
            Deletion = 5
        }
        #endregion
    }
}