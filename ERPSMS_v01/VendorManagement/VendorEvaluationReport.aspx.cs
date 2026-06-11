using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Xml;
using System.IO;
using System.Threading;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.VendorManagement
{
    public partial class VendorEvaluationReport : System.Web.UI.Page
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["PK"] != null)
                {
                    FillReport(Convert.ToInt32(Request.QueryString["PK"]));
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
        /// To display report
        /// </summary>
        /// <param name="storeAdjID"></param>
        private void FillReport(int evalID)
        {
            rvVendorEvaluation.Visible = true;
            LocalReport locRpt;
            string vndEvaluationDtls;
            vndEvaluationDtls = BusinessLogic.VendorManagement.VendorEvaluation.GetEvaluationReport(evalID);
            DataSet dsVendorEvaluation = new DataSet();
            dsVendorEvaluation.ReadXml(new XmlTextReader(new StringReader(vndEvaluationDtls)));
            if (dsVendorEvaluation != null)
            {
                if (dsVendorEvaluation.Tables.Count > 0)
                {
                    rvVendorEvaluation.Visible = true;
                    ReportDataSource dsStoreReqHeader = new ReportDataSource("VendorEvalHeader", dsVendorEvaluation.Tables[0]);
                    ReportDataSource dsStoreReqDetail = new ReportDataSource("VendorEvalDetail", dsVendorEvaluation.Tables[1]);
                    locRpt = null;
                    locRpt = rvVendorEvaluation.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/VendorEvaluationReport.rdlc");
                    rvVendorEvaluation.Height = 700;
                    rvVendorEvaluation.LocalReport.DataSources.Clear();
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
                    rvVendorEvaluation.LocalReport.DataSources.Add(dsStoreReqHeader);
                    rvVendorEvaluation.LocalReport.DataSources.Add(dsStoreReqDetail);
                    rvVendorEvaluation.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    rvVendorEvaluation.LocalReport.Refresh();
                }
                else
                {
                    rvVendorEvaluation.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                }
            }


        }
    }
}