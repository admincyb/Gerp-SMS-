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

namespace ERPSMS_v01.StoreManagement
{
    public partial class StockReport : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                FillReport(0, 0);
                FillStore(currentUser);
                FillItem(currentUser);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbSearch_Click(object sender, ImageClickEventArgs e)
        {
            FillReport(int.Parse(Store.SelectedValue), int.Parse(Item.SelectedValue));
        }
        private DataTable ConfigurationSettingsforReport()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="item"></param>
        private void FillReport(int store,int item)
        {
            RptStockReport.Visible = true;
            ReportDataSource dsStockDet;
            LocalReport locRpt;
            RptStockReport.LocalReport.DataSources.Clear();
            DataTable dtStockDtls = BusinessLogic.StoreManagement.StoreMaster.GetStockDetails(store, item);
            if (dtStockDtls.Rows.Count > 0)
            {
                RptStockReport.Visible = true;
                dsStockDet = new ReportDataSource("StockDetails", dtStockDtls);
                locRpt = null;
                locRpt = RptStockReport.LocalReport;
                locRpt.ReportPath = string.Empty;
                locRpt.ReportPath = Server.MapPath("StockReport.rdlc");
                locRpt.EnableHyperlinks = true;
                RptStockReport.LocalReport.DataSources.Clear();
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
                string ShowItemAmount = "0";
                DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ShowItemAmount", string.Empty, currentUser.SBUID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    ShowItemAmount = dt.Rows[0]["ACF_VALUE"].ToString();

                }
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                //string currencyformat="#"+currencysep+"#"+currencysep+ "#"+currencysep+"#"+currencysep+"#"+currencysep+"#0.";
                string currencyformat = "#" + currencysep + "#0.";
                string NoFormat = "#" + currencysep + "#0.";
                string currencydecimals = "";
                string Nodecimal = string.Empty;
                DataTable dt1 = ConfigurationSettingsforReport();
                if (dt1 != null && dt.Rows.Count > 0)
                {
                    int curdigit = Convert.ToInt32(dt1.Rows[0]["ACF_VALUE"].ToString());
                    for (int i = 0; i < curdigit; i++)
                    {
                        currencydecimals += "0";
                    }

                    int NoDigit = Convert.ToInt32(dt1.Rows[3]["ACF_VALUE"].ToString());

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

                parameters = new ReportParameter("ShowItemAmount", ShowItemAmount);
                locRpt.SetParameters(parameters);
                RptStockReport.LocalReport.DataSources.Add(dsStockDet);
                RptStockReport.ViewStateMode = System.Web.UI.ViewStateMode.Enabled;
                RptStockReport.LocalReport.Refresh();
            }
            else
            {
                RptStockReport.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "json", "$(document).ready(function() { showEmptyDataMsgBox();});", true);
            }
        }

        private void FillStore(BusinessObject.User objUser)
        {

            DataTable dtStore = BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetAllStores(0,2,objUser,objUser.SBUID,0,0);
            Store.DataTextField = "DPT_NAME";
            Store.DataValueField = "DPT_PK";
            Store.DataSource = dtStore;
            Store.DataBind();
            Store.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        private void FillItem(BusinessObject.User objUser)
        {
            DataTable dtItem = BusinessLogic.MaterialManagement.MaterialMaster.GetAllMaterial(objUser.SBUID);
            Item.DataTextField = "ITM_TEXT";
            Item.DataValueField = "ITM_PK";
            Item.DataSource = dtItem;
            Item.DataBind();
            Item.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }
}