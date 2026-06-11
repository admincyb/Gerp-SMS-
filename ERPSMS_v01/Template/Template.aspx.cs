#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using System.IO;
#endregion

namespace gERPDiscrete.UI
{
    public partial class Template : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties

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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

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

        #endregion

        #region Variables
        BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        RadioButton rbtn;
        DataTable dtPageData;
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
            try
            {
                if (!IsPostBack)
                {

                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);

                    //string[] itemkeyarray;
                    //itemkeyarray = new string[1];
                    //itemkeyarray[0] = "SCD_SEQUENCE";
                    //grdUploads.DataKeyNames = itemkeyarray;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string getxml;
            try
            {
                switch (type)
                {
                    #region Listing
                    case ControlsEnum.LIST:
                        //get the Shipping Agent
                        //dtPageData = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(0, cusPk, 1, (int)CustomerAddressType.ShippingAgent).Tables[0];
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
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;

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
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region WORKFLOW STATUS
                //case ControlsEnum.STATUS:
                //    //fill the Work flow status
                //    ddlStatus.Items.Clear();
                //    if (workflowStatusList != null && workflowStatusList.Count > 0)
                //    {
                //        ddlStatus.DataSource = workflowStatusList;
                //        ddlStatus.DataTextField = "ASC_NAME";
                //        ddlStatus.DataValueField = "ASC_VALUE";
                //        ddlStatus.DataBind();
                //    }
                //    ddlStatus.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //    break;
                #endregion
                default:
                    break;
            }
        }

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
                //BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (controlType)
                {
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
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = new DataTable();
            //   dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                // BizUnitConfigValue = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "SHIPPING")["ACF_VALUE"].ToString());
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

                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    #region  LIST
                    case ControlsEnum.LIST:
                        //uclPaging.TotalPages = TotalPages;
                        //PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        //uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        //grdShippingPlanList.DataSource = dsPageData.Tables[1];
                        //grdShippingPlanList.DataBind();
                        //uclPaging.Visible = true;
                        //uclPaging.BindPager();
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
        private void ResetForm()
        {
            CurrPK = 0;
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
                GridViewRow gvr;
                GridView grd;
                string arg;
                bool bIsChecked = false;
                int result;
                string action;
                int selectedItemPK;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
              
                //switch (commonActions)
                switch (commonActions)
                {
                   
                    #region extra grid ondemand data population
                    // Do Action for , when click btnOrderDetails button
                    case ActionsEnum.SODETAILS:
                        ////For SC Details
                        //arg = ((Button)sender).CommandArgument;
                        //gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        //if (gvr != null)
                        //{
                        //    grd = gvr.FindControl("grdOrderDetails") as GridView;
                        //    if (string.IsNullOrEmpty(arg))
                        //    {
                        //        dsSPDetails = null;
                        //    }
                        //    else
                        //    {
                        //        SPID = Convert.ToInt32(arg);
                        //        GetFieldValues(ControlsEnum.SPDEATILS);
                        //    }
                        //    grd.Visible = true;
                        //    if (dsSPDetails != null && dsSPDetails.Tables[0].Rows.Count > 0)
                        //    {
                        //        grd.DataSource = dsSPDetails.Tables[0];
                        //        grd.DataBind();
                        //    }
                        //    (gvr.FindControl("hdfIsExpandedOrders") as HiddenField).Value = "1";
                        //    int DelStatus = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfDeleteStatus")).Value);
                        //    if (DelStatus == 1)
                        //    {
                        //        btnEditforCancel.Visible = false;
                        //    }
                        //    else
                        //    {
                        //        btnEditforCancel.Visible = true;
                        //    }
                        //}
                        break;

                    #endregion

                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            //   litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {

                        }
                        break;
                    #endregion
                    #region Cancel
                    // Do Action for , when click cancel button
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region Clear
                    // Do Action for , when click clear button
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {

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
                    //if (SortDirection == Resources.Report.SortAscending)
                    //    SortDirection = Resources.Report.SortDescending;
                    //else
                    //    SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    //  SortDirection = Resources.Report.SortAscending;

                }
                this.PageIndex = "1";
                //GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                //SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
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
            int slno;
            try
            {
                if (((GridView)sender).ID == "grdShippingList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                    }
                }
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
            // uclPaging.CurrentPage = 1;
        }


        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            //this.Init += new EventHandler(this.Page_Init);
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
                    //case NavigationEnum.PAGECHANGE:
                    //    uclPaging.CurrentPage = e.CurrentPage;
                    //    break;
                    //case NavigationEnum.FIRST:
                    //    // Assignment the first page index.
                    //    if (e.CurrentPage > 1)
                    //        uclPaging.CurrentPage = 1;
                    //    break;
                    //case NavigationEnum.LAST:
                    //    // Assignment the last page index.
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPaging.CurrentPage = e.TotalPages;
                    //    break;
                    //case NavigationEnum.NEXT:
                    //    // Increment the next page index.
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPaging.CurrentPage++;
                    //    break;
                    //case NavigationEnum.PREVIOUS:
                    //    // Decrement the previous page index.
                    //    if (e.CurrentPage > 1)
                    //        uclPaging.CurrentPage--;
                    //    break;
                }
                //PageIndex = uclPaging.CurrentPage.ToString();
                //GetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                //SetFieldValues(ControlsEnum.SHIPPINGPLANHDR);
                //EnableDisableButtons(e.TotalPages);
                //EntryStatus = EntryStatus.LISTMODE;

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
            //// Should we disable the first link
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we disable the previous link
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we enable the next link
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //// Should we enable the last link
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    //lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    //lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowUploadDocDetails", "$(document).ready(function(){ShowHideManageResource();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST
        }

        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 1,
            APPROVED = 2,
            NEW = 0
        }
        #endregion
    }
}