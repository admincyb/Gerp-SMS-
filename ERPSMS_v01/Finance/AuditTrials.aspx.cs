using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.POInvoicing;
using ERP.Utilities; 
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using BusinessObject.AlertManagement;
using BusinessObject.PurchaseOrderManagement;
using BusinessObject.Finance;
using System.IO;
using Microsoft.Reporting.WebForms;
using System.Xml;
using BusinessLogic.VendorManagement;
using BusinessLogic.Sales;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Drawing;
using BusinessObject.Administration.Configurations;

namespace ERPSMS_v01.Finance
{
    public partial class AuditTrials : ERP.Store.UI.MyBasePage // ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }
        /// <summary>
        /// WorkFlow RefID
        /// </summary>
        public int WkfRefID
        {
            get
            {
                return (this.ViewState["BaseWkfRefID"] == null ? 0 : (int)this.ViewState["BaseWkfRefID"]);
            }
            set
            {
                this.ViewState["BaseWkfRefID"] = value;
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
        /// Is continue
        /// </summary>
        private bool Iscont
        {
            get
            {
                return this.ViewState[ViewstateStrings.Iscont] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.Iscont]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Iscont] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrPOPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
            }
        }
        /// <summary>
        /// Tax PK
        /// </summary>
        private int TaxPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TaxPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxPK] = value;
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
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
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
        /// <summary>
        /// To maintain the Then Direction in viewstate
        /// </summary>
        private string ThenDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenDirection] = value;
            }
        }
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        private int SelectedItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedItemPK] = value;
            }
        }
        private bool IsHeaderTax
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTax] = value;
            }
        }
        private bool IsEditMode
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEditMode] = value;
            }
        }
        private string SelectedTaxText
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SelectedTaxText];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTaxText] = value;
            }
        }
        /// <summary>
        /// Approved
        /// </summary>
        private int Approved
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.Approved]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Approved] = value;
            }
        }
        /// <summary>
        /// Approved
        /// </summary>
        private bool Posted
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.Posted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Posted] = value;
            }
        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedCurrency
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedCurrency]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }
        }
        private long SelectedVendors
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedVendors]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedVendors] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        //private int WkfStatus
        //{
        //    get
        //    {
        //        return this.ViewState["WkfStatus"] == null ? Convert.ToByte(0) : Convert.ToByte(this.ViewState["WkfStatus"]);
        //    }
        //    set
        //    {
        //        this.ViewState["WkfStatus"] = value;
        //    }
        //}
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        TextBox WrkfComments;
        DropDownList ddlWkfAction;
        bool hasValidRate;
        private string refID;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
        };

        //page related Entity Object
        DataTable dtReportTypeList;
        DataTable dtReportData;
        DataSet dsReportData;
        DataTable dtTaxCategory;
        string taxType;
        string AuditTrialsRPT;
        private DateTime fromDate;
        private DateTime toDate;
        LocalReport locRpt;
        private ERPEntities currentEntity;
        private StringBuilder sb;
        private int saveResult;
        int CompanyPK = 0;
        List<UsersBO> UsersList;

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
                    //GetFieldValues(ControlsEnum.REPORTTYPELIST);
                    //SetFieldValues(ControlsEnum.REPORTTYPELIST);
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
                    //case ControlsEnum.REPORTTYPELIST:
                    //    dtReportTypeList = BusinessLogic.Finance.AuditTrialsBL.GetGTSReportTypeList(0, 1, "AUDIT REPORTS", currentUser.SBUID, "");
                    //    break;
                    case ControlsEnum.REPORTVIEW:
                        dsReportData = BusinessLogic.Finance.AuditTrialsBL.GetReportData(Convert.ToInt32(DbActiveStatus.ACTIVE), txtItemFromDate.Text, txtItemToDate.Text, "0", Convert.ToInt32(currentUser.SBUID), Convert.ToInt32(ddlUsers.SelectedValue));
                        break;
                    case ControlsEnum.USERS:
                        UsersList = BusinessLogic.Administration.Configurations.UserManagementBL.GetUserMasterDetails(0, 1);
                        break;
                }
                AppTypeDetailsList = cm.GetReportParameters("AUDT", 1, DateTime.Now.Date);
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
                    //case ControlsEnum.REPORTTYPELIST:
                    //    BindDropDown(ControlsEnum.REPORTTYPELIST);
                    //    break;

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
                                SetReportParameters(locRpt);

                                if (chkModified.Checked) // Including with modified
                                {
                                    dtReportData = dsReportData.Tables[0];
                                }
                                else // with out modified
                                {                                   
                                    var resultRptData = (from Rpt in dsReportData.Tables[0].AsEnumerable()
                                                         where Rpt.Field<int>("FTH_IS_MODIFIED") != 1
                                                         select Rpt).ToList();

                                    if (resultRptData.Any())
                                    {
                                        dtReportData = resultRptData.CopyToDataTable<DataRow>();
                                    }                                    
                                }
                                if (dtReportData != null && dtReportData.Rows.Count > 0)
                                {
                                    rptDS = new ReportDataSource("AuditTrials", dtReportData);
                                    rvViewReport.LocalReport.DataSources.Add(rptDS);
                                    //CompanyPK = Convert.ToInt32(dtReportData.Rows[0]["FTH_COMPANY"].ToString());
                                    CompanyPK = Convert.ToInt32(currentUser.SBUID);
                                    rvViewReport.LocalReport.DataSources.Add(GetCompanyDetails(CompanyPK));
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
                    //parameters = new ReportParameter("ExchangeRate", ExchRateDigt);
                    //locRpt.SetParameters(parameters);
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
        private ReportDataSource GetCompanyDetails(int CmpnyPk)
        {
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, null,null).ToList();

            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[0].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;
                    bool OutputFileExists = false;
                    string LogoPath = string.Empty;
                    string OutputLogoPath = string.Empty;
                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO))
                        {
                            LogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    if (!fileExists)
                    {
                        LogoPath = string.Empty;
                    }
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_OP_LOGO))
                        {
                            OutputLogoPath = "file:///" + Server.MapPath(Resources.Controls.LogoPath) + CompanyList[0].CMP_OP_LOGO;
                            OutputFileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_OP_LOGO))
                        {
                            OutputLogoPath = "file:///" + System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[0].CMP_OP_LOGO;
                            OutputFileExists = true;
                        }
                    }
                    if (!OutputFileExists)
                    {
                        OutputLogoPath = string.Empty;
                    }
                    CompanyList[0].CMP_LOGO = LogoPath;
                    CompanyList[0].CMP_OP_LOGO = OutputLogoPath;
                }
            }
            CompanyDtls = new ReportDataSource("CompanyDtls", CompanyList);
            return CompanyDtls;
        }
        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            bool flag = true;
            return flag;
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            bool bIsChecked = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                }
                return retObject;

            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                //case ControlsEnum.REPORTTYPELIST:
                    //ddlItemReport.Items.Clear();
                    //if (dtReportTypeList != null)
                    //{
                    //    ddlItemReport.DataSource = dtReportTypeList;
                    //    ddlItemReport.DataTextField = "CFG_DATA";
                    //    ddlItemReport.DataValueField = "CFG_VALUE";
                    //    ddlItemReport.DataBind();
                    //}
                    ////ddlItemReport.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //ddlItemReport.Items.Insert(0, new ListItem(Resources.ErpRes.All, CommonConstants.SELECT_VALUE_ZERO));
                    //break;
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

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {

            }
        }
        private void setvisibility(ActionsEnum ActionsEnum)
        {
            //switch (ActionsEnum)
            {
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
                int? result;
                bool bIsChecked = false;

                string savePath = string.Empty;
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
                    if (((DropDownList)sender).ID == "ddlCurrency")
                    {
                        commonActions = ActionsEnum.BANKCURRENCY;
                    }
                    if (((DropDownList)sender).ID == "ddlHoldAccount")
                    {
                        commonActions = ActionsEnum.FCHOLDREVERTDTL;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtReverseNow")
                    {
                        commonActions = ActionsEnum.CHECKAMT;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
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

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Page Index Handler for grdVatSaleList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
        }
        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {

        }
        #endregion
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            //btnSave.PreRender += new EventHandler(btnAction_PreRender);
            ////btnNew.PreRender += new EventHandler(btnAction_PreRender);
            ////btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            ////btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            //btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            //btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            //btnView.PreRender += new EventHandler(btnAction_PreRender);
            ////lnkList.PreRender += new EventHandler(btnAction_PreRender);
            ////lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            ////btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            //btnSubmit.Load += new EventHandler(btnAction_Load);
            //btnSave.Load += new EventHandler(btnAction_Load);
            //btnCancel.Load += new EventHandler(btnAction_Load);
            ////btnNew.Load += new EventHandler(btnAction_Load);
            ////btnDelete.Load += new EventHandler(btnAction_Load);
            ////btnJournalize.Load += new EventHandler(btnAction_Load);
            //btnPrint.Load += new EventHandler(btnAction_Load);
            //btnEdit.Load += new EventHandler(btnAction_Load);
            //btnView.Load += new EventHandler(btnAction_Load);
            ////lnkList.Load += new EventHandler(btnAction_Load);
            ////lnkDetail.Load += new EventHandler(btnAction_Load);
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
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //    uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //    uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //    uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //    uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
            //REPORTTYPELIST,
            TAXCATEGORY,
            VIEWGSTREPORT,
            REPORTVIEW,
            CLEAR,
            USERS
        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }
        #endregion
    }
}