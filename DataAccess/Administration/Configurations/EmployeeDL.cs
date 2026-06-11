using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Administration.Configurations
{
    public class EmployeeDL
    {
        /// <summary>
        /// method for get Employees Details
        /// </summary>
        /// <param name="userID"></param>

        /// <returns>DataSet</returns>
        public static DataTable GetEmployees(int empID, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.EMP_PK, empID == 0 ? (object)DBNull.Value : empID),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.EMPCategory,  0),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.ACTIVE,  active),

            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Employees.Procedure.SP_GET_EMPLOYEES, colParameters);
            return dtUser.Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetEmployees(int empID, int category, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.EMP_PK, empID == 0 ? (object)DBNull.Value : empID),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.EMPCategory,  category),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.ACTIVE,  active),

            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Employees.Procedure.SP_GET_EMPLOYEES, colParameters);
            return dtUser.Tables[0];

        }

        /// <summary>
        /// method for  Get Employees List
        /// </summary>
        /// <param name="empID">Employee Pk</param>
        /// <param name="empCategory">Employee category</param>
        /// <param name="excludeHrmsEmp">Exclude/Include HRMS employees </param>
        /// <param name="excludeCusEmp">Exclude/Include Customer portal employees</param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetEmployees(int empID, int? empCategory, int? excludeHrmsEmp, int? excludeCusEmp, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.EMP_PK, empID == 0 ? (object)DBNull.Value : empID),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.EMPCategory,  empCategory.HasValue? empCategory : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.P_EXLUDE_HRMS,  excludeHrmsEmp.HasValue? excludeHrmsEmp : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.P_EXLUDE_CUST,  excludeCusEmp.HasValue? excludeCusEmp : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.ACTIVE,  active),

            };

            DataSet dtUser = new DataSet();
            dtUser = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Employees.Procedure.SP_GET_EMPLOYEES, colParameters);
            return dtUser.Tables[0];
        }

        public static DataTable GetEmployeeRoles(int empPK, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                 //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
                 new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.P_ACTIVE , 1),
                 new DBService.Parameters(GTIService.Constants.Configurations.Employees.Parameters.P_EMP_PK , empPK),

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Configurations.Employees.Procedure.SP_ADM_EMP_ROLE_GET, colParameters).Tables[0];
        }

        public static int SaveEmployeeRoleDetails(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML, strxml),               
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Configurations.Employees.Procedure.SPADM_EMP_ROLE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
            return result;

        }
    }
}
