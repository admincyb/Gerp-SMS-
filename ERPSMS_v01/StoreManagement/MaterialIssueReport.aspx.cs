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
using BusinessObject.CommonManagement;
using System.Threading;

namespace ERPSMS_v01.StoreManagement
{
    public partial class MaterialIssueReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["MIPK"] != null)
                {
                    FillReport(Convert.ToInt32(Request.QueryString["MIPK"]));
                }


            }
        }
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        private void FillReport(int storeIssueID)
        {
            RptStoreIssue.Visible = true;
            ReportDataSource dsStoreIssueHeader;
            ReportDataSource dsStoreIssueDetail;
            LocalReport locRpt;

            RptStoreIssue.LocalReport.DataSources.Clear();
            DataSet dsetStoreIssueDtls = new DataSet();
            string issueXml = string.Empty;
            issueXml = BusinessLogic.StoreManagement.MaterialIssue.GetMaterialIssueDetailsForReport(storeIssueID);
            dsetStoreIssueDtls.ReadXml(new XmlTextReader(new StringReader(issueXml)));
            if (dsetStoreIssueDtls != null)
            {
                DataTable dtHeader = dsetStoreIssueDtls.Tables[0];
                DataTable dtDetail = dsetStoreIssueDtls.Tables[1];
                if (dtHeader.Rows.Count > 0 || dtDetail.Rows.Count > 0)
                {
                    RptStoreIssue.Visible = true;
                    dsStoreIssueHeader = new ReportDataSource("Header", dtHeader);
                    dsStoreIssueDetail = new ReportDataSource("Detail", dtDetail);
                    locRpt = null;
                    locRpt = RptStoreIssue.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/StoreIssueReport.rdlc");
                    locRpt.EnableHyperlinks = true;
                    RptStoreIssue.LocalReport.DataSources.Clear();
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
                    RptStoreIssue.LocalReport.DataSources.Add(dsStoreIssueHeader);
                    RptStoreIssue.LocalReport.DataSources.Add(dsStoreIssueDetail);
                    RptStoreIssue.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    RptStoreIssue.LocalReport.Refresh();
                }
                else
                {
                    RptStoreIssue.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                }
            }
        }
    }
}