using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using BusinessObject.AccountManagement;
using System.Data;
using System.Xml;
using System.IO;
using GTIService.Dashboard;
using BusinessLogic;

namespace ERPSMS_v01.DashboardSMS
{
    public partial class VendorContactDtls : System.Web.UI.Page
    {
        #region Variables and Properties

        private ActionsEnum commonActions;
        private string spec;
        BusinessObject.User currentUser;

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

        #endregion

        #region PageAction

        /// <summary>
        /// Page load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            if (!IsPostBack)
            {
                CurrPK = Convert.ToInt32(Request.QueryString["PK"]);
                // Fill data to rdlc reprt
                FillReport();
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Fill Reprot
        /// </summary>
        /// <param name="srID"></param>
        private void FillReport()
        {
            rvrReport.Visible = true;
            LocalReport locRpt;
            string vndContactDtls;
            vndContactDtls = DashboardBL.GetVendorContactDetailsReport(CurrPK);
            DataSet dsVndContactDetails = new DataSet();
            dsVndContactDetails.ReadXml(new XmlTextReader(new StringReader(vndContactDtls)));
            if (dsVndContactDetails != null)
            {
                if (dsVndContactDetails.Tables.Count > 0)
                {
                    rvrReport.Visible = true;
                    ReportDataSource dsVendorHeader = new ReportDataSource("DataSet11", dsVndContactDetails.Tables["Root"] == null ? new DataTable() : dsVndContactDetails.Tables["Root"]);
                    ReportDataSource dsVndPODetail = new ReportDataSource("DataSet12", dsVndContactDetails.Tables["AddressBookDetails"] == null ? new DataTable() : dsVndContactDetails.Tables["AddressBookDetails"]);
                    ReportDataSource dsVndGRNDetail = new ReportDataSource("DataSet13", dsVndContactDetails.Tables["VendorPOList"] == null ? new DataTable() : dsVndContactDetails.Tables["VendorPOList"]);
                    ReportDataSource dsVndAddressBook = new ReportDataSource("DataSet14", dsVndContactDetails.Tables["VendorGRNList"] == null ? new DataTable() : dsVndContactDetails.Tables["VendorGRNList"]);
                    ReportDataSource dsVndEvaluation = new ReportDataSource("DataSet15", dsVndContactDetails.Tables["EvalDetails"] == null ? new DataTable() : dsVndContactDetails.Tables["EvalDetails"]);
                    locRpt = null;
                    locRpt = rvrReport.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/rptVendorContactDtls.rdlc");
                    rvrReport.Height = 500;
                    locRpt.DataSources.Clear();
                    locRpt.EnableExternalImages = true;
                    ReportParameter parameters;
                    parameters = new ReportParameter("Title", currentUser.CurrentSBU);
                    locRpt.SetParameters(parameters);
                    object[] strArg = new object[2];
                    strArg[0] = currentUser.UserName;
                    strArg[1] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    parameters = new ReportParameter("FooterText", string.Format(this.GetGlobalResourceObject("Messages", "ReportFooterMessages").ToString(), strArg));
                    locRpt.SetParameters(parameters);
                    parameters = new ReportParameter("HeaderImage", "file:///" + Server.MapPath(DashboardConstants.ImagePath + System.Configuration.ConfigurationManager.AppSettings["ReportLogo"]));
                    locRpt.SetParameters(parameters);
                    locRpt.DataSources.Add(dsVendorHeader);
                    locRpt.DataSources.Add(dsVndPODetail);
                    locRpt.DataSources.Add(dsVndGRNDetail);
                    locRpt.DataSources.Add(dsVndAddressBook);
                    locRpt.DataSources.Add(dsVndEvaluation);
                    rvrReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    locRpt.Refresh();
                }
                else
                {
                    rvrReport.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                }
            }

        }
        #endregion
    }
}