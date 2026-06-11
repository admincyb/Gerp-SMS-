using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.HRMS.Payroll;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Admin.Masters;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using BusinessLogic.HRMS.Payroll;
using BusinessLogic.HRMS.Employee;
using ERP.Utilities.HRMS;
using System.Threading;
using System.Text;
using System.IO;
using BusinessLogic.CommonManagement;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class BonusEntry : System.Web.UI.Page  //ERP.Store.UI.MyBasePage   
    {
        #region Properties and Variables

        #region Properties
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
        /// To keep popup RowIndex in view state
        /// </summary>
        private int RowIndexPopup
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndexPopup] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndexPopup];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndexPopup] = value;
            }
        }

        /// <summary>
        /// To keep popup display mode
        /// </summary>
        private bool PopupViewMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.PopupViewMode] == null ? false : (bool)this.ViewState[ViewstateStrings.PopupViewMode];
            }
            set
            {
                this.ViewState[ViewstateStrings.PopupViewMode] = value;
            }
        }

        private List<BusinessObject.HRMS.Payroll.BonusEntryDetails> BonusEntryDetailList
        {
            get
            {
                return (List<BusinessObject.HRMS.Payroll.BonusEntryDetails>)ViewState["BonusEntryDetailList"];
            }
            set
            {
                ViewState["BonusEntryDetailList"] = value;
            }
        }

        private bool MultiCurrencyEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.MultiCurrencyEnabled] == null ? false : Convert.ToBoolean((this.ViewState[ViewstateStrings.MultiCurrencyEnabled]));
            }
            set
            {
                this.ViewState[ViewstateStrings.MultiCurrencyEnabled] = value;
            }
        }

        #endregion

        #region Variables

        User currentUser;
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        private DataTable dtResult;
        private DataTable dtBonusType;
        private int commonPK = 0;
        DataTable dtCompany;
        private DataTable dtReligion;
        private DataTable dtSubReligion;
        private DataTable dtEmployeeDtl;
        private BonusEntryHeader objBonusEntryHeader;
        private BusinessObject.HRMS.Payroll.EmployeeBonusHeader_PopUp objEmployeeBonusHeaderPopUp;
        private List<BusinessObject.HRMS.Payroll.EmployeeBonus_PopUP> EmployeeBonus_PopUPList;
        private DataSet dsExchangeRate;
        private int CompanyPk = 0;
        #endregion

        #endregion

        #region Page Level Events

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            InitializeComponent();
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }

        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }

        #region Pager Methods
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SetBankEnable", "SetBankEnable();", true);
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(0);});", true);

                if (!MultiCurrencyEnabled)
                    txtCurrency.Enabled = false;

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
                    //GetFieldValues(ControlsEnum.LIST);
                    //SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            //  EntryStatus = EntryStatus.LISTMODE;
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

        #endregion

        #region Page ActionHandler

        private void PageActionHandler()
        {
            try
            {
                PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                    hdfCurrencyFormatWithComma.Value += "0";
                }
                hdfExchangeRateFormat.Value = "#0.";
                int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                for (int i = 0; i < exchrateDecimalDigits; i++)
                {
                    hdfExchangeRateFormat.Value += "0";
                }
                MultiCurrencyEnabled = CommonFunctions.IsMultyCurrencyEnabled();

                EntryStatus = EntryStatus.LISTMODE;
                GetFieldValues(ControlsEnum.BONUSTYPE);
                SetFieldValues(ControlsEnum.BONUSTYPE);
                GetFieldValues(ControlsEnum.RELIGION);
                SetFieldValues(ControlsEnum.RELIGION);
                GetFieldValues(ControlsEnum.SUBRELIGION);
                SetFieldValues(ControlsEnum.SUBRELIGION);
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

        #region Action Handler

        protected void ActionHandler(object sender, EventArgs e)
        {
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                bool bIsChecked = false;
                GridViewRow grvRow;
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
                    if (((DropDownList)sender).ID == "ddlReligion")
                    {
                        Session["SelectReligion"] = 1;
                        Session["SelectReligionDropdownChange"] = 1;

                        commonActions = BusinessObject.AccountManagement.ActionsEnum.SELECTSUBRELIGION;
                    }
                }
                switch (commonActions)
                {

                    #region SUB-RELIGION

                    case BusinessObject.AccountManagement.ActionsEnum.SELECTSUBRELIGION:
                        GetFieldValues(ControlsEnum.SUBRELIGION);
                        SetFieldValues(ControlsEnum.SUBRELIGION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                        Session["SelectReligionDropdownChange"] = null;
                        break;

                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objBonusEntryHeader = new BonusEntryHeader();
                            objBonusEntryHeader = (BonusEntryHeader)SetUIValuesToObject(ControlsEnum.BONUSENTRYHDR);
                            if (objBonusEntryHeader != null)
                            {
                                if (objBonusEntryHeader.BonusEntryDtl != null && objBonusEntryHeader.BonusEntryDtl.Count > 0)
                                {
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<BusinessObject.HRMS.Payroll.BonusEntryHeader>(objBonusEntryHeader);
                                    result = BusinessLogic.HRMS.Payroll.BonusEntryBL.SaveBonusEntryDetails(xmlDoc, out trxNo);

                                    if (result > 0)
                                    {
                                        lblBonNo.Text = trxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Bonus_Save_Success").ToString();
                                        // object[] args = new object[2];
                                        // args[0] = Resources.PageNameRes.SalaryPayment;
                                        // args[1] = trxNo;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
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
                                            litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BonusEntry);
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
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        GetFieldValues(ControlsEnum.BONUSTYPENEW);
                        SetFieldValues(ControlsEnum.BONUSTYPENEW);
                        SetFieldValues(ControlsEnum.BONUSDETAILLIST);
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

                    #region LIST, CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        Response.Redirect(Resources.PageURL.BonusEntry, true);
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region EDIT, DETAIL
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfBOH_PKListPage")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.BONUSTYPENEW);
                            SetFieldValues(ControlsEnum.BONUSTYPENEW);
                            GetFieldValues(ControlsEnum.EMPLOYEEBONUSDETAILS);
                            SetFieldValues(ControlsEnum.EMPLOYEEBONUSDETAILS);
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
                        result = BusinessLogic.HRMS.Payroll.BonusEntryBL.DeleteEmployeeBonus(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BonusEntry);
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
                                litErrorMsg.Text = Resources.PageNameRes.BonusEntry;
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
                                litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.BonusEntry + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BonusEntry);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region SHOW POPUP
                    case ActionsEnum.SHOWPOPUP:
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                        break;
                    #endregion

                    #region SEARCH POPUP
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                        break;
                    #endregion

                    #region  CANCEL POPUP
                    case ActionsEnum.CANCELPOPUP:
                        //ResetForm(ControlsEnum.CLEARADD);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion

                    #region ADD POPUP DETIALS
                    case ActionsEnum.SAVE_ACTIONPOPUP:
                        if (grdEmpBonus_PopUp.Rows.Count > 0)
                        {
                            if (BonusEntryDetailList == null)
                            {
                                BonusEntryDetailList = new List<BonusEntryDetails>();
                            }
                            List<BonusEntryDetails> objEmpBonusTempList = (List<BonusEntryDetails>)SetUIValuesToObject(ControlsEnum.SAVEMPLOYEEDETAILSPOPUP);
                            if (objEmpBonusTempList != null)
                            {
                                List<BonusEntryDetails> objEmpAddedList = new List<BonusEntryDetails>();
                                objEmpBonusTempList.ForEach(dtl =>
                                    {
                                        if (BonusEntryDetailList.Where(r => r.BOD_EMPLOYEE == dtl.BOD_EMPLOYEE).Count() > 0)
                                        {
                                            objEmpAddedList.Add(dtl);
                                        }
                                    });
                                if (objEmpAddedList.Count > 0)
                                {
                                    string errMsg = GetLocalResourceObject("Err_EmpSelected").ToString() + "<br />";
                                    objEmpAddedList.ForEach(dtl =>
                                    {
                                        errMsg = errMsg + dtl.empName_txt + "<br />";
                                    });
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(errMsg) + "');", true);
                                    return;
                                }
                                else
                                {

                                    BonusEntryDetailList.AddRange(objEmpBonusTempList);
                                }
                            }
                            if (objEmpBonusTempList.Count > 0)
                            {
                                SetFieldValues(ControlsEnum.BONUSDETAILLIST);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                                litErrorMsg.Text = GetLocalResourceObject("Err_NoAmount").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                            litErrorMsg.Text = GetLocalResourceObject("Err_NoRecords").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region GRID DELETE
                    case ActionsEnum.GRIDDELETE:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlsEnum.GRIDDELETE);
                        SetFieldValues(ControlsEnum.BONUSDETAILLIST);
                        break;
                    #endregion

                    #region CLEAR POPUP
                    case ActionsEnum.CLEARDETAIL:
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        GetFieldValues(ControlsEnum.SUBRELIGION);
                        SetFieldValues(ControlsEnum.SUBRELIGION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
                        break;
                    #endregion

                    #region CURRENCYSELECTED
                    case ActionsEnum.CURRENCYSELECTED:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
        }

        #endregion

        #region Get Field Values

        private void GetFieldValues(ControlsEnum type)
        {
            FilterParameters objFilterParam;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region BONUS TYPE
                    case ControlsEnum.BONUSTYPE:
                        //dtBonusType = BonusEntryBL.GetBonusType(currentUser.SBUID);
                        dtBonusType = BonusEntryBL.GetBonusType(CurrPK, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion

                    #region BONUS TYPE
                    case ControlsEnum.BONUSTYPENEW:
                        dtBonusType = BonusEntryBL.GetBonusType(CurrPK, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
                        break;
                    #endregion

                    #region RELIGION
                    case ControlsEnum.RELIGION:
                        dtReligion = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetdropdownFillReligion(commonPK);
                        break;
                    #endregion

                    #region SUB-RELIGION
                    case ControlsEnum.SUBRELIGION:
                        dtSubReligion = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetdropDownSubReligion(commonPK, Convert.ToInt32(ddlReligion.SelectedValue));
                        break;
                    #endregion

                    #region EMPLOYEE DETAILS POPUP
                    case ControlsEnum.EMPLOYEEDETAILSPOPUP:
                        objFilterParam = new FilterParameters();
                        objFilterParam.Religion = GetNullableInt(ddlReligion.SelectedValue) > 0 ? GetNullableInt(ddlReligion.SelectedValue) : null;
                        objFilterParam.SubReligion = GetNullableInt(ddlSubReligion.SelectedValue) > 0 ? GetNullableInt(ddlSubReligion.SelectedValue) : null;
                        objFilterParam.StatePk =Convert.ToInt32(hdfState.Value) > 0 ? hdfState.Value : null;;
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Date = Convert.ToDateTime(txtDate.Text);
                        objFilterParam.BonusType = Convert.ToInt16(ddlListBonusTypeNew.SelectedValue);
                        objFilterParam.Currency = GetNullableInt(hdfCurrency.Value) > 0 ? GetNullableInt(hdfCurrency.Value) : null;
                        objFilterParam.BizUnit = currentUser.SBUID;
                        objFilterParam.EmpDoj = txtDojBefore.Text != string.Empty ? Convert.ToDateTime(txtDojBefore.Text) : (DateTime?)null;
                        objFilterParam.ToDate = txtDate.Text != string.Empty ? Convert.ToDateTime(txtDate.Text) : (DateTime?)null;
                        dtEmployeeDtl = BonusEntryBL.GetEmployeeBonusDetailsPopUp(objFilterParam);
                        //if (objEmployeeBonusHeaderPopUp != null)
                        //    EmployeeBonus_PopUPList = objEmployeeBonusHeaderPopUp.EmployeeBonus_PopUPDtl;
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        dtResult = BusinessLogic.HRMS.Payroll.BonusEntryBL.GetEmployeeBonusList(txtListFromDate.Text, txtListToDate.Text, Convert.ToInt32(ddlListBonusType.SelectedValue), currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE), PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")));
                        break;
                    #endregion

                    #region EMPLOYEE BONUS DETAILS
                    case ControlsEnum.EMPLOYEEBONUSDETAILS:
                        objBonusEntryHeader = BonusEntryBL.GetEmployeeDetailsByPK(CurrPK);
                        if (objBonusEntryHeader == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (!string.IsNullOrEmpty(hdfCurrency.Value) && Convert.ToInt32(hdfCurrency.Value) > 0)
                        {
                            DateTime Date = DateTime.Now;
                            DateTime.TryParse(txtDate.Text.Trim(), out Date);
                            dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Date);
                        }
                        break;
                    #endregion

                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtResult = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion

                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
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
                    #region BONUS TYPE
                    case ControlsEnum.BONUSTYPE:
                        BindDropDown(ControlsEnum.BONUSTYPE);
                        break;
                    #endregion

                    #region BONUS TYPE
                    case ControlsEnum.BONUSTYPENEW:
                        BindDropDown(ControlsEnum.BONUSTYPENEW);
                        break;
                    #endregion

                    #region RELIGION

                    case ControlsEnum.RELIGION:
                        BindDropDown(ControlsEnum.RELIGION);
                        break;

                    #endregion

                    #region SUB-RELIGION

                    case ControlsEnum.SUBRELIGION:
                        BindDropDown(ControlsEnum.SUBRELIGION);
                        break;

                    #endregion

                    #region EMPLOYEE DETAILS POPUP
                    case ControlsEnum.EMPLOYEEDETAILSPOPUP:
                        BindGrid(ControlsEnum.EMPLOYEEDETAILSPOPUP);
                        break;
                    #endregion

                    #region BONUS DETAILS LIST
                    case ControlsEnum.BONUSDETAILLIST:
                        BindGrid(ControlsEnum.BONUSDETAILLIST);
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region GRID DELETE
                    case ControlsEnum.GRIDDELETE:
                        GetUIValuesFromObject(ControlsEnum.GRIDDELETE);
                        break;
                    #endregion

                    #region EMPLOYEE BONUS DETAILS
                    case ControlsEnum.EMPLOYEEBONUSDETAILS:
                        GetUIValuesFromObject(ControlsEnum.EMPLOYEEBONUSDETAILS);
                        break;
                    #endregion

                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        GetUIValuesFromObject(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion

                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        GetUIValuesFromObject(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
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
                    #region BONUS ENTRY HDR
                    case ControlsEnum.BONUSENTRYHDR:
                        Label lblFooterTotalBonusAmt;
                        objBonusEntryHeader.BOH_PK = CurrPK;
                        objBonusEntryHeader.BOH_NO = lblBonNo.Text;
                        objBonusEntryHeader.BOH_DATE = DateTime.Parse(txtDate.Text);
                        objBonusEntryHeader.BOH_BONUS_TYPE = Convert.ToInt16(ddlListBonusTypeNew.SelectedValue);
                        objBonusEntryHeader.BOH_REMARK = txtRemarksHdr.Text;
                        objBonusEntryHeader.BOH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        objBonusEntryHeader.BOH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                        objBonusEntryHeader.BOH_STATUS = Convert.ToInt32(DbActiveStatus.ACTIVE);
                        objBonusEntryHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                        objBonusEntryHeader.BOH_MOD_DT = LastModifiedTime;
                        objBonusEntryHeader.LAST_MOD_DT = LastModifiedTime;
                        objBonusEntryHeader.BOH_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        objBonusEntryHeader.BOH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                        objBonusEntryHeader.BOH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        objBonusEntryHeader.BOH_EXCHG_RATE = string.IsNullOrEmpty(txtExchangeRate.Text.Trim()) ? 1 : Convert.ToDouble(txtExchangeRate.Text.Trim());

                        if (grdEmpBonusTypeList.FooterRow != null)
                        {
                            objBonusEntryHeader.BOH_TOTAL_AMT = Convert.ToDecimal((grdEmpBonusTypeList.FooterRow.FindControl("lblFooterTotalBonusAmt") as Label).Text);
                        }

                        SetUIValuesToObject(ControlsEnum.BONUSENTRYDTL);
                        objBonusEntryHeader.BonusEntryDtl = BonusEntryDetailList;
                        retObject = objBonusEntryHeader;
                        break;
                    #endregion

                    #region BONUS ENTRY DTL
                    case ControlsEnum.BONUSENTRYDTL:
                        BonusEntryDetailList = new List<BusinessObject.HRMS.Payroll.BonusEntryDetails>();
                        foreach (GridViewRow grdrow in grdEmpBonusTypeList.Rows)
                        {
                            BusinessObject.HRMS.Payroll.BonusEntryDetails objBonusDetails = new BusinessObject.HRMS.Payroll.BonusEntryDetails();
                            objBonusDetails.ROW_NO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRowNo")).Value);
                            objBonusDetails.BOD_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfBodPk")).Value);
                            objBonusDetails.BOD_BOH_PK = CurrPK;
                            objBonusDetails.BOD_EMPLOYEE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfBodEmpPk")).Value);
                            objBonusDetails.BOD_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                            objBonusDetails.BOD_AMOUNT = Convert.ToDecimal(((Label)grdrow.FindControl("lblgrdAmount")).Text.ToString());
                            BonusEntryDetailList.Add(objBonusDetails);
                        }
                        break;
                    #endregion

                    #region SAVE EMPLOYEE DETAILS POPUP TO GRID
                    case ControlsEnum.SAVEMPLOYEEDETAILSPOPUP:
                        decimal val = 0;


                        List<BonusEntryDetails> objBonEntryDetailList = new List<BusinessObject.HRMS.Payroll.BonusEntryDetails>();

                        foreach (GridViewRow grdrow in grdEmpBonus_PopUp.Rows)
                        {
                            CheckBox chkEmpselect_PopUp = (CheckBox)grdrow.FindControl("chkEmpselect_PopUp");
                            TextBox txtAmnt = (TextBox)grdrow.FindControl("txtAmount_PopUp");
                            if (chkEmpselect_PopUp.Checked == true && txtAmnt.Text != "0.00")
                            {

                                BusinessObject.HRMS.Payroll.BonusEntryDetails objEmployeeBonusEntDetails = new BusinessObject.HRMS.Payroll.BonusEntryDetails();
                                objEmployeeBonusEntDetails.ROW_NO = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfROW_NO_PopUp")).Value);
                                objEmployeeBonusEntDetails.BOD_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfBOD_PK_PopUp")).Value);
                                objEmployeeBonusEntDetails.BOD_BOH_PK = CurrPK;
                                objEmployeeBonusEntDetails.BOD_EMPLOYEE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpPk_PopUp")).Value);
                                objEmployeeBonusEntDetails.empName_txt = ((Label)grdrow.FindControl("lblempName_txt_PopUp")).ToolTip.HtmlEncode();
                                objEmployeeBonusEntDetails.empBranchText = ((Label)grdrow.FindControl("lblempBranchText_PopUp")).ToolTip.HtmlEncode();
                                objEmployeeBonusEntDetails.empDepartmentText = ((Label)grdrow.FindControl("lblDepartment_PopUp")).ToolTip.HtmlEncode();
                                objEmployeeBonusEntDetails.empDesignationText = ((Label)grdrow.FindControl("lblDesigPopUp")).ToolTip.HtmlEncode();
                                objEmployeeBonusEntDetails.EPD_EMP_TYPE_TEXT = ((HiddenField)grdrow.FindControl("hdfEmpType")).Value.ToString();
                                if (((TextBox)grdrow.FindControl("txtAmount_PopUp")).Text == string.Empty)
                                {
                                    objEmployeeBonusEntDetails.BOD_AMOUNT = val;
                                }
                                else
                                {
                                    objEmployeeBonusEntDetails.BOD_AMOUNT = Convert.ToDecimal(((TextBox)grdrow.FindControl("txtAmount_PopUp")).Text.HtmlEncode());
                                }
                                objBonEntryDetailList.Add(objEmployeeBonusEntDetails);
                            }
                        }
                        retObject = objBonEntryDetailList;
                        break;
                    #endregion
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }

        #endregion

        #region Get UIValues From Object

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region GRID DELETE
                    case ControlsEnum.GRIDDELETE:
                        if (BonusEntryDetailList != null && BonusEntryDetailList.Count > 0 && RowIndex >= 0)
                        {
                            List<BonusEntryDetails> objTempBonusList = BonusEntryDetailList;
                            BonusEntryDetails objTemp = BonusEntryDetailList[RowIndex];
                            if (objTemp != null)
                            {
                                objTempBonusList.Remove(objTemp);
                                BonusEntryDetailList = objTempBonusList;
                            }
                        }
                        break;
                    #endregion

                    #region EMPLOYEE BONUS DETAILS
                    case ControlsEnum.EMPLOYEEBONUSDETAILS:
                        if (objBonusEntryHeader != null)
                        {
                            lblBonNo.Text = string.IsNullOrEmpty(objBonusEntryHeader.BOH_NO) ? Resources.ErpRes.Draft : objBonusEntryHeader.BOH_NO;
                            txtDate.Text = Convert.ToDateTime(objBonusEntryHeader.BOH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            ddlListBonusTypeNew.SelectedIndex = ddlListBonusTypeNew.Items.IndexOf(ddlListBonusTypeNew.Items.FindByValue(objBonusEntryHeader.BOH_BONUS_TYPE.ToString()));
                            //ddlListBonusTypeNew.SelectedValue = Convert.ToInt16(objBonusEntryHeader.BOH_BONUS_TYPE);
                            txtRemarksHdr.Text = objBonusEntryHeader.BOH_REMARK.HtmlDecode();
                            LastModifiedTime = objBonusEntryHeader.BOH_MOD_DT;
                            txtCurrency.Text = HttpUtility.HtmlDecode(objBonusEntryHeader.BOH_CURRENCY_TEXT);
                            hdfCurrency.Value = objBonusEntryHeader.BOH_CURRENCY.ToString();
                            CompanyPk = objBonusEntryHeader.BOH_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));    
                            if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))
                                txtExchangeRate.Enabled = false;
                            else
                                txtExchangeRate.Enabled = true;
                            txtExchangeRate.Text = GetFormattedExchangerate(objBonusEntryHeader.BOH_EXCHG_RATE);

                            BonusEntryDetailList = objBonusEntryHeader.BonusEntryDtl;
                            SetFieldValues(ControlsEnum.BONUSDETAILLIST);
                        }
                        break;
                    #endregion

                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) > 0)
                                txtExchangeRate.Text = GetFormattedExchangerate(dsExchangeRate.Tables[0].Rows[0][0].ToString());
                        }
                        else
                        {
                            txtExchangeRate.Text = string.Empty;
                        }
                        //  Exchange rate field is not editable(Domestic).ie,If selected currency is same as SBU base currency
                        if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))
                            txtExchangeRate.Enabled = false;
                        else
                            txtExchangeRate.Enabled = true;
                        break;
                    #endregion

                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            txtCurrency.Text = string.Format(GetLocalResourceObject("CurrencyDisplayFormat").ToString(), Convert.ToString(dtResult.Rows[0]["CUR_CODE"]), Convert.ToString(dtResult.Rows[0]["CUR_NAME"]));
                            hdfCurrency.Value = Convert.ToString(dtResult.Rows[0]["CUR_PK"]);
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

        #region HELPER METHODS

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                Label lblFooterTotalBonusAmt;
                Label lblgrdAmount;
                switch (controlType)
                {
                    #region EMPLOYEE DETAILS POPUP
                    case ControlsEnum.EMPLOYEEDETAILSPOPUP:
                        BusinessObject.HRMS.Payroll.BonusEntryDetails objEmployeeBonusEntDetails = new BusinessObject.HRMS.Payroll.BonusEntryDetails();
                        if (dtEmployeeDtl != null && dtEmployeeDtl.Rows.Count > 0)
                        {
                            grdEmpBonus_PopUp.DataSource = dtEmployeeDtl;
                            grdEmpBonus_PopUp.DataBind();
                        }
                        else
                        {
                            grdEmpBonus_PopUp.DataSource = null;
                            grdEmpBonus_PopUp.DataBind();
                        }
                        break;
                    #endregion

                    #region BONUS DETAILS LIST
                    case ControlsEnum.BONUSDETAILLIST:
                        if (BonusEntryDetailList != null && BonusEntryDetailList.Count > 0)
                        {
                            grdEmpBonusTypeList.DataSource = BonusEntryDetailList;
                            grdEmpBonusTypeList.DataBind();

                            if (grdEmpBonusTypeList.FooterRow != null)
                            {
                                lblFooterTotalBonusAmt = grdEmpBonusTypeList.FooterRow.FindControl("lblFooterTotalBonusAmt") as Label;
                                lblFooterTotalBonusAmt.Text = GetFormattedCurrencyWithComma(BonusEntryDetailList.Sum(tot => tot.BOD_AMOUNT).ToString(hdfCurrencyFormatWithComma.Value));
                            }

                            txtCurrency.Enabled = false;
                        }
                        else
                        {
                            grdEmpBonusTypeList.DataSource = null;
                            grdEmpBonusTypeList.DataBind();
                            txtCurrency.Enabled = true;
                        }
                        break;
                    #endregion

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

                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region BONUS TYPE
                case ControlsEnum.BONUSTYPE:
                    if (dtBonusType != null)
                    {
                        ddlListBonusType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtBonusType, ("BON_NAME").ToString());
                        ddlListBonusType.DataTextField = "BON_NAME";
                        ddlListBonusType.DataValueField = "BON_PK";
                        ddlListBonusType.DataBind();
                        //ddlListBonusType.Items.Insert(0, new ListItem("Select", "-1"));
                        ddlListBonusType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion

                #region BONUS TYPE
                case ControlsEnum.BONUSTYPENEW:
                    if (dtBonusType != null)
                    {
                        ddlListBonusTypeNew.DataSource = CommonFunctions.HtmlDecodeDataTable(dtBonusType, ("BON_NAME").ToString());
                        ddlListBonusTypeNew.DataTextField = "BON_NAME";
                        ddlListBonusTypeNew.DataValueField = "BON_PK";
                        ddlListBonusTypeNew.DataBind();
                        //ddlListBonusTypeNew.Items.Insert(0, new ListItem("Select", "-1"));
                        ddlListBonusTypeNew.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion

                #region RELIGION
                case ControlsEnum.RELIGION:
                    ddlReligion.Items.Clear();
                    if (dtReligion != null)
                    {
                        ddlReligion.DataSource = CommonFunctions.HtmlDecodeDataTable(dtReligion, ("CON_NAME").ToString());
                        ddlReligion.DataTextField = "CON_NAME";
                        ddlReligion.DataValueField = "CON_PK";
                        ddlReligion.DataBind();
                        //ddlReligion.Items.Insert(0, new ListItem("Select", "-1"));
                        ddlReligion.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    }

                    break;
                #endregion

                #region SUB-RELIGION
                case ControlsEnum.SUBRELIGION:
                    ddlSubReligion.Items.Clear();
                    if (dtSubReligion != null && dtSubReligion.Rows.Count > 0)
                    {
                        ddlSubReligion.DataSource = CommonFunctions.HtmlDecodeDataTable(dtSubReligion, ("CON_NAME").ToString());
                        ddlSubReligion.DataTextField = "CON_NAME";
                        ddlSubReligion.DataValueField = "CON_PK";
                        ddlSubReligion.DataBind();
                    }
                    ddlSubReligion.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                   // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdditionalInfo", "ShowHideAdditionalInfo('1');", true);

                    break;
                #endregion

                #region COMPANY
                case ControlsEnum.COMPANY:
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
                default:
                    break;
            }
        }

        public enum ControlsEnum
        {
            BONUSTYPE,
            BONUSTYPENEW,
            RELIGION,
            SUBRELIGION,
            BONUSENTRYHDR,
            BONUSENTRYDTL,
            SEARCHPOPUP,
            EMPLOYEEDETAILSPOPUP,
            CLEAR,
            LIST,
            CLEARPOPUPDETAILS,
            SAVEMPLOYEEDETAILSPOPUP,
            BONUSDETAILLIST,
            GRIDDELETE,
            EMPLOYEEBONUSDETAILS,
            CLEARDETAIL,
            CURRENCY,
            EXCHANGERATE,
            COMPANY
        }

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

        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }

        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetEvaluatedFormula(object formula)
        {
            double num = 0;
            DataTable dt = new DataTable();
            var amnt = dt.Compute(Convert.ToString(formula), "").ToString();
            double.TryParse(Convert.ToString(amnt), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        public string GetFormattedExchangerate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    lblBonNo.Text = Resources.ErpRes.Draft;
                    txtDate.Text = txtRemarksHdr.Text = string.Empty;
                    txtListFromDate.Text = txtListToDate.Text = string.Empty;
                    ddlReligion.SelectedIndex = ddlSubReligion.SelectedIndex = 0;
                    ddlListBonusTypeNew.SelectedIndex = -1;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    // txtDate.Focus();
                    lblBonNo.Focus();
                    BonusEntryDetailList = null;
                    grdEmpBonusTypeList.DataSource = null;
                    txtDojBefore.Text = string.Empty;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    hdfState.Value = CommonConstants.SELECTVAL;
                    hdfCountry.Value = CommonConstants.SELECTVAL;
                    txtState.Text = Resources.ErpRes.AutoDefaultValue;
                    txtCountry.Text = Resources.ErpRes.AutoDefaultValue; 
                    break;
                case ControlsEnum.CLEARPOPUPDETAILS:
                    ddlReligion.SelectedIndex = ddlSubReligion.SelectedIndex = 0;
                    grdEmpBonus_PopUp.DataSource = null;
                    grdEmpBonus_PopUp.DataBind();
                    txtDojBefore.Text = string.Empty;
                    //BonusEntryDetailList = null;
                    hdfState.Value = CommonConstants.SELECTVAL;
                    hdfCountry.Value = CommonConstants.SELECTVAL;
                    txtState.Text = Resources.ErpRes.AutoDefaultValue;
                    txtCountry.Text = Resources.ErpRes.AutoDefaultValue; 
                    ddlReligion.Focus();
                    break;
            }
        }

        //protected void ChkEmpPopup_Changed(object sender, EventArgs e)
        //{
        //    CheckBox cbx = (CheckBox)sender;
        //    string CurrentCbxId = ((CheckBox)sender).ClientID;
        //    foreach (GridViewRow Row in grdEmpBonus_PopUp.Rows)
        //    {
        //        RequiredFieldValidator rfvAmount = (RequiredFieldValidator)Row.FindControl("rfvAmnt");
        //        RegularExpressionValidator revAmount = (RegularExpressionValidator)Row.FindControl("revAmnt");
        //        CheckBox chkBox = (CheckBox)Row.FindControl("chkEmpselect_PopUp");

        //        if (chkBox.Checked)
        //        {
        //            rfvAmount.Enabled = true;
        //            revAmount.Enabled = true;
        //        }
        //        else
        //        {
        //            rfvAmount.Enabled = false;
        //            revAmount.Enabled = false;
        //        }
        //    }
        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
        //}

        //protected void ChkEmpHdrPopup_Changed(object sender, EventArgs e)
        //{
        //    CheckBox cbx = (CheckBox)sender;
        //    string CurrentCbxId = ((CheckBox)sender).ClientID;

        //    CheckBox chkBox = (CheckBox)grdEmpBonus_PopUp.FindControl("chkEmpselect_PopUp");
        //    RequiredFieldValidator rfvAmount = (RequiredFieldValidator)grdEmpBonus_PopUp.FindControl("rfvAmnt");
        //    RegularExpressionValidator revAmount = (RegularExpressionValidator)grdEmpBonus_PopUp.FindControl("revAmnt");
        //    CheckBox chkBoxRow = (CheckBox)grdEmpBonus_PopUp.FindControl("chkEmpselect_PopUp");

        //    if (chkBox.Checked && chkBoxRow.Checked)
        //    {
        //        rfvAmount.Enabled = true;
        //        revAmount.Enabled = true;
        //    }
        //    else
        //    {
        //        rfvAmount.Enabled = false;
        //        revAmount.Enabled = false;
        //    }
        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divEmployeeDetails_PopUp]','" + GetLocalResourceObject("EmployeeFilter") + "','800','460');", true);
        //}

        #endregion
    }
}