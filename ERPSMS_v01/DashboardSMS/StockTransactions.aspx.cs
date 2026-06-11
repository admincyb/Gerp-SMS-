using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using GTIService.Dashboard;
using System.Xml;
using System.IO;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessLogic;

namespace ERPSMS_v01.DashboardSMS
{
    public partial class StockTransactions : System.Web.UI.Page
    {
        #region Variables and Properties


        private ActionsEnum commonActions;
        private string spec;
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
        /// <summary>
        /// Current PK (Primary Key of the current)
        /// </summary>
        private int TypePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["TypePK"]);
            }
            set
            {
                this.ViewState["TypePK"] = value;
            }
        }

        BusinessObject.User currentUser;

        #endregion

        #region Set Page Variables

        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
            // Assign user details to Identity User Object
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
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
            // Set Page Variale like assign user details
            SetPageVariables();
            if (!IsPostBack)
            {
                TypePK = Convert.ToInt32(Request.QueryString["TypePK"]);
                CurrPK = Convert.ToInt32(Request.QueryString["PK"]);
                // Fill data to rdlc reprt
                FillReport(CurrPK, TypePK);

            }
        }

        #endregion

        #region Action Handlers

        #region Buttons Actions

        /// <summary>
        /// For Button Click (Save/Cancel)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            switch (commonActions)
            {

                case ActionsEnum.CANCEL:
                    //   Response.Redirect(PackingTransactions.REDIRECTTOPACKTRANSURL);
                    break;

                case ActionsEnum.SEARCH:
                    if (IsValid)
                    {
                        FillReport(0, 0);
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Fill Reprot
        /// </summary>
        /// <param name="srID"></param>
        private void FillReport(int pk, int type)
        {
            rvrReport.Visible = true;
            LocalReport locRpt;

            rvrReport.LocalReport.DataSources.Clear();
            string xml = String.Empty;//Gets thepacking list to report view 
            if (type == 1)
            {
                xml = DashboardBL.GetStockTransferRptDetails(pk);
                DataSet dsReport = new DataSet();
                dsReport.ReadXml(new XmlTextReader(new StringReader(xml)));
                locRpt = null;
                locRpt = rvrReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                if (dsReport != null)
                {
                    if (dsReport.Tables.Count > 0)
                    {
                        ReportDataSource dsStockTrfHeader = new ReportDataSource("StockTrfHeader", dsReport.Tables[0]);
                        ReportDataSource dsPODetail = new ReportDataSource("PODetail", dsReport.Tables[1]);
                        ReportDataSource dsPRDetail = new ReportDataSource("PRDetail", dsReport.Tables[2]);
                        ReportDataSource dsAddnlDetail;
                        if (dsReport.Tables.Count == 4)
                        {
                            dsAddnlDetail = new ReportDataSource("AddnlDetail", dsReport.Tables[3]);
                        }
                        else
                        {
                            dsAddnlDetail = new ReportDataSource("AddnlDetail", new DataTable());
                        }
                        rvrReport.LocalReport.DataSources.Add(dsStockTrfHeader);
                        rvrReport.LocalReport.DataSources.Add(dsPODetail);
                        rvrReport.LocalReport.DataSources.Add(dsPRDetail);
                        rvrReport.LocalReport.DataSources.Add(dsAddnlDetail);
                    }
                }

                locRpt.ReportPath = Server.MapPath("../Reports/rptStockTransferReport.rdlc");
                locRpt.EnableExternalImages = true;
                rvrReport.Height = 500;
                // rvrReport.LocalReport.DataSources.Clear();
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
                rvrReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvrReport.LocalReport.Refresh();
            }
            else if ((type == 2) || (type == 3))
            {
                xml = DashboardBL.GetMaterialIssueDetailsForReport(pk);
                DataSet dsReport = new DataSet();
                dsReport.ReadXml(new XmlTextReader(new StringReader(xml)));
                locRpt = null;
                locRpt = rvrReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                if (dsReport != null)
                {
                    if (dsReport.Tables.Count > 0)
                    {
                        ReportDataSource dsHeader = new ReportDataSource("DataSet1", dsReport.Tables[0]);
                        ReportDataSource dsDtls = new ReportDataSource("DataSet2", dsReport.Tables[1]);
                        rvrReport.LocalReport.DataSources.Add(dsHeader);
                        rvrReport.LocalReport.DataSources.Add(dsDtls);
                    }
                }

                locRpt.ReportPath = Server.MapPath("../Reports/rptStoreIssueReport.rdlc");
                rvrReport.Height = 500;
                //  rvrReport.LocalReport.DataSources.Clear();
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
                rvrReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvrReport.LocalReport.Refresh();
            }
            else if (type == 4)
            {
                xml = DashboardBL.GetStockAdjustmentDetailsForReport(pk);

                DataSet dsReport = new DataSet();
                dsReport.ReadXml(new XmlTextReader(new StringReader(xml)));
                locRpt = null;
                locRpt = rvrReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                if (dsReport != null)
                {
                    if (dsReport.Tables.Count > 0)
                    {
                        ReportDataSource dsHeader = new ReportDataSource("StockAdjHeader", dsReport.Tables[0]);
                        ReportDataSource dsDtls = new ReportDataSource("StockAdjDetail", dsReport.Tables[1]);
                        rvrReport.LocalReport.DataSources.Add(dsHeader);
                        rvrReport.LocalReport.DataSources.Add(dsDtls);
                    }
                }

                locRpt.ReportPath = Server.MapPath("../Reports/rptStoreAdjustmentReport.rdlc");
                rvrReport.Height = 500;
                //  rvrReport.LocalReport.DataSources.Clear();
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
                rvrReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvrReport.LocalReport.Refresh();
            }
        }


        #endregion
    }
}