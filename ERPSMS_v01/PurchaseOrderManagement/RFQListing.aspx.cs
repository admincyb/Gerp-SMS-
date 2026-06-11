using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using System.Data;
using ERP.Utilities;
using BusinessObject;
using BusinessObject.Common;
using System.Reflection;

using ERPSMS_v01.UserControls;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class RFQListing : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

        #region Properties
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
        /// <summary>
        /// To maintain the sort order in viewstate
        /// </summary>
        private string SortOrder
        {
            get
            {
                return (string)this.ViewState["SortOrder"];
            }
            set
            {
                this.ViewState["SortOrder"] = value;
            }
        }
        /// <summary>
        /// To maintain the sort field in viewstate
        /// </summary>
        private string SortFilter
        {
            get
            {
                return (string)this.ViewState["SortFilter"];
            }
            set
            {
                this.ViewState["SortFilter"] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        User currentUser;
        private DataSet dsPageData;
        private int listingRefID;
        private BusinessObject.User objUser;
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
                    Session[ERP.Utilities.SessionStrings.RFQPK] = null;
                    Session[ERP.Utilities.SessionStrings.PRSearchResult] = null;
                    Session[ERP.Utilities.SessionStrings.PRSelected] = null;
                    Session[ERP.Utilities.SessionStrings.RFQVendor] = null;
                    Session[ERP.Utilities.SessionStrings.RFQMODE] = null;
                    Session[ERP.Utilities.SessionStrings.RFQItemDetailParameters] = null;
                    Session[ERP.Utilities.SessionStrings.RFQResponseHeader] = null;
                    FillProcessId();
                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = Resources.DataFieldRes.RFQPK;
                    grdRFQList.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;

                    hdfAppType.Value = BusinessObject.CommonManagement.ApplicationType.QAC;
                    hdfAppSubType.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
            try
            {

                switch (type)
                {

                    case ControlsEnum.DEFAULT:
                        int deptPk = 0;
                        deptPk = string.IsNullOrEmpty(hdfDeptPk.Value) ? 0 : Convert.ToInt32(hdfDeptPk.Value);

                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.RFQPK : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage,
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"))
                            }, objUser, 0, currentUser.PKUser, deptPk);
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
                    case ControlsEnum.DEFAULT:
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
        #region Action Handlers

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            GridViewRow selectedGrdrow;
            HiddenField hdfDept;
            int dept;
            int selectedPK;
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
            switch (commonActions)
            {
                case ActionsEnum.NEW:
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQSearch), false);
                    break;
                case ActionsEnum.EDIT:
                    SetUIEditView(ActionsEnum.EDIT);
                    break;
                case ActionsEnum.ITEMSELECTED:
                    selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                    hdfDept = selectedGrdrow.FindControl("hdfDept") as HiddenField;
                    if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                    {
                        Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                        hdfRFQCurrentDepartment.Value = dept.ToString();
                        base.SetUserDept();
                    }
                    selectedPK = Convert.ToInt32(grdRFQList.DataKeys[selectedGrdrow.RowIndex].Values[0]);
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    listingRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                    break;
                case ActionsEnum.VIEW:
                    SetUIEditView(ActionsEnum.VIEW);
                    break;
                case ActionsEnum.RFQRESPONSE:
                    SetUIEditView(ActionsEnum.RFQRESPONSE);
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
                    int CurrPK = Convert.ToInt32(grdRFQList.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                    //Response.Redirect("../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "&ISEXCELPRINT=1" + "');", true);

                    break;
            }
        }


        #endregion

        #region --- For Grid Actions----


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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblNo = e.Row.FindControl("lblNo") as Label;
                lblNo.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.RFQNO).ToString() == "" ? Resources.Messages.DocGenerationNew : DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.RFQNO).ToString();
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (SortBy == e.SortExpression)
        //        {
        //            ////Toggle the sort expression
        //            //if (SortDirection == Resources.gComsRes.SortAscending)
        //            //    SortDirection = Resources.gComsRes.SortDescending;
        //            //else
        //            //    SortDirection = Resources.gComsRes.SortAscending;
        //        }
        //        else
        //        {
        //            //SortBy = e.SortExpression;
        //            //SortDirection = Resources.gComsRes.SortAscending;
        //        }

        //        this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}


        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewRowEventArgs e)
        //{
        //    int sthPK = 0;
        //    try
        //    {
        //        #region Grid Fixed Columns
        //        if ((sender as GridView).ID == "grdPoList")
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                Label lblVendor = e.Row.FindControl("lblVendor") as Label;
        //                Label lblShipping = e.Row.FindControl("lblShipping") as Label;
        //                if (PurOrderHdrList != null && PurOrderHdrList.Count > 0)
        //                {

        //                    //   lblVendor.Text = PurOrderHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_CODE;
        //                    ADM_DEPT_MST AdmDeptMstObj = new ADM_DEPT_MST();
        //                    //  lblShipping.Text = PurOrderHdrList[e.Row.RowIndex].ADM_DEPT_MST.DPT_NAME;
        //                }
        //            }
        //        }

        //        #endregion

        //        if ((sender as GridView).ID == "grdStockTransfer")
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                sthPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfStockTransferPK")).Value);
        //                Label lblTranTo = e.Row.FindControl("lblStTransferTo") as Label;

        //                List<INV_STK_TRAN_DTL> objList = InvStkTranHdrList[0].INV_STK_TRAN_DTL.Where(aa => aa.SFD_PK == sthPK).ToList();
        //                foreach (INV_STK_TRAN_DTL objItem in objList)
        //                    lblTranTo.Text = objItem.ADM_DEPT_MST1.DPT_NAME;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (SortBy == e.SortExpression)
        //        {
        //            ////Toggle the sort expression
        //            //if (SortDirection == Resources.gComsRes.SortAscending)
        //            //    SortDirection = Resources.gComsRes.SortDescending;
        //            //else
        //            //    SortDirection = Resources.gComsRes.SortAscending;
        //        }
        //        else
        //        {
        //            //SortBy = e.SortExpression;
        //            //SortDirection = Resources.gComsRes.SortAscending;
        //        }

        //        this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        #endregion
        #endregion
        #region Helper Methods
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessId()
        {
            string path;
            path = "/PurchaseOrderManagement/RequestForQuote.aspx"; ;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                PageProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcessID.Value = dtProcess.Rows[0]["PROCESS_PK"].ToString();
                base.WkfPageUrl = path;
                base.WkfPageType = (int)PageTypeEnum.Listing;
            }
        }
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        /// <param name="mode"></param>
        private void SetUIEditView(ActionsEnum mode)
        {
            try
            {
                foreach (GridViewRow grdrow in grdRFQList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        Session[ERP.Utilities.SessionStrings.RFQPK] = Convert.ToInt32(grdRFQList.DataKeys[grdrow.RowIndex].Values[0]);
                        switch (mode)
                        {
                            case ActionsEnum.EDIT:
                                Session[ERP.Utilities.SessionStrings.RFQMODE] = EntryStatus.ENTRYMODE;
                                break;
                            case ActionsEnum.VIEW:
                                Session[ERP.Utilities.SessionStrings.RFQMODE] = EntryStatus.VIEWMODE;
                                break;
                        }

                        if (mode == ActionsEnum.RFQRESPONSE)
                        {
                            HiddenField hdfStatus;
                            hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                            int status = hdfStatus != null ? !string.IsNullOrEmpty(hdfStatus.Value) ? Convert.ToInt32(hdfStatus.Value) : -1 : -1;
                            if (status == 2)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQResponse), false);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_Response").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RequestForQuote), false);
                        }
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
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
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsPageData != null)
                        {
                            if (dsPageData.Tables[0].Rows.Count > 0)
                            {
                                rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString());
                            }
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                          (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                          (rowCount / pageSize) + 1;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdRFQList.PageIndex = Convert.ToInt32(PageIndex);
                            grdRFQList.DataSource = dsPageData.Tables[1].DefaultView;
                            grdRFQList.DataBind();
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            hdfDeptPk.Value = "0";

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
            uclPaging.CurrentPage = 1;
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // uclPaging.CurrentPage = 1;
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnResponse.PreRender += new EventHandler(btnAction_PreRender);

            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
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
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
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
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                    EntryStatus = EntryStatus.LISTMODE;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
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
                base.WkfRefID = listingRefID;
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

        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            SELECTEDPR,
            VENDORS
        }

        #endregion
    }
}