using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;
using System.Configuration;
using ERP.Utilities;
using BusinessObject.ILibrary;
using BusinessObject.Sales;
using System.Xml;
using ERPSMS_v01.UserControls;
using DataAccess.ProductionDL;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERP.Utilities.Constants.DA;
using BusinessObject.Common;

namespace ERPSMS_v01.ProductionPlanning
{
    public partial class PackingTransaction : ERP.Store.UI.MyBasePage
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
        private int pkingPK
        {
            get
            {
                return this.ViewState["pkingPK"] == null ? 0 : (int)this.ViewState["pkingPK"];
            }
            set
            {
                this.ViewState["pkingPK"] = value;
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageIndex] == null ? "1" : (string)this.ViewState[ViewstateStrings.PageIndex];
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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 0 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To Maintain Despatch Details
        /// </summary>
        private List<DespatchDetailsBO> DespatchDetails
        {
            get
            {
                return (Session[ERP.Utilities.SessionStrings.DespatchDetails] == null ? null
                    : (List<DespatchDetailsBO>)Session[ERP.Utilities.SessionStrings.DespatchDetails]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.DespatchDetails] = value;
            }
        }

        //(List<DespatchDetailsBO>)Session[ERP.Utilities.SessionStrings.FormerItems]

        #endregion

        // For Common Actions
        ActionsEnum commonAction;

        //For get db return messages
        private DataSet dsDBMessgaes;

        //For fetch FinishedGoods
        private DataSet dsPage;

        //For Filter XML
        private string[] P_XML;

        //For Save XML
        private string[] S_XML;

        private int CustomerPk;

        int SlNo;

        User CurrentUser;

        #endregion

        #region Page Level Events

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();
                GetFieldValues(ControlEnum.HEADER);
                SetFieldValues(ControlEnum.HEADER);
                EntryStatus = EntryStatus.LISTMODE;
            }
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(2);", true);
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetTab", "SetTabs(1);", true);
            }

        }
        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.HEADER:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(2, P_XML);
                    break;
                case ControlEnum.GRID:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(1, P_XML);
                    break;
                case ControlEnum.SEARCH:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(1, P_XML);
                    break;
                case ControlEnum.SALEORDER:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(5, P_XML);
                    break;
                case ControlEnum.SALEORDERITEM:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(4, P_XML);
                    break;
                case ControlEnum.UOM:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(4, P_XML);
                    break;
                case ControlEnum.PACKINGDETAILS:
                    SetFilterXML(type);
                    dsPage = FetchDbValues(3, P_XML);
                    break;
                //case ControlEnum.DESPATCH:
                //    SetFilterXML(type);
                //    dsPage = FetchDbValues(3, P_XML);
                //    if (dsPage != null && dsPage.Tables.Count > 2)
                //    {
                //        SetDespatchDetails(dsPage.Tables[2]);
                //    }
                //    break;


                //case ControlEnum.SALEORDERITEM:
                //    SetFilterXML(type);
                //    dsPage = FetchDbValues(4, P_XML);
                //    break;

                //case ControlEnum.LISTING:
                //    SetFilterXML(ControlEnum.LISTING);
                //    dsPage = FetchDbValues(1, P_XML);
                //    break;
                //case ControlEnum.EDIT_DISPATCH_DETAILS:
                //    SetFilterXML(ControlEnum.EDIT_DISPATCH_DETAILS);
                //    dsPage = FetchDbValues(3, P_XML);

                //    break;
                //case ControlEnum.HEADER:
                //    SetFilterXML(ControlEnum.HEADER);
                //    dsPage = FetchDbValues(6, P_XML);
                //    break;
                //case ControlEnum.DISPATCH_FROM_ORDER:
                //    SetFilterXML(ControlEnum.DISPATCH_FROM_ORDER);
                //    dsPage = FetchDbValues(7, P_XML);
                //    break;
            }
        }
        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        public void SetFieldValues(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.GRID:
                    BindGrid(type);
                    break;
                case ControlEnum.HEADER:
                    BindDropDown(ControlEnum.CUSTOMER);
                    BindDropDown(ControlEnum.SHIFT);
                    break;
                case ControlEnum.SALEORDER:
                    BindDropDown(ControlEnum.SALEORDER);
                    break;
                case ControlEnum.SALEORDERITEM:
                    BindDropDown(ControlEnum.SALEORDERITEM);
                    txtBalToPack.Text = string.Empty;
                    break;
                case ControlEnum.UOM:
                    if (dsPage != null && dsPage.Tables.Count > 0)
                    {
                        hdfProductPk.Value = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.productPK].ToString();
                        txtBalToPack.Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.BalanceToPack].ToString();
                        txtUOM.Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.UomCode].ToString();
                        hdfUomPK.Value = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.UomPK].ToString();
                        txtOrderQty.Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.OrderQty].ToString();
                        hdfOrderQty.Value = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.OrderQty].ToString();
                        hdfBalToPack.Value = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.BalanceToPack].ToString();
                        txtQty.Text = txtBalToPack.Text;
                    }
                    break;
                case ControlEnum.PACKINGDETAILS:
                    if (dsPage != null && dsPage.Tables.Count > 0 && dsPage.Tables[0].Rows.Count > 0)
                    {
                        txtDate.Text = Convert.ToDateTime(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PackedDate].ToString()).ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                        txtRemarks.Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PackRemarks].ToString();
                        ddlShift.SelectedValue = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PackShiftPK].ToString();
                        hdfLastModDate.Value = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PackLastModDate].ToString();
                        AddItemDetails(dsPage.Tables[1]);

                    }
                    break;
                //case ControlEnum.DESPATCH:
                //    BindGrid(type);
                //    break;
                //case ControlEnum.CUSTOMER:
                //    BindDropDown(ControlEnum.CUSTOMER);
                //    //if (dsPage != null && dsPage.Tables.Count > 0 && dsPage.Tables[0].Rows.Count > 0)
                //    //{
                //    //    // CustomerPk = dsPage.Tables[2].Rows[0][Resources.DataFieldRes.dicNo].ToString();    
                //    //    lblDespatchNoTxt.Text = dsPage.Tables[2].Rows[0][Resources.DataFieldRes.dicNo].ToString();
                //    //}
                //    break;


                //case ControlEnum.LISTING:
                //    BindGrid(ControlEnum.LISTING);
                //    break;
                //case ControlEnum.EDIT_DISPATCH_DETAILS:
                //    ddlCustomer.SelectedValue = dsPage.Tables[2].Rows[0][Resources.DataFieldRes.DespatchCustomer].ToString();
                //    ddlCustomer.Enabled = false;

                //    txtDate.Text = Convert.ToDateTime(dsPage.Tables[2].Rows[0][Resources.DataFieldRes.Disp_Date].ToString()).ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                //   // lblDespatchNoTxt.Text = dsPage.Tables[2].Rows[0][Resources.DataFieldRes.Disp_No].ToString();
                //    txtRemarks.Text = dsPage.Tables[2].Rows[0][Resources.DataFieldRes.Disp_Remarks].ToString();

                //    AddItemDetails(dsPage.Tables[3]);
                //    break;

                //case ControlEnum.HEADER:
                //    //if (dsPage != null && dsPage.Tables.Count > 0 && dsPage.Tables[0].Rows.Count > 0)
                //    //{
                //    //    lblDespatchNoTxt.Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.dicNo].ToString();

                //    //}
                //    break;
                //case ControlEnum.DISPATCH_FROM_ORDER:
                //    ddlCustomer.SelectedValue = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.SohCustomer].ToString();
                //    txtDate.Text = System.DateTime.Today.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                //    AddItemFromSaleOrder(dsPage.Tables[0]);
                //    break;
            }
        }
        #endregion

        #region Action handler

        /// <summary>
        /// Button and other Conrol Actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvrItem;
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else
                    if (sender.GetType().IsEquivalentTo(typeof(Button)))
                    {
                        commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                    }
                    else
                        if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                        {
                            commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                        }
                        else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                        {
                            commonAction = ActionsEnum.CHANGE;
                        }

                switch (commonAction)
                {

                    case ActionsEnum.ADD_PACKING:
                        divEntryForm.Visible = true;
                        divListing.Visible = false;
                        ClearDDL();
                        ddlCustomer.Focus();
                        EntryStatus = EntryStatus.NEWMODE;
                        break;

                    case ActionsEnum.CANCEL:
                        divEntryForm.Visible = false;
                        divListing.Visible = true;
                        ResetEntryForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.ADD_ACTION:
                        Page.Validate("detail");
                        if (Page.IsValid)
                        {
                            if (!IsExisiItem(Convert.ToInt32(ddlSaleOrderItem.SelectedValue.ToString())))
                                AddItemDetails();
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ItemAded").ToString();// "Saleorder item already added.";
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }

                        break;
                    case ActionsEnum.SEARCH:
                        //if (txtFromDate.Text == string.Empty || txtToDate.Text == string.Empty)
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_ItemAded").ToString();// "Saleorder item already added.";
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        GetFieldValues(ControlEnum.SEARCH);
                        SetFieldValues(ControlEnum.GRID);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    case ActionsEnum.CLEAR:
                         ResetForm();
                     GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);
                    EntryStatus = EntryStatus.LISTMODE;
                    break;
                    case ActionsEnum.DELETE_ACTION:
                        gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        SlNo = Convert.ToInt32(((Label)grdPackingDetails.Rows[gvrItem.RowIndex].FindControl("lblSlNo")).Text);
                        if (SlNo != 0)
                        {
                            DeleteItemDetails(SlNo);
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        break;
                    case ActionsEnum.EDIT_ACTION:

                        gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;

                        SlNo = Convert.ToInt32(((Label)grdPackingDetails.Rows[gvrItem.RowIndex].FindControl("lblSlNo")).Text);
                        if (SlNo != 0)
                        {
                            // Delete Topuped Item details from grid
                            FillItemDetails(SlNo);
                            EntryStatus = EntryStatus.ENTRYMODE;

                        }
                        break;

                    #region Save Items
                    case ActionsEnum.SAVE:
                        Page.Validate("pack");
                        if (Page.IsValid)
                        {
                            SetPackingDetails();
                            SetFieldValuesToXML(commonAction);
                            dsDBMessgaes = SetValuesToDB(1, P_XML);
                            if (PrintDbMessages(dsDBMessgaes, commonAction))
                            {
                                ResetEntryForm();
                                GetFieldValues(ControlEnum.GRID);
                                SetFieldValues(ControlEnum.GRID);
                                divEntryForm.Visible = false;
                                divListing.Visible = true;
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                        }
                        break;
                    #endregion
                    case ActionsEnum.DELETE_LIST_ACTION:
                        gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        pkingPK = Convert.ToInt32(((HiddenField)grdPackingList.Rows[gvrItem.RowIndex].FindControl("hdfPkhPK")).Value);
                        //Code for delete write here 
                        SetFieldValuesToXML(commonAction);
                        dsDBMessgaes = SetValuesToDB(2, P_XML);
                        if (PrintDbMessages(dsDBMessgaes, commonAction))
                        {
                            ResetEntryForm();
                            GetFieldValues(ControlEnum.GRID);
                            SetFieldValues(ControlEnum.GRID);
                            divEntryForm.Visible = false;
                            divListing.Visible = true;
                            EntryStatus = EntryStatus.LISTMODE;
                        }


                        break;
                    case ActionsEnum.EDIT_LIST_ACTION:
                        ResetEntryForm();
                        gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        pkingPK = Convert.ToInt32(((HiddenField)grdPackingList.Rows[gvrItem.RowIndex].FindControl("hdfPkhPK")).Value);

                        GetFieldValues(ControlEnum.PACKINGDETAILS);
                        SetFieldValues(ControlEnum.PACKINGDETAILS);

                        divEntryForm.Visible = true;
                        divListing.Visible = false;
                        EntryStatus = EntryStatus.ENTRYMODE;

                        break;

                    case ActionsEnum.CUSTOMERSELECTED:
                         ClearLine();
                         GetFieldValues(ControlEnum.SALEORDER);
                         SetFieldValues(ControlEnum.SALEORDER);
                         break;

                    #region Dropdown list change
                    case ActionsEnum.CHANGE:

                        if (((DropDownList)sender).ID == "ddlCustomer")
                        {
                            ClearLine();
                            GetFieldValues(ControlEnum.SALEORDER);
                            SetFieldValues(ControlEnum.SALEORDER);

                        }
                        else
                            if (((DropDownList)sender).ID == "ddlSaleOrderNo")
                            {
                                GetFieldValues(ControlEnum.SALEORDERITEM);
                                SetFieldValues(ControlEnum.SALEORDERITEM);

                            }
                            else
                                if (((DropDownList)sender).ID == "ddlSaleOrderItem")
                                {
                                    GetFieldValues(ControlEnum.UOM);
                                    SetFieldValues(ControlEnum.UOM);

                                }

                        break;
                    #endregion


                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// GridView Row Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((sender as GridView).ID == "grdAffectedItems")
                {
                    if (e.Row.RowType == DataControlRowType.Footer && dsPage != null)
                    {
                        if (dsPage.Tables[0].Rows.Count > 0)
                        {
                            ((Label)e.Row.FindControl("lblTotalOrderQty")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.OrderQtyTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.OrderQtyTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalDispatched")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.DispatchedTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.DispatchedTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalAllocated")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.AllocatedTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.AllocatedTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalProduced")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.ProducedTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.ProducedTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalPLannedBefore")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PlannedBeforeTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PlannedBeforeTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalPLannedAfter")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PlannedAfterTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PlannedAfterTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalBtPBefore")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.BalanceBeforeTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.BalanceBeforeTotal]).ToString("n");
                            ((Label)e.Row.FindControl("lblTotalBtPAfter")).Text = dsPage.Tables[0].Rows[0][Resources.DataFieldRes.BalanceAfterTotal].Equals(DBNull.Value) ? "" :
                                Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.BalanceAfterTotal]).ToString("n");
                        }
                    }
                }
                if ((sender as GridView).ID == "grdPackingDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        ((Label)e.Row.FindControl("lblBalToPack")).Text = ((((Label)e.Row.FindControl("lblSaleOrderQty")).Text != string.Empty ? Convert.ToDecimal(((Label)e.Row.FindControl("lblSaleOrderQty")).Text) : 0) - (((TextBox)e.Row.FindControl("txtQty")).Text != string.Empty ? Convert.ToDecimal(((TextBox)e.Row.FindControl("txtQty")).Text) : 0)).ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region PageActionHandler

        /// <summary>
        /// Handle All page Load Event
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

                    txtDate.Text = System.DateTime.Today.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);

                    string[] datakeyarray1;
                    datakeyarray1 = new string[1];
                    datakeyarray1[0] = Resources.DataFieldRes.DespatchPK;
                    // grdDespatch.DataKeyNames = datakeyarray1;

                    string[] datakeyarray2;
                    datakeyarray2 = new string[1];
                    datakeyarray2[0] = "Pk";
                    // grdDespatch.DataKeyNames = datakeyarray2;

                    PageIndex = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                    //txtToDate.Text = DateTime.Now.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                    //txtFromDate.Text = DateTime.Now.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Method  For PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;

        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            // base.CheckBtnVisibility(sender);
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
                GetFieldValues(ControlEnum.GRID);
                SetFieldValues(ControlEnum.GRID);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;

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

        #endregion


        #region Detaild part entry and updation
        /// <summary>
        /// Add Item Details to Grid
        /// </summary>
        private void AddItemDetails()
        {

            //For grid quantity update
            SetPackingDetails();
            List<PackingDetailsBO> lstPackingDetails;
            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                lstPackingDetails = new List<PackingDetailsBO>();

            PackingDetailsBO objDetail = new PackingDetailsBO();
            if (hdfSLNo.Value == ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)
            {
                objDetail.PK = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                objDetail.SlNo = lstPackingDetails == null ? 1 : (lstPackingDetails.Count + 1);
                //objDetail.CustomerPK = Convert.ToInt32(ddlCustomer.SelectedValue.ToString());
                objDetail.CustomerPK = Convert.ToInt32(hdfCustomer.Value);
                objDetail.SaleOrderPK = Convert.ToInt32(ddlSaleOrderNo.SelectedValue.ToString());
                objDetail.SaleOrderText = ddlSaleOrderNo.SelectedItem.Text;
                objDetail.SaleOrderDetailPK = Convert.ToInt32(ddlSaleOrderItem.SelectedValue.ToString());
                objDetail.SaleOrderDetailText = ddlSaleOrderItem.SelectedItem.Text;
                objDetail.ItemPK = Convert.ToInt32(hdfProductPk.Value);
                objDetail.QtyPacked = Convert.ToDouble(txtQty.Text);
                objDetail.QtyApproved = Convert.ToDouble(txtQty.Text);
                objDetail.UOM = Convert.ToInt32(hdfUomPK.Value);
                objDetail.SaleOrderQty = Convert.ToDouble(hdfOrderQty.Value);

                objDetail.UomCode = txtUOM.Text;
                objDetail.BalToPack = Convert.ToDouble(hdfBalToPack.Value);
                objDetail.Store = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);
                lstPackingDetails.Add(objDetail);
                Session[ERP.Utilities.SessionStrings.PackingDetails] = lstPackingDetails;
                BindGrid(ControlEnum.PACKINGDETAILS);

            }
            else
                for (int i = 0; i < lstPackingDetails.Count; i++)
                {
                    if (lstPackingDetails[i].SlNo == Convert.ToInt32(hdfSLNo.Value))
                    {
                        lstPackingDetails[i].SaleOrderPK = Convert.ToInt32(ddlSaleOrderNo.SelectedValue.ToString());
                        lstPackingDetails[i].SaleOrderText = ddlSaleOrderNo.SelectedItem.Text;
                        lstPackingDetails[i].CustomerPK = Convert.ToInt32(ddlCustomer.SelectedValue.ToString());
                        lstPackingDetails[i].SaleOrderDetailPK = Convert.ToInt32(ddlSaleOrderItem.SelectedValue.ToString());
                        lstPackingDetails[i].SaleOrderDetailText = ddlSaleOrderItem.SelectedItem.Text;
                        lstPackingDetails[i].ItemPK = Convert.ToInt32(hdfProductPk.Value);
                        lstPackingDetails[i].QtyPacked = Convert.ToDouble(txtQty.Text);
                        lstPackingDetails[i].QtyApproved = Convert.ToDouble(txtQty.Text);
                        lstPackingDetails[i].SaleOrderQty = Convert.ToDouble(hdfOrderQty.Value);
                        lstPackingDetails[i].UOM = Convert.ToInt32(hdfUomPK.Value);
                        lstPackingDetails[i].UomCode = txtUOM.Text;
                        lstPackingDetails[i].BalToPack = Convert.ToDouble(hdfBalToPack.Value);
                        lstPackingDetails[i].Store = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);
                        Session[ERP.Utilities.SessionStrings.PackingDetails] = lstPackingDetails;
                        BindGrid(ControlEnum.PACKINGDETAILS);

                    }
                }
            ClearLine();


        }

        /// <summary>
        /// If item already exist
        /// </summary>
        private bool IsExisiItem(int saleOrderDetailPK)
        {
            List<PackingDetailsBO> lstPackingDetails;
            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                lstPackingDetails = new List<PackingDetailsBO>();

            for (int i = 0; i < lstPackingDetails.Count; i++)
            {  //hdfSaleOrderDetailPK
                if (hdfSLNo.Value != ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO && lstPackingDetails[i].SaleOrderDetailPK == Convert.ToInt32(hdfSaleOrderDetailPK.Value))
                    continue;
                if (lstPackingDetails[i].SaleOrderDetailPK == saleOrderDetailPK)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Add Item Details to Grid
        /// </summary>
        private void AddItemDetails(DataTable dtList)
        {
            List<PackingDetailsBO> lstPackingDetails = new List<PackingDetailsBO>();
            PackingDetailsBO objDetail;
            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                objDetail = new PackingDetailsBO();
                objDetail.PK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackPK].ToString());
                objDetail.CustomerPK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackCustomer].ToString());
                objDetail.CustomerName = dtList.Rows[i][Resources.DataFieldRes.PackCustomerName].ToString();
                objDetail.SlNo = (i + 1);
                objDetail.SaleOrderPK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackSaleOrderPK].ToString());
                objDetail.SaleOrderText = dtList.Rows[i][Resources.DataFieldRes.PackSaleOrderNo].ToString();
                objDetail.CustomerPK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackCustomer].ToString());
                objDetail.SaleOrderDetailPK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackSaleOrderDtlPk].ToString());
                objDetail.SaleOrderDetailText = dtList.Rows[i][Resources.DataFieldRes.BrandNameCode].ToString();
                objDetail.ItemPK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackItemPK].ToString());
                objDetail.QtyPacked = Convert.ToDouble(dtList.Rows[i][Resources.DataFieldRes.PackedQty].ToString());
                objDetail.QtyApproved = Convert.ToDouble(dtList.Rows[i][Resources.DataFieldRes.PackedQty].ToString());
                objDetail.SaleOrderQty = Convert.ToDouble(dtList.Rows[i][Resources.DataFieldRes.OrderQty].ToString());
                objDetail.UOM = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackedUomPk].ToString());
                objDetail.UomCode = dtList.Rows[i][Resources.DataFieldRes.UomCode].ToString();
                objDetail.BalToPack = Convert.ToDouble(dtList.Rows[i][Resources.DataFieldRes.BalanceToPack].ToString());
                objDetail.Store = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.PackedStore].ToString());
                lstPackingDetails.Add(objDetail);
            }
            Session[ERP.Utilities.SessionStrings.PackingDetails] = lstPackingDetails;
            BindGrid(ControlEnum.PACKINGDETAILS);

        }

        /// <summary>
        /// Add Item Details to Grid
        /// </summary>
        private void AddItemFromSaleOrder(DataTable dtList)
        {
            List<DespatchDetailsBO> lstDespatchDetails = new List<DespatchDetailsBO>();
            DespatchDetailsBO objDetail;
            for (int i = 0; i < dtList.Rows.Count; i++)
            {
                objDetail = new DespatchDetailsBO();
                objDetail.DPD_PK = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                //objDetail.DPD_SL_NO = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.Disp_RowNo].ToString());
                objDetail.DPD_SL_NO = (i + 1);
                objDetail.DPD_SALE_ORDER = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.SaleOrderPK].ToString());
                objDetail.DPD_SALE_ORDER_NO = dtList.Rows[i][Resources.DataFieldRes.SaleOrder].ToString();
                objDetail.DPD_SALE_ORDER_DTL = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.OrderDtlPK].ToString());
                objDetail.DPD_ITEM = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.productPK].ToString());
                objDetail.PRO_TEXT = dtList.Rows[i][Resources.DataFieldRes.ProductDateText].ToString();
                objDetail.DPD_QTY_DESPATCHED = Convert.ToDouble(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                objDetail.DPD_QTY_APPROVED = Convert.ToDouble(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                objDetail.DPD_UOM = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.UomPK].ToString());
                objDetail.UOM_CODE = dtList.Rows[i][Resources.DataFieldRes.Disp_UomCode].ToString();
                objDetail.STORE = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);
                objDetail.SOD_BAL_TO_DISPATCH = Convert.ToDouble(dtList.Rows[i][Resources.DataFieldRes.BalanceToDispatch].ToString());

                lstDespatchDetails.Add(objDetail);
            }
            Session[SessionString.DespatchDetails] = lstDespatchDetails;
            BindGrid(ControlEnum.DESPATCH);

        }


        private void ClearLine()
        {
            txtQty.Text = string.Empty;
            hdfBalToPack.Value = "0";
            hdfOrderQty.Value = "0";
            txtOrderQty.Text = string.Empty;
            ddlSaleOrderNo.SelectedIndex = ddlSaleOrderNo.Items.IndexOf(ddlSaleOrderNo.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL));
            ddlSaleOrderItem.Items.Clear();
            ddlSaleOrderItem.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECTVAL));
            ddlSaleOrderItem.SelectedIndex = ddlSaleOrderItem.Items.IndexOf(ddlSaleOrderItem.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL));
            txtBalToPack.Text = string.Empty;
            txtUOM.Text = string.Empty;
            hdfSLNo.Value = "0";

        }

        private void ResetEntryForm()
        {
            ClearLine();
            ddlSaleOrderNo.Items.Clear();
            ddlSaleOrderNo.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECTVAL));
            ddlSaleOrderNo.SelectedIndex = ddlSaleOrderNo.Items.IndexOf(ddlSaleOrderNo.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL));
            Session.Remove(ERP.Utilities.SessionStrings.PackingDetails);
            BindGrid(ControlEnum.PACKINGDETAILS);
            txtDate.Text = System.DateTime.Today.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL));
            txtRemarks.Text = string.Empty;
            pkingPK = 0;
            hdfLastModDate.Value = "0";
            hdfSLNo.Value = "0";
            ddlCustomer.Focus();
            ddlCustomer.Enabled = true;
            txtCustomer.Text = string.Empty;

        }

        //For sett Packing details
        private void SetPackingDetails()
        {

            List<PackingDetailsBO> lstPackingDetails;
            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                lstPackingDetails = new List<PackingDetailsBO>();
            TextBox txtGridQty;
            int i = 0;
            foreach (GridViewRow item in grdPackingDetails.Rows)
            {
                txtGridQty = (TextBox)item.FindControl("txtQty");
                DespatchDetailsBO objDetail = new DespatchDetailsBO();
                lstPackingDetails[i].QtyPacked = Convert.ToDouble(txtGridQty.Text);
                lstPackingDetails[i++].QtyApproved = Convert.ToDouble(txtGridQty.Text);


            }

            Session[SessionString.DespatchDetails] = lstPackingDetails;

        }


        /// <summary>
        /// Delete Item From the Item List
        /// </summary>
        private void DeleteItemDetails(int slNo)
        {
            List<PackingDetailsBO> lstPackingDetails;
            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                lstPackingDetails = new List<PackingDetailsBO>();

            if (lstPackingDetails.Count > 0)
            {
                for (int i = 0; i < lstPackingDetails.Count; i++)
                {
                    if (lstPackingDetails[i].SlNo == slNo)
                    {
                        lstPackingDetails.RemoveAt((slNo - 1));
                    }
                }
                Session[ERP.Utilities.SessionStrings.PackingDetails] = lstPackingDetails;
                UpdateItemList();
                BindGrid(ControlEnum.PACKINGDETAILS);

            }



        }

        /// <summary>
        /// Delete Item From the Item List
        /// </summary>
        private void FillItemDetails(int slNo)
        {

            List<PackingDetailsBO> lstPackingDetails;
            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                lstPackingDetails = new List<PackingDetailsBO>();

            if (lstPackingDetails.Count > 0)
            {
                for (int i = 0; i < lstPackingDetails.Count; i++)
                {
                    if (lstPackingDetails[i].SlNo == slNo)
                    {
                        hdfCustomerPK.Value = lstPackingDetails[i].CustomerPK.ToString();
                        hdfCustomer.Value = lstPackingDetails[i].CustomerPK.ToString();
                        GetFieldValues(ControlEnum.SALEORDER);
                        SetFieldValues(ControlEnum.SALEORDER);
                        ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(lstPackingDetails[i].CustomerPK.ToString()));
                        hdfCustomerPK.Value = "0";
                        ddlSaleOrderNo.SelectedIndex = ddlSaleOrderNo.Items.IndexOf(ddlSaleOrderNo.Items.FindByValue(lstPackingDetails[i].SaleOrderPK.ToString()));
                        GetFieldValues(ControlEnum.SALEORDERITEM);
                        SetFieldValues(ControlEnum.SALEORDERITEM);
                        ddlSaleOrderItem.SelectedIndex = ddlSaleOrderItem.Items.IndexOf(ddlSaleOrderItem.Items.FindByValue(lstPackingDetails[i].SaleOrderDetailPK.ToString()));
                        hdfSaleOrderDetailPK.Value = lstPackingDetails[i].SaleOrderDetailPK.ToString();
                        GetFieldValues(ControlEnum.UOM);
                        SetFieldValues(ControlEnum.UOM);
                        hdfSLNo.Value = slNo.ToString();
                        txtQty.Text = lstPackingDetails[i].QtyPacked.ToString();
                        hdfBalToPack.Value = lstPackingDetails[i].BalToPack.ToString();
                        txtBalToPack.Text = lstPackingDetails[i].BalToPack.ToString();
                        txtCustomer.Text = lstPackingDetails[i].CustomerName.ToString();
                    }
                }
            }


        }


        // 
        /// <summary>
        /// Update Sl Number for A list After delete an item
        /// </summary>
        private void UpdateItemList()
        {
            List<PackingDetailsBO> lstPackingDetails;
            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                lstPackingDetails = new List<PackingDetailsBO>();
            if (lstPackingDetails.Count > 0)
            {

                for (int i = 0; i < lstPackingDetails.Count; i++)
                {
                    lstPackingDetails[i].SlNo = (i + 1);
                }

            }
            Session[ERP.Utilities.SessionStrings.PackingDetails] = lstPackingDetails;
            BindGrid(ControlEnum.PACKINGDETAILS);
        }
        #endregion

        #region Healper methods

        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            txtFromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
        }

        /// <summary>
        /// Method for Print db messages
        /// </summary>
        private bool PrintDbMessages(DataSet dsMsg, ActionsEnum commonAction)
        {
            bool RetVal = false;
            string strHdrMsg;
            if (dsMsg != null && dsMsg.Tables.Count > 0 && dsMsg.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetVal].ToString()) > 0)
                {
                    RetVal = true;
                    //strHdrMsg = "Information Saved";
                    //BindGrid(ControlEnum.DBMSG);
                    //DiverrorMessages.Attributes.Remove("class");
                    //DiverrorMessages.Attributes.Add("class", "message-sucess");
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300');", true);
                    switch (commonAction)
                    {
                        case ActionsEnum.SAVE :
                            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                            break;
                        case ActionsEnum.DELETE_LIST_ACTION :
                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                            break;
                    }
                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PackingTransaction);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                }
                else
                {
                    litErrorMsg.Text = dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetValTxt].ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                    //strHdrMsg = "Error Message";
                    //BindGrid(ControlEnum.DBMSG);
                    //DiverrorMessages.Attributes.Remove("class");
                    //DiverrorMessages.Attributes.Add("class", "message-error");
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAssetType", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300');", true);
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlEnum type)
        {
            try
            {
                switch (type)
                {

                    case ControlEnum.PACKINGDETAILS:
                    case ControlEnum.DESPATCH:
                        List<PackingDetailsBO> lstPackingDetails;
                        if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                            lstPackingDetails = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
                        else
                            lstPackingDetails = new List<PackingDetailsBO>();
                        if (lstPackingDetails.Count > 0)
                            grdPackingDetails.DataSource = lstPackingDetails;
                        else
                            grdPackingDetails.DataSource = null;
                        grdPackingDetails.DataBind();

                        break;

                    case ControlEnum.DBMSG:
                        grdError.DataSource = dsDBMessgaes;
                        grdError.DataBind();
                        break;
                    case ControlEnum.GRID:
                        if (dsPage != null && dsPage.Tables.Count > 0)
                        {
                            TotalPages = (dsPage.Tables[0].Rows.Count > 0 ?
                                Convert.ToInt32(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString()) : 0);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? ERP.Utilities.CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdPackingList.DataSource = dsPage.Tables[0];
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        grdPackingList.DataBind();

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Bind DropDown
        /// </summary>
        public void BindDropDown(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.CUSTOMER:
                        ddlCustomer.Items.Clear();
                        if (dsPage != null && dsPage.Tables.Count > 0)
                        {
                            ddlCustomer.DataSource = dsPage.Tables[0];
                            ddlCustomer.DataTextField = Resources.DataFieldRes.CustomerName;
                            ddlCustomer.DataValueField = Resources.DataFieldRes.CusPK;
                            ddlCustomer.DataBind();
                        }
                        ddlCustomer.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                        ddlCustomer.SelectedIndex = Convert.ToInt32(ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));
                        break;
                    case ControlEnum.SHIFT:
                        if (dsPage != null && dsPage.Tables.Count > 0)
                        {
                            ddlShift.DataSource = dsPage.Tables[2];
                            ddlShift.DataTextField = Resources.DataFieldRes.ShiftName;
                            ddlShift.DataValueField = Resources.DataFieldRes.ShiftPK;
                            ddlShift.DataBind();
                        }
                        ddlShift.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                        ddlShift.SelectedIndex = Convert.ToInt32(ddlShift.Items.IndexOf(ddlShift.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));
                        break;
                    case ControlEnum.SALEORDER:
                        ddlSaleOrderNo.Items.Clear();
                        if (dsPage != null && dsPage.Tables.Count > 0 && dsPage.Tables[0].Rows.Count > 0)
                        {
                            ddlSaleOrderNo.DataSource = dsPage.Tables[0];
                            ddlSaleOrderNo.DataTextField = Resources.DataFieldRes.SaleOrder;
                            ddlSaleOrderNo.DataValueField = Resources.DataFieldRes.SaleOrderPK;
                            ddlSaleOrderNo.DataBind();
                        }
                        ddlSaleOrderNo.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                        ddlSaleOrderNo.SelectedIndex = Convert.ToInt32(ddlSaleOrderNo.Items.IndexOf(ddlSaleOrderNo.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));

                        break;
                    case ControlEnum.SALEORDERITEM:
                        ddlSaleOrderItem.Items.Clear();
                        if (dsPage != null && dsPage.Tables.Count > 0)
                        {
                            ddlSaleOrderItem.DataSource = dsPage.Tables[0];
                            ddlSaleOrderItem.DataTextField = Resources.DataFieldRes.BrandNameCode;
                            ddlSaleOrderItem.DataValueField = Resources.DataFieldRes.SaleODPK;
                            ddlSaleOrderItem.DataBind();
                        }
                        ddlSaleOrderItem.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                        ddlSaleOrderItem.SelectedIndex = Convert.ToInt32(ddlSaleOrderItem.Items.IndexOf(ddlSaleOrderItem.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));
                        break;

                    case ControlEnum.UOM:
                        //ddlUOM.Items.Clear();
                        //if (dsPage != null && dsPage.Tables.Count > 0)
                        //{
                        //    ddlUOM.DataSource = dsPage.Tables[0];
                        //    ddlUOM.DataTextField = Resources.DataFieldRes.UomCode;
                        //    ddlUOM.DataValueField = Resources.DataFieldRes.UomPK;
                        //    ddlUOM.DataBind();
                        //}
                        //ddlUOM.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                        //ddlUOM.SelectedIndex = Convert.ToInt32(ddlUOM.Items.IndexOf(ddlUOM.Items.FindByValue(CurrPK.ToString())));
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        protected void grdPackingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsPage != null)
            {
                if (dsPage.Tables[0].Rows.Count > 0)
                {

                    ((Label)e.Row.FindControl("lbllQtyTotal")).Text = Convert.ToDecimal(dsPage.Tables[0].Rows[0][Resources.DataFieldRes.SalePackedQtyTotal]).ToString("n0");

                }

            }
        }


        private void ClearDDL()
        {
            try
            {
                ddlShift.SelectedIndex = Convert.ToInt32(ddlShift.Items.IndexOf(ddlShift.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));
                ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                txtDate.Text = System.DateTime.Today.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                ddlSaleOrderItem.Items.Clear();
                ddlSaleOrderItem.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                ddlSaleOrderItem.SelectedIndex = Convert.ToInt32(ddlSaleOrderItem.Items.IndexOf(ddlSaleOrderItem.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));
                ddlSaleOrderNo.Items.Clear();
                ddlSaleOrderNo.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO));
                ddlSaleOrderNo.SelectedIndex = Convert.ToInt32(ddlSaleOrderNo.Items.IndexOf(ddlSaleOrderNo.Items.FindByValue(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)));

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Assign Fetch Filter XML
        /// </summary>
        /// <param name="type"></param>
        private void SetFilterXML(ControlEnum type)
        {
            XmlDocument xmlDoc;
            DespatchBO objDespatch;
            CustomerBO objCustomer;
            NumberBO objNumber;
            SalesOrderBO objSaleOrder;
            DespatchDetailsBO objDespatchDetails;
            NextDocNoBO objNextDocNo;
            ShifMasterDDlBO objShifMasterDDl;
            PackingTransactionBO objPackingTransaction;

            P_XML = new string[8];
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (type)
            {
                case ControlEnum.HEADER:
                    objCustomer = new CustomerBO();
                    objCustomer.Active = (int)ActiveStatus.ACTIVE;
                    objCustomer.BizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objCustomer);
                    P_XML[1] = xmlDoc.InnerXml;

                    objNumber = new NumberBO();
                    objNumber.BizUnit = CurrentUser.SBUID;
                    objNumber.DocType = (int)Mode.Packing;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objNumber);
                    P_XML[2] = xmlDoc.InnerXml;

                    objShifMasterDDl = new ShifMasterDDlBO();
                    objShifMasterDDl.Active = (int)ActiveStatus.ACTIVE;
                    objShifMasterDDl.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShifMasterDDl);
                    P_XML[3] = xmlDoc.InnerXml;
                    break;

                case ControlEnum.SALEORDER:
                    objSaleOrder = new SalesOrderBO();
                    objSaleOrder.BizUnit = CurrentUser.SBUID;
                    objSaleOrder.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                   // objSaleOrder.CustomerPK = hdfCustomerPK.Value == "0" ? ddlCustomer.SelectedValue.ToString() : hdfCustomerPK.Value;
                    objSaleOrder.CustomerPK = string.IsNullOrEmpty(hdfCustomer.Value) ? "0" : hdfCustomer.Value;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleOrder);
                    P_XML[4] = xmlDoc.InnerXml;
                    break;

                case ControlEnum.SALEORDERITEM:
                    objSaleOrder = new SalesOrderBO();
                    objSaleOrder.BizUnit = CurrentUser.SBUID;
                    objSaleOrder.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    objSaleOrder.PK = ddlSaleOrderNo.SelectedValue.ToString();
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleOrder);
                    P_XML[5] = xmlDoc.InnerXml;
                    break;

                case ControlEnum.UOM:
                    objSaleOrder = new SalesOrderBO();
                    objSaleOrder.BizUnit = CurrentUser.SBUID;
                    objSaleOrder.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    objSaleOrder.dtlPK = ddlSaleOrderItem.SelectedValue.ToString();
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleOrder);
                    P_XML[5] = xmlDoc.InnerXml;
                    break;

                case ControlEnum.GRID:
                    objPackingTransaction = new PackingTransactionBO();
                    objPackingTransaction.BizUnit = CurrentUser.SBUID;
                    objPackingTransaction.Active = (int)ActiveStatus.ACTIVE;
                    objPackingTransaction.PageNo = PageIndex == null ? "1" : PageIndex;

                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPackingTransaction);
                    P_XML[0] = xmlDoc.InnerXml;
                    break;
                case ControlEnum.SEARCH:
                    objPackingTransaction = new PackingTransactionBO();
                    objPackingTransaction.BizUnit = CurrentUser.SBUID;
                    objPackingTransaction.Active = (int)ActiveStatus.ACTIVE;
                    objPackingTransaction.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO; ;
                    objPackingTransaction.PackingDateFrom = txtFromDate.Text == string.Empty ? DateTime.Now.ToString() : txtFromDate.Text;
                    objPackingTransaction.PackingDateTo = txtToDate.Text == string.Empty ? DateTime.Now.ToString() : txtToDate.Text;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPackingTransaction);
                    P_XML[0] = xmlDoc.InnerXml;
                    break;
                case ControlEnum.PACKINGDETAILS:

                    objPackingTransaction = new PackingTransactionBO();
                    objPackingTransaction.PK = pkingPK;
                    objPackingTransaction.BizUnit = CurrentUser.SBUID;
                    objPackingTransaction.Active = (int)ActiveStatus.HASPK;
                    objPackingTransaction.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPackingTransaction);
                    P_XML[0] = xmlDoc.InnerXml;

                    objPackingTransaction = new PackingTransactionBO();
                    objPackingTransaction.PK = pkingPK;
                    objPackingTransaction.BizUnit = CurrentUser.SBUID;
                    objPackingTransaction.Active = (int)ActiveStatus.HASPK;
                    objPackingTransaction.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPackingTransaction);
                    P_XML[6] = xmlDoc.InnerXml;
                    break;
                //case ControlEnum.DESPATCH:
                //    objDespatch = new DespatchBO();
                //    objDespatch.DespatchPK = CurrPK.ToString();
                //    objDespatch.BizUnit = CurrentUser.SBUID;
                //    objDespatch.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objDespatch);
                //    P_XML[2] = xmlDoc.InnerXml;

                //    objDespatch.Active = ((int)ActiveStatus.HASPK).ToString();
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objDespatch);
                //    P_XML[0] = xmlDoc.InnerXml;

                //    objCustomer = new CustomerBO();
                //    if (CustomerPk > 0)
                //    {
                //        objCustomer.Active = (int)ActiveStatus.HASPK;
                //        objCustomer.CustomerPK = CustomerPk.ToString();
                //    }
                //    else
                //        objCustomer.Active = (int)ActiveStatus.ACTIVE;
                //    objCustomer.BizUnit = CurrentUser.SBUID;

                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objCustomer);
                //    P_XML[1] = xmlDoc.InnerXml;
                //    break;

                //case ControlEnum.CUSTOMER:
                //    objCustomer = new CustomerBO();
                //    objCustomer.Active = (int)ActiveStatus.ACTIVE;
                //    objCustomer.BizUnit = CurrentUser.SBUID;
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objCustomer);
                //    P_XML[1] = xmlDoc.InnerXml;

                //    objNumber = new NumberBO();
                //    objNumber.BizUnit = CurrentUser.SBUID;
                //    objNumber.DocType = (int)Mode.Dispatch;
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objNumber);
                //    P_XML[3] = xmlDoc.InnerXml;
                //    break;

                //case ControlEnum.SALEORDERITEM:
                //    objSaleOrder = new SalesOrderBO();
                //    objSaleOrder.BizUnit = CurrentUser.SBUID;
                //    objSaleOrder.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                //    objSaleOrder.PK = ddlSaleOrderNo.SelectedValue.ToString();
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleOrder);
                //    P_XML[4] = xmlDoc.InnerXml;

                //    break;

                //case ControlEnum.LISTING:
                //    objDespatch = new DespatchBO();
                //    objDespatch.Active = ((int)ActiveStatus.ACTIVE).ToString();
                //    objDespatch.BizUnit = CurrentUser.SBUID;
                //    objDespatch.PageNo = PageIndex;

                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objDespatch);
                //    P_XML[0] = xmlDoc.InnerXml;
                //    break;

                //case ControlEnum.EDIT_DISPATCH_DETAILS:
                //    objCustomer = new CustomerBO();
                //    objCustomer.Active = (int)ActiveStatus.ACTIVE;
                //    objCustomer.BizUnit = CurrentUser.SBUID;
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objCustomer);
                //    P_XML[1] = xmlDoc.InnerXml;

                //    objDespatch = new DespatchBO();
                //    objDespatch.BizUnit = CurrentUser.SBUID;
                //    objDespatch.Active = ((int)ActiveStatus.HASPK).ToString();
                //    objDespatch.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                //    objDespatch.DespatchPK = dphPK.ToString();
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objDespatch);
                //    P_XML[0] = xmlDoc.InnerXml;

                //    objDespatch = new DespatchBO();
                //    objDespatch.DespatchPK = dphPK.ToString();
                //    objDespatch.BizUnit = CurrentUser.SBUID;
                //    objDespatch.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;

                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objDespatch);
                //    P_XML[2] = xmlDoc.InnerXml;


                //    break;
                //case ControlEnum.HEADER:
                //    //Next Doc Number
                //    objNumber = new NumberBO();
                //    objNumber.BizUnit = CurrentUser.SBUID;
                //    objNumber.DocType = (int)Mode.Dispatch;
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objNumber);
                //    P_XML[3] = xmlDoc.InnerXml;
                //    break;
                //case ControlEnum.DISPATCH_FROM_ORDER:
                //    SalesOrderBO objSaleorder = new SalesOrderBO();
                //    objSaleorder.BizUnit = CurrentUser.SBUID;
                //    objSaleorder.PageNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                //    objSaleorder.UserPK = CurrentUser.UserPK.ToString();
                //    objSaleorder.DeptPK = CurrentUser.CurrentDeptPK.ToString();
                //    objSaleorder.OrderItemsList = new List<OrderItemsBO>();
                //    objSaleorder.OrderItemsList.Add((OrderItemsBO)Session[ERP.Utilities.SessionStrings.SelectedOrders]);
                //    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objSaleorder);
                //    P_XML[4] = xmlDoc.InnerXml;
                //    Session.Remove(ERP.Utilities.SessionStrings.SelectedOrders);

                //    break;


            }
        }

        /// <summary>
        /// Assign Save XML
        /// </summary>
        /// <param name="action"></param>
        private void SetFieldValuesToXML(ActionsEnum action)
        {
            XmlDocument xmlDoc;
            PackingTransactionBO objPackingTransaction;
            P_XML = new string[7];
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (action)
            {
                case ActionsEnum.SAVE:
                    objPackingTransaction = new PackingTransactionBO();
                    objPackingTransaction.PK = pkingPK;
                    objPackingTransaction.Packing_NO = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    objPackingTransaction.RefNo = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    objPackingTransaction.Active = Convert.ToInt32(ActiveStatus.ACTIVE);
                    objPackingTransaction.BizUnit = CurrentUser.SBUID;
                    objPackingTransaction.DeptPK = CurrentUser.CurrentDeptPK;
                    objPackingTransaction.UserPK = CurrentUser.PKUser;
                    objPackingTransaction.Status = ERP.Utilities.CommonConstants.SELECT_VALUE_ONE;
                    objPackingTransaction.Mode = Convert.ToInt32(Mode.Packing);
                    objPackingTransaction.Module = Convert.ToInt32(ConfigurationSettings.AppSettings["gERPModule"].ToString());
                    objPackingTransaction.PackingDate = txtDate.Text;
                    objPackingTransaction.LastModDate = hdfLastModDate.Value == ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO ? DateTime.Now.ToString() : hdfLastModDate.Value;
                    objPackingTransaction.Remarks = txtRemarks.Text;
                    objPackingTransaction.ShiftPK = ddlShift.SelectedValue.ToString();
                    objPackingTransaction.PackingDetailList = new List<PackingDetailBO>();
                    objPackingTransaction.PackingDetailList.Add(GetDespatchtDetails());
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPackingTransaction);
                    P_XML[0] = xmlDoc.InnerXml;
                    break;
                case ActionsEnum.DELETE_LIST_ACTION:
                    objPackingTransaction = new PackingTransactionBO();
                    objPackingTransaction.PK = pkingPK;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objPackingTransaction);
                    P_XML[1] = xmlDoc.InnerXml;
                    break;
            }

        }

        private PackingDetailBO GetDespatchtDetails()
        {
            PackingDetailBO objPackingDetails = new PackingDetailBO();

            List<PackingDetailsBO> PackingList = new List<PackingDetailsBO>();

            if (Session[ERP.Utilities.SessionStrings.PackingDetails] != null)
                PackingList = (List<PackingDetailsBO>)Session[ERP.Utilities.SessionStrings.PackingDetails];
            else
                PackingList = new List<PackingDetailsBO>();
            objPackingDetails.PackingDetailsList = PackingList;
            return objPackingDetails;
        }

        /// <summary>
        /// Set DespatchDetails From DataSet
        /// </summary>
        /// <param name="dtDetails"></param>
        private void SetDespatchDetails(DataTable dtDetails)
        {
            DespatchDetails = new List<DespatchDetailsBO>();
            if (dtDetails != null && dtDetails.Rows.Count > 0)
            {
                DespatchDetails = dtDetails.AsEnumerable().Select(itm => new DespatchDetailsBO()
                {
                    //Pk = itm.Field<int>("DPD_PK"),
                    //SlNo = itm.Field<int>("DPD_SL_NO"),
                    //SaleOrderPk = itm.Field<int>("DPD_SALE_ORDER"),
                    //SaleOrderItemPk = itm.Field<int>("DPD_SALE_ORDER_DTL"),
                    //ProductPk = itm.Field<int>("DPD_ITEM"),
                    //DespatchedQty = itm.Field<double>("DPD_QTY_DESPATCHED"),
                    //ApprovedQty = itm.Field<double>("DPD_QTY_APPROVED"),
                    //UomPk = itm.Field<int>("DPD_UOM"),
                    //Store = itm.Field<int>("DPD_STORE"),
                    //HdrPk = itm.Field<int>("DPD_DESPATCH_HDR"),
                    //HdrDate = itm.Field<DateTime>("DPD_DATE"),
                    //HdrNo = itm.Field<string>("DPD_NO"),
                    //SaleOrderNo = itm.Field<string>("DPD_SALE_ORDER_NO"),
                    //ProductName = itm.Field<string>("PRO_NAME"),
                    //SaleOrderItemDate = itm.Field<DateTime>("SOD_REQUIRED_BY"),
                    //UomName = itm.Field<string>("UOM_CODE")
                }).ToList();
            }
        }

        #endregion

        #region DB methods

        /// <summary>
        /// To Save Values to DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="XML"></param>
        /// <returns></returns>
        public DataSet SetValuesToDB(int val, string[] XML)
        {
            return PackingTransactionDL.SavePackingDtl(val, XML);
        }

        /// <summary>
        /// To Fetch Values from DB
        /// </summary>
        /// <param name="val"></param>
        /// <param name="XML"></param>
        /// <returns></returns>
        public DataSet FetchDbValues(int val, string[] XML)
        {
            return PackingTransactionDL.GetPackingDtl(val, P_XML);
        }

        #endregion

        #region Control Enum
        private enum ActionsEnum
        {
            ADD_ACTION,
            CANCEL_ACTION,
            EDIT_ACTION,
            DELETE_ACTION,
            SAVE,
            CANCEL,
            MOVE,
            PACKING,
            ADD_PACKING,
            CHANGE,
            EDIT_LIST_ACTION,
            DELETE_LIST_ACTION,
            SEARCH,
            CLEAR,
            CUSTOMERSELECTED
        }
        #endregion
    }
}