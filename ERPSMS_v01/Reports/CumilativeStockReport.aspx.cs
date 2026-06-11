using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.Threading;
using BusinessObject.CommonManagement;


namespace Production.Reports.Production
{
    public partial class CumilativeSttockReport : ERP.Store.UI.MyBasePage
    {

        #region Variables and Properties

        BusinessObject.User currentUser;
        private DataTable dtItem;

        #endregion

        #region Set Page Variables

        /// <summary>
        /// Set the Page Level variables and properties
        /// </summary>
        private void SetPageVariables()
        {
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
            SetPageVariables();
            if (!IsPostBack)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                FillItem(currentUser);
                FillReport(currentUser.SBUID);
            }
        }

        # endregion

        #region Action Handlers

        #region Buttons Actions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillReport(currentUser.SBUID);
        }

        #endregion

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
            rvStockReport.Visible = true;
            ReportDataSource dsStockDet;
            LocalReport locRpt;
            rvStockReport.LocalReport.DataSources.Clear();
            DataTable dtStockDtls = BusinessLogic.StoreManagement.StoreMaster.GetCumilativeStockDetails(currentUser.SBUID, int.Parse(ddlItem.SelectedValue), txtFromDate.Text, txtToDate.Text);
            if (dtStockDtls.Rows.Count > 0)
            {
                rvStockReport.Visible = true;
                dsStockDet = new ReportDataSource("CumilativeStockReport", dtStockDtls);
                locRpt = null;
                locRpt = rvStockReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                locRpt.ReportPath = Server.MapPath("CumilativeStockReport.rdlc");
                locRpt.EnableHyperlinks = true;
                rvStockReport.LocalReport.DataSources.Clear();
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
                rvStockReport.LocalReport.DataSources.Add(dsStockDet);
                rvStockReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                rvStockReport.LocalReport.Refresh();
            }
            else
            {
                rvStockReport.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        private void FillItem(BusinessObject.User objUser)
        {
            DataTable dtItem = BusinessLogic.MaterialManagement.MaterialMaster.GetAllMaterial(objUser.SBUID);
            ddlItem.DataTextField = "ITM_TEXT";
            ddlItem.DataValueField = "ITM_PK";
            ddlItem.DataSource = dtItem;
            ddlItem.DataBind();
            ddlItem.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        #endregion
    }
}