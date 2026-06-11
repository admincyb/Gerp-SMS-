using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
    public class LeaveTypeMasterDL
    {
        public static int? SaveLeaveType(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_LEAVE_TYPE_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataTable GetLeaveType(int? ltmPK, string code, string name, int active, int bizUnit, int pageNo, int pageSize,int accural,string sortOrder=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_LTM_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_LTM_CODE, code==string.Empty?(object)DBNull.Value:code),
                new DBService.Parameters(Parameters.P_LTM_NAME, name==string.Empty?(object)DBNull.Value:name),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_PAGE_NUM, pageNo==0?(object)DBNull.Value:pageNo),
                new DBService.Parameters(Parameters.P_PAGE_SIZE, pageSize==0?(object)DBNull.Value:pageSize),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_LTM_ACCURAL, accural==-1?(object)DBNull.Value:accural),
                new DBService.Parameters(Parameters.P_SORT_BY, sortOrder!=null ? sortOrder!=string.Empty?sortOrder:(object)DBNull.Value:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_LEAVE_TYPE_MST_GET_KV, colParameters).Tables[0];
        }
        public static DataTable GetLeaveTypeDDL(int? ltmPK, string code, string name, int active, int bizUnit, int pageNo, int pageSize, int accural, string sortOrder = null, int? IsCredit = null, int? EmployeePk = null  )
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_LTM_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_LTM_CODE, code==string.Empty?(object)DBNull.Value:code),
                new DBService.Parameters(Parameters.P_LTM_NAME, name==string.Empty?(object)DBNull.Value:name),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_PAGE_NUM, pageNo==0?(object)DBNull.Value:pageNo),
                new DBService.Parameters(Parameters.P_PAGE_SIZE, pageSize==0?(object)DBNull.Value:pageSize),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_LTM_ACCURAL, accural==-1?(object)DBNull.Value:accural),
                new DBService.Parameters(Parameters.P_LTM_CREDIT, !IsCredit.HasValue?(object)DBNull.Value:IsCredit),
                new DBService.Parameters(Parameters.P_EMP_PK, !EmployeePk.HasValue?(object)DBNull.Value:EmployeePk<=0?(object)DBNull.Value:EmployeePk)
                //new DBService.Parameters(Parameters.P_SORT_BY, sortOrder!=null ? sortOrder!=string.Empty?sortOrder:(object)DBNull.Value:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_LEAVE_TYPE_MST_GET_KV, colParameters).Tables[0];
        }
        public static int DeleteLeaveType(int ltmPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_LTM_PK, ltmPK),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT, lastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_LEAVE_TYPE_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int UpdateLeaveTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTM_PK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TYPE_MST_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }

    }
}
