using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.Sales
{
    public class ReceiptDL
    {
        /// <summary>
        /// Method to save sales receipt
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="ReceiptNo"></param>
        /// <returns></returns>
        public static long? SaveReceipt(string xmlDoc, out string ReceiptNo)
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
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_RECEIPT_CUS_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            ReceiptNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }

        /// <summary>
        /// Method to delete sales receipt
        /// </summary>
        /// <param name="ReceiptPk"></param>
        /// <param name="UserPk"></param>
        /// <param name="LastModifiedTime"></param>
        /// <param name="appType"></param>
        /// <returns></returns>
        public static long? DeleteReceipt(long ReceiptPk, int UserPk, DateTime LastModifiedTime, string appType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_PK, ReceiptPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, LastModifiedTime.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER ,UserPk),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.App_Type, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_RECEIPT_CUS_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to get receipt list
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="CustomerPk"></param>
        /// <param name="ReceiptPk"></param>
        /// <param name="invNo"></param>
        /// <param name="pageUrl"></param>
        /// <param name="status"></param>
        /// <param name="PDCStatus"></param>
        /// <param name="cmpPk"></param>
        /// <returns></returns>
        public static DataSet GetReceiptList(GridPrams grid, User objUser, int CustomerPk, long ReceiptPk, string invNo, string pageUrl, int status, int PDCStatus, int cmpPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : (grid.SearchBy =="PVH_PK"?grid.SearchValue:grid.SearchValue+"%")),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),      
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL,  pageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),   
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_STATUS_FILTER,  status),  
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_PDC_STATUS,  PDCStatus> 0 ? PDCStatus : (object)DBNull.Value),             
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_ICH_NO,  invNo == string.Empty ? (object) DBNull.Value : invNo),   
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_CUSTOMER,  CustomerPk > 0 ? CustomerPk : (object)DBNull.Value),            
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_RCH_COMPANY,  cmpPk > 0 ? cmpPk : (object)DBNull.Value)
            };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPFIN_RECEIPT_CUS_GET_LIST, colParameters);
            return dsList;
        }
    }
}
