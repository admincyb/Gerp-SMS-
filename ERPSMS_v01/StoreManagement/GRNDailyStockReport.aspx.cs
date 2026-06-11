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
    public partial class GRNDailyStockReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillStore();
                FillMaterialCatgeory();
                FillReport(0, DateTime.Now.ToString(), 0);
                hdfDateGRN.Value = string.Empty;
                Store.Focus();
            }
        }

        protected void imbClear_Click(object sender, ImageClickEventArgs e)
        {
            Store.SelectedIndex = 0;
            GRNDate.Text = string.Empty;
            ddlMaterialCatg.SelectedIndex = 0;
        }

        protected void imbSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillReport(Convert.ToInt32(Store.SelectedValue), GRNDate.Text.Trim(), Convert.ToInt32(ddlMaterialCatg.SelectedValue));
            hdfDateGRN.Value = GRNDate.Text;
        }
        /// <summary>
        /// 
        /// </summary>
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// Fill details to Rdlc
        /// </summary>
        /// <param name="store"></param>
        /// <param name="date"></param>
        /// <param name="catg"></param>
        private void FillReport(int store, string date,int catg)
        {
            LocalReport locRpt;
            RptViewerReport.LocalReport.DataSources.Clear();
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                DataTable dtGRNStockDtls = GRNDailyReport.GetGRNDailyDtls(store,date,catg, objUser.SBUID);
                if (dtGRNStockDtls.Rows.Count > 0)
                {
                    RptViewerReport.Visible = true;
                    ReportDataSource dsGRNDailyDtls = new ReportDataSource("GRNDailyReport", dtGRNStockDtls);
                    locRpt = null;
                    locRpt = RptViewerReport.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/GRNDailyStock.rdlc");
                    RptViewerReport.Height = 500;
                    RptViewerReport.LocalReport.DataSources.Clear();
                    ReportParameter parameters;
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                    string currencyformat = "#" + currencysep + "#0.";
                    string NoFormat = "#" + currencysep + "#0.";
                    string currencydecimals = "";
                    string Nodecimal = string.Empty;
                    DataTable dt = ConfigurationSettingsforReport();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int curdigit = Convert.ToInt32(dt.Rows[0]["ACF_VALUE"].ToString());
                        for (int i = 0; i < curdigit; i++)
                        {
                            currencydecimals += "0";
                        }

                        int NoDigit = Convert.ToInt32(dt.Rows[3]["ACF_VALUE"].ToString());

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

                    // currencyformat = {0:n} + currencydecimals;
                    parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("CurrencyFormat", currencyformat);
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("NumberFormat", NoFormat);
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
        /// Fill Store Details To dropDown
        /// </summary>
        public void FillStore()
        {
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtStoreDetails = GRNDailyReport.GetStoreDtls(objUser);
            Store.Items.Clear();
            if (dtStoreDetails != null)
            {
                if (dtStoreDetails.Rows.Count > 0)
                {
                    Store.DataSource = dtStoreDetails;
                    Store.DataValueField = GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD;
                    Store.DataTextField =  GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD; 
                    Store.DataBind();
                }
            }
            Store.Items.Insert(0, new ListItem("All", "0"));
        }
        /// <summary>
        /// Fill material Category 
        /// </summary>
        public void FillMaterialCatgeory()
        {
            BusinessObject.User objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtStoreDetails = GRNDailyReport.GetCategoryDtls(objUser.SBUID);
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

    }
}