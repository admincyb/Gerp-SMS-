using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.StoreManagement
{
    public class GRNDailyReport
    {
        /// <summary>
        /// Get GRN Daily Details For Report
        /// </summary>
        /// <param name="store"></param>
        /// <param name="date"></param>
        /// <param name="catg"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetGRNDailyDtls(int store, string date, int catg, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.DEPT_PK , store==0?(object)DBNull.Value:store),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.DATE , date==string.Empty?DateTime.Now: Convert.ToDateTime(date)),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.ITEM_CATG , catg==0?(object)DBNull.Value:catg),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.SBU  , sbu),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_GRN_STOCK_DTLS_REPORT, colParameters).Tables[0];
        }
    }
}
