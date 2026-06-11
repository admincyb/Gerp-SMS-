using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.Common;
using System.Configuration;
using CrystalDecisions.Shared;
using ERP.Utilities;

namespace ERPSMS_v01.Reports.CrystalReportFiles
{
    public partial class ShowReport : System.Web.UI.Page
    {
        private ReportDocument reportDocument;
        ParameterField paramField;
        ParameterFields paramFields;
        ParameterDiscreteValue paramDiscreteValue;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //if (reportDocument != null)
                //{
                //    reportDocument.Close();
                //    reportDocument.Dispose();
                //}
               // BindCrystalReport("@P_XML", "crptAccountPayableMovementReport_IGCL.rpt", "<FilterParameters> <FromDate>01-Feb-2016</FromDate> <ToDate>15-Mar-2016</ToDate> <BizUnit>1</BizUnit> <Dept>8</Dept> <UserPK>2</UserPK> <Currency>133</Currency> <Parameter> <ParamName>CMP_PK</ParamName>  <Values>  <Value>1</Value>   </Values> </Parameter> <Parameter> <ParamName>COA_PK</ParamName>  </Parameter> </FilterParameters>");
               
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            reportDocument = (ReportDocument)Session["rptData"];
            ParameterFields paramFields = (ParameterFields)Session["rptParam"];
            crReportViewer.ParameterFieldInfo = paramFields;
            crReportViewer.ReportSource = reportDocument;
        }
        protected void ActionHandler(object sender, EventArgs e)
        {
            BindCrystalReport("@P_XML", "crptAccountPayableMovementReport_IGCL.rpt", "<FilterParameters> <FromDate>01-Feb-2016</FromDate> <ToDate>15-Mar-2016</ToDate> <BizUnit>1</BizUnit> <Dept>8</Dept> <UserPK>2</UserPK> <Currency>133</Currency> <Parameter> <ParamName>CMP_PK</ParamName>  <Values>  <Value>1</Value>   </Values> </Parameter> <Parameter> <ParamName>COA_PK</ParamName>  </Parameter> </FilterParameters>");
        }
        #region Function
        //public void SetReport()
        //{
        //    paramField = new ParameterField();
        //    paramDiscreteValue = new ParameterDiscreteValue();
        //    paramFields = new ParameterFields();
        //    reportDocument = new ReportDocument();

        //    // paramField.Name = "@P_IVH_PK";
        //    //paramDiscreteValue.Value = txtInvNo.Text.ToString();
        //    //paramField.CurrentValues.Add(paramDiscreteValue);
        //    //paramFields.Add(paramField);

        //    reportDocument.Load(Server.MapPath("~/Reports/rptAccount.rpt"));
        //    crReportViewer.ReportSource = reportDocument;
        //    crReportViewer.ParameterFieldInfo = paramFields;
        //    ReportDBSetting.SetDataBaseConnection(reportDocument);
        //    Session["rptParam"] = paramFields;
        //    Session["rptData"] = reportDocument;

        //}
        private void BindCrystalReport(string paramName, string reportName, string curFilter)
        {
            
            paramField = new ParameterField();
            paramDiscreteValue = new ParameterDiscreteValue();
            paramFields = new ParameterFields();
            reportDocument = new ReportDocument();
            paramField.Name = paramName;
            paramDiscreteValue.Value = curFilter;
            paramField.CurrentValues.Add(paramDiscreteValue);
            paramFields.Add(paramField);

            reportDocument.Load(Server.MapPath("~/Reports/CrystalReportFiles/" + reportName));
            // crReportViewer.ParameterFieldInfo.Clear();
            reportDocument.Refresh();
            SetCrystalReportDataBaseConnection(reportDocument);
            crReportViewer.ReportSource = reportDocument;
            crReportViewer.ParameterFieldInfo = paramFields;

            //Report Database Connection Setting
            //CommonFunctions.SetCrystalReportDataBaseConnection(reportDocument);
            //crReportViewer.RefreshReport();
            //crReportViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.ParameterPanel;
            //crReportViewer.HasToggleGroupTreeButton = false;
            //Keep Parameter and report data for page navigation
            Session["rptParam"] = paramFields;
            Session["rptData"] = reportDocument;

        }
        public static void SetCrystalReportDataBaseConnection(ReportDocument reportDocument)
        {
            string connectString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);

            ConnectionInfo connectionInf = new ConnectionInfo();
            connectionInf.ServerName = builder.DataSource;
            connectionInf.UserID = builder.UserID;
            connectionInf.Password = builder.Password;
            connectionInf.DatabaseName = builder.InitialCatalog;
            TableLogOnInfo crtablelogoninfo = new TableLogOnInfo();
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in reportDocument.Database.Tables)
            {
                crtablelogoninfo = CrTable.LogOnInfo;
                crtablelogoninfo.ConnectionInfo = connectionInf;
                CrTable.ApplyLogOnInfo(crtablelogoninfo);
            }
        }
        #endregion
    }
}