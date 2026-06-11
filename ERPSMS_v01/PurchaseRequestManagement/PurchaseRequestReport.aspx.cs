using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Reporting.WebForms;
using BusinessLogic;
using System.Data;
using System.Threading;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.PurchaseRequestManagement
{
    public partial class PurchaseRequestReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["PRID"] != null)
                {
                    FillReport (Convert.ToInt32( Request.QueryString["PRID"]));
                }
            }
        }
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRID"></param>
        private void FillReport(int pRID)
        {
            rptPurchaseRequest.Visible = true;
            ReportDataSource dsPRHeaderDtls;
            ReportDataSource dsWorkFlowComments;
            ReportDataSource dsPRMaterialDtls;
            LocalReport locRpt;
            rptPurchaseRequest.LocalReport.DataSources.Clear();
            try
            {
                DataSet dsPurchaseRequest = BusinessLogic.PurchaseRequestManagement.PurchaseRequestListing.GetPurchaseRequestReportDetails(pRID);
                if (dsPurchaseRequest != null)
                {
                    DataTable dtPRHeaderDtls = dsPurchaseRequest.Tables[0];
                    DataTable dtPRDtls = dsPurchaseRequest.Tables[1];
                    DataTable dtWorkFlowComment = dsPurchaseRequest.Tables[2];
                    if (dtPRHeaderDtls.Rows.Count > 0 || dtPRDtls.Rows.Count > 0)
                    {
                        rptPurchaseRequest.Visible = true;
                        dsPRHeaderDtls = new ReportDataSource("PRHeaderDtls", dtPRHeaderDtls);
                        dsPRMaterialDtls = new ReportDataSource("PRMaterialDtls", dtPRDtls);
                        dsWorkFlowComments = new ReportDataSource("WorkFlowComment", dtWorkFlowComment);
                        locRpt = null;
                        locRpt = rptPurchaseRequest.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        locRpt.ReportPath = Server.MapPath("../Reports/PurchaseRequestReport.rdlc");
                        locRpt.EnableHyperlinks = true;
                        rptPurchaseRequest.LocalReport.DataSources.Clear();
                        locRpt.EnableExternalImages = true;
                        ReportParameter parameters;
                        parameters = new ReportParameter("Title", currentUser.CurrentSBU);
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
                        parameters = new ReportParameter("DateFormat", Resources.Constants.ReportDateFormat);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("CurrencyFormat", currencyformat);
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("NumberFormat", NoFormat);
                        locRpt.SetParameters(parameters);
                        object[] strArg = new object[2];
                        strArg[0] = currentUser.EmpName;
                        strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                        parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                        locRpt.SetParameters(parameters);
                        rptPurchaseRequest.LocalReport.DataSources.Add(dsPRHeaderDtls);
                        rptPurchaseRequest.LocalReport.DataSources.Add(dsPRMaterialDtls);
                        rptPurchaseRequest.LocalReport.DataSources.Add(dsWorkFlowComments);
                        
                        rptPurchaseRequest.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        rptPurchaseRequest.LocalReport.Refresh();
                    }
                    else
                    {
                        rptPurchaseRequest.Visible = false;
                    }
                }
            }
            catch
            {

            }

        }
    }
}