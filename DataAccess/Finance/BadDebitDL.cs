using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Finance;
using BusinessObject;
namespace DataAccess.Finance
{
   public class BadDebitDL
    {
        /// <summary>
        /// Get Bad Debit 
        /// </summary>
        /// <param name="ApplicationTypePk"></param>
        /// <param name="Active"></param>
        /// <param name="SplCondition"></param>
        /// <returns></returns>
       public static DataSet GetBadDebits(GridPrams grid, User objUser, int MonthInterval)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),             
               new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_MNTH_INTRVL, MonthInterval)                 
            };
           DataSet dsList = new DataSet();
           dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETBADDEBITDETAILS, colParameters);
          
           return dsList;
       }
       /// <summary>
       /// Get BadDebit Relief Details 
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="bizUnit"></param>
       /// <returns>DataSet</returns>
       public static DataSet GetReliefList(GridPrams grid, User objUser, int IBD_PK, int Active)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {     
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? (Object)DBNull.Value : grid.SortBy),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? (Object)DBNull.Value : grid.SortDirection),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
             // new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_USER_PK,  objUser.PKUser),
               new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_IBD_PK, IBD_PK == 0 ? (object) DBNull.Value :  IBD_PK),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active == 0 ? (object) DBNull.Value :  Active)  ,
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID)      
            };
           DataSet dsList = new DataSet();
           dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETBDRELIEFLIST, colParameters);
           return dsList;
       }

       /// <summary>
       /// Get BadDebit Relief Details 
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="bizUnit"></param>
       /// <returns>DataSet</returns>
       public static DataSet GetReliefSpecificDetails(GridPrams grid, User objUser, int IBD_PK, int Active)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_IBD_PK, IBD_PK == 0 ? (object) DBNull.Value :  IBD_PK),   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,Active),   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID)
            };
           DataSet dsList = new DataSet();
           dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETBDRELIEF_GET_KV, colParameters);
           return dsList;
       }
       /// <summary>
       /// Save BadDebit Relief Claim details
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns></returns>
       public static int? SaveBadDebitDetails(string strxml)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SAVEBADDEBITDETAILS, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }

       /// <summary>
       /// Delete BADDEBIT Relief details
       /// </summary>
       /// <param name="invPK"></param>
       /// <param name="lastModDate"></param>
       /// <returns></returns>
       public static int DeleteBadDebitReliefDetails(int IBD_PK, DateTime lastModDate, string appType, string currentUser)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_IBD_PK, IBD_PK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,(Object)DBNull.Value),
               // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER , currentUser),
               // new DBService.Parameters(GTIService.Constants.POInvoicing.Parameters.APP_TYPE, appType),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.DELETEBDRELIEFDETAILS, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }
    }
}
