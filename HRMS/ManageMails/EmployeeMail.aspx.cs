using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using BusinessObject;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using System.Xml;
using BusinessObject.CommonManagement;
using CustomControls;
using BusinessLogic.CommonManagement;
using BusinessLogic.BrandRates;
using BusinessObject.Mailer;
using BusinessLogic.Mailer;
using ERPSMS_v01.UserControls;
using MailSendCore;
using System.Globalization;
using BusinessObject.Common;
using BusinessLogic.HRMS.ManageMails;
using HRMS.BackgroundTasks;

namespace HRMS.ManageMails
{
    public partial class EmployeeMail : ERP.Store.UI.MyBasePage
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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);

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
                return this.ViewState[ViewstateStrings.CurrPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int CusPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CusPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CusPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CusPK] = value;
            }
        }
        /// <summary>
        /// Item PK
        /// </summary>
        private int ItemPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ItemPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemPK] = value;
            }
        }

        /// <summary>
        /// SelectedPK PK-- Used to keep the selected pk from a grid
        /// </summary>
        private int SelectedPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }
        /// <summary>
        /// To maintain Page Size
        /// </summary>
        private int PageSize
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageSize] == null ? 0 : (int)this.ViewState[ViewstateStrings.PageSize];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageSize] = value;
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
        /// To maintain the From Date in viewstate
        /// </summary>
        private DateTime FromDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.FromDate] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.FromDate];

            }
            set
            {
                this.ViewState[ViewstateStrings.FromDate] = value;
            }
        }

        /// <summary>
        /// To maintain the To Date in viewstate
        /// </summary>
        private DateTime ToDate
        {
            get
            {
                return this.ViewState[ViewstateStrings.ToDate] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.ToDate];

            }
            set
            {
                this.ViewState[ViewstateStrings.ToDate] = value;
            }
        }

        /// <summary>
        /// Keep Customer Rate Dataset
        /// </summary>
        private DataSet CustomerRates
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerRates] == null ? null : (DataSet)this.ViewState[ViewstateStrings.CustomerRates];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerRates] = value;
            }

        }
        /// <summary>
        /// Keep Customer Mails
        /// </summary>
        private DataTable CustomerMails
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerMails] == null ? null : (DataTable)this.ViewState[ViewstateStrings.CustomerMails];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerMails] = value;
            }
        }
        /// <summary>
        /// Customer List
        /// </summary>
        private List<CustomerBO> CustomerEmailList
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerEmailList] == null ? null : (List<CustomerBO>)this.ViewState[ViewstateStrings.CustomerEmailList];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerEmailList] = value;
            }

        }
        /// <summary>
        /// Status
        /// </summary>
        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.Status]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }
        /// <summary>
        /// Keep Currency List in viewstate
        /// </summary>
        private DataTable Currency
        {
            get
            {
                return this.ViewState[ViewstateStrings.Currency] == null ? null : (DataTable)this.ViewState[ViewstateStrings.Currency];
            }
            set
            {
                this.ViewState[ViewstateStrings.Currency] = value;
            }

        }
        /// <summary>
        /// Brand PK
        /// </summary>
        private int BrkPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.BrkPK] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.BrkPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BrkPK] = value;
            }
        }


        #endregion
        private BackgroundTaskService MailQ;
        User currentUser;
        private ActionsEnum commonActions;
        DataTable dtCustomers;
        DataTable dtCustomerMails;
        MailDetailsBO mailDetailsObj;
        DataSet dsPageData;
        DataTable dtPageData;
        XmlDocument xmlDoc;
        DataTable dtMailList;
        bool mailSelecte = false;
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
                    EntryStatus = EntryStatus.LISTMODE;
                    Status = (int)MailStatus.SEND;
                    PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    PageSize = Convert.ToInt32(grdMailList.PageSize);
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
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


        /// <summary>
        /// Get the User Rights, Checks Page Level Rights, 
        /// Hides sections in which user don't have access rights
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!IsPostBack)
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
        /// 
        private void GetFieldValues(ControlsEnum type)
        {          
            try
            {
                switch (type)
                {                   
                    #region Default
                    case ControlsEnum.DEFAULT:
                        int employeePk = string.IsNullOrEmpty(hdfEmployee.Value.Trim()) ? 0 : Convert.ToInt32(hdfEmployee.Value);
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);                       
                        dtPageData = EmployeeMailBL.GetEmpMailQList(new BusinessObject.GridPrams()
                        {
                            PageSize = this.PageSize,
                            PageNumber = Convert.ToInt32(this.PageIndex),
                            FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                            ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim()
                        },
                           employeePk, Status);
                        break;
                    #endregion                   
                }
            }
            catch
            {
                throw;
            }
            finally
            {

            }
        }       
        private void SetFieldValues(ControlsEnum type)
        {         
            switch (type)
            {               
                case ControlsEnum.DEFAULT:
                    BindGrid(ControlsEnum.DEFAULT);                   
                    break;
            }
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
            bool IsChecked = false;
            int result;
            result = 0;          
            try
            {
                //Get Action from CommandName
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                   
                    #region Search
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);                      
                        break;
                    #endregion                  
                  
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ClearForm(ControlsEnum.CLEARALL);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion                  
                   
                    #region Send
                    case ActionsEnum.SEND:
                        //Save Details
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            object[] lstFields = new object[] { "EPM_PK", "EPM_EPS_PK", "EPM_EMAIL", "EPM_IS_GENERATED", "EPM_SUBJECT", "EPM_CONTENT", "EPM_DATE", "EPM_FILE_NAME", "EPM_FROM_EMAIL" };
                            dtMailList = GTIService.CommonFunctions.CreateDataTable(lstFields);
                            foreach (GridViewRow grdrow in grdMailList.Rows)
                            {
                                CheckBox chk;
                                chk = (CheckBox)grdrow.FindControl("chkSelectMailList");
                                if (chk.Checked)
                                {
                                    IsChecked = true;
                                    DataRow dr;
                                    dr = dtMailList.NewRow();
                                    dr["EPM_PK"] = ((HiddenField)grdrow.FindControl("hdfEPM_PK")).Value;
                                    dr["EPM_EPS_PK"] = ((HiddenField)grdrow.FindControl("hdfEPM_EPS_PK")).Value;
                                    dr["EPM_EMAIL"] = ((Label)grdrow.FindControl("lblName")).Text;
                                    dr["EPM_IS_GENERATED"] = 0;
                                    dr["EPM_SUBJECT"] = ((Label)grdrow.FindControl("lblSubject")).Text;
                                    dr["EPM_CONTENT"] = ((Label)grdrow.FindControl("lblContent")).Text;
                                    dr["EPM_DATE"] = ((Label)grdrow.FindControl("lblDate")).Text;
                                    dr["EPM_FILE_NAME"] = ((HiddenField)grdrow.FindControl("hdfEPM_FILE_NAME")).Value;
                                    dr["EPM_FROM_EMAIL"] = ((HiddenField)grdrow.FindControl("hdfEPM_FROM_EMAIL")).Value;
                                    dtMailList.Rows.Add(dr);
                                }
                            }
                            if (IsChecked)
                            {
                                if (dtMailList != null && dtMailList.Rows.Count > 0)
                                {
                                    StartMailBackgroundTask(dtMailList);
                                    ResetForm();
                                    //Show Save success message and reset 
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }

                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        #endregion

        #region --- For Grid Actions----

        /// <summary>
        /// Handling Grid events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            string arg;
            ExtGridViewRow gvr;
            GridView grd;          
            try
            {
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Sorting Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            SortBy = e.SortExpression;
            if (SortDirection == Resources.Report.SortAscending)
                SortDirection = Resources.Report.SortDescending;
            else
                SortDirection = Resources.Report.SortAscending;

            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
        }

        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
        }

        #endregion

        #region Helper Methods 
        /// <summary>
        ///  Method for Bind Grid
        /// </summary>
        public void BindGrid(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.DEFAULT:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        if (dtPageData.Rows.Count > 0)
                        {
                            decimal pages = Convert.ToDecimal(Convert.ToDecimal(dtPageData.Rows[0]["TOTAL_ROW_COUNT"].ToString()) / Convert.ToDecimal(grdMailList.PageSize.ToString()));                            
                            TotalPages = Convert.ToInt32(Math.Ceiling(pages));
                        }
                        else
                            TotalPages = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        PageIndex = PageIndex == null ? "1" : PageIndex;
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdMailList.PageIndex = Convert.ToInt32(PageIndex);
                        grdMailList.DataSource = dtPageData;
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                    }
                    else
                    {
                        PageIndex = "1";
                        TotalPages = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                        uclPaging.TotalPages = TotalPages;
                        uclPaging.Visible = false;
                        uclPaging.BindPager();
                        grdMailList.DataSource = null; ;
                    }
                    grdMailList.DataBind();
                    break;               
            }
        }
        private void ClearForm(ControlsEnum type)
        {
            switch (type)
            {
                case ControlsEnum.CLEARALL:
                    LastModifiedTime = System.DateTime.Now;
                    lblLastModifiedHDR.Text = string.Empty;
                    EntryStatus = EntryStatus.LISTMODE;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = "0";
                    ddlStatus.SelectedValue = "-1";  
                    CurrPK = 0;                
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);                   
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    break;               
            }
        }      
        /// <summary>
        /// Reset Form
        /// </summary>
        private void ResetForm()
        {
            ClearForm(ControlsEnum.CLEARALL);          
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EntryStatus = EntryStatus.LISTMODE;           
        }

        /// <summary>
        /// Method to start mail service
        /// </summary>
        private void StartMailBackgroundTask(DataTable dtList)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (MailQ == null || !MailQ.IsStarted)
            {
                if (MailQ == null)
                {
                    MailQ = new BackgroundTaskService();
                }
                MailQ.dtPaySlipMailList = dtList;
                MailQ.FromExternalSource = true;
                MailQ.currentUser = currentUser;
                MailQ.StartBackgroundService();
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
            string s = "";
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


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
                        // Assign the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assign the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the current page index.
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
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

        }
        /// <summary>
        /// Method used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EntryMode", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);

                }              
                if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EntryMode", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(4);});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ListMode", "$(document).ready(function(){ShowListing(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                }
                else if (EntryStatus == EntryStatus.LISTDRAFTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ListMode", "$(document).ready(function(){ShowListing(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Enum
        /// <summary>
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {

        }

        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            CLEARALL,
            CUSTOMERS,
            CUSTOMERSEDIT,
            CUSTOMEREMAIL,
            MAILDTL,
            TYPE,
            VIEW,
            MAILATTACHMENT
        }

        private enum MailStatus
        {
            DRAFT = 4,
            SEND = 0
        }
        #endregion
    }
}