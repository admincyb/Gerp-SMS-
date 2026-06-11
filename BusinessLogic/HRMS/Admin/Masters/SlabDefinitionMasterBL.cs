using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.HRMS;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class SlabDefinitionMasterBL
    {
        //Get Employee Leave Master List
        public static string GetEmployeeLeaveList(int currPk, int bizUnit)
        {
            return DataAccess.HRMS.Admin.Masters.EmployeeLeaveMasterDL.GetEmployeeLeaveList(currPk, bizUnit);
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

        /// <summary>
        /// Method to Save Salary slab definitions
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static int SaveSlabDefinition(string xmlstr)
        {
            return DataAccess.HRMS.Admin.Masters.SlabDefinitionMasterDL.SaveSlabDefinition(xmlstr);
        }

        /// <summary>
        /// Method to Get Salary Slab Definition List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetSlabDefinitionList(FilterParameters gridParam, int bizUnit)
        {
            return DataAccess.HRMS.Admin.Masters.SlabDefinitionMasterDL.GetSlabDefinitionList(gridParam, bizUnit);
        }

        /// <summary>
        /// Method to Get Salary Slab Definition Details
        /// </summary>
        /// <param name="SlabPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetSlabDefinitionDetails(int SlabPk, int bizUnit, int Status, int? PayElement = null, int? VersionPk = null)
        {
            return DataAccess.HRMS.Admin.Masters.SlabDefinitionMasterDL.GetSlabDefinitionDetails(SlabPk, bizUnit, Status, PayElement, VersionPk);
        }

        /// <summary>
        /// Method to delete Slab definition
        /// </summary>
        /// <param name="SlabPk">PK</param>
        /// <param name="LastModDate">Modified date</param>
        /// <returns></returns>
        public static int DeleteSlabDefinition(int SlabPk, int? VersionPk, DateTime LastModDate)
        {
            return DataAccess.HRMS.Admin.Masters.SlabDefinitionMasterDL.DeleteSlabDefinition(SlabPk, VersionPk, LastModDate);
        }

        public static int UpdateSlabDefinitionStatus(int SlabPk, int Status, int UserPk, DateTime? LastModDate)
        {
            return DataAccess.HRMS.Admin.Masters.SlabDefinitionMasterDL.UpdateSlabDefinitionStatus(SlabPk, Status, UserPk, LastModDate);
        }
    }
}
