using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using GTIService.Dashboard;
using ERPService;
using BusinessObject;
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
using BusinessObject.Reports;
using Microsoft.Reporting.WebForms;
using System.Threading;

namespace ERPSMS_v01.Reports
{
    public partial class CashFlow : ERP.Store.UI.MyBasePageUserRight
    {
        #region Variables and Properties
        #region Properties

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
        /// To maintain the SortExpression or sort By in viewstate in 2,3,4th grid
        /// </summary>
        private string SortByGrd
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortByGrd];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortByGrd] = value;
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
        #endregion

        private CommonService cm;
        private ERPEntities currentEntity;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        decimal bBal = 0;
        decimal rBal = 0;
        decimal pBal = 0;
        decimal eBal = 0;
        private bool issorting = false;
        private DataSet dsTables;
        private DataSet dsReport;
        User currentUser;
        IList listObj;
        private ServiceUtility serviceUtilityObj;
        private ActionsEnum commonActions;
        private List<ADM_CURRENCY_MST> ADM_CURRENCY_MSTList;
        private ADM_CURRENCY_MST ADM_CURRENCY_MSTobj;
        CashFlowBO CashFlowObj;
        XmlDocument xmlDoc;
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
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    GetFieldValues(ControlsEnum.CURRENCY);
                    SetFieldValues(ControlsEnum.CURRENCY);

                    //string[] datakeyArray;
                    //datakeyArray = new string[1];
                    //datakeyArray[0] = Resources.DataFieldRes.Bank_PK;
                    //grdBank.DataKeyNames = datakeyArray;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);

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
            try
            {
                switch (type)
                {
                    #region DEFAULT
                    case ControlsEnum.DEFAULT:
                        SortBy = SortBy == null ? Resources.DataFieldRes.Bank_CODE : SortBy;
                        SortByGrd = SortByGrd == null ? Resources.DataFieldRes.Due_Date : SortByGrd;


                        SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        dsTables = BusinessLogic.ReportsManagement.CashFlowBL.GetCashFlowTables(txtDate.Text.Trim());
                        break;
                    #endregion
                    #region REPORT
                    case ControlsEnum.REPORT:
                        dsReport = BusinessLogic.ReportsManagement.CashFlowBL.GetReportDT(xmlDoc.InnerXml);
                        //TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                        //            (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                        //            (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region Currency
                    case ControlsEnum.CURRENCY:
                        cm = new CommonService();

                        cm = CommonFunctions.InitiateClient(cm);
                        ADM_CURRENCY_MSTobj = new ADM_CURRENCY_MST();
                        ADM_CURRENCY_MSTobj.CUR_PK = currentUser.BaseCurrency;
                        ADM_CURRENCY_MSTobj.CUR_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        ADM_CURRENCY_MSTList = cm.GetCurrency(ADM_CURRENCY_MSTobj);
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
                cm = null;
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
                    #region DEFAULT

                    case ControlsEnum.DEFAULT:
                        BindGrid();
                        foreach (GridViewRow rowB in grdBank.Rows)
                        {
                            Label lblBank_Bal = (Label)rowB.FindControl("lblBank_Bal");
                            bBal = bBal + Convert.ToDecimal(lblBank_Bal.Text);
                        }
                        foreach (GridViewRow rowR in grdreciept.Rows)
                        {
                            Label lblBalanceBC = (Label)rowR.FindControl("lblBalanceBC");
                            rBal = rBal + Convert.ToDecimal(lblBalanceBC.Text);
                        }
                        foreach (GridViewRow rowP in grdPayment.Rows)
                        {
                            Label lblBalanceBC = (Label)rowP.FindControl("lblBalanceBC");
                            pBal = pBal + Convert.ToDecimal(lblBalanceBC.Text);
                        }
                        foreach (GridViewRow rowe in grdExpenses.Rows)
                        {
                            Label lblBalanceBC = (Label)rowe.FindControl("lblBalanceBC");
                            eBal = eBal + Convert.ToDecimal(lblBalanceBC.Text);
                        }
                        lblfundval.Text = String.Format("{0:c}", bBal);
                        colorcode(lblfundval, bBal);
                        lblRecievedval.Text = String.Format("{0:c}", rBal);
                        colorcode(lblRecievedval, rBal);
                        lblPaidval.Text = String.Format("{0:c}", (pBal + eBal));
                        colorcode(lblPaidval, (pBal + eBal));
                        lbldebTotal.Text = String.Format("{0:c}", ((bBal + rBal)));
                        colorcode(lbldebTotal, (bBal + rBal));
                        lblcreditTotal.Text = String.Format("{0:c}", ((pBal + eBal)));
                        colorcode(lblcreditTotal, (pBal + eBal));
                        lblNetval.Text = String.Format("{0:c}", (((bBal + rBal) - (pBal + eBal))));
                        colorcode(lblNetval, ((bBal + rBal) - (pBal + eBal)));
                        ViewState["DebTotal"] = lbldebTotal.Text;
                        ViewState["CreditTotal"] = lblcreditTotal.Text;
                        break;
                    #endregion
                    #region REPORT
                    case ControlsEnum.REPORT:
                        GenerateReport();
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        if (ADM_CURRENCY_MSTList != null && ADM_CURRENCY_MSTList.Count > 0)
                        {
                            grdExpenses.Columns[6].HeaderText = GetLocalResourceObject("BalanceBC").ToString() + "(" + ADM_CURRENCY_MSTList[0].CUR_CODE + ")";
                            grdPayment.Columns[6].HeaderText = GetLocalResourceObject("BalanceBC").ToString() + "(" + ADM_CURRENCY_MSTList[0].CUR_CODE + ")";
                            grdreciept.Columns[6].HeaderText = GetLocalResourceObject("BalanceBC").ToString() + "(" + ADM_CURRENCY_MSTList[0].CUR_CODE + ")";
                            lblfund.Text = GetLocalResourceObject("Funds").ToString() + "(" + ADM_CURRENCY_MSTList[0].CUR_CODE + ")";
                            lblRecieved.Text = GetLocalResourceObject("Recieved").ToString() + "(" + ADM_CURRENCY_MSTList[0].CUR_CODE + ")";
                            lblPaid.Text = GetLocalResourceObject("Paid").ToString() + "(" + ADM_CURRENCY_MSTList[0].CUR_CODE + ")";
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
        public void colorcode(Label lbl, decimal amt)
        {
            if (amt < 0)
            { lbl.ForeColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("Red").ToString()); }
            else { lbl.ForeColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("Blue").ToString()); }

        #endregion


            #region Helper Methods


        }
        private void SetPageURL()
        {
            string path = string.Empty;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.PathAndQuery.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.PathAndQuery;
            base.WkfPageUrl = path.ToLower().Substring(0, path.ToLower().IndexOf("&dep="));
        }
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode)
        {
            object returnObj;
            returnObj = null;
            string value;
            value = string.Empty;
            try
            {
                switch (mode)
                {

                    #region CashFlow
                    case ActionsEnum.REPORT:
                        CashFlowObj = new CashFlowBO();
                        CashFlowObj.AS_ON_DATE = txtDate.Text.Trim();


                        List<DETAIL> detailsList = new List<DETAIL>();
                        DETAIL objDetails;
                        foreach (GridViewRow rowB in grdBank.Rows)
                        {
                            CheckBox rbtn = (CheckBox)rowB.FindControl("ChkSelect");
                            HiddenField hfBPK = (HiddenField)rowB.FindControl("hfBPK");
                            if (rbtn != null)
                            {
                                if (rbtn.Checked == true)
                                {
                                    objDetails = new DETAIL();
                                    objDetails.TYPE = ApplicationType.COA;
                                    objDetails.PK = Convert.ToInt16(hfBPK.Value);
                                    detailsList.Add(objDetails);
                                }
                            }
                        }
                        foreach (GridViewRow rowR in grdreciept.Rows)
                        {
                            CheckBox rbtn = (CheckBox)rowR.FindControl("ChkSelectR");
                            HiddenField hfRPK = (HiddenField)rowR.FindControl("hfRPK");
                            if (rbtn != null)
                            {
                                if (rbtn.Checked == true)
                                {
                                    objDetails = new DETAIL();
                                    objDetails.TYPE = ApplicationType.SI;
                                    objDetails.PK = Convert.ToInt16(hfRPK.Value);
                                    detailsList.Add(objDetails);
                                }
                            }
                        }
                        foreach (GridViewRow rowP in grdPayment.Rows)
                        {
                            CheckBox rbtn = (CheckBox)rowP.FindControl("ChkSelectP");
                            HiddenField hfPPK = (HiddenField)rowP.FindControl("hfPPK");
                            if (rbtn != null)
                            {
                                if (rbtn.Checked == true)
                                {
                                    objDetails = new DETAIL();
                                    objDetails.TYPE = ApplicationType.PI;
                                    objDetails.PK = Convert.ToInt16(hfPPK.Value);
                                    detailsList.Add(objDetails);
                                }
                            }
                        }
                        foreach (GridViewRow rowE in grdExpenses.Rows)
                        {
                            CheckBox rbtn = (CheckBox)rowE.FindControl("ChkSelectE");
                            HiddenField hfEPK = (HiddenField)rowE.FindControl("hfEPK");
                            if (rbtn != null)
                            {
                                if (rbtn.Checked == true)
                                {
                                    objDetails = new DETAIL();
                                    objDetails.TYPE = ApplicationType.EI;
                                    objDetails.PK = Convert.ToInt16(hfEPK.Value);
                                    detailsList.Add(objDetails);
                                }
                            }
                        }
                        CashFlowObj.DETAIL = detailsList;
                        returnObj = CashFlowObj;

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

        public void GenerateReport()
        {
            try
            {
                LocalReport locRpt;

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
                if (dsReport.Tables[0].Rows.Count > 0)
                {
                    dsAS = new ReportDataSource("CashFlowDtls", dsReport.Tables[0]);
                    if (dsAS != null)
                    {
                        AppTypeDetailsList = cm.GetReportParameters(ApplicationType.CFLW, 0, Convert.ToDateTime(DateTime.Now.ToString(Resources.Constants.DateFormatShort)));
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

                else
                {
                    divReportViewer.Visible = false;
                    rvViewReport.Visible = false;
                    divNodata.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                    string LastRefillDt = DateTime.Now.ToString(Resources.Constants.ReportDateFormat);
                    parameters = new ReportParameter("FromDate", LastRefillDt);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("ToDate", Convert.ToDateTime(txtDate.Text.Trim()).ToString(Resources.Constants.ReportDateFormat));
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("DebitTotal", ViewState["DebTotal"].ToString());
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CreditTotal", ViewState["CreditTotal"].ToString());
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
                            // lblBreadCrum.Text = this.GetLocalResourceObject("Print").ToString() + " >> " + dsParamSettings.Tables[0].Rows[0]["HEADING"].ToString();
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


                locRpt.EnableHyperlinks = true;

                parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogoPDF"]));
                locRpt.SetParameters(parameters);


                footer = "Printed by " + currentUser.EmpName + " On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", footer);
                locRpt.SetParameters(parameters);

                locRpt.EnableExternalImages = true;



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable ConfigurationSettings()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }

        public void BindGrid()
        {
            try
            {


                //uclPaging.TotalPages = TotalPages;
                dsTables.Tables[0].DefaultView.Sort = SortBy + " " + SortDirection;

                PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                grdBank.PageIndex = Convert.ToInt32(PageIndex);
                grdBank.DataSource = dsTables.Tables[0];
                grdBank.DataBind();


                //uclPaging.Visible = false;
                //uclPaging.BindPager();
                dsTables.Tables[1].DefaultView.Sort = SortByGrd + " " + SortDirection;
                grdreciept.PageIndex = Convert.ToInt32(PageIndex);
                grdreciept.DataSource = dsTables.Tables[1];
                grdreciept.DataBind();

                dsTables.Tables[2].DefaultView.Sort = SortByGrd + " " + SortDirection;
                grdPayment.PageIndex = Convert.ToInt32(PageIndex);
                grdPayment.DataSource = dsTables.Tables[2];
                grdPayment.DataBind();

                dsTables.Tables[3].DefaultView.Sort = SortByGrd + " " + SortDirection;
                grdExpenses.PageIndex = Convert.ToInt32(PageIndex);
                grdExpenses.DataSource = dsTables.Tables[3];
                grdExpenses.DataBind();


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
            //uclPaging.CurrentPage = 1;

            this.btnGo.PreRender += new EventHandler(btnAction_PreRender);

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
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
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
                        //     uclPaging.CurrentPage = e.CurrentPage;
                        break;
                        //case NavigationEnum.FIRST:
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            //       uclPaging.CurrentPage = 1;
                            break;
                        //  case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            //       uclPaging.CurrentPage = e.TotalPages;
                            break;
                        //  case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            //  uclPaging.CurrentPage++;
                            break;
                        //  case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            // uclPaging.CurrentPage--;
                            break;
                }
                // PageIndex = uclPaging.CurrentPage.ToString();
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
            //// Should we disable the first link
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we disable the previous link
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we enable the next link
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //// Should we enable the last link
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                //SetPageURL();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){calculate();});", true);

                if (!issorting)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchReciept", "$(document).ready(function(){ShowHideAdvancedSearchReciept();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchpay", "$(document).ready(function(){ShowHideAdvancedSearchpay();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchExp", "$(document).ready(function(){ShowHideAdvancedSearchExp();});", true);

                }
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
                    #region Search/GO
                    case ActionsEnum.DEFAULT:

                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINT:

                        CashFlowObj = (CashFlowBO)SetUIValuesToObject(ActionsEnum.REPORT);
                        if (CashFlowObj != null)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(CashFlowObj);
                        }

                        GetFieldValues(ControlsEnum.REPORT);
                        SetFieldValues(ControlsEnum.REPORT);

                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch();});", false);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchReciept", "$(document).ready(function(){ShowHideAdvancedSearchReciept();});", false);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchpay", "$(document).ready(function(){ShowHideAdvancedSearchpay();});", false);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchExp", "$(document).ready(function(){ShowHideAdvancedSearchExp();});", false);
                        break;
                    #endregion

                }



            }
            catch (Exception ex)
            {
                if (ex.Message.Contains(GetLocalResourceObject("DuplicateException").ToString()) && ex.Message.Contains(GetLocalResourceObject("PackingSpecDuplicate").ToString()))
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex).Replace("Code", Resources.Controls.PackingSpecs)) + "','" + Resources.Messages.Information + "');", true);
                else
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

                    //Label lblCustomerLastSODate = e.Row.FindControl("lblCustomerLastSODate") as Label;
                    //if (DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastSODate) != null)
                    //{
                    //    lblCustomerLastSODate.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastSODate).ToString() == "" ? string.Empty : Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastSODate).ToString()).ToString(Resources.Constants.DateFormatShort);
                    //    lblCustomerLastSODate.ToolTip = lblCustomerLastSODate.Text;
                    //}

                    //Label lblNextOrderExp = e.Row.FindControl("lblNextOrderExp") as Label;
                    //if (DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerNextSoDate) != null)
                    //{
                    //    lblNextOrderExp.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerNextSoDate).ToString() == "" ? string.Empty : Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerNextSoDate).ToString()).ToString(Resources.Constants.DateFormatShort);
                    //    lblNextOrderExp.ToolTip = lblNextOrderExp.Text;
                    //}

                    //Label lblMailSenton = e.Row.FindControl("lblMailSenton") as Label;
                    //if (DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastMailOn) != null)
                    //{
                    //    lblMailSenton.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastMailOn).ToString() == "" ? string.Empty : Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastMailOn).ToString()).ToString(Resources.Constants.DateTimeFormat);
                    //    lblMailSenton.ToolTip = lblMailSenton.Text;
                    //}


                    //Label lblLastOrderQty = e.Row.FindControl("lblLastOrderQty") as Label;
                    //if (DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastSOQty) != null)
                    //{
                    //    lblLastOrderQty.Text = DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastSOQty).ToString() == "" ? string.Empty : Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, Resources.DataFieldRes.CustomerLastSOQty)).ToString("0.00");
                    //    lblLastOrderQty.ToolTip = lblLastOrderQty.Text;
                    //}


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

                issorting = true;
                if (((GridView)sender).ID == "grdBank")
                {

                    SortBy = e.SortExpression;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchReciept", "$(document).ready(function(){ShowHideAdvancedSearchReciept();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchpay", "$(document).ready(function(){ShowHideAdvancedSearchpay();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchExp", "$(document).ready(function(){ShowHideAdvancedSearchExp();});", true);
                }
                else
                {
                    SortByGrd = e.SortExpression;
                    if (((GridView)sender).ID == "grdreciept")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchReciept", "$(document).ready(function(){ShowHideAdvancedSearchReciept(1);});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchpay", "$(document).ready(function(){ShowHideAdvancedSearchpay();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchExp", "$(document).ready(function(){ShowHideAdvancedSearchExp();});", true);

                    }
                    else if (((GridView)sender).ID == "grdPayment")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchpay", "$(document).ready(function(){ShowHideAdvancedSearchpay(1);});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchReciept", "$(document).ready(function(){ShowHideAdvancedSearchReciept();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchExp", "$(document).ready(function(){ShowHideAdvancedSearchExp();});", true);

                    }
                    else if (((GridView)sender).ID == "grdExpenses")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchExp", "$(document).ready(function(){ShowHideAdvancedSearchExp(1);});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchpay", "$(document).ready(function(){ShowHideAdvancedSearchpay(0);});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearchReciept", "$(document).ready(function(){ShowHideAdvancedSearchReciept();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowHideAdvancedSearch", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                    }
                }
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;

                PageIndex = CommonConstants.SELECT_VALUE_ONE;
                //uclPaging.CurrentPage = 1;
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
            REPORT,
            CURRENCY
        }
        #endregion


    }
}