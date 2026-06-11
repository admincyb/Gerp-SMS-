using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;

using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using System.Xml;
using ERPSMS_v01.UserControls;
using System.Threading;
using ERP.Utilities.HRMS;
using CustomControls;

namespace HRMS.Admin.Masters
{
    public partial class SlabDefinitionMaster : ERP.Store.UI.MyBasePage
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
        /// To keep values in view state
        /// </summary>
        private List<SlabDefinitionBO.SlabDefinitionDetail> SlabDefinitionDetailList
        {
            get
            {
                return ViewState[ViewstateStrings.SlabDefinitionDetailList] == null ? new List<SlabDefinitionBO.SlabDefinitionDetail>() : (List<SlabDefinitionBO.SlabDefinitionDetail>)ViewState[ViewstateStrings.SlabDefinitionDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.SlabDefinitionDetailList] = value;
            }
        }

        private SlabDefinitionBO.SlabDefinition SlabDefinitionViewState
        {
            get
            {
                return ViewState[ViewstateStrings.SlabDefinitionViewState] == null ? new SlabDefinitionBO.SlabDefinition() : (SlabDefinitionBO.SlabDefinition)ViewState[ViewstateStrings.SlabDefinitionViewState];
            }
            set
            {
                ViewState[ViewstateStrings.SlabDefinitionViewState] = value;
            }
        }

        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        /// 
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

        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
            }
        }

        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }

        /// <summary>
        /// To keep Current PK in view state
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
        /// To keep Current PK in view state
        /// </summary>
        private int VersionPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.VersionPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VersionPK] = value;
            }
        }

        private int IsHdrFormula
        {
            get
            {
                return (int)ViewState[ViewstateStrings.IsHdrFormula];
            }
            set
            {
                ViewState[ViewstateStrings.IsHdrFormula] = value;
            }
        }

        #endregion

        private BusinessObject.User currentUser;
        private DataTable dtCompany;
        private DataTable dtResult;
        private DataSet dsPageData;
        private int PayElementPk = 0;

        private string dummyPK { get; set; }
        private SlabDefinitionBO.SlabDefinition objSlabDefinition;
        private List<SlabDefinitionBO.VersionDetail> tempVerList;
        private SlabDefinitionBO.VersionDetail objTempVer;
        #endregion

        #region PageEvents
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            //lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
            //    "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
            if (!txtPayElement.Enabled)
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_Disableauto", "Disableautocomplete();", true);
        }
        #endregion

        #region InitializeComponent
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
        #endregion

        #region Custom Pager Control Navigated Event
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
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndexList = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
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
                ucFormula.AfterApply += new EventHandler(ucFormula_AfterApply);
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                InitializeComponent();
                if (!IsPostBack)
                {
                    //set of hidden fields used to format Quantity, Amount, Rate
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }

                    uclPaging.CurrentPage = 1;
                    ResetForm(ControlsEnum.CLEAR);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            int result;
            XmlDocument xmlDoc;
            List<SlabDefinitionBO.SlabDefinitionDetail> tempList = SlabDefinitionDetailList;
            Dictionary<int, string> Employees = new Dictionary<int, string>();
            bool bIsChecked = false;
            GridViewRow grvRow;
            string strPayaelementText;
            string arg;
            ExtGridViewRow extGvr;
            GridView grd;
            try
            {
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
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
                #endregion
                switch (commonActions)
                {
                    #region LIST
                    case ActionsEnum.LIST:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        if (RowIndex >= 0 && tempList != null && tempList.Count > 0) // update
                        {
                            List<SlabDefinitionBO.SlabDefinitionDetail> objTempList = tempList.DeepClone();
                            SlabDefinitionBO.SlabDefinitionDetail existSlab = tempList[RowIndex];

                            objTempList.Remove(objTempList[RowIndex]);
                            if (objTempList.Where(r => (Convert.ToDecimal(txtRangeFrom.Text) > r.PDS_RANGE_FROM && Convert.ToDecimal(txtRangeFrom.Text) < r.PDS_RANGE_TO)
                                                   || (Convert.ToDecimal(txtRangeTo.Text) > r.PDS_RANGE_FROM && Convert.ToDecimal(txtRangeTo.Text) < r.PDS_RANGE_TO)
                                                   || (Convert.ToDecimal(txtRangeFrom.Text) == r.PDS_RANGE_FROM && Convert.ToDecimal(txtRangeTo.Text) == r.PDS_RANGE_TO)).Count() > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_RangeExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                               + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            existSlab.PDS_RANGE_FROM = Convert.ToDecimal(txtRangeFrom.Text);
                            existSlab.PDS_RANGE_TO = Convert.ToDecimal(txtRangeTo.Text);
                            existSlab.PDS_VALUE = hdfRangeValue.Value;
                            existSlab.PDS_VALUE_TEXT = txtRangeValue.Text;
                            existSlab.PDS_SLAB_HDR = CurrPK;
                        }
                        else // new
                        {
                            if (tempList == null)
                                tempList = new List<SlabDefinitionBO.SlabDefinitionDetail>();
                            SlabDefinitionBO.SlabDefinitionDetail objSlab = new SlabDefinitionBO.SlabDefinitionDetail();
                            objSlab.PDS_PK = 0;
                            objSlab.PDS_SLAB_HDR = CurrPK;
                            objSlab.PDS_RANGE_FROM = Convert.ToDecimal(txtRangeFrom.Text);
                            objSlab.PDS_RANGE_TO = Convert.ToDecimal(txtRangeTo.Text);
                            objSlab.PDS_VALUE = hdfRangeValue.Value;
                            objSlab.PDS_VALUE_TEXT = txtRangeValue.Text;
                            if (tempList.Where(r => (objSlab.PDS_RANGE_FROM > r.PDS_RANGE_FROM && objSlab.PDS_RANGE_FROM < r.PDS_RANGE_TO)
                                                     || (objSlab.PDS_RANGE_TO > r.PDS_RANGE_FROM && objSlab.PDS_RANGE_TO < r.PDS_RANGE_TO)
                                                     || (objSlab.PDS_RANGE_FROM == r.PDS_RANGE_FROM && objSlab.PDS_RANGE_TO == r.PDS_RANGE_TO)).Count() > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_RangeExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                               + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            else
                                tempList.Add(objSlab);
                        }
                        SlabDefinitionDetailList = tempList;
                        tempVerList = new List<SlabDefinitionBO.VersionDetail>();
                        objTempVer = new SlabDefinitionBO.VersionDetail();
                        objTempVer.SlabDefinitionDetails = tempList;
                        tempVerList.Add(objTempVer);
                        //SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails = tempList;
                        SlabDefinitionViewState.VersionDetails = tempVerList;
                        ResetForm(ControlsEnum.ADDTOLIST);
                        SetFieldValues(ControlsEnum.SLABDETAILSLIST);
                        break;
                    #endregion
                    #region GRID EDIT
                    case ActionsEnum.GRIDEDIT:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlsEnum.GRIDEDIT);
                        break;
                    #endregion
                    #region GRID DELETE
                    case ActionsEnum.GRIDDELETE:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlsEnum.GRIDDELETE);
                        SetFieldValues(ControlsEnum.SLABDETAILSLIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        SlabDefinitionViewState = (SlabDefinitionBO.SlabDefinition)SetUIValuesToObject(ControlsEnum.SAVE);

                        if (SlabDefinitionViewState != null
                        && SlabDefinitionViewState.VersionDetails != null
                        && SlabDefinitionViewState.VersionDetails.Count > 0
                        && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails != null
                        && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.Count > 0)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(SlabDefinitionViewState);
                            result = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.SaveSlabDefinition(xmlDoc.InnerXml);
                            if (result > 0)
                            {
                                ResetForm(ControlsEnum.CLEAR);
                                ResetForm(ControlsEnum.CLEARSEARCH);
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                                this.EntryStatus = EntryStatus.LISTMODE;
                                litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SlabDefinitions + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SlabDefinitions + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.SlabDefinitions + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.ALREADYEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("SlabDatePeriodAlteadyExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = String.Format(Resources.ErrorMessages.Msg_Code_Already_Exists, GetLocalResourceObject("SlabDefinitions").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        else 
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        //foreach (GridViewRow grdrow in grdList.Rows)
                        //{
                        //    RadioButton rbtn;
                        //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //    if (rbtn.Checked)
                        //    {
                        //        bIsChecked = true;
                        //        ResetForm(ControlsEnum.CLEAR);
                        //        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSlabPk")).Value);
                        //        break;
                        //    }
                        //}
                        bIsChecked = false;
                        foreach (ExtGridViewRow grdrow in grdList.Rows)
                        {
                            CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSlabPk")).Value);
                            GridView grdVersionList = grdrow.FindControl("grdVersionList") as GridView;
                            if (grdVersionList != null)
                            {
                                foreach (GridViewRow egrdrow in grdVersionList.Rows)
                                {
                                    RadioButton rbtn;
                                    rbtn = (RadioButton)egrdrow.FindControl("rbtSelect");
                                    if (rbtn.Checked)
                                    {
                                        bIsChecked = true;
                                        VersionPK = Convert.ToInt32(((HiddenField)egrdrow.FindControl("hdfVersionPk")).Value);
                                        break;
                                    }
                                }
                            }
                            if (bIsChecked)
                                break;
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.SLABDETAILS);
                            SetFieldValues(ControlsEnum.SLABDETAILS);
                            SetFieldValues(ControlsEnum.SLABDETAILSLIST);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        SlabDefinitionViewState = new SlabDefinitionBO.SlabDefinition();
                        SlabDefinitionDetailList = null;
                        ResetForm(ControlsEnum.CLEAR);
                        SetFieldValues(ControlsEnum.SLABDETAILSLIST);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.DeleteSlabDefinition(SlabDefinitionViewState.PHS_PK, VersionPK, SlabDefinitionViewState.LAST_MOD_DT);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SlabDefinitions;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SlabDefinitions + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SlabDefinitions + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);

                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;

                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region BASED ON FORMULA POPUP
                    case ActionsEnum.BASEDONFORMULAPOPUP:
                        IsHdrFormula = 1;
                        strPayaelementText = string.Empty;
                        if (!string.IsNullOrEmpty(txtPayElement.Text.Trim()) && !txtPayElement.Text.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                        {
                            strPayaelementText = txtPayElement.Text;
                            PayElementPk = Convert.ToInt32(hdfPayElement.Value);
                        }
                        GetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        SetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        ucFormula.IsSlab = 0;
                        ucFormula.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PARAMETERS);
                        ucFormula.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PARAMETERS);
                        ucFormula.SetData(txtBasedOn.Text, strPayaelementText, 0, 0);
                        ShowFormulaPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                        break;
                    #endregion
                    #region RANGE VALUE FORMULA POPUP
                    case ActionsEnum.RANGEFORMULAPOPUP:
                        IsHdrFormula = 0;
                        strPayaelementText = string.Empty;
                        if (!string.IsNullOrEmpty(txtPayElement.Text.Trim()) && !txtPayElement.Text.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                        {
                            strPayaelementText = txtPayElement.Text;
                            PayElementPk = Convert.ToInt32(hdfPayElement.Value);
                        }

                        GetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        SetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                        ucFormula.IsSlab = 1;
                        ucFormula.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PARAMETERS);
                        ucFormula.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PARAMETERS);
                        ucFormula.SetData(txtRangeValue.Text, txtPayElement.Text, 0, 0);
                        ShowFormulaPopup();
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                        break;
                    #endregion
                    #region ACTIVATE
                    case ActionsEnum.ACTIVATE:
                        grvRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.UpdateSlabDefinitionStatus(Convert.ToInt32(((HiddenField)grvRow.FindControl("hdfSlabPk")).Value), (int)DbActiveStatus.ACTIVE, currentUser.PKUser, Convert.ToDateTime(((HiddenField)grvRow.FindControl("hdfModifiedDate")).Value));
                        if (result > 0)
                        {
                            uclPaging.CurrentPage = 0;
                            this.PageIndexList = "1";
                            this.CurrPK = 0;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region DEACTIVATE
                    case ActionsEnum.DEACTIVATE:
                        grvRow = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.UpdateSlabDefinitionStatus(Convert.ToInt32(((HiddenField)grvRow.FindControl("hdfSlabPk")).Value), (int)DbActiveStatus.INACTIVE, currentUser.PKUser, Convert.ToDateTime(((HiddenField)grvRow.FindControl("hdfModifiedDate")).Value));
                        if (result > 0)
                        {
                            uclPaging.CurrentPage = 0;
                            this.PageIndexList = "1";
                            this.CurrPK = 0;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SlabDefinitions.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        break;
                    #endregion
                    #region PAY ELEMENT CHANGE
                    case ActionsEnum.PAYELEMENTCHANGE:
                        if (string.IsNullOrEmpty(txtPayElement.Text.Trim()) || txtPayElement.Text.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                        {
                            hdfPayElement.Value = "0";
                        }
                        txtBasedOn.Text = txtBasedOn.ToolTip = string.Empty;
                        hdfBasedOnValue.Value = string.Empty;
                        txtRangeValue.Text = string.Empty;
                        hdfRangeValue.Value = string.Empty;
                        break;
                    #endregion
                    #region VERSION LIST
                    case ActionsEnum.VERSIONLIST:
                        arg = ((Button)sender).CommandArgument;
                        extGvr = ((Button)sender).Parent.Parent as ExtGridViewRow;
                        if (extGvr != null)
                        {
                            grd = extGvr.FindControl("grdVersionList") as GridView;
                            if (string.IsNullOrEmpty(arg))
                            {
                                objSlabDefinition = null;
                            }
                            else
                            {
                                CurrPK = Convert.ToInt32(arg);
                                GetFieldValues(ControlsEnum.VERSIONLIST);
                            }
                            grd.Visible = true;
                            if (objSlabDefinition != null && objSlabDefinition.VersionDetails != null && objSlabDefinition.VersionDetails.Count > 0)
                                grd.DataSource = objSlabDefinition.VersionDetails;
                            else
                                grd.DataSource = null;
                            grd.DataBind();

                            (extGvr.FindControl("hdfIsExpandedSlab") as HiddenField).Value = "1";
                        }
                        break;
                    #endregion
                    #region ADD NEW VERSION
                    case ActionsEnum.ADDNEWVERSION:
                        extGvr = ((ImageButton)sender).Parent.Parent as ExtGridViewRow;
                        if (extGvr != null)
                        {
                            VersionPK = 0;
                            CurrPK = Convert.ToInt32(((HiddenField)extGvr.FindControl("hdfSlabPk")).Value);
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.SLABDETAILS);
                            SetFieldValues(ControlsEnum.SLABDETAILS);
                            SetFieldValues(ControlsEnum.SLABDETAILSLIST);
                        }
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        private void ShowFormulaPopup()
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','230');", true);
        }
        #endregion

        #region --- For Grid Actions----
        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>       
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            //BusinessObject.GridPrams gridParam;
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        FilterParameters objFilterParam = new FilterParameters();
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.Code = txtSearchCode.Text.Trim();
                        objFilterParam.Name = txtSearchName.Text.Trim();
                        objFilterParam.Status = null;
                        if (string.IsNullOrEmpty(txtSearchPayElement.Text.Trim()) || txtSearchPayElement.Text.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                            hdfSearchPayElement.Value = string.Empty;
                        objFilterParam.PayElementPk = string.IsNullOrEmpty(hdfSearchPayElement.Value) ? (int?)null : Convert.ToInt32(hdfSearchPayElement.Value);
                        dsPageData = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.GetSlabDefinitionList(objFilterParam, currentUser.SBUID);
                        break;
                    #endregion
                    #region SLAB DETAILS
                    case ControlsEnum.SLABDETAILS:
                        string xmlData = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.GetSlabDefinitionDetails(GetNullableInt(Convert.ToString(CurrPK)).Value, currentUser.SBUID, (int)DbActiveStatus.HASPK, null, VersionPK);
                        SlabDefinitionBO.SlabDefinition tempSlabMaster;
                        if (xmlData == "<Root/>")
                        {
                            tempSlabMaster = new SlabDefinitionBO.SlabDefinition();
                        }
                        else
                        {
                            tempSlabMaster = CommonFunctions.XmlDeserialize<SlabDefinitionBO.SlabDefinition>(xmlData);
                        }
                        SlabDefinitionViewState = tempSlabMaster;
                        break;
                    #endregion
                    #region VERSION LIST
                    case ControlsEnum.VERSIONLIST:
                        string xmlResult = BusinessLogic.HRMS.Admin.Masters.SlabDefinitionMasterBL.GetSlabDefinitionDetails(GetNullableInt(Convert.ToString(CurrPK)).Value, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        SlabDefinitionBO.SlabDefinition tempSlabMast;
                        if (xmlResult == "<Root/>")
                        {
                            tempSlabMast = new SlabDefinitionBO.SlabDefinition();
                        }
                        else
                        {
                            tempSlabMast = CommonFunctions.XmlDeserialize<SlabDefinitionBO.SlabDefinition>(xmlResult);
                        }
                        objSlabDefinition = tempSlabMast;
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(PayElementPk, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
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
                    #region SLAB DETAILS LIST
                    case ControlsEnum.SLABDETAILSLIST:
                        BindGrid(ControlsEnum.SLABDETAILSLIST);
                        break;
                    #endregion
                    #region SLAB DETAILS
                    case ControlsEnum.SLABDETAILS:
                        GetUIValuesFromObject(ControlsEnum.SLABDETAILS);
                        break;
                    #endregion
                    #region GRID EDIT
                    case ControlsEnum.GRIDEDIT:
                        GetUIValuesFromObject(ControlsEnum.GRIDEDIT);
                        break;
                    #endregion
                    #region GRID DELETE
                    case ControlsEnum.GRIDDELETE:
                        GetUIValuesFromObject(ControlsEnum.GRIDDELETE);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        GetUIValuesFromObject(ControlsEnum.PAYELEMENTDETAILS);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                default:
                    break;
            }
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region SLAB DETAILS LIST
                    case ControlsEnum.SLABDETAILSLIST:
                        if (
                            SlabDefinitionViewState.VersionDetails != null
                            && SlabDefinitionViewState.VersionDetails.Count > 0
                            && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails != null
                            && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.Count > 0)
                        {
                            grdSlabDetList.DataSource = SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails;
                            grdSlabDetList.DataBind();
                            txtPayElement.Enabled = false;
                        }
                        else
                        {
                            grdSlabDetList.DataSource = null;
                            grdSlabDetList.DataBind();
                            txtPayElement.Enabled = true;
                        }
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdList.DataSource = dsPageData.Tables[0];
                        grdList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "GetUIValuesFromObject"
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {

                switch (controlType)
                {


                    #region SLAB DETAILS
                    case ControlsEnum.SLABDETAILS:
                        if (SlabDefinitionViewState != null)
                        {

                            txtAddnAmnt.Text = GetFormattedCurrency(SlabDefinitionViewState.PHS_ADD_AMT);
                            txtMaxAmnt.Text = GetFormattedCurrency(SlabDefinitionViewState.PHS_MAX_AMT);
                            txtMinAmnt.Text = GetFormattedCurrency(SlabDefinitionViewState.PHS_MIN_AMT);
                            hdfBasedOnValue.Value = SlabDefinitionViewState.PHS_BASED_ON;
                            txtSlabCode.Text = HttpUtility.HtmlDecode(SlabDefinitionViewState.PHS_CODE);
                            txtSlabName.Text = HttpUtility.HtmlDecode(SlabDefinitionViewState.PHS_NAME);
                            txtDescription.Text = HttpUtility.HtmlDecode(SlabDefinitionViewState.PHS_DESC);
                            //txtFromDate.Text = SlabDefinitionViewState.PHS_EFFECT_FROM.ToString(Resources.Constants.HRMSDateFormatShort);
                            //txtToDate.Text = SlabDefinitionViewState.PHS_EFFECT_TO.ToString(Resources.Constants.HRMSDateFormatShort);
                            if (SlabDefinitionViewState.PHS_IS_DIRECT > 0)
                                chkDirect.Checked = true;
                            else
                                chkDirect.Checked = false;
                            if (SlabDefinitionViewState.PHS_ACTIVE > 0)
                                chkActive.Checked = true;
                            else
                                chkActive.Checked = false;
                            txtPayElement.Text = SlabDefinitionViewState.PHS_PAY_ELEMENT_TEXT;
                            hdfPayElement.Value = SlabDefinitionViewState.PHS_PAY_ELEMENT.ToString();
                            txtBasedOn.Text=txtBasedOn.ToolTip = SlabDefinitionViewState.PHS_BASED_ON_TEXT;
                            hdfBasedOnValue.Value = SlabDefinitionViewState.PHS_BASED_ON;
                            LastModifiedTime = SlabDefinitionViewState.LAST_MOD_DT;
                            if (SlabDefinitionViewState.VersionDetails != null && SlabDefinitionViewState.VersionDetails.Count > 0)
                            {
                                txtFromDate.Text = SlabDefinitionViewState.VersionDetails[0].PEV_EFFECT_FROM.ToString(Resources.Constants.HRMSDateFormatShort);
                                txtToDate.Text = SlabDefinitionViewState.VersionDetails[0].PEV_EFFECT_TO.ToString(Resources.Constants.HRMSDateFormatShort);
                                txtVerDesc.Text = HttpUtility.HtmlDecode(SlabDefinitionViewState.VersionDetails[0].PEV_DESC);
                                if (SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails != null)
                                    SlabDefinitionDetailList = SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.ToList();
                            }
                            else
                            {
                                SlabDefinitionDetailList = null;
                                txtFromDate.Text = string.Empty;
                                txtToDate.Text = string.Empty;
                                txtVerDesc.Text = string.Empty;
                            }
                        }
                        break;
                    #endregion
                    #region GRID EDIT
                    case ControlsEnum.GRIDEDIT:
                        if (SlabDefinitionViewState != null && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.Count > 0 && RowIndex >= 0)
                        {
                            SlabDefinitionBO.SlabDefinitionDetail objSlabDet = SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails[RowIndex];
                            if (objSlabDet != null)
                            {
                                txtRangeFrom.Text = GetFormattedCurrency(objSlabDet.PDS_RANGE_FROM);
                                txtRangeTo.Text = GetFormattedCurrency(objSlabDet.PDS_RANGE_TO);
                                txtRangeValue.Text = objSlabDet.PDS_VALUE_TEXT;
                                hdfRangeValue.Value = objSlabDet.PDS_VALUE;
                            }
                        }
                        break;
                    #endregion
                    #region GRID DELETE
                    case ControlsEnum.GRIDDELETE:
                        if (SlabDefinitionViewState != null && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.Count > 0 && RowIndex >= 0)
                        {
                            SlabDefinitionDetailList = SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.ToList();
                            SlabDefinitionBO.SlabDefinitionDetail objSlabDet = SlabDefinitionDetailList[RowIndex];
                            if (objSlabDet != null)
                            {
                                SlabDefinitionDetailList.Remove(objSlabDet);
                                SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails = SlabDefinitionDetailList;
                            }
                        }
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ucFormula.IsDeduction = Convert.ToInt32(dtResult.Rows[0]["PEL_IS_DEDUCTION"]);
                        }
                        ucFormula.GetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        ucFormula.SetFieldValues(Admin.Masters.UserControls.FormulaMaster.ControlsEnum.PAYELEMENT);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            switch (controlType)
            {
                #region SAVE
                case ControlsEnum.SAVE:
                    //List<SlabDefinitionBO.SlabDefinitionDetail> tempDetails = SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails;
                    //if (tempDetails == null) tempDetails = new List<SlabDefinitionBO.SlabDefinitionDetail>();
                    SlabDefinitionBO.SlabDefinition tempSlabMaster = SlabDefinitionViewState;
                    tempSlabMaster.PHS_PK = CurrPK;
                    tempSlabMaster.PHS_ADD_AMT = Convert.ToDecimal(txtAddnAmnt.Text);
                    tempSlabMaster.PHS_BASED_ON = hdfBasedOnValue.Value;
                    tempSlabMaster.PHS_CODE = HttpUtility.HtmlEncode(txtSlabCode.Text);
                    tempSlabMaster.PHS_NAME = HttpUtility.HtmlEncode(txtSlabName.Text);
                    tempSlabMaster.PHS_DESC = string.IsNullOrEmpty(txtDescription.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtDescription.Text);
                    //tempSlabMaster.PHS_EFFECT_FROM = Convert.ToDateTime(txtFromDate.Text);
                    //tempSlabMaster.PHS_EFFECT_TO = Convert.ToDateTime(txtToDate.Text);
                    tempSlabMaster.PHS_IS_DIRECT = chkDirect.Checked ? (int)DbActiveStatus.ACTIVE : (int)DbActiveStatus.INACTIVE;
                    tempSlabMaster.PHS_MAX_AMT = Convert.ToDecimal(txtMaxAmnt.Text);
                    tempSlabMaster.PHS_MIN_AMT = Convert.ToDecimal(txtMinAmnt.Text);
                    tempSlabMaster.PHS_PAY_ELEMENT = Convert.ToInt32(hdfPayElement.Value);
                    tempSlabMaster.USER_PK = currentUser.PKUser;
                    tempSlabMaster.PHS_DEPT = currentUser.CurrentDeptPK;
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempSlabMaster.PHS_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempSlabMaster.PHS_BIZUNIT = currentUser.SBUID;
                    tempSlabMaster.PHS_ACTIVE = chkActive.Checked ? (int)DbActiveStatus.ACTIVE : (int)DbActiveStatus.INACTIVE;
                    tempSlabMaster.LAST_MOD_DT = LastModifiedTime;

                    // version details
                    tempVerList = new List<SlabDefinitionBO.VersionDetail>();
                    objTempVer = new SlabDefinitionBO.VersionDetail();
                    objTempVer.PEV_DESC = string.IsNullOrEmpty(txtVerDesc.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtVerDesc.Text);
                    objTempVer.PEV_EFFECT_FROM = Convert.ToDateTime(txtFromDate.Text);
                    objTempVer.PEV_EFFECT_TO = Convert.ToDateTime(txtToDate.Text);
                    objTempVer.PEV_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    objTempVer.PEV_PK = VersionPK;
                    tempVerList.Add(objTempVer);
                    if (SlabDefinitionViewState != null
                        && SlabDefinitionViewState.VersionDetails != null
                        && SlabDefinitionViewState.VersionDetails.Count > 0
                        && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails != null
                        && SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails.Count > 0)
                    {
                        objTempVer.SlabDefinitionDetails = SlabDefinitionViewState.VersionDetails[0].SlabDefinitionDetails;
                    }
                    //tempVersionDetails[0].SlabDefinitionDetails = tempDetails;
                    tempSlabMaster.VersionDetails = tempVerList;
                    returnObject = tempSlabMaster;
                    break;
                #endregion
            }
            return returnObject;
        }
        #endregion
        #region FORMULA APPLY
        void ucFormula_AfterApply(object sender, EventArgs e)
        {
            if (IsHdrFormula > 0)
            {
                txtBasedOn.Text = txtBasedOn.ToolTip = ucFormula.FormulaText;
                hdfBasedOnValue.Value = ucFormula.FormulaValue;
            }
            else
            {
                txtRangeValue.Text = ucFormula.FormulaText;
                hdfRangeValue.Value = ucFormula.FormulaValue;
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    VersionPK = 0;
                    RowIndex = -1;
                    txtSlabCode.Text = string.Empty;
                    txtSlabName.Text = string.Empty;
                    txtPayElement.Text = string.Empty;
                    hdfPayElement.Value = string.Empty;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    txtBasedOn.Text = txtBasedOn.ToolTip = string.Empty;
                    hdfBasedOnValue.Value = string.Empty;
                    chkDirect.Checked = false;
                    chkActive.Checked = true;
                    txtAddnAmnt.Text = txtMaxAmnt.Text = txtMinAmnt.Text = txtRangeFrom.Text = txtRangeTo.Text = GetFormattedCurrency(0);
                    txtDescription.Text = string.Empty;
                    txtVerDesc.Text = string.Empty;
                    txtRangeValue.Text = string.Empty;
                    hdfRangeValue.Value = string.Empty;
                    uclPaging.CurrentPage = 0;
                    PageIndexList = "1";
                    break;
                #endregion
                #region ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    txtRangeFrom.Text = txtRangeTo.Text = GetFormattedCurrency(0);
                    txtRangeValue.Text = string.Empty;
                    hdfRangeValue.Value = string.Empty;
                    RowIndex = -1;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtSearchPayElement.Text = hdfSearchPayElement.Value = string.Empty;                   
                    txtSearchCode.Text = string.Empty;
                    txtSearchName.Text = string.Empty;
                    break;
                #endregion
            }
        }
        #endregion

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

        #region UtitlityMethods
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            return (decimal.TryParse(str, out result) ? (decimal?)result : null);
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            string format = "#0.0";
            string s = num.ToString(format);
            return s;
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            COMPANY,
            CLEARSEARCH,
            CLEAR,
            LIST,
            ADDTOLIST,
            SLABDETAILSLIST,
            SAVE,
            SLABDETAILS,
            GRIDEDIT,
            GRIDDELETE,
            PAYELEMENTDETAILS,
            VERSIONLIST
        }
        #endregion
    }
}