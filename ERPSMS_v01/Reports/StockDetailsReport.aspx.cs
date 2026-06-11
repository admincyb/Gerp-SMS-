using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic.StoreManagement;
using Microsoft.Reporting.WebForms;
using System.Threading;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StockDetailsReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                FillStore();
                FillMaterialCatgeory();
                FillMaterials();
                hdfDateStock.Value = string.Empty;
                FillReport( DateTime.Now.ToString(),0,0 ,0);
            }
        }


        /// <summary>
        /// Fill Store Details To dropDown
        /// </summary>
        public void FillStore()
        {
            DataTable dtStoreDetails = BusinessLogic.StoreManagement.StockDetailsReport.GetStoreDtls(currentUser);
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
        /// <summary>
        /// Fill material Category 
        /// </summary>
        public void FillMaterialCatgeory()
        {
            DataTable dtStoreDetails = GRNDailyReport.GetCategoryDtls(currentUser.SBUID);
            ddlMaterialCatg.Items.Clear();
            if (dtStoreDetails != null)
            {
                if (dtStoreDetails.Rows.Count > 0)
                {
                    ddlMaterialCatg.DataSource = dtStoreDetails;
                    ddlMaterialCatg.DataValueField = GTIService.Constants.Material.Fields.CATEGORYPK;
                    ddlMaterialCatg.DataTextField = GTIService.Constants.Material.Fields.CATEGORYNAME;
                    ddlMaterialCatg.DataBind();
                }
            }
            ddlMaterialCatg.Items.Insert(0, new ListItem("All", "0"));

        }

        /// <summary>
        /// Fill Material Details By Category
        /// </summary>
        public void FillMaterials()
        {
            ddlItem.Items.Clear();
            if (ddlMaterialCatg.SelectedValue != "0")
            {
                DataTable dtMaterialDtls = BusinessLogic.StoreManagement.StockDetailsReport.GetMaterialDtls(Convert.ToInt32(ddlMaterialCatg.SelectedValue), 0, currentUser.SBUID);
                if (dtMaterialDtls != null)
                {
                    if (dtMaterialDtls.Rows.Count > 0)
                    {
                        ddlItem.DataSource = dtMaterialDtls;
                        ddlItem.DataValueField = GTIService.Constants.Material.Fields.ITEMPK;
                        ddlItem.DataTextField = GTIService.Constants.Material.Fields.ITMEMCODENAME;
                        ddlItem.DataBind();
                    }
                }
                
            }
            ddlItem.Items.Insert(0, new ListItem("All", "0"));
        }
        /// <summary>
        /// Fill Material Details By Category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMaterialCatg_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdfDateStock.Value = StockDate.Text;
            FillMaterials();

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
        private void FillReport(string date, int store, int catg, int item)
        {
            LocalReport locRpt;
            RptViewerReport.LocalReport.DataSources.Clear();
            try
            {
                DataTable dtStockDtls = BusinessLogic.StoreManagement.StockDetailsReport.GetStockDetails(date, store, catg, item, currentUser.SBUID);
                if (dtStockDtls.Rows.Count > 0)
                {
                    RptViewerReport.Visible = true;
                    ReportDataSource dsGRNDailyDtls = new ReportDataSource("StockDetailsReport", dtStockDtls);
                    locRpt = null;
                    locRpt = RptViewerReport.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("StockDetailsReport.rdlc");
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
                    //Current Date in Report
                    parameters = new ReportParameter("Date",DateTime.Now.ToString("dd-MMM-yyyy"));
                    locRpt.SetParameters(parameters);
                    string ShowItemAmount = "0";
                    DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ShowItemAmount", string.Empty, currentUser.SBUID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ShowItemAmount = dt.Rows[0]["ACF_VALUE"].ToString();
                       
                    }

                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
                    DataTable dt1 = ConfigurationSettingsforReport();
                    if (dt1 != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt1.Rows[0]["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }

                        int NoDigit = Convert.ToInt32(dt1.Rows[3]["ACF_VALUE"].ToString());

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

                    parameters = new ReportParameter("ShowItemAmount", ShowItemAmount);
                    locRpt.SetParameters(parameters);
                   
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
        /// To search 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSearch_Click(object sender, ImageClickEventArgs e)
        {
            hdfDateStock.Value = StockDate.Text;
            FillReport(StockDate.Text, Convert.ToInt32(ddlStore.SelectedValue), Convert.ToInt32(ddlMaterialCatg.SelectedValue), Convert.ToInt32(ddlItem.SelectedValue));
        }
    }
}