using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using BusinessLogic.StoreManagement;
using System.Data;
using System.Xml;
using System.IO;
using GTIService.Constants.Store;
using System.Threading;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Reports
{
    public partial class GRNReport : ERP.Store.UI.MyBasePage
    {

        #region Variables and Properties
        /// <summary>
        /// Current PK (Primary Key of the current)
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["CurrPK"]);
            }
            set
            {
                this.ViewState["CurrPK"] = value;
            }
        }

        BusinessObject.User currentUser;
        #endregion
        #region PageAction
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                CurrPK = Convert.ToInt32(Request.QueryString["PRID"]);
                FillReport(currentUser.SBUID);
            }
        }
        #endregion
        #region Helper Methods
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// Fill Reprot
        /// </summary>
        /// <param name="srID"></param>
        private void FillReport(int sbuID)
        {
            ltNodata.Visible = false;
            RptGrnReport.Visible = true;
            LocalReport locRpt;
            RptGrnReport.LocalReport.DataSources.Clear();
            try
            {
                string xml = GoodsReceiptNote.GetGoodsReceiptNoteDetailsReport(CurrPK);//Gets the Godds receipt list to report view 
                DataSet dsGrn = new DataSet();
                dsGrn.ReadXml(new XmlTextReader(new StringReader(xml)));
                if (dsGrn != null)
                {
                    if (dsGrn.Tables.Count > 0)
                    {
                        RptGrnReport.Visible = true;
                        ReportDataSource dsGrnReportHeader = new ReportDataSource(Constants.GRNHEADERDATASET, dsGrn.Tables[0]);
                        ReportDataSource dsGrnReportDtls = new ReportDataSource(Constants.GRNDETAILDATASET, dsGrn.Tables[1]);
                        locRpt = null;
                        locRpt = RptGrnReport.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        locRpt.ReportPath = Server.MapPath(Constants.GRNINGRDLC);
                        RptGrnReport.Height = 500;
                        RptGrnReport.LocalReport.DataSources.Clear();
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
                        RptGrnReport.LocalReport.DataSources.Add(dsGrnReportHeader);
                        RptGrnReport.LocalReport.DataSources.Add(dsGrnReportDtls);
                        RptGrnReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        RptGrnReport.LocalReport.Refresh();
                    }
                    else
                    {
                        ltNodata.Visible = true;
                        RptGrnReport.Visible = false;
                    }
                }

            }
            catch
            {

            }
        }
        #endregion
    }
}