using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace ERPSMS_v01.Reports
{
    public partial class PRReport : ERP.Store.UI.MyBasePage
    {

        BusinessObject.User currentUser;

        #region Events

        /// <summary>
        /// Page Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string dateFrom = hdfFromDate.Value;
            string dateTO = hdfToDate.Value;
            if (!IsPostBack)
            {
                FillFilterValues(0);
                FillReport();
            }
        }
        /// <summary>
        /// Method for Filter By DropDown Change events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillFilterValues(Convert.ToInt32(ddlFilterBy.SelectedValue));
            SetSelectedDate();
        }
        /// <summary>
        /// Method for Image Button Search Click events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillReport();
            SetSelectedDate();
        }
        /// <summary>
        /// ReportViewer Drillthrough event for Item & Vendor Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RptViewerReport_Drillthrough(object sender, Microsoft.Reporting.WebForms.DrillthroughEventArgs e)
        {
            LocalReport localReport = (LocalReport)e.Report;
            string prNumber = localReport.OriginalParametersToDrillthrough[0].Values[0];
            int prPk = Convert.ToInt32(localReport.OriginalParametersToDrillthrough[1].Values[0]);
            ReportDataSource level1datasource;
            localReport.DataSources.Clear();
            if (e.ReportPath == "PRVendorReport")
            {
                DataTable dtVendour = BusinessLogic.ReportsManagement.PurchaseRequestReport.GetPRVendour(prPk, currentUser.SBUID);
                level1datasource = new ReportDataSource("PRVendor", dtVendour);
                localReport.EnableExternalImages = true;
                ReportParameter parameters;
                parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                localReport.SetParameters(parameters);
                object[] strArg = new object[2];
                strArg[0] = currentUser.UserName;
                strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                localReport.SetParameters(parameters);
                parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                localReport.SetParameters(parameters);
                localReport.DataSources.Add(level1datasource);
            }
            localReport.Refresh();
        }

        #endregion
        
        /// <summary>
        /// Method to fill the Filter Value DropDowm
        /// </summary>
        /// <param name="option"></param>
        private void FillFilterValues(int option)
        {
            ddlFilterValue.Items.Clear();
            switch (option)
            {
                case 0:
                    break;
                case 1:                    
                    string fromDate = DateFrom.Text;
                    string toDate = DateTo.Text;
                    DataTable dtPRNumbers = BusinessLogic.ReportsManagement.PurchaseRequestReport.GetPurchaseRequests(fromDate, toDate, -1, string.Empty, currentUser.SBUID);
                    ddlFilterValue.DataTextField = GTIService.Constants.Reports.Fields.PRR_NO;
                    ddlFilterValue.DataValueField = GTIService.Constants.Reports.Fields.PRR_NO;
                    ddlFilterValue.DataSource = dtPRNumbers;
                    ddlFilterValue.DataBind();
                    break;
                case 2:
                    ddlFilterValue.Items.Insert(0, new ListItem("All", "-1"));
                    DataTable dtStatus = BusinessLogic.ReportsManagement.PurchaseRequestReport.GetPRStatus(currentUser.SBUID);
                    ddlFilterValue.DataTextField = GTIService.Constants.Reports.Fields.PRR_STATUSTEXT;
                    ddlFilterValue.DataValueField = GTIService.Constants.Reports.Fields.PRR_STATUS;
                    ddlFilterValue.DataSource = dtStatus;
                    ddlFilterValue.DataBind();
                    break;
            }
            ddlFilterValue.Items.Insert(0, new ListItem("All", "-1"));
        }
        /// <summary>
        /// Method to fill the RDLC Report
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="filterBy"></param>
        /// <param name="filterValue"></param>
        private void FillReport()
        {
            string fromDate = string.Empty;
            string toDate = string.Empty;
            DateTime dt;
            if (DateTime.TryParse(DateFrom.Text, out dt) && DateTime.TryParse(DateTo.Text, out dt))
            {
                fromDate = DateFrom.Text;
                toDate = DateTo.Text;
            }
            int status = -1;
            string requestNumber = string.Empty;
            int option = Convert.ToInt32(ddlFilterBy.SelectedValue);
            if (option == 2 && ddlFilterValue.SelectedValue.Trim() != "-1")
            {
                status = Convert.ToInt32(ddlFilterValue.SelectedValue);
            }
            else if (option == 1 && ddlFilterValue.SelectedValue.Trim()!="-1")
            {
                requestNumber = ddlFilterValue.SelectedValue;
            }
            LocalReport locRpt;
            RptViewerReport.LocalReport.DataSources.Clear();
            try
            {
                DataTable dtPurchaseRequests = BusinessLogic.ReportsManagement.PurchaseRequestReport.GetPurchaseRequests(fromDate, toDate, status, requestNumber, currentUser.SBUID);
               
                if (dtPurchaseRequests.Rows.Count > 0)
                {
                    RptViewerReport.Visible = true;
                    ReportDataSource dsPurchaseRequest = new ReportDataSource("PurchaseRequestReport", dtPurchaseRequests);
                    locRpt = null;
                    locRpt = RptViewerReport.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    //locRpt.ReportPath = Server.MapPath("PurchaseRequestRpt.rdlc");
                    locRpt.ReportPath = Server.MapPath("PurchaseRequestItemsVendorsReport.rdlc");
                    locRpt.SubreportProcessing += new SubreportProcessingEventHandler(this.GenerateSubReport);

                    RptViewerReport.Height = 500;
                    RptViewerReport.LocalReport.DataSources.Clear();
                    locRpt.EnableExternalImages = true;
                    ReportParameter parameters;
                    parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                    locRpt.SetParameters(parameters);
                    object[] strArg = new object[2];
                    strArg[0] = currentUser.EmpName;
                    strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                    locRpt.SetParameters(parameters);
                    RptViewerReport.LocalReport.DataSources.Add(dsPurchaseRequest);
                    RptViewerReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    RptViewerReport.LocalReport.Refresh();
                }
                else
                {
                    RptViewerReport.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void GenerateSubReport(object sender, SubreportProcessingEventArgs e)
        {
                      
            int prPk =int.Parse( e.Parameters["PRId"].Values[0].ToString()); 
         
            DataTable dtVendour = BusinessLogic.ReportsManagement.PurchaseRequestReport.GetPRVendour(prPk, currentUser.SBUID);
            e.DataSources.Add(new ReportDataSource("PRVendor", dtVendour));

       

        }

        /// <summary>
        /// Method to set back the selected dates
        /// </summary>
        private void SetSelectedDate()
        {
            DateTime dt;
            if (DateTime.TryParse(DateFrom.Text, out dt) && DateTime.TryParse(DateTo.Text, out dt))
            {
                //To set Date Range
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dates", "_dtFrom='" + DateFrom.Text + "';_dtTo='" + DateTo.Text + "';", true);
            }
            else
            {
                //To reset Date Range
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "dates", "_dtFrom=null;_dtTo=null;", true);
            }
        }
    }
}