using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Reporting.WebForms;
using BusinessLogic;
using System.Data;
using BusinessObject.CommonManagement;
using System.Threading;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["POID"] != null)
                {
                    FillReport(Convert.ToInt32(Request.QueryString["POID"]));
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
        /// fill Report Details
        /// </summary>
        /// <param name="pOID"></param>
        private void FillReport(int pOID)
        {
            RptViewerReport.Visible = true;
            LocalReport locRpt;
            RptViewerReport.LocalReport.DataSources.Clear();
            try
            {
                DataSet dsPODtls = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetails(pOID,GetGlobalResourceObject("ConfigurationsRes", "PurchaseOrderOutRPTSP").ToString());
                DataSet dsCurrentSBUAddress = BusinessLogic.PurchaseOrderManagement.PurchaseOrderCreation.GetCurrentSBUAddress(currentUser.SBUID, 2);
                if (dsPODtls != null)
                {
                    DataTable dtPOHeaderDtls = dsPODtls.Tables[0];
                    if (dtPOHeaderDtls.Rows.Count > 0)
                    {
                        RptViewerReport.Visible = true;
                        ReportDataSource dsPOHeaderDtls = new ReportDataSource("POHeaderDtls", dtPOHeaderDtls);
                        ReportDataSource dsProductDtls = new ReportDataSource("POProductDtls", dsPODtls.Tables[1]);
                        ReportDataSource dsUserAddressDtls = new ReportDataSource("UserAddressDtls", dsCurrentSBUAddress.Tables[0]);

                        locRpt = null;
                        locRpt = RptViewerReport.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        locRpt.ReportPath = Server.MapPath("../Reports/POReport.rdlc");
                        RptViewerReport.Height = 500;
                        RptViewerReport.LocalReport.DataSources.Clear();
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
                        //parameters = new ReportParameter("Address", "Head Office:" + dsCurrentSBUAddress.Tables[0].Rows[0][4] +
                        //    "\r\n" + "Tele Fax:" + dsCurrentSBUAddress.Tables[0].Rows[0][8] + "," +
                        //    dsCurrentSBUAddress.Tables[0].Rows[0][9] +
                        //    "\r\n" + "Email:" + dsCurrentSBUAddress.Tables[0].Rows[0][10]);
                        //locRpt.SetParameters(parameters);
                        object[] strArg = new object[2];
                        strArg[0] = currentUser.EmpName;
                        strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                        parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                        locRpt.SetParameters(parameters);
                        parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["POReportLogo"]));
                        locRpt.SetParameters(parameters);
                        RptViewerReport.LocalReport.DataSources.Add(dsPOHeaderDtls);
                        RptViewerReport.LocalReport.DataSources.Add(dsProductDtls);
                        RptViewerReport.LocalReport.DataSources.Add(dsUserAddressDtls);
                        RptViewerReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        RptViewerReport.LocalReport.Refresh();

                    }
                    else
                    {
                        RptViewerReport.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
         
        }


    
}
 