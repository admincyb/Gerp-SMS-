using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using GTIService;
using GTIService.Dashboard;
using WcfSamples.DynamicProxy;
using gAssetsData;
using Microsoft.Reporting.WebForms;
using System.Web.UI.HtmlControls;
using GTIService.Constants.Common;
using System.Data;
using gAssetsManager;
using BusinessObject.AccountManagement;
using BusinessLogic;
using System.Xml;
using System.IO;

namespace ERPSMS_v01.DashboardSMS
{
    public partial class Dashboard : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Properties

        BusinessObject.User CurrentUser;

        /// <summary>
        /// Reports Flag
        /// </summary>
        private bool IsRdlc
        {
            get
            {
                return this.ViewState["isRdlc"] == null ? false : (bool)this.ViewState["isRdlc"];
            }
            set
            {
                this.ViewState["isRdlc"] = value;
            }
        }
        /// <summary>
        /// Zoom PK
        /// </summary>
        private string ZoomPK
        {
            get
            {
                return hdfZoomPK.Value.Trim();
            }
            set
            {
                hdfZoomPK.Value = value;
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
                return this.ViewState[ViewstateStrings.TotalPages]== null ? 1 : (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
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

        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            PageActionHandler();
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            //Show/hide empty div
            if (!hasChart && !IsRdlc)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowEmpty1", "ShowEmpty(true);", true);
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitDate", "$(document).ready(function () {InitComponents();});", true);
            if (isZoom)
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPopZoom", "GrandScriptUtils.ShowModalID('divPopup','" + dsbDashletMstLst[0].DashletTitle + "',false,'960','600');", true);
            if (!string.IsNullOrEmpty(breadCrumb))
                lblBreadCrum.Text = breadCrumb;
        }

        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            object[] result;
            DashBoardAdminServiceClient dashBoardAdminServiceClient;
            dashBoardAdminServiceClient = null;
            try
            {
                dashBoardAdminServiceClient = new DashBoardAdminServiceClient();
                dashBoardAdminServiceClient = CommonFunctions.InitiateClient(dashBoardAdminServiceClient);
                switch (type)
                {
                    //Used Case to swich using Controls Enum in this Name Space
                    case ControlsEnum.DEFAULT:
                        dsbPageMstObj = dashBoardAdminServiceClient.GetInitilizedDsbPageMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = 1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.DsbPagePK;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        dsbPageMstObj.PagePk = 0;
                        dsbPageMstObj.PageBizUnit = Convert.ToInt16(CurrentUser.SBUID);
                        dsbPageMstObj.PageDept = CurrentUser.CurrentDeptPK;
                        dsbPageMstLst = dashBoardAdminServiceClient.GetDsbPageMst(dsbPageMstObj, serviceUtilityObj);
                        serviceUtilityObj = dashBoardAdminServiceClient.GetDsbPageMstCount(dsbPageMstObj, serviceUtilityObj);
                        TotalPages = (serviceUtilityObj.TotalRecords == 0) ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 : (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) : (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;

                    case ControlsEnum.PAGE:
                        dsbPageMstObj = dashBoardAdminServiceClient.GetInitilizedDsbPageMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        dsbPageMstObj.PagePk = CurrPage;
                        dsbPageMstObj.PageBizUnit = Convert.ToInt16(CurrentUser.SBUID);
                        dsbPageMstObj.PageDept = CurrentUser.CurrentDeptPK;
                        dsbPageMstLst = dashBoardAdminServiceClient.GetDsbPageMst(dsbPageMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.DASHLET:
                        dsbDashletMstObj = dashBoardAdminServiceClient.GetInitilizedDsbDashletMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        dsbDashletMstObj.DashletPk = CurrDashlet;
                        dsbDashletMstLst = dashBoardAdminServiceClient.GetDsbDashletMst(dsbDashletMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.FILTER:
                        dsbFilterParameterMstObj = dashBoardAdminServiceClient.GetInitilizedDsbFilterParameterMst();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = -1;
                        serviceUtilityObj.PageSize = -1;
                        serviceUtilityObj.SortBy = Resources.DataFieldRes.DashletParameterPK;
                        serviceUtilityObj.SortDirection = Resources.Report.SortAscending;
                        dsbFilterParameterMstObj.ParamDashletPk = CurrDashlet;
                        dsbFilterParameterMstObj.ParamPk = 0;
                        dsbFilterParameterMstLst = dashBoardAdminServiceClient.GetDsbFilterParameterMst(dsbFilterParameterMstObj, serviceUtilityObj);
                        break;

                    case ControlsEnum.RDLCFILTER:
                        reportData = DashboardBL.GetFilterData(reportProcedure, reportParameters);
                        break;

                    case ControlsEnum.RDLCREPORT:
                        result = DashboardBL.GetChartData(reportProcedure, reportParameters);
                        reportData = (DataSet)result[1];
                        reportTitle = (string)result[0];
                        break;
                }
                dashBoardAdminServiceClient.Close();
            }
            catch (Exception ex)
            {
                dashBoardAdminServiceClient.Abort();
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
                    //Used Fill the UI Values From Object
                    case ControlsEnum.PAGE:
                        GetUIValuesFromObject();
                        break;
                    case ControlsEnum.DEFAULT:
                        GetUIValuesFromObject();
                       // BindPager();
                        break;
                    case ControlsEnum.DASHLET:
                        RemoveChart(divPopupDashlet);
                        BindDashlet();
                        break;
                    case ControlsEnum.FILTER:
                        BindFilter();
                        break;
                    case ControlsEnum.FILTERDASHLET:
                        BindFilter();
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject()
        {
            Table ChartTable;
            LayoutEnum dimension;
            int j;
            ChartTable = new Table();
            hasChart = false;
            try
            {
                //Remove existing charts
                RemoveChart(ChartCell00);
                RemoveChart(ChartCell01);
                RemoveChart(ChartCell10);
                RemoveChart(ChartCell11);
                RemoveChart(ChartCell20);
                RemoveChart(ChartCell21);

                ChartTable0.Visible = false;
                ChartTable1.Visible = false;
                ChartTable2.Visible = false;

                if (dsbPageMstLst != null && dsbPageMstLst.Count() > 0)
                {
                    CurrPage = dsbPageMstLst[0].PagePk;

                    ////To Set BreadCrumb according to page
                    if (!string.IsNullOrEmpty(dsbPageMstLst[0].PageTitle))
                    {
                        breadCrumb = GetLocalResourceObject("Breadcrumb").ToString() + ">>" + dsbPageMstLst[0].PageTitle;
                        
                        breadCrumb = breadCrumb.Replace(">>", "<span style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px;float:none'>&#9658;&#9658;</span>");
                        //breadCrumb = breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;&#9658;</label>");
                    }

                    //For RDLC
                    if (dsbPageMstLst.Count == 1 && dsbPageMstLst[0].DsbRowMsts != null && dsbPageMstLst[0].DsbRowMsts.Count == 1 &&
                    dsbPageMstLst[0].DsbRowMsts[0].DsbDashletMsts != null && dsbPageMstLst[0].DsbRowMsts[0].DsbDashletMsts.Count == 1
                    && (ChartsEnum)dsbPageMstLst[0].DsbRowMsts[0].DsbDashletMsts[0].DsbChtSubTypeCfg.cstChartType == ChartsEnum.RDLC)
                    {
                        IsRdlc = true;
                        BindRdlc(dsbPageMstLst[0].DsbRowMsts[0].DsbDashletMsts[0]);
                    }

                    //For Other Charts
                    else
                    {
                        IsRdlc = false;
                        RdlcDiv.Visible = false;
                        ChartDiv.Visible = true;
                        for (int i = 0; i < dsbPageMstLst[0].DsbRowMsts.Count && i < 3; i++)
                        {
                            j = 0;
                            //Select the Chart Table
                            if (i == 0)
                                ChartTable = ChartTable0;
                            else if (i == 1)
                                ChartTable = ChartTable1;
                            else if (i == 2)
                                ChartTable = ChartTable2;
                            hasChart = hasChart ? hasChart : dsbPageMstLst[0].DsbRowMsts[i].DsbDashletMsts.Count > 0;
                            dimension = (LayoutEnum)dsbPageMstLst[0].DsbRowMsts[i].RowLayout;
                            switch (dimension)
                            {
                                //If Row has only one Cell(full size Dashlet)
                                case LayoutEnum.FULL:
                                    BindDashletCell(i, 0, ChartTable, dimension);
                                    ChartTable.Rows[0].Cells[1].Visible = false;
                                    break;
                                //If Row has 2 Cells(all other options)
                                default:
                                    for (j = 0; j < dsbPageMstLst[0].DsbRowMsts[i].DsbDashletMsts.Count && j < 2; j++)
                                    {
                                        BindDashletCell(i, j, ChartTable, dimension);
                                    }
                                    if (j == 1)
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ToggleCell", "$('#" + ChartTable.Rows[0].Cells[1].ClientID + "').css('visibility', 'hidden');", true);
                                    break;
                            }
                        }
                    }
                }
                //If Page doesn't exist, fetch default page
                else
                {
                    if (PageIndex != null && Convert.ToInt32(PageIndex) > 1)
                    {
                        PageIndex = null;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for Pager binding
        /// </summary>
        private void BindPager()
        {
            try
            {
                uclPaging.TotalPages = TotalPages;
                PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                uclPaging.Visible = false ;
                uclPaging.BindPager();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To bind the Chart Table Cells
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="ChartTable"></param>
        /// <param name="dimension"></param>
        private void BindDashletCell(int row, int column, Table ChartTable, LayoutEnum dimension)
        {
            ChartsEnum chType;
            string chartDataXml;
            UpdatePanel upnlDashlet;
            HtmlGenericControl chartDiv;
            IDashboard dashboardHelper = null;
            Label lblHeader;
            Label lblHeadFilter;
            HiddenField hdfDashlet;
            int width;
            width = 299;
            upnlDashlet = new UpdatePanel();
            try
            {
                CurrDashlet = dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletPk;
                upnlDashlet.ID = "udplChart" + row.ToString() + column.ToString();
                chartDiv = new HtmlGenericControl("div");
                chartDiv.ID = "chartDiv" + row.ToString() + column.ToString();
                chartDiv.Attributes.Add("class", "chart-border");
                chType = (ChartsEnum)dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletChartType;
                if (DashboardConstants.IsMSChart == DashboardsEnum.MSCHART)
                {
                    switch (chType)
                    {
                        case ChartsEnum.GRID:
                            dashboardHelper = new GridChart();
                            break;
                    }
                }
                switch (dimension)
                {
                    case LayoutEnum.ONETOTWO:
                        if (column == 0)
                            width = 298;
                        else if (column == 1)
                            width = 598;
                        ChartTable.Rows[0].Cells[0].CssClass = DashboardConstants.OneToTwoStyle;//one-two-graph
                        ChartTable.Rows[0].Cells[1].CssClass = DashboardConstants.TwoToOneStyle;//two-one-graph
                        break;

                    case LayoutEnum.TWOTOONE:
                        if (column == 0)
                            width = 598;
                        else if (column == 1)
                            width = 298;
                        ChartTable.Rows[0].Cells[0].CssClass = DashboardConstants.TwoToOneStyle;//two-one-graph
                        ChartTable.Rows[0].Cells[1].CssClass = DashboardConstants.OneToTwoStyle;//one-two-graph
                        break;

                    case LayoutEnum.ONETOONE:
                        width = 448;
                        ChartTable.Rows[0].Cells[0].CssClass = DashboardConstants.HalfStyle;//half-graph
                        ChartTable.Rows[0].Cells[1].CssClass = DashboardConstants.HalfStyle;//half-graph
                        break;

                    case LayoutEnum.FULL:
                        width = 914;
                        ChartTable.Rows[0].Cells[0].CssClass = DashboardConstants.FullStyle;//full-graph
                        ChartTable.Rows[0].Cells[1].CssClass = DashboardConstants.NullStyle;//null-graph
                        break;
                }
                //Set Dashlet PK
                hdfDashlet = ChartTable.Rows[0].Cells[column].FindControl("hdfDashlet" + row.ToString() + column.ToString()) as HiddenField;
                if (hdfDashlet != null)
                    hdfDashlet.Value = dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletPk.ToString();

                //Set Dashlet Properties and Type
                dashboardHelper.SetChart(dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column]);
                dashboardHelper.SetProperty(dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DsbDshProprtyMpgs);

                //Set Default Filter Get Chart Data and Bind Chart
                if (chType == ChartsEnum.GRID)
                {
                    //(dashboardHelper as GridChart).PageSize = 10;
                    (dashboardHelper as GridChart).GridSize = (GridSizeEnum)dimension;
                    (dashboardHelper as GridChart).Service = dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletDataSource;
                    (dashboardHelper as GridChart).Method = dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletMethod;
                    (dashboardHelper as GridChart).GetData += GetServiceData;
                    chartDataXml = GetServiceData(dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletDataSource,
                        dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletMethod, (dashboardHelper as GridChart).PageSize, 1);
                }
                else
                {
                    dashboardHelper.SetDimension(248, width);
                    chartDataXml = GetServiceData(dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column]);
                }
                dashboardHelper.BindDataSource(chartDataXml);

                //Bind Chart Header
                lblHeader = ChartTable.Rows[0].Cells[column].FindControl("lblHeader" + row.ToString() + column.ToString()) as Label;
                lblHeadFilter = ChartTable.Rows[0].Cells[column].FindControl("lblHeadFilter" + row.ToString() + column.ToString()) as Label;
                //limit the Filter details length using maxlength
                BindDashletHeader(lblHeader, lblHeadFilter, dsbPageMstLst[0].DsbRowMsts[row].DsbDashletMsts[column].DashletTitle,
                    width == 298 ? 40 : width == 598 ? 80 : width == 448 ? 60 : width == 914 ? 120 : 40);

                //Add Chart to Container
                dashboardHelper.PrepareChart(chartDiv);
                upnlDashlet.ContentTemplateContainer.Controls.Add(chartDiv);
                ChartTable.Rows[0].Cells[column].Controls.Add(upnlDashlet);
                ChartTable.Rows[0].Cells[column].Visible = true;
                ChartTable.Visible = true;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
        /// <summary>
        ///  Method for Dashlet popup binding
        /// </summary>
        private void BindDashlet()
        {
            string chartDataXml;
            UpdatePanel upnlDashlet;
            HtmlGenericControl chartDiv;
            IDashboard dashboardHelper = null;
            ChartsEnum chType;
            try
            {
                if (dsbDashletMstLst != null && dsbDashletMstLst.Count() > 0)
                {
                    CurrDashlet = dsbDashletMstLst[0].DashletPk;
                    upnlDashlet = new UpdatePanel();
                    upnlDashlet.ID = "upnlDashletPopup";
                    chartDiv = new HtmlGenericControl("div");
                    chartDiv.ID = "chartPopupDiv";
                    chartDiv.Attributes.Add("class", DashboardConstants.GridWrapStyle);
                    chType = (ChartsEnum)dsbDashletMstLst[0].DashletChartType;
                    if (DashboardConstants.IsMSChart == DashboardsEnum.MSCHART)
                    {
                        switch (chType)
                        {
                            case ChartsEnum.GRID:
                                dashboardHelper = new GridChart();
                                break;
                        }
                    }
                    if (chType == ChartsEnum.GRID)
                    {
                        (dashboardHelper as GridChart).IsPopup = true;
                        (dashboardHelper as GridChart).GridSize = GridSizeEnum.Popup;
                        (dashboardHelper as GridChart).Service = dsbDashletMstLst[0].DashletDataSource;
                        (dashboardHelper as GridChart).Method = dsbDashletMstLst[0].DashletMethod;
                        (dashboardHelper as GridChart).GetData += GetServiceData;
                        chartDataXml = GetServiceData(dsbDashletMstLst[0].DashletDataSource,
                            dsbDashletMstLst[0].DashletMethod, (dashboardHelper as GridChart).PageSize, 1);
                    }
                    else
                    {
                        dashboardHelper.SetDimension(490, 910);
                        chartDataXml = GetServiceData(dsbDashletMstLst[0]);
                    }
                    dashboardHelper.SetChart(dsbDashletMstLst[0]);
                    dashboardHelper.SetProperty(dsbDashletMstLst[0].DsbDshProprtyMpgs);
                    dashboardHelper.BindDataSource(chartDataXml);
                    BindDashletHeader(null, lblPopHeadFilter, dsbDashletMstLst[0].DashletTitle, 140);

                    dashboardHelper.PrepareChart(chartDiv);
                    upnlDashlet.ContentTemplateContainer.Controls.Add(chartDiv);
                    divPopupDashlet.Controls.Add(upnlDashlet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  Method for Rdlc binding
        /// </summary>
        private void BindRdlc(DsbDashletMst reportDashlet)
        {
            LocalReport lptChart;
            ReportDataSource rdsChart;
            FilterParameters parameter;
            ReportParameter parameters;
            object chartData;
            try
            {
                if (reportDashlet != null)
                {
                    CurrDashlet = reportDashlet.DashletPk;
                    hdfDashletRdlc.Value = reportDashlet.DashletPk.ToString();
                    if ((ChartsEnum)reportDashlet.DashletChartType == ChartsEnum.RDLC)
                    {
                        ChartDiv.Visible = false;
                        rvrChart.Visible = true;
                        rvrChart.LocalReport.DataSources.Clear();
                        lptChart = rvrChart.LocalReport;
                        lptChart.ReportPath = Server.MapPath(DashboardConstants.ReportPath + reportDashlet.DashletReportUrl);
                        if (reportDashlet.DashletReportUrl == "rptStockTransactionReportType.rdlc")
                        {
                            lptChart.SubreportProcessing += new SubreportProcessingEventHandler(this.GenerateSubReport);
                        }
                        lptChart.EnableExternalImages = true;
                        lptChart.EnableHyperlinks = true;


                        if (string.IsNullOrEmpty(CurrFilter))
                        {
                            parameter = new FilterParameters();
                            parameter.BizUnit = CurrentUser.SBUID;
                            parameter.Dept = CurrentUser.CurrentDeptPK;
                            parameter.FromDate = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                            parameter.ToDate = DateTime.Now;
                            if (!string.IsNullOrEmpty(txtFromDateSch.Text))
                            {
                                parameter.FromDate = Convert.ToDateTime(txtFromDateSch.Text);
                            }
                            if (!string.IsNullOrEmpty(txtToDateSch.Text))
                            {
                                parameter.ToDate = Convert.ToDateTime(txtToDateSch.Text);
                            }
                            CurrFilter = parameter.XmlSerialize();
                        }
                        //Bind Service Data
                        if (reportDashlet.DashletSourceIsService)
                        {
                            chartData = GetServiceData(reportDashlet);
                            rdsChart = new ReportDataSource(DashboardConstants.ReportDataset, chartData);
                            lptChart.DataSources.Add(rdsChart);
                        }
                        //Bind Procedure Data
                        else
                        {
                            reportProcedure = reportDashlet.DashletDataSource;
                            reportParameters = CurrFilter;
                            GetFieldValues(ControlsEnum.RDLCREPORT);

                            for (int k = 0; k < reportData.Tables.Count; k++)
                            {
                                rdsChart = new ReportDataSource(DashboardConstants.ReportDataset + (k + 1).ToString(), reportData.Tables[k]);
                                lptChart.DataSources.Add(rdsChart);
                            }
                            //Set Report Filter Title
                            parameters = new ReportParameter();
                            parameters.Name = DashboardConstants.FilterTitle;
                            parameters.Values.Add(reportTitle);
                           
                            lptChart.SetParameters(parameters);
                        }
                      
                        parameters = new ReportParameter("Title", CurrentUser.CurrentSBU);
                        lptChart.SetParameters(parameters);
                        object[] strArg = new object[2];
                        strArg[0] = CurrentUser.UserName;
                        strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                        parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                        lptChart.SetParameters(parameters);
                       
                        parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath(DashboardConstants.ImagePath + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                        lptChart.SetParameters(parameters);
                        rvrChart.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        lptChart.Refresh();
                        BindDashletHeader(lblHeaderRdlc, lblHeadFilterRdlc, reportDashlet.DashletTitle, 120);
                        RdlcDiv.Visible = true;
                        hasChart = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GenerateSubReport(object sender, SubreportProcessingEventArgs e)
        {

            try
            {
                int pk = 0;
                int type = 0;

                int.TryParse(e.Parameters["PK"].Values[0].ToString(), out pk);
                int.TryParse(e.Parameters["Type"].Values[0].ToString(), out type);

                string xml = String.Empty;//Gets thepacking list to report view 
                if (type == 1)
                {
                    if (e.ReportPath == "rptStockTransferReportType")
                    {
                        xml = DashboardBL.GetStockTransferRptDetails(pk);
                        DataSet dsReport = new DataSet();
                        if (xml != "")
                        {
                            dsReport.ReadXml(new XmlTextReader(new StringReader(xml)));
                        }

                        if (dsReport != null)
                        {

                            ReportDataSource dsStockTrfHeader;
                            if (dsReport.Tables.Count > 0)
                            {
                                dsStockTrfHeader = new ReportDataSource("StockTrfHeader", dsReport.Tables[0]);
                            }
                            else
                            {
                                dsStockTrfHeader = new ReportDataSource("StockTrfHeader", new DataTable());
                            }

                            ReportDataSource dsPODetail;
                            if (dsReport.Tables.Count > 1)
                            {
                                dsPODetail = new ReportDataSource("PODetail", dsReport.Tables[1]);
                            }
                            else
                            {
                                dsPODetail = new ReportDataSource("PODetail", new DataTable());
                            }

                            ReportDataSource dsPRDetail;
                            if (dsReport.Tables.Count > 2)
                            {
                                dsPRDetail = new ReportDataSource("PRDetail", dsReport.Tables[2]);
                            }
                            else
                            {
                                dsPRDetail = new ReportDataSource("PRDetail", new DataTable());
                            }

                            ReportDataSource dsAddnlDetail;
                            if (dsReport.Tables.Count == 4)
                            {
                                dsAddnlDetail = new ReportDataSource("AddnlDetail", dsReport.Tables[3]);
                            }
                            else
                            {
                                dsAddnlDetail = new ReportDataSource("AddnlDetail", new DataTable());
                            }
                            e.DataSources.Add(dsStockTrfHeader);
                            e.DataSources.Add(dsPODetail);
                            e.DataSources.Add(dsPRDetail);
                            e.DataSources.Add(dsAddnlDetail);




                        }
                    }
                    else
                    {
                        ReportDataSource dsHeader = new ReportDataSource("DataSet1", new DataTable());
                        ReportDataSource dsDtls = new ReportDataSource("DataSet2", new DataTable());
                        e.DataSources.Add(dsHeader);
                        e.DataSources.Add(dsDtls);
                        dsHeader = new ReportDataSource("StockAdjHeader", new DataTable());
                        dsDtls = new ReportDataSource("StockAdjDetail", new DataTable());
                        e.DataSources.Add(dsHeader);
                        e.DataSources.Add(dsDtls);
                    }
                }
                else if ((type == 2) || (type == 3))
                {
                    if (e.ReportPath == "rptStoreIssueReportType")
                    {
                        xml = DashboardBL.GetMaterialIssueDetailsForReport(pk);
                        DataSet dsReport = new DataSet();
                        dsReport.ReadXml(new XmlTextReader(new StringReader(xml)));

                        if (dsReport != null)
                        {
                            if (dsReport.Tables.Count > 0)
                            {
                                ReportDataSource dsHeader = new ReportDataSource("DataSet1", dsReport.Tables[0]);
                                ReportDataSource dsDtls = new ReportDataSource("DataSet2", dsReport.Tables[1]);
                                e.DataSources.Add(dsHeader);
                                e.DataSources.Add(dsDtls);
                            }
                        }

                    }
                    else
                    {
                        ReportDataSource dsStockTrfHeader = new ReportDataSource("StockTrfHeader", new DataTable());
                        ReportDataSource dsPODetail = new ReportDataSource("PODetail", new DataTable());
                        ReportDataSource dsPRDetail = new ReportDataSource("PRDetail", new DataTable());
                        ReportDataSource dsAddnlDetail = new ReportDataSource("AddnlDetail", new DataTable());

                        e.DataSources.Add(dsStockTrfHeader);
                        e.DataSources.Add(dsPODetail);
                        e.DataSources.Add(dsPRDetail);
                        e.DataSources.Add(dsAddnlDetail);

                        ReportDataSource dsHeader = new ReportDataSource("StockAdjHeader", new DataTable());
                        ReportDataSource dsDtls = new ReportDataSource("StockAdjDetail", new DataTable());
                        e.DataSources.Add(dsHeader);
                        e.DataSources.Add(dsDtls);
                    }
                }
                else if (type == 4)
                {
                    if (e.ReportPath == "rptStoreAdjustmentReportType")
                    {
                        xml = DashboardBL.GetStockAdjustmentDetailsForReport(pk);

                        DataSet dsReport = new DataSet();
                        dsReport.ReadXml(new XmlTextReader(new StringReader(xml)));

                        if (dsReport != null)
                        {
                            if (dsReport.Tables.Count > 0)
                            {
                                ReportDataSource dsHeader = new ReportDataSource("StockAdjHeader", dsReport.Tables[0]);
                                ReportDataSource dsDtls = new ReportDataSource("StockAdjDetail", dsReport.Tables[1]);
                                e.DataSources.Add(dsHeader);
                                e.DataSources.Add(dsDtls);
                            }
                        }

                    }
                    else
                    {
                        ReportDataSource dsStockTrfHeader = new ReportDataSource("StockTrfHeader", new DataTable());
                        ReportDataSource dsPODetail = new ReportDataSource("PODetail", new DataTable());
                        ReportDataSource dsPRDetail = new ReportDataSource("PRDetail", new DataTable());
                        ReportDataSource dsAddnlDetail = new ReportDataSource("AddnlDetail", new DataTable());

                        e.DataSources.Add(dsStockTrfHeader);
                        e.DataSources.Add(dsPODetail);
                        e.DataSources.Add(dsPRDetail);
                        e.DataSources.Add(dsAddnlDetail);

                        ReportDataSource dsHeader = new ReportDataSource("DataSet1", new DataTable());
                        ReportDataSource dsDtls = new ReportDataSource("DataSet2", new DataTable());
                        e.DataSources.Add(dsHeader);
                        e.DataSources.Add(dsDtls);
                    }
                }

            }
            catch (Exception ex)
            {
               // lblerrorEx.Text = ex.ToString();
            }

        }

      

        /// <summary>
        /// To bind the Header and Filter Detail Labels
        /// </summary>
        /// <param name="header"></param>
        /// <param name="filter"></param>
        /// <param name="title"></param>
        /// <param name="maxlength"></param>
        private void BindDashletHeader(Label header, Label filter, string title, int maxlength)
        {
            FilterParameters filterParams;
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
                        filterParams = FilterParameters.XmlDeserialize(CurrFilter);
                        if (filterParams != null)
                        {
                            if (filterParams.FromDate != DateTime.MinValue || filterParams.ToDate != DateTime.MinValue)
                            {
                                dateFilter = "<b>Date:</b> ";
                                dateFilter += filterParams.FromDate != DateTime.MinValue ? filterParams.FromDate.ToString(Resources.Report.DateFormat) : string.Empty;
                                dateFilter += filterParams.FromDate != DateTime.MinValue && filterParams.ToDate != DateTime.MinValue ? " to " : string.Empty;
                                dateFilter += filterParams.ToDate != DateTime.MinValue ? filterParams.ToDate.ToString(Resources.Report.DateFormat) : string.Empty;
                            }
                            if (filterParams.Parameters != null && filterParams.Parameters.Count > 0)
                            {
                                foreach (GTIService.Dashboard.Parameter prs in filterParams.Parameters)
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
                            filter.Text = CommonFunctions.GetShortString(filter.ToolTip, filter.ToolTip.Substring(0, maxlength).EndsWith("<") || filter.ToolTip.Substring(0, maxlength).EndsWith("</")
                                || filter.ToolTip.Substring(0, maxlength).EndsWith("<b") || filter.ToolTip.Substring(0, maxlength).EndsWith("</b") ? maxlength + 3 : maxlength);
                        else
                            filter.Text = CommonFunctions.GetShortString(filter.ToolTip, maxlength);
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
        private void BindFilter()
        {
            FilterParameters filterParams;
            try
            {
                txtFromDateSch.Text = string.Empty;
                txtToDateSch.Text = string.Empty;
                if (!string.IsNullOrEmpty(CurrFilter))
                {
                    filterParams = FilterParameters.XmlDeserialize(CurrFilter);
                    if (filterParams != null)
                    {
                        if (filterParams.FromDate != DateTime.MinValue)
                            txtFromDateSch.Text = filterParams.FromDate.ToString(Resources.Report.DateFormat);
                        if (filterParams.ToDate != DateTime.MinValue)
                            txtToDateSch.Text = filterParams.ToDate.ToString(Resources.Report.DateFormat);
                    }
                }
                rptFilter.DataSource = dsbFilterParameterMstLst;
                rptFilter.DataBind();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPopFilter", "ShowContainerDiv('#divFilterPopup', 'Filter Parameters', '960', '600');", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Get Service

        /// <summary>
        /// Method to get Service Chart Data
        /// </summary>
        /// <param name="dashletObj"></param>
        /// <returns></returns>
        private string GetServiceData(DsbDashletMst dashletObj)
        {
            object result;
            string xml;
            FilterParameters parameter;
            if (string.IsNullOrEmpty(CurrFilter))
            {
                parameter = new FilterParameters();
                parameter.BizUnit = CurrentUser.SBUID;
                parameter.Dept = CurrentUser.CurrentDeptPK;
                parameter.FromDate = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                parameter.ToDate = DateTime.Now;
                if (!string.IsNullOrEmpty(txtFromDateSch.Text))
                {
                    parameter.FromDate = Convert.ToDateTime(txtFromDateSch.Text);
                }
                if (!string.IsNullOrEmpty(txtToDateSch.Text))
                {
                    parameter.ToDate = Convert.ToDateTime(txtToDateSch.Text);
                }
                CurrFilter = parameter.XmlSerialize();
            }
            result = GetServiceData(dashletObj.DashletDataSource, dashletObj.DashletMethod, CurrFilter);
            xml = (string)result;
            return xml;
        }
        /// <summary>
        /// Method to get Service Grid Data
        /// </summary>
        /// <param name="serviceWsdlUri"></param>
        /// <param name="serviceMethod"></param>
        /// <param name="pageSize"></param>
        /// <param name="currentPage"></param>
        /// <returns></returns>
        private string GetServiceData(string serviceWsdlUri, string serviceMethod, int pageSize, int currentPage)
        {
            object result;
            string xml;
            FilterParameters parameter;
            if (string.IsNullOrEmpty(CurrFilter))
            {
                parameter = new FilterParameters();
                parameter.BizUnit = CurrentUser.SBUID;
                parameter.Dept = CurrentUser.CurrentDeptPK;
                parameter.FromDate = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                parameter.ToDate = DateTime.Now;
                if (!string.IsNullOrEmpty(txtFromDateSch.Text))
                {
                    parameter.FromDate = Convert.ToDateTime(txtFromDateSch.Text);
                }
                if (!string.IsNullOrEmpty(txtToDateSch.Text))
                {
                    parameter.ToDate = Convert.ToDateTime(txtToDateSch.Text);
                }
            }
            else
                parameter = FilterParameters.XmlDeserialize(CurrFilter);
            parameter.PageSize = pageSize;
            parameter.PageNumber = currentPage;
            CurrFilter = parameter.XmlSerialize();
            result = GetServiceData(serviceWsdlUri, serviceMethod, CurrFilter);
            xml = (string)result;
            return xml;
        }
        /// <summary>
        /// Method to get Service Parameter Data
        /// </summary>
        /// <param name="serviceWsdlUri"></param>
        /// <param name="serviceMethod"></param>
        /// <returns></returns>
        private GTIService.Dashboard.Parameter GetFilterParamData(string serviceWsdlUri, string serviceMethod)
        {
            GTIService.Dashboard.Parameter rlt;
            object result;
            string xml;
            GTIService.Dashboard.FilterParameters parameter;
            parameter = new GTIService.Dashboard.FilterParameters();
            parameter.BizUnit = CurrentUser.SBUID;
            parameter.Dept = CurrentUser.CurrentDeptPK;
            parameter.PageSize = 0;
            result = GetServiceData(serviceWsdlUri, serviceMethod, parameter.XmlSerialize());
            xml = (string)result;
            rlt = GTIService.Dashboard.Parameter.XmlDeserialize(xml);
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
            object result;
            System.ServiceModel.BasicHttpBinding binding;
            intfcName = serviceWsdlUri.Split('/').Last().Split('.').First();
            intfcName = intfcName.Trim() == string.Empty ? string.Empty : "I" + intfcName;
            serviceWsdlUri = serviceWsdlUri + "?wsdl";
            DynamicProxyFactory factory = new DynamicProxyFactory(serviceWsdlUri);
            binding = (System.ServiceModel.BasicHttpBinding)factory.Bindings.First();
            binding.TransferMode = System.ServiceModel.TransferMode.Streamed;
            binding.MaxBufferPoolSize = 2147483646;
            binding.MaxBufferSize = 2147483646;
            binding.MaxReceivedMessageSize = 2147483646;
            binding.ReaderQuotas.MaxStringContentLength = 2147483646;


            DynamicProxy serviceProxy = factory.CreateProxy(intfcName);
            result = serviceProxy.CallMethod(serviceMethod, methodParam);
            return result;
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
            ZoomPK = string.Empty;
            txtFromDateSch.Text = string.Empty;
            txtToDateSch.Text = string.Empty;
            hdfIsAlreadyFiltered.Value = string.Empty;
        }

        #region Code to Remove

        private string GetData(string service)
        {
            GTIService.Dashboard.ChartData chartSource = new GTIService.Dashboard.ChartData();
            chartSource.SeriesCount = 9;
            chartSource.ChartItems = new List<ChartItem>();
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
                new ChartItem()
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
                new ChartItem()
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
                new ChartItem()
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
                new ChartItem()
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
                new ChartItem()
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


            chartSource.TotalPages = 5 % pageSize > 0 ? 5 / pageSize + 1 : 5 / pageSize;
            result = chartSource.XmlSerialize();
            return result;
        }

        private string GetGaugeData(string service)
        {
            GTIService.Dashboard.ChartData chartSource = new GTIService.Dashboard.ChartData();
            chartSource.SeriesCount = 2;
            chartSource.ChartTitles = new List<string>();
            chartSource.ChartTitles.Add("Speed");
            chartSource.ChartTitles.Add("RPM");
            chartSource.ChartTitles.Add("Fuel");
            chartSource.GuageItems = new List<GaugeItem>();
            chartSource.GuageItems.Add(
                new GaugeItem()
                {
                    min = 0,
                    max = 100,
                    value = 45
                }
            );
            chartSource.GuageItems.Add(
                new GaugeItem()
                {
                    min = 0,
                    max = 500,
                    value = 350
                }
            );
            chartSource.GuageItems.Add(
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
            TableCell tcDashlet;
            HiddenField hdfDashlet;
            int dashletPK;
            FilterParameters filterParams;
            HiddenField hdfFilterParam;
            Label lblFilterLabel;
            CheckBoxList cblFilterParams;
            GTIService.Dashboard.Parameter prms;
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
                switch (commonActions)
                {
                    #region ZOOM
                    // Do Action for , when click ZOOM button
                    case ActionsEnum.ZOOM:
                        tcDashlet = ((Button)sender).Parent as TableCell;
                        if (tcDashlet != null)
                        {
                            hdfDashlet = null;
                            foreach (Control ctr in tcDashlet.Controls)
                            {
                                if (ctr.GetType().Equals(typeof(HiddenField)))
                                {
                                    hdfDashlet = (HiddenField)ctr;
                                    break;
                                }
                            }

                            if (hdfDashlet != null && int.TryParse(hdfDashlet.Value, out dashletPK))
                            {
                                ZoomPK = ((Button)sender).ClientID;
                                CurrDashlet = dashletPK;
                                hdfPopupDashletPk.Value = dashletPK.ToString();
                                GetFieldValues(ControlsEnum.DASHLET);
                                SetFieldValues(ControlsEnum.DASHLET);
                                isZoom = true;
                            }
                        }
                        break;
                    #endregion

                    #region FILTER

                    // Do Action for , when click FILTER button
                    case ActionsEnum.FILTER:
                        tcDashlet = ((Button)sender).Parent as TableCell;
                        if (tcDashlet != null)
                        {
                            hdfDashlet = null;
                            foreach (Control ctr in tcDashlet.Controls)
                            {
                                if (ctr.GetType().Equals(typeof(HiddenField)))
                                {
                                    hdfDashlet = (HiddenField)ctr;
                                    break;
                                }
                            }

                            if (hdfDashlet != null && int.TryParse(hdfDashlet.Value, out dashletPK))
                            {
                                CurrDashlet = dashletPK;
                                hdfPopupDashletPk.Value = dashletPK.ToString();
                                GetFieldValues(ControlsEnum.FILTER);
                                SetFieldValues(ControlsEnum.FILTER);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SelectAll", "CheckListChange();", true);
                                isZoom = false;
                            }
                        }

                        break;

                    #endregion

                    #region FILTER RDLC

                    // Do Action for , when click FILTER button
                    case ActionsEnum.FILTERRDLC:
                        if (int.TryParse(hdfDashletRdlc.Value, out dashletPK))
                        {
                            CurrDashlet = dashletPK;
                            hdfPopupDashletPk.Value = dashletPK.ToString();
                            
                         //   hdfIsAlreadyFiltered.Value = CommonConstants.SELECT_VALUE_ONE;
                            GetFieldValues(ControlsEnum.FILTER);
                            SetFieldValues(ControlsEnum.FILTER);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SelectAll", "CheckListChange();", true);
                            isZoom = false;
                        }

                        break;

                    #endregion

                    #region FILTERDASHLET

                    // Do Action for , when click FILTERDASHLET button
                    case ActionsEnum.FILTERDASHLET:
                        CurrDashlet = Convert.ToInt32(hdfPopupDashletPk.Value);
                        GetFieldValues(ControlsEnum.FILTER);
                        SetFieldValues(ControlsEnum.FILTER);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SelectAll", "CheckListChange();", true);
                        isZoom = false;
                        break;

                    #endregion

                    #region SELECT
                    // Do Action for , when click Apply button
                    case ActionsEnum.SELECT:
                        filterParams = new FilterParameters();
                        filterParams.BizUnit = CurrentUser.SBUID;
                        filterParams.Dept = CurrentUser.CurrentDeptPK;
                        filterParams.FromDate = DateTime.Now.AddDays(1 - DateTime.Now.Day);
                        filterParams.ToDate = DateTime.Now;
                        if (!string.IsNullOrEmpty(txtFromDateSch.Text))
                        {
                            filterParams.FromDate = Convert.ToDateTime(txtFromDateSch.Text);
                        }
                        if (!string.IsNullOrEmpty(txtToDateSch.Text))
                        {
                            filterParams.ToDate = Convert.ToDateTime(txtToDateSch.Text);
                        }
                        filterParams.Parameters = new List<GTIService.Dashboard.Parameter>();
                        foreach (RepeaterItem itm in rptFilter.Items)
                        {
                            hdfFilterParam = itm.FindControl("hdfFilterField") as HiddenField;
                            lblFilterLabel = itm.FindControl("lblFilterField") as Label;
                            cblFilterParams = itm.FindControl("cblFilterParameter") as CheckBoxList;
                            if (hdfFilterParam != null && !string.IsNullOrEmpty(hdfFilterParam.Value) && cblFilterParams != null)
                            {
                                prms = new GTIService.Dashboard.Parameter();
                                prms.Name = hdfFilterParam.Value.Trim();
                                prms.Label = lblFilterLabel == null || string.IsNullOrEmpty(lblFilterLabel.Text) ? prms.Name : lblFilterLabel.Text.Trim();
                                prms.Values = new List<ParamItem>();
                                foreach (ListItem litem in cblFilterParams.Items)
                                {
                                    if (litem.Selected)
                                    {
                                        prms.Values.Add(
                                            new ParamItem()
                                            {
                                                Name = litem.Text,
                                                Value = Convert.ToInt32(litem.Value)
                                            });
                                        if (litem.Value == CommonConstants.SELECTVAL)
                                            break;
                                    }
                                }
                                if (prms.Values.Count > 0)
                                    filterParams.Parameters.Add(prms);
                            }
                        }
                        CurrDashlet = Convert.ToInt32(hdfPopupDashletPk.Value);
                        CurrFilter = filterParams.XmlSerialize();
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        

                        if (ZoomPK != string.Empty)
                        {
                            CurrDashlet = Convert.ToInt32(hdfPopupDashletPk.Value);
                            GetFieldValues(ControlsEnum.DASHLET);
                            SetFieldValues(ControlsEnum.DASHLET);
                            isZoom = true;
                        }
                        else
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion

                    #region CANCEL
                    // Do Action for , when click Cancel button
                    case ActionsEnum.CANCEL:
                        CurrDashlet = Convert.ToInt32(hdfPopupDashletPk.Value);
                        CurrFilter = null;
                        txtFromDateSch.Text = string.Empty;
                        txtToDateSch.Text = string.Empty;
                        hdfIsAlreadyFiltered.Value = string.Empty;
                        GetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.DEFAULT);
                        if (ZoomPK != string.Empty)
                        {
                            CurrDashlet = Convert.ToInt32(hdfPopupDashletPk.Value);
                            GetFieldValues(ControlsEnum.DASHLET);
                            SetFieldValues(ControlsEnum.DASHLET);
                            isZoom = true;
                        }
                        else
                        {
                            isZoom = false;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
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
        /// Handling Repeater Item Created event
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(Object Sender, RepeaterItemEventArgs e)
        {
            FilterParameters parameter;
            CheckBoxList cbl;
            GTIService.Dashboard.Parameter paramList;
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
                            parameter = new FilterParameters();
                            parameter.BizUnit = CurrentUser.SBUID;
                            parameter.Dept = CurrentUser.CurrentDeptPK;
                            reportParameters = parameter.XmlSerialize();
                            GetFieldValues(ControlsEnum.RDLCFILTER);
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
            FilterParameters filterParams;
            DsbFilterParameterMst dataRow;
            isAll = true;
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    cbl = e.Item.FindControl("cblFilterParameter") as CheckBoxList;
                    if (cbl != null)
                        cbl.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));

                    dataRow = e.Item.DataItem as DsbFilterParameterMst;
                    if (dataRow != null)
                    {
                        if (!string.IsNullOrEmpty(CurrFilter))
                        {
                            filterParams = FilterParameters.XmlDeserialize(CurrFilter);
                            if (filterParams != null)
                            {
                                foreach (GTIService.Dashboard.Parameter pms in filterParams.Parameters)
                                {
                                    if (dataRow.ParamName == pms.Name)
                                    {
                                        foreach (ParamItem pmr in pms.Values)
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

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }


        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new FirstPageEventHandler(this.FirstPage);
            this.uclPaging.PreviousPage += new PreviousPageEventHandler(this.PreviousPage);
            this.uclPaging.NextPage += new NextPageEventHandler(this.NextPage);
            this.uclPaging.LastPage += new LastPageEventHandler(this.LastPage);
            this.uclPaging.PageChanged += new PageChangedEventHandler(this.PageChanged);
            this.Init += new EventHandler(this.Page_Init);
        }

        protected void FirstPage(object sender, DataNavigatorEventArgs e)
        {
            uclPaging.CurrentPage = 1;
            PageIndex = uclPaging.CurrentPage.ToString();
            if (e.CurrentPage > 1)
            {
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void PreviousPage(object sender, DataNavigatorEventArgs e)
        {
            uclPaging.CurrentPage--;
            PageIndex = uclPaging.CurrentPage.ToString();
            if (e.CurrentPage > 1)
            {
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void NextPage(object sender, DataNavigatorEventArgs e)
        {
            uclPaging.CurrentPage++;
            PageIndex = uclPaging.CurrentPage.ToString();
            if (e.CurrentPage <= e.TotalPages)
            {
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void LastPage(object sender, DataNavigatorEventArgs e)
        {
            uclPaging.CurrentPage = e.TotalPages;
            PageIndex = uclPaging.CurrentPage.ToString();
            if (e.CurrentPage <= e.TotalPages)
            {
                GetFieldValues(ControlsEnum.DEFAULT);
                SetFieldValues(ControlsEnum.DEFAULT);
                EnableDisableButtons(e.TotalPages);
            }
        }

        protected void PageChanged(object sender, DataNavigatorEventArgs e)
        {
            uclPaging.CurrentPage = e.CurrentPage;
            PageIndex = uclPaging.CurrentPage.ToString();
            GetFieldValues(ControlsEnum.DEFAULT);
            SetFieldValues(ControlsEnum.DEFAULT);
            EnableDisableButtons(e.TotalPages);
        }

        private void EnableDisableButtons(int iTotalPages)
        {
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;

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
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.QueryString["ReportNo"] != null)
                        ReportNo = Convert.ToInt32(Request.QueryString["ReportNo"]);
                    else
                        ReportNo = 1;

                    PageIndex = ReportNo.ToString();
                    uclPaging.CurrentPage = ReportNo;
                    uclPaging.Visible = false;

                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    //PageIndex = CommonConstants.SELECT_VALUE_ONE;
                    //uclPaging.TotalPages = TotalPages;
                    //uclPaging.CurrentPage = 1;

                }
                else if (!IsRdlc)
                {
                    //To bind the dynamic controls
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.PAGE);
                    if (ZoomPK != string.Empty)
                    {
                        CurrDashlet = Convert.ToInt32(hdfPopupDashletPk.Value);
                        GetFieldValues(ControlsEnum.DASHLET);
                        SetFieldValues(ControlsEnum.DASHLET);
                        isZoom = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Report.Information + "');", true);
            }
        }
        #endregion

        #region ControlEnum
        /// <summary>
        /// Controls Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            PAGE,
            DASHLET,
            FILTER,
            FILTERDASHLET,
            RDLCREPORT,
            RDLCFILTER
        }
        #endregion
    }
}