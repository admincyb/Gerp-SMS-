using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.HRMS;
using BusinessObject.HRMS.Payroll;
using GTIService;

namespace BusinessLogic.HRMS.Payroll
{
    public class MonthlyLeaveBL
    {
        //Get Employee Leave Master List
        public static string GetEmployeeMonthlyLeaveList(int employeeID, DateTime? date)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.GetEmployeeMonthlyLeaveList(employeeID, date);
        }

        /// <summary>
        /// Method to Save Employee Monthly LeaveMaster
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns>int</returns>
        public static int SaveEmployeeMonthlyLeaveMaster(string xmlstr)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.SaveEmployeeMonthlyLeaveMaster(xmlstr);
        }

        /// <summary>
        /// Method to Get Yearly Leave List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetYearlyLeaveListListingPage(BusinessObject.GridPrams gridParam, int salaryYearPk)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.GetYearlyLeaveListListingPage(gridParam, salaryYearPk);
        }

        /// <summary>
        /// Method to Get Salary Pks
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalaryPkList(int hyrPk, int active, int bizUnit)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.GetSalaryPkList(hyrPk, active, bizUnit);
        }

        public static double GetBalanceLeave(int EmployeePk, int LeaveType, DateTime? DateUpTo)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.GetBalanceLeave(EmployeePk, LeaveType, DateUpTo);
        }

        public static int SaveEmployeeLeave(string xmlstr, out string RetNo, out string empResignDate)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.SaveEmployeeLeave(xmlstr, out RetNo,out  empResignDate);
        }

        public static DataSet GetLeaveList(FilterParameters objFilterParam)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.GetLeaveList(objFilterParam);
        }

        public static MonthlyLeaveBO.LeaveEntryMaster GetLeaveDetails(FilterParameters objFilterParam)
        {
            try
            {
                MonthlyLeaveBO.LeaveEntryMaster objLeave = new MonthlyLeaveBO.LeaveEntryMaster();
                string result = DataAccess.HRMS.Payroll.MonthlyLeaveDL.GetLeaveDetails(objFilterParam);
                if (!string.IsNullOrEmpty(result))
                {
                    objLeave = (MonthlyLeaveBO.LeaveEntryMaster)CommonFunctions.DeserializeObject(result, objLeave);
                    return objLeave;
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

        public static int DeleteLeave(int CurrPK, DateTime LastModifiedTime, int lvExist = 0)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.DeleteLeave(CurrPK, LastModifiedTime,lvExist);
        }

        public static int DeleteDtlLeave(int CurrPK, DateTime LastModifiedTime, int lvExist = 0)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.DeleteDtlLeave(CurrPK, LastModifiedTime,lvExist);
        }

        public static int DeleteLeaveDays(string xmlstr)
        {
            return DataAccess.HRMS.Payroll.MonthlyLeaveDL.DeleteLeaveDays(xmlstr);
        }
        
    }
}
