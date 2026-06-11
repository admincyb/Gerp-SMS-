using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;

namespace DataAccess.Finance
{
    public class GSTReportDL
    {
        //Get Report list for dropdown bind
        public static DataTable GetGSTReportTypeList(int CfgPk, byte Active, string CFG_Type, int bizUnit, string CfgSplCond)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.CFG_PK,CfgPk ),
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.CFG_TYPE, CFG_Type),
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.CFG_SPL_COND, CfgSplCond == string.Empty ?(Object)DBNull.Value : CfgSplCond),
                };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTREPORTLIST, colParameters).Tables[0];
            return dtProcess;
        }
        //Get Tax category for listbox bind
        public static DataTable GetTaxCategory(int TaxPk, byte Active,int TaxCategory,int TaxSubCategory,int taxIsPurchase,int taxIsReturn, int bizUnit)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.TAX_PK, TaxPk),
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.TAX_CATEGORY, TaxCategory),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.TAX_SUB_CATEGORY, TaxSubCategory == 0 ? (Object)DBNull.Value : TaxSubCategory),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.TAX_IS_RETURN, taxIsReturn), //GST Tax Filter
                    //new DBService.Parameters( GTIService.Constants.Finance.Parameters.TAX_IS_PURCHASE, taxIsPurchase),
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETTAXCATEGORY, colParameters).Tables[0];
            return dtProcess;
        }
        //Get Report for Report viewer
        public static DataSet GetReportData(GridPrams grid, byte Active, string taxType, string reportType, int unRecRcd, int bizUnit)
        {
            DataTable dtProcess = new DataTable();
            DataSet dsProccess = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.V_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.V_FROM_DATE , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.V_TO_DATE , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.TAX_TYPE, taxType== string.Empty ?(Object)DBNull.Value : taxType),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.REPORT_TYPE, reportType),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.UNRECRCD, unRecRcd),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.CMP_PK,(Object)DBNull.Value),
            };
            if (reportType == "6")//Monthly summary tax rpt
                dsProccess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTMONTHLYREPORTDATA, colParameters);
            else
                dsProccess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTREPORTDATA, colParameters);
            return dsProccess;
        }

        /// <summary>
        /// Method to get GSTR Reports(GSTR1,GSTR2,GSTR3)
        /// objParams.ReportType = 1 -- GSTR1
        /// objParams.ReportType = 2 -- GSTR2
        /// objParams.ReportType = 3 -- GSTR3
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public static DataSet GetGSTRReports(BusinessObject.Finance.GSTRParams objParams)
        {
            DataSet dsResult = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FROM_DATE, objParams.FromDate.HasValue ? objParams.FromDate : (Object)DBNull.Value),                    
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_TO_DATE, objParams.ToDate.HasValue ? objParams.ToDate : (Object)DBNull.Value),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_RPT_TYPE, objParams.ReportType > 0 ? objParams.ReportType : (Object)DBNull.Value),                   
                    new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, objParams.BizUnit)
                };
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_GSTR_GET, colParameters);
            return dsResult;
        }
    }
}
