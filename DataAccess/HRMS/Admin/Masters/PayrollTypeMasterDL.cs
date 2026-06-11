using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
   public class PayrollTypeMasterDL
   {
       /// <summary>
       /// Method to Get Salary Templates for Listing Page
       /// </summary>
       /// <param name="gridParam"></param>
       ///<param name="bizUnit"></param>
       /// <returns>DataSet</returns>
       public static DataSet GetPayrollTypeList(int? currPK, int active, int pageNo, int pageSize, int bizUnit, int ProcessMode,string PayrollType = null, string PayrollTypeName = null)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PTM_PK, currPK.HasValue?currPK:(object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_NO , pageNo),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  pageSize),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
              new DBService.Parameters(Parameters.P_SORT_BY, "PTM_CODE"),
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_PTM_CODE, PayrollType!=null ? PayrollType!=string.Empty?PayrollType:(object)DBNull.Value:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_PTM_NAME, PayrollTypeName!=null ? PayrollTypeName!=string.Empty?PayrollTypeName:(object)DBNull.Value:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_PTM_PRC_MODE,  ProcessMode >0 ? ProcessMode : (object)DBNull.Value),
            }; 
           return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_PAYROLL_TYPE_GET_KV, colParameters);
       }

       public static int UpdatePayrollTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PTM_PK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_PAYROLL_TYPE_MST_ACTIVATE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
           return result;
       }

       public static DataTable GetSPayrollType(int? ltmPK,  int active, int bizUnit)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PTM_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_PAYROLL_TYPE_GET_KV, colParameters).Tables[0];
       }

       /// <summary>
        /// Method to Save Salary Template
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
       public static int SavePayrollTypeMaster(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_PAYROLL_TYPE_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

       /// <summary>
       /// Method to Delete StockTransfer Details
       /// </summary>
       /// <param name="pk"></param>
       /// /// <param name="lastModifiedDate"></param>
       /// <returns>int</returns>
       public static int DeletePayrollTypeMaster(int pk, string lastModifiedDate)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PTM_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_PAYROLL_TYPE_MST_DELETE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
           return result;
       }

       public static string GetUser(int pk)
       {
           string strRetVal = "";
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.V_PTM_PK , pk)
            };

           DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_PAYROLL_TYPE_GET_XML, colParameters).Tables[0];
           foreach (DataRow dr in dtxml.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }
    }
}
