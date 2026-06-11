using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using BusinessObject;
using GTIService.Dashboard;
using System.Data;
using BusinessLogic.CommonManagement;
using BusinessLogic.ReportsManagement;
using System.Xml.Linq;
using System.Web.Hosting;
using System.Text;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Reports
{
    public partial class ProfitReport : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties

        private ActionsEnum commonActions;
        User currentUser;
        private DataTable dtRptData;
        string XMLFilePath = "~/Reports/XMLFiles/";
        private string CurrencyFormat = "#0.";
        private string DecimalFormat = "#0.";
        public string innerCSS = string.Empty;


        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }
        #endregion

        #region PageLevel Events

        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        //protected override void OnInit(EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(ReportXML))
        //    {
        //        HTMLReportBuilder objHtml = new HTMLReportBuilder();
        //        objHtml.reportURL = ReportXML;
        //        styleInsert.Text = "<style type=\"text/css\">" + objHtml.GetColumnStyle() + "</style>";
        //    }
        //}
        #endregion

        #region Pager Methods + Init
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                //styleInsert.Text = innerCSS;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        /// 
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    #region  REPORT
                    case ControlsEnum.REPORT:
                        ReportParameters reportParams;
                        reportParams = SetUIValuesToObject(type);
                        string CurrFilter = string.Empty;
                        CurrFilter = reportParams.XmlSerialize();
                        dtRptData = GenerateReportBL.GetProfitReport(CurrFilter);
                        break;
                    #endregion

                    //#region PROCESS
                    //case ControlsEnum.PROCESS:
                    //    ReportParameters reportParams;
                    //    reportParams = SetUIValuesToXMLObject(type);
                    //    break;
                    //#endregion
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }
        #endregion

        #region Set Field Values

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region  REPORT
                    case ControlsEnum.REPORT:
                        GenerateReport(ReportType.SCWiseProfit);
                        break;
                    #endregion
                }
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        #endregion

        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int? result;
                currentUser = GetUserIdentity();
                //Get Action from CommandName
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region VIEW
                    case ActionsEnum.VIEW:
                        GetFieldValues(ControlsEnum.REPORT);
                        SetFieldValues(ControlsEnum.REPORT);
                        break;
                    #endregion

                    #region PROCESS
                    case ActionsEnum.PROCESS:
                        ReportParameters reportParams;
                        reportParams = SetUIValuesToObject(ControlsEnum.PROCESS);
                        string CurrFilter = string.Empty;
                        CurrFilter = reportParams.XmlSerialize();
                        result = GenerateReportBL.ProcessProfitReport(CurrFilter);
                        if (result > 0)
                        {
                            GetFieldValues(ControlsEnum.REPORT);
                            SetFieldValues(ControlsEnum.REPORT);
                            //string msg = GetLocalResourceObject("ProfitReportProcessedSuccessfully").ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + msg + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            string msg = GetLocalResourceObject("ProcessFailed").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + msg + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
            }
        }

        #endregion

        #region Helper Methods

        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(CurrencyFormat);
        }

        public string GetFormattedCurrencyWithComa(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            // return num.ToString(hdfCurrencyFormat.Value);
            string resultNum = string.Format("{0:c}", Convert.ToDecimal(num.ToString(CurrencyFormat)));
            return resultNum;
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(DecimalFormat);
        }

        private void GenerateReport(ReportType reportType)
        {
            try
            {

                string XmlDocUrl = string.Empty;
                //XDocument xDoc = XDocument.Load(HostingEnvironment.MapPath(XmlDocUrl));

                if (dtRptData.Rows.Count > 0)
                    lblLastProcess.Text = "From " + Convert.ToDateTime(dtRptData.Rows[0]["SCP_FROM_DT"]).ToString("dd-MMM-yyyy") + " To " + Convert.ToDateTime(dtRptData.Rows[0]["SCP_TO_DT"]).ToString("dd-MMM-yyyy");

                HTMLReportBuilder objHtmlReport = new HTMLReportBuilder();
                switch (reportType)
                {
                    case ReportType.SCWiseProfit:
                        XmlDocUrl = XMLFilePath + GetLocalResourceObject("SCProfitReprotFile").ToString();
                        break;
                }
                decimal profitSum = dtRptData.AsEnumerable().Sum(s => s.Field<decimal>("SCP_SOH_PROFIT"));
                decimal sellingSum = dtRptData.AsEnumerable().Sum(s => s.Field<decimal>("SCP_SOH_SELLING"));
                decimal otherIncomeSum = dtRptData.AsEnumerable().Sum(s => s.Field<decimal>("SCP_SOH_OTHER_INCOME"));
                decimal result = 0;
                if ((sellingSum + otherIncomeSum) > 0)
                    result = (profitSum / (sellingSum + otherIncomeSum)) * 100;

                objHtmlReport.Result1 = GetFormattedNumber(result);
                data_container.InnerHtml = objHtmlReport.GetHtml(dtRptData, XmlDocUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private ReportParameters SetUIValuesToObject(ControlsEnum type)
        {
            ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
            switch (type)
            {
                case ControlsEnum.REPORT:
                    tempReportParams.BizUnit = currentUser.SBUID;
                    tempReportParams.Dept = currentUser.CurrentDeptPK;
                    tempReportParams.UserPK = currentUser.PKUser;
                    tempReportParams.RptPK = Session[ERP.Utilities.SessionStrings.REPORTPK] == null ? -1 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK]);
                    tempReportParams.Currency = currentUser.BaseCurrency;
                    tempReportParams.Parameters = new List<GTIService.Dashboard.ReportParameterName>();
                    tempReportParams.FromDate = txtSearchFromDate.Text; //"01-Sep-2020";
                    tempReportParams.ToDate = txtSearchToDate.Text; //"30-Sep-2020";
                    break;
                case ControlsEnum.PROCESS:
                    tempReportParams.BizUnit = currentUser.SBUID;
                    tempReportParams.Currency = currentUser.BaseCurrency;
                    tempReportParams.FromDate = txtSearchFromDate.Text; 
                    tempReportParams.ToDate = txtSearchToDate.Text; 
                    break;
            }

            return tempReportParams;
        }

        #endregion

        #region Enums
        public enum ControlsEnum
        {
            REPORT,
            PROCESS
        }
        public enum ReportType
        {
            SCWiseProfit,
            ShippingPlanWiseProfit,
            ActualProfit
        }
        #endregion
    }
}