using BusinessLogic.OrderPlanning;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject.OrderPlanningBO;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;


namespace CustomerPortal.UserControls
{
    public partial class PendingOrders : System.Web.UI.UserControl
    {
      
       
        #region Variables and Properties
        #region Event
        public event EventHandler ShowPendingOrder;
        public event EventHandler AfterApply;
        #endregion
        private ActionsEnum commonActions;
        private User currentUser;
        private PendingOrderBO objPendingOrders;
        private List<PendingOrderList> objPendingOrderLst;
        private PendingOrderBO objALLPendingOrders;
        private List<PendingOrderList> SelectedPOList;
        private List<PendingOrderList> SelectedOrderList;
        private ProductPropertyBO objProductProperties;
        private List<ItemSpecDetails> TempItemDtlList;
        private ItemSpecGroup objItemGroups;

        private DataTable dtPlanGroup;

        HiddenField hdfDecimalCoundMst;
        private int PageSize = 0;
        private int GroupValue;
        private int PlanGroupPK = 0;
        public bool isVisibleOrder = false;
        public bool isVisibleAllocation = false;
        public bool isVisibleRelease = false;
        private DataTable dtPage = null;
        private int ScPk = 0;
        private List<Int32> subList_Properties;  //Public variable 
        XmlDocument xmlDoc;
        int result;
        double Gridtotal = 0;

        #region Properties
        /// To maintain the WeightFormat in viewstate
        /// </summary>
        private string WeightFormat
        {
            get
            {
                return (string)this.ViewState["WeightFormat"];
            }
            set
            {
                this.ViewState["WeightFormat"] = value;
            }
        }
        private string DecimalFormat
        {
            get
            {
                return (string)this.ViewState["DecimalFormat"];
            }
            set
            {
                this.ViewState["DecimalFormat"] = value;
            }

        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        public string POPageIndex
        {
            get
            {
                return (string)this.ViewState["POPageIndex"];
            }
            set
            {
                this.ViewState["POPageIndex"] = value;
            }
        }

        /// <summary>
        /// object for selected orders , for maintaining selection while paging
        /// </summary>
        public List<PendingOrderList> SelectedList
        {
            get
            {
                return (List<PendingOrderList>)(this.ViewState["SelectedList"]);
            }
            set
            {
                this.ViewState["SelectedList"] = value;
            }
        }

        /// <summary>
        /// object for selected orders , for maintaining selection of all items
        /// </summary>
        public List<PendingOrderList> objALLPendingOrderLst
        {
            get
            {
                return (List<PendingOrderList>)(this.ViewState["objALLPendingOrderLst"]);
            }
            set
            {
                this.ViewState["objALLPendingOrderLst"] = value;
            }
        }
        /// <summary>
        /// To store SO numbers for checkbox
        /// </summary>
        private PendingOrderBO PendingOrderBO
        {
            get
            {
                return (PendingOrderBO)(this.ViewState["PendingOrderBO"]);
            }
            set
            {
                this.ViewState["PendingOrderBO"] = value;
            }
        }
        /// <summary>
        /// To store search condition
        /// </summary>
        private string xmlSrchString
        {
            get
            {
                return (string)(this.ViewState["xmlSrchString"]);
            }
            set
            {
                this.ViewState["xmlSrchString"] = value;
            }
        }
        /// <summary>
        /// To store Order planning PK to exclude selected sc dtls
        /// </summary>
        public int OrderPlanPK
        {
            get
            {
                return (int)(this.ViewState["OrderPlanPK"]);
            }
            set
            {
                this.ViewState["OrderPlanPK"] = value;
            }
        }

        /// <summary>
        /// To store SO numbers for checkbox
        /// </summary>
        private Params ParamsBO
        {
            get
            {
                return this.ViewState["Params"] == null ? new Params() : (Params)(this.ViewState["Params"]);
            }
            set
            {
                this.ViewState["Params"] = value;
            }
        }

        /// <summary>
        /// To store SO numbers for checkbox
        /// </summary>
        private Params SessionParamsBO
        {
            get
            {
                return this.Session["SessionParamsBO"] == null ? new Params() : (Params)(this.Session["SessionParamsBO"]);
            }
            set
            {
                this.Session["SessionParamsBO"] = value;
            }
        }
        /// <summary>
        /// To identify allocation details
        /// </summary>
        public int IsAllocation
        {
            get
            {
                return (this.ViewState["IsAllocation"]) == null ? 0 : (int)(this.ViewState["IsAllocation"]);
            }
            set
            {
                this.ViewState["IsAllocation"] = value;
            }
        }
        #endregion
        #endregion

        #region Set Page Variables
        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            currentUser = (User)HttpContext.Current.User.Identity;
        }
        #endregion

        #region ActionHandlers
        #region -- For Buttons ---
        /// <summary>
        /// For Button And ImageButton Click (Save/Delete And Edit/Delete GO (Grid))
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvr;
            if (sender is Button)
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender is LinkButton)
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            else if (sender is ImageButton)
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                commonActions = ActionsEnum.CHANGE;
            }
            else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
            {
                commonActions = ActionsEnum.CHECK_CHANGE;
            }
            switch (commonActions)
            {
                #region POPUP-FILTER
                case ActionsEnum.POPUPADD:
                    uclPagingPO.CurrentPage = 1;
                    SetFieldValues(ControlsEnum.PRODUCTPROPERTY);
                    SetFieldValues(ControlsEnum.ORDERNUMBER);
                    SetFieldValues(ControlsEnum.PRODUCTPLANGROUP);
                    SetFieldValues(ControlsEnum.SBU);
                    SetFieldValues(ControlsEnum.CUSTOMER);

                    foreach (ListItem item in chkSBU.Items)
                        item.Selected = true;
                    if (chkSBU.Items.Count > 0)
                        chkHdrSbu.Visible = chkHdrSbu.Checked = true;
                    else
                        chkHdrSbu.Visible = false;


                    //foreach (ListItem item in chkProductGroup.Items)
                    //    item.Selected = true;
                    //if (chkProductGroup.Items.Count > 0)
                    //    chkHdrPlanGroup.Visible = chkHdrPlanGroup.Checked = true;
                    //else
                    //    chkHdrPlanGroup.Visible = false;


                    foreach (ListItem item in chkOrders.Items)
                        item.Selected = true;
                    if (chkOrders.Items.Count > 0)
                        chkHdrOrders.Visible = chkHdrOrders.Checked = true;
                    else
                        chkHdrOrders.Visible = false;


                    foreach (ListItem item in chkCustomer.Items)
                        item.Selected = true;
                    if (chkCustomer.Items.Count > 0)
                        chkHdrCustomer.Visible = chkHdrCustomer.Checked = true;
                    else
                        chkHdrCustomer.Visible = false;

                    //chkWithStock.Checked = true;


                    if (chkWithStock.Checked == true && chkFullyAllocated.Checked == true && chkFullyPlanned.Checked == true)
                        chkSelectAll.Checked = true;
                    else
                        chkSelectAll.Checked = false;

                    ShowAdvanceFilterPopUp();
                    break;
                #endregion

                #region APPLY
                case ActionsEnum.APPLY:
                    if (((Button)sender).ID == "btnSelectOrder")
                    {
                        Button dummyButton = new Button();
                        dummyButton.CommandName = ActionsEnum.AFTERAPPLY.ToString();
                        dummyButton.CommandArgument = ((Button)sender).CommandArgument;
                        if (AfterApply != null)
                            this.AfterApply(dummyButton, EventArgs.Empty);
                    }
                    else if (((Button)sender).ID == "btnApplyFilter")
                    {
                        if (IsAllocation == 0)
                            SessionParamsBO = new Params();

                        POPageIndex = "1";
                        ParamsBO = null;
                        GetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                        SetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ClosePopup", "ClosePopup();", true);
                    }
                    if (((Button)sender).ID == "btnSelectAllocation")
                    {
                        Button dummyButton = new Button();
                        dummyButton.CommandName = ActionsEnum.AFTERAPPLY.ToString();
                        dummyButton.CommandArgument = ((Button)sender).CommandArgument;
                        if (AfterApply != null)
                            this.AfterApply(dummyButton, EventArgs.Empty);
                    }
                    break;
                #endregion

                #region CANCEL
                case ActionsEnum.CANCEL:
                    if (((Button)sender).ID == "btnPopupCancel")
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ClosePopup", "ClosePopup();", true);
                    else if (((Button)sender).ID == "btnCancelReleasePopUp")
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ClosePopup", "ClosePopup();", true);
                    else if (((Button)sender).ID == "btnCancelBinDtls")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ClosePopup", "ClosePopup();", true);
                        ShowReleasePopup();
                    }
                    break;
                #endregion

                #region CLEAR
                case ActionsEnum.CLEAR:
                    POPageIndex = "1";
                    ParamsBO = null;
                    txtDispatchDate.Text = string.Empty;
                    chkWithStock.Checked = false;
                    chkFullyAllocated.Checked = true;
                    chkFullyPlanned.Checked = false;
                    if (chkWithStock.Checked == true && chkFullyAllocated.Checked == true && chkFullyPlanned.Checked == true)
                        chkSelectAll.Checked = true;
                    else
                        chkSelectAll.Checked = false;
                    GetFieldValues(ControlsEnum.CLEARLIST);
                    SetFieldValues(ControlsEnum.CLEARLIST);
                    ActionHandler(btnShowFilter, EventArgs.Empty);
                    break;
                #endregion

                #region DEALLOCATION
                case ActionsEnum.DEALLOCATION:
                    result = 0;
                    DeAllocationBO temDeAllocationBO = new DeAllocationBO();
                    temDeAllocationBO = (DeAllocationBO)SetUIValuesToObject(ActionsEnum.DEALLOCATION);
                    xmlDoc = CommonFunctions.ObjectTOXml(temDeAllocationBO);
                    result = OrderPlanningBL.SaveDeAllocation(xmlDoc.InnerXml);
                    if (result > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + (GetLocalResourceObject("Msg_DeallocatedSuccessfully").ToString()) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                        GetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                        SetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                    }
                    else
                    {
                        switch (Convert.ToInt32(result))
                        {
                            case (int)DbSaveStatus.SQLERROR:
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Sql_Error) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                                break;
                            default:
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_SavError) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                                break;
                        }
                    }
                    break;
                #endregion

                #region RELEASE
                case ActionsEnum.RELEASE:
                    GetFieldValues(ControlsEnum.RELEASESC);
                    SetFieldValues(ControlsEnum.RELEASESC);
                    ShowReleasePopup();
                    break;
                #endregion

                #region DEALLOCATION
                case ActionsEnum.RELEASESAVE:
                    int countcheck = 0;
                    foreach (GridViewRow gvr1 in grdReleaseSc.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gvr1.FindControl("chkSelect") as CheckBox;
                        if (chkSelect.Checked == true)
                        {
                            countcheck = 1;
                            break;
                        }
                    }
                    if (countcheck == 0)
                    {
                        ShowReleasePopup();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_NeedToSelectAtleastOneRow").ToString()) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                        break;
                    }
                    result = 0;
                    ReleaseBO temReleaseBO = new ReleaseBO();
                    temReleaseBO = (ReleaseBO)SetUIValuesToObject(ActionsEnum.RELEASESAVE);
                    xmlDoc = CommonFunctions.ObjectTOXml(temReleaseBO);
                    result = OrderPlanningBL.SaveRelease(xmlDoc.InnerXml);
                    if (result > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ClosePopup();ShowErrorMessage('" + (GetLocalResourceObject("Msg_ReleasedSuccessfully").ToString()) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                    }
                    else
                    {
                        switch (Convert.ToInt32(result))
                        {
                            case (int)DbSaveStatus.SQLERROR:
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Sql_Error) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                                break;
                            default:
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_SavError) + "','" + Resources.ErrorMessages.Msg_Heading_Information + "');", true);
                                break;
                        }
                    }
                    break;
                #endregion

                #region BINDETAILS
                case ActionsEnum.DETAILS:
                    gvr = ((ImageButton)sender).Parent.Parent as GridViewRow;
                    HiddenField hdfDtlPK = (HiddenField)gvr.FindControl("hdfDtlPK") as HiddenField;
                    ScPk = hdfDtlPK.Value == string.Empty ? 0 : Convert.ToInt32(hdfDtlPK.Value);
                    GetFieldValues(ControlsEnum.BINDETAILS);
                    SetFieldValues(ControlsEnum.BINDETAILS);
                    lblScDisplay.Text = ((Label)gvr.FindControl("lblSCNo") as Label).Text;
                    ShowBinDetialsPopup();
                    break;
                #endregion

                #region POPUPADD
                case ActionsEnum.POPUPSHOW:
                    ShowReleasePopup();
                    break;
                #endregion

                #region Details
                case ActionsEnum.LISTBIN:

                    txtScPopup.Text = string.Empty;
                    txtProductGroupPopup.Text = string.Empty;
                    txtSizePopup.Text = string.Empty;

                    gvr = ((LinkButton)sender).Parent.Parent as GridViewRow;
                    ScPk = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfOrderDtlPk") as HiddenField).Value);
                    GridViewRow gridView = ((LinkButton)sender).Parent.Parent as GridViewRow;
                    txtScPopup.Text = txtScPopup.ToolTip = ((Label)gridView.FindControl("lblOrderNo") as Label).Text;
                    txtProductGroupPopup.Text = PendingOrderBO.Details[gvr.RowIndex].ITM_PLAN_GROUP_TEXT;
                    txtSizePopup.Text = txtSizePopup.ToolTip = ((Label)gridView.FindControl("lblProductSizeText") as Label).Text;

                    GetFieldValues(ControlsEnum.ALLOCATIONDETAILS);
                    SetFieldValues(ControlsEnum.ALLOCATIONDETAILS);
                    ShowBinHistory();

                    break;
                #endregion

                #region Print
                case ActionsEnum.PRINTPENDINGORDERS:
                    //Pending Orders based on Product Group
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ClosePopup", "ClosePopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + ApplicationType.OPLN + "&APPSUBTYPE=3" + "&ISEXCELPRINT=1") + "');", true);
                    break;
                #endregion

                #region SELECTALLPAGEITEMS
                case ActionsEnum.CHECK_CHANGE:
                    if (ChkAllPages.Checked)
                    {
                        hdfIsAllPagsSelected.Value = "1";
                        GetFieldValues(ControlsEnum.SELECTALLPAGEITEMS);
                        foreach (PendingOrderList items in objALLPendingOrderLst)
                        {
                            items.ITM_CHECKED = 1;
                        }
                    }
                    else
                    {
                        hdfIsAllPagsSelected.Value = "0";
                        objALLPendingOrders = null;
                        objALLPendingOrderLst = null;
                    }
                    break;
                    #endregion
            }
        }
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdBinDtls")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label txtQty = e.Row.FindControl("lblAllocated") as Label;
                        Gridtotal += Convert.ToDouble(txtQty.Text);
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalQty = (Label)e.Row.FindControl("lblTotalQty");
                        lblTotalQty.Text = GetFormattedNumber(Gridtotal).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region -- For Datalist --
        /// <summary>
        /// Datalsit OnItemDataBound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdfName = (HiddenField)e.Item.FindControl("hdfName");
                HiddenField hdfValue = (HiddenField)e.Item.FindControl("hdfValue");
                CheckBoxList chk = (CheckBoxList)e.Item.FindControl("chk");
                CheckBox chkHdrSelect = (CheckBox)e.Item.FindControl("chkHdrSelect");

                GroupValue = Convert.ToInt32(hdfValue.Value);
                TempItemDtlList = new List<ItemSpecDetails>();
                objItemGroups = new ItemSpecGroup();
                objItemGroups = PendingOrderBO.ItemSpecGroup.SingleOrDefault(x => x.CNG_PK == GroupValue);
                if (objItemGroups != null && objItemGroups.ItemSpecDetails != null)
                    TempItemDtlList = objItemGroups.ItemSpecDetails.ToList();
                chk.Items.Clear();
                if (TempItemDtlList != null && TempItemDtlList.Count > 0)
                {
                    chk.DataSource = TempItemDtlList;
                    chk.DataTextField = Common.CON_NAME;
                    chk.DataValueField = Common.CON_PK;
                    chk.DataBind();
                    foreach (ListItem item in chk.Items)
                        item.Selected = true;
                    chkHdrSelect.Checked = true;
                }
            }
        }
        #endregion
        #endregion

        #region Page Level Event
        /// <summary>
        /// For Page Load Event, and fiill details as default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageVariables();
            if (!IsPostBack)
            {
                OrderPlanPK = 0;
                #region Set Decimal Count
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                DecimalFormat = "#" + currencysep + "#";
                hdfDecimalCoundMst = this.Page.Master.FindControl("hdfWeightDecimalDigit") as HiddenField;//hdfBinDecimal
                int weightDecimalDigits = (Session[ERP.Utilities.SessionStrings.WeightDecimalDigit] == null
                      ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                      : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.WeightDecimalDigit]));

                for (int i = 0; i < weightDecimalDigits; i++)
                {
                    WeightFormat += "0";
                }
                #endregion
                SetButtonVisibility();
                GetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                SetFieldValues(ControlsEnum.PENDINGORDERSLIST);
            }
        }

        /// <summary>
        /// To handle page prerender event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ChkAllPages.Visible = grdPendingOrders.Rows.Count > 0 ? true : false;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_UcrInitComponents", "UcrInitComponents();", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlsEnum ControlType)
        {
            try
            {
                dtPage = null;
                string xmlString = string.Empty;
                switch (ControlType)
                {
                    case ControlsEnum.PENDINGORDERSLIST:
                    case ControlsEnum.CLEARLIST:
                        //if (IsAllocation != 1 && Session["SessionAllocation"] != null)
                        //{
                        POPageIndex = POPageIndex == null ? "1" : POPageIndex;
                        PageSize = Convert.ToInt32(GetLocalResourceObject("grdPendingOrdersPageSize"));

                        if (ControlType == ControlsEnum.CLEARLIST)
                            SessionParamsBO = new Params();//if clear then clear search condition

                        //for manage search values from allocation keep value in session
                        if (IsAllocation == 0 && SessionParamsBO != null && SessionParamsBO.Parameters != null)
                            ParamsBO = SessionParamsBO;

                        if (ParamsBO.Parameters == null)
                            // for get value and field from product properties
                            if (ControlType == ControlsEnum.PENDINGORDERSLIST)
                            {
                                Params prms = new Params();
                                if (prms.Parameters == null)
                                    prms.Parameters = new List<Parameters>();
                                foreach (DataListItem dli in dtlProperties.Items)
                                {
                                    if (dli.ItemType == ListItemType.Item || dli.ItemType == ListItemType.AlternatingItem)
                                    {
                                        CheckBoxList chk = dli.FindControl("chk") as CheckBoxList;
                                        for (int i = 0; i <= chk.Items.Count - 1; i++)
                                        {
                                            Parameters ObjParameters = new Parameters();
                                            if (chk.Items[i].Selected)
                                            {
                                                ObjParameters.ParamName = (dli.FindControl("hdfName") as HiddenField).Value;
                                                ObjParameters.Value = chk.Items[i].Value;
                                            }
                                            prms.Parameters.Add(ObjParameters);
                                        }
                                    }
                                }
                                //// for get value and field from product group
                                //for (int i = 0; i <= chkProductGroup.Items.Count - 1; i++)
                                //{
                                //    if (chkProductGroup.Items[i].Selected)
                                //    {
                                //        Parameters ObjParameters = new Parameters();
                                //        ObjParameters.ParamName = GetLocalResourceObject("ITEM_GROUP").ToString();
                                //        ObjParameters.Value = chkProductGroup.Items[i].Value;
                                //        prms.Parameters.Add(ObjParameters);
                                //    }
                                //}
                                // for get value and field from order groups
                                for (int i = 0; i <= chkOrders.Items.Count - 1; i++)
                                {
                                    if (chkOrders.Items[i].Selected)
                                    {
                                        Parameters ObjParameters = new Parameters();
                                        ObjParameters.ParamName = GetLocalResourceObject("SOH_PK").ToString();
                                        ObjParameters.Value = chkOrders.Items[i].Value;
                                        prms.Parameters.Add(ObjParameters);
                                    }
                                }
                                // for get value and field from sbu
                                for (int i = 0; i <= chkSBU.Items.Count - 1; i++)
                                {
                                    if (chkSBU.Items[i].Selected)
                                    {
                                        Parameters ObjParameters = new Parameters();
                                        ObjParameters.ParamName = GetLocalResourceObject("BIZUNIT").ToString();
                                        ObjParameters.Value = chkSBU.Items[i].Value;
                                        prms.Parameters.Add(ObjParameters);
                                    }
                                }
                                // for get value and field from Customers
                                for (int i = 0; i <= chkCustomer.Items.Count - 1; i++)
                                {
                                    if (chkCustomer.Items[i].Selected)
                                    {
                                        Parameters ObjParameters = new Parameters();
                                        ObjParameters.ParamName = GetLocalResourceObject("CUS_PK").ToString();
                                        ObjParameters.Value = chkCustomer.Items[i].Value;
                                        prms.Parameters.Add(ObjParameters);
                                    }
                                }
                                ParamsBO = prms;
                            }
                        if (ParamsBO != null && ParamsBO.Parameters != null)
                        {
                            xmlString = CommonFunctions.XmlSerialize<Params>(ParamsBO);

                            //xmlDoc = CommonFunctions.ObjectTOXml(ParamsBO);//juno
                            if (IsAllocation == 1)
                                SessionParamsBO = ParamsBO;
                        }
                        objPendingOrders = PendingOrderBO = OrderPlanningBL.GetPendingOrdersList(currentUser.CurrentSBUPK, xmlString, txtDispatchDate.Text, Convert.ToInt32(POPageIndex), PageSize, OrderPlanPK, Convert.ToInt32(chkWithStock.Checked), Convert.ToInt32(chkFullyAllocated.Checked), Convert.ToInt32(chkFullyPlanned.Checked));
                        objPendingOrderLst = objPendingOrders.Details;

                        //}
                        //else
                        //{
                        //    objPendingOrderLst = (List<PendingOrderList>)this.Session["SessionAllocation"];
                        //}
                        break;
                    case ControlsEnum.SELECTALLPAGEITEMS:
                        //for manage search values from allocation keep value in session
                        if (IsAllocation == 0 && SessionParamsBO != null && SessionParamsBO.Parameters != null)
                            ParamsBO = SessionParamsBO;

                        if (ParamsBO.Parameters == null)
                            // for get value and field from product properties
                            if (ControlType == ControlsEnum.PENDINGORDERSLIST)
                            {
                                Params prms = new Params();
                                if (prms.Parameters == null)
                                    prms.Parameters = new List<Parameters>();
                                foreach (DataListItem dli in dtlProperties.Items)
                                {
                                    if (dli.ItemType == ListItemType.Item || dli.ItemType == ListItemType.AlternatingItem)
                                    {
                                        CheckBoxList chk = dli.FindControl("chk") as CheckBoxList;
                                        for (int i = 0; i <= chk.Items.Count - 1; i++)
                                        {
                                            Parameters ObjParameters = new Parameters();
                                            if (chk.Items[i].Selected)
                                            {
                                                ObjParameters.ParamName = (dli.FindControl("hdfName") as HiddenField).Value;
                                                ObjParameters.Value = chk.Items[i].Value;
                                            }
                                            prms.Parameters.Add(ObjParameters);
                                        }
                                    }
                                }
                                //// for get value and field from product group
                                //for (int i = 0; i <= chkProductGroup.Items.Count - 1; i++)
                                //{
                                //    if (chkProductGroup.Items[i].Selected)
                                //    {
                                //        Parameters ObjParameters = new Parameters();
                                //        ObjParameters.ParamName = GetLocalResourceObject("ITEM_GROUP").ToString();
                                //        ObjParameters.Value = chkProductGroup.Items[i].Value;
                                //        prms.Parameters.Add(ObjParameters);
                                //    }
                                //}
                                // for get value and field from order groups
                                for (int i = 0; i <= chkOrders.Items.Count - 1; i++)
                                {
                                    if (chkOrders.Items[i].Selected)
                                    {
                                        Parameters ObjParameters = new Parameters();
                                        ObjParameters.ParamName = GetLocalResourceObject("SOH_PK").ToString();
                                        ObjParameters.Value = chkOrders.Items[i].Value;
                                        prms.Parameters.Add(ObjParameters);
                                    }
                                }
                                // for get value and field from sbu
                                for (int i = 0; i <= chkSBU.Items.Count - 1; i++)
                                {
                                    if (chkSBU.Items[i].Selected)
                                    {
                                        Parameters ObjParameters = new Parameters();
                                        ObjParameters.ParamName = GetLocalResourceObject("BIZUNIT").ToString();
                                        ObjParameters.Value = chkSBU.Items[i].Value;
                                        prms.Parameters.Add(ObjParameters);
                                    }
                                }
                                // for get value and field from Customers
                                for (int i = 0; i <= chkCustomer.Items.Count - 1; i++)
                                {
                                    if (chkCustomer.Items[i].Selected)
                                    {
                                        Parameters ObjParameters = new Parameters();
                                        ObjParameters.ParamName = GetLocalResourceObject("CUS_PK").ToString();
                                        ObjParameters.Value = chkCustomer.Items[i].Value;
                                        prms.Parameters.Add(ObjParameters);
                                    }
                                }
                                ParamsBO = prms;
                            }
                        if (ParamsBO != null && ParamsBO.Parameters != null)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXml(ParamsBO);
                            if (IsAllocation == 1)
                                SessionParamsBO = ParamsBO;
                        }
                        objPendingOrders = new PendingOrderBO();
                        objALLPendingOrderLst = new List<PendingOrderList>();
                        objALLPendingOrders = PendingOrderBO = OrderPlanningBL.GetPendingOrdersList(currentUser.CurrentSBUPK, (xmlDoc == null ? string.Empty : xmlDoc.InnerXml), txtDispatchDate.Text, 0, 0, OrderPlanPK, Convert.ToInt32(chkWithStock.Checked), Convert.ToInt32(chkFullyAllocated.Checked), Convert.ToInt32(chkFullyPlanned.Checked));
                        objALLPendingOrderLst = objALLPendingOrders.Details;

                        break;
                    case ControlsEnum.RELEASESC:
                        dtPage = OrderPlanningBL.GetReleaseScs(currentUser.CurrentSBUPK);
                        break;
                    case ControlsEnum.BINDETAILS:
                        dtPage = OrderPlanningBL.GetSCBinDetails(ScPk);
                        break;
                    case ControlsEnum.ALLOCATIONDETAILS:
                        dtPage = OrderPlanningBL.GetAllocationDetails(ScPk.ToString());
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlsEnum ControlType)
        {
            try
            {
                switch (ControlType)
                {
                    case ControlsEnum.PENDINGORDERSLIST:
                    case ControlsEnum.CLEARLIST:
                        BindGrid(ControlType);
                        break;
                    case ControlsEnum.PRODUCTPROPERTY:
                        BindDatalist(ControlsEnum.PRODUCTPROPERTY);
                        break;
                    case ControlsEnum.ORDERNUMBER:
                        BindCheckboxList(ControlsEnum.ORDERNUMBER);
                        break;
                    case ControlsEnum.PRODUCTPLANGROUP:
                        BindCheckboxList(ControlsEnum.PRODUCTPLANGROUP);
                        break;
                    case ControlsEnum.SBU:
                        BindCheckboxList(ControlsEnum.SBU);
                        break;
                    case ControlsEnum.CUSTOMER:
                        BindCheckboxList(ControlsEnum.CUSTOMER);
                        break;
                    case ControlsEnum.RELEASESC:
                        BindGrid(ControlType);
                        break;
                    case ControlsEnum.BINDETAILS:
                        BindGrid(ControlType);
                        break;
                    case ControlsEnum.ALLOCATIONDETAILS:
                        BindGrid(ControlsEnum.ALLOCATIONDETAILS);
                        break;

                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region HelperMethods
        #region BindGrid
        private void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PENDINGORDERSLIST:
                case ControlsEnum.CLEARLIST:
                    if (objPendingOrderLst != null && objPendingOrderLst.Count > 0)
                    {
                        if ((Convert.ToInt32(objPendingOrderLst[0].ROW_COUNT) >= 0))
                        {
                            uclPagingPO.Visible = true;
                            if ((Convert.ToInt32(objPendingOrderLst[0].ROW_COUNT) % PageSize) == 0)
                                uclPagingPO.TotalPages = Convert.ToInt32(objPendingOrderLst[0].ROW_COUNT) / PageSize;
                            else
                                uclPagingPO.TotalPages = (Convert.ToInt32(objPendingOrderLst[0].ROW_COUNT) / PageSize) + 1;
                        }
                        grdPendingOrders.DataSource = objPendingOrderLst;
                        btnPendingOrderPrint.Enabled = true;
                    }
                    else
                    {
                        grdPendingOrders.DataSource = null;
                        uclPagingPO.TotalPages = 0;
                        //btnPendingOrderPrint.Attributes.Add("onclick", "this.disabled=true;");
                        btnPendingOrderPrint.Enabled = false;
                    }
                    uclPagingPO.CurrentPage = Convert.ToInt32(POPageIndex);
                    grdPendingOrders.DataBind();
                    uclPagingPO.BindPager();
                    break;
                case ControlsEnum.RELEASESC:
                    grdReleaseSc.DataSource = dtPage;
                    grdReleaseSc.DataBind();
                    break;
                case ControlsEnum.BINDETAILS:
                    grdBins.DataSource = dtPage;
                    grdBins.DataBind();
                    break;
                case ControlsEnum.ALLOCATIONDETAILS:
                    grdBinDtls.DataSource = dtPage;
                    grdBinDtls.DataBind();
                    break;
            }
        }
        #endregion

        #region BindDatalist
        private void BindDatalist(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.PRODUCTPROPERTY:
                    if (PendingOrderBO != null && PendingOrderBO.ItemSpecGroup.Count > 0)
                        dtlProperties.DataSource = PendingOrderBO.ItemSpecGroup;
                    else
                        dtlProperties.DataSource = null;
                    dtlProperties.DataBind();
                    break;
            }
        }
        #endregion

        #region BindCheckboxList
        private void BindCheckboxList(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.ORDERNUMBER:
                    chkOrders.Items.Clear();
                    if (PendingOrderBO != null && PendingOrderBO.SaleOrder.Count > 0)
                    {
                        chkOrders.DataSource = PendingOrderBO.SaleOrder;
                        chkOrders.DataTextField = HttpUtility.HtmlDecode(Common.F_SOH_NO); 
                        chkOrders.DataValueField = Common.F_SOH_PK;
                        chkOrders.DataBind();
                    }
                    break;
                case ControlsEnum.PRODUCTPLANGROUP:
                    //chkProductGroup.Items.Clear();
                    //if (PendingOrderBO != null && PendingOrderBO.PlanItemGroup.Count > 0)
                    //{
                    //    chkProductGroup.DataSource = PendingOrderBO.PlanItemGroup;
                    //    chkProductGroup.DataTextField = Common.F_PIG_NAME;
                    //    chkProductGroup.DataValueField = Common.F_PIG_PK;
                    //    chkProductGroup.DataBind();
                    //}
                    break;
                case ControlsEnum.SBU:
                    chkSBU.Items.Clear();
                    if (PendingOrderBO != null && PendingOrderBO.BizUnit.Count > 0)
                    {
                        chkSBU.DataSource = PendingOrderBO.BizUnit;
                        chkSBU.DataTextField = Common.F_BZU_CODE;
                        chkSBU.DataValueField = Common.F_BZU_PK;
                        chkSBU.DataBind();
                    }
                    break;
                case ControlsEnum.CUSTOMER:
                    chkCustomer.Items.Clear();
                    if (PendingOrderBO != null && PendingOrderBO.Customer.Count > 0)
                    {
                        chkCustomer.DataSource = PendingOrderBO.Customer;
                        chkCustomer.DataTextField = Common.F_CUS_CODE;
                        chkCustomer.DataValueField = Common.F_CUS_PK;
                        chkCustomer.DataBind();
                    }
                    break;
            }
        }
        #endregion

        //To show PO pop up
        private void ShowAdvanceFilterPopUp()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=tbladvancedSearch]','" + GetLocalResourceObject("AdvanceFilter").ToString() + "','" + GetLocalResourceObject("POPopupWidth").ToString() + "','auto');", true);
        }
        private void ShowReleasePopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divReleaseSC]','" + GetLocalResourceObject("ReleaseDetials").ToString() + "','" + GetLocalResourceObject("ReleasePOPopupWidth").ToString() + "','auto');", true);
        }
        private void ShowBinDetialsPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divBinDetials]','" + GetLocalResourceObject("BinDetails").ToString() + "','" + GetLocalResourceObject("BinDetailsPOPopupWidth").ToString() + "','auto');", true);
        }

        private void ShowBinHistory()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divBinHistory]','" + GetLocalResourceObject("BinDetails").ToString() + "','" + "800" + "','auto');", true);
        }
        /// <summary>
        /// Manage Decimal Points 
        /// </summary>
        /// <param name="number">Number For Formating</param>
        /// <returns></returns>
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            if (num != 0)
                return num.ToString(DecimalFormat);
            else
                return num.ToString();

        }
        /// <summary>
        /// Manage Decimal Points with comma seperation
        /// </summary>
        /// <param name="number">Number For Formating</param>
        /// <returns></returns>
        public string GetFormattedWeightwithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(WeightFormat);
        }
        /// <summary>
        /// for set button visibility
        /// </summary>
        private void SetButtonVisibility()
        {
            btnSelectOrder.Visible = isVisibleOrder;
            //chkWithStock.Checked = isVisibleOrder == true ? false : true;
            btnSelectAllocation.Visible = isVisibleAllocation;
            btnClickDeAllocation.Visible = isVisibleAllocation;
            btnRelease.Visible = isVisibleRelease;
        }

        public void PopulatePendingListGrid()
        {
            ChkAllPages.Checked = false;
            GetFieldValues(ControlsEnum.PENDINGORDERSLIST);
            SetFieldValues(ControlsEnum.PENDINGORDERSLIST);
        }

        public void PopulatePendingListClear()
        {
            POPageIndex = "1";
            ParamsBO = null;
            txtDispatchDate.Text = string.Empty;
            chkWithStock.Checked = false;
            chkFullyAllocated.Checked = true;
            chkFullyPlanned.Checked = false;
            ChkAllPages.Checked = false;
            if (chkWithStock.Checked == true && chkFullyAllocated.Checked == true && chkFullyPlanned.Checked == true)
                chkSelectAll.Checked = true;
            else
                chkSelectAll.Checked = false;
            GetFieldValues(ControlsEnum.CLEARLIST);
            SetFieldValues(ControlsEnum.CLEARLIST);
        }
        #region Set Ui Values to Object
        private Object SetUIValuesToObject(ActionsEnum controlType)
        {
            try
            {
                Object retObject;
                retObject = null;
                switch (controlType)
                {
                    #region Save
                    case ActionsEnum.DEALLOCATION:
                        DeAllocationBO tempDeAllocationBO = new DeAllocationBO();
                        tempDeAllocationBO.BIZUNIT_PK = currentUser.CurrentSBUPK;
                        tempDeAllocationBO.DEPT_PK = currentUser.CurrentDeptPK;
                        tempDeAllocationBO.USER_PK = currentUser.PKUser;
                        tempDeAllocationBO.LAST_MOD_DT = string.Empty;

                        List<DeAllocationDetails> lstDetails = new List<DeAllocationDetails>();
                        foreach (GridViewRow gvr in grdPendingOrders.Rows)
                        {
                            CheckBox ChkOrder = (CheckBox)gvr.FindControl("ChkOrder") as CheckBox;
                            if (ChkOrder.Checked == true)
                            {
                                DeAllocationDetails objDetails = new DeAllocationDetails();
                                HiddenField hdfOrderDtlPk = (HiddenField)gvr.FindControl("hdfOrderDtlPk") as HiddenField;
                                objDetails.SOD_PK = hdfOrderDtlPk.Value;
                                lstDetails.Add(objDetails);
                            }
                        }
                        tempDeAllocationBO.Detail = lstDetails;
                        retObject = tempDeAllocationBO;
                        break;

                    case ActionsEnum.RELEASESAVE:
                        ReleaseBO tempReleaseBO = new ReleaseBO();
                        tempReleaseBO.BIZUNIT_PK = currentUser.CurrentSBUPK;
                        tempReleaseBO.DEPT_PK = currentUser.CurrentDeptPK;
                        tempReleaseBO.USER_PK = currentUser.PKUser;
                        tempReleaseBO.LAST_MOD_DT = string.Empty;

                        List<ReleaseDetails> lstRelDetails = new List<ReleaseDetails>();
                        foreach (GridViewRow gvr in grdReleaseSc.Rows)
                        {
                            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect") as CheckBox;
                            if (chkSelect.Checked == true)
                            {
                                ReleaseDetails objDetails = new ReleaseDetails();
                                HiddenField hdfDtlPK = (HiddenField)gvr.FindControl("hdfDtlPK") as HiddenField;
                                objDetails.SOD_PK = hdfDtlPK.Value;
                                lstRelDetails.Add(objDetails);
                            }
                        }
                        tempReleaseBO.Detail = lstRelDetails;
                        retObject = tempReleaseBO;
                        break;
                        #endregion
                }
                return retObject;
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #endregion

        #region Pager Methods+Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///    // Add Common to All pages
        protected void Page_Init(object sender, System.EventArgs e)
        {
            this.uclPagingPO.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPO.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPO.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPO.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPagingPO.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        // Add Common to All pages
        private void InitializeComponent()
        {

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
                        uclPagingPO.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPagingPO.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPagingPO.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPagingPO.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPagingPO.CurrentPage--;
                        break;
                }
                SetAllocationDetails();
                POPageIndex = uclPagingPO.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                SetFieldValues(ControlsEnum.PENDINGORDERSLIST);
                ImageButton imb = new ImageButton();
                imb.CommandName = "POPUPADD";
                ShowPendingOrder(imb, e);
                EnableDisableButtons(e.TotalPages);
                SetGridStatus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

        }
        // Add Common to All pages
        protected void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPagingPO.FirstButtonEnabled = (uclPagingPO.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPagingPO.PreviousButtonEnabled = (uclPagingPO.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPagingPO.NextButtonEnabled = (uclPagingPO.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPagingPO.LastButtonEnabled = (uclPagingPO.CurrentPage < iTotalPages) ? true : false;

        }
        #endregion

        #region GRID SELECTION WITH PAGING
        #region Grd Status maintains

        //For sett allocation details

        private void SetAllocationDetails()
        {
            if (hdfIsAllPagsSelected.Value == "1")
            {
                SelectedList = new List<PendingOrderList>();
                SelectedList.AddRange(objALLPendingOrderLst);
            }

            if (SelectedList != null)
                SelectedPOList = (List<PendingOrderList>)SelectedList;
            else
                SelectedPOList = new List<PendingOrderList>();

            CheckBox chbSelect;
            foreach (GridViewRow item in grdPendingOrders.Rows)
            {
                chbSelect = (CheckBox)item.FindControl("ChkOrder");
                HiddenField hdfSOPk = (HiddenField)item.FindControl("hdfSOPk");
                HiddenField hdfOrderDtlPk = (HiddenField)item.FindControl("hdfOrderDtlPk");
                HiddenField hdfItemPlanGroup = (HiddenField)item.FindControl("hdfItemPlanGroup");
                HiddenField hdfProductPk = (HiddenField)item.FindControl("hdfProductPk");
                Label lblOrderNo = (Label)item.FindControl("lblOrderNo");
                Label lblProduct = (Label)item.FindControl("lblProduct");
                HiddenField hdfProductSize = (HiddenField)item.FindControl("hdfProductSize");
                HiddenField hdfItemPlanGroupText = (HiddenField)item.FindControl("hdfItemPlanGroupText");

                HiddenField hdfAvailQty = (HiddenField)item.FindControl("hdfAvailQty");
                HiddenField hdfAllocatedQty = (HiddenField)item.FindControl("hdfAllocatedQty");
                HiddenField hdfBaltoAllocate = (HiddenField)item.FindControl("hdfBaltoAllocate");
                HiddenField hdfSizeSequence = (HiddenField)item.FindControl("hdfSizeSequence");
                HiddenField hdfAGradePerc = (HiddenField)item.FindControl("hdfAGradePerc");

                Label lblProductSizeText = (Label)item.FindControl("lblProductSizeText");
                Label lblQuantity = (Label)item.FindControl("lblQuantity");
                Label lblRequiredby = (Label)item.FindControl("lblRequiredby");
                Label lblDispatchedQty = (Label)item.FindControl("lblDispatchedQty");
                LinkButton lnkAllocatedQty = (LinkButton)item.FindControl("lnkAllocatedQty");
                Label lblPlannedQty = (Label)item.FindControl("lblPlannedQty");
                Label lblBalPlanQty = (Label)item.FindControl("lblBalPlanQty");

                PendingOrderList objTempPO = new PendingOrderList();
                objTempPO.ITM_CHECKED = 0;
                objTempPO.SOH_PK = string.IsNullOrEmpty(hdfSOPk.Value) ? 0 : Convert.ToInt32(hdfSOPk.Value);
                objTempPO.SOH_NO = lblOrderNo.Text;
                objTempPO.SOD_PK = string.IsNullOrEmpty(hdfOrderDtlPk.Value) ? 0 : Convert.ToInt32(hdfOrderDtlPk.Value);
                objTempPO.SOD_ITEM = string.IsNullOrEmpty(hdfProductPk.Value) ? 0 : Convert.ToInt32(hdfProductPk.Value);
                objTempPO.SOD_ITEM_CODE = lblProduct.Text;
                objTempPO.SOD_ITEM_TEXT = lblProduct.ToolTip;
                objTempPO.ISD_SIZE = string.IsNullOrEmpty(hdfProductSize.Value) ? 0 : Convert.ToInt32(hdfProductSize.Value);
                objTempPO.ISD_SIZE_TEXT = lblProductSizeText.Text;
                objTempPO.SOD_QTY = string.IsNullOrEmpty(lblQuantity.Text) ? 0 : Convert.ToDouble(lblQuantity.Text);
                objTempPO.SOD_REQUIRED_BY = lblRequiredby.Text;
                objTempPO.SOD_QTY_DISPATCHED = string.IsNullOrEmpty(lblDispatchedQty.Text) ? 0 : Convert.ToDouble(lblDispatchedQty.Text);
                objTempPO.SOD_QTY_ALLOCATED = string.IsNullOrEmpty(lnkAllocatedQty.Text) ? 0 : Convert.ToDouble(lnkAllocatedQty.Text);
                objTempPO.SOD_QTY_PLANNED = string.IsNullOrEmpty(lblPlannedQty.Text) ? 0 : Convert.ToDouble(lblPlannedQty.Text);
                objTempPO.SOD_BAL_TO_PLAN = string.IsNullOrEmpty(lblBalPlanQty.Text) ? 0 : Convert.ToDouble(lblBalPlanQty.Text);
                objTempPO.ITM_PLAN_GROUP = string.IsNullOrEmpty(hdfItemPlanGroup.Value) ? 0 : Convert.ToInt32(hdfItemPlanGroup.Value);
                objTempPO.ITM_PLAN_GROUP_TEXT = hdfItemPlanGroupText.Value;
                objTempPO.GRP_ALLOCATED_QTY = string.IsNullOrEmpty(hdfAllocatedQty.Value) ? 0 : Convert.ToInt32(hdfAllocatedQty.Value);
                objTempPO.GRP_AVAILABLE_QTY = string.IsNullOrEmpty(hdfAvailQty.Value) ? 0 : Convert.ToInt32(hdfAvailQty.Value);
                objTempPO.SOD_BAL_TO_ALLOCATE = string.IsNullOrEmpty(hdfBaltoAllocate.Value) ? 0 : Convert.ToInt32(hdfBaltoAllocate.Value);
                objTempPO.ISD_SIZE_SEQUENCE = string.IsNullOrEmpty(hdfSizeSequence.Value) ? 0 : Convert.ToInt32(hdfSizeSequence.Value);
                objTempPO.ISD_AGRADE_PER = string.IsNullOrEmpty(hdfAGradePerc.Value) ? 0 : Convert.ToDouble(hdfAGradePerc.Value);

                if (chbSelect.Checked)
                    objTempPO.ITM_CHECKED = 1;
                bool alreadyExists = SelectedPOList.Exists(itemLst => itemLst.SOD_PK == objTempPO.SOD_PK);
                if (alreadyExists)
                    ChangeItem(objTempPO);
                else
                    SelectedPOList.Add(objTempPO);
            }
            SelectedList = SelectedPOList;
        }
        #endregion

        //for change the status of checked items
        private void ChangeItem(PendingOrderList item)
        {
            if (SelectedPOList.Count > 0)
                foreach (var Items in SelectedPOList)
                    if (Items.SOD_PK == item.SOD_PK)
                    {
                        Items.ITM_CHECKED = item.ITM_CHECKED;
                    }
        }

        //For Reset grid status
        private void SetGridStatus()
        {
            if (SelectedPOList != null)
            {
                SelectedPOList = (List<PendingOrderList>)SelectedList;
                int SodPk;
                ((CheckBox)grdPendingOrders.HeaderRow.FindControl("ChkAll")).Checked = true;
                foreach (GridViewRow item in grdPendingOrders.Rows)
                {
                    SodPk = Convert.ToInt32(((HiddenField)item.FindControl("hdfOrderDtlPk")).Value);
                    PendingOrderList objTemp = new PendingOrderList();
                    objTemp = (PendingOrderList)SelectedPOList.SingleOrDefault(itemLst => itemLst.SOD_PK == SodPk);
                    if (objTemp != null)
                    {
                        if (objTemp.ITM_CHECKED == 1)
                            ((CheckBox)item.FindControl("ChkOrder")).Checked = true;
                        else
                        {
                            ((CheckBox)item.FindControl("ChkOrder")).Checked = false;
                            ((CheckBox)grdPendingOrders.HeaderRow.FindControl("ChkAll")).Checked = false;
                        }
                    }
                    else
                        ((CheckBox)grdPendingOrders.HeaderRow.FindControl("ChkAll")).Checked = false;
                }

            }
        }
        /// <summary>
        /// To check selected sc count in Order Planning
        /// </summary>
        /// <returns></returns>
        public int SelectedItemsCount()
        {
            int count = 0;
            if (hdfIsAllPagsSelected.Value == "0")
            {
                foreach (GridViewRow gvr in grdPendingOrders.Rows)
                {
                    CheckBox chbkSelect = (CheckBox)gvr.FindControl("ChkOrder");
                    if (chbkSelect.Checked)
                        count++;

                }
            }
            else
                count = 1;
            return count;
        }

        //While Save
        /// <summary>
        /// Method for get selected orders
        /// </summary>
        public List<PendingOrderList> GetSelectedOrders()
        {
            List<PendingOrderList> ItemsList = new List<PendingOrderList>();
            SetAllocationDetails();
            foreach (var Items in SelectedPOList)
                if (Items.ITM_CHECKED == 1)
                {
                    PendingOrderList objitems = new PendingOrderList();
                    objitems.ITM_CHECKED = Items.ITM_CHECKED;
                    objitems.SOH_PK = Items.SOH_PK;
                    objitems.SOD_PK = Items.SOD_PK;
                    objitems.SOH_NO = Items.SOH_NO;
                    objitems.SOD_ITEM = Items.SOD_ITEM;
                    objitems.SOD_ITEM_CODE = Items.SOD_ITEM_CODE;
                    objitems.SOD_ITEM_TEXT = Items.SOD_ITEM_TEXT;
                    objitems.ISD_SIZE = Items.ISD_SIZE;
                    objitems.ISD_SIZE_TEXT = Items.ISD_SIZE_TEXT;
                    objitems.SOD_QTY = Items.SOD_QTY;
                    objitems.SOD_QTY_DISPATCHED = Items.SOD_QTY_DISPATCHED;
                    objitems.SOD_QTY_ALLOCATED = Items.SOD_QTY_ALLOCATED;
                    objitems.SOD_QTY_PLANNED = Items.SOD_QTY_PLANNED;
                    objitems.SOD_BAL_TO_PLAN = Items.SOD_BAL_TO_PLAN;
                    objitems.SOD_REQUIRED_BY = Items.SOD_REQUIRED_BY;
                    objitems.ITM_PLAN_GROUP = Items.ITM_PLAN_GROUP;
                    objitems.ITM_PLAN_GROUP_TEXT = Items.ITM_PLAN_GROUP_TEXT;
                    objitems.GRP_ALLOCATED_QTY = Items.GRP_ALLOCATED_QTY;
                    objitems.GRP_AVAILABLE_QTY = Items.GRP_AVAILABLE_QTY;
                    objitems.SOD_BAL_TO_ALLOCATE = Items.SOD_BAL_TO_ALLOCATE;
                    objitems.ISD_SIZE_SEQUENCE = Items.ISD_SIZE_SEQUENCE;
                    objitems.ISD_AGRADE_PER = Items.ISD_AGRADE_PER;
                    ItemsList.Add(objitems);
                }
            return ItemsList;
        }
        #endregion
     
    }

    public enum ControlsEnum
    {
        PENDINGORDERSLIST,
        PRODUCTPROPERTY,
        PROPERTY,
        ORDERNUMBER,
        PLANHEADER,
        PRODUCTPLANGROUP,
        EDIT,
        PRODUCTPLANGROUPLIST,
        SBU,
        PLANLISTING,
        CLEARLIST,
        PLANDETAILS,
        ORDERDETAILS,
        LINELIST,
        LINEDETAILS,
        LINEGRID,
        SIZE,
        CUSTOMER,
        ALLOCATIONDETAILS,
        SUMMARYDETAILS,
        SUMMARYPLANTLIST,
        SUMMARYLINELIST,
        SUMMARYGROUPLIST,
        PLANVERSIONS,
        RELEASESC,
        BINDETAILS,
        LINEWISESUMMARY,
        SCDETAILS,
        SCENARIODETAILS,
        MACHINEFILL,
        LINEFORMERDETAILS,
        GROUPLINES,
        PLANGROUPLINES,
        PLANCALCULATION,
        SELECTALLPAGEITEMS
    }
    class Common
    {
        public const string CON_NAME = "CON_NAME";
        public const string CON_CODE = "CON_CODE";
        public const string CON_PK = "CON_PK";
        public const string F_CUS_PK = "CUS_PK";
        public const string F_CUS_CODE = "CUS_CODE";
        public const string F_BZU_CODE = "BZU_CODE";
        public const string F_BZU_PK = "BZU_PK";
        public const string F_PIG_PK = "PIG_PK";
        public const string F_PIG_NAME = "PIG_NAME";
        public const string F_SOH_NO = "SOH_NO";
        public const string F_SOD_PK = "SOD_PK";
        public const string F_SOH_PK = "SOH_PK";

    }
}