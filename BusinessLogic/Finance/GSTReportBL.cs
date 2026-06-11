using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using DataAccess.Finance;
using BusinessObject;
using System.Data;
using BusinessObject.Finance;
namespace BusinessLogic.Finance
{
    public class GSTReportBL
    {
        public static DataTable GetGTSReportTypeList(int CfgPk, byte Active, string CFG_Type, int bizUnit, string CfgSplCond)
        {
            return GSTReportDL.GetGSTReportTypeList(CfgPk, Active, CFG_Type, bizUnit, CfgSplCond);
        }
        public static DataTable GetTaxCategory(int TaxPk, byte Active, int TaxCategory, int TaxSubCategory,int taxIsPurchase,int taxIsReturn, int bizUnit)
        {
            return GSTReportDL.GetTaxCategory(TaxPk, Active, TaxCategory, TaxSubCategory,taxIsPurchase,taxIsReturn, bizUnit);
        }
        public static DataSet GetReportData(GridPrams grid, byte Active, string taxType, string reportType, int unRecRcd, int bizUnit)
        {
            return GSTReportDL.GetReportData(grid,Active,taxType,reportType,unRecRcd,bizUnit);
        }

        /// <summary>
        /// Method to get GSTR Reports(GSTR1,GSTR2,GSTR3)
        /// objParams.ReportType = 1 -- GSTR1
        /// objParams.ReportType = 2 -- GSTR2
        /// objParams.ReportType = 3 -- GSTR3
        /// </summary>
        /// <param name="objParams"></param>
        /// <returns></returns>
        public static DataSet GetGSTRReports(GSTRParams objParams)
        {
            return GSTReportDL.GetGSTRReports(objParams);
        }
    }
}
