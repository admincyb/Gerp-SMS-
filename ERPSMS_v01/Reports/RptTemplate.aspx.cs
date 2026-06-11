using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace ERPSMS_v01.Reports
{

    public partial class RptTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            string commonActions = string.Empty;
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (((Button)sender).CommandName).ToString();
            }
            FillReport(commonActions);
        }

        public void FillReport(string commonActions)
        {
            rvTempReport.Visible = true;
            ReportDataSource dsReportDet;
            LocalReport locRpt;
            locRpt = null;
            rvTempReport.LocalReport.DataSources.Clear();
            rvTempReport.Visible = true;
            switch (commonActions)
            {
                case "SHOWRPTLOGO":
                    DataTable dtReptDtls;
                    dtReptDtls = GetReportData();
                    if (dtReptDtls.Rows.Count > 0)
                    {
                        dsReportDet = new ReportDataSource("ReportDetails", dtReptDtls);
                        
                        locRpt = rvTempReport.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        locRpt.ReportPath = Server.MapPath("RptTemplateWithLogo.rdlc");
                        locRpt.EnableHyperlinks = true;
                        rvTempReport.LocalReport.DataSources.Clear();
                        locRpt.EnableExternalImages = true;
                        //To set Header & footer properties
                        SetReportParameters(locRpt);

                        rvTempReport.LocalReport.DataSources.Add(dsReportDet);
                        rvTempReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        rvTempReport.LocalReport.Refresh();
                    }
                    else
                    {
                        rvTempReport.Visible = false;
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                    }
                    break;
                case "SHOWRPT":
                    DataTable dtReptDtl;
                    dtReptDtl = GetReportData();
                    if (dtReptDtl.Rows.Count > 0)
                    {
                        dsReportDet = new ReportDataSource("ReportDetails", dtReptDtl);
                        locRpt = null;
                        locRpt = rvTempReport.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        locRpt.ReportPath = Server.MapPath("RptTemplateWithoutLogo.rdlc");
                        locRpt.EnableHyperlinks = true;
                        rvTempReport.LocalReport.DataSources.Clear();
                        locRpt.EnableExternalImages = true;
                        //To set Header & footer properties
                        SetReportParameters(locRpt);

                        rvTempReport.LocalReport.DataSources.Add(dsReportDet);
                        rvTempReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        rvTempReport.LocalReport.Refresh();
                    }
                    else
                    {
                        rvTempReport.Visible = false;
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                    }
                    break;
            }
        }

        private DataTable GetReportData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Item");
            dt.Columns.Add("Value");
            try
            {
                DataRow dr;
                for (int i = 0; i < 1000; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = "Item - " + i.ToString();
                    dr[1] = i.ToString();
                    dt.Rows.Add(dr);
                }
                    return dt;
            }
            catch
            { 
                return null;
            }
            finally 
            {
                dt.Dispose();
            }
        }

        private void SetReportParameters(LocalReport locRpt)
        {
            ReportParameter parameters;
            string footer;
            footer = string.Empty;

            parameters = new ReportParameter("Logo", "file:///" + Server.MapPath("~\\Reports\\Images\\" + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HideLogo", "false");
            locRpt.SetParameters(parameters);

            parameters = new ReportParameter("HeadTitle", "Purchase Requisition");
            locRpt.SetParameters(parameters);
            parameters = new ReportParameter("HideHeadTitle", "false");
            locRpt.SetParameters(parameters);

            parameters = new ReportParameter("SubTitle", "Title From Report");
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
            parameters = new ReportParameter("HidePageNo", "true");
            locRpt.SetParameters(parameters);
        }
    }
}