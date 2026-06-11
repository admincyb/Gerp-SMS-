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

namespace ERPSMS_v01.StoreManagement
{
    public partial class MaterialReturnReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["ConsumptionID"] != null)
                {
                    FillReport(Convert.ToInt32(Request.QueryString["ConsumptionID"]));
                }


            }
        }
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        private void FillReport(int ReqID)
        { 
            RptMaterialReturn.Visible = true;
            ReportDataSource dsStoreReqHeader;
            ReportDataSource dsStoreReqDetail;
            LocalReport locRpt;
            RptMaterialReturn.LocalReport.DataSources.Clear();
            DataSet dsetStoreReqDtls = null;
            dsetStoreReqDtls = BusinessLogic.StoreManagement.MaterialConsumption.GetConsumptionReportByReqId(ReqID);
            if (dsetStoreReqDtls != null)
            {
                DataTable dtRequestHeader = dsetStoreReqDtls.Tables[0];
                DataTable dtRequestDetail = dsetStoreReqDtls.Tables[1];
                if (dtRequestHeader.Rows.Count > 0 || dtRequestDetail.Rows.Count > 0)
                {
                    RptMaterialReturn.Visible = true;
                    dsStoreReqHeader = new ReportDataSource("StoreRequestHeader", dtRequestHeader);
                    dsStoreReqDetail = new ReportDataSource("StoreRequestDetail", dtRequestDetail);

                    locRpt = null;
                    locRpt = RptMaterialReturn.LocalReport;
                    locRpt.ReportPath = string.Empty;
                    locRpt.ReportPath = Server.MapPath("../Reports/MaterialComsumptionReport.rdlc");
                    locRpt.EnableHyperlinks = true;
                    RptMaterialReturn.LocalReport.DataSources.Clear();
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
                    RptMaterialReturn.LocalReport.DataSources.Add(dsStoreReqHeader);
                    RptMaterialReturn.LocalReport.DataSources.Add(dsStoreReqDetail);
                    RptMaterialReturn.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                    RptMaterialReturn.LocalReport.Refresh();
                }
                else
                {
                    RptMaterialReturn.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
                }
            }
        }
    }
}