using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using System.Threading;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using ERP.Utilities.HRMS;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmployeeTraining : ERP.Store.UI.MyBasePage //: System.Web.UI.Page // 
    {
        #region Variables and Properties
        #region  Properties
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
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
        private List<BusinessObject.HRMS.Employee.EmployeeTrainingDetails> EmployeeTrainingDetailList
        {
            get
            {
                return (List<BusinessObject.HRMS.Employee.EmployeeTrainingDetails>)ViewState["EmployeeTrainingDetails"];
            }
            set
            {
                ViewState["EmployeeTrainingDetails"] = value;
            }
        }
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        #endregion
        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        DataTable dtCompany;
        private BusinessObject.HRMS.Employee.EmployeeTrainingHeader objEmployeeTrainingHeader;
        private BusinessObject.HRMS.Employee.EmployeeTrainingHeader_PopUp objEmployeeTrainingHeaderPopUp;
        private List<BusinessObject.HRMS.Employee.EmployeeTraining_PopUP> EmployeeTraininge_PopUPList;
        private int CompanyPk = 0;
        #endregion
        #endregion

        #region PageLevel Events
        #region Page Load
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            InitializeComponent();
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
         #endregion
        #region Page Init
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
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
                PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                    hdfCurrencyFormatWithComma.Value += "0";
                }
                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.COMPANY);
                SetFieldValues(ControlsEnum.COMPANY);
                GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                GetFieldValues(ControlsEnum.COMPANY);
                SetFieldValues(ControlsEnum.COMPANY);
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion
        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
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
                    PageIndex = uclPaging.CurrentPage;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            //  EntryStatus = EntryStatus.LISTMODE;
        }
        #endregion
         #endregion

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                int? result;
                bool bIsChecked = false;
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                }
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {

                            objEmployeeTrainingHeader = new BusinessObject.HRMS.Employee.EmployeeTrainingHeader();
                            objEmployeeTrainingHeader = (BusinessObject.HRMS.Employee.EmployeeTrainingHeader)SetUIValuesToObject(ControlsEnum.EMPTRAININGHDR);
                            if (objEmployeeTrainingHeader != null)
                            {
                                if (objEmployeeTrainingHeader.EmployeeTrainingDtl != null && objEmployeeTrainingHeader.EmployeeTrainingDtl.Count > 0)
                                {
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<BusinessObject.HRMS.Employee.EmployeeTrainingHeader>(objEmployeeTrainingHeader);
                                    //result = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.SaveEmployeeTrainingDetails(xmlDoc, out trxNo);
                                    result = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.SaveEmployeeTrainingDetails(xmlDoc, out trxNo);
                                    if (result > 0)
                                    {
                                        lblTrxNo.Text = trxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.EmployeeTraining;
                                        args[1] = trxNo;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTraining);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = (GetLocalResourceObject("Err_NoEmployeeForSave")).ToString(); 
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.TRAININGDETAILLIST);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region DETAIL
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSH_PKListPage")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.EMPLOYEETRAININGDETAILS);
                            SetFieldValues(ControlsEnum.EMPLOYEETRAININGDETAILS);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.DeleteEmployeeTraining(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTraining);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeTraining + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeTraining);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter").ToString() + "','860','485');", true);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion 
                    #region CLEAR DETAIL
                    case ActionsEnum.CLEARDETAIL:
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter").ToString() + "','860','485');", true);
                        break;
                    #endregion
                    #region  CLOSE MENU POPUP
                    case ActionsEnum.CANCELPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region SHOWPOPUP
                    case ActionsEnum.SHOWPOPUP:
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter").ToString() + "','860','485');", true);
                        break;
                    #endregion
                    #region SAVE POPUP TO LIST
                    case ActionsEnum.SAVE_ACTIONPOPUP:
                        SetUIValuesToObject(ControlsEnum.SAVEMPLOYEEDETAILSPOPUP);
                        SetFieldValues(ControlsEnum.TRAININGDETAILLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    case ActionsEnum.PRINT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPSH_PKListPage")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&CurPK=" + CurrPK + "&APPTYPE=" + "ETR" + "&APPSUBTYPE= 0") + "&ISEXCELPRINT= 1" + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdEmpTrainingList")
            {
                if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfETL_PK = row.FindControl("hdfETL_PK") as HiddenField;
                    HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;
                    BusinessObject.HRMS.Employee.EmployeeTrainingDetails detail = EmployeeTrainingDetailList
                        .Where(x => x.ROW_NO == Convert.ToInt32(hdfROW_NO.Value) && x.ETL_PK == Convert.ToInt32(hdfETL_PK.Value))
                        .SingleOrDefault();
                    if (detail != null)
                    {
                        EmployeeTrainingDetailList.Remove(detail);
                        SetFieldValues(ControlsEnum.TRAININGDETAILLIST);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailed;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
        }
        #endregion

        #region Helper Methods
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            FilterParameters objFilterParam;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.GetEmployeeTrainingList(txtFromDateList.Text, txtToDateList.Text, txtTopicList.Text, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")));
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        int commonPK = 0;
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion
                    #region EMPLOYEE DETAILS POPUP
                    case ControlsEnum.EMPLOYEEDETAILSPOPUP:
                        objFilterParam = new FilterParameters();
                        objFilterParam.ToDate = txtToDateHd.Text != string.Empty ? Convert.ToDateTime(txtToDateHd.Text) : (DateTime?)null;
                        objFilterParam.Company = GetNullableInt(ddlCompany_Filter.SelectedValue) > 0 ? GetNullableInt(ddlCompany_Filter.SelectedValue) : null;
                        objFilterParam.BranchLocation = GetNullableInt(hdfBranchLocation_Filter.Value) > 0 ? GetNullableInt(hdfBranchLocation_Filter.Value) : null;
                        objFilterParam.Department = GetNullableInt(hdfDepartment_Filter.Value) > 0 ? GetNullableInt(hdfDepartment_Filter.Value) : null;
                        objFilterParam.EmploymentType = GetNullableInt(ddlEmploymentType_Filter.SelectedValue) > 0 ? GetNullableInt(ddlEmploymentType_Filter.SelectedValue) : null;
                        objFilterParam.Employee = GetNullableInt(hdfEmployee_Filter.Value) > 0 ? GetNullableInt(hdfEmployee_Filter.Value) : null;
                        objFilterParam.EmployeeType = GetNullableInt(ddlEmployeeType_Filter.SelectedValue) > 0 ? GetNullableInt(ddlEmployeeType_Filter.SelectedValue) : null;
                        objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = Convert.ToInt32(CommonConstants.ACTIVE);
                        objFilterParam.BizUnit = currentUser.SBUID;

                        objEmployeeTrainingHeaderPopUp = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.GetEmployeeTrainingDetailsPopUp(objFilterParam);
                        if (objEmployeeTrainingHeaderPopUp != null)
                            EmployeeTraininge_PopUPList = objEmployeeTrainingHeaderPopUp.EmployeeTraining_PopUPDtl;
                        break;
                    #endregion
                    #region GET EMPLOYEE TRAINING DETAILS
                    case ControlsEnum.EMPLOYEETRAININGDETAILS:
                        objEmployeeTrainingHeader = BusinessLogic.HRMS.Employee.EmployeeTrainingBL.GetEmployeeTrainingByPK(CurrPK);
                        if (objEmployeeTrainingHeader == null && CurrPK != 0)
                          {
                              ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                          }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #region Set Field Values
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region EMPLOYEETYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        BindDropDown(ControlsEnum.EMPLOYEETYPE);
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EMPLOYEE DETAILS POPUP
                    case ControlsEnum.EMPLOYEEDETAILSPOPUP:
                        BindGrid(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        break;
                    #endregion
                    #region EMPLOYEE TRAINING DETAILS
                    case ControlsEnum.EMPLOYEETRAININGDETAILS:
                        GetUIValuesFromObject(ControlsEnum.EMPLOYEETRAININGDETAILS);
                        break;
                    #endregion
                    #region TRAINING DETAIL LIST
                    case ControlsEnum.TRAININGDETAILLIST:
                        BindGrid(ControlsEnum.TRAININGDETAILLIST);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region EMP TRAINING HDR
                    case ControlsEnum.EMPTRAININGHDR:
                        objEmployeeTrainingHeader.ETA_PK = CurrPK;
                        objEmployeeTrainingHeader.ETA_NO = lblTrxNo.Text;
                        objEmployeeTrainingHeader.ETA_DATE = DateTime.Parse(txtDateHd.Text);
                        objEmployeeTrainingHeader.ETA_FROM_DATE = string.IsNullOrEmpty(txtFromDateHd.Text) ? null : (txtFromDateHd.Text);
                        objEmployeeTrainingHeader.ETA_TO_DATE = string.IsNullOrEmpty(txtToDateHd.Text) ? null : (txtToDateHd.Text);
                        objEmployeeTrainingHeader.ETA_PLACE = txtPlaceHd.Text.HtmlEncode();
                        objEmployeeTrainingHeader.ETA_TOPIC = txtTopic.Text.HtmlEncode();
                        objEmployeeTrainingHeader.ETA_DURATION = string.IsNullOrEmpty(txtDurationHd.Text) ? 0 : ConvertTimeStringToDouble(txtDurationHd.Text);
                        objEmployeeTrainingHeader.ETA_TRAINER = txtTrainerHd.Text.HtmlEncode();
                        objEmployeeTrainingHeader.ETA_DESCRIPTION = txtDescHd.Text.HtmlEncode();
                        objEmployeeTrainingHeader.ETA_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objEmployeeTrainingHeader.BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                        //objEmployeeTrainingHeader.PSH_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objEmployeeTrainingHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objEmployeeTrainingHeader.LAST_MOD_DT = LastModifiedTime;
                        objEmployeeTrainingHeader.ETA_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                         SetUIValuesToObject(ControlsEnum.EMPTRAININGDTL);
                        objEmployeeTrainingHeader.EmployeeTrainingDtl = EmployeeTrainingDetailList;
                        retObject = objEmployeeTrainingHeader;
                        break;
                    #endregion
                    #region EMP TRAINING DTL
                    case ControlsEnum.EMPTRAININGDTL:
                        EmployeeTrainingDetailList = new List<BusinessObject.HRMS.Employee.EmployeeTrainingDetails>();
                        foreach (GridViewRow grdrow in grdEmpTrainingList.Rows)
                        {
                            BusinessObject.HRMS.Employee.EmployeeTrainingDetails objMenuMappingDetails = new BusinessObject.HRMS.Employee.EmployeeTrainingDetails();
                            objMenuMappingDetails.ROW_NO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfROW_NO")).Value);
                            objMenuMappingDetails.ETL_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfETL_PK")).Value);
                            objMenuMappingDetails.ETL_ETA_PK = CurrPK;
                            objMenuMappingDetails.ETL_EMP_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfETL_EMP_PK")).Value);
                            EmployeeTrainingDetailList.Add(objMenuMappingDetails);
                        }
                        break;
                    #endregion
                    #region SAVE EMPLOYEE DETAILS POPUP TO GRID
                    case ControlsEnum.SAVEMPLOYEEDETAILSPOPUP:
                        foreach (GridViewRow grdrow in grdEmpTraining_PopUp.Rows)
                        {
                            CheckBox chkEmpselect_PopUp = (CheckBox)grdrow.FindControl("chkEmpselect_PopUp");
                            if (chkEmpselect_PopUp.Checked == true)
                            {
                                if (EmployeeTrainingDetailList == null)
                                    EmployeeTrainingDetailList = new List<BusinessObject.HRMS.Employee.EmployeeTrainingDetails>();
                                if (EntryStatus == EntryStatus.EDITMODE)
                                {
                                    if (EmployeeTrainingDetailList.Where(r => r.ETL_EMP_PK == Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk_PopUp")).Value)).Count() > 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_RecordExist").ToString()) + "','" + Resources.Captions.Information + "');", true);
                                        break;
                                    }
                                }
                                else
                                {
                                    EmployeeTrainingDetailList.RemoveAll(x => x.ETL_PK == Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfETL_PK_PopUp")).Value)
                                           && x.ETL_EMP_PK == Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk_PopUp")).Value));
                                }
                                BusinessObject.HRMS.Employee.EmployeeTrainingDetails objEmployeeTrainingDetails = new BusinessObject.HRMS.Employee.EmployeeTrainingDetails();
                                objEmployeeTrainingDetails.ROW_NO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfROW_NO_PopUp")).Value);
                                objEmployeeTrainingDetails.ETL_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfETL_PK_PopUp")).Value);
                                objEmployeeTrainingDetails.ETL_ETA_PK = CurrPK;
                                objEmployeeTrainingDetails.ETL_EMP_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk_PopUp")).Value);
                                objEmployeeTrainingDetails.empName_txt = ((Label)grdrow.FindControl("lblempName_txt_PopUp")).ToolTip.HtmlEncode();
                                objEmployeeTrainingDetails.empBranchText = ((Label)grdrow.FindControl("lblempBranchText_PopUp")).ToolTip.HtmlEncode();
                                objEmployeeTrainingDetails.empDepartmentText = ((Label)grdrow.FindControl("lblDepartment_PopUp")).ToolTip.HtmlEncode();
                                objEmployeeTrainingDetails.empDesignationText = ((Label)grdrow.FindControl("lblDesigPopUp")).ToolTip.HtmlEncode();
                                EmployeeTrainingDetailList.Add(objEmployeeTrainingDetails);
                            }
                        }
                        break;
                    #endregion
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
        #endregion
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOYEE TRAINING DETAILS
                    case ControlsEnum.EMPLOYEETRAININGDETAILS:
                        if (objEmployeeTrainingHeader != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(objEmployeeTrainingHeader.ETA_NO) ? Resources.ErpRes.Draft : objEmployeeTrainingHeader.ETA_NO;
                            txtDateHd.Text = Convert.ToDateTime(objEmployeeTrainingHeader.ETA_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtFromDateHd.Text = string.IsNullOrEmpty(objEmployeeTrainingHeader.ETA_FROM_DATE) ? string.Empty : Convert.ToDateTime(objEmployeeTrainingHeader.ETA_FROM_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtToDateHd.Text = string.IsNullOrEmpty(objEmployeeTrainingHeader.ETA_TO_DATE) ? string.Empty : Convert.ToDateTime(objEmployeeTrainingHeader.ETA_TO_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtTopic.Text = objEmployeeTrainingHeader.ETA_TOPIC.HtmlDecode();
                            txtDurationHd.Text =GetTime(objEmployeeTrainingHeader.ETA_DURATION.ToString());
                            txtPlaceHd.Text = objEmployeeTrainingHeader.ETA_PLACE.HtmlDecode();
                            txtTrainerHd.Text = objEmployeeTrainingHeader.ETA_TRAINER.HtmlDecode();
                            txtDescHd.Text = objEmployeeTrainingHeader.ETA_DESCRIPTION.HtmlDecode(); ;
                            LastModifiedTime = objEmployeeTrainingHeader.LAST_MOD_DT;
                            CompanyPk = objEmployeeTrainingHeader.ETA_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));    
                            EmployeeTrainingDetailList = objEmployeeTrainingHeader.EmployeeTrainingDtl;
                            SetFieldValues(ControlsEnum.TRAININGDETAILLIST);
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
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany_Filter.Items.Clear();
                    ddlCompany_Filter.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany_Filter.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany_Filter.DataSource = dtCompany;
                    ddlCompany_Filter.DataBind();
                    ddlCompany_Filter.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany_Filter.Items.HtmlDecode();

                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));

                    break;
                #endregion
                #region EMPLOYMENTTYPE
                case ControlsEnum.EMPLOYMENTTYPE:
                    ddlEmploymentType_Filter.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType_Filter.DataSource = dtResult;
                        ddlEmploymentType_Filter.DataTextField = "CON_NAME";
                        ddlEmploymentType_Filter.DataValueField = "CON_PK";
                        ddlEmploymentType_Filter.DataBind();
                        ddlEmploymentType_Filter.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        ddlEmploymentType_Filter.Items.HtmlDecode();
                    }
                    break;
                #endregion
                #region EMPLOYEE TYPE
                case ControlsEnum.EMPLOYEETYPE:
                    ddlEmployeeType_Filter.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmployeeType_Filter.DataSource = dtResult;
                        ddlEmployeeType_Filter.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                        ddlEmployeeType_Filter.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                        ddlEmployeeType_Filter.DataBind();
                        ddlEmployeeType_Filter.Items.HtmlDecode();
                    }
                    ddlEmployeeType_Filter.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
                        }
                        break;
                    #endregion
                    #region TRAINING DETAIL LIST
                    case ControlsEnum.TRAININGDETAILLIST:
                        if (EmployeeTrainingDetailList != null)
                        {
                            grdEmpTrainingList.DataSource = EmployeeTrainingDetailList;
                            grdEmpTrainingList.DataBind();
                           
                        }
                        else
                        {
                            grdEmpTrainingList.DataSource = null;
                            grdEmpTrainingList.DataBind();
                        }
                        break;
                    #endregion
                    #region EMPLOYEE DETAILS POPUP
                    case ControlsEnum.EMPLOYEEDETAILSPOPUP:
                        if (EmployeeTraininge_PopUPList != null)
                        {
                            grdEmpTraining_PopUp.DataSource = EmployeeTraininge_PopUPList;
                            grdEmpTraining_PopUp.DataBind();
                        }
                        else
                        {
                            grdEmpTraining_PopUp.DataSource = null;
                            grdEmpTraining_PopUp.DataBind();
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtDateHd.Text = string.Empty;
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtTopic.Text = string.Empty;
                    txtFromDateHd.Text = string.Empty;
                    txtToDateHd.Text = string.Empty;
                    txtDurationHd.Text = string.Empty;
                    txtPlaceHd.Text = string.Empty;
                    txtTrainerHd.Text = string.Empty;
                    txtDescHd.Text = string.Empty;
                    txtFromDateList.Text = string.Empty;
                    txtToDateList.Text = string.Empty;
                    txtTopicList.Text = string.Empty;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    EmployeeTrainingDetailList = null;
                    txtDateHd.Focus();
                    //lblTrxNo.Focus();
                    break;
                #endregion
                # region CLEAR POPUP DETAILS
                case ControlsEnum.CLEARPOPUPDETAILS:
                    txtEmployee_Filter.Text = string.Empty;
                    hdfEmployee_Filter.Value=hdfBranchLocation_Filter.Value=hdfDepartment_Filter.Value = CommonConstants.SELECTVAL;
                    txtEmployee_Filter.Text = txtBranchLocation_Filter.Text = txtDepartment_Filter.Text = string.Empty;
                    ddlEmploymentType_Filter.SelectedIndex = ddlCompany_Filter.SelectedIndex = ddlEmployeeType_Filter.SelectedIndex = 0;
                    EmployeeTraininge_PopUPList = null;
                    ddlCompany_Filter.Focus();
                    break;
                #endregion
            }
        }
        #endregion
        #region Get Formatted Currency With Comma
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        #endregion
        #region Get Nullable Int
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        #endregion
        #region Convert TimeString To Double
        private double ConvertTimeStringToDouble(string hourminutes)
        {
            double result = 0;
            try
            {
                if (!string.IsNullOrEmpty(hourminutes))
                {
                    string[] arrTime = hourminutes.Split(':');
                    double hours = Convert.ToDouble(arrTime[0]);
                    double minutes = 0;
                    if (arrTime[1].Length > 1)
                        minutes = Convert.ToDouble(arrTime[1]);
                    result = (minutes / Convert.ToDouble(60)) + Convert.ToDouble(hours);
                    result = Math.Round(result, 2);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
        #endregion
        #region Get Time
        private string GetTime(string time)
        {
            string result = string.Empty;
            if (time == string.Empty) return result;

            string[] arr = time.Split('.');
            string h = arr[0];
            string m = string.Empty.PadRight(2, '0');
            if (arr.Length == 2)
            {
                if (arr[1].Length > 1) m = arr[1];
                else m = arr[1].PadRight(2, '0');
            }
            decimal mm = Convert.ToDecimal(m);
            mm = Math.Round(((mm * 60) / 100));
            m = mm.ToString();
            if (h.Length < 2) h = "0" + h;
            if (m.Length < 2) m = "0" + m;
            result = h + ":" + m;
            return result;
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
        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }
        #endregion
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST,
            CLEAR,
            EDIT,
            EMPTRAININGHDR,
            TRAININGDETAILLIST,
            EMPLOYEEDETAILSPOPUP,
            EMPLOYEETRAININGDETAILS,
            CLEARPOPUPDETAILS,
            COMPANY,
            EMPLOYEETYPE,
            EMPLOYMENTTYPE,
            EMPTRAININGDTL,
            SAVEMPLOYEEDETAILSPOPUP
        }
        #endregion
    } 
}