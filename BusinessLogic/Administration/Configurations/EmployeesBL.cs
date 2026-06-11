using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.Administration.Configurations;
using System.Data;
using DataAccess.Administration.Configurations;
using BusinessObject;

namespace BusinessLogic.Administration.Configurations
{
    public class EmployeesBL
    {

        /// <summary>
        /// method for  Get Employees List
        /// </summary>
        /// <param name="roleActionPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetEmployees(int userID, int active = 1)
        {
            return EmployeeDL.GetEmployees(userID, active);
        }
        /// <summary>
        /// Get category Employee
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetEmployees(int userID, int category, int active = 1)
        {
            return EmployeeDL.GetEmployees(userID, category, active);
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
            return EmployeeDL.GetEmployees(empID, empCategory, excludeHrmsEmp, excludeCusEmp, active);
        }
        public static DataTable GetEmployeeRoles(int empPK, int bizUnit)
        {
            return EmployeeDL.GetEmployeeRoles(empPK, bizUnit);
        }

        public static int SaveEmployeeRoleDetails(string strxml)
        {
            return EmployeeDL.SaveEmployeeRoleDetails(strxml);
        }
    }
}
