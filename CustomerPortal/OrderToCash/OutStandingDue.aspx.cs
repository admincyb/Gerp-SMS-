using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
//using GTIService.Dashboard;
using ERPService;
using System.ComponentModel;
using System.Collections;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessLogic.CommonManagement;
using ERPData;
using ERPManager;
using ERPSMS_v01.UserControls;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.Xml;
using BusinessLogic.Mailer;
using System.Xml.Serialization;

namespace CustomerPortal.OrderToCash
{
    public partial class OutStandingDue : ERP.Store.UI.MyBasePage // System.Web.UI.Page // 
    {
        #region Variables and Properties
        #region Properties

        ///// <summary>
        ///// Current PK
        ///// </summary>
        //private int CurrPK
        //{
        //    get
        //    {
        //        return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.CurrPK] = value;
        //    }
        //}


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

        ///// <summary>
        ///// To maintain the LastModifiedTime in viewstate
        ///// </summary>
        //private DateTime LastModifiedTime
        //{
        //    get
        //    {
        //        return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
        //    }
        //    set
        //    {
        //        this.ViewState[ViewstateStrings.LastModifiedTime] = value;
        //    }
        //}
        #endregion

        User currentUser;
        DataSet dsTemplate;
        private ServiceUtility serviceUtilityObj;
        private ActionsEnum commonActions;
        SaleForecastBO forecastDetailsObj;
        XmlDocument xmlDoc;
        DataTable dtResult;
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
                    txtCalender.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort); // DateTime.Now.ToString("MMM-yyyy");
                    string[] datakeyArray;
                    datakeyArray = new string[1];
                    datakeyArray[0] = "ICH_CUST_PK";
                    grdOutStandingDueMst.DataKeyNames = datakeyArray;
                    GetFieldValues(ControlsEnum.TEMPLATE);
                    SetFieldValues(ControlsEnum.TEMPLATE);
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    btnNew.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }
        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.TEMPLATE:
                        dsTemplate = SaleForecastBL.GetMailTemplate(ApplicationType.OUTDUE);
                        break;
                    case ControlsEnum.DEFAULT:
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdOutStandingDueMst.PageSize;
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CustomerNameText : SortBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        string xmlString = SetUIValuesToXMLString();
                        int pageNo = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        int pageSize = grdOutStandingDueMst.PageSize;
                        dtResult = BusinessLogic.OutStandingDue.OutStandingDueBL.GetOutStandingDueList(xmlString, pageNo, pageSize);
                        serviceUtilityObj.TotalRecords = dtResult.Rows.Count > 0 ? int.Parse(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;//dtResult.Rows.Count;
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
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
                    case ControlsEnum.TEMPLATE:
                        if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
                        {
                            lblCCAddress.Text = Convert.ToString(dsTemplate.Tables[0].Rows[0]["TML_TO_CC"]);
                            lblBCCAddress.Text = Convert.ToString(dsTemplate.Tables[0].Rows[0]["TML_TO_BCC"]);
                        }
                        break;
                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode, List<ForecastResult> customers)
        {
            object returnObj;
            returnObj = null;
            string value;
            value = string.Empty;
            try
            {
                switch (mode)
                {
                    #region Send mails
                    case ActionsEnum.SENDMAIL:
                        forecastDetailsObj = new SaleForecastBO();
                        forecastDetailsObj.Details = new List<ForecastDetails>();
                        List<ForecastDetails> detailsList = new List<ForecastDetails>();
                        ForecastDetails objForecastDetails;
                        foreach (ForecastResult node in customers)
                        {
                            objForecastDetails = new ForecastDetails();
                            string dateMP = string.IsNullOrEmpty(txtCalender.Text) ? string.Empty : txtCalender.Text.Trim();
                            string ToDate = dateMP;
                            int totaldays = DateTime.DaysInMonth((Convert.ToDateTime(ToDate)).Year, (Convert.ToDateTime(ToDate)).Month);
                            objForecastDetails.OMD_FORECAST_DATE = node.LAST_MAIL_ON.Value; // Convert.ToDateTime(totaldays.ToString() + "-" + dateMP);
                            objForecastDetails.OMD_CUSTOMER = node.CUS_PK;
                            objForecastDetails.OMD_MAIL_DATE = DateTime.Now.ToString();
                            objForecastDetails.OMD_RPT_BIZUNIT = currentUser.SBUID;
                            objForecastDetails.OMD_RPT_CRTD_BY = currentUser.PKUser;
                            objForecastDetails.OMD_RPT_CRTD_DT = DateTime.Now.ToString();
                            objForecastDetails.OMD_INVOICE_PK = node.INVOICE_PK;
                            detailsList.Add(objForecastDetails);
                        }
                        forecastDetailsObj.Details = detailsList;
                        returnObj = forecastDetailsObj;
                        break;
                    #endregion
                }
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        private string SetUIValuesToXMLString()
        {
            string resultXml = string.Empty;
            string toDate, bizUnit, dept, userPk, value;
            toDate = string.IsNullOrEmpty(txtCalender.Text) ? string.Empty : txtCalender.Text.Trim();
            bizUnit = currentUser.SBUID.ToString();
            dept = currentUser.CurrentDeptPK.ToString();
            userPk = currentUser.PKUser.ToString();
            value = ddlStatus2.SelectedValue;
            string customerPk = hdfCustomerPK.Value;
            string invoiceNo = txtInvoiceNo.Text.Trim();
            resultXml = "<FilterParameters><ToDate>" + toDate + "</ToDate><BizUnit>" + bizUnit + "</BizUnit><Dept>" + dept + "</Dept>";
            resultXml += "<UserPK>" + userPk + "</UserPK><Parameter><ParamName>DUE_DATE</ParamName><Values><Value>" + value + "</Value>";
            resultXml += "</Values></Parameter>";
            if (customerPk == string.Empty || customerPk == "0")
            {

            }
            else
            {
                resultXml += "<Parameter><ParamName>CUSTOMER_PK</ParamName><Values><Value>" + customerPk + "</Value></Values></Parameter>";
            }
            if (invoiceNo != string.Empty)
            {
                resultXml += "<Parameter><ParamName>INV_NO</ParamName><Values><Value>" + invoiceNo + "</Value></Values></Parameter>";
            }
            resultXml += "</FilterParameters>";
            return resultXml;
        }

        public void BindGrid()
        {
            try
            {
                if (dtResult != null)
                {
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    grdOutStandingDueMst.PageIndex = Convert.ToInt32(PageIndex) - 1;
                    grdOutStandingDueMst.DataSource = dtResult;
                    grdOutStandingDueMst.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                }
                else
                {
                    uclPaging.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Pager Methods + Init

        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
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

        //<summary>
        //Action Handlers For Pager Control
        //</summary>
        //<param name="sender"></param>
        //<param name="e"></param>
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                //grdOutStandingDueMst.BottomPagerRow.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_v01.ERPSMS_2).ValidatePageDept())
                return;
             
            try
            {
                List<ForecastResult> SelectedCustomers = null;
                int? result;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.TOOLTIP;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                switch (commonActions)
                {

                    #region Search
                    case ActionsEnum.SEARCH:
                        PageIndex = null;
                        uclPaging.CurrentPage = 1;
                        btnSearch2.Focus();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    #endregion
                    #region SENDMAIL
                    case ActionsEnum.SENDMAIL:
                        SelectedCustomers = new List<ForecastResult>();
                        foreach (GridViewRow row in grdOutStandingDueMst.Rows)
                        {
                            CheckBox rbtn = (CheckBox)row.FindControl("ChkSelect");
                            HiddenField hfCusEmail = (HiddenField)row.FindControl("hfCusEmail");
                            Label lblCustomerNameTxt = (Label)row.FindControl("lblCustomerNameTxt");
                            Label lblDueDate = (Label)row.FindControl("lblDueDate");
                            //Label lblStatus = (Label)row.FindControl("lblStatus");
                            HiddenField hdfInvoicePk = (HiddenField)row.FindControl("hdfInvoicePk");
                            HiddenField hdfInvoiceDate = (HiddenField)row.FindControl("hdfInvoiceDate");
                            Label lblInvoiceNo = (Label)row.FindControl("lblInvoiceNo");
                            Label lblDueAmount = (Label)row.FindControl("lblDueAmount");
                            HiddenField hdfCurCode = (HiddenField)row.FindControl("hdfCurCode");

                            if (rbtn != null)
                            {
                                if (rbtn.Checked)//if (rbtn.Checked && lblStatus.Text == "Due")
                                {
                                    ForecastResult items = new ForecastResult();
                                    items.CUS_PK = Convert.ToInt32(grdOutStandingDueMst.DataKeys[row.RowIndex].Value);
                                    items.CUS_EMAIL = hfCusEmail.Value;
                                    items.CUS_NAME = lblCustomerNameTxt.ToolTip;
                                    items.LAST_MAIL_ON = Convert.ToDateTime(lblDueDate.Text);
                                    items.INVOICE_PK = Convert.ToInt32(hdfInvoicePk.Value);
                                    items.INVOICE_DATE = Convert.ToDateTime(hdfInvoiceDate.Value);
                                    items.INVOICE_NO = lblInvoiceNo.ToolTip;
                                    items.DUE_AMOUNT = lblDueAmount.Text;
                                    items.DUE_DATE = lblDueDate.Text;
                                    items.CUR_CODE = hdfCurCode.Value;
                                    SelectedCustomers.Add(items);
                                }
                            }
                        }

                        //Save Details
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else//valid
                        {
                            if (SelectedCustomers.Count > 0)
                            {
                                forecastDetailsObj = (SaleForecastBO)SetUIValuesToObject(ActionsEnum.SENDMAIL, SelectedCustomers);
                                if (forecastDetailsObj != null)
                                {
                                    xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(forecastDetailsObj);
                                    result = BusinessLogic.OutStandingDue.OutStandingDueBL.SaveMail(xmlDoc.InnerXml);
                                    if (result >= 0) // Success ! re-initialize the page
                                    {
                                        DataSet dtTemplate = SaleForecastBL.GetMailTemplate(ApplicationType.OUTDUE);
                                        if (dtTemplate != null && dtTemplate.Tables[0].Rows.Count > 0)
                                        {
                                            //Send Mail
                                            string subject = string.Format(dtTemplate.Tables[0].Rows[0]["TML_NAME"].ToString(), txtCalender.Text.Trim());
                                            foreach (ForecastResult dr in SelectedCustomers)
                                            {
                                                object[] paramCustomer = new object[5];
                                                paramCustomer[0] = dr.INVOICE_NO;
                                                paramCustomer[1] = dr.INVOICE_DATE.ToString(Resources.Constants.DateFormatShort);//   txtCalender.Text.Trim();
                                                paramCustomer[2] = dr.CUR_CODE;
                                                paramCustomer[3] = dr.DUE_AMOUNT;
                                                paramCustomer[4] = dr.DUE_DATE;
                                                string Message = string.Format(dtTemplate.Tables[0].Rows[0]["TML_TEMPLATE"].ToString(), paramCustomer);
                                                string toCC = Convert.ToString(dtTemplate.Tables[0].Rows[0]["TML_TO_CC"]);
                                                string toBCC = Convert.ToString(dtTemplate.Tables[0].Rows[0]["TML_TO_BCC"]);
                                                string mailFrom = Convert.ToString(dtTemplate.Tables[0].Rows[0]["TML_FROM"]);
                                                //CommonFunctions.SendMail(subject, Message, dr.CUS_EMAIL);
                                                CommonFunctions.SaveMailQue(subject, Message, dr.CUS_EMAIL, 0, currentUser.SBUID, currentUser.PKUser, ApplicationType.OUTDUE, 1, Convert.ToInt32(dr.CUS_PK), (int)MailStatus.SEND, null, null, toCC, toBCC, mailFrom);
                                            }
                                        }
                                        if (SelectedCustomers.Count > 0)
                                        {
                                            //Show Save success message and reset Contract Entry
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Send_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        GetFieldValues(ControlsEnum.DEFAULT);
                                        SetFieldValues(ControlsEnum.DEFAULT);
                                        btnNew.Focus();
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.MailSending + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MailSending);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Customers").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    //#region PRINT
                    //case ActionsEnum.PRINT:
                    //    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=0&APPTYPE=" + ApplicationType.SALFRCST + "&APPSUBTYPE=&Date=" + CommonFunctions.Encrypt(txtCalender.Text.Trim())) + "&IsDue=" + (ddlStatus.SelectedItem.Text == GetLocalResourceObject("All").ToString() ? 0 : 1) + "');", true);                        
                    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=0&APPTYPE=" + ApplicationType.OUTDUE + "&APPSUBTYPE=&Date=" + CommonFunctions.Encrypt(txtCalender.Text.Trim())) + "&IsDue=" + (ddlStatus.SelectedItem.Text == GetLocalResourceObject("All").ToString() ? 0 : 1) + "');", true);                        
                    //    break;
                    //#endregion
                }
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                //else
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Row data bound Event Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblDueDate = e.Row.FindControl("lblDueDate") as Label;
                    if (DataBinder.Eval(e.Row.DataItem, "ICH_INV_DUE_DATE") != null)
                    {
                        lblDueDate.Text = DataBinder.Eval(e.Row.DataItem, "ICH_INV_DUE_DATE").ToString() == "" ? string.Empty : Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "ICH_INV_DUE_DATE").ToString()).ToString(Resources.Constants.DateFormatShort);
                        lblDueDate.ToolTip = lblDueDate.Text;
                    }
                    Label lblMailSenton = e.Row.FindControl("lblMailSenton") as Label;
                    if (DataBinder.Eval(e.Row.DataItem, "LAST_MAIL_ON") != null)
                    {
                        lblMailSenton.Text = DataBinder.Eval(e.Row.DataItem, "LAST_MAIL_ON").ToString() == "" ? string.Empty : Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "LAST_MAIL_ON").ToString()).ToString(Resources.Constants.DateTimeFormat);
                        lblMailSenton.ToolTip = lblMailSenton.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            try
            {
                PageIndex = e.NewPageIndex.ToString();
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }


        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
                PageIndex = CommonConstants.SELECT_VALUE_ONE;
                uclPaging.CurrentPage = 1;
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            TEMPLATE
        }
        #endregion

        private enum MailStatus
        {
            DRAFT = 4,
            SEND = 0
        }

        #region XmlClasses
        [Serializable]
        public class ForecastDetails
        {
            [XmlElement("OMD_FORECAST_DATE")]
            public DateTime OMD_FORECAST_DATE { get; set; }
            [XmlElement("OMD_CUSTOMER")]
            public int OMD_CUSTOMER { get; set; }
            [XmlElement("OMD_MAIL_DATE")]
            public string OMD_MAIL_DATE { get; set; }
            [XmlElement("OMD_RPT_BIZUNIT")]
            public int OMD_RPT_BIZUNIT { get; set; }
            [XmlElement("OMD_RPT_CRTD_BY")]
            public int OMD_RPT_CRTD_BY { get; set; }
            [XmlElement("OMD_RPT_CRTD_DT")]
            public string OMD_RPT_CRTD_DT { get; set; }
            [XmlElement("OMD_INVOICE_PK")]
            public int OMD_INVOICE_PK { get; set; }
        }

        [Serializable]
        [XmlRoot("Root")]
        public class SaleForecastBO
        {
            [XmlElement("Details")]
            public List<ForecastDetails> Details { get; set; }

        }

        [Serializable]
        public class ForecastResult
        {
            [XmlElement("CUS_PK")]
            public int CUS_PK { get; set; }
            [XmlElement("CUS_EMAIL")]
            public string CUS_EMAIL { get; set; }
            [XmlElement("CUS_NAME")]
            public string CUS_NAME { get; set; }
            [XmlElement("LAST_MAIL_ON")]
            public DateTime? LAST_MAIL_ON { get; set; }
            [XmlElement("INVOICE_PK")]
            public int INVOICE_PK { get; set; }
            [XmlElement("INVOICE_DATE")]
            public DateTime INVOICE_DATE { get; set; }
            [XmlElement("INVOICE_NO")]
            public string INVOICE_NO { get; set; }
            [XmlElement("DUE_AMOUNT")]
            public string DUE_AMOUNT { get; set; }
            [XmlElement("DUE_DATE")]
            public string DUE_DATE { get; set; }
            [XmlElement("CUR_CODE")]
            public string CUR_CODE { get; set; }
        }
        #endregion

    }
}