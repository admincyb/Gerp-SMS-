using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeSalaryRevisionBL
    {
        /// <summary>
        /// Get Employee Salary/Designation History
        /// </summary>
        /// <param name="employeePK">int</param>
        /// <returns>DataTable</returns>
        public static DataTable GetEmployeeSalaryHistory(int employeePK)
        {
           return DataAccess.HRMS.Employee.EmployeeSalaryRevisionDL.GetEmployeeSalaryHistory(employeePK);
        }
    }
}
