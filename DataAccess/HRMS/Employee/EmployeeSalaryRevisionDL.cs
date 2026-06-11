using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeSalaryRevisionDL
    {
        /// <summary>
        /// Get Employee Salary/Designation History
        /// </summary>
        /// <param name="employeePK">int</param>
        /// <returns>DataTable</returns>
        public static DataTable GetEmployeeSalaryHistory(int employeePK)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMP_PK,employeePK)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PAY_ELEMENT_HISTORY_GET, colParameters).Tables[0];
            return dtList;
        }
    }
}
