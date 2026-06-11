using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.ReportViewer.Html5.WebForms;

namespace ERPSMS_v01
{
    public partial class TelerikRptViewer : System.Web.UI.Page
    {
        //C:\Program Files (x86)\Progress\Telerik UI for ASP.NET AJAX R1 2021\Bin45\Telerik.Web.UI.dll
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

               // Test();
                Test2();

                //Telerik.Reporting.SqlDataSource sqlDataSource = new Telerik.Reporting.SqlDataSource();
                ////sqlDataSource.ConnectionString = "MyAdventureWorksDB";
                //sqlDataSource.SelectCommand = "dbo.SPPRD_COMP_TRX_DTL_MIS_RPT";
                //sqlDataSource.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
                //sqlDataSource.ConnectionString = "TEST";
                //sqlDataSource.Parameters.Add("@P_XML", System.Data.DbType.Xml, "<FilterParameters><FromDate>08-Mar-2021</FromDate><ToDate>08-Mar-2021</ToDate><BizUnit>2</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>");

                //////var typeReportSource = new Telerik.Reporting.TypeReportSource();
                ////var typeReportSource = new Telerik.Reporting.
                ////typeReportSource.TypeName = "Telerik.Reporting.Examples.CSharp.ListBoundReport, CSharp.ReportLibrary";
                ////this.ReportViewer1.ReportSource = typeReportSource;
                //// ReportViewer1.ReportSource.Identifier = "CompoundDetails.trdp";

                ////var uriReportSource = new Telerik.Reporting.UriReportSource();
                ////uriReportSource.Uri = "CompoundDetails.trdp";
                ////// Adding the initial parameter values
                ////uriReportSource.Parameters.Add(new Telerik.Reporting.Parameter("P_XML", "<FilterParameters><FromDate>01-Mar-2021</FromDate><ToDate>08-Mar-2021</ToDate><BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>"));
                ////this.ReportViewer1.ReportSource = uriReportSource;

                //var uriReportSource1 = new Telerik.ReportViewer.Html5.WebForms.ReportSource();

                //uriReportSource1.Identifier = "CompoundDetails.trdp";
                //// Adding the initial parameter values
                ////  uriReportSource1.Parameters.Add("P_XML", "<FilterParameters><FromDate>08-Mar-2021</FromDate><ToDate>08-Mar-2021</ToDate><BizUnit>2</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>");
                //Telerik.ReportViewer.Html5.WebForms.Parameter p1 = new Telerik.ReportViewer.Html5.WebForms.Parameter();
                //p1.Name = "@P_XML";
                //p1.Value = "<FilterParameters><FromDate>08-Mar-2021</FromDate><ToDate>08-Mar-2021</ToDate><BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>";

                //uriReportSource1.Parameters.Add(p1);


                //this.ReportViewer1.ReportSource = uriReportSource1;







            }
        }
        private void Test()
        {
            if (!IsPostBack)
            {
                var uriReportSource1 = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
                uriReportSource1.Identifier = "CompoundDetails.trdp";
                uriReportSource1.IdentifierType = IdentifierType.UriReportSource;
                uriReportSource1.Parameters.Add("P_XML", "<FilterParameters><FromDate>8-Mar-2021</FromDate><ToDate>26-Mar-2021</ToDate><BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>");
                this.ReportViewer1.ReportSource = uriReportSource1;
            }

        }
        private void Test2()
        {
            var typeReportSource1 = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
            typeReportSource1.Identifier = "CompoundDetails.trdp";
            typeReportSource1.IdentifierType = IdentifierType.TypeReportSource;
            typeReportSource1.Parameters.Add("P_XML", "<FilterParameters><FromDate>9-Mar-2021</FromDate><ToDate>9-Mar-2021</ToDate><BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>");
            this.ReportViewer1.ReportSource = typeReportSource1;

        }

        private void Test3()
        {
            if (!IsPostBack)
            {
                Telerik.ReportViewer.Html5.WebForms.ReportSource rptSource = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
                rptSource.IdentifierType = Telerik.ReportViewer.Html5.WebForms.IdentifierType.TypeReportSource;
              //  rptSource.Identifier = typeof(SellThroughSummary).AssemblyQualifiedName;
              //  rptSource.Parameters.Add("CoName", "Beside LLC");
                rptSource.Parameters.Add("BrandDes", "ALL");
                rptSource.Parameters.Add("SeasonDes", "SS18");
                rptSource.Parameters.Add("StoreDes", "TDM-D");
                rptSource.Parameters.Add("GenDes", "ALL");
                rptSource.Parameters.Add("CatDes", "ALL");
                rptSource.Parameters.Add("Descr", "ALL");
                rptSource.Parameters.Add("FromDt", "2018-01-01");
                rptSource.Parameters.Add("ToDt", "2018-01-31");

                this.ReportViewer1.ReportSource = rptSource;

                Telerik.Reporting.SqlDataSource sqlDataSource = new Telerik.Reporting.SqlDataSource();
                sqlDataSource.ProviderName = "System.Data.SqlClient";
                sqlDataSource.ConnectionString = "ReportLibrary.Properties.Settings.ReportServer";
                sqlDataSource.SelectCommand = "MIS_SellThru";
                sqlDataSource.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
                sqlDataSource.Parameters.Add("CoName", System.Data.DbType.String, "=Parameters.CoName");
                sqlDataSource.Parameters.Add("BrandDes", System.Data.DbType.String, "=Parameters.BrandDes");
                sqlDataSource.Parameters.Add("SeasonDes", System.Data.DbType.String, "=Parameters.SeasonDes");
                sqlDataSource.Parameters.Add("StoreDes", System.Data.DbType.String, "=Parameters.StoreDes");
                sqlDataSource.Parameters.Add("GenDes", System.Data.DbType.String, "=Parameters.GenDes");
                sqlDataSource.Parameters.Add("CatDes", System.Data.DbType.String, "=Parameters.CatDes");
                sqlDataSource.Parameters.Add("Descr", System.Data.DbType.String, "=Parameters.Descr");
                sqlDataSource.Parameters.Add("FromDt", System.Data.DbType.String, "=Parameters.FromDt");
                sqlDataSource.Parameters.Add("ToDt", System.Data.DbType.String, "=Parameters.ToDt");
                sqlDataSource.CommandTimeout = 0;

             //   report.DataSource = sqlDataSource; //you will also need to bind the data source to the report
            }
        }
    }


}