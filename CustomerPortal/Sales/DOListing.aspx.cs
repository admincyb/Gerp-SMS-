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
using System.Data;
using BusinessObject.PurchaseOrderManagement;

namespace CustomerPortal.Sales
{
    public partial class DOListing : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
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
        private short EnqStatus
        {
            get
            {
                return (short)(ViewState["EnqStatus"] == null ? 3 : (short)ViewState["EnqStatus"]);
            }
            set
            {
                ViewState["EnqStatus"] = value;
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
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        User currentUser;
        private DataSet dsPageData;
        private int listingRefID;
        private DataTable dtPageData;
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            int cusPK;
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["STATUS"] != null)
                    {
                        EnqStatus = Convert.ToByte(Request.QueryString["STATUS"]);
                        if (EnqStatus < 3)
                            EnqStatus = 3;
                    }
                    FillProcessId();
                    if (EnqStatus == 3)
                    {
                        spnEnqList.Attributes.Remove("class");
                        spnEnqList.Attributes.Add("class", "tab-active");
                        lbnList.CssClass = "tab-active";
                        spnQtnList.Attributes.Remove("class");
                        spnQtnList.Attributes.Add("class", "tab-inactive");
                        lbnQtnList.CssClass = "tab-inactive";
                        Page.Title = Resources.Captions.Title_DirectOrderListing;
                    }
                    else
                    {
                        spnEnqList.Attributes.Remove("class");
                        spnEnqList.Attributes.Add("class", "tab-inactive");
                        lbnList.CssClass = "tab-inactive";
                        spnQtnList.Attributes.Remove("class");
                        spnQtnList.Attributes.Add("class", "tab-active");
                        lbnQtnList.CssClass = "tab-active";
                        Page.Title = Resources.Captions.Title_OrderAcceptListing;
                    }

                    GetFieldValues(ControlsEnum.USERCUSTOMER);
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        if (int.TryParse(dtPageData.Rows[0]["CUS_PK"].ToString(), out cusPK) && cusPK > 0)
                        {
                            txtCustomer.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0]["CUS_NAME"].ToString());
                            hdfCustomer.Value = cusPK.ToString();
                            txtCustomer.Enabled = false;
                        }
                    }
                    txtFromDate.Text = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    Session["EnqDtlList"] = null;
                    Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = null;
                    Session["EnquiryMode"] = null;
                    Session[ERP.Utilities.SessionStrings.QUOTATIONPK] = null;
                    Session["QuotationToEnquiry"] = null;

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "CEH_PK";
                    grdEnquiryList.DataKeyNames = datakeyarray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    EntryStatus = EntryStatus.LISTMODE;

                    if (EnqStatus == 4)
                    {
                        hdfAppType.Value = BusinessObject.CommonManagement.ApplicationType.CQTN;
                        hdfAppSubType.Value = string.Empty;
                        //btnQuote.Visible = btnEdit.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                    }
                    else
                    {
                        //btnPrint.Visible = false;
                        //btnQuote.Visible = btnEdit.Visible = 
                      //  btnNew.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId(1));
                    }

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
                        dsPageData = BusinessLogic.Sales.Enquiry.GetEnquiryList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? "CEH_DATE" : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = !string.IsNullOrEmpty(SortBy) && (SortBy == ThenBy || SortBy == "CEH_NO") ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "CEH_NO" : ThenBy,
                                ThenDirection = !string.IsNullOrEmpty(SortBy) && (SortBy == ThenBy || SortBy == "CEH_NO") ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim()
                            }, currentUser, 0, currentUser.PKUser, EnqStatus
                            , string.IsNullOrEmpty(hdfEnqNumber.Value) ?
                            string.IsNullOrEmpty(hdfQuotNumber.Value) ? 0 : Convert.ToInt32(hdfQuotNumber.Value) : Convert.ToInt32(hdfEnqNumber.Value)
                            , string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value)
                            , Resources.PageURL.DODetails);
                        break;
                    case ControlsEnum.USERCUSTOMER:
                        dtPageData = BusinessLogic.CommonManagement.CommonManagement.GetUserCustomer(currentUser.PKUser, currentUser.SBUID);
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
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;

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
                    Session.Remove(ERP.Utilities.SessionStrings.ENQUIRYPK);
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DODetails), false);
                    break;
                case ActionsEnum.ITEMSELECTED:
                    SetUIEditView(commonActions);
                    break;
                case ActionsEnum.EDIT:
                    SetUIEditView(ActionsEnum.EDIT);
                    break;
                case ActionsEnum.VIEW:
                    SetUIEditView(ActionsEnum.VIEW);
                    break;
                case ActionsEnum.QUOTE:
                    SetUIEditView(ActionsEnum.QUOTE);
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
                    SetUIEditView(commonActions);
                    break;
                case ActionsEnum.ENQUIRY:
                    if (EnqStatus == 3)
                    {
                        if (btnEdit.Visible)
                        {
                            SetUIEditView(ActionsEnum.EDIT);
                        }
                        else
                        {
                            SetUIEditView(ActionsEnum.VIEW);
                        }
                    }
                    else
                    {
                        bool btnClick = false;
                        foreach (GridViewRow grdrow in grdEnquiryList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                            int CurrPK;
                            // check row selected or not
                            if (rbtn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK = Convert.ToInt32(grdEnquiryList.DataKeys[grdrow.RowIndex].Values[0]);
                                Session["EnquiryMode"] = EntryStatus.VIEWMODE;
                                Session["QuotationToEnquiry"] = "1";
                                btnClick = true;
                                break;
                            }

                        }
                        if (btnClick)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DODetails), false);
                        }
                        else
                        {
                            // if no items selected, Show Error Message
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                    }
                    break;
                case ActionsEnum.QUOTATION:
                    if (EnqStatus == 3)
                    {
                        SetUIEditView(ActionsEnum.QUOTE);
                    }
                    else
                    {
                        SetUIEditView(ActionsEnum.EDIT);
                    }
                    break;
                case ActionsEnum.ENQUIRYLIST:
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DOListing), false);
                    break;
                case ActionsEnum.QUOTATIONLIST:
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DOAcceptListing), false);
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
            Label lblCustomerName;
            Label lblCustCountry;
            Label lblRemarks;
            Button imgSOCreated;
            HiddenField hdfSOCreated;
            short isSOCreated;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
                {
                    if (EnqStatus == 3)
                    {
                        //e.Row.Cells[1].Visible = false;
                        //e.Row.Cells[2].Visible = false;
                        //e.Row.Cells[8].Visible = false;
                        e.Row.Cells[10].Visible = false;

                        //e.Row.Cells[5].Width = Unit.Percentage(18);
                        //e.Row.Cells[6].Width = Unit.Percentage(13);
                        //e.Row.Cells[7].Width = Unit.Percentage(13);
                        //if (e.Row.RowType == DataControlRowType.DataRow)
                        //{
                        //    lblCustomerName = e.Row.Cells[5].FindControl("lblCustomerName") as Label;
                        //    lblCustomerName.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustomerName.Text, 27);

                        //    lblCustCountry = e.Row.Cells[6].FindControl("lblCustCountry") as Label;
                        //    lblCustCountry.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustCountry.Text, 19);

                        //    lblRemarks = e.Row.Cells[7].FindControl("lblRemarks") as Label;
                        //    lblRemarks.Text = ERP.Utilities.CommonFunctions.GetShortString(lblRemarks.Text, 18);
                        //}
                    }
                    else
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //lblCustomerName = e.Row.Cells[5].FindControl("lblCustomerName") as Label;
                            //lblCustomerName.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustomerName.Text, 25);

                            //lblCustCountry = e.Row.Cells[6].FindControl("lblCustCountry") as Label;
                            //lblCustCountry.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustCountry.Text, 16);

                            //lblRemarks = e.Row.Cells[7].FindControl("lblRemarks") as Label;
                            //lblRemarks.Text = ERP.Utilities.CommonFunctions.GetShortString(lblRemarks.Text, 18);

                            imgSOCreated = e.Row.FindControl("imgSOCreated") as Button;
                            hdfSOCreated = e.Row.FindControl("hdfSOCreated") as HiddenField;
                            isSOCreated = Convert.ToInt16(hdfSOCreated.Value);
                            if (isSOCreated > 0)
                            {
                                imgSOCreated.Visible = true;
                            }
                            else
                            {
                                imgSOCreated.Visible = false;
                            }
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Label lblNo = e.Row.FindControl("lblNo") as Label;
                    //lblNo.Text = DataBinder.Eval(e.Row.DataItem, "CEH_REF_NO").ToString() == "" ? Resources.Messages.DocGenerationNew : DataBinder.Eval(e.Row.DataItem, "CEH_REF_NO").ToString();
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
            string path = "";
                if (EnqStatus == 4)
                    path = "/Sales/DOAccept.aspx";
                else
                    path = "/Sales/DODetails.aspx";

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                PageProcessID = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
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
            HiddenField hdfSOCreated;
            HiddenField hdfDept;
            int dept;
            short isSOCreated;
            try
            {
                foreach (GridViewRow grdrow in grdEnquiryList.Rows)
                {
                    HiddenField hdfStatus;
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    int CurrPK;
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK = Convert.ToInt32(grdEnquiryList.DataKeys[grdrow.RowIndex].Values[0]);
                        switch (mode)
                        {
                            case ActionsEnum.QUOTE:
                            case ActionsEnum.EDIT:
                                Session["EnquiryMode"] = EntryStatus.ENTRYMODE;
                                break;
                            case ActionsEnum.VIEW:
                                Session["EnquiryMode"] = EntryStatus.VIEWMODE;
                                break;
                            case ActionsEnum.PRINT:
                                rbtn.Checked = false;
                                //Response.Redirect("../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "');", true);
                                break;
                            case ActionsEnum.ITEMSELECTED:
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                listingRefID = workflowCore.GetRefID(CurrPK, PageProcessID);
                                break;
                        }
                        if (mode == ActionsEnum.PRINT || mode == ActionsEnum.ITEMSELECTED)
                        {
                            return;
                        }

                        if (mode == ActionsEnum.QUOTE)
                        {
                            Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DOAccept), false);
                            //hdfSOCreated = grdrow.FindControl("hdfSOCreated") as HiddenField;
                            //isSOCreated = Convert.ToInt16(hdfSOCreated.Value);

                            //if (BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId(2)) || isSOCreated > 0)
                            //{
                            //hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                            //int status = hdfStatus != null ? !string.IsNullOrEmpty(hdfStatus.Value) ? Convert.ToInt32(hdfStatus.Value) : -1 : -1;
                            //if (status == (int)WorkFlowStatus.APPROVED)
                            //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DOAccept), false);
                            //else
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Quote").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            //}
                            //else
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_No_Quote").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else
                        {
                            if (EnqStatus == 3)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DODetails), false);
                            else
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DOAccept), false);
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
                        if (dsPageData != null)
                        {
                            //SortExpression = SortExpression == null ? "RFH_NO" : SortExpression;
                            //SortOrder = SortOrder == null ? "asc" : SortOrder;
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            //SortFilter = SortExpression + " " + SortOrder;
                            //dsPageData.Tables[1].DefaultView.Sort = SortFilter;
                            grdEnquiryList.PageIndex = Convert.ToInt32(PageIndex);
                            grdEnquiryList.DataSource = dsPageData.Tables[1].DefaultView;

                            grdEnquiryList.DataBind();
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
            if (txtCustomer.Enabled)
            {
                txtCustomer.Text = string.Empty;
                hdfCustomer.Value = "0";
            }
            txtQuotNumber.Text = string.Empty;
            hdfQuotNumber.Value = "0";
            txtEnqNumber.Text = string.Empty;
            hdfEnqNumber.Value = "0";

            txtFromDate.Text = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
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
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnQuote.PreRender += new EventHandler(btnAction_PreRender);
            this.btnPrint.PreRender += new EventHandler(btnAction_PreRender);
          
            this.lbnList.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnQtnList.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnEnquiry.PreRender += new EventHandler(btnAction_PreRender);
            this.lbnQuotation.PreRender += new EventHandler(btnAction_PreRender);

            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnQuote.Load += new EventHandler(btnAction_Load);
            this.btnPrint.Load += new EventHandler(btnAction_Load);
         
            this.lbnList.Load += new EventHandler(btnAction_Load);
            this.lbnEnquiry.Load += new EventHandler(btnAction_Load);
            this.lbnQuotation.Load += new EventHandler(btnAction_Load);
            this.lbnQtnList.Load += new EventHandler(btnAction_Load);
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
        //protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        //{
        //    try
        //    {
        //        //switch (e.Action)
        //        //{
        //        //    case NavigationEnum.PAGECHANGE:
        //        //        uclPaging.CurrentPage = e.CurrentPage;
        //        //        break;
        //        //    case NavigationEnum.FIRST:
        //        //        // Assignment the first page index.
        //        //        if (e.CurrentPage > 1)
        //        //            uclPaging.CurrentPage = 1;
        //        //        break;
        //        //    case NavigationEnum.LAST:
        //        //        // Assignment the last page index.
        //        //        if (e.CurrentPage <= e.TotalPages)
        //        //            uclPaging.CurrentPage = e.TotalPages;
        //        //        break;
        //        //    case NavigationEnum.NEXT:
        //        //        // Increment the next page index.
        //        //        if (e.CurrentPage <= e.TotalPages)
        //        //            uclPaging.CurrentPage++;
        //        //        break;
        //        //    case NavigationEnum.PREVIOUS:
        //        //        // Decrement the previous page index.
        //        //        if (e.CurrentPage > 1)
        //        //            uclPaging.CurrentPage--;
        //        //        break;
        //        //}
        //        //PageIndex = uclPaging.CurrentPage.ToString();
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EnableDisableButtons(e.TotalPages);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link
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

                if (EnqStatus != 3)
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "QuotationMode", "$(document).ready(function(){QuotationMode(1);});", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "QuotationMode", "$(document).ready(function(){QuotationMode();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = (EnqStatus == 3 ? GetLocalResourceObject("Breadcrumb_Enq").ToString() : GetLocalResourceObject("Breadcrumb_Quotation").ToString())
            .Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
            VENDORS,
            USERCUSTOMER
        }

        #endregion
    }
}