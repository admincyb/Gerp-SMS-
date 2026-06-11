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
namespace ERPSMS_v01.Reports
{
    public partial class GINReport : ERP.Store.UI.MyBasePage
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
        #region Page Action
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
                CurrPK = Convert.ToInt32(Request.QueryString["GINID"]);
                FillReport(currentUser.SBUID);
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Fill Reprot
        /// </summary>
        /// <param name="srID"></param>
        private void FillReport(int sbuID)
        {
            ltNodata.Visible = false;
            RptGinReport.Visible = true;
            LocalReport locRpt;
            RptGinReport.LocalReport.DataSources.Clear();
            try
            {
                DataSet dsGin = new DataSet();
                dsGin = GoodsInspectionNote.GetGoodsInspectionNoteTables(CurrPK);//Gets the Inspection list to report view             
                if (dsGin != null)
                {
                    if (dsGin.Tables.Count > 0)
                    {
                        RptGinReport.Visible = true;
                        ReportDataSource dsGinReportHeader = new ReportDataSource(Constants.GINHEADERDATASET, dsGin.Tables[0]);
                        ReportDataSource dsGinReportDtls = new ReportDataSource(Constants.GINDETAILDATASET, dsGin.Tables[1]);
                        locRpt = null;
                        locRpt = RptGinReport.LocalReport;
                        locRpt.ReportPath = string.Empty;
                        locRpt.ReportPath = Server.MapPath(Constants.GININGRDLC);
                        RptGinReport.Height = 500;
                        RptGinReport.LocalReport.DataSources.Clear();
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
                        RptGinReport.LocalReport.DataSources.Add(dsGinReportHeader);
                        RptGinReport.LocalReport.DataSources.Add(dsGinReportDtls);
                        RptGinReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                        RptGinReport.LocalReport.Refresh();
                    }
                    else
                    {
                        ltNodata.Visible = true;
                        RptGinReport.Visible = false;
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