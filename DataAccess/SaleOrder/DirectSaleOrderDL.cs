using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;


namespace DataAccess.SaleOrder
{
    public class DirectSaleOrderDL
    {
        /// <summary>
        /// Save Sale Order Workflow Details
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int? SaveDirectSaleOrderWkfDetails(string xmlDoc, out int refID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            return result;
        }

        /// <summary>
        /// Get Direct Sale Order details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataSet GetDirectSOGetList(GridPrams grid, User objUser, string dsoNoSearch, int cusPk, string sohType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE,  grid.PageSize),            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUS, string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, objUser.SBUID),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_NO, dsoNoSearch == string.Empty ? (object)DBNull.Value :dsoNoSearch),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_CUSTOMER,  cusPk==0? (object)DBNull.Value:cusPk),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_TYPE,  string.IsNullOrEmpty(sohType) ? (object)DBNull.Value : sohType),     
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_GET_LIST, colParameters);
        }

        /// <summary>
        /// Method to Save StockTransfer Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string GetDirectSaleOrderByPk(int sohPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, sohPk)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_GET, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// Delete Sale order details by PK
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static int DeleteSaleOrderDetails(int sohPK, DateTime lastModDate, string deleteReason = "")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, sohPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DELETEREASON, string.IsNullOrEmpty(deleteReason) ? (object) DBNull.Value :  deleteReason),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get DirectDODetail SaleOrder Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectDOSaleOrderDetailList(int sohPk = 0, int sodPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  sohPk==0? (object)DBNull.Value:sohPk),
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOD_PK,  sodPk==0? (object)DBNull.Value:sodPk) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_DTL_GET, colParameters).Tables[0];
        }

        /// <summary>
        /// Method to get Auto Complete Search 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetDirectSONoAutocomplete(string searchBy, string searchValue, User objUser, string pageURl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE  ,  searchValue),            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  ,  objUser.PKUser > 0 ? objUser.PKUser : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT  ,  objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL  ,  pageURl),  
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_DIR_AUTO, colParameters).Tables[0];
        }

        /// <summary>
        ///Validation For Cancellation of Direct SO cancel 
        /// </summary>
        /// <param name="CurrPK"></param>    
        /// <returns></returns>
        public static bool ValidationForCancellationSO(int SohPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {    
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK, SohPK > 0 ? SohPK : (object)DBNull.Value)
            };
            DataSet dsArchive = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_SO_DIRECT_CANCEL_CHECK, colParameters);
            return dsArchive == null || dsArchive.Tables.Count == 0 || dsArchive.Tables[0].Rows.Count == 0;
        }
        /// <summary>
        /// Save Direct Sale Order Short Close
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="reason"></param>
        /// <param name="refNo"></param>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static int? SaveDirectSaleOrderShortClose(int sohPK, string reason, string refNo, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SOH_PK,  sohPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SHORT_CLS_REASON, reason),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.SHORT_CLS_REFNO, refNo),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SAVEDIRECTSALEORDER_SHORT_CLS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
