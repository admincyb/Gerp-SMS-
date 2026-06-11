using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Threading;
using BusinessObject.CommonManagement;
using DataAccess.StoreManagement;

namespace ERPSMS_v01.Reports
{
    public partial class StockReportEKK : System.Web.UI.Page
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                FillStore();
                FillCategory();
                StockDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                StockToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                hdfDateStock.Value = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                hdfStockToDate.Value = DateTime.Now.ToString("dd-MMM-yyyy");
                FillReport(DateTime.Now.AddDays(-1).ToString(), DateTime.Now.ToString(),0,  0);
            }
        }


        /// <summary>
        /// Fill Store Details To dropDown
        /// </summary>
        public void FillStore()
        {
            DataTable dtStoreDetails = BusinessLogic.StoreManagement.StockDetailsReport.GetAllStoreDtls(currentUser,0);
            ddlStore.Items.Clear();
            if (dtStoreDetails != null)
            {
                if (dtStoreDetails.Rows.Count > 0)
                {
                    ddlStore.DataSource = dtStoreDetails;
                    ddlStore.DataValueField = GTIService.Constants.Designation.Fields.DEPTPK;
                    ddlStore.DataTextField = GTIService.Constants.Designation.Fields.DEPTNAME;
                    ddlStore.DataBind();
                }
            }
            ddlStore.Items.Insert(0, new ListItem("All", "0"));
        }
        public void FillCategory()
        {
            DataTable dtStoreDetails = BusinessLogic.StoreManagement.GRNDailyReport.GetCategoryDtls(currentUser.SBUID);
            ddlCategory.Items.Clear();
            if (dtStoreDetails != null)
            {
                if (dtStoreDetails.Rows.Count > 0)
                {
                    ddlCategory.DataSource = dtStoreDetails;
                    ddlCategory.DataValueField = GTIService.Constants.Material.Fields.CATEGORYPK;
                    ddlCategory.DataTextField = GTIService.Constants.Material.Fields.CATEGORYNAME;
                    ddlCategory.DataBind();
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("All", "0"));
        }


        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// To Fill Details from dataable to Rdlc
        /// </summary>
        /// <param name="date"></param>
        /// <param name="store"></param>
        /// <param name="catg"></param>
        /// <param name="item"></param>
        private void FillReport(string date, string dateTo,int Category, int store)
        {
            LocalReport locRpt;
            RptViewerReport.LocalReport.DataSources.Clear();
            
            try
            {
                DataTable dtStockDtls = BusinessLogic.StoreManagement.StockDetailsReport.GetStockDetailsForRpt(date, store, dateTo,Category, currentUser.SBUID);
                if (dtStockDtls.Rows.Count > 0)
                {
                    RptViewerReport.Visible = true;
                    ReportDataSource dsGRNDailyDtls = new ReportDataSource("StockDetails", dtStockDtls);
                    locRpt = null;
                    locRpt = RptViewerReport.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("StockDetailsNew1_EKK.rdlc");//StockReport_EKK.rdlc
                    RptViewerReport.Height = 500;
                    RptViewerReport.LocalReport.DataSources.Clear();
                    locRpt.EnableExternalImages = true;
                    //ReportParameter parameters;
                    //parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                    //locRpt.SetParameters(parameters);
                    //object[] strArg = new object[2];
                    //strArg[0] = currentUser.UserName;
                    //strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    //parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                    //locRpt.SetParameters(parameters);
                    //parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                    //locRpt.SetParameters(parameters);
                    //parameters = new ReportParameter("HeadTitle", "Stock Details Report");
                    //locRpt.SetParameters(parameters);
                    ////Current Date in Report
                    ////parameters = new ReportParameter("Date", DateTime.Now.ToString("dd-MMM-yyyy"));
                    ////locRpt.SetParameters(parameters);
                    ////string ShowItemAmount = "0";
                    ////DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ShowItemAmount", string.Empty, currentUser.SBUID);
                    ////if (dt != null && dt.Rows.Count > 0)
                    ////{
                    ////    ShowItemAmount = dt.Rows[0]["ACF_VALUE"].ToString();

                    ////}
                    //parameters = new ReportParameter("ShowItemAmount", ShowItemAmount);
                    //locRpt.SetParameters(parameters);
                     SetReportParameters(locRpt);

                    RptViewerReport.LocalReport.DataSources.Add(dsGRNDailyDtls);
                    RptViewerReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    RptViewerReport.LocalReport.Refresh();
                }
                else
                {
                    RptViewerReport.Visible = false;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                }
            }
            catch (Exception ex)
            {

            }
        }

             /// <summary>
        /// To set report parameters
        /// </summary>
        /// <param name="locRpt"></param>
        private void SetReportParameters(LocalReport locRpt)
        {
            ReportParameter parameters;
            string footer;
            string rptName = string.Empty;
            footer = string.Empty;

            //locRpt.ReportPath = string.Empty;
            
            //locRpt.ReportPath = Server.MapPath("StockReport_EKK.rdlc");// ("RptTemplateWithLogo.rdlc");
            locRpt.EnableHyperlinks = true;

            parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HideLogo", "false");
            locRpt.SetParameters(parameters);

            parameters = new ReportParameter("HeadTitle", currentUser.CurrentSBU);
            locRpt.SetParameters(parameters);
            //parameters = new ReportParameter("HeadTitle", "EKK");
            //locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HideHeadTitle", "false");
            locRpt.SetParameters(parameters);

            parameters = new ReportParameter("SubTitle", "Store Stock Statement (Details)");
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HideSubTitle", "false");
            locRpt.SetParameters(parameters);

            footer = "Printed On " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
            parameters = new ReportParameter("FooterText", footer);
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HideFooterText", "false");
            locRpt.SetParameters(parameters);

            parameters = new ReportParameter("HideQMSRef", "false");
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HidePageNo", "false");
            locRpt.SetParameters(parameters);
            //.ValueParameters!.ValueParameters!.Value

            //Page specific parameters
            parameters = new ReportParameter("StoreText", ddlStore.SelectedItem.ToString());
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("FromDate", hdfDateStock.Value);
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("ToDate", hdfStockToDate.Value);
            locRpt.SetParameters(parameters);

            string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
            //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
            string currencyformat = "#" + currencysep + "#0.";
            string NoFormat = "#" + currencysep + "#0.";
            string currencydecimals = "";
            string Nodecimal = string.Empty;
            DataTable dt = ConfigurationSettingsforReport();
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
            parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("CurrencyFormat", currencyformat);
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("NumberFormat", NoFormat);
            locRpt.SetParameters(parameters);

        }
       
        /// <summary>
        /// To search 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (StockDate.Text.Trim() == string.Empty)
            {
                StockDate.Text = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                StockToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            if (StockToDate.Text.Trim() == string.Empty)
            {
                StockToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            hdfDateStock.Value = StockDate.Text;
            hdfStockToDate.Value = StockToDate.Text;
            int Category = Convert.ToInt32(ddlCategory.SelectedValue);
            FillReport(StockDate.Text,StockToDate.Text,Convert.ToInt32(ddlCategory.SelectedValue), Convert.ToInt32(ddlStore.SelectedValue));
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
            {}
        }
    }
}