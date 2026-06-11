using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic.ReportsManagement;
using BusinessObject;
using ERP.Utilities;
using ERP.Utilities.Constants;
using ERP.Utilities.Dashboard;
using ERPData;
using ERPManager;
using Microsoft.Reporting.WebForms;
using System.Text.RegularExpressions;

using DashBoardUtil = ERP.Utilities.Dashboard;
using Resources;
using BusinessObject.AccountManagement;
using ERP.UserControl;
using System.Text;
using ERP.Store.UI;
using ERPService;
using DataAccess.DashboardManagement;
using System.Web;
using BusinessObject.CommonManagement;
namespace ERP.Dashboard
{
    public partial class Dashboard : MyBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Reports Flag
        /// </summary>
        private bool IsRdlc
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsRdlc] == null ? false : (bool)this.ViewState[ViewstateStrings.IsRdlc];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsRdlc] = value;
            }
        }

        /// <summary>
        /// Current Filter
        /// </summary>
        private string CurrFilter
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.CurrFilter + CurrDashlet.ToString()];
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrFilter + CurrDashlet.ToString()] = value;
            }
        }
        /// <summary>
        /// Current Dashlet
        /// </summary>
        private int CurrDashlet
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrCell]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrCell] = value;
            }
        }
        /// <summary>
        /// Current Page
        /// </summary>
        private int CurrPage
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPage]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPage] = value;
            }
        }
        /// <summary>
        /// Page size
        /// </summary>
        private int CurrPageSize
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrentPageSize]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrentPageSize] = value;
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
        /// To Maintain Asset view By
        /// </summary>
        private int AssetViewByPK
        {
            get
            {
                return ViewState["AssetViewByPK"] != null ? (int)ViewState["AssetViewByPK"] : 0;
            }
            set
            {
                ViewState["AssetViewByPK"] = value;
            }
        }
        /// <summary>
        /// To Maintain Current PK
        /// </summary>
        private int CurrentPK
        {
            get
            {
                return ViewState["CurrentPK"] != null ? (int)ViewState["CurrentPK"] : 0;
            }
            set
            {
                ViewState["CurrentPK"] = value;
            }
        }

        private short CurrParent
        {
            get
            {
                return Convert.ToInt16(this.ViewState[ViewstateStrings.CurrParent]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrParent] = value;
            }
        }

        private DateTime FromDate
        {
            get
            {
                return Convert.ToDateTime(this.ViewState[ViewstateStrings.fromDate]);
            }
            set
            {
                this.ViewState[ViewstateStrings.fromDate] = value;
            }
        }
        private DateTime ToDate
        {
            get
            {
                return Convert.ToDateTime(this.ViewState[ViewstateStrings.toDate]);
            }
            set
            {
                this.ViewState[ViewstateStrings.toDate] = value;
            }
        }

        private bool IsExportExcel
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsExportExcel] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsExportExcel].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsExportExcel] = value;
            }
        }

        private bool IsAssetTypePage
        {
            get
            {
                return true;
                //if (string.IsNullOrEmpty(Request.QueryString["Type"]))
                //    return false;
                //else if (Request.QueryString["Type"] == "1")
                //    return true;
                //else
                //    return false;
            }
        }
        #endregion

        //page related class objects
        private DsbPageMst dsbPageMstObj;
        private DsbDashletMst dsbDashletMstObj;
        private DsbFilterParameterMst dsbFilterParameterMstObj;

        //Report Data
        private DataSet reportData;
        private string reportProcedure;
        private string reportParameters;
        private string reportTitle;

        private ServiceUtility serviceUtilityObj;

        //List for binding details to controls
        private List<DsbPageMst> dsbPageMstLst;
        private List<DsbDashletMst> dsbDashletMstLst;
        private List<DsbFilterParameterMst> dsbFilterParameterMstLst;

        private ActionsEnum commonActions;

        //breadcrumb string
        private string breadCrumb;
        //Flag indicating Page has chart
        private bool hasChart;
        //Flag indicating Zoom Popup
        private bool isZoom;

        private int ReportNo;
        private string BaseCurrency ;
        BusinessObject.User CurrentUser;

        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = (User)(System.Web.HttpContext.Current.User.Identity);
            TotalPages = 1;
            PageActionHandler();
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            SetStatementGridBalance();
            SetStatus();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitComponents", "$(document).ready(function () {InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_InitDate", "$(document).ready(function () {InitDate();});", true);
        }

        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            DashBoardDataService dashBoardAdminServiceClient;
            dashBoardAdminServiceClient = null;

            try
            {
                dashBoardAdminServiceClient = new DashBoardDataService();
                DashBoardUtil.FilterParameters parameter;
                parameter = new DashBoardUtil.FilterParameters();
                parameter.BizUnit = CurrentUser.SBUID;
                parameter.UserPK = CurrentUser.PKUser;
                parameter.Dept = CurrentUser.CurrentDeptPK;
                TextBox txtFromDate = (TextBox)Page.Master.FindControl("txtFromDate");
                TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");
                if (txtFromDate != null && txtToDate != null)
                {
                    parameter.FromDate = Convert.ToDateTime(txtFromDate.Text);
                    parameter.ToDate = Convert.ToDateTime(txtToDate.Text);
                }

                DataTable dtCustomer = dashBoardAdminServiceClient.ExecuteSPCustomer("SPCRM_CUSTOMER_GET_FROM_USER", CurrentUser.PKUser);
                if (dtCustomer != null && dtCustomer.Rows.Count > 0)
                {
                    parameter.CusPK = Convert.ToInt32(dtCustomer.Rows[0]["CUS_PK"]);
                }
                else//If IGPL user
                {                    
                    HiddenField hdfCustomer = (HiddenField)Page.Master.FindControl("hdfCustomerSearch");                   
                    TextBox txtCustomerSearch = (TextBox)Page.Master.FindControl("txtCustomerSearch");
                    if (string.IsNullOrEmpty(txtCustomerSearch.Text) || txtCustomerSearch.Text == "Select/Type")//For clearing dash board, after remove customer name
                    {
                        hdfCustomer.Value = "0"; 
                    }                   
                    if (hdfCustomer != null)
                    {
                        if (!string.IsNullOrEmpty(hdfCustomer.Value))
                            parameter.CusPK = Convert.ToInt32(hdfCustomer.Value);
                    }
                }
                DashBoardUtil.Parameter userPK = new DashBoardUtil.Parameter();


                List<DashBoardUtil.ParamItem> paramValues = new List<DashBoardUtil.ParamItem>();
                paramValues.Add(new DashBoardUtil.ParamItem()
                {
                    Value = CurrentPK
                });
                DataTable dt = new DataTable();
                ParamItem pi = new ParamItem();
                DashBoardUtil.Parameter p1 = new DashBoardUtil.Parameter();
                switch (type)
                {
                    //Used Case to swich using Controls Enum in this Name Space
                    case ControlsEnum.PIE:
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_TOP_BRANDS", parameter.XmlSerialize());
                        chPie.Series["Series1"].XValueMember = "Brand";
                        chPie.Series["Series1"].YValueMembers = "Ordered";
                        chPie.Series["Series1"].Tag = "";
                        chPie.Series["Series1"]["PieLabelStyle"] = "Disabled";
                        //chPie.Legends.Add("Ordered").LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Table;
                        //chPie.Legends.FirstOrDefault().TableStyle = System.Web.UI.DataVisualization.Charting.LegendTableStyle.Auto;
                        //chPie.Legends.FirstOrDefault().Docking = System.Web.UI.DataVisualization.Charting.Docking.Bottom;

                        chPie.DataSource = dt;
                        chPie.DataBind();
                        break;
                    case ControlsEnum.BAR:
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_MONTHLY_ORDERS", parameter.XmlSerialize());
                        chBar.Series["Ordered"].XValueMember = "Month";
                        chBar.Series["Ordered"].YValueMembers = "Orderred";
                        chBar.Series["Dispatched"].YValueMembers = "Dispatched";

                        chBar.Legends.Add("Ordered").LegendStyle = System.Web.UI.DataVisualization.Charting.LegendStyle.Table;
                        chBar.Legends.FirstOrDefault().TableStyle = System.Web.UI.DataVisualization.Charting.LegendTableStyle.Auto;
                        chBar.Legends.FirstOrDefault().Docking = System.Web.UI.DataVisualization.Charting.Docking.Bottom;



                        chBar.DataSource = dt;
                        chBar.DataBind();
                        break;
                    case ControlsEnum.INVOICESGRID:
                        pi.Value = Convert.ToInt16(ddlInvoiceStatus.SelectedValue);
                        p1.Name = "FILTER_STATUS";
                        p1.Label = "Status";
                        p1.Values = new List<ParamItem>();
                        p1.Values.Add(pi);
                        parameter.Parameters = new List<DashBoardUtil.Parameter>();
                        parameter.Parameters.Add(p1);
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_INVOICES", parameter.XmlSerialize());
                        grdInvoices.DataSource = dt;
                        grdInvoices.DataBind();
                        break;
                    case ControlsEnum.ORDERSGRID:
                        pi.Value = Convert.ToInt16(ddlOrdersStatus.SelectedValue);
                        p1.Name = "FILTER_STATUS";
                        p1.Label = "Status";
                        p1.Values = new List<ParamItem>();
                        p1.Values.Add(pi);
                        parameter.Parameters = new List<DashBoardUtil.Parameter>();
                        parameter.Parameters.Add(p1);
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_ORDERS", parameter.XmlSerialize());
                        grdOrders.DataSource = dt;
                        grdOrders.DataBind();
                        break;
                    case ControlsEnum.ENQUIRIESGRID:
                        pi.Value = Convert.ToInt16(ddlEnquiryStatus.SelectedValue);
                        p1.Name = "FILTER_STATUS";
                        p1.Label = "Status";
                        p1.Values = new List<ParamItem>();
                        p1.Values.Add(pi);
                        parameter.Parameters = new List<DashBoardUtil.Parameter>();
                        parameter.Parameters.Add(p1);
                        DataTable dtDtls = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_ENQUIRIES", parameter.XmlSerialize());
                        if (dtDtls != null && dtDtls.Rows.Count > 0)
                        {
                            var q = from t in dtDtls.AsEnumerable() where (t.Field<byte>("STATUS") != 0 && t.Field<byte>("STATUS")!=10) select t;
                            if (q != null && q.Any())
                            {
                                dt = q.CopyToDataTable();
                            }
                        }
                        grdEnquiries.DataSource = dt;
                        grdEnquiries.DataBind();
                        break;
                    case ControlsEnum.DISPATCHGRID:
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_DISPATCH", parameter.XmlSerialize());
                        grdDispatch.DataSource = dt;
                        grdDispatch.DataBind();
                        break;

                    #region BASECURRENCY

                    case ControlsEnum.BASECURRENCY:
                        //Gets Base Currency
                        DataTable  dtcurr=getcurrency();
                        if (dtcurr != null && dtcurr.Rows.Count > 0)
                        {
                            CurrencyMstService CurrencyMstServiceClient = new CurrencyMstService();
                             BaseCurrency = (CurrencyMstServiceClient.GetCurrencyCodeName(Convert.ToInt16(dtcurr.Rows[0]["ACF_DATA"].ToString()))).Split('-')[0].Trim();;
                            
                        }
                        break;
                    #endregion
                    case ControlsEnum.STATEMENTGRID:
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_STATEMENT", parameter.XmlSerialize());

                        //For Balance
                        DataColumn dc = new DataColumn("Balance");
                        dc.DataType = System.Type.GetType("System.Decimal");
                        dt.Columns.Add(dc);
                        double runningBalance = 0;

                        //For TRXAmt
                        DataColumn dc1 = new DataColumn("TRXAmt");
                        dc1.DataType = System.Type.GetType("System.String");
                        dt.Columns.Add(dc1);
                        string TRXAmt = "";


                        foreach (DataRow dr in dt.Rows)
                        {
                            //For Balance
                            double debit = 0;
                            double credit = 0;

                            double.TryParse(Convert.ToString(dr["Debit"]), out debit);
                            double.TryParse(Convert.ToString(dr["Credit"]), out credit);

                            runningBalance = ((runningBalance + debit) - credit);

                            dr["Balance"] = runningBalance;

                            //For TRXAmt
                            double DEBIT_TC = 0;
                            double CREDIT_TC = 0;

                            double.TryParse(Convert.ToString(dr["DEBIT_TC"]), out DEBIT_TC);
                            double.TryParse(Convert.ToString(dr["CREDIT_TC"]), out CREDIT_TC);

                            TRXAmt = dr["BASE_CUR_TEXT"] + " " + (String.Format("{0:c}", DEBIT_TC + CREDIT_TC)).ToString();
                            dr["TRXAmt"] = TRXAmt;
                        }


                        GetFieldValues(ControlsEnum.BASECURRENCY);
                        dt.AcceptChanges();
                        string currency = BaseCurrency;
                        decimal outstatnding = 0;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                           // currency = dt.Rows[dt.Rows.Count - 1]["base_cur_text"].ToString();
                            outstatnding = Convert.ToDecimal(dt.Rows[dt.Rows.Count - 1]["Balance"].ToString());

                            if (Convert.ToDecimal(outstatnding) < 0)
                            {
                                lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", -1 * outstatnding);
                                lblOutstanding.ToolTip = "Current Outstanding: " + currency + " " + String.Format("{0:c}", -1 * outstatnding);
                            }
                            else
                            {
                                lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", outstatnding);
                                lblOutstanding.ToolTip = "Current Outstanding: " + currency + " " + String.Format("{0:c}", outstatnding);
                            }
                        }
                        else
                        {
                            lblOutstanding.Text = string.Empty;
                        }
                        grdStatement.DataSource = dt;
                        grdStatement.DataBind();
                        break;
                    case ControlsEnum.RECENTDETAILS:
                        dt = dashBoardAdminServiceClient.ExecuteSP("SPDSB_CRM_RECENT_TRX", parameter.XmlSerialize());
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow[] dr = null;
                            dr = dt.Select("TRANS_TYPE='Dispatch'");
                            if (dr != null && dr.Count() > 0)
                                ltRecent_Dispatch.Text = (Convert.ToString(dr[0]["TRANS_DATE"]) == "" ? "" : Convert.ToDateTime(dr[0]["TRANS_DATE"]).ToString(Resources.ErpRes.DateFormat).ToString()) + "<br>" + Convert.ToString(dr[0]["TRANS_NO"]) + "<br>" + Convert.ToString(dr[0]["TRANS_VALUE"]);

                            dr = dt.Select("TRANS_TYPE='Order'");
                            if (dr != null && dr.Count() > 0)
                                ltRecent_Order.Text = (Convert.ToString(dr[0]["TRANS_DATE"]) == "" ? "" : Convert.ToDateTime(dr[0]["TRANS_DATE"]).ToString(Resources.ErpRes.DateFormat).ToString()) + "<br>" + Convert.ToString(dr[0]["TRANS_NO"]) + "<br>" + Convert.ToString(dr[0]["TRANS_VALUE"]);

                            dr = dt.Select("TRANS_TYPE='Enquiry'");
                            if (dr != null && dr.Count() > 0)
                                ltRecent_Enquiry.Text = (Convert.ToString(dr[0]["TRANS_DATE"]) == "" ? "" : Convert.ToDateTime(dr[0]["TRANS_DATE"]).ToString(Resources.ErpRes.DateFormat).ToString()) + "<br>" + Convert.ToString(dr[0]["TRANS_NO"]) + "<br>" + Convert.ToString(dr[0]["TRANS_VALUE"]);

                            dr = dt.Select("TRANS_TYPE='Invoice'");
                            if (dr != null && dr.Count() > 0)
                                ltRecent_Invoice.Text = (Convert.ToString(dr[0]["TRANS_DATE"]) == "" ? "" : Convert.ToDateTime(dr[0]["TRANS_DATE"]).ToString(Resources.ErpRes.DateFormat).ToString()) + "<br>" + Convert.ToString(dr[0]["TRANS_NO"]) + "<br>" + Convert.ToString(dr[0]["TRANS_VALUE"]);

                            dr = dt.Select("TRANS_TYPE='Payment'");
                            if (dr != null && dr.Count() > 0)
                                ltRecent_Payment.Text = (Convert.ToString(dr[0]["TRANS_DATE"]) == "" ? "" : Convert.ToDateTime(dr[0]["TRANS_DATE"]).ToString(Resources.ErpRes.DateFormat).ToString()) + "<br>" + Convert.ToString(dr[0]["TRANS_NO"]) + "<br>" + Convert.ToString(dr[0]["TRANS_VALUE"]);

                            dr = dt.Select("TRANS_TYPE='Quotation'");
                            if (dr != null && dr.Count() > 0)
                                ltRecent_Quote.Text = (Convert.ToString(dr[0]["TRANS_DATE"]) == "" ? "" : Convert.ToDateTime(dr[0]["TRANS_DATE"]).ToString(Resources.ErpRes.DateFormat).ToString()) + "<br>" + Convert.ToString(dr[0]["TRANS_NO"]) + "<br>" + Convert.ToString(dr[0]["TRANS_VALUE"]);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dsbPageMstObj = null;
                dsbDashletMstObj = null;
                dsbFilterParameterMstObj = null;
                serviceUtilityObj = null;
                dashBoardAdminServiceClient = null;
            }
        }
        #endregion
        private DataTable getcurrency()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BASE CURRENCY", string.Empty, CurrentUser.SBUID);
            return dt;
        }
        private void ConfigurationSettings()
        {
            IsExportExcel = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "IsExportExcel")));
        }
        protected void Refresh_Area(object sender, EventArgs e)
        {
            try
            {
                string action = string.Empty;

                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    action = ((ImageButton)sender).CommandName;
                }

                switch (action.ToUpper().Trim())
                {
                    case "INVOICE":
                        GetFieldValues(ControlsEnum.INVOICESGRID);
                        break;
                    case "DISPATCH":
                        GetFieldValues(ControlsEnum.DISPATCHGRID);
                        break;
                    case "ORDERS":
                        GetFieldValues(ControlsEnum.ORDERSGRID);
                        break;
                    case "ENQUIRIES":
                        GetFieldValues(ControlsEnum.ENQUIRIESGRID);
                        break;
                    case "STATEMENT":
                        GetFieldValues(ControlsEnum.STATEMENTGRID);
                        break;
                    case "PIECHART":
                        GetFieldValues(ControlsEnum.PIE);
                        break;
                    case "BARCHART":
                        // GetFieldValues(ControlsEnum.BAR);
                        break;
                    case "RECENTDETAILS":
                        GetFieldValues(ControlsEnum.RECENTDETAILS);
                        break;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','Information');", true);
            }
        }
        protected void grdInvoices_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdInvoices.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.INVOICESGRID);
        }
        protected void grdDispatch_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdDispatch.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.DISPATCHGRID);
        }
        protected void grdOrders_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdOrders.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.ORDERSGRID);
        }
        protected void grdEnquiries_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdEnquiries.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.ENQUIRIESGRID);
        }
        protected void grdStatement_OnPaging(object sender, GridViewPageEventArgs e)
        {
            grdStatement.PageIndex = e.NewPageIndex;
            GetFieldValues(ControlsEnum.STATEMENTGRID);
        }

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
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
        #endregion

        #region Helper Methods
        /// <summary>
        /// For Remove HTML tags in String
        /// 
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>

        static string StripHTML(string inputString)
        {
            const string HTML_TAG_PATTERN = "<.*?>";
            return Regex.Replace
              (inputString, HTML_TAG_PATTERN, string.Empty);
        }


        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>

        /// <summary>
        /// Method for Pager binding
        /// </summary>
        private void BindPager()
        {
            try
            {
                //uclPagingInvoice.TotalPages = TotalPages;
                //PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                //uclPagingInvoice.CurrentPage = Convert.ToInt32(PageIndex);
                //uclPagingInvoice.Visible = true;
                //uclPagingInvoice.BindPager();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Method for Rdlc binding
        /// </summary>

        /// <summary>
        /// To bind the Header and Filter Detail Labels
        /// </summary>
        /// <param name="header"></param>
        /// <param name="filter"></param>
        /// <param name="title"></param>
        /// <param name="maxlength"></param>
        private void BindDashletHeader(Label header, Label filter, string title, int maxlength)
        {
            DashBoardUtil.FilterParameters filterParams;
            string dateFilter;
            string paramFilter;
            try
            {
                if (header != null)
                {
                    header.Text = title;
                }
                if (filter != null)
                {
                    filter.Text = string.Empty;
                    if (!string.IsNullOrEmpty(CurrFilter))
                    {
                        dateFilter = string.Empty;
                        paramFilter = string.Empty;
                        filterParams = DashBoardUtil.FilterParameters.XmlDeserialize(CurrFilter);
                        if (filterParams != null)
                        {
                            if (filterParams.FromDate != DateTime.MinValue || filterParams.ToDate != DateTime.MinValue)
                            {
                                dateFilter = "<b>Date:</b> ";
                                dateFilter += filterParams.FromDate != DateTime.MinValue ? filterParams.FromDate.ToString(Resources.ErpRes.DateFormat) : string.Empty;
                                dateFilter += filterParams.FromDate != DateTime.MinValue && filterParams.ToDate != DateTime.MinValue ? " - " : string.Empty;
                                dateFilter += filterParams.ToDate != DateTime.MinValue ? filterParams.ToDate.ToString(Resources.ErpRes.DateFormat) : string.Empty;
                            }
                            if (filterParams.Parameters != null && filterParams.Parameters.Count > 0)
                            {
                                foreach (ERP.Utilities.Dashboard.Parameter prs in filterParams.Parameters)
                                {
                                    if (prs.Values != null && prs.Values.Count > 0)
                                    {
                                        paramFilter += paramFilter == string.Empty ? string.Empty : "; ";
                                        paramFilter += "<b>" + (string.IsNullOrEmpty(prs.Label) ? prs.Name : prs.Label) + ":</b> ";
                                        for (int k = 0; k < prs.Values.Count; k++)
                                        {
                                            paramFilter += k == 0 ? prs.Values[k].Name : ", " + prs.Values[k].Name;
                                        }
                                    }
                                }
                            }
                        }
                        filter.ToolTip = dateFilter.Trim();
                        filter.ToolTip += paramFilter.Trim() == string.Empty ? string.Empty :
                            dateFilter.Trim() == string.Empty ? paramFilter.Trim() : ", " + paramFilter.Trim();
                        if (maxlength <= filter.ToolTip.Length)
                            filter.Text = Utilities.CommonFunctions.GetShortString(filter.ToolTip, filter.ToolTip.Substring(0, maxlength).EndsWith("<") || filter.ToolTip.Substring(0, maxlength).EndsWith("</")
                                || filter.ToolTip.Substring(0, maxlength).EndsWith("<b") || filter.ToolTip.Substring(0, maxlength).EndsWith("</b") ? maxlength + 3 : maxlength);
                        else
                            filter.Text = Utilities.CommonFunctions.GetShortString(filter.ToolTip, maxlength);
                        filter.ToolTip = filter.ToolTip.Replace("<b>", "").Replace("</b>", "");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To bind the filter parameters
        /// </summary>

        #region Get Service

        /// <summary>
        /// Method to get Service Chart Data
        /// </summary>
        /// <param name="dashletObj"></param>
        /// <returns></returns>
        private string GetFilterParams(DsbDashletMst dashletObj)
        {
            DashBoardUtil.FilterParameters parameter;
            parameter = new DashBoardUtil.FilterParameters();
            parameter.BizUnit = CurrentUser.SBUID;
            parameter.UserPK = CurrentUser.PKUser;
            parameter.Dept = CurrentUser.CurrentDeptPK;
            //parameter.FromDate = DateTime.Now.AddDays(1 - DateTime.Now.Day);
            parameter.FromDate = FromDate;
            parameter.ToDate = ToDate;

            DashBoardUtil.Parameter userPK = new DashBoardUtil.Parameter();
            if (CurrentPK >= 0)
            {
                parameter.Parameters = new List<DashBoardUtil.Parameter>();
                List<DashBoardUtil.ParamItem> paramValues = new List<DashBoardUtil.ParamItem>();
                paramValues.Add(new DashBoardUtil.ParamItem()
                {
                    Value = CurrentPK
                });


            }

            CurrFilter = parameter.XmlSerialize();
            return CurrFilter;
        }

        /// Method to get Service Parameter Data
        /// </summary>
        /// <param name="serviceWsdlUri"></param>
        /// <param name="serviceMethod"></param>
        /// <returns></returns>
        private ERP.Utilities.Dashboard.Parameter GetFilterParamData(string serviceWsdlUri, string serviceMethod)
        {
            ERP.Utilities.Dashboard.Parameter rlt;
            object result;
            string xml;
            DashBoardUtil.FilterParameters parameter;
            parameter = new DashBoardUtil.FilterParameters();
            parameter.BizUnit = CurrentUser.SBUID;
            parameter.Dept = CurrentUser.CurrentDeptPK;
            result = GetServiceData(serviceWsdlUri, serviceMethod, parameter.XmlSerialize());
            xml = (string)result;
            rlt = ERP.Utilities.Dashboard.Parameter.XmlDeserialize(xml);
            return rlt;
        }
        /// <summary>
        /// Method to get Service Data
        /// </summary>
        /// <param name="serviceWsdlUri"></param>
        /// <param name="serviceMethod"></param>
        /// <param name="methodParam"></param>
        /// <returns></returns>
        public object GetServiceData(string serviceWsdlUri, string serviceMethod, string methodParam)
        {
            string intfcName;
            object[] result;
            //System.ServiceModel.BasicHttpBinding binding;

            //result = DashboardBL.GetChartData(serviceMethod, CurrFilter);

            //return result[0];
            return new object();
        }
        public object GetSPData(string serviceMethod, string methodParam)
        {
            string intfcName;
            object[] result;
            //System.ServiceModel.BasicHttpBinding binding;

            //result = DashboardBL.GetChartData(serviceMethod, methodParam);

            //return result[0];
            return new object();
        }

        #endregion
        /// <summary>
        /// To remove the dynamic content from the Chart Table cells
        /// </summary>
        /// <param name="ctrl"></param>
        private void RemoveChart(Control ctrl)
        {
            UpdatePanel upnlDashlet;
            try
            {
                upnlDashlet = ctrl.Controls.OfType<UpdatePanel>() != null && ctrl.Controls.OfType<UpdatePanel>().Count() > 0 ? ctrl.Controls.OfType<UpdatePanel>().First() : null;
                if (upnlDashlet != null)
                    ctrl.Controls.Remove(upnlDashlet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrDashlet = 0;
        }
        //public enum InvoiceType
        //{
        //    DOMESTIC = "1",
        //    COMMERCIAL = "2",
        //    PROFORMA="3"
        //}
        #region Code to Remove

        private string GetData(string service)
        {
            DashBoardUtil.ChartData chartSource = new DashBoardUtil.ChartData();
            chartSource.SeriesCount = 9;
            chartSource.ChartItems = new List<DashBoardUtil.ChartItem>();
            chartSource.ChartTitles = new List<string>();
            chartSource.XTitle = "Year";
            chartSource.YTitle = "Sales in Millions(Rs)";
            chartSource.ChartTitles.Add("Desktop");
            chartSource.ChartTitles.Add("Laptop");
            chartSource.ChartTitles.Add("Palmtop");
            chartSource.ChartTitles.Add("Net Book");
            chartSource.ChartTitles.Add("Others 1");
            chartSource.ChartTitles.Add("Others 2");
            chartSource.ChartTitles.Add("Others 3");
            chartSource.ChartTitles.Add("Others 4");
            chartSource.ChartTitles.Add("Others 5");
            chartSource.ChartTitles.Add("Others 6");

            chartSource.ChartItems.Add(
                new DashBoardUtil.ChartItem()
                {
                    x = "2006",
                    y = 100,
                    y1 = 180,
                    y2 = 260,
                    y3 = 340,
                    y4 = 420,
                    y5 = 500,
                    y6 = 580,
                    y7 = 660,
                    y8 = 740,
                    //y9 = 820,
                    //y10 = 900,
                }
            );
            chartSource.ChartItems.Add(
                new DashBoardUtil.ChartItem()
                {
                    x = "2007",
                    y = 150,
                    y1 = 230,
                    y2 = 310,
                    y3 = 390,
                    y4 = 470,
                    y5 = 550,
                    y6 = 630,
                    y7 = 670,
                    y8 = 750,
                    //y9 = 830,
                    //y10 = 910,
                }
            );
            chartSource.ChartItems.Add(
                new DashBoardUtil.ChartItem()
                {
                    x = "2008",
                    y = 120,
                    y1 = 200,
                    y2 = 280,
                    y3 = 360,
                    y4 = 440,
                    y5 = 520,
                    y6 = 600,
                    y7 = 680,
                    y8 = 760,
                    //y9 = 840,
                    //y10 = 920,
                }
            );
            chartSource.ChartItems.Add(
                new DashBoardUtil.ChartItem()
                {
                    x = "2009",
                    y = 160,
                    y1 = 270,
                    y2 = 350,
                    y3 = 420,
                    y4 = 400,
                    y5 = 500,
                    y6 = 610,
                    y7 = 690,
                    y8 = 770,
                    //y9 = 850,
                    //y10 = 930,
                }
            );
            chartSource.ChartItems.Add(
                new DashBoardUtil.ChartItem()
                {
                    x = "2010",
                    y = 110,
                    y1 = 180,
                    y2 = 260,
                    y3 = 340,
                    y4 = 400,
                    y5 = 540,
                    y6 = 620,
                    y7 = 700,
                    y8 = 780,
                    //y9 = 860,
                    //y10 = 940,
                }
            );
            //chartSource.LegendTexts = new List<string>();
            //chartSource.LegendTexts.Add("Legend 1");
            //chartSource.LegendTexts.Add("Legend 2");
            //chartSource.LegendTexts.Add("Legend 3");

            return chartSource.XmlSerialize();
        }

        private string GetGridData(string service, int pageSize, int currentPage)
        {
            string result;
            result = string.Empty;

            GridData chartSource = new GridData();
            chartSource.SeriesCount = 10;
            chartSource.GridItems = new List<GridItem>();
            chartSource.ChartTitles = new List<string>();
            chartSource.ChartTitles.Add("BaseYears");
            chartSource.ChartTitles.Add("Desktop");
            chartSource.ChartTitles.Add("Laptop");
            chartSource.ChartTitles.Add("Palmtop");
            chartSource.ChartTitles.Add("NetBook");
            chartSource.ChartTitles.Add("AllOthers1");
            chartSource.ChartTitles.Add("AllOthers2");
            chartSource.ChartTitles.Add("AllOthers3");
            chartSource.ChartTitles.Add("AllOthers4");
            chartSource.ChartTitles.Add("AllOthers5");
            chartSource.ChartTitles.Add("AllOthers6");
            switch (currentPage)
            {
                case 1:
                    chartSource.GridItems.Add(
                new GridItem()
                {
                    x = "2006",
                    y = 100.ToString(),
                    y1 = 180.ToString(),
                    y2 = 260.ToString(),
                    y3 = 340.ToString(),
                    y4 = 420.ToString(),
                    y5 = 500.ToString(),
                    y6 = 580.ToString(),
                    y7 = 660.ToString(),
                    y8 = 740.ToString(),
                    //y9 = 820,
                    //y10 = 900,
                }
            );
                    chartSource.GridItems.Add(
                        new GridItem()
                        {
                            x = "2007",
                            y = 150.ToString(),
                            y1 = 230.ToString(),
                            y2 = 310.ToString(),
                            y3 = 390.ToString(),
                            y4 = 470.ToString(),
                            y5 = 550.ToString(),
                            y6 = 630.ToString(),
                            y7 = 670.ToString(),
                            y8 = 750.ToString(),
                            //y9 = 830,
                            //y10 = 910,
                        }
                    );
                    break;
                case 2:
                    chartSource.GridItems.Add(
                new GridItem()
                {
                    x = "2008",
                    y = 120.ToString(),
                    y1 = 200.ToString(),
                    y2 = 280.ToString(),
                    y3 = 360.ToString(),
                    y4 = 440.ToString(),
                    y5 = 520.ToString(),
                    y6 = 600.ToString(),
                    y7 = 680.ToString(),
                    y8 = 760.ToString(),
                    //y9 = 840,
                    //y10 = 920,
                }
            );
                    chartSource.GridItems.Add(
                        new GridItem()
                        {
                            x = "2009",
                            y = 160.ToString(),
                            y1 = 270.ToString(),
                            y2 = 350.ToString(),
                            y3 = 420.ToString(),
                            y4 = 400.ToString(),
                            y5 = 500.ToString(),
                            y6 = 610.ToString(),
                            y7 = 690.ToString(),
                            y8 = 770.ToString(),
                            //y9 = 850,
                            //y10 = 930,
                        }
                    );
                    break;
                case 3:
                    chartSource.GridItems.Add(
                new GridItem()
                {
                    x = "2010",
                    y = 110.ToString(),
                    y1 = 180.ToString(),
                    y2 = 260.ToString(),
                    y3 = 340.ToString(),
                    y4 = 400.ToString(),
                    y5 = 540.ToString(),
                    y6 = 620.ToString(),
                    y7 = 700.ToString(),
                    y8 = 780.ToString(),
                    //y9 = 860,
                    //y10 = 940,
                }
            );
                    break;
            }


            chartSource.TotalRows = 5 % pageSize > 0 ? 5 / pageSize + 1 : 5 / pageSize;
            result = chartSource.XmlSerialize();
            return result;
        }

        private string GetGaugeData(string service)
        {
            DashBoardUtil.ChartData chartSource = new DashBoardUtil.ChartData();
            chartSource.SeriesCount = 2;
            chartSource.ChartTitles = new List<string>();
            chartSource.ChartTitles.Add("Speed");
            chartSource.ChartTitles.Add("RPM");
            chartSource.ChartTitles.Add("Fuel");
            chartSource.GaugeItems = new List<GaugeItem>();
            chartSource.GaugeItems.Add(
                new GaugeItem()
                {
                    min = 0,
                    max = 100,
                    value = 45
                }
            );
            chartSource.GaugeItems.Add(
                new GaugeItem()
                {
                    min = 0,
                    max = 500,
                    value = 350
                }
            );
            chartSource.GaugeItems.Add(
                new GaugeItem()
                {
                    min = 0,
                    max = 8.5,
                    value = 3.2
                }
            );

            return chartSource.XmlSerialize();
        }

        #endregion

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
                if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                string ScPK;
                string DOPK;
                switch (commonActions)
                {
                    case ActionsEnum.DEFAULT:
                        GetFieldValues(ControlsEnum.ORDERSGRID);
                        GetFieldValues(ControlsEnum.INVOICESGRID);
                        GetFieldValues(ControlsEnum.ENQUIRIESGRID);
                        GetFieldValues(ControlsEnum.DISPATCHGRID);
                        GetFieldValues(ControlsEnum.STATEMENTGRID);
                        GetFieldValues(ControlsEnum.RECENTDETAILS);
                        break;
                    #region Invoice Print
                    case ActionsEnum.PRINT:
                        HiddenField hdfInvType;
                        HiddenField hdfInvCode;
                        HiddenField hdfInvPK;
                        HiddenField hdfInvCategory;
                        GridViewRow grwInvDetails = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        hdfInvType = (HiddenField)grwInvDetails.FindControl("hdfInvType");
                        hdfInvCode = (HiddenField)grwInvDetails.FindControl("hdfAptCode");
                        hdfInvPK = (HiddenField)grwInvDetails.FindControl("hdfInvPK");
                        hdfInvCategory = (HiddenField)grwInvDetails.FindControl("hdfCategory");
                        ConfigurationSettings();
                        if (hdfInvType.Value != string.Empty)
                        {
                            if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Domestic && Convert.ToInt32(hdfInvCategory.Value)==(int)SalesInvoiceCategory.Invoice)
                            {
                                if (IsExportExcel)
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=1");
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=1") + "');", true);
                                }
                            }
                            else if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Domestic && Convert.ToInt32(hdfInvCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=11") + "');", true);
                            }
                            else if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Export)
                            {
                                if (IsExportExcel && hdfInvCode.Value == AppSubType.SI)
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=2");
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=2") + "');", true);
                                }
                            }
                            else if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Proforma && Convert.ToInt32(hdfInvCategory.Value) == (int)SalesInvoiceCategory.Invoice)
                            {
                                if (IsExportExcel)
                                {
                                    Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=3");
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + hdfInvCode.Value + "&APPSUBTYPE=3") + "');", true);
                                }
                            }
                            else if (Convert.ToInt32(hdfInvType.Value) == (int)SalesInvoiceType.Proforma && Convert.ToInt32(hdfInvCategory.Value) == (int)SalesInvoiceCategory.Advanced)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfInvPK.Value + "&APPTYPE=" + ApplicationType.SIJ + "&APPSUBTYPE=1") + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SC Print
                    case ActionsEnum.SCPRINT:
                        ScPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ScPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region DO Print
                    case ActionsEnum.DOPRINT:
                        DOPK = ((LinkButton)sender).CommandArgument;
                        ConfigurationSettings();
                        if (IsExportExcel)
                        {
                            Response.Redirect(Resources.PageURL.ReportUrl + "?ID=" + DOPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + DOPK + "&APPTYPE=" + ApplicationType.DO + "&APPSUBTYPE=6") + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','Information');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdEnquiries")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        string status = DataBinder.Eval(e.Row.DataItem, "STATUS").ToString();
                        ImageButton iEnquiryStatus = e.Row.FindControl("iEnquiryStatus") as ImageButton;
                        //HiddenField hdfEnquiryStatus = e.Row.FindControl("hdfEnquiryStatus") as HiddenField;
                        if (status == "0")
                        {
                            iEnquiryStatus.ImageUrl = "~/Images/Classic/layout/grd-red.png";
                        }
                        else if (status == "2")
                        {
                            iEnquiryStatus.ImageUrl = "~/Images/Classic/layout/grd-green.png";
                        }
                        else
                        {
                            iEnquiryStatus.ImageUrl = "~/Images/Classic/layout/grd-yellow.png";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Handling Repeater Item Created event
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(Object Sender, RepeaterItemEventArgs e)
        {
            DashBoardUtil.FilterParameters parameter;
            CheckBoxList cbl;
            ERP.Utilities.Dashboard.Parameter paramList;
            DsbFilterParameterMst dataRow;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    cbl = e.Item.FindControl("cblFilterParameter") as CheckBoxList;
                    dataRow = e.Item.DataItem as DsbFilterParameterMst;
                    if (dataRow != null)
                    {
                        if (dataRow.ParamSourceIsService)
                        {
                            paramList = GetFilterParamData(dataRow.ParamDatasource, dataRow.ParamMethod);
                            cbl.Items.Clear();
                            cbl.DataSource = paramList.Values;
                            cbl.DataTextField = "Name";
                            cbl.DataValueField = "Value";
                            cbl.DataBind();
                        }
                        else
                        {
                            reportProcedure = dataRow.ParamDatasource;
                            parameter = new DashBoardUtil.FilterParameters();
                            parameter.BizUnit = CurrentUser.SBUID;
                            parameter.Dept = CurrentUser.CurrentDeptPK;
                            reportParameters = parameter.XmlSerialize();
                            cbl.Items.Clear();
                            cbl.DataSource = reportData;
                            cbl.DataTextField = DashboardConstants.FilterName;
                            cbl.DataValueField = DashboardConstants.FilterKey;
                            cbl.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','Information');", true);
            }
        }
        /// <summary>
        /// Handling Repeater Item Data Bound event
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void ItemBoundActionHandler(Object Sender, RepeaterItemEventArgs e)
        {
            bool isAll;
            CheckBoxList cbl;
            DashBoardUtil.FilterParameters filterParams;
            DsbFilterParameterMst dataRow;
            isAll = true;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    cbl = e.Item.FindControl("cblFilterParameter") as CheckBoxList;
                    if (cbl != null)
                        cbl.Items.Insert(0, new ListItem(Resources.ErpRes.SelectAll, CommonConstants.SELECTVAL));

                    dataRow = e.Item.DataItem as DsbFilterParameterMst;
                    if (dataRow != null)
                    {
                        if (!string.IsNullOrEmpty(CurrFilter))
                        {
                            filterParams = DashBoardUtil.FilterParameters.XmlDeserialize(CurrFilter);
                            if (filterParams != null)
                            {
                                foreach (ERP.Utilities.Dashboard.Parameter pms in filterParams.Parameters)
                                {
                                    if (dataRow.ParamName == pms.Name)
                                    {
                                        foreach (DashBoardUtil.ParamItem pmr in pms.Values)
                                        {
                                            if (cbl.Items.FindByValue(pmr.Value.ToString()) != null)
                                                cbl.Items.FindByValue(pmr.Value.ToString()).Selected = true;
                                            isAll = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (isAll)
                            cbl.Items.FindByValue(CommonConstants.SELECTVAL).Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','Information');", true);
            }
        }

        #endregion

        #region Handle Tree

        /// <summary>
        /// Method to Load the Tree
        /// </summary>
        //private void Tree_Load()
        //{
        //    try
        //    {
        //        //if (CurrentPK > 0)
        //        //    TreeAssetType.RestrictPK = CurrentPK;
        //        if (CurrParent > 0)
        //        {
        //            TreeAssetType.ExpandPK = CurrParent;
        //            TreeAssetType.NeedSpecificExpand = true;
        //        }
        //        TreeAssetType.TreeDataSource = CreateDockDS();
        //        TreeAssetType.BindTree();

        //        btnOk.CommandName = ActionsEnum.ASIGNASSETHIERARCHY.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private List<AccordionTreeBO> CreateDockDS()
        //{
        //    try
        //    {
        //        accordionTreeNodeBOList = new List<AccordionTreeBO>();
        //        if (asrAssetHierarchyMstTree != null)
        //        {
        //            var rootAssetTypes = from assettype in asrAssetHierarchyMstTree
        //                                 where assettype.athParent == null || assettype.athParent.Value == 0
        //                                 select assettype;
        //            foreach (AsrAssetHierarchyMst rootAssetType in rootAssetTypes)
        //            {
        //                AccordionTreeBO actVendors = new AccordionTreeBO
        //                {
        //                    PK = rootAssetType.athPK,
        //                    Name = rootAssetType.athName,
        //                    Type = Resources.ErpRes.TreeObject_Type,
        //                };
        //                accordionTreeNodeBOList.Add(actVendors);
        //                BindTreeChildren(actVendors, rootAssetType);
        //            }
        //        }
        //        return accordionTreeNodeBOList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Recursive Method to generate Child nodes
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parent"></param>
        //private void BindTreeChildren(AccordionTreeBO source, AsrAssetHierarchyMst parent)
        //{
        //    try
        //    {
        //        if (asrAssetHierarchyMstTree != null)
        //        {
        //            var children = from assettype in asrAssetHierarchyMstTree
        //                           where assettype.athParent != null && assettype.athParent.Value == parent.athPK
        //                           select assettype;
        //            foreach (AsrAssetHierarchyMst rootAssetType in children)
        //            {
        //                AccordionTreeBO actVendors = new AccordionTreeBO
        //                {
        //                    PK = rootAssetType.athPK,
        //                    ParentPK = rootAssetType.athParent.Value,
        //                    Name = rootAssetType.athName,
        //                    Type = Resources.ErpRes.TreeObject_Type,
        //                };
        //                if (source.innerlevel == null)
        //                    source.innerlevel = new List<AccordionTreeBO>();
        //                source.innerlevel.Add(actVendors);
        //                BindTreeChildren(actVendors, rootAssetType);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Method to Handle OnTreePopulationOnDemand event of the Tree User Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void TreeAssetType_TreePopulationOnDemand(object sender, TreeNodeEventArgs e)
        //{
        //    try
        //    {
        //        // e.Node -- node value get here.
        //        GetFieldValues(ControlsEnum.TREE);
        //        if (CurrentPK > 0)
        //            TreeAssetType.RestrictPK = CurrentPK;
        //        TreeAssetType.TreeDataSource = CreateDockDS();
        //        TreeAssetType.PopulateChild(e.Node, e.Node.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
        //    }
        //}

        #endregion Handle Tree

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //uclPagingInvoice.CurrentPage = 1;
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
        }

        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPagingInvoice.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPagingInvoice.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPagingInvoice.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPagingInvoice.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPagingInvoice.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

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
                    //case NavigationEnum.PAGECHANGE:
                    //    uclPagingInvoice.CurrentPage = e.CurrentPage;
                    //    break;
                    //case NavigationEnum.FIRST:
                    //    // Assign the current page index.
                    //    if (e.CurrentPage > 1)
                    //        uclPagingInvoice.CurrentPage = 1;
                    //    break;
                    //case NavigationEnum.LAST:
                    //    // Assign the current page index.
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPagingInvoice.CurrentPage = e.TotalPages;
                    //    break;
                    //case NavigationEnum.NEXT:
                    //    // Increment the current page index.
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPagingInvoice.CurrentPage++;
                    //    break;
                    //case NavigationEnum.PREVIOUS:
                    //    // Decrement the current page index.
                    //    if (e.CurrentPage > 1)
                    //        uclPagingInvoice.CurrentPage--;
                    //    break;

                }
                ResetForm();
                // PageIndex = uclPagingInvoice.CurrentPage.ToString();

                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlInvoiceStatus_IndexChanged(object sender, EventArgs e)
        {
            GetFieldValues(ControlsEnum.INVOICESGRID);
        }

        protected void ddlOrdersStatus_IndexChanged(object sender, EventArgs e)
        {
            GetFieldValues(ControlsEnum.ORDERSGRID);
        }

        protected void ddlEnquiryStatus_IndexChanged(object sender, EventArgs e)
        {
            GetFieldValues(ControlsEnum.ENQUIRIESGRID);
        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            //uclPagingInvoice.FirstButtonEnabled = (uclPagingInvoice.CurrentPage == 1) ? false : true;

            //// Should we disable the previous link?
            //uclPagingInvoice.PreviousButtonEnabled = (uclPagingInvoice.CurrentPage == 1) ? false : true;

            //// Should we enable the next link?
            //uclPagingInvoice.NextButtonEnabled = (uclPagingInvoice.CurrentPage < iTotalPages) ? true : false;

            //// Should we enable the last link?
            //uclPagingInvoice.LastButtonEnabled = (uclPagingInvoice.CurrentPage < iTotalPages) ? true : false;
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
            InitializeComponent();
        }

        #endregion

        #region PageActionHandler
        public void RefreshAllGrids()
        {
            GetFieldValues(ControlsEnum.ORDERSGRID);
            GetFieldValues(ControlsEnum.INVOICESGRID);
            GetFieldValues(ControlsEnum.ENQUIRIESGRID);
            GetFieldValues(ControlsEnum.DISPATCHGRID);
            GetFieldValues(ControlsEnum.STATEMENTGRID);
            GetFieldValues(ControlsEnum.RECENTDETAILS);
        }

        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {


            try
            {
                if (!IsPostBack)
                {
                    TextBox txtFromDate = (TextBox)Page.Master.FindControl("txtFromDate");
                    TextBox txtToDate = (TextBox)Page.Master.FindControl("txtToDate");

                    HiddenField hdfFromDate = (HiddenField)Page.Master.FindControl("hdfFromDate");
                    HiddenField hdfToDate = (HiddenField)Page.Master.FindControl("hdfToDate");

                    txtToDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                    DateTime dtFromDt = DateTime.Now.AddMonths(-6).AddDays(-((DateTime.Now.AddMonths(-1).Day) - 1));
                    txtFromDate.Text = dtFromDt.ToString(Resources.ErpRes.DateFormat);

                    if (!string.IsNullOrEmpty(txtFromDate.Text) && !string.IsNullOrEmpty(txtToDate.Text))
                    {
                        FromDate = Convert.ToDateTime(txtFromDate.Text);
                        ToDate = Convert.ToDateTime(txtToDate.Text);
                        hdfFromDate.Value = Convert.ToDateTime(txtFromDate.Text).ToString();
                        hdfToDate.Value = Convert.ToDateTime(txtToDate.Text).ToString();
                    }

                    PageIndex = ReportNo.ToString();

                    GetFieldValues(ControlsEnum.ORDERSGRID);
                    GetFieldValues(ControlsEnum.INVOICESGRID);
                    GetFieldValues(ControlsEnum.ENQUIRIESGRID);
                    GetFieldValues(ControlsEnum.DISPATCHGRID);
                    GetFieldValues(ControlsEnum.STATEMENTGRID);
                    GetFieldValues(ControlsEnum.RECENTDETAILS);
                }
                GetFieldValues(ControlsEnum.PIE);
                GetFieldValues(ControlsEnum.BAR);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        private void SetStatementGridBalance()
        {
            string currency = string.Empty;
            decimal outstatnding = 0;
            foreach (GridViewRow dr in grdStatement.Rows)
            {
                HiddenField hdfStatementBalance = (HiddenField)dr.FindControl("hdfStatementBalance");
                Label lblStatementBalance = (Label)dr.FindControl("lblStatementBalance");
                outstatnding = Convert.ToDecimal(hdfStatementBalance.Value);
                HiddenField hdfBasecurrency = (HiddenField)dr.FindControl("hdfStatementBasecurrency");
                //lblStatementBalance.Text = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                if (Convert.ToDecimal(hdfStatementBalance.Value) == 0)
                {
                    lblStatementBalance.Text = lblStatementBalance.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value));
                    lblStatementBalance.ToolTip = lblStatementBalance.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value));
                }
                else if (Convert.ToDecimal(hdfStatementBalance.Value) < 0)
                {
                    lblStatementBalance.Text = lblStatementBalance.ToolTip = String.Format("{0:c}", -1 * Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                    lblStatementBalance.ToolTip = lblStatementBalance.ToolTip = String.Format("{0:c}", -1 * Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                }
                else
                {
                    lblStatementBalance.Text = lblStatementBalance.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                    lblStatementBalance.ToolTip = lblStatementBalance.ToolTip = String.Format("{0:c}", Convert.ToDecimal(hdfStatementBalance.Value)) + " " + (Convert.ToDecimal(hdfStatementBalance.Value) < 0 ? "Cr" : "Dr");
                }



                currency = hdfBasecurrency.Value;
            }

            //  lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", outstatnding);
            //if (Convert.ToDecimal(outstatnding) < 0)
            //{
            //    lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", -1 * outstatnding);
            //}
            //else
            //{
            //    lblOutstanding.Text = "Current Outstanding: " + currency + " " + String.Format("{0:c}", outstatnding);
            //}
            GetFieldValues(ControlsEnum.BASECURRENCY);
            currency = BaseCurrency;
            ltStatementCurrency.Text = string.Format(GetLocalResourceObject("StatementCurrency").ToString(), currency);
        }

        private void SetStatus()
        {
            foreach (GridViewRow dr in grdOrders.Rows)
            {
                HiddenField hdfStatus = (HiddenField)dr.FindControl("hdfOrderStatus");
                Image iStatus = (Image)dr.FindControl("iOrderStatus");
                if (hdfStatus != null)
                {
                    iStatus.ToolTip = hdfStatus.Value;
                    if (hdfStatus.Value == "Completed")
                    {
                        iStatus.ImageUrl = "~/Images/Classic/layout/grd-green.png";
                    }
                    else if (hdfStatus.Value == "Pending")
                    {
                        iStatus.ImageUrl = "~/Images/Classic/layout/grd-red.png";
                    }
                    else
                    {
                        iStatus.ImageUrl = "~/Images/Classic/layout/grd-grey.png";
                    }
                }
            }

            foreach (GridViewRow dr in grdEnquiries.Rows)
            {
                HiddenField hdfStatus = (HiddenField)dr.FindControl("hdfEnquiryStatus");
                HiddenField hdfEnquiryStatusText = (HiddenField)dr.FindControl("hdfEnquiryStatusText");
                Image iStatus = (Image)dr.FindControl("iEnquiryStatus");
                if (hdfStatus != null)
                {
                    if (hdfEnquiryStatusText != null)
                        iStatus.ToolTip = hdfEnquiryStatusText.Value;
                    int cstatus = 0;
                    int.TryParse(hdfStatus.Value, out cstatus);

                    if (cstatus < 10)
                    {
                        iStatus.ImageUrl = "~/Images/Classic/layout/grd-red.png";
                    }
                    else
                    {
                        iStatus.ImageUrl = "~/Images/Classic/layout/grd-green.png";
                    }
                }
            }
        }

        #endregion

        #region ControlEnum
        /// <summary>
        /// Controls Enum
        /// </summary>
        public enum ControlsEnum
        {
            PIE,
            BAR,
            INVOICESGRID,
            DISPATCHGRID,
            ORDERSGRID,
            ENQUIRIESGRID,
            STATEMENTGRID,
            RECENTDETAILS,
            BASECURRENCY,
        }

        /// <summary>
        /// Enum for node type
        /// </summary>

        public enum NodeType
        {
            Region = 1,
            Custodian,
            System,
            Subsystem,
            AssetType,
            Facility,
            Asset
        }

        #endregion


    }
}