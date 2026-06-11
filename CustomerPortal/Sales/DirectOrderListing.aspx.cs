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
    public partial class DirectOrderListing : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
        private short EnqStatus
        {
            get
            {
                return (short)(ViewState["EnqStatus"] == null ? 1 : (short)ViewState["EnqStatus"]);
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
                    //if (Request.QueryString["STATUS"] != null)
                    //{
                    //    EnqStatus = Convert.ToByte(Request.QueryString["STATUS"]);
                    //}
                    EnqStatus = (short)DirectOrderStatus.DirectOrder;
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
                    txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
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

                    //if (EnqStatus == 2)
                    //{
                    //    hdfAppType.Value = BusinessObject.CommonManagement.ApplicationType.CQTN;
                    //    hdfAppSubType.Value = string.Empty;
                    //    //btnQuote.Visible = btnEdit.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId());
                    //}
                    //else
                    //{
                        btnPrint.Visible = false;
                        //btnQuote.Visible = btnEdit.Visible = 
                        btnNew.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId(1));
                    //}

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
                        //dsPageData = BusinessLogic.Sales.Enquiry.GetEnquiryList(
                        //    new BusinessObject.GridPrams()
                        //    {
                        //        SortBy = string.IsNullOrEmpty(SortBy) ? EnqStatus == 1 ? "CEH_REF_DATE" : "CEH_DATE" : SortBy,
                        //        SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                        //        ThenBy = string.IsNullOrEmpty(ThenBy) ? EnqStatus == 1 ? "CEH_REF_NO" : "CEH_NO" : ThenBy,
                        //        ThenDirection = string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                        //        FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                        //        ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim()
                        //    }, currentUser, 0, currentUser.PKUser, EnqStatus
                        //    , string.IsNullOrEmpty(hdfEnqNumber.Value) ?
                        //    string.IsNullOrEmpty(hdfQuotNumber.Value) ? 0 : Convert.ToInt32(hdfQuotNumber.Value) : Convert.ToInt32(hdfEnqNumber.Value)
                        //    , string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value));

                        dsPageData = BusinessLogic.Sales.Enquiry.GetEnquiryList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? "CEH_DATE" : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = string.IsNullOrEmpty(ThenBy) ? "CEH_NO" : ThenBy,
                                ThenDirection = string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim()
                            }, currentUser, 0, currentUser.PKUser, EnqStatus
                            , string.IsNullOrEmpty(hdfEnqNumber.Value) ?0 : Convert.ToInt32(hdfEnqNumber.Value)
                            , string.IsNullOrEmpty(hdfCustomer.Value) ? 0 : Convert.ToInt32(hdfCustomer.Value)
                            ,Resources.PageURL.DirectOrderDetails)
                            ;
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

            switch (commonActions)
            {
                case ActionsEnum.NEW:
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderDetails), false);
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
                    ////if (EnqStatus == 1)
                    //{
                    //    if (btnEdit.Visible)
                    //    {
                            SetUIEditView(ActionsEnum.ENQUIRY);
                    //    }
                    //    else
                    //    {
                    //        SetUIEditView(ActionsEnum.VIEW);
                    //    }
                    //}
                    //else
                    //{
                    //    bool btnClick = false;
                    //    foreach (GridViewRow grdrow in grdEnquiryList.Rows)
                    //    {
                    //        RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    //        int CurrPK;
                    //         check row selected or not
                    //        if (rbtn.Checked)
                    //        {
                    //            Session[ERP.Utilities.SessionStrings.ENQUIRYPK] = CurrPK = Convert.ToInt32(grdEnquiryList.DataKeys[grdrow.RowIndex].Values[0]);
                    //            Session["EnquiryMode"] = EntryStatus.VIEWMODE;
                    //            Session["QuotationToEnquiry"] = "1";
                    //            btnClick = true;
                    //            break;
                    //        }

                    //    }
                    //    if (btnClick)
                    //    {
                    //        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderDetails), false);
                    //    }
                    //    else
                    //    {
                    //         if no items selected, Show Error Message
                    //        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    //        EntryStatus = EntryStatus.LISTMODE;
                    //    }
                    //}
                    break;
                case ActionsEnum.QUOTATION:
                    //if (EnqStatus == 1)
                    {
                        SetUIEditView(ActionsEnum.QUOTE);
                    }
                    //else
                    //{
                    //    SetUIEditView(ActionsEnum.EDIT);
                    //}
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
                    if (EnqStatus == 1)
                    {
                        //e.Row.Cells[1].Visible = false;
                        //e.Row.Cells[2].Visible = false;
                        //e.Row.Cells[8].Visible = false;
                        //e.Row.Cells[10].Visible = false;

                        //e.Row.Cells[5].Width = Unit.Percentage(30);
                        //e.Row.Cells[6].Width = Unit.Percentage(18);
                        //e.Row.Cells[7].Width = Unit.Percentage(16);
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            lblCustomerName = e.Row.Cells[5].FindControl("lblCustomerName") as Label;
                            lblCustomerName.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustomerName.Text, 30);

                            lblCustCountry = e.Row.Cells[6].FindControl("lblCustCountry") as Label;
                            lblCustCountry.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustCountry.Text, 20);

                            lblRemarks = e.Row.Cells[7].FindControl("lblRemarks") as Label;
                            lblRemarks.Text = ERP.Utilities.CommonFunctions.GetShortString(lblRemarks.Text, 17);
                        }
                    }
                    else
                    {
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            lblCustomerName = e.Row.Cells[5].FindControl("lblCustomerName") as Label;
                            lblCustomerName.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustomerName.Text, 15);

                            lblCustCountry = e.Row.Cells[6].FindControl("lblCustCountry") as Label;
                            lblCustCountry.Text = ERP.Utilities.CommonFunctions.GetShortString(lblCustCountry.Text, 10);

                            lblRemarks = e.Row.Cells[7].FindControl("lblRemarks") as Label;
                            lblRemarks.Text = ERP.Utilities.CommonFunctions.GetShortString(lblRemarks.Text, 9);

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
        private int FillProcessId(int type)
        {
            int procId = 0;
            string path;
            if (type == 1)
            {
                //if (EnqStatus == 2)
                //    path = "Sales/DirectOrderAccept.aspx";
                //else
                    path = "Sales/DirectOrderDetails.aspx";
            }
            else
                path = "Sales/DirectOrderAccept.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
            }
            return procId;
        }
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        /// <param name="mode"></param>
        private void SetUIEditView(ActionsEnum mode)
        {
            HiddenField hdfSOCreated;
            short isSOCreated;
            int trxStatus;
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
                        trxStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTrxStatus")).Value);
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
                            case ActionsEnum.ENQUIRY:
                                Session["EnquiryMode"] = EntryStatus.ENTRYMODE;
                                break;
                            case ActionsEnum.PRINT:
                                //Response.Redirect("../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + hdfAppType.Value + "&APPSUBTYPE=" + hdfAppSubType.Value + "');", true);
                                break;
                        }
                        if (mode == ActionsEnum.PRINT)
                        {
                            return;
                        }

                        if (mode == ActionsEnum.ENQUIRY)
                        {
                            if (trxStatus == 3)
                            {
                                Session["EnquiryMode"] = EntryStatus.ENTRYMODE;
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderDetails), false);
                            }
                            else
                            {
                                Session["EnquiryMode"] = EntryStatus.VIEWMODE;
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderDetails), false);
                            }
                        }
                        else if (mode == ActionsEnum.QUOTE)
                        {
                            if (trxStatus == 4)
                            {
                                hdfSOCreated = grdrow.FindControl("hdfSOCreated") as HiddenField;
                                isSOCreated = Convert.ToInt16(hdfSOCreated.Value);

                                //if (BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, FillProcessId(2)) || isSOCreated > 0)
                                //{
                                //hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                //int status = hdfStatus != null ? !string.IsNullOrEmpty(hdfStatus.Value) ? Convert.ToInt32(hdfStatus.Value) : -1 : -1;
                                //if (status == (int)WorkFlowStatus.APPROVED)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderAccept), false);
                            }
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Quote").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            //}
                            //else
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_No_Quote").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else
                        {
                            if (trxStatus == 3)
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderDetails), false);
                            else
                                Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.DirectOrderAccept), false);
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
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
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

                //if (EnqStatus != 1)
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "QuotationMode", "$(document).ready(function(){QuotationMode(1);});", true);
                //else
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "QuotationMode", "$(document).ready(function(){QuotationMode();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            //lblBreadCrum.Text = (EnqStatus == 1 ? GetLocalResourceObject("Breadcrumb_Enq").ToString() : GetLocalResourceObject("Breadcrumb_Quotation").ToString())
            //.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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