using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.HRMS.Payroll;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities.HRMS;
using System.Configuration;
using System.IO;
using System.Text;
using System.Data.OleDb;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class ExtraDays : ERP.Store.UI.MyBasePage
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
        /// <summary>
        /// To keep Leave Details List in view state
        /// </summary>
        private List<ExtraDaysDetails> ExtraDaysDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.ExtraDaysDetailsList] == null ? new List<ExtraDaysDetails>() : (List<ExtraDaysDetails>)ViewState[ViewstateStrings.ExtraDaysDetailsList];
            }
            set
            {
                ViewState[ViewstateStrings.ExtraDaysDetailsList] = value;
            }
        }
        /// <summary>
        /// To keep RowIndex in view state
        /// </summary>
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
        #endregion

        private BusinessObject.User currentUser;
        private DataTable dtCompany;
        private DataTable dtPageData;
        private int CompanyPk = 0;
        private ExtraDaysHeader objExtraDaysHdr;
        private string uploadPath;
        private StringBuilder sb;
        private string xmlLanding;
        private string[] excelColumns;
        private string[] airColums_General = { Resources.Constants.ExlExtraDaysLocCol, Resources.Constants.ExlExtraDaysDateCol,Resources.Constants.ExlExtraDaysEmpCodeCol,
                                              Resources.Constants.ExlExtraDaysEmpNameCol,Resources.Constants.ExlExtraDaysDayCol};
        private FileInfo attchInfo;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private DataTable dtExcelSchema;
        private DataSet dsImportedExtraDays;
        private string landingSheet;
        //int currPK;
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
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            hdfCurrentPk.Value = CurrPK.ToString();
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (!IsPostBack)
                {
                    uclPaging.CurrentPage = 1;                  
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void ConfigurationSettings()
        {
            
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
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result;
            XmlDocument xmlDoc;
            List<ExtraDaysDetails> tempList = new List<ExtraDaysDetails>();
            bool bIsChecked = false;
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
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        tempList = ExtraDaysDetailsList;
                        if (RowIndex >= 0)
                        {
                            DateTime date = DateTime.Parse(txtDate.Text.Trim());
                            List<ExtraDaysDetails> objExtraDaysList = tempList.DeepClone();
                            ExtraDaysDetails objExtraDays = tempList[RowIndex];
                            int employeeID = GetNullableInt(hdfEmployee.Value).Value;
                            string employeeText = txtEmployee.Text.Trim();
                            objExtraDaysList.Remove(objExtraDaysList[RowIndex]);
                            if (objExtraDaysList.Where(x => x.EXD_EMPLOYEE == Convert.ToInt32(hdfEmployee.Value)
                                && x.EXD_DATE == date).Count() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                       CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RecordExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            objExtraDays.EXD_EMPLOYEE = employeeID;
                            objExtraDays.EXD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(employeeText);
                            objExtraDays.EXD_DAYS = Convert.ToDouble(txtExtraDays.Text.Trim());
                            objExtraDays.EXD_REMARK = HttpUtility.HtmlEncode(txtRemarks.Text);
                            objExtraDays.EXD_DATE = DateTime.Parse(txtDate.Text);
                        }
                        else
                        {
                            if (tempList == null)
                                tempList = new List<ExtraDaysDetails>();
                            hdfIscontYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                            ExtraDaysDetails leaveDataObj = new ExtraDaysDetails();
                            int employeeID = GetNullableInt(hdfEmployee.Value).Value;
                            string employeeText = txtEmployee.Text.Trim();
                            DateTime date = DateTime.Parse(txtDate.Text.Trim());
                            if (tempList.Count > 0)
                            {
                                if (tempList.Where(x => x.EXD_EMPLOYEE == Convert.ToInt32(hdfEmployee.Value)
                                    && x.EXD_DATE == date).Count() > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                           CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RecordExist").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            leaveDataObj.EXD_EMPLOYEE = employeeID;
                            leaveDataObj.EXD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(employeeText);
                            leaveDataObj.EXD_PK = 0;
                            leaveDataObj.EXD_DAYS = Convert.ToDouble(txtExtraDays.Text.Trim());
                            leaveDataObj.EXD_REMARK = HttpUtility.HtmlEncode(txtRemarks.Text);
                            leaveDataObj.EXD_DATE = DateTime.Parse(txtDate.Text);
                            tempList.Add(leaveDataObj);
                        }
                        ExtraDaysDetailsList = tempList;
                        ResetForm(ControlsEnum.ADDTOLIST);
                        SetFieldValues(ControlsEnum.EXTRADAYSDETAILSLIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        objExtraDaysHdr = (ExtraDaysHeader)SetUIValuesToObject(ControlsEnum.EXTRADAYSHEADER);
                        if (objExtraDaysHdr.ExtraDaysDetails.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoEmployeeForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        string TrxNo = string.Empty;
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objExtraDaysHdr);
                        result = BusinessLogic.HRMS.Payroll.AttendanceBL.SaveExtraDays(xmlDoc.InnerXml, out TrxNo);
                        if (result > 0)
                        {
                            lblTrxNo.Text = TrxNo;
                            ResetForm(ControlsEnum.CLEAR);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                            object[] args = new object[2];
                            args[0] = Resources.PageNameRes.ExtraDays;
                            args[1] = TrxNo;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
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
                                litErrorMsg.Text = Resources.PageNameRes.ExtraDays + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ExtraDays + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ExtraDays + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExtraDays);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        string monthYear = string.Empty;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpLeavePk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.EXTRADAYSBYPK);
                            SetFieldValues(ControlsEnum.EXTRADAYSBYPK);
                            SetFieldValues(ControlsEnum.EXTRADAYSDETAILSLIST);
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
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.EXTRADAYSDETAILSLIST);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Payroll.AttendanceBL.DeleteExtraDays(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExtraDays);
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(uclPaging.CurrentPage) > 1)
                            {
                                uclPaging.CurrentPage = Convert.ToInt32(uclPaging.CurrentPage) - 1;
                            }
                            ResetForm(ControlsEnum.CLEAR);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ExtraDays + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ExtraDays + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.ExtraDays + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExtraDays);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CLEAR ADD TO LIST
                    case ActionsEnum.CLEARADDTOLIST:
                        ResetForm(ControlsEnum.ADDTOLIST);
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region SAVEIMPORT
                    case ActionsEnum.SAVEIMPORT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (fupImport.HasFile)
                            {
                                string conStr;
                                string filePath = SaveDetails(out conStr, fupImport);
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    ImportToGrid(filePath, conStr);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InvalidFile").ToString())
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = this.GetLocalResourceObject("AttachFile").ToString();
                                Page.ClientScript.RegisterStartupScript(typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            }
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
        #endregion

        #region --- For Grid Actions----
        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdExtraDaysList")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    #region EDIT_ACTION
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    ExtraDaysDetails leaveData = ExtraDaysDetailsList[row.RowIndex];
                    RowIndex = row.RowIndex;
                    hdfEmployee.Value = leaveData.EXD_EMPLOYEE.ToString();
                    txtEmployee.Text =  HttpUtility.HtmlDecode(leaveData.EXD_EMPLOYEE_TEXT.ToString());
                    txtExtraDays.Text = GetFormattedNumber(leaveData.EXD_DAYS);                    
                    txtDate.Text = leaveData.EXD_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtRemarks.Text = HttpUtility.HtmlDecode(leaveData.EXD_REMARK);
                    SetFieldValues(ControlsEnum.EXTRADAYSDETAILSLIST); 
                    #endregion
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    #region DELETE_ACTION
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    List<ExtraDaysDetails> tempList = ExtraDaysDetailsList;
                    tempList.Remove(tempList[row.RowIndex]);
                    ExtraDaysDetailsList = tempList;
                    ResetForm(ControlsEnum.ADDTOLIST);
                    SetFieldValues(ControlsEnum.EXTRADAYSDETAILSLIST); 
                    #endregion
                }
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
            int brLoc = 0;
            try
            {
                FilterParameters objFilterParam;
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        objFilterParam = new FilterParameters();
                        int.TryParse(hdfFilterBranch.Value, out brLoc);
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtFilterToDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterToDate.Text);
                        objFilterParam.BranchLocation = brLoc > 0 ? brLoc : (int?)null;
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.BizUnit = currentUser.SBUID;
                        dtPageData = BusinessLogic.HRMS.Payroll.AttendanceBL.GetExtraDaysList(objFilterParam);
                        break;
                    #endregion                   
                    #region EXTRA DAYS BY PK
                    case ControlsEnum.EXTRADAYSBYPK:
                        objFilterParam = new FilterParameters();
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = (int)DbActiveStatus.HASPK;
                        objExtraDaysHdr = BusinessLogic.HRMS.Payroll.AttendanceBL.GetExtraDaysByPK(objFilterParam);
                        if (objExtraDaysHdr != null)
                            ExtraDaysDetailsList = objExtraDaysHdr.ExtraDaysDetails;
                        break;
                    #endregion                   
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
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
                    #region EXTRADAYSDETAILSLIST
                    case ControlsEnum.EXTRADAYSDETAILSLIST:
                        BindGrid(ControlsEnum.EXTRADAYSDETAILSLIST);
                        break;
                    #endregion

                    #region SALARYPKLIST
                    case ControlsEnum.SALARYPKLIST:
                        BindDropDown(ControlsEnum.SALARYPKLIST);
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    
                    #region EXTRADAYSBYPK
                    case ControlsEnum.EXTRADAYSBYPK:
                        GetUIValuesFromObject(ControlsEnum.EXTRADAYSBYPK);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
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

        #region Sets the UI input controls from the object values
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {  
                    #region EXTRADAYSBYPK
                    case ControlsEnum.EXTRADAYSBYPK:
                        if (objExtraDaysHdr != null)
                        {
                            txtDateHd.Text = objExtraDaysHdr.EXH_SAL_MONTH.ToString(Resources.Constants.HRMSDateFormatShort);
                            lblTrxNo.Text = string.IsNullOrEmpty(objExtraDaysHdr.EXH_NO) ? Resources.ErpRes.Draft : objExtraDaysHdr.EXH_NO;
                            txtBrLoc.Text = HttpUtility.HtmlDecode(objExtraDaysHdr.EXH_BRANCH_TEXT);
                            hdfBrLoc.Value = objExtraDaysHdr.EXH_BRANCH.ToString();
                            txtDescription.Text = HttpUtility.HtmlDecode(objExtraDaysHdr.EXH_DESC);
                            LastModifiedTime = objExtraDaysHdr.LAST_MOD_DT;
                            CompanyPk = objExtraDaysHdr.EXH_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                            ExtraDaysDetailsList = objExtraDaysHdr.ExtraDaysDetails;
                            SetFieldValues(ControlsEnum.EXTRADAYSDETAILSLIST);
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

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();

                    ddlCompanyImpt.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompanyImpt.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompanyImpt.DataSource = dtCompany;
                    ddlCompanyImpt.DataBind();
                    ddlCompanyImpt.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompanyImpt.Items.HtmlDecode();

                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                        ddlCompanyImpt.SelectedIndex = ddlCompanyImpt.Items.IndexOf(ddlCompanyImpt.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                    }
                    break;
                #endregion

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
                    #region EXTRADAYSDETAILSLIST
                    case ControlsEnum.EXTRADAYSDETAILSLIST:
                        if (ExtraDaysDetailsList != null && ExtraDaysDetailsList.Count > 0)
                            grdExtraDaysList.DataSource = ExtraDaysDetailsList;
                        else
                            grdExtraDaysList.DataSource = null;
                        grdExtraDaysList.DataBind();
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dtPageData.Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dtPageData.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdList.DataSource = dtPageData;
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
                #region EXTRADAYSHEADER
                case ControlsEnum.EXTRADAYSHEADER:
                    ExtraDaysHeader objExDaysHdr = new ExtraDaysHeader();
                    objExDaysHdr.EXH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                    objExDaysHdr.USER_PK = currentUser.PKUser;
                    objExDaysHdr.EXH_NO = lblTrxNo.Text;
                    objExDaysHdr.BIZUNIT = currentUser.SBUID;
                    objExDaysHdr.EXH_DEPT = currentUser.CurrentDeptPK;
                    objExDaysHdr.EXH_BRANCH = Convert.ToInt32(hdfBrLoc.Value);
                    objExDaysHdr.EXH_SAL_MONTH = DateTime.Parse(txtDateHd.Text);
                    objExDaysHdr.EXH_PK = CurrPK;
                    objExDaysHdr.EXH_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    objExDaysHdr.EXH_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                    objExDaysHdr.LAST_MOD_DT = LastModifiedTime;
                    objExDaysHdr.ExtraDaysDetails = ExtraDaysDetailsList;
                    returnObject = objExDaysHdr;
                    break;
                #endregion
            }
            return returnObject;
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
                    RowIndex = -1;                   
                    txtDate.Text = string.Empty;
                    txtDateHd.Text = string.Empty;
                    txtBrLoc.Text = string.Empty;
                    hdfBrLoc.Value = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;                  
                    txtExtraDays.Text = string.Empty;                   
                    txtDescription.Text = string.Empty;
                    txtImportDate.Text = string.Empty;
                    ExtraDaysDetailsList = null;
                    uclPaging.CurrentPage = 0;
                    lblTrxNo.Text = Resources.ErpRes.Draft; 
                    break;
                #endregion
                #region ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    RowIndex = -1;
                    txtExtraDays.Text = GetLocalResourceObject("DefaultExtraDays").ToString();                 
                    txtRemarks.Text = string.Empty;                    
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtDate.Text = string.Empty;
                    break;
                #endregion                
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    txtFilterBranch.Text = string.Empty;
                    hdfFilterBranch.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtFilterFromDate.Text = string.Empty;
                    txtFilterToDate.Text = string.Empty;
                    txtImportDate.Text = string.Empty;
                    break;
                #endregion

                #region IMPORT
                case ControlsEnum.IMPORT:
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    txtImportDate.Text = string.Empty;
                    CurrPK = 0;
                    break;
                #endregion
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
            string format = "#0.00";
            string s = num.ToString(format);
            return s;
        }

        public string GetSubstring(object str)
        {
            return ((string)str).Substring(0, 3);
        }
        #endregion

        #region Helper Methods

        #region Excel Import

        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr)
        {
            try
            {
                string SheetName = Resources.Constants.ExlExtraDaysSheetName;
                excelColumns = airColums_General;
                conStr = String.Format(conStr, filePath);
                connExcel = new OleDbConnection(conStr);
                cmdExcel = new OleDbCommand();
                oleDbDataAdapter = new OleDbDataAdapter();
                dtExcelSchema = new DataTable();
                dsImportedExtraDays = new DataSet();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                {
                    if (dtExcelSchema != null)
                    {
                        landingSheet = string.Empty;
                        foreach (DataRow dr in dtExcelSchema.Rows)
                        {
                            if (dr["TABLE_NAME"].ToString().ToLower() == SheetName.ToLower())
                            {
                                landingSheet = dr["TABLE_NAME"].ToString();
                            }
                        }
                        if (landingSheet == string.Empty)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_ExelsheetName").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, SheetName.Replace("$", ""));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                    }
                    cmdExcel.CommandText = "SELECT * From [" + landingSheet + "]";
                    oleDbDataAdapter.SelectCommand = cmdExcel;
                    oleDbDataAdapter.Fill(dsImportedExtraDays, "Landing");
                    connExcel.Close();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    throw new Exception(GetLocalResourceObject("Err_IncorrectFormat").ToString());
                }
                if (dsImportedExtraDays != null && dsImportedExtraDays.Tables.Count > 0)
                {
                    foreach (DataColumn item in dsImportedExtraDays.Tables[0].Columns)
                    {
                        item.ColumnName = item.ColumnName.Replace(" ", "");
                    }
                    int cnt = (from p in excelColumns
                               where this.dsImportedExtraDays.Tables[0].Columns.Contains(p)
                               select p).Count();
                    if (cnt != excelColumns.Length)
                    {
                        sb = new StringBuilder();
                        sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                        if (excelColumns != null && excelColumns.Count() > 0)
                        {
                            foreach (string item in excelColumns)
                            {
                                sb.Append("<ul><li>" + item + "</li></ul>");
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                           + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }

                    DataTable dtImportData = dsImportedExtraDays.Tables[0];
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    ExtraDaysHeader objExDaysHdr = new ExtraDaysHeader();
                    objExDaysHdr.EXH_COMPANY = Convert.ToInt32(ddlCompanyImpt.SelectedValue);
                    objExDaysHdr.USER_PK = currentUser.PKUser;
                    objExDaysHdr.BIZUNIT = currentUser.SBUID;
                    objExDaysHdr.EXH_DEPT = currentUser.CurrentDeptPK;
                    objExDaysHdr.EXH_SAL_MONTH = DateTime.Parse(txtImportDate.Text);
                    objExDaysHdr.EXH_PK = CurrPK;
                    objExDaysHdr.EXH_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    objExDaysHdr.LAST_MOD_DT = LastModifiedTime;

                    List<ExtraDaysDetails> ExtraDaysimportDet = new List<ExtraDaysDetails>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        DateTime attnDate = new DateTime();
                        string Location = string.Empty;
                        string Date = string.Empty;
                        string EmpCode = string.Empty;
                        string EmpName = string.Empty;
                        string ExtDays = string.Empty;
                        Location = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlExtraDaysLocCol]);
                        Date = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlExtraDaysDateCol]).Trim();
                        EmpCode = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlExtraDaysEmpCodeCol]);
                        EmpName = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlExtraDaysEmpNameCol]).Trim();
                        ExtDays = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlExtraDaysDayCol]);
                        if (!string.IsNullOrEmpty(Location.Trim()) && !string.IsNullOrEmpty(Date.Trim()) && !string.IsNullOrEmpty(EmpCode.Trim()) && !string.IsNullOrEmpty(ExtDays.Trim()))
                        {
                            DateTime.TryParse(txtImportDate.Text, out attnDate);
                            ExtraDaysDetails objExtraDaysDetail = new ExtraDaysDetails();
                            objExtraDaysDetail.EXD_EMPLOYEE_TEXT = EmpName.HtmlEncode();
                            objExtraDaysDetail.EXD_DAYS = Convert.ToDouble(ExtDays.Trim());
                            objExtraDaysDetail.EXD_DATE = DateTime.Parse(Date);
                            objExtraDaysDetail.EXD_BRANCH = Location.HtmlEncode();
                            objExtraDaysDetail.EXD_EMP_CODE = EmpCode.HtmlEncode();
                            ExtraDaysimportDet.Add(objExtraDaysDetail);
                        }
                    }
                    objExDaysHdr.ExtraDaysDetails = ExtraDaysimportDet;// dt;

                    if (objExDaysHdr.ExtraDaysDetails != null && objExDaysHdr.ExtraDaysDetails.Count > 0)
                    {
                        string xmlDoc = CommonFunctions.XmlSerialize<ExtraDaysHeader>(objExDaysHdr);
                        int? result = BusinessLogic.HRMS.Payroll.AttendanceBL.ImportExtraDays(xmlDoc);
                        if (result > 0 && dsImportedExtraDays.Tables.Count > 0)
                        {
                            ResetForm(ControlsEnum.IMPORT);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = GetLocalResourceObject("ExtraDaysImported").ToString();
                            object[] args = new object[2];
                            args[0] = Resources.PageNameRes.ExtraDays;
                           // args[1] = TrxNo;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.SQLERROR)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_IncorrectFile").ToString(); //Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.PENDINGEXIST) // Invalid location
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_InvalidLocation").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForImport").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    }
                }
            }
            catch (OleDbException ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);

            }
        }
        #endregion
        #region SaveDetails

        /// <summary>
        /// Methode used to save the excel file
        /// </summary>
        private string SaveDetails(out string conStr, FileUpload fupUpload)
        {

            //uploadInfo = new FileInfo(fupImport.PostedFile.FileName);
            //extns = uploadInfo.Extension;
            uploadPath = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\ExtraDays";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\ExtraDays\\";
            }
            else
            {
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "ExtraDays";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "ExtraDays\\";
            }

            FileInfo tempFileInfoObj;
            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
            string attachmentFileFormat = tempFileInfoObj.Extension;

            string FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
            conStr = CheckValidFileType(attachmentFileFormat);

            if (attachmentFileFormat.ToLower() != ".xls" && attachmentFileFormat.ToLower() != ".xlsx")
            {
                return string.Empty;
            }
            if ((!string.IsNullOrEmpty(conStr)) || (attachmentFileFormat.ToLower() == ".txt"))
            {
                fupUpload.SaveAs(uploadPath + FileName);
            }

            return uploadPath + FileName;

        }
        #endregion
        #region CheckValidFileType
        /// <summary>
        /// Check File is valid or not , using File Extension , if valid then get the connection string
        /// </summary>
        /// <param name="extn"></param>
        /// <returns>bool : True - valid File, false - Invalid File</returns>
        private string CheckValidFileType(string extn)
        {
            string conStr;
            switch (extn.ToLower())
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.AppSettings["Excel03ConString"];
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.AppSettings["Excel07ConString"];
                    break;
                default: conStr = string.Empty; break;
            }
            return conStr;
        }
        #endregion
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEES,           
            COMPANY,
            EXTRADAYSDETAILSLIST,
            ADDTOLIST,           
            EXTRADAYSHEADER,
            CLEAR,
            SALARYPKLIST,
            LIST,
            CLEARSEARCH,                
            EXTRADAYSBYPK,
            IMPORT
        }
        #endregion
    }
}