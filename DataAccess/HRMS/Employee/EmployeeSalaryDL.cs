using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Employee;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeSalaryDL
    {
        public static string GetEmployeeSalaryDetails(int empPk, string toDate, string trxDate = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_EMPLOYEE_PK, empPk),
                new DBService.Parameters(Parameters.P_TO_DATE, toDate==string.Empty?(Object)DBNull.Value:toDate),
                new DBService.Parameters(Parameters.P_TR_DATE, trxDate==string.Empty?(Object)DBNull.Value:trxDate)
            };

            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAY_ELEMENT_DTL_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        public static int SaveEmployeeSalaryDetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAY_ELEMENT_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int DeleteEmployeeSalary(int EmployeePK, DateTime? LastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_EDP_EMPLOYEE	, EmployeePK),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT	, LastModDate.HasValue? LastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SALARY_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ERP.Utilities.HRMS.Employee.P_RET_VAL]).Value);
        }
    }
}
