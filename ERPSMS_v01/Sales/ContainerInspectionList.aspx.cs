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
using ERPData;
using ERPManager;
using ERPService.Sales;
using BusinessObject.CommonManagement;
using ERPSMS_v01.UserControls;
using BusinessObject.Sales;

namespace ERPSMS_v01
{
    public partial class ContainerInspectionList : System.Web.UI.Page
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
        /// To maintain the sort expression in viewstate
        /// </summary>
        private string SortExpression
        {
            get
            {
                return (string)this.ViewState["SortExpression"];
            }
            set
            {
                this.ViewState["SortExpression"] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;

        //page related class objects      
        private SAL_CONTAINER_INSP_HDR admSalContainerInspHdrObj;
        private ServiceUtility serviceUtilityObj;

        private List<SAL_CONTAINER_INSP_HDR> admSalContainerInspHdrList;

        private BusinessObject.User currentUser;
      
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
                if (!IsPostBack)
                {

                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();

                    ResetMode();

                    BindStatusDropDown();

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.CSHPK;
                    grdContainerInspectionList.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;

                    hdfAppType.Value = ApplicationType.CNTINSP;
                    hdfAppSubType.Value = string.Empty;
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
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            ContainerInspectionService ContainerInspectionServiceClient = null;
            try
            {
                ContainerInspectionServiceClient = new ContainerInspectionService();
                ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                admSalContainerInspHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_INSP_HDR>();
                switch (type)
                {

                    case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdContainerInspectionList.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CSHPK : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortDescending : SortDirection;
                        admSalContainerInspHdrObj.CSH_PK = 0;
                        admSalContainerInspHdrObj.CSH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admSalContainerInspHdrList = ContainerInspectionServiceClient.GetContainerInspectionList(admSalContainerInspHdrObj, serviceUtilityObj);
                        if (admSalContainerInspHdrList != null && admSalContainerInspHdrList.Count > 0)
                        {
                            if (txtSealno.Text.Trim() != string.Empty)
                            {
                                admSalContainerInspHdrList = admSalContainerInspHdrList.Where(a => a.CSH_SEAL_NO.StartsWith(txtSealno.Text.Trim())).ToList();
                            }
                            if (txtFromDate.Text.Trim() != string.Empty && txtToDate.Text.Trim() != string.Empty)
                            {
                                admSalContainerInspHdrList = admSalContainerInspHdrList.Where(a => a.CSH_DATE >= Convert.ToDateTime(txtFromDate.Text) && a.CSH_DATE <= Convert.ToDateTime(txtToDate.Text)).ToList();
                            }
                            else if (txtFromDate.Text.Trim() != string.Empty)
                            {
                                admSalContainerInspHdrList = admSalContainerInspHdrList.Where(a => a.CSH_DATE >= Convert.ToDateTime(txtFromDate.Text)).ToList();
                            }
                            else if (txtToDate.Text.Trim() != string.Empty)
                            {
                                admSalContainerInspHdrList = admSalContainerInspHdrList.Where(a => a.CSH_DATE <= Convert.ToDateTime(txtToDate.Text)).ToList();
                            }
                            if (Convert.ToInt32(ddlstatus.SelectedValue) >= 0)
                            {
                                admSalContainerInspHdrList = admSalContainerInspHdrList.Where(a => a.CSH_STATUS == Convert.ToInt32(ddlstatus.SelectedValue)).ToList();
                            }

                        }
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    case ControlsEnum.SEARCH:
                                          
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admSalContainerInspHdrObj = null;
                ContainerInspectionServiceClient = null;
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
                    case ControlsEnum.DEFAULT:
                        
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SEARCH:
                        //BindGrid(controlType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
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

            switch (commonActions)
            {
                case ActionsEnum.NEW:
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionCreate), false);
                    break;
                case ActionsEnum.EDIT:
                    SetUIEditView(ActionsEnum.EDIT);
                    break;
                case ActionsEnum.VIEW:
                    SetUIEditView(ActionsEnum.VIEW);
                    break;
                case ActionsEnum.SEARCH:
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
                    break;
                case ActionsEnum.CLEAR:
                    ResetForm();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;
                    break;

                case ActionsEnum.PRINT:
                    SetUIEditView(ActionsEnum.PRINT);
                    break;
            }
        }

        #endregion
    

        #region gridActions

        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
        }


        /// <summary>
        /// Sorting Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            //Type type = typeof(AircraftTypes);
            //FieldInfo fieldInfo = type.GetField(e.SortExpression);
            //FieldInfo fieldInfo = (Type)e.SortExpression;
            //SortExpression = Convert.ToString(fieldInfo.GetValue(0));
            //SortExpression = e.SortExpression;
            SortBy = e.SortExpression;
            if (SortDirection == Resources.Report.SortAscending)
                SortDirection = Resources.Report.SortDescending;
            //SortOrder = "desc";
            else
                SortDirection = Resources.Report.SortAscending;
            //SortOrder = "asc";
            //ResetForm();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
        }

        /// <summary>
        /// Row data bound Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {


                    //Label lblDate = e.Row.FindControl("lblDate") as Label;
                    //lblDate.Text = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CSHDATE).ToString()).ToString("dd-MMM-yyyy");
                    //lblDate.ToolTip = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CSHDATE).ToString()).ToString("dd-MMM-yyyy");


                    Label lblNo = e.Row.FindControl("lblNo") as Label;
                    lblNo.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CSHNO).ToString() == "" ? Resources.Messages.DocGenerationNew : DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CSHNO).ToString();


                    Label lblStatus = e.Row.FindControl("lblStatus") as Label;
                    lblStatus.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CSHSTATUS).ToString() == ((int)WorkFlowStatus.DRAFT).ToString() ? WorkFlowStatusText.DRAFT : WorkFlowStatusText.APPROVED;
                    lblStatus.ToolTip = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CSHSTATUS).ToString() == ((int)WorkFlowStatus.DRAFT).ToString() ? WorkFlowStatusText.DRAFT : WorkFlowStatusText.APPROVED;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

       

        

        #endregion
        #endregion

        #region Helper Methods
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        /// <param name="mode"></param>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdContainerInspectionList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        Session[ERP.Utilities.SessionStrings.ContainerInspectionPK] = Convert.ToInt32(grdContainerInspectionList.DataKeys[grdrow.RowIndex].Values[0]);
                        switch (mode)
                        {
                            case ActionsEnum.EDIT:
                                Session[ERP.Utilities.SessionStrings.ContainerInspectionMode] = EntryStatus.ENTRYMODE;
                                break;
                            case ActionsEnum.VIEW:
                                Session[ERP.Utilities.SessionStrings.ContainerInspectionMode] = EntryStatus.VIEWMODE;
                                break;
                            case ActionsEnum.PRINT:
                                int CurrentPK = Session[ERP.Utilities.SessionStrings.ContainerInspectionPK] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ContainerInspectionPK]) : 0;
                                // Response.Redirect("../Reports/GenerateReport.aspx?ID=" + CurrentPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrentPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "');", true);
                                return;
                                break;
                        }
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ContainerInspectionCreate), false);
                          
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch
            {
                throw;
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
                    case ControlsEnum.DEFAULT:
                        if (admSalContainerInspHdrList != null )
                        {
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            grdContainerInspectionList.PageIndex = Convert.ToInt32(PageIndex);
                            grdContainerInspectionList.DataSource = admSalContainerInspHdrList;
                            grdContainerInspectionList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
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
        /// Method for Bind Status Dropdown
        /// </summary>
        private void BindStatusDropDown()
        {
            ddlstatus.Items.Clear();
            ddlstatus.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            ddlstatus.Items.Insert(1, new ListItem(WorkFlowStatusText.DRAFT, ((int)WorkFlowStatus.DRAFT).ToString()));
            ddlstatus.Items.Insert(2, new ListItem(WorkFlowStatusText.APPROVED, ((int)WorkFlowStatus.APPROVED).ToString()));

        }

        private void ResetMode()
        {
            Session[ERP.Utilities.SessionStrings.ContainerInspectionPK] = null;
            Session[ERP.Utilities.SessionStrings.ContainerInspectionMode] = null;
        }

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            ddlstatus.SelectedValue = CommonConstants.SELECTVAL;
            txtSealno.Text = string.Empty;
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
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {

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
         //<summary>
         //Action Handlers For Pager Control
         //</summary>
         //<param name="sender"></param>
         //<param name="e"></param>
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);

              
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            SEARCH
        }

        #endregion
    }
}