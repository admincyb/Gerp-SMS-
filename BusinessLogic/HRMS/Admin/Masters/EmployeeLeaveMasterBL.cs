using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class EmployeeLeaveMasterBL
    {
        //Get Employee Leave Master List
        public static string GetEmployeeLeaveList(int currPk, int bizUnit)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.GetEmployeeLeaveList(currPk, bizUnit);
        }
         
        /// <summary>
        /// Method to Save Employee LeaveMaster
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns>int</returns>
        public static int SaveEmployeeLeaveMaster(string xmlstr, out DataTable dtErrorList)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.SaveEmployeeLeaveMaster(xmlstr, out dtErrorList);
        }

        /// <summary>
        /// Method to Delete EmployeeLeaveMaster
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteEmployeeLeaveMaster(int pk, string lastModifiedDate)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.DeleteEmployeeLeaveMaster(pk, lastModifiedDate);
        }

        /// <summary>
        /// Method to Get Current Leave Credit
        /// </summary>
        /// <param name="employeeID"></param>
        ///<param name="leaveTypeID"></param>
        ///<param name="date"></param>
        /// <returns>int</returns>
        public static int GetCurrentLeaveCredit(int employeeID, int leaveTypeID, string date)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.GetCurrentLeaveCredit(employeeID, leaveTypeID, date);
        }

        /// <summary>
        /// Method to Get Leave Credit List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetEmployeeLeaveListListingPage(BusinessObject.GridPrams gridParam, int bizUnit, string FilterFromDate, string FilterToDate, string Employee)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.GetEmployeeLeaveListListingPage(gridParam, bizUnit, FilterFromDate, FilterToDate, Employee);
        }

        public static DataTable GetLeaveCredt(int? lvType, int? empPk, int active, int bizUnit)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.GetLeaveCredt(lvType, empPk,active, bizUnit);
        }

        public static DataTable GetOBLeaveEmployeeListByFilter(int? empPK,  int? branch = null, int? EmpDept = null, int? EmpType = null,int? leaveType = null, string toDate = null)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.GetOBLeaveEmployeeListByFilter(empPK, branch, EmpDept, EmpType, leaveType, toDate);
        }

    }
}
