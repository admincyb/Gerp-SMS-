using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using System.Xml;
using BusinessObject.HRMS.Payroll;
using BusinessObject.CommonManagement;
using BusinessLogic.HRMS.Payroll;
using System.Threading;
using BusinessLogic.HRMS.Employee;
using System.Reflection;
using System.ComponentModel;
using BusinessLogic.CommonManagement;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class Income_Tax_01 : ERP.Store.UI.MyBasePage //System.Web.UI.Page 
    {
        #region Variables and Properties
        #region Variables
        private ActionsEnum commonActions;
        private ERPData.ADM_COMPANY_MST admCompanyMstObj;
        private List<ERPData.ADM_COMPANY_MST> admCompanyMstList;
        private Income_Tax_01BO.Tax01_PayrollDetails objEmpPayHeader;
        private BusinessObject.HRMS.Employee.EmployeeBasicInfomtn objEmployeeBasicInfo;
        User currentUser;
        int sectionIndex = 0;
        private DataSet dsList;
        #endregion
        #region Properties
        private int CurrEmployeePayrollPK
        {
            get
            {
                return Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.CurrEmployeePayrollPK]);
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.CurrEmployeePayrollPK] = value;
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
        private List<Income_Tax_01BO.Items> IncomeTax_ItemsList
        {
            get
            {
                return ViewState["IncomeTax_Items"] == null ? new List<Income_Tax_01BO.Items>() : (List<Income_Tax_01BO.Items>)ViewState["IncomeTax_Items"];
            }
            set
            {
                ViewState["IncomeTax_Items"] = value;
            }
        }
        private List<Income_Tax_01BO.Section> IncomeTax_SectionsList
        {
            get
            {
                return ViewState["IncomeTax_Sections"] == null ? new List<Income_Tax_01BO.Section>() : (List<Income_Tax_01BO.Section>)ViewState["IncomeTax_Sections"];
            }
            set
            {
                ViewState["IncomeTax_Sections"] = value;
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
        /// EmpPK
        /// </summary>
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

        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatusHdr
        {
            get
            {
                return this.Session[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.Session[ViewstateStrings.EntryState]);
            }
            set
            {
                this.Session[ViewstateStrings.EntryState] = value;
            }
        }
        #endregion
        #endregion

        #region PageEvents
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }
        #endregion
        #region Page Action Handler
        private void PageActionHandler()
        {
            try
            {
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                }
                if (CurrEmployeePayrollPK > 0)
                {
                    pnlPrint.Visible = true;
                    GetFieldValues(ControlsEnum.EMPPAYROLLDETAILLIST);
                    GetUIValuesFromObject(ControlsEnum.EMPPAYROLLDETAILLIST);
                    lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",    "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    hdfPageFlag.Value = PageEnum.Payroll.ToString();
                }
                else
                {
                    pnlPrint.Visible = false;
                    hdfPageFlag.Value = PageEnum.EmployeeMaster.ToString();
                    if (this.CurrPK != 0)
                    {
                        hdfEmployeePK.Value = Convert.ToString(CurrPK);
                        GetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILSBYID);
                    }
                    else if (this.CurrPK == 0)
                    {
                        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        Session["SelectMessage"] = litErrorMsg.Text;
                        Response.Redirect(Resources.PageURL.HrmsEmpListing);
                    }
                    lblBreadCrum.Text = GetLocalResourceObject("BreadcrumbIT").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                }

                GetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                SetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Page PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);

            if (hdfPageFlag.Value == PageEnum.Payroll.ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkIncomeTax.CssClass = GetLocalResourceObject("TabActive").ToString();
                pnlSaveAndContinue.Visible = false;
            }
            else if (hdfPageFlag.Value == PageEnum.EmployeeMaster.ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                pnlSaveAndContinue.Visible = true;
            }
            if (EntryStatusHdr == EntryStatus.VIEWMODE)
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);


        }
        #endregion
        #region OnLoadComplete
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (hdfPageFlag.Value == PageEnum.Payroll.ToString())
            {
                lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                this.Title = Resources.Captions.Title_PayrollProcess;
            }
            else if(hdfPageFlag.Value == PageEnum.EmployeeMaster.ToString())
            {
                lblBreadCrum.Text = GetLocalResourceObject("BreadcrumbIT").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                this.Title = Resources.Captions.Title_ITDeclaration;
            }
        }
        #endregion

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
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {

            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
        }
        #endregion

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                int? result = null;
                XmlDocument xmlDoc;
                string redirectUrl = Resources.PageURL.HrmsEmpListing;
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

                switch (commonActions)
                {
                    #region LIST
                    case ActionsEnum.LIST:
                        CurrEmployeePayrollPK = 0;
                        redirectUrl = Resources.PageURL.HrmsPayroll;// "~/Employees/EmployeeList.aspx";
                        Response.Redirect(redirectUrl, false);
                        break;
                    #endregion
                    #region DETAIL
                    case ActionsEnum.DETAIL:
                        // this.ActiveTab = ActionsEnum.DEFAULT;
                        redirectUrl = Resources.PageURL.HrmsPayroll;// "~/Employees/EmployeeList.aspx";
                        Response.Redirect(redirectUrl,false);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        Income_Tax_01BO.Income_Tax_01 IncomeTax_Data = (Income_Tax_01BO.Income_Tax_01)SetUIValuesToObject(ControlsEnum.INCOMETAXDETAILS);
                        if (IncomeTax_Data.Section.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(IncomeTax_Data);
                        result = Income_Tax_01BL.SaveIncomeTax_01(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                            SetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.IncomeTax + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.IncomeTax + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.IncomeTax + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.IncomeTax);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SAVEANDCONTINUE
                    case ActionsEnum.SAVEANDCONTINUE:
                        Income_Tax_01BO.Income_Tax_01 IncTax_Data = (Income_Tax_01BO.Income_Tax_01)SetUIValuesToObject(ControlsEnum.INCOMETAXDETAILS);
                        if (IncTax_Data.Section.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(IncTax_Data);
                        result = Income_Tax_01BL.SaveIncomeTax_01(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                            CommonBL userAuth = new CommonBL();
                            string nextPageUrl = BusinessLogic.HRMS.Common.HRMSCommonBL.GetNextTabUrl(Convert.ToInt16(EmpTabEnum.ITDeclaration), GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpTabNextURL").ToString().Split(','),
                                                   GetGlobalResourceObject("ConfigurationsRes", "HrmsEmpInactiveTabs").ToString().Split(','), Resources.PageURL.EmployeeList.ToString());
                            if (userAuth.IsUserHasRights(currentUser.PKUser, nextPageUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK))
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(nextPageUrl) + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.EmployeeList) + "');", true);
                            }
                            //ResetForm(ControlsEnum.CLEAR);
                            //GetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                            //SetFieldValues(ControlsEnum.GETINCOMETAXDETAILS);
                            //this.EntryStatus = EntryStatus.LISTMODE;
                           // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.IncomeTax + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.IncomeTax + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.IncomeTax + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.IncomeTax);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Cancel
                    case BusinessObject.AccountManagement.ActionsEnum.CANCEL:
                        CurrPK = 0;
                        if (hdfPageFlag.Value == PageEnum.Payroll.ToString())
                        {
                            CurrEmployeePayrollPK = 0;
                            Response.Redirect(Resources.PageURL.HrmsPayroll, true);
                        }
                        else if(hdfPageFlag.Value == PageEnum.EmployeeMaster.ToString())
                            Response.Redirect(Resources.PageURL.EmployeeList, true);
                        break;
                    #endregion
                    #region PRINT

                    case ActionsEnum.PRINT:

//                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + hdfEmployeePK.Value + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 3" + "&CurPK=" + CurrEmployeePayrollPK) + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=" + "&APPTYPE=" + "PAYRL" + "&APPSUBTYPE= 3" + "&CurPK=" + CurrEmployeePayrollPK) + "');", true);                        
                        break;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                Panel pnlIncomeTaxDetails = (Panel)item.FindControl("pnlIncomeTaxDetails");
                ImageButton imbHideTaxDetails = (ImageButton)item.FindControl("imbHideTaxDetails");
                ImageButton imbShowTaxDetails = (ImageButton)item.FindControl("imbShowTaxDetails");
                Table str = (Table)item.FindControl("tblDetailsSection");
                imbHideTaxDetails.Attributes.Add("onclick", string.Format("ImbHideTaxDetails_Click('{0}','{1}','{2}');return false;", pnlIncomeTaxDetails.ClientID, imbHideTaxDetails.ClientID, imbShowTaxDetails.ClientID));
                imbShowTaxDetails.Attributes.Add("onclick", string.Format("ImbShowTaxDetails_Click('{0}','{1}','{2}');return false;", pnlIncomeTaxDetails.ClientID, imbHideTaxDetails.ClientID, imbShowTaxDetails.ClientID));

                Label lbl_HTCol1 = (Label)item.FindControl("lbl_HTCol1");
                Label lbl_HTCol2 = (Label)item.FindControl("lbl_HTCol2");
                Label lbl_HTCol3 = (Label)item.FindControl("lbl_HTCol3");
                Label lbl_HTCol4 = (Label)item.FindControl("lbl_HTCol4");
                Label lbl_HTCol5 = (Label)item.FindControl("lbl_HTCol5");
                Panel pnlthCol5 = (Panel)item.FindControl("pnlthCol5");
                lbl_HTCol1.Text = IncomeTax_SectionsList[sectionIndex].Col1;
                lbl_HTCol2.Text = IncomeTax_SectionsList[sectionIndex].Col2;
                lbl_HTCol3.Text = IncomeTax_SectionsList[sectionIndex].Col3;
                lbl_HTCol4.Text = IncomeTax_SectionsList[sectionIndex].Col4;
                lbl_HTCol5.Text = IncomeTax_SectionsList[sectionIndex].Col5;
                pnlthCol5.Visible = (IncomeTax_SectionsList[sectionIndex].Col5_Visible == 1 ? true : false);

                // Calculation Expression Saved into hiddenfields based on Section
                HiddenField hdfSecCol3FunId = (HiddenField)item.FindControl("hdfSecCol3FunId");
                HiddenField hdfSecCol3FunExpression = (HiddenField)item.FindControl("hdfSecCol3FunExpression");
                HiddenField hdfSecCol3ExcludeId = (HiddenField)item.FindControl("hdfSecCol3ExcludeId");
                HiddenField hdfSectionDbId = (HiddenField)item.FindControl("hdfSectionDbId");
                HiddenField hdfSecCol3SumCopyId = (HiddenField)item.FindControl("hdfSecCol3SumCopyId");

                switch ((SectionEnum)(Enum.Parse(typeof(SectionEnum), hdfSectionDbId.Value)))
                {
                    case SectionEnum.Section01:
                        hdfSec1Col3FunId.Value = hdfSecCol3FunId.Value.HtmlDecode();
                        hdfSec1Col3FunExpression.Value = hdfSecCol3FunExpression.Value.HtmlDecode();
                        hdfSec1Col3ExcludeId.Value = hdfSecCol3ExcludeId.Value.HtmlDecode();
                        break;
                    case SectionEnum.Section02:
                        hdfSec2Col3FunId.Value = hdfSecCol3FunId.Value.HtmlDecode();
                        hdfSec2Col3FunExpression.Value = hdfSecCol3FunExpression.Value.HtmlDecode();
                        hdfSec2Col3ExcludeId.Value = hdfSecCol3ExcludeId.Value.HtmlDecode();
                        hdfSec2Col3SumCopyId.Value = hdfSecCol3SumCopyId.Value.HtmlDecode();
                        break;
                    case SectionEnum.Section03:
                        hdfSec3Col3FunId.Value = hdfSecCol3FunId.Value.HtmlDecode();
                        hdfSec3Col3FunExpression.Value = hdfSecCol3FunExpression.Value.HtmlDecode();
                        hdfSec3Col3ExcludeId.Value = hdfSecCol3ExcludeId.Value.HtmlDecode();
                        hdfSec3Col3SumCopyId.Value = hdfSecCol3SumCopyId.Value.HtmlDecode();
                        break;
                }

                Repeater rprIncomeTax = (Repeater)item.FindControl("rprIncomeTax");
                rprIncomeTax.DataSource = IncomeTax_SectionsList[item.ItemIndex].Items;
                rprIncomeTax.DataBind();
            }
        }

        protected void Inner_ActionHandler(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType == ListItemType.Header)
            {

            }

            if (item.ItemType == ListItemType.AlternatingItem || item.ItemType == ListItemType.Item)
            {
                Panel pnltdCol5 = (Panel)item.FindControl("pnltdCol5");
                TextBox txt_ITCol3 = (TextBox)item.FindControl("txt_ITCol3");
                HiddenField hdfColScript = (HiddenField)item.FindControl("hdfColScript");
                if (hdfPageFlag.Value == PageEnum.Payroll.ToString())
                {
                    CheckBox chkDefualtValue = (CheckBox)item.FindControl("chkDefualtValue");
                    chkDefualtValue.Enabled = false;
                }

                pnltdCol5.Visible = (IncomeTax_SectionsList[sectionIndex].Col5_Visible == 1 ? true : false);
                if (!string.IsNullOrEmpty(hdfColScript.Value))
                    txt_ITCol3.Attributes.Add("onkeyup", string.Format(hdfColScript.Value.HtmlDecode()));
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
            ERPService.AdmCompanyMstService admCompanyMstServiceClient;
            ERPManager.ServiceUtility serviceUtilityObj;
            try
            {
                switch (type)
                {
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new ERPService.AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ERPData.ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ERPManager.ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region Get Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        objEmployeeBasicInfo = EmployeeBasicInfoBL.GetEmployeebyID(CurrPK);
                        break;
                    #endregion
                    #region EMPLOYEE PAYROLL LIST
                    case ControlsEnum.EMPPAYROLLDETAILLIST:
                        objEmpPayHeader = Income_Tax_01BL.GetEmployeePayrollDetailList(CurrEmployeePayrollPK);
                        break;
                    #endregion
                    #region GET INCOME TAX DETAILS
                    case ControlsEnum.GETINCOMETAXDETAILS:
                        XmlDocument doc = new XmlDocument();
                        string XmlIcomeTax = GetLocalResourceObject("XmlIncomeTaxFile").ToString();
                        doc.Load(Server.MapPath(XmlIcomeTax));
                        string xmlcontents = doc.InnerXml;

                        string xmlData = doc.InnerXml;
                        if (xmlData == "<Root/>" || xmlData == string.Empty)
                        {
                            IncomeTax_SectionsList = new List<Income_Tax_01BO.Section>();
                        }
                        else
                        {
                            Income_Tax_01BO.Income_Tax_01 root = CommonFunctions.XmlDeserialize<Income_Tax_01BO.Income_Tax_01>(xmlData);
                            IncomeTax_SectionsList = root.Section;
                            foreach (Income_Tax_01BO.Section section in root.Section)
                            {
                                dsList = Income_Tax_01BL.GetTaxAmountByPk(CurrEmployeePayrollPK, Convert.ToInt32(hdfEmployeePK.Value), root.Type, section.SectionID);
                                // no. op people* defulat amount, also not exceed in max amount(both values may be null or empty)
                                foreach (DataRow row in dsList.Tables[1].Rows)
                                {
                                    section.Items.Where(r => r.DbId == Convert.ToInt32(row["TIT_ITEM"])).ToList().ForEach(c =>
                                    {
                                        c.Col2 += " " + row["TIT_VALUE1"].ToString() + " " + row["TIT_DESC1"].ToString();
                                        c.DefaultAmount = (row["TIT_VALUE1"] == null ? c.DefaultAmount : (row["TIT_VALUE1"].ToString().Length <= 0 ? c.DefaultAmount : (Convert.ToDouble(row["TIT_VALUE1"]) > 0 ?
                                       ((c.DefaultAmount == string.Empty ? string.Empty : ((Convert.ToDouble(Convert.ToDouble(c.DefaultAmount) * Convert.ToDouble(row["TIT_VALUE1"])))) >
                                       (c.Col3_Max == string.Empty ? (Convert.ToDouble(Convert.ToDouble(c.DefaultAmount) * Convert.ToDouble(row["TIT_VALUE1"]))) : Convert.ToDouble(c.Col3_Max))
                                       ? c.Col3_Max : (Convert.ToDouble(c.DefaultAmount) * Convert.ToDouble(row["TIT_VALUE1"])).ToString())) : c.DefaultAmount)));
                                    });
                                }

                                if (dsList.Tables[0].Rows.Count > 0)
                                {
                                    foreach (DataRow row in dsList.Tables[0].Rows)
                                    {
                                        section.Items.Where(r => r.DbId == Convert.ToInt32(row["IT1_ITEM"])).ToList().ForEach(c =>
                                        {
                                            c.Col3 = row["IT1_AMOUNT"].ToString(); c.Col4 = row["IT1_VALUE"].ToString(); c.IT1_PK = (row["IT1_PK"] == null ? 0 : Convert.ToInt32(row["IT1_PK"]));
                                            c.IT1_CHECKBOX_1 = (row["IT1_CHECKBOX_1"] == null ? 0 : (row["IT1_CHECKBOX_1"]).ToString() == string.Empty?0:Convert.ToInt32(row["IT1_CHECKBOX_1"]));
                                        });
                                    }
                                }
                                else
                                {
                                    if (hdfPageFlag.Value == PageEnum.EmployeeMaster.ToString())
                                    {
                                        section.Items.Where(r => r.ChkBoxReq == 0).ToList().ForEach(c => { c.Col3 = c.DefaultAmount; });
                                    }
                                }

                            }
                        }

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
                    #region Set Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region GET INCOME TAX DETAILS
                    case ControlsEnum.GETINCOMETAXDETAILS:
                        BindRepeater(ControlsEnum.GETINCOMETAXDETAILS);
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
                #region INCOMETAXDETAILS
                case ControlsEnum.INCOMETAXDETAILS:
                    Income_Tax_01BO.Income_Tax_01 tempIncome_TaxMaster = new Income_Tax_01BO.Income_Tax_01();
                    tempIncome_TaxMaster.Type = 1;
                    tempIncome_TaxMaster.LAST_MOD_DT = LastModifiedTime;
                    tempIncome_TaxMaster.IT1_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    tempIncome_TaxMaster.USER_PK = currentUser.PKUser;
                    tempIncome_TaxMaster.IT1_EPS_PK = CurrEmployeePayrollPK;
                    tempIncome_TaxMaster.IT1_EMP_PK = Convert.ToInt32(hdfEmployeePK.Value);;
                    SetUIValuesToObject(ControlsEnum.SECTIONINCOMETAXDETAILS);
                    tempIncome_TaxMaster.Section = IncomeTax_SectionsList;
                    returnObject = tempIncome_TaxMaster;
                    break;
                #endregion
                #region SECTION INCOME TAX DETAILS
                case ControlsEnum.SECTIONINCOMETAXDETAILS:
                    IncomeTax_SectionsList = new List<Income_Tax_01BO.Section>();
                    int count = 0;
                    foreach (RepeaterItem RepeaterItem_Master in rprIncomeTaxMaster.Items)
                    {
                        Income_Tax_01BO.Section tempIncome_Sections = new Income_Tax_01BO.Section();
                        HiddenField hdfSectionDbId = (HiddenField)RepeaterItem_Master.FindControl("hdfSectionDbId");
                        Repeater rprIncomeTax = (Repeater)RepeaterItem_Master.FindControl("rprIncomeTax");
                        tempIncome_Sections.SectionID = Convert.ToInt32(hdfSectionDbId.Value);

                        IncomeTax_ItemsList = new List<Income_Tax_01BO.Items>();
                        foreach (RepeaterItem repeaterItem_Sections in rprIncomeTax.Items)
                        {
                            Income_Tax_01BO.Items tempIncome_Items = new Income_Tax_01BO.Items();
                            HiddenField hdfItemDbId = (HiddenField)repeaterItem_Sections.FindControl("hdfItemDbId");
                            HiddenField hdfIT1_PK = (HiddenField)repeaterItem_Sections.FindControl("hdfIT1_PK");
                            Label lbl_ITCol1 = (Label)repeaterItem_Sections.FindControl("lbl_ITCol1");
                            TextBox txt_ItAmount = (TextBox)repeaterItem_Sections.FindControl("txt_ITCol3");
                            TextBox txt_ItValue = (TextBox)repeaterItem_Sections.FindControl("txt_ITCol4");
                            Label lbl_Desc = (Label)repeaterItem_Sections.FindControl("lbl_ITCol5");
                            CheckBox chkDefualtValue = (CheckBox)repeaterItem_Sections.FindControl("chkDefualtValue");

                            txt_ItAmount.Attributes.Remove("disabled");
                            if (!string.IsNullOrEmpty(lbl_ITCol1.Text))
                                tempIncome_Items.IT1_SEQ_NO = lbl_ITCol1.Text;
                            tempIncome_Items.DbId = Convert.ToInt32(hdfItemDbId.Value);
                            tempIncome_Items.Col3 = string.IsNullOrEmpty(txt_ItAmount.Text) ? "0" : txt_ItAmount.Text;
                            tempIncome_Items.Col4 = string.IsNullOrEmpty(txt_ItValue.Text) ? "0" : txt_ItValue.Text;
                            tempIncome_Items.IT1_PK = hdfIT1_PK.Value == null ? 0 : Convert.ToInt32(hdfIT1_PK.Value);
                            tempIncome_Items.IT1_CHECKBOX_1 = chkDefualtValue.Checked == true ? 1 : 0;
                            tempIncome_Items.Col5 = lbl_Desc.Text;
                            IncomeTax_ItemsList.Add(tempIncome_Items);
                            tempIncome_Sections.Items = (IncomeTax_ItemsList);
                        }
                        IncomeTax_SectionsList.Add(tempIncome_Sections);
                        count++;
                    }
                    break;
                #endregion
                #region ITEMS INCOME TAX DETAILS
                case ControlsEnum.ITEMSINCOMETAXDETAILS:
                    IncomeTax_ItemsList = new List<Income_Tax_01BO.Items>();
                    foreach (Repeater grdrow in rprIncomeTaxMaster.Items)
                    {
                        Income_Tax_01BO.Items tempDetials = new Income_Tax_01BO.Items();
                        HiddenField hdfSectionDbId = (HiddenField)grdrow.FindControl("hdfSectionDbId");
                        tempDetials.DbId = Convert.ToInt32(hdfSectionDbId.Value);
                        IncomeTax_ItemsList.Add(tempDetials);
                    }
                    break;
                #endregion
            }
            return returnObject;
        }
        #endregion

        #region GetUIValuesFromObject
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region Get - Employee Details
                    case ControlsEnum.EMPLOYEEDETAILSBYID:
                        if (objEmployeeBasicInfo != null)
                        {
                            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                            CurrPK = objEmployeeBasicInfo.empPK;
                            UCempBasicHdr.objEmployeeBasicInfo = objEmployeeBasicInfo;
                            UCempBasicHdr.SetFieldValues();
                        }
                        break;
                    #endregion
                    #region EMP PAYROLL DETAIL LIST
                    case ControlsEnum.EMPPAYROLLDETAILLIST:
                        if (objEmpPayHeader != null)
                        {
                            hdfEmployeePK.Value =Convert.ToString(objEmpPayHeader.empPK);
                            lblPayrollProcess.Text = Convert.ToDateTime(objEmpPayHeader.EPH_FROM_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(objEmpPayHeader.EPH_TO_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat)
                                                        + " " + ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmpPayHeader.EPH_PRC_NAME), 20);
                            lblPayrollProcess.ToolTip = Convert.ToDateTime(objEmpPayHeader.EPH_FROM_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat) + " to " + Convert.ToDateTime(objEmpPayHeader.EPH_TO_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat)
                                                        + " " + HttpUtility.HtmlDecode(objEmpPayHeader.EPH_PRC_NAME);
                            lblPayrollProcessDate.Text = lblPayrollProcessDate.ToolTip = Convert.ToDateTime(objEmpPayHeader.EPH_DATE).ToString(Resources.Constants.HRMSDateDisplayFormat);
                            lblPayrollEmployee.Text = ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(objEmpPayHeader.empText), 60);
                            lblPayrollEmployee.ToolTip = HttpUtility.HtmlDecode(objEmpPayHeader.empText);
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
                    //ddlCompany.Items.Clear();
                    //if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    //{
                    //    ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs); ;
                    //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlCompany.DataBind();
                    //    ddlCompany.Items.Insert(0, new ListItem("Select", "-1"));
                    //    ddlCompany.Items.HtmlDecode();
                    //}
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
                    #region EMPLOYEE OT DETAILS
                   // case ControlsEnum.EMPLOYEEOTDETAILS:
                        //if (OTDataList != null && OTDataList.Count > 0)
                        //{
                        //    grdOTList.DataSource = OTDataList;
                        //    grdOTList.DataBind();
                        //}
                        //else
                        //{
                        //    grdOTList.DataSource = null;
                        //    grdOTList.DataBind();
                        //}
                     //   break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Repeater
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindRepeater(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region GET INCOME TAX DETAILS
                    case ControlsEnum.GETINCOMETAXDETAILS:
                        if (IncomeTax_SectionsList != null && IncomeTax_SectionsList.Count > 0)
                        {
                            rprIncomeTaxMaster.DataSource = IncomeTax_SectionsList;
                            rprIncomeTaxMaster.DataBind();
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
                    break;
                #endregion


            }
        }
        #endregion

        #region Helper Methods
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            GETINCOMETAXDETAILS,
            COMPANY,
            CLEAR,
            INCOMETAXDETAILS,
            SECTIONINCOMETAXDETAILS,
            ITEMSINCOMETAXDETAILS,
            EMPPAYROLLDETAILLIST,
            EMPLOYEEDETAILSBYID
        }
        #endregion

        #region SectionEnum
        public enum SectionEnum
        {
            Section01 = 1,
            Section02 = 2,
            Section03 = 3
        }
        #endregion

        #region PageEnum
        public enum PageEnum
        {
            EmployeeMaster = 0,
            Payroll = 1 
        }
        #endregion

    }
}