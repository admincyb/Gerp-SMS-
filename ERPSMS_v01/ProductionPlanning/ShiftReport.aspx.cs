using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.ILibrary;
using BusinessObject.Sales;
using System.Xml;
using ERPSMS_v01.UserControls;
using DataAccess.ProductionDL;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Configuration;
using ERP.Utilities;
using ERP.Utilities.Constants.DA;
using BusinessObject.Common;
using ERPData;
using ERPService;

namespace ERPSMS_v01.ProductionPlanning
{
    public partial class ShiftReport : ERP.Store.UI.MyBasePage
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
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return this.ViewState["PageIndex"] == null ? 1 : (int)this.ViewState["PageIndex"];
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }
        int shhPK
        {
            get
            {
                return this.ViewState["shhPK"] == null ? 0 : (int)this.ViewState["shhPK"];
            }
            set
            {
                this.ViewState["shhPK"] = value;
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return this.ViewState["TotalPages"] == null ? 0 : (int)this.ViewState["TotalPages"];
            }
            set
            {
                this.ViewState["TotalPages"] = value;
            }
        }
        /// <summary>
        /// Current Session
        /// </summary>
        private int CurrentSession
        {
            get
            {
                //AccountDL.SetCurrentSession(CurrentUser);
                return Session[ERP.Utilities.SessionStrings.SessionPK] == null ? 0 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SessionPK]);
            }
            set
            {
                if (value == 0)
                    Session[ERP.Utilities.SessionStrings.SessionPK] = null;
                else
                    Session[ERP.Utilities.SessionStrings.SessionPK] = value;
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

        #endregion

        // For Common Actions
        ActionsEnum commonAction;
        private int ID = 0;

        //For get db return messages
        private DataSet dsDBMessgaes;

        //For Filter XML
        private string[] P_XML;

        //For seriol Number
        int SlNo;

        //For Shift details
        DataSet dsShiftDetails;

        //For Shift details
        DataSet dsShiftList;
        //Shift Header PK

        User CurrentUser;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private string ShiftNo=string.Empty;
        #endregion

        #region Page Level Events
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                GetFieldValues(ControlEnum.LISTING);
                SetFieldValues(ControlEnum.LISTING);
                divEntryForm.Visible = false;
                divListing.Visible = true;
                EntryStatus = EntryStatus.LISTMODE;

                AST_DOC_MODE.Value = GetDOCMODE();
               // AST_CODE.Value = ApplicationType.SFTRPT;
                lblShiftNo.Text = Resources.Messages.DocGenerationNew;
                hdfShiftNo.Value = string.Empty;

                hdfAppType.Value = ApplicationType.SFTRPT;
                hdfAppSubType.Value = string.Empty;

            }
            PageActionHandler();
        }


        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function(){InitComponents();});", true);

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
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");

        }



        #endregion

        #region Get Field Values

        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        public void GetFieldValues(ControlEnum type)
        {
            CommonService CommonServiceClient = null;
            try
            {
                CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                CommonServiceClient = new CommonService();
                switch (type)
                {
                    case ControlEnum.EDIT_SHIFT_DETAILS:
                        SetFilterXML(ControlsEnum.EDIT_SHIFT_DETAILS);
                        dsShiftDetails = FetchDbValues(3, P_XML);
                        break;
                    case ControlEnum.HEADER:
                        SetFilterXML(ControlsEnum.HEADER);
                        dsShiftDetails = FetchDbValues(4, P_XML);
                        break;
                    case ControlEnum.HEADER_DDL:
                        SetFilterXML(ControlsEnum.HEADER_DDL);
                        dsShiftDetails = FetchDbValues(2, P_XML);
                        break;
                    case ControlEnum.LISTING:
                        SetFilterXML(ControlsEnum.LISTING);
                        dsShiftList = FetchDbValues(1, P_XML);
                        break;
                    case ControlEnum.PRODUCT:
                        SetFilterXML(ControlsEnum.PRODUCT);
                        dsShiftList = FetchDbValues(5, P_XML);
                        break;
                    case ControlEnum.SHIFT:
                        ShiftNo = CommonServiceClient.GetTrxDocNo(ApplicationType.SFTRPT, 0, 1,
                            DateTime.Now, CurrentUser.PKUser, true, 0);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CommonServiceClient = null;
               
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
                case ControlEnum.HEADER:
                    txtShiftDate.Text = System.DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    if (dsShiftDetails != null && dsShiftDetails.Tables.Count > 0 && dsShiftDetails.Tables[0].Rows.Count > 0)
                    {
                        //lblShiftNo.Text = dsShiftDetails.Tables[3].Rows[0][0].ToString();
                        //hdfShiftNo.Value = dsShiftDetails.Tables[3].Rows[0][0].ToString();

                        lblShiftNo.Text = Resources.Messages.DocGenerationNew;
                        hdfShiftNo.Value = string.Empty;

                        //txtShiftNo.Enabled = false;
                    }
                    break;

                case ControlEnum.HEADER_DDL:

                    BindDropDown(ControlsEnum.SHIFT);
                    BindDropDown(ControlsEnum.LINE);
                    BindDropDown(ControlsEnum.PRODUCT);
                    break;
                case ControlEnum.LISTING:
                    BindGrid(ControlEnum.LISTING);
                    break;
                case ControlEnum.EDIT_SHIFT_DETAILS:
                    BindDropDown(ControlsEnum.SHIFT);
                    BindDropDown(ControlsEnum.LINE);
                    //BindDropDown(ControlsEnum.PRODUCT);
                    SetShiftData();

                    break;
                case ControlEnum.PRODUCT:
                    BindDropDown(ControlsEnum.PRODUCT);
                    break;

            }

        }
        #endregion

        #region Action handler
        /// <summary>
        /// For mange all actions on page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {

            ModifiedDatePnl.Visible = false;

            GridViewRow gvrItem;
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
                    else
                        if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                        {
                            commonAction = ActionsEnum.CHANGE;
                        }
            if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
            {
                commonAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), Resources.Controls.RadioSelect));
            }

            switch (commonAction)
            {
                case ActionsEnum.ADD:
                    Page.Validate("ShiftLine");
                    if (Page.IsValid)
                        AddItemDetails();
                    break;
                case ActionsEnum.DELETE_ACTION:
                    gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;
                    SlNo = Convert.ToInt32(((Label)grdShiftLines.Rows[gvrItem.RowIndex].FindControl("lblSlNo")).Text);
                    if (SlNo != 0)
                    {

                        DeleteItemDetails(SlNo);
                        EntryStatus = EntryStatus.ENTRYMODE;
                    }
                    break;
                case ActionsEnum.EDIT_ACTION:

                    gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;

                    SlNo = Convert.ToInt32(((Label)grdShiftLines.Rows[gvrItem.RowIndex].FindControl("lblSlNo")).Text);
                    if (SlNo != 0)
                    {
                        // Delete Topuped Item details from grid
                        FillItemDetails(SlNo);
                        EntryStatus = EntryStatus.ENTRYMODE;
                    }
                    break;
                case ActionsEnum.SAVE:
                    Page.Validate("Shift");
                    if (Page.IsValid)
                    {
                        SetFieldValuesToXML();
                        dsDBMessgaes = SetValuesToDB(1, P_XML);

                        if (PrintDbMessages(dsDBMessgaes))
                        {
                            
                            ResetForm();
                            ClearLine();
                            GetFieldValues(ControlEnum.LISTING);
                            SetFieldValues(ControlEnum.LISTING);
                            divEntryForm.Visible = false;
                            divListing.Visible = true;
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        
                    }
                    break;
                case ActionsEnum.ADDSHIFTDETAILS:

                    GetFieldValues(ControlEnum.HEADER_DDL);
                    SetFieldValues(ControlEnum.HEADER);
                    SetFieldValues(ControlEnum.HEADER_DDL);
                    ClearLine();
                    ResetForm();
                    divEntryForm.Visible = true;
                    divListing.Visible = false;
                    EntryStatus = EntryStatus.NEWMODE;
                    txtShiftDate.Focus();
                    break;
                case ActionsEnum.CANCEL:

                    GetFieldValues(ControlEnum.LISTING);
                    SetFieldValues(ControlEnum.LISTING);
                    divEntryForm.Visible = false;
                    divListing.Visible = true;
                    EntryStatus = EntryStatus.LISTMODE;
                    break;
                case ActionsEnum.EDIT_LIST_ACTION:
                    gvrItem = ((ImageButton)sender).Parent.Parent as GridViewRow;
                    shhPK = Convert.ToInt32(((Label)grdShiftReportList.Rows[gvrItem.RowIndex].FindControl("lblShhPK")).Text);
                    LastModifiedTime = Convert.ToDateTime(((HiddenField)grdShiftReportList.Rows[gvrItem.RowIndex].FindControl("hdfCreatedDate")).Value);
                    GetFieldValues(ControlEnum.EDIT_SHIFT_DETAILS);
                    SetFieldValues(ControlEnum.EDIT_SHIFT_DETAILS);
                    GetFieldValues(ControlEnum.PRODUCT);
                    SetFieldValues(ControlEnum.PRODUCT);
                    divEntryForm.Visible = true;
                    divListing.Visible = false;
                    EntryStatus = EntryStatus.ENTRYMODE;
                    break;
                case ActionsEnum.CHANGE:
                    if (((DropDownList)sender).ID == "ddlLine")
                    {
                        GetFieldValues(ControlEnum.PRODUCT);
                        SetFieldValues(ControlEnum.PRODUCT);
                        EntryStatus = EntryStatus.ENTRYMODE;
                        ddlShift.Focus();
                    }
                    break;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        protected void ActionHandler(Object sender, GridViewCommandEventArgs e)
        {
            #region grdFormerAllocation
            if (e.CommandName == "EXPANDMAIN")
            {

                if ((sender as GridView).ID == "grdFormerAllocation")
                {


                }
            }
            #endregion

        }
        protected void ActionHandler(object sender, GridViewEditEventArgs e)
        {

        }
        #endregion


        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            uclPaging.TotalPages = TotalPages;
            uclPaging.CurrentPage = 1;
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


                PageIndex = Convert.ToInt32(uclPaging.CurrentPage.ToString());
                GetFieldValues(ControlEnum.LISTING);
                SetFieldValues(ControlEnum.LISTING);
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

   

        #region Healper methods


        private void SetShiftData()
        {
            if (dsShiftDetails != null && dsShiftDetails.Tables.Count > 0 && dsShiftDetails.Tables[0].Rows.Count > 0)
            {
                txtShiftDate.Text = Convert.ToDateTime(dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.ShiftDate].ToString()).ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
                //lblShiftNo.Text = dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.shiftHeaderNo].ToString();
                //hdfShiftNo.Value = dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.shiftHeaderNo].ToString();

                hdfShiftNo.Value = dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.shiftHeaderNo].ToString() == string.Empty ? "" : dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.shiftHeaderNo].ToString();
                lblShiftNo.Text = dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.shiftHeaderNo].ToString() == string.Empty ? "[NEW]" : dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.shiftHeaderNo].ToString();

                ddlLine.SelectedValue = dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.ShiftLine].ToString();
                ddlShift.SelectedValue = dsShiftDetails.Tables[3].Rows[0][Resources.DataFieldRes.ShhShift].ToString();
                ddlLine.Enabled = false;

                lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                ModifiedDatePnl.Visible = true;
                //LastModifiedTime = SalContainerEvalHdrList[0].CVH_MOD_DT;
                //lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);



            }

            AddItemDetails(dsShiftDetails.Tables[4]);


        }


        protected void grdShiftReportList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer && dsShiftList != null)
            {
                if (dsShiftList.Tables[0].Rows.Count > 0)
                {

                    ((Label)e.Row.FindControl("lblActualQtyTotal")).Text = Convert.ToDecimal(dsShiftList.Tables[0].Rows[0][Resources.DataFieldRes.QtyActualTotal]).ToString("n0");
                    ((Label)e.Row.FindControl("lblProductQtyTotal")).Text = Convert.ToDecimal(dsShiftList.Tables[0].Rows[0][Resources.DataFieldRes.QtyProducedTotal]).ToString("n0");

                }

            }
        }

        /// <summary>
        /// Method for Print db messages
        /// </summary>
        private bool PrintDbMessages(DataSet dsMsg)
        {
            string strHdrMsg;
            bool RetVal = false;

            if (dsMsg != null)
                if (dsMsg.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetVal].ToString()) > 0)
                    {
                        RetVal = true;
                        //strHdrMsg = Resources.Messages.InformationSaved;
                        //BindGrid(ControlEnum.DBMSG);

                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.ShiftReport);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);


                        //DiverrorMessages.Attributes.Remove("class");
                        //DiverrorMessages.Attributes.Add("class", "message-sucess");
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowDbMsg", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300');", true);
                        //   Response.Redirect(Resources.PageURL.SaleOrder);
                    }
                    else
                    {
                         litErrorMsg.Text=dsMsg.Tables[0].Rows[0][Resources.DataFieldRes.dbRetValTxt].ToString();
                         ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //strHdrMsg = Resources.Messages.ErrorMessage;
                        //BindGrid(ControlEnum.DBMSG);
                        //DiverrorMessages.Attributes.Remove("class");
                        //DiverrorMessages.Attributes.Add("class", "message-error");

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ShowDbMsgErr", "ShowContainerDiv('#" + DiverrorMessages.ClientID + "','" + strHdrMsg + "','500','300');", true);

                    }


                }
            return RetVal;

        }

        private void ResetForm()
        {
            Session.Remove(ERP.Utilities.SessionStrings.ShiftDetails);
            shhPK = 0;
            hdfSLNo.Value = "0";
            GetFieldValues(ControlEnum.HEADER);
            txtShiftDate.Text = System.DateTime.Now.ToString(ERP.Utilities.CommonConstants.DATEFORMAT);
            if (dsShiftDetails != null && dsShiftDetails.Tables.Count > 0 && dsShiftDetails.Tables[0].Rows.Count > 0)
            {
                //lblShiftNo.Text = dsShiftDetails.Tables[0].Rows[0][0].ToString();
                //hdfShiftNo.Value = dsShiftDetails.Tables[0].Rows[0][0].ToString();

              
                // txtShiftNo.Enabled = false;
            }
            BindGrid(ControlEnum.SHIFT_DETAILS);
            ddlLine.Enabled = true;

        }


        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.SHIFT_DETAILS:
                    List<ShiftDetailBO> lstShiftDetail;
                    if (Session[ERP.Utilities.SessionStrings.ShiftDetails] != null)
                        lstShiftDetail = (List<ShiftDetailBO>)Session[ERP.Utilities.SessionStrings.ShiftDetails];
                    else
                        lstShiftDetail = new List<ShiftDetailBO>();
                    grdShiftLines.DataSource = lstShiftDetail;
                    grdShiftLines.DataBind();
                    break;
                case ControlEnum.DBMSG:
                    grdError.DataSource = dsDBMessgaes;
                    grdError.DataBind();
                    break;
                case ControlEnum.LISTING:
                    if (dsShiftList != null)
                    {
                        if (dsShiftList.Tables[0].Rows.Count > 0 && dsShiftList.Tables[0].Rows.Count > 0)
                            TotalPages = Convert.ToInt32(dsShiftList.Tables[0].Rows[0][Resources.DataFieldRes.PageCount].ToString());
                        uclPaging.TotalPages = TotalPages;
                        //  PageIndex = PageIndex == null ?Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                        grdShiftReportList.DataSource = dsShiftList;
                        grdShiftReportList.DataBind();

                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                    }

                    //
                    break;
            }
        }

        private void SetFilterXML(ControlsEnum type)
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            XmlDocument xmlDoc;
            NextDocNoBO objNextDocNo;
            ShifMasterDDlBO objShifMasterDDl;
            ProductMasterBO objProductMaster;
            ShiftListingBO objShiftListing;
            ShiftHeaderBO objShiftHeader;
            LineListBO objLineList;
            ShiftDetailsGetBO objShiftDetailsGet;

            P_XML = new string[6];
            switch (type)
            {
                case ControlsEnum.PRODUCT:
                    objProductMaster = new ProductMasterBO();
                    objProductMaster.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objProductMaster.BizUnit = CurrentUser.SBUID;
                    objProductMaster.category = Convert.ToInt32(BusinessObject.CommonManagement.ProductCategory.Product);
                    objProductMaster.linePK = ddlLine.SelectedValue;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objProductMaster);
                    P_XML[2] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.LISTING:
                    objShiftListing = new ShiftListingBO();
                    objShiftListing.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objShiftListing.BizUnit = CurrentUser.SBUID;
                    objShiftListing.PageNo = PageIndex;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShiftListing);
                    P_XML[4] = xmlDoc.InnerXml;


                    break;
                case ControlsEnum.HEADER:
                    //Next Doc Number
                    objNextDocNo = new NextDocNoBO();
                    objNextDocNo.Mode = Convert.ToInt32(Mode.ShiftReport);
                    objNextDocNo.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objNextDocNo);
                    P_XML[0] = xmlDoc.InnerXml;
                    break;
                case ControlsEnum.HEADER_DDL:
                    //Next Doc Number
                    objNextDocNo = new NextDocNoBO();
                    objNextDocNo.Mode = Convert.ToInt32(Mode.ShiftReport);
                    objNextDocNo.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objNextDocNo);
                    P_XML[0] = xmlDoc.InnerXml;

                    //Shift Master
                    objShifMasterDDl = new ShifMasterDDlBO();
                    objShifMasterDDl.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objShifMasterDDl.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShifMasterDDl);
                    P_XML[1] = xmlDoc.InnerXml;

                    //Product Master
                    objProductMaster = new ProductMasterBO();
                    objProductMaster.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objProductMaster.BizUnit = CurrentUser.SBUID;
                    objProductMaster.category = Convert.ToInt32(BusinessObject.CommonManagement.ProductCategory.Product);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objProductMaster);
                    P_XML[2] = xmlDoc.InnerXml;

                    //Line Master
                    objLineList = new LineListBO();
                    objLineList.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objLineList.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objLineList);
                    P_XML[3] = xmlDoc.InnerXml;

                    break;
                case ControlsEnum.EDIT_SHIFT_DETAILS:
                    //Shift Master
                    objShifMasterDDl = new ShifMasterDDlBO();
                    objShifMasterDDl.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objShifMasterDDl.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShifMasterDDl);
                    P_XML[1] = xmlDoc.InnerXml;

                    //Product Master
                    objProductMaster = new ProductMasterBO();
                    objProductMaster.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objProductMaster.BizUnit = CurrentUser.SBUID;
                    objProductMaster.category = Convert.ToInt32(BusinessObject.CommonManagement.ProductCategory.Product);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objProductMaster);
                    P_XML[2] = xmlDoc.InnerXml;

                    //Line Master
                    objLineList = new LineListBO();
                    objLineList.Active = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    objLineList.bizUnit = CurrentUser.SBUID;
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objLineList);
                    P_XML[3] = xmlDoc.InnerXml;

                    // Shift Header
                    objShiftHeader = new ShiftHeaderBO();
                    objShiftHeader.shhPK = shhPK;
                    objShiftHeader.Active = Convert.ToInt32(DbActiveStatus.HASPK);
                    objShiftHeader.bizUnit = CurrentUser.SBUID;
                    objShiftHeader.PageNo = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ONE);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShiftHeader);
                    P_XML[4] = xmlDoc.InnerXml;

                    //Shift Details
                    objShiftDetailsGet = new ShiftDetailsGetBO();
                    objShiftDetailsGet.shhPK = shhPK;
                    objShiftDetailsGet.bizUnit = CurrentUser.SBUID;
                    objShiftDetailsGet.PageNo = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShiftDetailsGet);
                    P_XML[5] = xmlDoc.InnerXml;

                    break;
            }
        }

        private void SetFieldValuesToXML()
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            XmlDocument xmlDoc;
            P_XML = new string[7];
            ShiftReportBO objShiftReport = new ShiftReportBO();
            objShiftReport.ACTIVE = shhPK == 0 ? Convert.ToInt32(DbActiveStatus.ACTIVE) : Convert.ToInt32(DbActiveStatus.HASPK);
            objShiftReport.ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
            objShiftReport.BIZUNIT = CurrentUser.SBUID;
            objShiftReport.DEPT_PK = CurrentUser.CurrentDeptPK;
            objShiftReport.USER_PK = CurrentUser.PKUser;
            objShiftReport.MODE = Convert.ToInt32(Mode.ShiftReport);
            objShiftReport.MODULE = Convert.ToInt32(ConfigurationSettings.AppSettings["gERPModule"].ToString());
            objShiftReport.SHH_SHIFT = Convert.ToInt32(ddlShift.SelectedValue.ToString());
            
            objShiftReport.SHH_LINE = Convert.ToInt32(ddlLine.SelectedValue.ToString());

            objShiftReport.SHH_PK = shhPK;
            if (hdfShiftNo.Value == string.Empty && AST_DOC_MODE.Value == ((int)DOCMODE.Draft).ToString())
            {
                GetFieldValues(ControlEnum.SHIFT);
                hdfShiftNo.Value = ShiftNo;
            }
            else
            {
                ShiftNo = hdfShiftNo.Value;
            }

            objShiftReport.SHH_NO = ShiftNo;


            objShiftReport.SHH_DATE = Convert.ToDateTime(txtShiftDate.Text);
            objShiftReport.ShiftDetailsList = new List<ShiftDetailsBO>();
            objShiftReport.ShiftDetailsList.Add(GetShiftDetails());

            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objShiftReport);
            P_XML[0] = xmlDoc.InnerXml;

        }

        private ShiftDetailsBO GetShiftDetails()
        {
            ShiftDetailsBO objShiftDetails = new ShiftDetailsBO();

            List<ShiftDetailBO> ShiftList = new List<ShiftDetailBO>();

            if (Session[ERP.Utilities.SessionStrings.ShiftDetails] != null)
                ShiftList = (List<ShiftDetailBO>)Session[ERP.Utilities.SessionStrings.ShiftDetails];
            else
                ShiftList = new List<ShiftDetailBO>();

            // Count = ShiftList.Count;
            objShiftDetails.ShiftDetailList = ShiftList;
            return objShiftDetails;
        }

        /// <summary>
        /// Method for Bind DropDown
        /// </summary>
        public void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SHIFT:
                        ddlShift.Items.Clear();
                        if (dsShiftDetails != null && dsShiftDetails.Tables.Count > 0 && dsShiftDetails.Tables[0].Rows.Count > 0)
                        {
                            ddlShift.DataSource = dsShiftDetails.Tables[0];
                            ddlShift.DataTextField = Resources.DataFieldRes.ShiftName;
                            ddlShift.DataValueField = Resources.DataFieldRes.ShiftPK;
                            ddlShift.DataBind();
                        }
                        ddlShift.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECTVAL));
                        ddlShift.SelectedIndex = Convert.ToInt32(ddlShift.Items.IndexOf(ddlShift.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL)));
                        break;
                    case ControlsEnum.LINE:

                        ddlLine.Items.Clear();
                        if (dsShiftDetails != null && dsShiftDetails.Tables.Count > 0 && dsShiftDetails.Tables[0].Rows.Count > 0)
                        {
                            ddlLine.DataSource = dsShiftDetails.Tables[2];
                            ddlLine.DataTextField = Resources.DataFieldRes.LineName;
                            ddlLine.DataValueField = Resources.DataFieldRes.LinePK;
                            ddlLine.DataBind();
                        }
                        ddlLine.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECTVAL));
                        ddlLine.SelectedIndex = Convert.ToInt32(ddlLine.Items.IndexOf(ddlLine.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL)));
                        break;

                    case ControlsEnum.PRODUCT:

                        ddlProduct.Items.Clear();
                        if (dsShiftList != null && dsShiftList.Tables.Count > 0 && dsShiftList.Tables[0].Rows.Count > 0)
                        {
                            ddlProduct.DataSource = dsShiftList.Tables[0];
                            ddlProduct.DataTextField = Resources.DataFieldRes.ItemText;
                            ddlProduct.DataValueField = Resources.DataFieldRes.proPK;
                            ddlProduct.DataBind();
                        }
                        ddlProduct.Items.Insert(0, new ListItem(Resources.Report.Select, ERP.Utilities.CommonConstants.SELECTVAL));
                        ddlProduct.SelectedIndex = Convert.ToInt32(ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(ERP.Utilities.CommonConstants.SELECTVAL)));
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.SFTRPT, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        #region Detaild part entry and updation
        /// <summary>
        /// Add Item Details to Grid
        /// </summary>
        private void AddItemDetails()
        {
            ddlLine.Enabled = false;
            List<ShiftDetailBO> lstShiftDetail;
            if (Session[ERP.Utilities.SessionStrings.ShiftDetails] != null)
                lstShiftDetail = (List<ShiftDetailBO>)Session[ERP.Utilities.SessionStrings.ShiftDetails];
            else
                lstShiftDetail = new List<ShiftDetailBO>();

            ShiftDetailBO objDetail = new ShiftDetailBO();
            if (hdfSLNo.Value == ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO)
            {
                objDetail.SL_NO = lstShiftDetail == null ? "1" : (lstShiftDetail.Count + 1).ToString();
                objDetail.SPD_PK = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                objDetail.SPD_PRODUCT = Convert.ToInt32(ddlProduct.SelectedValue.ToString());
                objDetail.PRO_CODE = ddlProduct.SelectedItem.Text;
                objDetail.SPD_QTY_PRODUCED = txtQty.Text == "" ? 0 : Convert.ToDecimal(txtQty.Text);
                objDetail.SPD_QTY_ACTUAL = txtGradeA.Text == "" ? 0 : Convert.ToDecimal(txtGradeA.Text);
                objDetail.STORE = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);
                lstShiftDetail.Add(objDetail);
                Session[ERP.Utilities.SessionStrings.ShiftDetails] = lstShiftDetail;
                BindGrid(ControlEnum.SHIFT_DETAILS);
            }
            else
            {

                for (int i = 0; i < lstShiftDetail.Count; i++)
                {
                    if (lstShiftDetail[i].SL_NO == hdfSLNo.Value)
                    {
                        lstShiftDetail[i].SPD_PK = Convert.ToInt32(ERP.Utilities.CommonConstants.SELECT_VALUE_ZERO);
                        lstShiftDetail[i].SPD_PRODUCT = Convert.ToInt32(ddlProduct.SelectedValue.ToString());
                        lstShiftDetail[i].PRO_CODE = ddlProduct.SelectedItem.Text;
                        lstShiftDetail[i].SPD_QTY_PRODUCED = txtQty.Text == "" ? 0 : Convert.ToDecimal(txtQty.Text);
                        lstShiftDetail[i].STORE = Convert.ToInt32(ERP.Utilities.CommonConstants.DEFAULT_STORE);
                        lstShiftDetail[i].SPD_QTY_ACTUAL = txtGradeA.Text == "" ? 0 : Convert.ToDecimal(txtGradeA.Text);
                        Session[ERP.Utilities.SessionStrings.ShiftDetails] = lstShiftDetail;
                        BindGrid(ControlEnum.SHIFT_DETAILS);

                    }
                }

            }
            ClearLine();

        }

        /// <summary>
        /// Add Item Details to Grid
        /// </summary>
        private void AddItemDetails(DataTable dtList)
        {
            List<ShiftDetailBO> lstShiftDetail = new List<ShiftDetailBO>();
            ShiftDetailBO objDetail;
            int i;
            for (i = 0; i < dtList.Rows.Count; i++)
            {
                objDetail = new ShiftDetailBO();
                objDetail.SL_NO = dtList.Rows[i][Resources.DataFieldRes.RowNo].ToString();
                objDetail.SPD_PK = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.spdPK].ToString());
                objDetail.SPD_PRODUCT = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.spdProduct].ToString());
                objDetail.PRO_CODE = dtList.Rows[i][Resources.DataFieldRes.ProductText].ToString();
                objDetail.SPD_QTY_PRODUCED = Convert.ToDecimal(dtList.Rows[i][Resources.DataFieldRes.QtyProduced].ToString());
                objDetail.SPD_QTY_ACTUAL = Convert.ToDecimal(dtList.Rows[i][Resources.DataFieldRes.QtyActual].ToString());
                objDetail.STORE = Convert.ToInt32(dtList.Rows[i][Resources.DataFieldRes.spdStore].ToString());
                lstShiftDetail.Add(objDetail);
            }
            Session[ERP.Utilities.SessionStrings.ShiftDetails] = lstShiftDetail;
            BindGrid(ControlEnum.SHIFT_DETAILS);

        }


        private void ClearLine()
        {
            txtQty.Text = string.Empty;
            txtGradeA.Text = string.Empty;
            hdfSLNo.Value = "0";
            ddlProduct.SelectedValue = "-1";

        }

        private void ClearEntryForm()
        {


        }

        /// <summary>
        /// Delete Item From the Item List
        /// </summary>
        private void DeleteItemDetails(int slNo)
        {
            List<ShiftDetailBO> lstShiftDetail;
            if (Session[ERP.Utilities.SessionStrings.ShiftDetails] != null)
                lstShiftDetail = (List<ShiftDetailBO>)Session[ERP.Utilities.SessionStrings.ShiftDetails];
            else
                lstShiftDetail = new List<ShiftDetailBO>();
            if (lstShiftDetail.Count > 0)
            {
                for (int i = 0; i < lstShiftDetail.Count; i++)
                {
                    if (lstShiftDetail[i].SL_NO == slNo.ToString())
                    {
                        lstShiftDetail.RemoveAt((slNo - 1));
                    }
                }
                UpdateItemList();
                Session[ERP.Utilities.SessionStrings.ShiftDetails] = lstShiftDetail;
                BindGrid(ControlEnum.SHIFT_DETAILS);

            }


        }

        /// <summary>
        /// Delete Item From the Item List
        /// </summary>
        private void FillItemDetails(int slNo)
        {

            List<ShiftDetailBO> lstShiftDetail;
            if (Session[ERP.Utilities.SessionStrings.ShiftDetails] != null)
                lstShiftDetail = (List<ShiftDetailBO>)Session[ERP.Utilities.SessionStrings.ShiftDetails];
            else
                lstShiftDetail = new List<ShiftDetailBO>();

            if (lstShiftDetail.Count > 0)
            {
                for (int i = 0; i < lstShiftDetail.Count; i++)
                {
                    if (lstShiftDetail[i].SL_NO == slNo.ToString())
                    {
                        ddlProduct.SelectedIndex = ddlProduct.Items.IndexOf(ddlProduct.Items.FindByValue(lstShiftDetail[i].SPD_PRODUCT.ToString()));
                        // ddlProduct.SelectedValue  =lstShiftDetail[i].SPD_PRODUCT.ToString();
                        txtGradeA.Text = lstShiftDetail[i].SPD_QTY_ACTUAL.ToString();
                        txtQty.Text = lstShiftDetail[i].SPD_QTY_PRODUCED.ToString();
                        hdfSLNo.Value = slNo.ToString();

                       

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
            List<ShiftDetailBO> lstShiftDetail;
            if (Session[ERP.Utilities.SessionStrings.ShiftDetails] != null)
                lstShiftDetail = (List<ShiftDetailBO>)Session[ERP.Utilities.SessionStrings.ShiftDetails];
            else
                lstShiftDetail = new List<ShiftDetailBO>();
            if (lstShiftDetail.Count > 0)
            {

                for (int i = 0; i < lstShiftDetail.Count; i++)
                {
                    lstShiftDetail[i].SL_NO = (i + 1).ToString();
                }

            }
            Session[ERP.Utilities.SessionStrings.ShiftDetails] = lstShiftDetail;
            BindGrid(ControlEnum.SHIFT_DETAILS);
        }
        #endregion


        #endregion

        #region Action methods

        public  DataSet FetchDbValues(int val, string[] XML)
        {
            return ShiftReportDL.GetShiftReport(val, P_XML);

        }

        public  DataSet SetValuesToDB(int val, string[] XML)
        {
            return ShiftReportDL.SaveShiftReport(val, XML);
        }

        #endregion

        #region Control Enum
        private enum ActionsEnum
        {
            GRID,
            ADD,
            SAVE,
            CANCEL,
            DELETE,
            EDIT_ACTION,
            SELECT,
            DELETE_ACTION,
            CONTINUE,
            CHANGE,
            ADDSHIFTDETAILS,
            EDIT_LIST_ACTION,
            DELETE_LIST_ACTION


        }
        /// <summary>
        /// To control Page Actions
        /// </summary>
        public enum ControlsEnum
        {
            HEADER_DDL,
            SHIFT,
            PRODUCT,
            LINE,
            HEADER,
            LISTING,
            EDIT_SHIFT_DETAILS,
            SHIFTNO

        }
        #endregion
    }
}