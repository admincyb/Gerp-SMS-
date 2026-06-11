using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.ReportsManagement
{
    public class CashFlowDL
    {
        /// <summary>
        /// Methord to get the bank Details
        /// </summary>
        /// <param name="pimPk"></param>
        /// <param name="active"></param>
        /// <param name="bizunit"></param>
        /// <param name="packingPk"></param>
        /// <param name="CusPk"></param>
        /// <param name="BrandPk"></param>
        /// <param name="Artwork"></param>
        /// <returns>DataTable</returns>
        public static DataSet GetCashFlowTables(string Date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Reports.Parameters.BDATE , Date)
            };
            DataSet dsSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_PR_CASHFLOW, colParameters);
            return dsSearchValue;

        }


        /// <summary>
        /// Get Report details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataSet GetReportDT(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_XML, pXML),
            };
            DataSet dsGetreportVal = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.RPT_PR_CASHFLOW, colParameters);
            return dsGetreportVal;
        }

    }
}
