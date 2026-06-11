using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Employee;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;

namespace BusinessLogic.HRMS.Employee
{
   public class EmployeePerformanceBL
    {


       public static DataTable GetAction(int? GROUP_TYPE_VALUE, int BIZUNIT, int P_ACTIVE, int GROUP_Pk)
        {
            return EmployeePerformanceDL.GetAction(GROUP_TYPE_VALUE, BIZUNIT, P_ACTIVE, GROUP_Pk);
        }

        public static DataTable GetCategory(int? CNG_GRP_TYPE, int BIZUNIT, int P_ACTIVE, string CNG_SPL_COND)
        {
            return EmployeePerformanceDL.GetCategory(CNG_GRP_TYPE, BIZUNIT, P_ACTIVE, CNG_SPL_COND);
        }

        public static DataTable GetEmployeePerformanceList(int empPk)
        {
            return EmployeePerformanceDL.GetEmployeePerformanceList(empPk);
        }

        public static int? SaveEmployeePerformance(string strxml)
        {
            return EmployeePerformanceDL.SaveEmployeePerformance(strxml);
        }

        public static EmployeePerformanceBO GetEmployeePerformanceByPk(int pk)
        {
            try
            {
                EmployeePerformanceBO ObjEmployeePerformance = new EmployeePerformanceBO();
                string employee = EmployeePerformanceDL.GetEmployeePerformanceByPk(pk);
                if (employee != string.Empty)
                {
                    ObjEmployeePerformance = (EmployeePerformanceBO)CommonFunctions.DeserializeObject(employee, ObjEmployeePerformance);
                    return ObjEmployeePerformance;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static int DeleteEmployeePerformanceById(int pk,string lastModifiedDate)
        {
            return EmployeePerformanceDL.DeleteEmployeePerformanceById(pk,lastModifiedDate);
        }

    }
}
