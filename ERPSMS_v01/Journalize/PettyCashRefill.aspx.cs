using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using System.Data;
using BusinessLogic.Jouralize;
using ERPService;
using ERPData;
using System.Threading;
using BusinessObject.CommonManagement;
using Microsoft.Reporting.WebForms;
using BusinessObject.Common;
using BusinessObject;
using ERPSMS_v01.UserControls;


namespace ERPSMS_v01.Journalize
{
    public partial class PettyCashRefill : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        private string refID;
        private string inboxFlag;

        private ActionsEnum commonActions;
        DataSet dsPettyCash;
        DataTable dtPageData;
        private DataSet SelectedAccountsList;
        BusinessObject.User currentUser;
        private ERPEntities currentEntity;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;
        private string RptType
        {
            get
            {
                return (string)this.ViewState["ReportType"];
            }
            set
            {
                this.ViewState["ReportType"] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int RptSubType
        {
            get
            {
                return (int)this.ViewState["ReportSubType"];
            }
            set
            {
                this.ViewState["ReportSubType"] = value;
            }
        }
        private DateTime AppvdDate
        {
            get
            {
                return (DateTime)(this.ViewState["AppvdDate"] == null ? DateTime.Now.Date : this.ViewState["AppvdDate"]);
            }
            set
            {
                this.ViewState["AppvdDate"] = value;
            }
        }
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

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PageIndex] ?? "1");
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
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }

        /// <summary>
        /// To maintain the PageSize in viewstate
        /// </summary>
        private int PageSize
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.PageSize] ?? Convert.ToInt32(GetLocalResourceObject("PageSize").ToString()));
            }
            set
            {
                this.ViewState[ViewstateStrings.PageSize] = value;
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

        private double? BalanceCr = 0;
        private double? BalanceDr = 0;
        #endregion
        #region PageLevelEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region PageActionHandler
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                btnSaveSubmit.Visible = false;
                btnSave.Visible = false;
                btnSubmit.Visible = false;
                btnGo.Enabled = false;
                txtRefillExpenseTill.Enabled = false;
                txtPettyCashAccount.Enabled = false;
                ModifiedDatePnl.Visible = true;
            }
            else if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                btnGo.Enabled = true;
                btnSaveSubmit.Visible = false;
                btnSubmit.Visible = false;
                txtRefillExpenseTill.Enabled = true;
                txtPettyCashAccount.Enabled = false;
                ModifiedDatePnl.Visible = true;
            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                btnGo.Enabled = true;
                txtRefillExpenseTill.Enabled = true;
                txtPettyCashAccount.Enabled = true;
                ModifiedDatePnl.Visible = false;
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                ModifiedDatePnl.Visible = false;
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            lblBreadCrum.Text = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        private void PageActionHandler()
        {
            ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
            ucrWrkf.ViewType = 1;

            if (!IsPostBack)
            {
                this.EntryStatus = BusinessObject.Common.EntryStatus.ENTRYMODE;
                PageIndex = 1;

                uclPaging.TotalPages = TotalPages;
                uclPaging.CurrentPage = 1;

                //Used for Integration purpose
                FillProcessID();

                refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
               : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                if (!string.IsNullOrEmpty(refID))
                {
                    ReferanceID = int.Parse(refID);
                    if (!string.IsNullOrEmpty(inboxFlag))
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        //btnSave.Visible = false;
                    }
                    else
                    {
                        ucrWrkf.ViewType = 1;
                        EntryStatus = EntryStatus.ENTRYMODE;
                    }
                    // base.WkfRefID = 
                    ucrWrkf.RefID = int.Parse(refID);
                    CurrPK = GetApplicationID(ucrWrkf.RefID);
                }

                if (CurrPK > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    //base.WkfRefID = 
                    ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                    ucrWrkf.FillWorkFlowDetails();

                    EntryStatus = EntryStatus.ENTRYMODE;
                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                        ucrWrkf.ViewType = 1;
                    else
                    {
                        ucrWrkf.ViewType = 0;
                        //EntryStatus = EntryStatus.VIEWMODE;
                    }
                    ucrWrkf.ViewAction();
                }

                divReportViewer.Visible = false;
            }
        }
        #endregion
        #region GetFieldValues
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.LIST:
                        DateTime? fromDate = null;
                        DateTime? toDate = null;
                        int? cvdAccount = null;
                        dtPageData = BusinessLogic.Jouralize.PettyCashRefillBL.GetPettyCashList(fromDate, toDate, cvdAccount, currentUser.SBUID, this.PageIndex, this.PageSize);
                        break;
                    case ControlsEnum.PETTYCASHREFILL:
                        dsPettyCash = BusinessLogic.Jouralize.PettyCashRefillBL.GetPettyCashRefillDate(Convert.ToInt32(hdfAccount.Value));
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                LocalReport locRpt;
                switch (controlType)
                {
                    #region List
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Petty Cash Refill
                    case ControlsEnum.PETTYCASHREFILL:
                        BindLastRefillDate();
                        break;
                    #endregion
                    #region Report
                    case ControlsEnum.REPORT:
                        cm = new CommonService();
                        locRpt = null;
                        rvViewReport.LocalReport.DataSources.Clear();

                        divReportViewer.Visible = true;
                        rvViewReport.Visible = true;
                        divNodata.Visible = false;

                        locRpt = rvViewReport.LocalReport;

                        rvViewReport.LocalReport.DataSources.Clear();
                        locRpt.EnableExternalImages = true;
                        ReportDataSource dsAS;
                        DataRow dr;
                        dr = SelectedAccountsList.Tables["ACCHEAD"].NewRow();
                        string LastRefillDt = Convert.ToDateTime(txtLastRefillDate.Text.Trim()).AddDays(1).ToString("dd-MMM-yyyy");
                        dr["FROM_DATE"] = LastRefillDt;
                        dr["TO_DATE"] = txtRefillExpenseTill.Text.Trim();
                        dr["BIZUNIT"] = currentUser.SBUID;
                        dr["CURRENCY"] = "";
                        SelectedAccountsList.Tables["ACCHEAD"].Rows.Add(dr);
                        if (SelectedAccountsList.Tables.Count > 0)
                        {
                            if (SelectedAccountsList.Tables[1].Rows.Count > 0)
                            {
                                string paramXml = SelectedAccountsList.GetXml();

                                List<SPFIN_ACCOUNT_STATEMENT_RPT_Result> AccList = currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(paramXml).ToList();
                                dsAS = new ReportDataSource("ASDtls", AccList);
                                if (AccList != null && AccList.Count > 0)
                                {
                                    for (int i = 0; i <= AccList.Count - 1; i++)
                                    {
                                        if (AccList[i].FTH_NARRATION == "Balance B/F")
                                        {
                                            BalanceCr = AccList[i].COA_CR + BalanceCr;
                                            BalanceDr = AccList[i].COA_DR + BalanceDr;
                                        }
                                    }
                                    AppTypeDetailsList = cm.GetReportParameters(ApplicationType.AS, 1, AppvdDate);
                                    SetReportParameters(locRpt);
                                    rvViewReport.LocalReport.DataSources.Add(dsAS);
                                }

                                else
                                {
                                    divReportViewer.Visible = false;
                                    rvViewReport.Visible = false;
                                    divNodata.Visible = true;
                                }
                            }
                        }
                        // dsAS = new ReportDataSource("ASDtls", currentEntity.SPFIN_ACCOUNT_STATEMENT_RPT(currentUser.SBUID, Convert.ToDateTime(txtFromDate.Text.Trim()), Convert.ToDateTime(txtToDate.Text.Trim()), null));

                        else
                        {
                            divReportViewer.Visible = false;
                            rvViewReport.Visible = false;
                            divNodata.Visible = true;
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
        #region HelperMethod

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
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
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
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                //((HiddenField)this.Master.FindControl("hdfPageID")).Value = dtProcess.Rows[0][CommonConstants.F_PAGE].ToString();
            }
        }

        private DataTable ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
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
                    rptName = sa.AST_OP_FILE1;//
                    // locRpt.ReportPath = "~/Reports/PettyCashRefillReport_IGPL.rdlc";
                    locRpt.ReportPath = Server.MapPath("~/Reports/" + rptName);
                    parameters = new ReportParameter("QMSRef", sa.AST_QMS_REF);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HideQMSRef", sa.AST_QMS_VISIBILITY.ToString());
                    locRpt.SetParameters(parameters);
                    if (sa.AST_RPT_SETTINGS != null)
                        dsParamSettings.ReadXml(new System.IO.StringReader(Convert.ToString(sa.AST_RPT_SETTINGS)));


                    string LastRefillDt = Convert.ToDateTime(txtLastRefillDate.Text.Trim()).AddDays(1).ToString("dd-MMM-yyyy");
                    parameters = new ReportParameter("FromDate", LastRefillDt);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("ToDate", txtRefillExpenseTill.Text.Trim());
                    locRpt.SetParameters(parameters);

                    #region FormatCalculation
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
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
                    }
                    else
                    {
                        currencydecimals = "00";
                        Nodecimal = "00";
                    }
                    currencyformat = currencyformat + currencydecimals;
                    NoFormat = NoFormat + Nodecimal;
                    #endregion
                    // currencyformat = {0:n} + currencydecimals;
                    parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CurrencyFormat", currencyformat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NumberFormat", NoFormat);
                    locRpt.SetParameters(parameters);

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
                            lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString() + " >> " + dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
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
                parameters = new ReportParameter("BalanceCr", BalanceCr.ToString());
                locRpt.SetParameters(parameters);
                parameters = new ReportParameter("BalanceDr", BalanceDr.ToString());
                locRpt.SetParameters(parameters);

                locRpt.EnableHyperlinks = true;

                parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"]));
                locRpt.SetParameters(parameters);


                footer = "Printed by " + currentUser.UserName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", footer);
                locRpt.SetParameters(parameters);

                locRpt.EnableExternalImages = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.LIST:
                    int rowCount = 0;
                    if (dtPageData.Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dtPageData.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    }
                    if (this.dtPageData.Rows.Count > 0) this.TotalPages = Convert.ToInt32(dtPageData.AsEnumerable().FirstOrDefault().Field<int>("TOTAL_ROW_COUNT"));

                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                      (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                      (rowCount / this.PageSize) + 1;
                    PageIndex = PageIndex == null ? 1 : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                    grdList.DataSource = dtPageData;
                    grdList.DataBind();

                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
            }
        }
        private void BindLastRefillDate()
        {
            if (dsPettyCash != null && dsPettyCash.Tables[0].Rows.Count > 0)
            {
                txtLastRefillDate.Text = Convert.ToDateTime(dsPettyCash.Tables[0].Rows[0]["CVD_FROM_DATE"]).ToString("dd-MMM-yyyy");
            }
            else
            {
                txtLastRefillDate.Text = string.Empty;
            }
            txtRefillExpenseTill.Text = string.Empty;
            divReportViewer.Visible = false;
            rvViewReport.Visible = false;
        }
        private void ResetForm()
        {
            this.CurrPK = 0;
            hdfAccount.Value = "0";
            txtPettyCashAccount.Text = "Select/Type";
            txtLastRefillDate.Text = string.Empty;
            txtRefillExpenseTill.Text = string.Empty;
            //GetFieldValues(ControlsEnum.PETTYCASHREFILL);
            //SetFieldValues(ControlsEnum.PETTYCASHREFILL);
            divReportViewer.Visible = false;
            rvViewReport.Visible = false;

        }
        public void GetSelectedAccounts()
        {
            DataRow dr;
            string emptyXml;

            currentEntity = new ERPEntities();
            SelectedAccountsList = new DataSet();

            emptyXml = "<ROOT><ACCHEAD><FROM_DATE/><TO_DATE/><BIZUNIT/><CURRENCY/></ACCHEAD><ACCOUNT><COA_PK/></ACCOUNT></ROOT>";

            SelectedAccountsList.ReadXml(new System.IO.StringReader(emptyXml));

            if (hdfAccount.Value != "0" && hdfAccount.Value != string.Empty)
            {
                dr = SelectedAccountsList.Tables["ACCOUNT"].NewRow();
                dr["COA_PK"] = hdfAccount.Value;
                SelectedAccountsList.Tables["ACCOUNT"].Rows.Add(dr);
            }
        }
        #endregion
        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            DropDownList ddlWkfAction;
            string action;
            GridViewRow row;
            string fromDate;
            string toDate ;
            bool bIsChecked = false;
            try
            {
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
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }

                switch (commonActions)
                {
                    #region List
                    case ActionsEnum.LIST:
                        this.EntryStatus = BusinessObject.Common.EntryStatus.LISTMODE;
                        this.PageIndex = 1;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region Entry Or Detail
                    case ActionsEnum.DETAIL:
                        ResetForm();
                        this.EntryStatus = BusinessObject.Common.EntryStatus.ENTRYMODE;
                        break;
                    #endregion

                    #region ACCOUNTCHANGE
                    case ActionsEnum.PETTYCASHACCOUNTSELECTED:
                        if (hdfAccount.Value != CommonConstants.SELECT_VALUE_ZERO && hdfAccount.Value != string.Empty)
                        {
                            GetFieldValues(ControlsEnum.PETTYCASHREFILL);
                            SetFieldValues(ControlsEnum.PETTYCASHREFILL);
                        }
                        break;
                    #endregion

                    #region Save
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (txtRefillExpenseTill.Text != null && txtRefillExpenseTill.Text != string.Empty && hdfAccount.Value != null && hdfAccount.Value != string.Empty)
                        {
                            User objUser = new User();
                            objUser.PKUser = currentUser.PKUser;
                            objUser.SBUID = currentUser.SBUID;
                            objUser.CurrentDeptPK = currentUser.CurrentDeptPK;

                            if (divReportViewer.Visible == true)
                            {
                                DateTime? lastModDate = this.EntryStatus == BusinessObject.Common.EntryStatus.EDITMODE ? this.LastModifiedTime : (DateTime?)null;
                                result = PettyCashRefillBL.SavePettyCashRefill(Convert.ToInt32(hdfAccount.Value), Convert.ToDateTime(txtRefillExpenseTill.Text), objUser,this.CurrPK,lastModDate);
                                if (result > 0)
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PettyCashRefill);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    ResetForm();
                                }
                                else
                                {

                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PackingMaster);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.NoRecordFound;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                        }

                        break;
                    #endregion

                    #region REPORT
                    case ActionsEnum.REPORT:

                        GetSelectedAccounts();
                        SetFieldValues(ControlsEnum.REPORT);

                        break;

                    #endregion

                    #region View
                    case ActionsEnum.VIEW:
                        this.EntryStatus = BusinessObject.Common.EntryStatus.VIEWMODE;
                        row = (GridViewRow)((ImageButton)sender).Parent.Parent;

                        fromDate = ((HiddenField)row.FindControl("hdfFromDate")).Value.Trim();
                        toDate = ((HiddenField)row.FindControl("hdfToDate")).Value.Trim();

                        hdfAccount.Value = ((HiddenField)row.FindControl("hdfCVDAccount")).Value.Trim();
                        txtPettyCashAccount.Text = ((Label)row.FindControl("lblAccount")).ToolTip.Trim(); 
                        txtLastRefillDate.Text = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                        txtRefillExpenseTill.Text = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");

                        this.LastModifiedTime = Convert.ToDateTime(((HiddenField)row.FindControl("hdfLastModDate")).Value.Trim());
                        lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);

                        GetSelectedAccounts();
                        SetFieldValues(ControlsEnum.REPORT);
                        break;
                    #endregion

                    #region Edit
                    case ActionsEnum.EDITITEM:
                        this.EntryStatus = BusinessObject.Common.EntryStatus.EDITMODE;
                        row = (GridViewRow)((ImageButton)sender).Parent.Parent;

                        fromDate = ((HiddenField)row.FindControl("hdfFromDate")).Value.Trim();
                        toDate = ((HiddenField)row.FindControl("hdfToDate")).Value.Trim();

                        this.CurrPK = Convert.ToInt32(((HiddenField)row.FindControl("hdfCVD_PK")).Value.Trim());
                        hdfAccount.Value = ((HiddenField)row.FindControl("hdfCVDAccount")).Value.Trim();
                        txtPettyCashAccount.Text = ((Label)row.FindControl("lblAccount")).ToolTip.Trim(); 
                        txtLastRefillDate.Text = Convert.ToDateTime(fromDate).ToString("dd-MMM-yyyy");
                        txtRefillExpenseTill.Text = Convert.ToDateTime(toDate).ToString("dd-MMM-yyyy");
                        this.LastModifiedTime = Convert.ToDateTime(((HiddenField)row.FindControl("hdfLastModDate")).Value.Trim());
                        lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                        
                        GetSelectedAccounts();
                        SetFieldValues(ControlsEnum.REPORT);

                        break;
                    #endregion

                    #region Cancel
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        this.EntryStatus = BusinessObject.Common.EntryStatus.ENTRYMODE;
                        break;
                    #endregion

                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        //Show WorkFlow Popup
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WRKSUBMIT
                    case ActionsEnum.WRKFSUBMIT:

                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {

                                if (txtRefillExpenseTill.Text != null && txtRefillExpenseTill.Text != string.Empty && hdfAccount.Value != null && hdfAccount.Value != string.Empty)
                                {

                                    if (divReportViewer.Visible == true)
                                    {
                                       // DateTime? lastModDate = this.EntryStatus == BusinessObject.Common.EntryStatus.EDITMODE ? this.LastModifiedTime : (DateTime?)null;
                                        result = PettyCashRefillBL.SavePettyCashRefill(Convert.ToInt32(hdfAccount.Value), Convert.ToDateTime(txtRefillExpenseTill.Text), currentUser,this.CurrPK);
                                        if (result > 0)
                                        {
                                            ucrWrkf.ApplicationID = Convert.ToInt32(result);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PackingMaster);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.NoRecordFound;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                        return;
                                    }

                                }
                            }
                            else
                            {

                                ucrWrkf.ApplicationID = CurrPK;
                            }
                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.PettyCashRefill);
                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                                        {
                                            ResetForm();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            ResetForm();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //Trx not saved
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Error_NoPK;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PurchaseOrder);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }

                        break;
                    #endregion

                }

            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                //else
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }


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
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }

                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.LIST);
                SetFieldValues(ControlsEnum.LIST);
                EntryStatus = EntryStatus.LISTMODE;
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }

        #endregion

        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

        #region OnPageIndexChanging Event
        /// <summary>
        /// Page Index Handler for grd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            EntryStatus = EntryStatus.LISTMODE;
        }
        #endregion
        #endregion

        #region Pager Methods + Init

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
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {

            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);


            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnCancel.Load += new EventHandler(btnAction_Load);
            uclPaging.CurrentPage = 1;

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
            //base.CheckBtnVisibility(sender);
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
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            DEFAULT,
            PETTYCASHREFILL,
            CONTROLS,
            REPORT,
            LIST

        }
        #endregion
    }
}