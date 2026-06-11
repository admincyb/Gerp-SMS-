using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using ERPData;
using ERPService;
using BusinessObject.CommonManagement;
using System.Data;
using BusinessObject.Administration.Configurations;
using Microsoft.Reporting.WebForms;
using System.Threading;
using BusinessObject.AccountManagement;
using GTIService;
using GTIService.Constants.Common;


namespace ERPSMS_v01.Finance
{
    public partial class AuditTrailInvoiceReport : ERP.Store.UI.MyBasePage
    {
        User currentUser;
        private ActionsEnum commonActions;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        DataSet dsReportData;
        DataTable dtReportData;
        DataTable dtTaxDetails;
        List<UsersBO> UsersList;
        LocalReport locRpt;

        /// <summary>
        /// Currency Format String
        /// </summary>
        private string CurrencyFormatString
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] == null ?
                    String.Format("{{0:n{0}}}", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)
                    : (string)ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

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
                    SetDateFields();
                    GetFieldValues(ControlsEnum.USERS);
                    SetFieldValues(ControlsEnum.USERS);
                    GetFieldValues(ControlsEnum.REPORTVIEW);
                    SetFieldValues(ControlsEnum.REPORTVIEW);

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
                AppTypeDetailsList = new List<SPADM_APP_SUB_TYPE_DATA_GET_Result>();
                cm = new CommonService();
                switch (type)
                {
                    case ControlsEnum.REPORTVIEW:
                        bool ismodified = chkModified.Checked;
                        dsReportData = BusinessLogic.Finance.AuditTrialsBL.GetAuditTrailInvReport(txtItemFromDate.Text, txtItemToDate.Text, ismodified, Convert.ToInt32(currentUser.SBUID), Convert.ToInt32(ddlUsers.SelectedValue));
                        break;
                    case ControlsEnum.USERS:
                        UsersList = BusinessLogic.Administration.Configurations.UserManagementBL.GetUserMasterDetails(0, 1);
                        break;
                }
                AppTypeDetailsList = cm.GetReportParameters("AUDT", 4, DateTime.Now.Date);
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
                    #region Report View
                    case ControlsEnum.REPORTVIEW:
                        if (dsReportData != null)
                        {
                            if (dsReportData.Tables[0].Rows.Count > 0)
                            {
                                cm = new CommonService();
                                locRpt = null;
                                rvViewReport.LocalReport.DataSources.Clear();
                                divReportViewer.Visible = true;
                                rvViewReport.Visible = true;
                                divNodata.Visible = false;
                                locRpt = rvViewReport.LocalReport;
                                rvViewReport.LocalReport.DataSources.Clear();
                                locRpt.EnableExternalImages = true;
                                ReportDataSource rptDS;
                                ReportDataSource rptDSTax;
                                SetReportParameters(locRpt);

                                dtReportData = dsReportData.Tables[0];
                                dtTaxDetails = dsReportData.Tables[1];
                                if (dtReportData != null && dtReportData.Rows.Count > 0)
                                {
                                    rptDS = new ReportDataSource("AuditTrialDtls", dtReportData);
                                    rptDSTax = new ReportDataSource("TaxDtls", dtTaxDetails);
                                    rvViewReport.LocalReport.DataSources.Add(rptDS);
                                    rvViewReport.LocalReport.DataSources.Add(rptDSTax);
                                    rvViewReport.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);

                                }
                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvViewReport.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                            else
                            {
                                divReportViewer.Visible = false;
                                rvViewReport.Visible = false;
                                divNodata.Visible = true;
                            }
                        }
                        else
                        {
                            divReportViewer.Visible = false;
                            rvViewReport.Visible = false;
                            divNodata.Visible = true;
                        }
                        break;
                    #endregion

                    #region Users
                    case ControlsEnum.USERS:
                        BindDropDown(ControlsEnum.USERS);
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

        #region Set Report Parameters
        /// <summary>
        /// To set report parameters
        /// </summary>
        /// <param name="locRpt"></param>
        private void SetReportParameters(LocalReport locRpt)
        {
            try
            {
                ReportParameter parameters;
                DataSet dsParamSettings;
                string footer;
                string rptName = string.Empty;
                footer = string.Empty;
                string signaturePath = string.Empty;
                dsParamSettings = new DataSet();
                locRpt.ReportPath = string.Empty;
                foreach (SPADM_APP_SUB_TYPE_DATA_GET_Result sa in AppTypeDetailsList)
                {
                    rptName = sa.AST_OP_FILE1;
                    locRpt.ReportPath = Server.MapPath(rptName);
                    if (sa.AST_RPT_SETTINGS != null)
                    {
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(sa.AST_RPT_SETTINGS)));
                    }

                    //parameters = new ReportParameter("HideQMSRef",sa.AST_QMS_VISIBILITY.ToString());
                    //locRpt.SetParameters(parameters);
                    locRpt.ReportPath = Server.MapPath("../Reports/" + rptName);

                    #region FormatCalculation
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string ExchRateDigt = "#" + currencysep + "#0.";
                    string RateDeciDigt = "#" + currencysep + "#0.";
                    string RateDecDigitPP = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
                    string ExchRateDigit = string.Empty;
                    string RateDecimalDigit = string.Empty;
                    string RateDecimalDigitPP = string.Empty;
                    DataTable dt = ConfigurationSettings();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "CurrencyDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }

                        int NoDigit = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "NumberDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < NoDigit; i++)
                        {
                            Nodecimal += "0";
                        }
                        int ExchRate = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "ExchRateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < ExchRate; i++)
                        {
                            ExchRateDigit += "0";
                        }

                        int RateDecimal = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigit")["ACF_VALUE"].ToString());
                        for (int i = 0; i < RateDecimal; i++)
                        {
                            RateDecimalDigit += "0";
                        }
                        int RateDecimalPP = Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(str => str.Field<string>("ACF_DATA") == "RateDecimalDigitP2P")["ACF_VALUE"].ToString());
                        for (int i = 0; i < RateDecimalPP; i++)
                        {
                            RateDecimalDigitPP += "0";
                        }
                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    ExchRateDigt = ExchRateDigt + ExchRateDigit;
                    RateDeciDigt = RateDeciDigt + RateDecimalDigit;
                    RateDecDigitPP = RateDecDigitPP + RateDecimalDigitPP;
                    #endregion
                    // currencyformat = {0:n} + currencydecimals;
                    parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CurrencyFormat", currencyformat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NumberFormat", NoFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("DateTimeFormat", Resources.Constants.ReportDateTimeFormat);
                    locRpt.SetParameters(parameters);
                    CurrencyFormatString = currencyformat;

                    //parameters = new ReportParameter("RateFormat", RateDeciDigt);
                    //locRpt.SetParameters(parameters); 
                    //parameters = new ReportParameter("RateFormatPP", RateDecDigitPP);
                    //locRpt.SetParameters(parameters);
                    if (dsParamSettings.Tables.Count > 0)
                    {
                        if (dsParamSettings.Tables[0].Columns.Contains("LOGO_HIDE"))
                        {
                            parameters = new ReportParameter("HideLogo", dsParamSettings.Tables[0].Rows[0]["LOGO_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING_HIDE"))
                        {
                            parameters = new ReportParameter("HideHeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING_HIDE"))
                        {
                            parameters = new ReportParameter("HideSubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_LEFT_HIDE"))
                        {
                            parameters = new ReportParameter("HideFooterText", dsParamSettings.Tables[0].Rows[0]["FOOTER_LEFT_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("FOOTER_RIGHT_HIDE"))
                        {
                            parameters = new ReportParameter("HidePageNo", dsParamSettings.Tables[0].Rows[0]["FOOTER_RIGHT_HIDE"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("HEADING"))
                        {
                            parameters = new ReportParameter("HeadTitle", dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                        if (dsParamSettings.Tables[0].Columns.Contains("SUB_HEADING"))
                        {
                            parameters = new ReportParameter("SubTitle", dsParamSettings.Tables[0].Rows[0]["SUB_HEADING"].ToString());
                            locRpt.SetParameters(parameters);
                        }
                    }
                }


                //locRpt.EnableHyperlinks = true;

                parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"]));
                locRpt.SetParameters(parameters);
                footer = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", footer);
                locRpt.SetParameters(parameters);

                //locRpt.EnableExternalImages = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        private DataTable ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.USERS:
                    ddlUsers.Items.Clear();
                    if (UsersList != null && UsersList.Count > 0)
                    {
                        ddlUsers.DataSource = UsersList;
                        ddlUsers.DataTextField = GTIService.Constants.Configurations.Users.Fields.usrEmployeeText;
                        ddlUsers.DataValueField = GTIService.Constants.Configurations.Users.Fields.usrPK;
                        ddlUsers.DataBind();
                    }
                    ddlUsers.Items.Insert(0, new ListItem(GTIService.Constants.Common.CommonConstants.ALL, GTIService.Constants.Common.CommonConstants.ALLVAL));
                    ddlUsers.SelectedIndex = -1;
                    break;
            }
        }

        private void SetDateFields()
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            fromDate = (Convert.ToDateTime(System.DateTime.Now.Month + "/" + "01" + "/" + System.DateTime.Now.Year)).ToString("dd-MMM-yyyy");
            toDate = (Convert.ToDateTime(System.DateTime.Now.ToShortDateString())).ToString("dd-MMM-yyyy");
            txtItemFromDate.Text = fromDate;
            txtItemToDate.Text = toDate;
        }

        public void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                ReportDataSource rptDSTax;
                DataTable dtTaxDetails = dsReportData.Tables[1];
                rptDSTax = new ReportDataSource("TaxDtls", dtTaxDetails);
                int FIN_PK = Convert.ToInt32(e.Parameters["FIN_PK"].Values.First());
                int FIN_TRX_TYPE = Convert.ToInt32(e.Parameters["FIN_TRX_TYPE"].Values.First());
                int FIN_VERSION = Convert.ToInt32(e.Parameters["FIN_VERSION"].Values.First());
                string CurrencyFormat = CurrencyFormatString; 
                e.DataSources.Add(rptDSTax);
            }
            catch (Exception ex)
            {
                throw ex;
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                switch (commonActions)
                {
                    case ActionsEnum.VIEW:
                        GetFieldValues(ControlsEnum.REPORTVIEW);
                        SetFieldValues(ControlsEnum.REPORTVIEW);
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
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

        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
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
            //InitializeComponent();
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            REPORTVIEW,
            USERS
        }
        #endregion

    }
}