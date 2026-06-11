using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities.HRMS;
using GTIService;

namespace BusinessLogic.HRMS.Payroll
{
    public class AttendanceBL
    {
        /// <summary>
        /// Get Mapped Customers
        /// </summary>
        /// <param name="eatPk"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <param name="eatDate"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetAttendance(int? eatPk, int? active, int? bizUnit, DateTime? eatDate, int? designation, int? branchLocation, int? department, int? employeeType, int? company, int? employee, DateTime? fromDate, int? empCategory = null)
        {
            try
            {
                DataTable dtAttendance = DataAccess.HRMS.Payroll.AttendanceDL.GetAttendance(eatPk, active, bizUnit, eatDate, designation, branchLocation, department, employeeType, company, employee, fromDate, empCategory);
                return dtAttendance;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Save Attendance
        /// </summary>
        /// <param name="employeeAttendance"></param>        
        /// <returns>int</returns>
        public static int SaveAttendance(EmployeeAttendance employeeAttendance, out string EmpNames, out string TrxNo)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.SaveAttendance(employeeAttendance, out EmpNames, out TrxNo);
        }

        /// <summary>
        /// Import Attendance details
        /// </summary>
        /// <param name="employeeAttendance"></param>        
        /// <returns>int</returns>
        public static int ImportAttendanceDetails(string pXML, out string EmpNames, out string TrxNo)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.ImportAttendanceDetails(pXML, out EmpNames, out TrxNo);
        }

        /// <summary>
        /// Import Attendance
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int ImportAttendance(string pXML, ref DataTable dtOut)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.ImportAttendance(pXML, ref dtOut);
        }

        /// <summary>
        /// GetWorkingHour
        /// </summary>
        /// <param name="employeePk"></param>        
        /// <param name="date"></param>   
        /// <returns>DataTable</returns>     
        public static DataTable GetWorkingHour(int employeePk, string date)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetWorkingHour(employeePk, date);
        }


        /// <summary>
        /// Get employee working hours
        /// </summary>
        /// <param name="EmpPk">Employee Pk</param>
        /// <param name="AttendanceDate">Attendance Date</param>
        /// <returns></returns>
        public static double GetEmpWorkHour(int EmpPk, DateTime AttendanceDate)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetEmpWorkHour(EmpPk, AttendanceDate);
        }

        /// <summary>
        /// Method to delete attendance
        /// </summary>
        /// <param name="attendancePk">Attendance PK</param>
        /// <param name="lastModDate">Modified Date</param>
        /// <returns></returns>
        public static int? DeleteAttendance(int? HeaderPk, int? attendancePk, DateTime? lastModDate)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.DeleteAttendance(HeaderPk, attendancePk, lastModDate);
        }

        public static DataTable GetAttendanceList(FilterParameters objFilterParam, int bizUnit)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetAttendanceList(objFilterParam, bizUnit);
        }

        public static EmployeeAttendance GetAttendanceDetails(FilterParameters objFilterParam)
        {
            try
            {
                EmployeeAttendance objAttendance = new EmployeeAttendance();
                string result = DataAccess.HRMS.Payroll.AttendanceDL.GetAttendanceDetails(objFilterParam);
                if (!string.IsNullOrEmpty(result))
                {
                    objAttendance = (EmployeeAttendance)CommonFunctions.DeserializeObject(result, objAttendance);
                    return objAttendance;
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

        public static AttendanceGen GetAttendance(FilterParameters objFilterParam)
        {
            try
            {
                AttendanceGen objAttendance = new AttendanceGen();
                string result = DataAccess.HRMS.Payroll.AttendanceDL.GetAttendance(objFilterParam);
                if (!string.IsNullOrEmpty(result))
                {
                    objAttendance = (AttendanceGen)CommonFunctions.DeserializeObject(result, objAttendance);
                    return objAttendance;
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

        public static DataTable GetAttendanceDetails(int pk)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetAttendanceDetails(pk);
        }

        public static DataSet GetAttendanceReport(int currPK)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetAttendanceReport(currPK);
        }
        public static DataSet GetAttendanceReportDetails(int currPK)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetAttendanceReportDetails(currPK);
        }
        public static DataSet GetAdditionDeductionReport(int currPK)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetAdditionDeductionReport(currPK);
        }

        # region Extra Days Start
        public static int SaveExtraDays(string xmlDoc, out string TrxNo)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.SaveExtraDays(xmlDoc, out  TrxNo);
        }
        public static DataTable GetExtraDaysList(FilterParameters objFilterParam)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GetExtraDaysList(objFilterParam);
        }
        public static ExtraDaysHeader GetExtraDaysByPK(FilterParameters objFilterParam)
        {
            try
            {
                ExtraDaysHeader ObjEmployeePerformance = new ExtraDaysHeader();
                string employee = DataAccess.HRMS.Payroll.AttendanceDL.GetExtraDaysByPK(objFilterParam);
                if (employee != string.Empty)
                {
                    ObjEmployeePerformance = (ExtraDaysHeader)CommonFunctions.DeserializeObject(employee, ObjEmployeePerformance);
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
        public static int DeleteExtraDays(int CurrPK, DateTime LastModifiedTime)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.DeleteExtraDays(CurrPK, LastModifiedTime);
        }
        public static int ImportExtraDays(string xmlDoc)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.ImportExtraDays(xmlDoc);
        }
        #endregion

       
        /// <summary>
        /// Save General Attendance
        /// </summary>
        /// <param name="employeeAttendance"></param>        
        /// <returns>int</returns>
        public static int SaveGeneralAttendance(AttendanceGen employeeAttendance, out string EmpNames, out string TrxNo)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.SaveGeneralAttendance(employeeAttendance, out EmpNames, out TrxNo);
        }

        public static AttendanceGen GetGeneralAttendanceDetails(FilterParameters objFilterParam)
        {
            try
            {
                AttendanceGen objAttendance = new AttendanceGen();
                string result = DataAccess.HRMS.Payroll.AttendanceDL.GetGeneralAttendanceDetails(objFilterParam);
                if (!string.IsNullOrEmpty(result))
                {
                    objAttendance = (AttendanceGen)CommonFunctions.DeserializeObject(result, objAttendance);
                    return objAttendance;
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

        /// <summary>
        /// Import General Attendance
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int GeneralAttendanceImport(string pXML, ref DataTable dtOut)
        {
            return DataAccess.HRMS.Payroll.AttendanceDL.GeneralAttendanceImport(pXML, ref dtOut);
        }
    }
}
