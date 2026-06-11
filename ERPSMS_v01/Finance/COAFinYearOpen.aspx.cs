using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;
using ERP.Utilities;
using ERPManager;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Finance
{
    public partial class COAFinYearOpen : ERP.Store.UI.WorkFlowBasePage//ERP.Store.UI.MyBasePage
    {
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
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }

        private int ProcessID
        {
            get
            {
                return this.ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["ProcessID"].ToString());
            }
            set
            {
                this.ViewState["ProcessID"] = value;
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

        private COAFinYearOpening finYearOpening
        {
            get
            {
                return ViewState["finYearOpening"] == null ? null : (COAFinYearOpening)ViewState["finYearOpening"];
            }
            set
            {
                if (ViewState["finYearOpening"] != null)
                    ViewState.Remove("finYearOpening");
                ViewState.Add("finYearOpening", value);
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

        #endregion
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private int processPK;
        private string refID;
        private string inboxFlag;

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
            ScriptManager.GetCurrent(this).AsyncPostBackTimeout = 600;
        }

        #region Pager Methods + Init

        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);

            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
        }

        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }

        void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }

        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
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
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
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
            // Should we disable the first link
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                }
                SetViewMode(EntryStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            string prefID;
            try
            {
                ucrWrkf.ViewType = 1;
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    this.DataBind();
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithComma.Value = "#" + currencysep + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormatWithComma.Value += "0";
                    }

                    #region Process & Workflow   
                    FillProcessID();
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                      : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        ReferanceID = int.Parse(refID);
                        ucrWrkf.RefID = int.Parse(refID);
                        //base.WkfRefID = ucrWrkf.RefID;
                        CurrPK = GetApplicationID(ucrWrkf.RefID);

                        ucrWrkf.ViewType = 1;

                        ucrWrkf.FillWorkFlowDetails();
                        btnSaveSubmit.Visible = false;

                        if (CurrPK > 0)
                        {

                        }
                    }
                    else
                    {
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.FillWorkFlowDetails();
                    }
                    #endregion

                    GetFieldValues(ControlsEnum.FINYEAR);
                    SetFieldValues(ControlsEnum.FINYEAR);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);

                    if (CurrPK == 0)
                    {
                        lblTransNo.Text = Resources.Messages.DocGenerationNew;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        #endregion

        #region WorkFlow Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                ProcessID = ucrWrkf.ProcessID;
                ucrWrkf.FillWorkFlowDetails();
            }
            return ProcessID;
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
                int retRefID;
                int? result;
                bool IsChecked = false;
                string transNo = string.Empty;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                WorkflowCore.CoreService workflowCore;
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlFinYear")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                switch (commonActions)
                {
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;

                        FillProcessID();
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();

                        lblTransNo.Text = Resources.Messages.DocGenerationNew;
                        txtTransDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        GetFieldValues(ControlsEnum.FINYEAR);
                        SetFieldValues(ControlsEnum.FINYEAR);
                        GetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        SetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        break;
                    #endregion
                    #region EDIT
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdOpeningList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCOH_PK") as HiddenField).Value);
                                break;
                            }
                        }

                        if (!IsChecked)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }

                        EntryStatus = EntryStatus.EDITMODE;
                        //CurrPK = int.Parse(((ImageButton)sender).CommandArgument.ToString());

                        FillProcessID();
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        ucrWrkf.ViewAction();

                        GetFieldValues(ControlsEnum.FINYEAR);
                        SetFieldValues(ControlsEnum.FINYEAR);
                        GetFieldValues(ControlsEnum.VIEWITEM);
                        GetUIValuesFromObject(ControlsEnum.VIEWITEM);
                        GetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        SetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        break;
                    #endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdOpeningList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCOH_PK") as HiddenField).Value);
                                break;
                            }
                        }

                        if (!IsChecked)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }

                        EntryStatus = EntryStatus.VIEWMODE;
                        //CurrPK = int.Parse(((ImageButton)sender).CommandArgument.ToString());
                        //FillProcessID();
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();
                        }
                        GetFieldValues(ControlsEnum.FINYEAR);
                        SetFieldValues(ControlsEnum.FINYEAR);
                        GetFieldValues(ControlsEnum.VIEWITEM);
                        GetUIValuesFromObject(ControlsEnum.VIEWITEM);
                        SetFieldValues(ControlsEnum.VIEWITEM);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Finance.CommSetupBL.DeleteFinYearOpening(CurrPK, currentUser.PKUser);
                            if (result > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Delete_Success").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                ResetForm(ControlsEnum.CHARTOFACCOUNT);
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.DEFAULT);
                                SetFieldValues(ControlsEnum.DEFAULT);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("HigherFYExists").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.REFNOEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("FinYearLocked").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.CHARTOFACCOUNT);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //GetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        //SetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        finYearOpening = new COAFinYearOpening();
                        BindGrid(ControlsEnum.CHARTOFACCOUNT);
                        break;
                    case ActionsEnum.LOADFROMTEMPLATE:
                        GetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        SetFieldValues(ControlsEnum.CHARTOFACCOUNT);
                        break;
                    case ActionsEnum.SHOWDETAILS:
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        //CurrPK = Convert.ToInt32((gvr.FindControl("hdfCOH_PK") as HiddenField).Value);
                        int status = Convert.ToInt32((gvr.FindControl("hdfStatus") as HiddenField).Value);
                        if (status == 2)
                        {
                            btnEdit.Visible = true;
                            btnView.Visible = true;
                        }
                        else
                        {
                            btnEdit.Visible = true;
                            btnView.Visible = true;
                        }
                        break;
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (finYearOpening != null && finYearOpening.COAFinYearOpeningDetails != null)
                            {
                                finYearOpening.WKF_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                finYearOpening.USER_PK = currentUser.PKUser;
                                finYearOpening.TransDate = Convert.ToDateTime(txtTransDate.Text);
                                string xmlDoc = CommonFunctions.XmlSerialize<COAFinYearOpening>(finYearOpening);
                                // save Process Control inspection details
                                result = BusinessLogic.Finance.CommSetupBL.SaveCOAFinYearOpeninBalanceWkf(xmlDoc, out retRefID, out transNo);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    CurrPK = (int)result;
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_DraftSave_Success").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                    ResetForm(ControlsEnum.CHARTOFACCOUNT);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.DEFAULT);
                                    SetFieldValues(ControlsEnum.DEFAULT);
                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        ResetForm(ControlsEnum.CHARTOFACCOUNT);
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.DATEOVERLAP || result == (int)DbSaveStatus.CODEEXIST) //ALREADYEXIST
                                    {
                                        ResetForm(ControlsEnum.CHARTOFACCOUNT);
                                        litErrorMsg.Text = GetLocalResourceObject("AlreadyExists").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("HigherFYExists").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("FinYearLocked").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (finYearOpening != null && finYearOpening.COAFinYearOpeningDetails == null || finYearOpening.COAFinYearOpeningDetails.Count == 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("COANotFound").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        else
                        {
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion
                    #region WRKFSUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            finYearOpening.WKF_FLAG = 1;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                SaveTransaction(finYearOpening, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT), sender);
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                        }
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

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                string pageUrl = string.Empty;
                string xmlResult = string.Empty;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        GridPrams gridParamObj = new GridPrams();
                        gridParamObj.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        gridParamObj.PageSize = grdOpeningList.PageSize;
                        gridParamObj.SortBy = SortBy = SortBy == null ? GetLocalResourceObject("SortBy").ToString() : SortBy;//"COH_DATE"
                        gridParamObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortDescending : SortDirection;
                        int TransPK = Convert.ToInt32(hdfTransNoSearch.Value);
                        string TransDate = txtTranDateSearch.Text;
                        int finYearPk = Convert.ToInt32(ddlYearSearch.SelectedValue);

                        DataSet dsResult = new DataSet();
                        dsResult = BusinessLogic.Finance.CommSetupBL.GetCOAOpeningBalanceList(currentUser.SBUID, finYearPk, TransPK, TransDate, gridParamObj);
                        gridParamObj.TotalRecords = Convert.ToInt32(dsResult.Tables[0].Rows.Count > 0 ? dsResult.Tables[0].Rows[0][0] : 0);
                        TotalPages = (gridParamObj.TotalRecords == 0) ? 1 : (gridParamObj.TotalRecords <= gridParamObj.PageSize) ? 1 : (gridParamObj.TotalRecords % gridParamObj.PageSize) == 0 ? (gridParamObj.TotalRecords / gridParamObj.PageSize) : (gridParamObj.TotalRecords / gridParamObj.PageSize) + 1;
                        dtResult = dsResult.Tables[1];
                        break;
                    case ControlsEnum.FINYEAR:
                        dtResult = BusinessLogic.Finance.ClosingStockBL.GetFinYear("%", currentUser.SBUID);
                        break;
                    case ControlsEnum.CHARTOFACCOUNT:
                        xmlResult = BusinessLogic.Finance.CommSetupBL.GetCOAOpeningBalance(Convert.ToInt32(ddlFinYear.SelectedValue), currentUser.SBUID);
                        finYearOpening = CommonFunctions.XmlDeserialize<COAFinYearOpening>(xmlResult);
                        break;
                    case ControlsEnum.VIEWITEM:
                        xmlResult = BusinessLogic.Finance.CommSetupBL.GetCOAOpeningBalanceByPK(CurrPK);
                        finYearOpening = CommonFunctions.XmlDeserialize<COAFinYearOpening>(xmlResult);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    case ControlsEnum.FINYEAR:
                        BindDropDownList(controlType);
                        break;
                    case ControlsEnum.CHARTOFACCOUNT:
                    case ControlsEnum.VIEWITEM:
                        BindGrid(controlType);
                        break;
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

        #region Helper Metods
        private void SaveTransaction(COAFinYearOpening objFinYearOpening, int workflowFlag, object sender)
        {
            try
            {
                int retRefID;
                int? result = 0;
                string transNo = string.Empty;
                string savePath = string.Empty;
                WorkflowDetails wkfDetails = null;
                string TrxNo = string.Empty;
                string action = string.Empty;
                if (objFinYearOpening == null)
                    objFinYearOpening = new COAFinYearOpening();

                #region New workflow Submition
                wkfDetails = ucrWrkf.GetWorkflowDetails();
                objFinYearOpening.USER_PK = wkfDetails.UserPK;
                objFinYearOpening.WKF_APPLICATION = CurrPK;
                objFinYearOpening.WKF_COMMENTS = wkfDetails.Comments;
                objFinYearOpening.WKF_TRX_FLAG = workflowFlag;
                objFinYearOpening.WKF_PROCESS = wkfDetails.ProcessID;
                objFinYearOpening.WKF_REFERENCE = wkfDetails.ReferenceID;
                objFinYearOpening.WKF_TASK = wkfDetails.TaskID;
                objFinYearOpening.WKF_TASK_ACTION = wkfDetails.ActionID;
                action = wkfDetails.ActionText;
                #endregion

                objFinYearOpening.TransDate = Convert.ToDateTime(txtTransDate.Text);
                string xmlDoc = CommonFunctions.XmlSerialize<COAFinYearOpening>(objFinYearOpening);
                result = BusinessLogic.Finance.CommSetupBL.SaveCOAFinYearOpeninBalanceWkf(xmlDoc, out retRefID, out transNo);
                if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                {
                    CurrPK = (int)result;
                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_Submit_Success").ToString(), transNo);

                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ControlsEnum.CHARTOFACCOUNT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm(ControlsEnum.CHARTOFACCOUNT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                    }
                    #endregion
                    ucrWrkf.ApplicationID = result.Value;
                }
                else
                {
                    if (result == (int)DbSaveStatus.SQLERROR)
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.DATEOVERLAP || result == (int)DbSaveStatus.CODEEXIST) //ALREADYEXIST
                    {
                        litErrorMsg.Text = GetLocalResourceObject("AlreadyExists").ToString(); //Resources.PageNameRes.StockClosing + " " + Resources.Messages.AlreadyExists;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("HigherFYExists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("FinYearLocked").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.VIEWITEM:
                        lblTransNo.Text = finYearOpening.TransNo;
                        ddlFinYear.SelectedValue = finYearOpening.FinYearPK.ToString();
                        txtTransDate.Text = finYearOpening.TransDate.ToString(Resources.ErpRes.DateFormat);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CHARTOFACCOUNT:
                    CurrPK = 0;
                    txtTransDate.Text = string.Empty;
                    lblTransNo.Text = string.Empty;
                    break;
                case ControlsEnum.CLEAR:
                    txtTranDateSearch.Text = string.Empty;
                    txtTransNoSearch.Text = string.Empty;
                    ddlYearSearch.SelectedIndex = 0;
                    break;
            }
        }

        private void SetViewMode(EntryStatus status)
        {
            try
            {
                switch (status)
                {
                    case EntryStatus.VIEWMODE:
                        btnSaveSubmit.Visible = false;
                        btnSave.Visible = false;
                        ddlFinYear.Enabled = false;
                        btnDelete.Visible = true;
                        break;
                    case EntryStatus.NEWMODE:
                        btnSaveSubmit.Visible = true;
                        btnSave.Visible = false;
                        ddlFinYear.Enabled = true;
                        btnDelete.Visible = false;
                        break;
                    case EntryStatus.EDITMODE:
                        btnSaveSubmit.Visible = true;
                        btnSave.Visible = true;
                        ddlFinYear.Enabled = false;
                        btnDelete.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.CHARTOFACCOUNT:
                    case ControlsEnum.VIEWITEM:
                        grdCOA.DataSource = finYearOpening.COAFinYearOpeningDetails;
                        grdCOA.DataBind();
                        break;
                    case ControlsEnum.DEFAULT:
                        TotalPages = TotalPages;
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = PageIndex == null ? "1" : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdOpeningList.DataSource = dtResult;
                        grdOpeningList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.FINYEAR:
                        ddlFinYear.Items.Clear();
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlFinYear.DataValueField = "FYR_PK";
                            ddlFinYear.DataTextField = "FYR_NAME";
                            ddlFinYear.DataSource = dtResult;
                            ddlFinYear.DataBind();
                        }
                        ddlFinYear.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));

                        ddlYearSearch.Items.Clear();
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            ddlYearSearch.DataValueField = "FYR_PK";
                            ddlYearSearch.DataTextField = "FYR_NAME";
                            ddlYearSearch.DataSource = dtResult;
                            ddlYearSearch.DataBind();
                        }
                        ddlYearSearch.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetFormattedNumberWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithComma.Value);
        }
        #endregion

        public enum ControlsEnum
        {
            CHARTOFACCOUNT,
            FINYEAR,
            OPENINGLIST,
            DEFAULT,
            VIEWITEM,
            CLEAR
        }
    }
}