using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using System.Xml;
//using BusinessObject.AccountManagement;
using BusinessObject.HRMS.Employee;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmployeeSalaryRevision : ERP.Store.UI.MyBasePage
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

        private List<EmployeeSalaryRevisionBO.RevisionHistory> RevisionHistoryListViewState
        {
            get
            {
                return ViewState["RevisionHistoryListViewState"] == null ? new List<EmployeeSalaryRevisionBO.RevisionHistory>() : (List<EmployeeSalaryRevisionBO.RevisionHistory>)ViewState["RevisionHistoryListViewState"];
            }
            set
            {
                ViewState["RevisionHistoryListViewState"] = value;
            }
        }


        //private SalaryTemplateBO.SalaryTemplate SalaryTemplateViewState
        //{
        //    get
        //    {
        //        return ViewState["SalaryTemplateViewState"] == null ? new SalaryTemplateBO.SalaryTemplate() : (SalaryTemplateBO.SalaryTemplate)ViewState["SalaryTemplateViewState"];
        //    }
        //    set
        //    {
        //        ViewState["SalaryTemplateViewState"] = value;
        //    }
        //}

        //private Dictionary<string, int> PayElementConfigurationViewState
        //{
        //    get
        //    {
        //        return this.ViewState["PayElementConfigurationViewState"] == null ? new Dictionary<string, int>() : (Dictionary<string, int>)(this.ViewState["PayElementConfigurationViewState"]);
        //    }
        //    set
        //    {
        //        this.ViewState["PayElementConfigurationViewState"] = value;
        //    }
        //}

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

        public int CurrPK
        {
            get
            {
                return Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CurrentPK]);
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.CurrentPK] = value;
            }
        }

        #endregion
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private EmployeeBasicInfomtn objEmployeeBasicInfo;
        private string EffectToDate;
        #endregion

        #region PageEvents

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
            //if (EntryStatus == EntryStatus.EDITMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            //}
            //else if (EntryStatus == EntryStatus.NEWMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            //}
            //else if (EntryStatus == EntryStatus.ENTRYMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            //}
            //else if (EntryStatus == EntryStatus.LISTMODE)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
            //    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
            //    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            //}
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
        }
        #endregion


        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));         
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);           
            this.btnCancel.Load += new EventHandler(btnAction_Load);
        }

        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
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
                //ucFormulaMaster.AfterApply += new EventHandler(ucFormulaMaster_AfterApply);
                //InitializeComponent();
                if (!IsPostBack)
                {
                    if (this.CurrPK == 0)
                    {
                        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        Session["SelectMessage"] = litErrorMsg.Text;
                        //Response.Redirect("~/Employees/EmployeeList.aspx");
                        Response.Redirect(Resources.PageURL.EmployeeList);
                    }


                    //uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.PAYCLASIFICATION);
                    SetFieldValues(ControlsEnum.PAYCLASIFICATION);
                    GetFieldValues(ControlsEnum.PAYELEMENTCONFIGURATION);
                    SetFieldValues(ControlsEnum.PAYELEMENTCONFIGURATION);
                    GetFieldValues(ControlsEnum.REVISIONHISTORY);
                    SetFieldValues(ControlsEnum.REVISIONHISTORY);

                    BindGrid(ControlsEnum.REVISIONHISTORY);
                }
                if (CurrPK > 0)
                {
                    GetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                    SetFieldValues(ControlsEnum.EMPLOYEEDETAILSHEADER);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            XmlDocument xmlDoc;
            bool bIsChecked = false;
            try
            {
                int? result = null;
                #region Getting Command Action
                BusinessObject.AccountManagement.ActionsEnum commonActions = BusinessObject.AccountManagement.ActionsEnum.UNKNOWN;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (BusinessObject.AccountManagement.ActionsEnum)(Enum.Parse(typeof(BusinessObject.AccountManagement.ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    //if ((((DropDownList)sender).ID == "ddlPayClassification"))
                    //{
                    //    commonActions = BusinessObject.AccountManagement.ActionsEnum.CHANGE;
                    //}
                    //if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    //{
                    //    commonActions = BusinessObject.AccountManagement.ActionsEnum.CHANGETYPE;
                    //}
                }

                #endregion
                switch (commonActions)
                {
                    #region SHOWPOPUP
                    case BusinessObject.AccountManagement.ActionsEnum.SHOWPOPUP:
                        //((Button)sender).CommandName = BusinessObject.AccountManagement.ActionsEnum.DEFAULT.ToString();
                        //ucFormulaMaster.ActionHandler(sender, e);      
                        //if (ucFormulaMaster.FormulaText != string.Empty) ucFormulaMaster.FormulaTextFromParent = ucFormulaMaster.FormulaText;

                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpFormula]','Formula','660','200');", true);
                        break;
                    #endregion
                    #region CHANGE
                    case BusinessObject.AccountManagement.ActionsEnum.CHANGE:
                        //if (ddlPayClassification.SelectedIndex > 0)
                        //{
                        //    GetFieldValues(ControlsEnum.PAYELEMENTS);
                        //    SetFieldValues(ControlsEnum.PAYELEMENTS);
                        //}
                        //else
                        //{
                        //    BindDropDown(ControlsEnum.PAYELEMENTS);
                        //}
                        break;
                    #endregion
                    #region ADDTOLIST
                    case BusinessObject.AccountManagement.ActionsEnum.ADDTOLIST:
                        //List<SalaryTemplateBO.SalaryTemplateDetail> tempSalaryTemplateDetailsList = SalaryTemplateViewState.SalaryTemplateDetails;
                        //if (tempSalaryTemplateDetailsList == null) tempSalaryTemplateDetailsList = new List<SalaryTemplateBO.SalaryTemplateDetail>();
                        //int payElementPk = GetNullableInt(ddlPayElement.SelectedValue).Value;
                        //string payCalculationModeText = string.Empty;
                        //int payCalculationMode = 0;
                        //if (rbtFormula.Checked)
                        //{
                        //    payCalculationModeText = PayCalculationModeEnum.FormulaDependOnAnotherPayElement.ToString();
                        //    payCalculationMode = PayElementConfigurationViewState["Formula"];
                        //}
                        //else if (rbtFixedAmount.Checked)
                        //{
                        //    payCalculationModeText = PayCalculationModeEnum.FixedAmount.ToString();
                        //    payCalculationMode = PayElementConfigurationViewState["Fixed Amount"];
                        //}

                        //// else if (rbtNone.Checked) payCalculationMode = PayCalculationModeEnum.None.ToString();

                        //SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplateDetailsObj = new SalaryTemplateBO.SalaryTemplateDetail();
                        //if ((GetNullableInt(hdfSerialNo.Value) ?? 0) == 0) // New
                        //{
                        //    tempSalaryTemplateDetailsObj = tempSalaryTemplateDetailsList
                        //        .Where(x => x.PayElementPk == payElementPk)
                        //        .SingleOrDefault();
                        //    if (tempSalaryTemplateDetailsObj != null)
                        //    {
                        //        litErrorMsg.Text = string.Format(GetLocalResourceObject("PayElementAlreadyExists").ToString(), tempSalaryTemplateDetailsObj.PayElementName);
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                        //            CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //        return;
                        //    }
                        //    int slNo = tempSalaryTemplateDetailsList.Count > 0 ? (tempSalaryTemplateDetailsList.Max(x => x.SlNo)) + 1 : 0;
                        //    if (slNo < 1) slNo = 1;
                        //    tempSalaryTemplateDetailsObj = new SalaryTemplateBO.SalaryTemplateDetail();
                        //    tempSalaryTemplateDetailsObj.SlNo = slNo;
                        //    tempSalaryTemplateDetailsObj.PayElementPk = payElementPk;
                        //    SalaryTemplateBO.PayElement payEelement = PayElementsViewState
                        //        .Where(x => x.PelPk == payElementPk)
                        //        .Single();

                        //    tempSalaryTemplateDetailsObj.PayClassificationPk = GetNullableInt(ddlPayClassification.SelectedValue).Value;
                        //    tempSalaryTemplateDetailsObj.PayElementName = ddlPayElement.SelectedItem.Text;
                        //    //tempSalaryTemplateObj.PayElementName = payEelement.PelName;
                        //    tempSalaryTemplateDetailsObj.PayElementDisplay = txtAmountOrFormula.Text.Trim().HtmlEncode();
                        //    tempSalaryTemplateDetailsObj.PayElementValue = hdfFormula.Value;
                        //    tempSalaryTemplateDetailsObj.PayElementDeduction = payEelement.PelDeduction;
                        //    tempSalaryTemplateDetailsObj.PayCalculationModeText = payCalculationModeText;
                        //    tempSalaryTemplateDetailsObj.PayCalculationMode = payCalculationMode;
                        //    if (!rbtFormula.Checked) tempSalaryTemplateDetailsObj.PayElementValue = tempSalaryTemplateDetailsObj.PayElementDisplay;
                        //    tempSalaryTemplateDetailsList.Add(tempSalaryTemplateDetailsObj);
                        //}
                        //else // Edit
                        //{
                        //    int slNo = GetNullableInt(hdfSerialNo.Value).Value;
                        //    tempSalaryTemplateDetailsObj = tempSalaryTemplateDetailsList
                        //        .Where(x => x.PayElementPk == payElementPk && x.SlNo != slNo)
                        //        .SingleOrDefault();
                        //    if (tempSalaryTemplateDetailsObj != null)
                        //    {
                        //        litErrorMsg.Text = string.Format(GetLocalResourceObject("PayElementAlreadyExists").ToString(), tempSalaryTemplateDetailsObj.PayElementName);
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                        //            CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //        return;
                        //    }

                        //    tempSalaryTemplateDetailsObj = tempSalaryTemplateDetailsList
                        //         .Where(x => x.SlNo == slNo)
                        //         .Single();
                        //    tempSalaryTemplateDetailsObj.PayElementPk = payElementPk;
                        //    SalaryTemplateBO.PayElement payEelement = PayElementsViewState
                        //        .Where(x => x.PelPk == payElementPk)
                        //        .Single();
                        //    tempSalaryTemplateDetailsObj.PayClassificationPk = GetNullableInt(ddlPayClassification.SelectedValue).Value;
                        //    tempSalaryTemplateDetailsObj.PayElementName = ddlPayElement.SelectedItem.Text;
                        //    //tempSalaryTemplateObj.PayElementName = payEelement.PelName;
                        //    tempSalaryTemplateDetailsObj.PayElementDisplay = txtAmountOrFormula.Text.Trim().HtmlEncode();
                        //    tempSalaryTemplateDetailsObj.PayElementValue = hdfFormula.Value;
                        //    tempSalaryTemplateDetailsObj.PayElementDeduction = payEelement.PelDeduction;
                        //    tempSalaryTemplateDetailsObj.PayCalculationModeText = payCalculationModeText;
                        //    tempSalaryTemplateDetailsObj.PayCalculationMode = payCalculationMode;
                        //    if (!rbtFormula.Checked) tempSalaryTemplateDetailsObj.PayElementValue = tempSalaryTemplateDetailsObj.PayElementDisplay;
                        //}
                        //SalaryTemplateBO.SalaryTemplate tempSalaryTemplate = SalaryTemplateViewState;
                        //tempSalaryTemplate.SalaryTemplateDetails = tempSalaryTemplateDetailsList;
                        //SalaryTemplateViewState = tempSalaryTemplate;
                        //BindGrid(ControlsEnum.EARNING);
                        //BindGrid(ControlsEnum.DEDUCTION);
                        //ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
                        break;
                    #endregion
                    #region SAVE
                    case BusinessObject.AccountManagement.ActionsEnum.SAVE:
                        //SalaryTemplateViewState = (SalaryTemplateBO.SalaryTemplate)SetUIValuesToObject(ControlsEnum.SALARYTEMPLATE);
                        //if (SalaryTemplateViewState.SalaryTemplateDetails.Count == 0)
                        //{
                        //    litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.Captions.Information + "');", true);
                        //    return;
                        //}

                        //xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(SalaryTemplateViewState);
                        //result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.SaveSalaryTemplate(xmlDoc.InnerXml);
                        //if (result > 0)
                        //{
                        //    ResetForm(ControlsEnum.CLEAR);
                        //    GetFieldValues(ControlsEnum.LIST);
                        //    SetFieldValues(ControlsEnum.LIST);
                        //    this.EntryStatus = EntryStatus.LISTMODE;
                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        //}
                        //else
                        //{
                        //    if (result == (int)DbSaveStatus.SQLERROR)
                        //    {
                        //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.Captions.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.EditUsedByAnotherUser;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.Captions.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.AlreadyDeleted;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //            + "','" + Resources.Captions.Information + "');", true);
                        //        EntryStatus = EntryStatus.LISTMODE;
                        //    }
                        //    else if (result == (int)DbSaveStatus.CODEEXIST)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.Captions.Information + "','" + "');", true);
                        //    }
                        //    else
                        //    {
                        //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.Captions.Information + "');", true);
                        //    }
                        //}
                        break;
                    #endregion
                    //#region LIST
                    //case BusinessObject.AccountManagement.ActionsEnum.LIST:
                    //    GetFieldValues(ControlsEnum.LIST);
                    //    SetFieldValues(ControlsEnum.LIST);
                    //    break;
                    //#endregion
                    //#region LIST, CANCEL
                    //case BusinessObject.AccountManagement.ActionsEnum.LIST:
                    //case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                    //    EntryStatus = EntryStatus.LISTMODE;
                    //    //ResetForm(ControlsEnum.CLEAR);
                    //    ////ResetForm(ControlsEnum.CLEARSEARCH);
                    //    //GetFieldValues(ControlsEnum.LIST);
                    //    //SetFieldValues(ControlsEnum.LIST);
                    //    break;
                    //#endregion
                    #region CANCEL
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        CurrPK = 0;
                        Session["SalaryRevisionCancelClicked"] = 1;
                        Response.Redirect("~/Employees/EmployeeList.aspx");
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case BusinessObject.AccountManagement.ActionsEnum.EDIT:
                    case BusinessObject.AccountManagement.ActionsEnum.DETAIL:
                        //foreach (GridViewRow grdrow in grdList.Rows)
                        //{
                        //    RadioButton rbtn;
                        //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //    if (rbtn.Checked)
                        //    {
                        //        bIsChecked = true;
                        //        ResetForm(ControlsEnum.CLEAR);
                        //        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfTemplatePkListPage")).Value);
                        //        GetFieldValues(ControlsEnum.SALARYTEMPLATE);
                        //        SetFieldValues(ControlsEnum.SALARYTEMPLATE);
                        //        break;
                        //    }
                        //}
                        //if (bIsChecked)
                        //{

                        //    EntryStatus = EntryStatus.EDITMODE;
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        break;
                    #endregion
                    #region NEW
                    case BusinessObject.AccountManagement.ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        ////SalaryTemplateViewState = new SalaryTemplateBO.SalaryTemplate();
                        //ResetForm(ControlsEnum.CLEAR);
                        break;
                    #endregion
                    #region DELETE
                    case BusinessObject.AccountManagement.ActionsEnum.DELETE:
                        //result = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.DeleteSalaryTemplate(CurrPK, SalaryTemplateViewState.LAST_MOD_DT);
                        //if (result > 0)
                        //{
                        //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "');", true);
                        //    ActionHandler(lnkList, EventArgs.Empty);
                        //}
                        //else
                        //{
                        //    if (result == (int)DbSaveStatus.REFERRED)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate;
                        //        litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.SQLERROR)
                        //    {
                        //        litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " +
                        //            GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.CODEEXIST)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " +
                        //            GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.ALREADYDELETED)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.SalaryTemplate + " " +
                        //            GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        //    }
                        //    else
                        //    {
                        //        litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                        //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SalaryTemplate);
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //}
                        break;
                    #endregion
                    #region FILTER
                    case BusinessObject.AccountManagement.ActionsEnum.FILTER:
                        //uclPaging.CurrentPage = 0;
                        //this.PageIndexList = "1";
                        //this.EntryStatus = EntryStatus.LISTMODE;
                        //this.CurrPK = 0;
                        //GetFieldValues(ControlsEnum.LIST);
                        //SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case BusinessObject.AccountManagement.ActionsEnum.CLEAR:
                        //ResetForm(ControlsEnum.CLEARSEARCH);
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
            if (senderGridView.ID == "grdRevisionHistory")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfEffectTo = (HiddenField)row.FindControl("hdfEffectTo");
                    HiddenField hdfEDP_EFFECT_FROM = (HiddenField)row.FindControl("hdfEDP_EFFECT_FROM");

                    EffectToDate = string.IsNullOrEmpty(hdfEDP_EFFECT_FROM.Value) ? string.Empty : hdfEDP_EFFECT_FROM.Value;
                    GetFieldValues(ControlsEnum.EMPLOYEESALARY);
                    SetFieldValues(ControlsEnum.SALARYTEMPLATEDETAILS);
                    //HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    //HiddenField hdfPk = row.FindControl("hdfPk") as HiddenField;
                    //// HiddenField hdfPayElementPk = row.FindControl("hdfPayElementPk") as HiddenField;
                    //hdfSerialNo.Value = hdfSlNo.Value;
                    //slNo = GetNullableInt(hdfSlNo.Value).Value;

                    //SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplateDetails = SalaryTemplateViewState.SalaryTemplateDetails
                    //    .Where(x => x.SlNo == slNo)
                    //    .Single();

                    //ddlPayClassification.SelectedIndex = ddlPayClassification.Items.IndexOf(ddlPayClassification.Items.FindByValue(tempSalaryTemplateDetails.PayClassificationPk.ToString()));
                    //ActionHandler(ddlPayClassification, EventArgs.Empty);
                    //ddlPayElement.SelectedIndex = ddlPayElement.Items.IndexOf(ddlPayElement.Items.FindByValue(tempSalaryTemplateDetails.PayElementPk.ToString()));
                    //txtAmountOrFormula.Text = tempSalaryTemplateDetails.PayElementDisplay;
                    //hdfFormula.Value = tempSalaryTemplateDetails.PayElementValue;
                    //if (tempSalaryTemplateDetails.PayCalculationMode == 0)// PayCalculationModeEnum.FixedAmount.ToString())
                    //{
                    //    rbtFixedAmount.Checked = true;
                    //}
                    //else if (tempSalaryTemplateDetails.PayCalculationMode == 1)// PayCalculationModeEnum.FormulaDependOnAnotherPayElement.ToString())
                    //{
                    //    rbtFormula.Checked = true;
                    //    //ucFormulaMaster.FormulaText = txtAmountOrFormula.Text;
                    //    //ucFormulaMaster.FormulaValue = hdfFormula.Value;
                    //    ucFormulaMaster.SetData(txtAmountOrFormula.Text.Trim());
                    //}
                    ////else if (tempSalaryTemplate.PayCalculationMode == PayCalculationModeEnum.None.ToString())
                    ////{
                    ////    rbtNone.Checked = true;
                    ////}                   
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    //HiddenField hdfSlNo = row.FindControl("hdfSlNo") as HiddenField;
                    //HiddenField hdfPk = row.FindControl("hdfPk") as HiddenField;
                    //// HiddenField hdfPayElementPk = row.FindControl("hdfPayElementPk") as HiddenField;
                    //hdfSerialNo.Value = string.Empty;
                    //slNo = GetNullableInt(hdfSlNo.Value).Value;

                    //SalaryTemplateBO.SalaryTemplate tempSalaryTemplateViewState = SalaryTemplateViewState;
                    //List<SalaryTemplateBO.SalaryTemplateDetail> tempList = tempSalaryTemplateViewState.SalaryTemplateDetails;
                    //SalaryTemplateBO.SalaryTemplateDetail tempSalaryTemplate = tempSalaryTemplateViewState.SalaryTemplateDetails
                    //    .Where(x => x.SlNo == slNo)
                    //    .Single();
                    //tempList.Remove(tempSalaryTemplate);
                    //tempSalaryTemplateViewState.SalaryTemplateDetails = tempList;
                    //BindGrid(ControlsEnum.EARNING);
                    //BindGrid(ControlsEnum.DEDUCTION);
                    //ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
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
            //BusinessObject.GridPrams gridParam;
            try
            {
                switch (type)
                {
                    #region EMPLOYEEDETAILSHEADER
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        objEmployeeBasicInfo = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;
                    #endregion
                    #region EMPLOYEESALARY
                    case ControlsEnum.EMPLOYEESALARY: // Get employee salary details
                        UCEmpSalary.EmployeePK = CurrPK;
                        UCEmpSalary.EffectTo = EffectToDate;
                        UCEmpSalary.GetFieldValues(ActionsEnum.EMPLOYEESALARY);
                        break;
                    #endregion
                    #region REVISIONHISTORY
                    case ControlsEnum.REVISIONHISTORY:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeSalaryRevisionBL.GetEmployeeSalaryHistory(CurrPK);
                        break;
                    #endregion
                    //        #region PAYELEMENTS
                    //        case ControlsEnum.PAYELEMENTS:
                    //            int pelClass = (GetNullableInt(ddlPayClassification.SelectedValue) ?? 0);
                    //            dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(0, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, 0, 0, pelClass);
                    //            // BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("PAY ELEMENT CLASSIFICATIONS", "", currentUser.SBUID);
                    //            break;
                    //        #endregion
                    //        #region COMPANY
                    //        case ControlsEnum.COMPANY:
                    //            dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                    //            break;
                    //        #endregion
                    //        #region PAYELEMENTCONFIGURATION
                    //        case ControlsEnum.PAYELEMENTCONFIGURATION:
                    //            dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAY ELEMENT CALCULATION MODE");
                    //            break;
                    //        #endregion
                    //        #region LIST
                    //        case ControlsEnum.LIST:
                    //            gridParam = new BusinessObject.GridPrams();
                    //            gridParam.SearchBy = string.Empty;
                    //            gridParam.SearchValue = txtTemplateNameListPage.Text.Trim();
                    //            gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                    //            gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    //            // gridParam.Fields = GTIService.Constants.DirectStockTransfer.Parameters.GridParmeters;
                    //            //"[GRH_PK],[GRH_NO],[GRH_DATE],[GRH_STATUS],[GRH_STATUS_TEXT],[REF_ID],[DPT_NAME],[GRH_VENDOR_TEXT],[GRH_PO_NO],[GRH_VND_REF_NO]";
                    //            // gridParam.SortBy = GTIService.Constants.DirectStockTransfer.Fields.GRH_PK_SortBy;
                    //            //  gridParam.SortDirection = "DESC";
                    //            //gridParam.FromDate = txtFromDate.Text;
                    //            //gridParam.ToDate = txtToDate.Text;
                    //            //gridParam.FilterStatus = ddlStatus.SelectedValue;                     

                    //            dsPageData = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.GetSalaryTemplateList(gridParam, currentUser.SBUID);
                    //            break;
                    //        #endregion
                    //        #region SALARYTEMPLATE
                    //        case ControlsEnum.SALARYTEMPLATE:
                    //            string xmlData = BusinessLogic.HRMS.Admin.Masters.SalaryTemplateBL.GetSalaryTemplate(CurrPK);
                    //            SalaryTemplateBO.SalaryTemplate tempSalaryTemplate;
                    //            if (xmlData == "<Root/>")
                    //            {
                    //                tempSalaryTemplate = new SalaryTemplateBO.SalaryTemplate();
                    //            }
                    //            else
                    //            {
                    //                tempSalaryTemplate = CommonFunctions.XmlDeserialize<SalaryTemplateBO.SalaryTemplate>(xmlData);
                    //            }
                    //            int slNo = 0;
                    //            foreach (var item in tempSalaryTemplate.SalaryTemplateDetails)
                    //            {
                    //                item.SlNo = ++slNo;
                    //                if (string.IsNullOrWhiteSpace(item.PayElementDisplay))
                    //                {
                    //                    item.PayElementDisplay = item.PayElementValue;
                    //                }
                    //            }
                    //            SalaryTemplateViewState = tempSalaryTemplate;
                    //            break;
                    //        #endregion

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
                    #region SALARYTEMPLATEDETAILS
                    case ControlsEnum.SALARYTEMPLATEDETAILS:
                        UCEmpSalary.SetFieldValues(ActionsEnum.EMPLOYEESALARY);
                        break;
                    #endregion
                    #region EMPLOYEEDETAILSHEADER
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region REVISIONHISTORY
                    case ControlsEnum.REVISIONHISTORY:
                        BindGrid(ControlsEnum.REVISIONHISTORY);

                        //if (txtDateFrom.Text != string.Empty) dtFromDate = Convert.ToDateTime(txtDateFrom.Text);
                        //if (txtDate.Text != string.Empty) dtToDate = Convert.ToDateTime(txtDate.Text);
                        //int? designation = GetNullableInt(hdfDesignation.Value);
                        //int? branchLocation = GetNullableInt(hdfBranchLocation.Value);
                        //int? department = GetNullableInt(hdfDepartment.Value);
                        //int? employeeType = GetNullableInt(ddlEmploymentType.SelectedValue) == -1 ? null : GetNullableInt(ddlEmploymentType.SelectedValue);
                        //int? company = GetNullableInt(ddlCompany.SelectedValue) == -1 ? null : GetNullableInt(ddlCompany.SelectedValue);
                        //int? employee = GetNullableInt(ddlEmployee.SelectedValue) < 1 ? null : GetNullableInt(ddlEmployee.SelectedValue);
                        //dtResult = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendance(null, 1, currentUser.SBUID, dtToDate, designation, branchLocation, department, employeeType, company, employee, dtFromDate);

                        //List<AttendancePopupDetails> tempList = new List<AttendancePopupDetails>();
                        //foreach (DataRow row in dtResult.Rows)
                        //{
                        //    AttendancePopupDetails attendance = new AttendancePopupDetails();
                        //    attendance.SlNo = 0;
                        //    attendance.Pk = GetNullableInt(Convert.ToString(row["EAT_PK"]));
                        //    attendance.EmpPk = GetNullableInt(Convert.ToString(row["EAT_EMPLOYEE_PK"])).Value;
                        //    attendance.WorkingHrs = GetNullableDecimal(Convert.ToString(row["EAT_WORK_HRS"]));
                        //    attendance.DateDt = GetNullableDate(Convert.ToString(row["EAT_DATE"]));
                        //    attendance.EmpCode = (Convert.ToString(row["EAT_EMPLOYEE_TEXT"])).HtmlDecode();
                        //    attendance.EmpName = (Convert.ToString(row["EAT_EMPLOYEE_NAME"])).HtmlDecode();
                        //    attendance.Location = (Convert.ToString(row["empBranchText"])).HtmlDecode();
                        //    attendance.InDt = GetNullableDate(Convert.ToString(row["EAT_IN_TIME"]));
                        //    attendance.OutDt = GetNullableDate(Convert.ToString(row["EAT_OUT_TIME"]));
                        //    attendance.TotalDt = GetNullableDate(Convert.ToString(row["EAT_NORMAL_HRS"]));
                        //    attendance.TotalHrs = GetNullableDecimal(Convert.ToString(row["EAT_NORMAL_HRS"]));
                        //    attendance.ShortHrs = GetNullableDecimal(Convert.ToString(row["EAT_SHORT_HRS"]));
                        //    attendance.OTDtHrs = GetNullableDecimal(Convert.ToString(row["EAT_OT_HRS"]));
                        //    attendance.CheckedFlag = 0;
                        //    tempList.Add(attendance);
                        //}
                        //AttendanceEntryGridViewList = tempList;



                        break;
                    #endregion
                    //    #region PAYELEMENTS
                    //    case ControlsEnum.PAYELEMENTS:
                    //        BindDropDown(ControlsEnum.PAYELEMENTS);
                    //        List<SalaryTemplateBO.PayElement> tempPayElementList = new List<SalaryTemplateBO.PayElement>();
                    //        foreach (DataRow row in dtResult.Rows)
                    //        {
                    //            SalaryTemplateBO.PayElement element = new SalaryTemplateBO.PayElement();
                    //            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK] != null)
                    //                element.PelPk = Convert.ToInt32(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK]);
                    //            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_CODE] != null)
                    //                element.PelCode = Convert.ToString(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_CODE]).HtmlDecode();
                    //            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME] != null)
                    //                element.PelName = Convert.ToString(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME]).HtmlDecode();
                    //            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_FORMULA_CODE] != null)
                    //                element.PelFormulaCode = Convert.ToString(row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_FORMULA_CODE]).HtmlDecode();
                    //            if (row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_IS_DEDUCTION] != null)
                    //                element.PelDeduction = (GetNullableInt((row[GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_IS_DEDUCTION]).ToString()) ?? 0) == 0 ? false : true;
                    //            tempPayElementList.Add(element);
                    //        }
                    //        PayElementsViewState = tempPayElementList;
                    //        break;
                    //    #endregion
                    //    #region PAYELEMENTCONFIGURATION
                    //    case ControlsEnum.PAYELEMENTCONFIGURATION:
                    //        Dictionary<string, int> tempPayElementConfiguration = new Dictionary<string, int>();
                    //        foreach (DataRow row in dtResult.Rows)
                    //        {
                    //            if (row["CFG_DATA"] != null && row["CFG_VALUE"] != null)
                    //                tempPayElementConfiguration.Add(row["CFG_DATA"].ToString(), GetNullableInt(row["CFG_VALUE"].ToString()).Value);
                    //        }
                    //        PayElementConfigurationViewState = tempPayElementConfiguration;
                    //        break;
                    //    #endregion
                    //    #region LIST
                    //    case ControlsEnum.LIST:
                    //        BindGrid(ControlsEnum.LIST);
                    //        break;
                    //    #endregion
                    //    #region SALARYTEMPLATE
                    //    case ControlsEnum.SALARYTEMPLATE:
                    //        GetUIValuesFromObject(ControlsEnum.SALARYTEMPLATE);
                    //        break;
                    //    #endregion
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
            //switch (controlType)
            //{
            //    #region PAYCLASIFICATION
            //    case ControlsEnum.PAYCLASIFICATION:
            //        ddlPayClassification.Items.Clear();
            //        ddlPayClassification.DataSource = dtResult;
            //        ddlPayClassification.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
            //        ddlPayClassification.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
            //        ddlPayClassification.DataBind();
            //        ddlPayClassification.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            //        ddlPayClassification.Items.HtmlDecode();
            //        break;
            //    #endregion
            //    #region PAYELEMENTS
            //    case ControlsEnum.PAYELEMENTS:
            //        ddlPayElement.Items.Clear();
            //        if (dtResult != null)
            //        {
            //            ddlPayElement.DataSource = dtResult;
            //            ddlPayElement.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
            //            ddlPayElement.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;

            //        }
            //        else
            //        {
            //            ddlPayElement.DataSource = null;
            //        }
            //        ddlPayElement.DataBind();
            //        ddlPayElement.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
            //        ddlPayElement.Items.HtmlDecode();
            //        break;
            //    #endregion
            //}
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
                    #region REVISIONHISTORY
                    case ControlsEnum.REVISIONHISTORY:
                        //List<EmployeeSalaryRevisionBO.RevisionHistory> tempList = RevisionHistoryListViewState;
                        //tempList.Add(new EmployeeSalaryRevisionBO.RevisionHistory
                        //{
                        //    SlNo = 1,
                        //    Pk = 1,
                        //    EffectivePeriod = "P1-P2",
                        //    CTC = 1000,
                        //    PreviousCTC = 950,
                        //    Designation = "Des1",
                        //    Department = "Dep1"
                        //}
                        //);
                        //tempList.Add(new EmployeeSalaryRevisionBO.RevisionHistory
                        //{
                        //    SlNo = 2,
                        //    Pk = 2,
                        //    EffectivePeriod = "P2-P3",
                        //    CTC = 1500,
                        //    PreviousCTC = 1000,
                        //    Designation = "Des2",
                        //    Department = "Dep2"
                        //}
                        //);
                        //RevisionHistoryListViewState = tempList;
                        //if (RevisionHistoryListViewState != null && RevisionHistoryListViewState.Count > 0)
                        //{
                        //    grdRevisionHistory.DataSource = RevisionHistoryListViewState;
                        //    grdRevisionHistory.DataBind();
                        //}
                        //else
                        //{
                        //    grdRevisionHistory.DataSource = null;
                        //    grdRevisionHistory.DataBind();
                        //}

                        grdRevisionHistory.DataSource = dtResult;
                        grdRevisionHistory.DataBind();

                        break;
                    #endregion
                    //    #region EARNING
                    //    case ControlsEnum.DEDUCTION:
                    //        if (SalaryTemplateViewState.SalaryTemplateDetails != null && SalaryTemplateViewState.SalaryTemplateDetails.Count > 0)
                    //        {
                    //            grdDeduction.DataSource = SalaryTemplateViewState.SalaryTemplateDetails
                    //                .Where(x => x.PayElementDeduction == true);
                    //            //.Where(x => x.IS_DELETED == 0);
                    //            grdDeduction.DataBind();
                    //        }
                    //        else
                    //        {
                    //            grdDeduction.DataSource = null;
                    //            grdDeduction.DataBind();
                    //        }
                    //        break;
                    //    #endregion
                    //    #region LIST
                    //    case ControlsEnum.LIST:
                    //        int rowCount = 0;
                    //        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    //        if (dsPageData.Tables[0].Rows.Count > 0)
                    //        {
                    //            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    //        }
                    //        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                    //                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                    //                      (rowCount / pageSize) + 1;
                    //        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                    //        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                    //        grdList.DataSource = dsPageData.Tables[0];
                    //        grdList.DataBind();
                    //        uclPaging.Visible = true;
                    //        uclPaging.BindPager();
                    //        break;
                    //    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region grdRevisionHistory_DataBound
        //protected void grdRevisionHistory_DataBound(object sender, EventArgs e)
        //{
        //    if (grdRevisionHistory.Rows.Count > 0) ((Panel)grdRevisionHistory.Rows[0].FindControl("pnlAction")).Visible = true;
        //}
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
                    #region EMPLOYEEDETAILSHEADER
                    case ControlsEnum.EMPLOYEEDETAILSHEADER:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;                           
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                            //lblhdrEmployeeNoTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode), 20);
                            //lblhdrEmployeeNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText), 20);
                            //if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDOJText))
                            //{
                            //    string dojText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText).Trim(); ;
                            //    if (!dojText.IsNullOrEmptyOrWhitespace() && dojText[dojText.Length - 2] == ' ')
                            //    {
                            //        dojText = dojText.Remove(dojText.Length - 2, 1);
                            //    }
                            //    lblhdrDOJText.Text = dojText;
                            //}
                            //if (!string.IsNullOrEmpty(objEmployeeBasicInfo.empDOBText))
                            //{
                            //    string dobText = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText).Trim();
                            //    if (!dobText.IsNullOrEmptyOrWhitespace() && dobText[dobText.Length - 2] == ' ')
                            //    {
                            //        dobText = dobText.Remove(dobText.Length - 2, 1);
                            //    }
                            //    lblhdrDOBTxt.Text = dobText;
                            //}
                            //lblhdrDesignationTxt.Text = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText), 20);
                            //lblhdrEmployeeNoTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empCode);
                            //lblhdrEmployeeNameTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empNameText);
                            //lblhdrDOJText.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOJText);
                            //lblhdrDOBTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDOBText);
                            //lblhdrDesignationTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDesignationText);
                            //lblhdrDepartmentTxt.ToolTip = HttpUtility.HtmlDecode(objEmployeeBasicInfo.empDepartmentText);
                        }
                        break;
                    #endregion

                    //    #region PAYCLASIFICATION
                    //    case ControlsEnum.PAYCLASIFICATION:
                    //        BindDropDown(ControlsEnum.PAYCLASIFICATION);
                    //        break;
                    //    #endregion
                    //    #region SALARYTEMPLATE
                    //    case ControlsEnum.SALARYTEMPLATE:

                    //        txtTemplateCode.Text = SalaryTemplateViewState.STE_CODE.HtmlDecode();
                    //        txtTemplateName.Text = SalaryTemplateViewState.STE_NAME.HtmlDecode();
                    //        BindGrid(ControlsEnum.EARNING);
                    //        BindGrid(ControlsEnum.DEDUCTION);
                    //        break;
                    //    #endregion
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
            //switch (controlType)
            //{
            //    #region SALARYTEMPLATE
            //    case ControlsEnum.SALARYTEMPLATE:

            //        List<SalaryTemplateBO.SalaryTemplateDetail> tempDetails = SalaryTemplateViewState.SalaryTemplateDetails;
            //        if (tempDetails == null) tempDetails = new List<SalaryTemplateBO.SalaryTemplateDetail>();
            //        foreach (SalaryTemplateBO.SalaryTemplateDetail item in tempDetails)
            //            if (string.IsNullOrWhiteSpace(item.PayElementValue)) item.PayElementValue = item.PayElementDisplay;

            //        SalaryTemplateBO.SalaryTemplate tempSalaryTemplate = SalaryTemplateViewState;
            //        tempSalaryTemplate.STE_PK = (tempSalaryTemplate.STE_PK == null ? 0 : tempSalaryTemplate.STE_PK);
            //        tempSalaryTemplate.STE_CODE = txtTemplateCode.Text.Trim().HtmlEncode();
            //        tempSalaryTemplate.STE_NAME = txtTemplateName.Text.Trim().HtmlEncode();
            //        tempSalaryTemplate.STE_DEPT = currentUser.CurrentDeptPK;
            //        GetFieldValues(ControlsEnum.COMPANY);
            //        tempSalaryTemplate.STE_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
            //        tempSalaryTemplate.BIZUNIT_PK = currentUser.SBUID;
            //        tempSalaryTemplate.ACTIVE = 1;
            //        tempSalaryTemplate.USER_PK = currentUser.PKUser;
            //        tempSalaryTemplate.SalaryTemplateDetails = tempDetails;
            //        returnObject = tempSalaryTemplate;
            //        break;
            //    #endregion
            //}
            return returnObject;
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            //switch (controlType)
            //{
            //    #region AFTERGRIDVIEWDELETE
            //    case ControlsEnum.AFTERGRIDVIEWDELETE:
            //        rbtFixedAmount.Checked = true;
            //        rbtFormula.Checked = false;
            //        //rbtNone.Checked = false;
            //        txtAmountOrFormula.Text = hdfFormula.Value = hdfSerialNo.Value = string.Empty;
            //        ucFormulaMaster.FormulaText = ucFormulaMaster.FormulaValue = string.Empty;
            //        ddlPayClassification.SelectedIndex = 0;
            //        ActionHandler(ddlPayClassification, EventArgs.Empty);
            //        ucFormulaMaster.ResetForm();
            //        break;
            //    #endregion
            //    #region CLEAR
            //    case ControlsEnum.CLEAR:
            //        txtTemplateCode.Text = txtTemplateName.Text = string.Empty;
            //        SalaryTemplateViewState = null;
            //        CurrPK = 0;
            //        ResetForm(ControlsEnum.AFTERGRIDVIEWDELETE);
            //        BindGrid(ControlsEnum.EARNING);
            //        BindGrid(ControlsEnum.DEDUCTION);
            //        break;
            //    #endregion
            //    #region CLEARSEARCH
            //    case ControlsEnum.CLEARSEARCH:
            //        txtTemplateNameListPage.Text = string.Empty;
            //        break;
            //    #endregion
            //}
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
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            REVISIONHISTORY,
            EMPLOYEESALARY,
            SALARYTEMPLATEDETAILS,
            EMPLOYEEDETAILSHEADER,


            PAYCLASIFICATION,
            PAYELEMENTS,
            CLEAR,
            ADDTOLIST,
            EARNING,
            DEDUCTION,
            AFTERGRIDVIEWDELETE,
            SALARYTEMPLATE,
            COMPANY,
            PAYELEMENTCONFIGURATION,
            LIST,
            CLEARSEARCH
        }
        #endregion

    }
}