using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Sales;
using System.Xml;
using ERPSMS_v01.UserControls;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERP.Utilities;
using ERP.Utilities.Constants.DA;
using BusinessObject.Common;
using ERPData;
using ERPService;
using System.Data;
using BusinessObject.Shipping;
using ERPManager;
using CustomControls;
using BusinessObject.DashBoard;
using System.Threading;

namespace ERPSMS_v01.DashboardSMS
{
    public partial class PurchaseDashboard : ERP.Store.UI.MyBasePage
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

        private PurchaseDashboardHdr DashboarHdr
        {
            get
            {
                return this.ViewState[ViewstateStrings.DashboarHdr] == null ? new PurchaseDashboardHdr() : (PurchaseDashboardHdr)this.ViewState[ViewstateStrings.DashboarHdr];
            }
            set
            {
                this.ViewState[ViewstateStrings.DashboarHdr] = value;
            }
        }
        private DataTable dtPayments
        {
            get
            {
                return this.ViewState[ViewstateStrings.Payments] == null ? new DataTable() : (DataTable)this.ViewState[ViewstateStrings.Payments];
            }
            set
            {
                this.ViewState[ViewstateStrings.Payments] = value;
            }
        }

        //For AuoComplete
        public static int CurrentSBUPK;

        // For Common Actions
        ActionsEnum commonAction;
        public event TreeOnDemand TreePopulationOnDemand;
        public delegate void TreeOnDemand(object sender, TreeNodeEventArgs e);

        public event NodeClicked NodeClickAction;
        public delegate void NodeClicked(object sender, EventArgs e);

        private DataSet dsPageData;
        private DataTable dtPageData;
        private DataTable dtInvoiceList;

        private int VendorPk = 0;
        private int POPk = 0;
        private int TrnType;
        private int TrnPk;
        private int InvPk;
        private POHeader objPOHeader;
        User CurrentUser;
        #endregion
        #endregion

        #region Page Level Events
        protected void Page_Load(object sender, EventArgs e)
        {
            EntryStatus = EntryStatus.LISTMODE;
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                PageActionHandler();
                CurrentSBUPK = CurrentUser.SBUID;
            }

        }



        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(ControlsEnum type)
        {
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (type)
            {
                #region VENDOR POS
                case ControlsEnum.VENDORPOS:
                    VendorPk = 0;
                    int.TryParse(hdfVendor.Value, out VendorPk);
                    DashboarHdr = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGenerateBL.GetVendorPOs(VendorPk, txtPONo.Text.Trim());
                    break;
                #endregion
                #region PO ITEMS
                case ControlsEnum.POITEMS:
                    objPOHeader = new POHeader();
                    objPOHeader = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGenerateBL.GetPODetails(POPk);
                    break;
                #endregion
                #region GRN/GIN/SA
                case ControlsEnum.GRN_GIN_SA:
                    dtPageData = new DataTable();
                    dtPageData = BusinessLogic.PurchaseOrderManagement.PurchaseOrderGeneration.GetPOListItemDetails(TrnType, TrnPk);
                    break;
                #endregion
                #region INVOICE LIST
                case ControlsEnum.INVOICELIST:
                    VendorPk = 0;
                    int.TryParse(hdfVendor.Value, out VendorPk);
                    InvPk = 0;
                    int InvStatus = 3;
                    int cmpPk = 0;
                    string customer = string.Empty;
                    dsPageData = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceList(
                        new BusinessObject.GridPrams()
                        {
                            SortBy = "IVH_DATE",
                            SortDirection = Resources.Report.SortDescending,
                            ThenBy = "IVH_NO",
                            ThenDirection = string.Empty,
                            FromDate = string.Empty,
                            ToDate = string.Empty,
                            SearchBy = "IVH_NO",
                            SearchValue = string.Empty
                        }, currentUser, VendorPk, InvPk, POPk, customer, string.Empty
                        , Resources.PageURL.PurchaseOrderInvoicing.Replace("~", "")
                        , 0
                        , InvStatus
                        , 0, (byte)POInvoiceCategory.Invoice
                        , (byte)0
                        , null
                        , string.Empty
                        , cmpPk);
                    if (dsPageData != null)
                    {
                        DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                        dtInvoiceList = dvInvoice.ToTable();
                    }
                    break;
                #endregion
                #region PAYMENT LIST
                case ControlsEnum.PAYMENTLIST:
                    dtPayments = new DataTable();
                    dtPayments = BusinessLogic.POInvoicing.POInvoiceBL.GetInvPayments(InvPk);
                    break;
                #endregion
            }

        }
        #endregion

        #region Set Field Values

        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        public void SetFieldValues(ControlsEnum type)
        {
            switch (type)
            {
                #region VENDOR POS
                case ControlsEnum.VENDORPOS:
                    BindTreeView();
                    ClearGrid();
                    break;
                #endregion
                #region PO ITEMS
                case ControlsEnum.POITEMS:
                    BindGrid(ControlsEnum.POITEMS);
                    break;
                #endregion
                #region GRN LIST
                case ControlsEnum.GRNLIST:
                    BindGrid(ControlsEnum.GRNLIST);
                    break;
                #endregion
                #region INVOICE LIST
                case ControlsEnum.INVOICELIST:
                    BindGrid(ControlsEnum.INVOICELIST);
                    break;
                #endregion
            }

        }
        #endregion


        #region Action handler
        public void ActionHandler(object sender, EventArgs e)
        {
            GridViewRow gvr;
            GridView grd;
            string arg;

            if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
            {
                commonAction = ActionsEnum.GRNDETAILS;
            }
            else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
            {
                commonAction = ActionsEnum.SELECTINDEXCHANGED;
            }
            switch (commonAction)
            {
                #region GRN DETAILS
                case ActionsEnum.GRNDETAILS:
                    gvr = ((RadioButton)sender).NamingContainer as GridViewRow;
                    HiddenField hdfPODtlPk = (HiddenField)gvr.FindControl("hdfPODtlPk");
                    TrnType = (int)TrnTypes.GRN;
                    TrnPk = Convert.ToInt32(hdfPODtlPk.Value);
                    GetFieldValues(ControlsEnum.GRN_GIN_SA);
                    SetFieldValues(ControlsEnum.GRNLIST);
                    break;
                #endregion
                #region GIN DETAILS
                case ActionsEnum.GINDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdGINList") as GridView;
                        if (!string.IsNullOrEmpty(arg))
                        {
                            TrnType = (int)TrnTypes.GIN;
                            TrnPk = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.GRN_GIN_SA);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                                grd.DataSource = dtPageData;
                            else
                                grd.DataSource = null;
                            grd.DataBind();

                        }
                        grd.Visible = true;
                        (gvr.FindControl("hdfIsExpandedGrn") as HiddenField).Value = "1";
                    }
                    break;
                #endregion
                #region STK ADMISSION DETAILS
                case ActionsEnum.STKADMISSIONDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdStockTransfer") as GridView;
                        if (!string.IsNullOrEmpty(arg))
                        {
                            TrnType = (int)TrnTypes.STKADMISSION;
                            TrnPk = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.GRN_GIN_SA);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                                grd.DataSource = dtPageData;
                            else
                                grd.DataSource = null;
                            grd.DataBind();
                        }
                        grd.Visible = true;
                        (gvr.FindControl("hdfIsExpandedGinList") as HiddenField).Value = "1";
                    }
                    break;
                #endregion
                #region PAYMENT DETAILS
                case ActionsEnum.PAYMENTDETAILS:
                    arg = ((Button)sender).CommandArgument;
                    gvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                    if (gvr != null)
                    {
                        grd = gvr.FindControl("grdPayments") as GridView;
                        if (!string.IsNullOrEmpty(arg))
                        {
                            InvPk = Convert.ToInt32(arg);
                            GetFieldValues(ControlsEnum.PAYMENTLIST);
                            if (dtPayments != null && dtPayments.Rows.Count > 0)
                                grd.DataSource = dtPayments;
                            else
                                grd.DataSource = null;
                            grd.DataBind();
                        }
                        grd.Visible = true;
                        (gvr.FindControl("hdfIsExpandedInvList") as HiddenField).Value = "1";
                    }
                    break;
                #endregion
                #region SEARCH
                case ActionsEnum.SEARCH:
                    GetFieldValues(ControlsEnum.VENDORPOS);
                    SetFieldValues(ControlsEnum.VENDORPOS);
                    break;
                #endregion
                #region CLEAR
                case ActionsEnum.CLEAR:
                    txtPONo.Text = string.Empty;
                    hdfVendor.Value = string.Empty;
                    txtVendor.Text = string.Empty;
                    GetFieldValues(ControlsEnum.VENDORPOS);
                    SetFieldValues(ControlsEnum.VENDORPOS);
                    break;
                #endregion
            }

        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (((GridView)sender).ID == "grdPayments")
                    {
                        Label lblHdrAmountBaseCur = e.Row.FindControl("lblHdrAmountBaseCur") as Label;
                        lblHdrAmountBaseCur.Text = GetLocalResourceObject("Amount").ToString() + " (" + dtPayments.Rows[0]["PVH_BASE_CURR_TEXT"] + ")";
                    }
                }

                if (((GridView)sender).ID == "grdCustomersOrders")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        HiddenField hdfTrxStatus = e.Row.FindControl("hdfTrxStatus") as HiddenField;
                        if (Convert.ToInt32(hdfTrxStatus.Value) == (int)ShippingTabsEnum.BillofLoading)
                        {
                            imgApproved.CssClass = GetLocalResourceObject("Completed").ToString();
                            imgApproved.ToolTip = Resources.Captions.Completed;

                        }
                        else
                        {
                            imgApproved.CssClass = GetLocalResourceObject("pending").ToString();
                            imgApproved.ToolTip = Resources.Captions.Pending;
                        }
                    }

                }
                if (((GridView)sender).ID == "grdOrderDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        (e.Row.FindControl("hdfHasChildren") as HiddenField).Value = ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrFormatWithSep.Value = "#" + currencysep + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrFormatWithSep.Value += "0";
                    }
                    GetFieldValues(ControlsEnum.VENDORPOS);
                    SetFieldValues(ControlsEnum.VENDORPOS);
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

            //uclPagingCustomersOrders.CurrentPage = 1;


        }
        /// <summary>
        /// Prerender event of the Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideCustomersOrdersd", "ShowHideCustomersOrders(1);", true);
            if (grdInvoice.Rows.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideInvoiced", "ShowHideInvoice(0);", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowHideInvoiced", "ShowHideInvoice(1);", true);
            }           
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
            this.Init += new EventHandler(this.Page_Init);
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


        #region Helper methods


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
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (controlType)
                {
                    default:
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

        /// <summary>
        /// Method for TreeView binding
        /// </summary>
        /// <summary>
        /// Method to bind the Role Tree
        /// </summary>
        private void BindTreeView()
        {
            try
            {
                TreeNode parentNode = null;
                trvCustomers.Nodes.Clear();
                foreach (VendorDetails vendor in DashboarHdr.VendorList)
                {
                    parentNode = new TreeNode(vendor.VEN_CODE, vendor.VEN_PK.ToString());
                    parentNode.ToolTip = vendor.VEN_NAME;
                    parentNode.NavigateUrl = "javascript:void(0)";
                    foreach (PODetails order in vendor.POList)
                    {
                        TreeNode childNode = new TreeNode(order.POH_NO, order.POH_PK.ToString() + "," + ApplicationType.PO);
                        childNode.ToolTip = order.POH_NO;
                        foreach (PRDetails prdet in order.PRList)
                        {
                            TreeNode childNode_Pr = new TreeNode(prdet.PRH_NO, prdet.PRH_PK.ToString() + "," + ApplicationType.PR);
                            childNode_Pr.ToolTip = prdet.PRH_NO;
                            childNode.ChildNodes.Add(childNode_Pr);
                        }
                        parentNode.ChildNodes.Add(childNode);
                    }
                    parentNode.Collapse();
                    // Show all checkboxes
                    //trvUtilityTree.ShowCheckBoxes = TreeNodeTypes.All;
                    trvCustomers.Nodes.Add(parentNode);
                }
                VendorPk = 0;
                int.TryParse(hdfVendor.Value, out VendorPk);
                if (VendorPk > 0 || !string.IsNullOrEmpty(txtPONo.Text.Trim()))
                    trvCustomers.ExpandAll();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }


        #region Tree Events

        /// <summary>
        /// Delegate call for populate on demand true - style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PopulateParentsChild(object sender, TreeNodeEventArgs e)
        {
            if (TreePopulationOnDemand != null)
            {
                TreePopulationOnDemand(sender, e);
            }
        }

        /// <summary>
        /// Action Handler to Fired on Tree Node Select
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler_onSelect(object sender, EventArgs e)
        {
            if (NodeClickAction != null)
            {
                NodeClickAction(sender, e);

            }
            ClearGrid();
            string[] strPkApptye = trvCustomers.SelectedNode.Value.Split(',');
            if (strPkApptye.Length > 1)
            {
                if (strPkApptye[1] == ApplicationType.PR)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + strPkApptye[0] + "&APPTYPE=" + ApplicationType.PR + "&APPSUBTYPE=") + "');", true);
                }
                else
                {
                    POPk = Convert.ToInt32(strPkApptye[0]);
                    GetFieldValues(ControlsEnum.POITEMS);
                    SetFieldValues(ControlsEnum.POITEMS);
                    GetFieldValues(ControlsEnum.INVOICELIST);
                    SetFieldValues(ControlsEnum.INVOICELIST);
                }
            }

        }

        #endregion


        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                default:
                    break;
            }
        }

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum type)
        {
            switch (type)
            {
                #region PO ITEMS
                case ControlsEnum.POITEMS:
                    if (objPOHeader != null && objPOHeader.POItems != null && objPOHeader.POItems.POItemDetails != null)
                        grdPOItems.DataSource = objPOHeader.POItems.POItemDetails.ToList();
                    else
                        grdPOItems.DataSource = null;
                    grdPOItems.DataBind();
                    break; 
                #endregion
                #region GRN LIST
                case ControlsEnum.GRNLIST:
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                        grdGRNList.DataSource = dtPageData;
                    else
                        grdGRNList.DataSource = null;
                    grdGRNList.DataBind();
                    break; 
                #endregion
                #region INVOICE LIST
                case ControlsEnum.INVOICELIST:
                    if (dtInvoiceList != null && dtInvoiceList.Rows.Count > 0)
                        grdInvoice.DataSource = dtInvoiceList;
                    else
                        grdInvoice.DataSource = null;
                    grdInvoice.DataBind();
                    break; 
                #endregion
            }
        }


        private void ClearGrid()
        {
            grdPOItems.DataSource = null;
            grdPOItems.DataBind();

            grdGRNList.DataSource = null;
            grdGRNList.DataBind();

            grdInvoice.DataSource = null;
            grdInvoice.DataBind();

        }
        public string GetFormattedCurrencyWithSeperation(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrFormatWithSep.Value);
        }
        #endregion        

        #region Control Enum
        private enum ActionsEnum
        {
            GRID,
            CANCEL,
            SEARCH,
            SHOWDETAILS,
            VIEW,
            SELECTINDEXCHANGED,
            PRINT,
            CLEAR,
            GINDETAILS,
            STDETAILS,
            STKADMISSIONDETAILS,
            GRNDETAILS,
            PAYMENTDETAILS

        }
        /// <summary>
        /// To control Page Actions
        /// </summary>
        public enum ControlsEnum
        {
            VENDORPOS,
            POITEMS,
            GRN_GIN_SA,
            GRNLIST,
            GINLIST,
            STKADMISSIONLIST,
            INVOICELIST,
            PAYMENTLIST
        }

        public enum TrnTypes
        {
            PO = 1,
            GRN = 2,
            GIN = 3,
            STKADMISSION = 4
        }
        #endregion
    }
}