using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.StoreManagement
{
    public class StockDetailsReportDL
    {
        #region Methods
        /// <summary>
        /// Get daily Stock Details
        /// </summary>
        /// <param name="date"></param>
        /// <param name="store"></param>
        /// <param name="catg"></param>
        /// <param name="item"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetStockDetails(string date, int store, int catg, int item, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.DATE  , date==string.Empty?DateTime.Now: Convert.ToDateTime(date)),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.DEPT_PK, store==0?(object)DBNull.Value:store),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.ITEM_CATG_PK, catg==0?(object)DBNull.Value:catg),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.ITEM_PK  , item==0?(object)DBNull.Value:item),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.SBU  , sbu),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_DAILY_STOCK_REPORT, colParameters).Tables[0];
        }
        /// <summary>
        /// Get all store name with Active, Inavctive and damage store  
        /// </summary>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetStoreNames(int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.WRKF_SBU, sbu),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.GET_STORE_NAME, colParameters).Tables[0];
        }
        #endregion

        public static DataTable GetStockDetailsForRpt(string date, int store, string dateTo,int Category, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_FROMDATE  , date==string.Empty?DateTime.Now: Convert.ToDateTime(date)),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.DEPT_PK, store==0?(object)DBNull.Value:store),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.PRR_TODATE, dateTo==string.Empty?DateTime.Now: Convert.ToDateTime(dateTo)),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_ITM_CATEGORY, Category==0?(object)DBNull.Value:Category),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.SBU  , sbu),
              
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.SPINV_STK_TRX_VALUE_RPT, colParameters).Tables[0];

        }



        public static DataTable GetAllStoreNames(BusinessObject.User objUser, int category)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.SBU, objUser.SBUID),
new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_DPT_TYPE, 2),
              new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_DPT_CATEGORY,  category)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Reports.Procedures.SPADM_USER_DEPT_GET_KV, colParameters).Tables[0];
        }
    }
}
