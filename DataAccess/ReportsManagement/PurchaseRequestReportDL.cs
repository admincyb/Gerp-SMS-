using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.ReportsManagement
{
    public class PurchaseRequestReportDL
    {
        /// <summary>
        /// Procedure to get Purchase Request Report
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="status"></param>
        /// <param name="requestNumber"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseRequests(string fromDate, string toDate, int status, string requestNumber, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_FROMDATE  , string.IsNullOrEmpty(fromDate)?DateTime.Now.AddDays(-1).ToShortDateString(): Convert.ToDateTime(fromDate).ToShortDateString()),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_TODATE, string.IsNullOrEmpty(toDate)?DateTime.Now.ToShortDateString(): Convert.ToDateTime(toDate).ToShortDateString()),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_STATUS, status<0?(object)DBNull.Value:status),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_NO  , string.IsNullOrEmpty(requestNumber)?(object)DBNull.Value:requestNumber),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_SBU  , sbu),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_PR_REPORT, colParameters).Tables[0];
        }
        /// <summary>
        /// Procedure to get Purchase Request Statuses
        /// </summary>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPRStatus(int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_SBU  , sbu),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_PR_STATUS, colParameters).Tables[0];
        }
        /// <summary>
        /// Procedure to get Purchase Request Items
        /// </summary>
        /// <param name="requestNumber"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPRItems(int requestNumber, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_PK  , requestNumber==0?(object)DBNull.Value:requestNumber)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_PR_ITEMS, colParameters).Tables[0];
        }
        /// <summary>
        /// Procedure to get Purchase Request Vendour
        /// </summary>
        /// <param name="requestNumber"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPRVendour(int requestNumber, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_PRR_PK  , requestNumber==0?(object)DBNull.Value:requestNumber)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_PR_VENDOUR, colParameters).Tables[0];
        }
    }
}
