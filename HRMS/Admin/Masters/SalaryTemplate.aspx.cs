using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.CommonManagement;
using System.Xml;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using System.Threading;
using System.Web.UI.HtmlControls;

namespace HRMS.Admin.Masters
{
    public partial class SalaryTemplate : ERP.Store.UI.MyBasePage
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
        private SalaryTemplateBO.SalaryTemplate SalaryTemplateViewState
        {
            get
            {
                return ViewState["SalaryTemplateViewState"] == null ? new SalaryTemplateBO.SalaryTemplate() : (SalaryTemplateBO.SalaryTemplate)ViewState["SalaryTemplateViewState"];
            }
            set
            {
                ViewState["SalaryTemplateViewState"] = value;
            }
        }
        private Dictionary<string, int> PayElementConfigurationViewState
        {
            get
            {
                return this.ViewState["PayElementConfigurationViewState"] == null ? new Dictionary<string, int>() : (Dictionary<string, int>)(this.ViewState["PayElementConfigurationViewState"]);
            }
            set
            {
                this.ViewState["PayElementConfigurationViewState"] = value;
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

        /// <summary>
        /// Current PK
        /// </summary>
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
        /// To keep Salary template detail PK in viewstate
        /// </summary>
        private int TemplateDetailPk
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.TemplateDetailPk] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.TemplateDetailPk];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.TemplateDetailPk] = value;
            }
        }
        #endregion
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int PayrollTypePk = 0;

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

        #region Page_PreRender
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
        }
        #endregion
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                ucPayItemControl1.AfterApply += new EventHandler(ucPayItemControl1_AfterApply);
                InitializeComponent();
                if (!IsPostBack)
                {
                    hdfSalTempDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfSalTempCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfSalTempCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfSalTempCurrencyFormat.Value += "0";
                        hdfSalTempCurrencyFormatWithComma.Value += "0";
                    }

                    this.PageIndexList = "1";
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.PAYELEMENTCONFIGURATION);
                    SetFieldValues(ControlsEnum.PAYELEMENTCONFIGURATION);
                    GetFieldValues(ControlsEnum.SEARCHPAYROLLTYPE);
                    SetFieldValues(ControlsEnum.SEARCHPAYROLLTYPE);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.PAYROLLTYPE);
                    SetFieldValues(ControlsEnum.PAYROLLTYPE);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        void ucPayItemControl1_AfterApply(object sender, EventArgs e)
        {
            List<SalaryTemplateBO.SalaryTemplateDetail> tempSalaryTemplateDetailsList = SalaryTemplateViewState.SalaryTemplateDetails;
            if (tempSalaryTemplateDetailsList == null) tempSalaryTemplateDetailsList = new List<SalaryTemplateBO.SalaryTemplateDetail>();
            int payElementPk = ucPayItemControl1.PayElement;
            string payCalculationModeText = string.Empty;
            int payCalculationMode = Convert.ToInt32(CommonConstants.SELECTVAL);
            payCalculationModeText = PayCalculationModeEnum.FormulaDependOnAnotherPayElement.ToString();
            //payCalculationMode = PayElementConfigurationViewState["Formula"];
            payCalculationMode = ucPayItemControl1.ElementTypeCalcMode;
            SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplateDetailsObj = new SalaryTemplateBO.SalaryTemplateDetail();
            if ((GetNullableInt(hdfSerialNo.Value) ?? 0) == 0) // New
            {
                tempSalaryTemplateDetailsObj = tempSalaryTemplateDetailsList
                    .Where(x => x.PayElementPk == payElementPk)
                    .SingleOrDefault();
                if (tempSalaryTemplateDetailsObj != null)
                {
                    litErrorMsg.Text = string.Format(GetLocalResourceObject("PayElementAlreadyExists").ToString(), tempSalaryTemplateDetailsObj.PayElementName);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" +
                        CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    return;
                }
                int slNo = tempSalaryTemplateDetailsList.Count > 0 ? (tempSalaryTemplateDetailsList.Max(x => x.SlNo)) + 1 : 0;

                if (slNo < 1) slNo = 1;
                tempSalaryTemplateDetailsObj = new SalaryTemplateBO.SalaryTemplateDetail();
                tempSalaryTemplateDetailsObj.SlNo = slNo;
                tempSalaryTemplateDetailsObj.PayElementPk = payElementPk;
                tempSalaryTemplateDetailsObj.IsEdited = 1;
                tempSalaryTemplateDetailsObj.PayClassificationPk = ucPayItemControl1.PayClassification;
                tempSalaryTemplateDetailsObj.PayElementName = ucPayItemControl1.PayElementName;
                tempSalaryTemplateDetailsObj.PayElementDisplay = ucPayItemControl1.FormulaText;
                tempSalaryTemplateDetailsObj.PayElementValue = ucPayItemControl1.FormulaValue;
                tempSalaryTemplateDetailsObj.PayElementDeduction = ucPayItemControl1.PayElementDeduction;
                tempSalaryTemplateDetailsObj.PayCalculationModeText = payCalculationModeText;
                tempSalaryTemplateDetailsObj.PayCalculationMode = payCalculationMode;
                tempSalaryTemplateDetailsObj.MinAmount = ucPayItemControl1.MinimunAmount;
                tempSalaryTemplateDetailsObj.MaxAmount = ucPayItemControl1.MaximumAmount;
                int STS_SL_NO;
                STS_SL_NO = tempSalaryTemplateDetailsList.Count > 0 ?
                                        (tempSalaryTemplateDetailsList
                                            .Where(x => x.PayElementDeduction == tempSalaryTemplateDetailsObj.PayElementDeduction)
                                            .Count() > 0 ?
                                            (tempSalaryTemplateDetailsList
                                            .Where(x => x.PayElementDeduction == tempSalaryTemplateDetailsObj.PayElementDeduction)
                                            .Max(x => x.STS_SL_NO) + 1) : 1)
                                        : 1;

                tempSalaryTemplateDetailsObj.STS_SL_NO = STS_SL_NO;
                tempSalaryTemplateDetailsList.Add(tempSalaryTemplateDetailsObj);
            }
            else // Edit
            {
                int slNo = GetNullableInt(hdfSerialNo.Value).Value;
                tempSalaryTemplateDetailsObj = tempSalaryTemplateDetailsList
                    .Where(x => x.PayElementPk == payElementPk && x.SlNo != slNo)
                    .SingleOrDefault();
                if (tempSalaryTemplateDetailsObj != null)
                {
                    litErrorMsg.Text = string.Format(GetLocalResourceObject("PayElementAlreadyExists").ToString(), tempSalaryTemplateDetailsObj.PayElementName);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" +
                        CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    return;
                }

                tempSalaryTemplateDetailsObj = tempSalaryTemplateDetailsList
                     .Where(x => x.SlNo == slNo)
                     .Single();
                tempSalaryTemplateDetailsObj.PayElementPk = payElementPk;
                tempSalaryTemplateDetailsObj.IsEdited = 1;
                tempSalaryTemplateDetailsObj.PayClassificationPk = ucPayItemControl1.PayClassification;
                tempSalaryTemplateDetailsObj.PayElementName = ucPayItemControl1.PayElementName;
                tempSalaryTemplateDetailsObj.PayElementDisplay = ucPayItemControl1.FormulaText;
                tempSalaryTemplateDetailsObj.PayElementValue = ucPayItemControl1.FormulaValue;
                tempSalaryTemplateDetailsObj.PayElementDeduction = ucPayItemControl1.PayElementDeduction;
                tempSalaryTemplateDetailsObj.PayCalculationModeText = payCalculationModeText;
                tempSalaryTemplateDetailsObj.PayCalculationMode = payCalculationMode;
                tempSalaryTemplateDetailsObj.MinAmount = ucPayItemControl1.MinimunAmount;
                tempSalaryTemplateDetailsObj.MaxAmount = ucPayItemControl1.MaximumAmount;
            }
            SalaryTemplateBO.SalaryTemplate tempSalaryTemplate = SalaryTemplateViewState;
            tempSalaryTemplate.SalaryTemplateDetails = tempSalaryTemplateDetailsList;
            SalaryTemplateViewState = tempSalaryTemplate;
            BindGrid(ControlsEnum.EARNING);
            BindGrid(ControlsEnum.DEDUCTION);
            ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
            ucPayItemControl1.ResetForm();
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
            GridViewRow gvrTemplate;
            XmlDocument xmlDoc;
            bool bIsChecked = false;
            SalaryTemplateBO.EmpSalaryTemplate objEmpSalTemplate;
            try
            {
                int? result = null;
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
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
                    //if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    //{
                    //    commonActions = ActionsEnum.CHANGETYPE;
                    //}
                }

                #endregion
                switch (commonActions)
                {
                    #region SAVE
                    case ActionsEnum.SAVE:
                        SalaryTemplateViewState = (SalaryTemplateBO.SalaryTemplate)SetUIValuesToObject(ControlsEnum.SALARYTEMPLATE);
                        if (SalaryTemplateViewState.SalaryTemplateDetails.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            //ResetForm(ControlsEnum.CLEAR);
                            ucPayItemControl1.ResetMode();
                            return;
                        }

                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(SalaryTemplateViewState);
                        result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.SaveSalaryTemplate(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            ucPayItemControl1.ResetForm();
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            ucPayItemControl1.SetData(string.Empty, string.Empty, -1, -1, -1, 0, 0);
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.ItemCodeAlreadyExists;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.INCORRECT)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_SalaryTemplateKeyBaseExists").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.ItemNameAlreadyExists;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region LIST, CANCEL
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ucPayItemControl1.ResetForm();
                        //ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                ucPayItemControl1.ResetForm();
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTemplatePkListPage")).Value);
                                GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                                SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {

                            EntryStatus = EntryStatus.EDITMODE;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        //SalaryTemplateViewState = new SalaryTemplateBO.SalaryTemplate();
                        ResetForm(ControlsEnum.CLEAR);
                        ucPayItemControl1.ResetForm();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(1);", true);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.DeleteSalaryTemplate(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndexList) > 1)
                            {
                                PageIndexList = Convert.ToString(Convert.ToInt32(PageIndexList) - 1);
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            ucPayItemControl1.SetData(string.Empty, string.Empty, -1, -1, -1, 0, 0);
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate;
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
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
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
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        this.PageIndexList = "1";
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region MOVEUP
                    case ActionsEnum.MOVEUP:
                        if (((ImageButton)sender).ID == "imbRuleUpEarn")  // 
                        {
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstTemp =
                               SalaryTemplateViewState.SalaryTemplateDetails
                                .OrderBy(x => x.STS_SL_NO)
                                .ToList();
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstEarn = lstTemp
                                .Where(x => x.PayElementDeduction != true)
                                .ToList();


                            int STS_SL_NO = Convert.ToInt32((sender as ImageButton).CommandArgument);
                            SalaryTemplateBO.SalaryTemplateDetail currLine = lstEarn.Where(rl => rl.STS_SL_NO == STS_SL_NO).First();
                            int lineIndex = lstEarn.FindIndex(rl => rl.STS_SL_NO == STS_SL_NO);
                            SalaryTemplateBO.SalaryTemplateDetail nextLine = lineIndex == 0 ? null : lstEarn.ElementAt(lineIndex - 1);
                            if (nextLine != null)
                            {
                                int currentlineNumber = currLine.STS_SL_NO;
                                currLine.STS_SL_NO = nextLine.STS_SL_NO;
                                nextLine.STS_SL_NO = currentlineNumber;
                            }
                            SalaryTemplateViewState.SalaryTemplateDetails = lstTemp
                                 .OrderBy(x => x.STS_SL_NO)
                                 .ToList();
                        }
                        else  // ((ImageButton)sender).ID =imbRuleUpDedu
                        {
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstTemp =
                               SalaryTemplateViewState.SalaryTemplateDetails
                                .OrderBy(x => x.STS_SL_NO)
                                .ToList();
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstDed = lstTemp
                               .Where(x => x.PayElementDeduction == true)
                               .ToList();


                            int STS_SL_NO = Convert.ToInt32((sender as ImageButton).CommandArgument);
                            SalaryTemplateBO.SalaryTemplateDetail currLine = lstDed.Where(rl => rl.STS_SL_NO == STS_SL_NO).First();
                            int lineIndex = lstDed.FindIndex(rl => rl.STS_SL_NO == STS_SL_NO);
                            SalaryTemplateBO.SalaryTemplateDetail nextLine = lineIndex == 0 ? null : lstDed.ElementAt(lineIndex - 1);
                            if (nextLine != null)
                            {
                                int currentlineNumber = currLine.STS_SL_NO;
                                currLine.STS_SL_NO = nextLine.STS_SL_NO;
                                nextLine.STS_SL_NO = currentlineNumber;
                            }
                            SalaryTemplateViewState.SalaryTemplateDetails = lstTemp
                                 .OrderBy(x => x.STS_SL_NO)
                                 .ToList();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(-1);", true);
                        BindGrid(ControlsEnum.EARNING);
                        BindGrid(ControlsEnum.DEDUCTION);
                        break;
                    #endregion
                    #region MOVEDOWN
                    case ActionsEnum.MOVEDOWN:
                        if (((ImageButton)sender).ID == "imbRuleDownEarn")  // 
                        {
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstTemp =
                                SalaryTemplateViewState.SalaryTemplateDetails
                                 .OrderBy(x => x.STS_SL_NO)
                                 .ToList();
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstEarn = lstTemp
                                .Where(x => x.PayElementDeduction != true)
                                .ToList();


                            int STS_SL_NO = Convert.ToInt32((sender as ImageButton).CommandArgument);
                            SalaryTemplateBO.SalaryTemplateDetail currLine = lstEarn.Where(rl => rl.STS_SL_NO == STS_SL_NO).First();
                            int lineIndex = lstEarn.FindIndex(rl => rl.STS_SL_NO == STS_SL_NO);
                            SalaryTemplateBO.SalaryTemplateDetail nextLine = lineIndex == lstEarn.Count - 1 ? null : lstEarn.ElementAt(lineIndex + 1);
                            if (nextLine != null)
                            {
                                int currentlineNumber = currLine.STS_SL_NO;
                                currLine.STS_SL_NO = nextLine.STS_SL_NO;
                                nextLine.STS_SL_NO = currentlineNumber;
                            }
                            SalaryTemplateViewState.SalaryTemplateDetails = lstTemp
                                 .OrderBy(x => x.STS_SL_NO)
                                 .ToList();
                        }
                        else  // ((ImageButton)sender).ID =imbRuleUpDedu
                        {
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstTemp =
                                SalaryTemplateViewState.SalaryTemplateDetails
                                 .OrderBy(x => x.STS_SL_NO)
                                 .ToList();
                            List<SalaryTemplateBO.SalaryTemplateDetail> lstDed = lstTemp
                               .Where(x => x.PayElementDeduction == true)
                               .ToList();

                            int STS_SL_NO = Convert.ToInt32((sender as ImageButton).CommandArgument);
                            SalaryTemplateBO.SalaryTemplateDetail currLine = lstDed.Where(rl => rl.STS_SL_NO == STS_SL_NO).First();
                            int lineIndex = lstDed.FindIndex(rl => rl.STS_SL_NO == STS_SL_NO);
                            SalaryTemplateBO.SalaryTemplateDetail nextLine = lineIndex == lstDed.Count - 1 ? null : lstDed.ElementAt(lineIndex + 1);
                            if (nextLine != null)
                            {
                                int currentlineNumber = currLine.STS_SL_NO;
                                currLine.STS_SL_NO = nextLine.STS_SL_NO;
                                nextLine.STS_SL_NO = currentlineNumber;
                            }
                            SalaryTemplateViewState.SalaryTemplateDetails = lstTemp
                                 .OrderBy(x => x.STS_SL_NO)
                                 .ToList();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PayElementCalcMode", "PayElementCalcMode(-1);", true);
                        BindGrid(ControlsEnum.EARNING);
                        BindGrid(ControlsEnum.DEDUCTION);
                        break;
                    #endregion

                    #region Activate
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.UpdateSalaryTemplateStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfTemplatePkListPage")).Value), 1, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfListSTE_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate.ToString());
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
                    // Do Action if click DeActivate Button
                    case ActionsEnum.DEACTIVATE:
                        gvrTemplate = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.UpdateSalaryTemplateStatus(Convert.ToInt32(((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfTemplatePkListPage")).Value), 0, currentUser.PKUser, ((HiddenField)grdList.Rows[gvrTemplate.RowIndex].FindControl("hdfListSTE_MOD_DT")).Value);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate.ToString());
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
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate.ToString());
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
                    #region APPLY
                    case ActionsEnum.APPLY:                        
                        objEmpSalTemplate = new SalaryTemplateBO.EmpSalaryTemplate();
                        objEmpSalTemplate = (SalaryTemplateBO.EmpSalaryTemplate)SetUIValuesToObject(ControlsEnum.EMPPAYELEMENTUPDATEALL);
                        if (objEmpSalTemplate.EmpTemplateDetails.Count == 0 && objEmpSalTemplate.INSERT_NEW == 0)
                        {
                            ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupUpdateElement]','" + GetLocalResourceObject("PayElementUpdation").ToString() + "','720','300');", true);
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsSelected")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        objEmpSalTemplate.IS_DELETE = 0;
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objEmpSalTemplate);
                        result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.UpdateEmployeeSalaryTemplate(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.EMPPAYELEMENTUPDATEALL);
                            litErrorMsg.Text = GetLocalResourceObject("Msg_UpdateSuccess").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DELETE EMP PAY ELEMENT
                    case ActionsEnum.DELETEEMPPAYELEMENT:                        
                        objEmpSalTemplate = new SalaryTemplateBO.EmpSalaryTemplate();
                        objEmpSalTemplate = (SalaryTemplateBO.EmpSalaryTemplate)SetUIValuesToObject(ControlsEnum.EMPPAYELEMENTUPDATEALL);
                        if (objEmpSalTemplate.EmpTemplateDetails.Count == 0 && objEmpSalTemplate.INSERT_NEW == 0)
                        {
                            ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupUpdateElement]','" + GetLocalResourceObject("PayElementUpdation").ToString() + "','720','300');", true);
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsSelected")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        objEmpSalTemplate.IS_DELETE = 1;
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objEmpSalTemplate);
                        result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.UpdateEmployeeSalaryTemplate(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.EMPPAYELEMENTUPDATEALL);
                            litErrorMsg.Text = GetLocalResourceObject("Msg_DeleteSuccess").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.PayElement;
                                litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
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

        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            int slNo;
            if (senderGridView.ID == "grdEarning" || senderGridView.ID == "grdDeduction")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    HiddenField hdfPk = row.FindControl("hdfPk") as HiddenField;
                    //HiddenField hdfErnMinAmount = row.FindControl("hdfErnMinAmount") as HiddenField;
                    //HiddenField hdfErnMaxAmount = row.FindControl("hdfErnMaxAmount") as HiddenField;
                    // HiddenField hdfPayElementPk = row.FindControl("hdfPayElementPk") as HiddenField;
                    hdfSerialNo.Value = hdfSlNo.Value;
                    slNo = GetNullableInt(hdfSlNo.Value).Value;

                    SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplateDetails = SalaryTemplateViewState.SalaryTemplateDetails
                        .Where(x => x.SlNo == slNo)
                        .Single();
                    ucPayItemControl1.SetData(tempSalaryTemplateDetails.PayElementDisplay, tempSalaryTemplateDetails.PayElementValue
                        , tempSalaryTemplateDetails.PayClassificationPk, tempSalaryTemplateDetails.PayElementPk, tempSalaryTemplateDetails.PayCalculationMode
                        , tempSalaryTemplateDetails.MinAmount, tempSalaryTemplateDetails.MaxAmount);
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    HiddenField hdfPk = row.FindControl("hdfPk") as HiddenField;
                    // HiddenField hdfPayElementPk = row.FindControl("hdfPayElementPk") as HiddenField;
                    hdfSerialNo.Value = string.Empty;
                    slNo = GetNullableInt(hdfSlNo.Value).Value;

                    SalaryTemplateBO.SalaryTemplate tempSalaryTemplateViewState = SalaryTemplateViewState;
                    List<SalaryTemplateBO.SalaryTemplateDetail> tempList = tempSalaryTemplateViewState.SalaryTemplateDetails;
                    SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplate = tempSalaryTemplateViewState.SalaryTemplateDetails
                        .Where(x => x.SlNo == slNo)
                        .Single();
                    tempList.Remove(tempSalaryTemplate);

                    List<SalaryTemplateBO.SalaryTemplateDetail> selectedList = tempList
                        .Where(x => x.PayElementDeduction == tempSalaryTemplate.PayElementDeduction)
                        .OrderBy(x => x.STS_SL_NO)
                        .ToList();
                    for (int i = 0; i < selectedList.Count; i++)
                    {
                        selectedList[i].STS_SL_NO = i + 1;
                    }

                    tempSalaryTemplateViewState.SalaryTemplateDetails = tempList;
                    BindGrid(ControlsEnum.EARNING);
                    BindGrid(ControlsEnum.DEDUCTION);
                    ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                }
                else if (e.CommandName == "UPDATE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    HiddenField hdfPk = row.FindControl("hdfPk") as HiddenField;
                    Label lblModifiedPayElement = row.FindControl("lblModifiedPayElement") as Label;
                    slNo = GetNullableInt(hdfSlNo.Value).Value;
                    SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplateDetails = SalaryTemplateViewState.SalaryTemplateDetails.Where(x => x.SlNo == slNo).Single();
                    lblModifiedPayElementName.Text = tempSalaryTemplateDetails.PayElementName;
                    lblModifiedPayElementValue.Text = tempSalaryTemplateDetails.PayElementDisplay;
                    hdfModifiedPayElementValue.Value = tempSalaryTemplateDetails.PayElementValue;
                    chkInsertNew.Checked = false;
                    int detpk = 0;
                    int.TryParse(hdfPk.Value, out detpk);
                    TemplateDetailPk = detpk;
                    GetFieldValues(ControlsEnum.EMPPAYELEMENT);
                    SetFieldValues(ControlsEnum.EMPPAYELEMENT);
                    ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupUpdateElement]','" + GetLocalResourceObject("PayElementUpdation").ToString() + "','720','300');", true);
                }
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdEarning")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfEarnCalcMode = e.Row.FindControl("hdfEarnCalcMode") as HiddenField;
                        HtmlControl divEarnMode = e.Row.FindControl("divEarnMode") as HtmlControl;
                        //Image imgEarnMode = e.Row.FindControl("imgEarnMode") as Image;
                        SetPayelementIconCss(Convert.ToInt32(hdfEarnCalcMode.Value), divEarnMode);
                    }
                }
                if (((GridView)sender).ID == "grdDeduction")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfDeductCalcMode = e.Row.FindControl("hdfDeductCalcMode") as HiddenField;
                        HtmlControl divEarnMode = e.Row.FindControl("divDeductMode") as HtmlControl;
                        //Image imgDeductMode = e.Row.FindControl("imgDeductMode") as Image;
                        SetPayelementIconCss(Convert.ToInt32(hdfDeductCalcMode.Value), divEarnMode);
                    }
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
            BusinessObject.GridPrams gridParam;
            try
            {
                switch (type)
                {
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                        break;
                    #endregion
                    #region PAYELEMENTCONFIGURATION
                    case ControlsEnum.PAYELEMENTCONFIGURATION:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAY ELEMENT CALCULATION MODE");
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.SearchBy = string.Empty;
                        gridParam.SearchValue = txtTemplateNameListPage.Text.Trim();
                        gridParam.PageNumber = Convert.ToInt32(PageIndexList);
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        int payrolltype = Convert.ToInt32(ddlSrchPayrollType.SelectedValue) > 0 ? Convert.ToInt32(ddlSrchPayrollType.SelectedValue) : 0;
                        dsPageData = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.GetSalaryTemplateList(gridParam, currentUser.SBUID, payrolltype, txtFilterCode.Text.Trim());
                        break;
                    #endregion
                    #region SALARYTEMPLATE
                    case ControlsEnum.SALARYTEMPLATE:
                        string xmlData = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.GetSalaryTemplate(CurrPK);
                        SalaryTemplateBO.SalaryTemplate tempSalaryTemplate;
                        if (xmlData == "<Root/>")
                        {
                            tempSalaryTemplate = new SalaryTemplateBO.SalaryTemplate();
                        }
                        else
                        {
                            tempSalaryTemplate = CommonFunctions.XmlDeserialize<SalaryTemplateBO.SalaryTemplate>(xmlData);
                        }
                        int slNo = 0;
                        foreach (var item in tempSalaryTemplate.SalaryTemplateDetails)
                        {
                            item.SlNo = ++slNo;
                            // 25-05-2016 Changed in SP
                            //if (string.IsNullOrWhiteSpace(item.PayElementDisplay))
                            //{
                            //    item.PayElementDisplay = item.PayElementValue;
                            //}
                        }
                        SalaryTemplateViewState = tempSalaryTemplate;
                        break;
                    #endregion

                    #region PAYROLL TYPE
                    case ControlsEnum.PAYROLLTYPE:
                    case ControlsEnum.SEARCHPAYROLLTYPE:
                        dtResult = BusinessLogic.HRMS.Payroll.PayrollProcessBL.GetPayrollType(PayrollTypePk, currentUser.SBUID, Convert.ToInt32(DbActiveStatus.ACTIVE));
                        break;
                    #endregion
                    #region EMP PAY ELEMENT
                    case ControlsEnum.EMPPAYELEMENT:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.GetEmployeePayElement(TemplateDetailPk);
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
                    #region PAYELEMENTCONFIGURATION
                    case ControlsEnum.PAYELEMENTCONFIGURATION:
                        Dictionary<string, int> tempPayElementConfiguration = new Dictionary<string, int>();
                        foreach (DataRow row in dtResult.Rows)
                        {
                            if (row["CFG_DATA"] != null && row["CFG_VALUE"] != null)
                                tempPayElementConfiguration.Add(row["CFG_DATA"].ToString(), GetNullableInt(row["CFG_VALUE"].ToString()).Value);
                        }
                        PayElementConfigurationViewState = tempPayElementConfiguration;
                        break;
                    #endregion

                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region SALARYTEMPLATE
                    case ControlsEnum.SALARYTEMPLATE:
                        GetUIValuesFromObject(ControlsEnum.SALARYTEMPLATE);
                        break;
                    #endregion

                    case ControlsEnum.PAYROLLTYPE:
                        BindDropDown(ControlsEnum.PAYROLLTYPE);
                        break;

                    case ControlsEnum.SEARCHPAYROLLTYPE:
                        BindDropDown(ControlsEnum.SEARCHPAYROLLTYPE);
                        break;

                    #region EMP PAY ELEMENT
                    case ControlsEnum.EMPPAYELEMENT:
                        BindGrid(ControlsEnum.EMPPAYELEMENT);
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
                #region PAYROLL TYPE
                case ControlsEnum.PAYROLLTYPE:
                    ddlPayrollType.Items.Clear();
                    ddlPayrollType.DataSource = dtResult;
                    ddlPayrollType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.PTM_NAME;
                    ddlPayrollType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.PTM_PK;
                    ddlPayrollType.DataBind();
                    ddlPayrollType.Items.HtmlDecode();
                    ddlPayrollType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlsEnum.SEARCHPAYROLLTYPE:
                    ddlSrchPayrollType.Items.Clear();
                    ddlSrchPayrollType.DataSource = dtResult;
                    ddlSrchPayrollType.DataTextField = GTIService.Constants.HRMS.Employee.Fields.PTM_NAME;
                    ddlSrchPayrollType.DataValueField = GTIService.Constants.HRMS.Employee.Fields.PTM_PK;
                    ddlSrchPayrollType.DataBind();
                    ddlSrchPayrollType.Items.HtmlDecode();
                    ddlSrchPayrollType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region CLEAR
                case ControlsEnum.CLEAR:
                    // I keep the code just as a template for dropdown binding...
                    //ddlPayClassification.Items.Clear();
                    //ddlPayClassification.DataSource = dtResult;
                    //ddlPayClassification.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    //ddlPayClassification.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    //ddlPayClassification.DataBind();
                    //ddlPayClassification.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    //ddlPayClassification.Items.HtmlDecode();
                    break;
                #endregion

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
                    #region EARNING
                    case ControlsEnum.EARNING:
                        if (SalaryTemplateViewState.SalaryTemplateDetails != null && SalaryTemplateViewState.SalaryTemplateDetails.Count > 0)
                        {
                            grdEarning.DataSource = SalaryTemplateViewState.SalaryTemplateDetails
                                .Where(x => x.PayElementDeduction != true)
                                .OrderBy(x => x.STS_SL_NO);
                            //.Where(x => x.IS_DELETED == 0);
                            grdEarning.DataBind();
                        }
                        else
                        {
                            grdEarning.DataSource = null;
                            grdEarning.DataBind();
                        }
                        break;
                    #endregion
                    #region EARNING
                    case ControlsEnum.DEDUCTION:
                        if (SalaryTemplateViewState.SalaryTemplateDetails != null && SalaryTemplateViewState.SalaryTemplateDetails.Count > 0)
                        {
                            grdDeduction.DataSource = SalaryTemplateViewState.SalaryTemplateDetails
                                .Where(x => x.PayElementDeduction == true)
                                  .OrderBy(x => x.STS_SL_NO);
                            //.Where(x => x.IS_DELETED == 0);
                            grdDeduction.DataBind();
                        }
                        else
                        {
                            grdDeduction.DataSource = null;
                            grdDeduction.DataBind();
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
                    #region EM PPAY ELEMENT:
                    case ControlsEnum.EMPPAYELEMENT:
                        if (dtResult != null)
                            grdEmpPaylement.DataSource = dtResult;
                        else
                            grdEmpPaylement.DataSource = null;
                        grdEmpPaylement.DataBind();
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

        protected void GridView_DataBound(object sender, EventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdEarning")
            {
                if (grdEarning.Rows.Count > 0)
                {
                    ((ImageButton)grdEarning.Rows[0].FindControl("imbRuleUpEarn")).Visible = false;
                    ((ImageButton)grdEarning.Rows[grdEarning.Rows.Count - 1].FindControl("imbRuleDownEarn")).Visible = false;
                }
            }
            if (senderGridView.ID == "grdDeduction")
            {
                if (grdDeduction.Rows.Count > 0)
                {
                    ((ImageButton)grdDeduction.Rows[0].FindControl("imbRuleUpDedu")).Visible = false;
                    ((ImageButton)grdDeduction.Rows[grdDeduction.Rows.Count - 1].FindControl("imbRuleDownDedu")).Visible = false;
                }
            }
        }

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
                    #region SALARYTEMPLATE
                    case ControlsEnum.SALARYTEMPLATE:
                        if (SalaryTemplateViewState != null)
                        {
                            PayrollTypePk = SalaryTemplateViewState.STE_PAYROLL_TYPE;
                            txtTemplateCode.Text = SalaryTemplateViewState.STE_CODE.HtmlDecode();
                            txtTemplateName.Text = SalaryTemplateViewState.STE_NAME.HtmlDecode();
                            txtDescription.Text = SalaryTemplateViewState.STE_DESC.HtmlDecode();
                            chkActive.Checked = SalaryTemplateViewState.ACTIVE == 1 ? true : false;
                            GetFieldValues(ControlsEnum.PAYROLLTYPE);
                            SetFieldValues(ControlsEnum.PAYROLLTYPE);
                            if (Convert.ToInt32(SalaryTemplateViewState.STE_PAYROLL_TYPE) > 0)
                            {
                                ddlPayrollType.SelectedValue = SalaryTemplateViewState.STE_PAYROLL_TYPE.ToString();
                            }
                            LastModifiedTime = SalaryTemplateViewState.LAST_MOD_DT;
                            BindGrid(ControlsEnum.EARNING);
                            BindGrid(ControlsEnum.DEDUCTION);
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
                #region SALARYTEMPLATE
                case ControlsEnum.SALARYTEMPLATE:

                    List<SalaryTemplateBO.SalaryTemplateDetail> tempDetails = SalaryTemplateViewState.SalaryTemplateDetails;
                    if (tempDetails == null) tempDetails = new List<SalaryTemplateBO.SalaryTemplateDetail>();
                    foreach (SalaryTemplateBO.SalaryTemplateDetail item in tempDetails)
                        if (string.IsNullOrWhiteSpace(item.PayElementValue)) item.PayElementValue = item.PayElementDisplay;

                    SalaryTemplateBO.SalaryTemplate tempSalaryTemplate = SalaryTemplateViewState;
                    tempSalaryTemplate.STE_PK = tempSalaryTemplate.STE_PK;
                    tempSalaryTemplate.STE_CODE = txtTemplateCode.Text.Trim().HtmlEncode();
                    tempSalaryTemplate.STE_NAME = txtTemplateName.Text.Trim().HtmlEncode();
                    tempSalaryTemplate.STE_DESC = txtDescription.Text.HtmlEncode();
                    tempSalaryTemplate.STE_DEPT = currentUser.CurrentDeptPK;
                    tempSalaryTemplate.STE_PAYROLL_TYPE = Convert.ToInt32(ddlPayrollType.SelectedValue);
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempSalaryTemplate.STE_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempSalaryTemplate.BIZUNIT_PK = currentUser.SBUID;
                    tempSalaryTemplate.ACTIVE = Convert.ToInt32(chkActive.Checked == true ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO));
                    tempSalaryTemplate.USER_PK = currentUser.PKUser;
                    tempSalaryTemplate.LAST_MOD_DT = this.LastModifiedTime;
                    tempSalaryTemplate.SalaryTemplateDetails = tempDetails;
                    returnObject = tempSalaryTemplate;
                    break;
                #endregion
                #region SALARYTEMPLATE
                case ControlsEnum.EMPPAYELEMENTUPDATEALL:
                    SalaryTemplateBO.EmpSalaryTemplate objTemplate = new SalaryTemplateBO.EmpSalaryTemplate();
                    List<SalaryTemplateBO.EmpTemplateDetails> empSalTempList = new List<SalaryTemplateBO.EmpTemplateDetails>();
                    objTemplate.STE_PK = CurrPK;
                    objTemplate.STS_PK = TemplateDetailPk;
                    objTemplate.STS_CALC_VALUE = hdfModifiedPayElementValue.Value;
                    objTemplate.INSERT_NEW = chkInsertNew.Checked ? 1 : 0;
                    objTemplate.BIZUNIT_PK = currentUser.SBUID;
                    objTemplate.USER_PK = currentUser.PKUser;
                    objTemplate.LAST_MOD_DT = this.LastModifiedTime;

                    foreach (GridViewRow grvRow in grdEmpPaylement.Rows)
                    {
                        SalaryTemplateBO.EmpTemplateDetails objEmpTemplateDetails = new SalaryTemplateBO.EmpTemplateDetails();
                        Label lblEmpPayElement = (Label)grvRow.FindControl("lblEmpPayElement");
                        HiddenField hdfEmpPayElementValue = (HiddenField)grvRow.FindControl("hdfEmpPayElementValue");
                        CheckBox chkEmpPayelement = (CheckBox)grvRow.FindControl("chkEmpPayelement");
                        if (chkEmpPayelement.Checked)
                        {
                            objEmpTemplateDetails.STS_CALC_VALUE = hdfEmpPayElementValue.Value;
                            objEmpTemplateDetails.STS_CALC_VALUE_TEXT = lblEmpPayElement.ToolTip;
                            empSalTempList.Add(objEmpTemplateDetails);
                        }
                    }
                    objTemplate.EmpTemplateDetails = empSalTempList;
                    returnObject = objTemplate;
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
                #region AFTERGRIDVIEWDELETE
                case ControlsEnum.AFTERGRIDVIEWDELETE:
                    hdfSerialNo.Value = string.Empty;
                    ucPayItemControl1.SetData(string.Empty, string.Empty, -1, -1, -1, 0, 0);
                    break;
                #endregion
                #region CLEAR
                case ControlsEnum.CLEAR:
                    txtTemplateCode.Text = txtTemplateName.Text = txtFilterCode.Text = string.Empty;
                    txtTemplateCode.Focus();
                    chkActive.Checked = true;
                    SalaryTemplateViewState = null;
                    txtDescription.Text = string.Empty;
                    ddlPayrollType.ClearSelection();
                    CurrPK = 0;
                    ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                    BindGrid(ControlsEnum.EARNING);
                    BindGrid(ControlsEnum.DEDUCTION);
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtTemplateNameListPage.Text = txtFilterCode.Text = string.Empty;
                    ddlSrchPayrollType.ClearSelection();
                    break;
                #endregion
                #region EMP PAY ELEMENT UPDATE ALL
                case ControlsEnum.EMPPAYELEMENTUPDATEALL:
                    lblModifiedPayElementName.Text = string.Empty;
                    lblModifiedPayElementValue.Text = string.Empty;
                    TemplateDetailPk = 0;
                    hdfModifiedPayElementValue.Value = string.Empty;
                    chkInsertNew.Checked = false;
                    hdfSerialNo.Value = string.Empty;
                    ucPayItemControl1.SetData(string.Empty, string.Empty, -1, -1, -1, 0, 0);
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
        //public string GetFormattedNumber(object number)
        //{
        //    double num = 0;
        //    double.TryParse(Convert.ToString(number), out num);
        //    string format = "#0.0";
        //    string s = num.ToString(format);
        //    return s;
        //}

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfSalTempCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfSalTempCurrencyFormatWithComma.Value);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            CLEAR,
            ADDTOLIST,
            EARNING,
            DEDUCTION,
            AFTERGRIDVIEWDELETE,
            SALARYTEMPLATE,
            COMPANY,
            PAYELEMENTCONFIGURATION,
            LIST,
            CLEARSEARCH,
            PAYROLLTYPE,
            SEARCHPAYROLLTYPE,
            EMPPAYELEMENT,
            EMPPAYELEMENTUPDATEALL
        }
        #endregion

        #region PayCalculationModeEnum
        public enum PayCalculationModeEnum
        {
            FixedAmount,
            FormulaDependOnAnotherPayElement,
            None
        }
        #endregion

        #region  Helper Methods

        /// <summary>
        /// Set image css
        /// </summary>
        /// <param name="PayElementMode"></param>
        /// <param name="imgMode"></param>
        private void SetPayelementIconCss(int PayElementMode, HtmlControl imgMode)
        {
            switch (PayElementMode)
            {
                case (int)PayElementCalcMode.Custom:
                    imgMode.Attributes.Add("class", "custom-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Custom);
                    break;
                case (int)PayElementCalcMode.FixedAmount:
                    imgMode.Attributes.Add("class", "fixed-amount-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_FixedAmount);
                    break;
                case (int)PayElementCalcMode.Formula:
                    imgMode.Attributes.Add("class", "formula-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Formula);
                    break;
                case (int)PayElementCalcMode.Slab:
                    imgMode.Attributes.Add("class", "slab-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Slab);
                    break;
                default:
                    imgMode.Attributes.Add("class", "hide");
                    break;
            }
        }
        #endregion
    }
}


