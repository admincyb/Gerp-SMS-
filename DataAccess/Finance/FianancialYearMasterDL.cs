using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;


namespace DataAccess.Finance
{
   public class FianancialYearMasterDL
    {
       public static int? SaveFinancialYear(FinancialYearMasterBO objFinYear, int deptPK, int userPK, int bizunit, string lastmodifieddate = null)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK, objFinYear.FYR_PK),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_NAME,objFinYear.FYR_NAME),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_DESC,objFinYear.FYR_DESC),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_DATE_FROM,objFinYear.FYR_DATE_FROM),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_DATE_TO,objFinYear.FYR_DATE_TO),
               // new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_ACTIVE,active),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_STATUS,objFinYear.FYR_STATUS),
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_DEPT,deptPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,objFinYear.FYR_ACTIVE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,bizunit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT,lastmodifieddate==string.Empty ? (object)DBNull.Value:lastmodifieddate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_MST_SAVE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }

       public static DataTable GetFinYearDetails( int active, int bizunit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK,  (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,active),        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,bizunit),
               // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_LEAVE_TYPE_MST_DELETE, colParameters);
           return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_MST_GET_KV, colParameters).Tables[0];

       }
       public static DataTable GetFinYearEditDetails(int? CurrPK, int active, int bizunit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK,  CurrPK.HasValue?CurrPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,2),        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,bizunit),
               // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_LEAVE_TYPE_MST_DELETE, colParameters);
           return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_MST_GET_KV, colParameters).Tables[0];
           
       }


       public static int DeleteFinancialYear(int? ltmPK, string lastModDate=null)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT,lastModDate==string.Empty ? (object)DBNull.Value:lastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_MST_DELETE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }


    }
}
