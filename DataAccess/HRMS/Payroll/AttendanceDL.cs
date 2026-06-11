using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Payroll
{
    public class AttendanceDL
    {

        /// <summary>
        /// Get Mapped Customers
        /// </summary>
        /// <param name="eatPk"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <param name="eatDate"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetAttendance(int? eatPk, int? active, int? bizUnit, DateTime? eatDate, int? designation, int? branchLocation, int? department, int? employeeType, int? company, int? employee, DateTime? fromDate,int? empCategory = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAT_PK, eatPk.HasValue?eatPk.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE, active.HasValue?active.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BIZUNIT, bizUnit.HasValue?bizUnit.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAT_DATE, eatDate.HasValue?eatDate.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDesignation, designation.HasValue?designation.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empBranch, branchLocation.HasValue?branchLocation.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDepartment, department.HasValue?department.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empEmploymentType, employeeType.HasValue?employeeType.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCompany, company.HasValue?company.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCategory, empCategory.HasValue?empCategory.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK, employee.HasValue?employee.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAT_FROM_DATE, fromDate.HasValue?fromDate.Value:(object)DBNull.Value)
            };

            DataTable dtCustomers = new DataTable();
            dtCustomers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_DTL_GET, colParameters).Tables[0];
            return dtCustomers;
        }

        /// <summary>
        /// Save Attendance
        /// </summary>
        /// <param name="employeeAttendance"></param>        
        /// <returns>int</returns>     
        public static int SaveAttendance(EmployeeAttendance employeeAttendance, out string EmpNames, out string TrxNo)
        {


            string xmlDoc = CommonFunctions.XmlSerialize(employeeAttendance);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_EMP, string.Empty, 5000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            EmpNames = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_EMP]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        /// <summary>
        /// Save Attendance
        /// </summary>
        /// <param name="employeeAttendance"></param>        
        /// <returns>int</returns>     
        public static int ImportAttendanceDetails(string pXML, out string EmpNames, out string TrxNo)
        {


            //string xmlDoc = CommonFunctions.XmlSerialize(employeeAttendance);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_EMP, string.Empty, 5000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_DTL_IMPORT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            EmpNames = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_EMP]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        /// <summary>
        ///  Import Attendance
        /// </summary>
        /// <param name="pXML"></param>
        /// <param name="dtOut"></param>
        /// <returns></returns>
        public static int ImportAttendance(string pXML, ref DataTable dtOut)
        {
            DBService dbService;
            DBService.Parameters[] colParameters;

            dbService = new DBService();
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };           
            dtOut = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_IMPORT_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// GetWorkingHour
        /// </summary>
        /// <param name="employeePk"></param>        
        /// <param name="date"></param>   
        /// <returns>DataTable</returns>     
        public static DataTable GetWorkingHour(int employeePk, string date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK, employeePk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE, date)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_WORK_HR_GET, colParameters).Tables[0];
            return dtResult;
        }


        public static double GetEmpWorkHour(int EmpPk, DateTime AttendanceDate)
        {
            double WorkHours = 0;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK ,  EmpPk),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DATE ,  AttendanceDate),              
              new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_RET_VAL , 0, 20,ParameterDirection.ReturnValue, DBService.ParameterType.Number) 
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.FNHRM_EMP_WORK_HR_GET, colParameters);
            WorkHours = Convert.ToDouble(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Payroll.Parameters.P_RET_VAL]).Value);
            return WorkHours;
        }

        /// <summary>
        /// Method to delete attendance
        /// </summary>
        /// <param name="attendancePk">Attendance PK</param>
        /// <param name="lastModDate">Modified Date</param>
        /// <returns></returns>
        public static int? DeleteAttendance(int? HeaderPk, int? DetailPk, DateTime? lastModDate)
        {            
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAT_PK,DetailPk.HasValue? DetailPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_PK,HeaderPk.HasValue? HeaderPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,lastModDate.HasValue?lastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetAttendanceList(FilterParameters objFilterParam, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE, objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_FROM_DATE, objFilterParam.FromDate.HasValue?objFilterParam.FromDate.Value:(object)DBNull.Value),        
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empBranch, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_PK, objFilterParam.TransactionNo.HasValue?objFilterParam.TransactionNo.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDepartment, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_ATT_MODE, objFilterParam.ProcessMode.HasValue?objFilterParam.ProcessMode.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EMP_NAME, objFilterParam.Employee.HasValue?objFilterParam.Employee.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, objFilterParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, objFilterParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, bizUnit)
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_GET_LIST, colParameters).Tables[0];
            return dtResult;
        }

        public static string GetAttendanceDetails(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_PK, (objFilterParam.PK.HasValue && objFilterParam.PK > 0)?objFilterParam.PK.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE, objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BIZUNIT, objFilterParam.BizUnit.HasValue?objFilterParam.BizUnit.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDesignation, objFilterParam.Designation.HasValue?objFilterParam.Designation.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empBranch, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDepartment, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empEmploymentType, objFilterParam.EmployeeType.HasValue?objFilterParam.EmployeeType.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCompany, objFilterParam.Company.HasValue?objFilterParam.Company.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCategory, objFilterParam.EmployeeCategory.HasValue?objFilterParam.EmployeeCategory.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK, objFilterParam.Employee.HasValue?objFilterParam.Employee.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_FROM_DATE, objFilterParam.FromDate.HasValue?objFilterParam.FromDate.Value:(object)DBNull.Value)        
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_DTL_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static string GetAttendance(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_PK, (objFilterParam.PK.HasValue && objFilterParam.PK > 0)?objFilterParam.PK.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE, objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BIZUNIT, objFilterParam.BizUnit.HasValue?objFilterParam.BizUnit.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDesignation, objFilterParam.Designation.HasValue?objFilterParam.Designation.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empBranch, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDepartment, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empEmploymentType, objFilterParam.EmployeeType.HasValue?objFilterParam.EmployeeType.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCompany, objFilterParam.Company.HasValue?objFilterParam.Company.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCategory, objFilterParam.EmployeeCategory.HasValue?objFilterParam.EmployeeCategory.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK, objFilterParam.Employee.HasValue?objFilterParam.Employee.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value)        
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        //Get Attendance History
        public static DataTable GetAttendanceDetails(int pk)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAL_EAT_PK,pk)
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_IN_OUT_GET_KV, colParameters).Tables[0];
            return dtList;
        }


        public static DataSet GetAttendanceReport(int currPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EAR_PK, currPK), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_IMPORT_OUTPUT_RPT, colParameters);
        }
        public static DataSet GetAttendanceReportDetails(int currPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EAR_PK, currPK), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_OUTPUT_RPT, colParameters);
        }


        //Addition/Deduction details
        public static DataSet GetAdditionDeductionReport(int currPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.HDR_PK, currPK), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_OTHER_ADD_DED_OUTPUT_RPT, colParameters);
        }



        //----------------Extra Days Start--------------
        public static int SaveExtraDays(string xmlDoc, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_EXTRA_DAYS_SAVE, colParameters);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);            
            return result;
        }

        public static DataTable GetExtraDaysList(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE, objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_SAL_MONTH_FROM, objFilterParam.FromDate.HasValue?objFilterParam.FromDate.Value:(object)DBNull.Value),        
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_SAL_MONTH_TO, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EXH_BRANCH, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT,  objFilterParam.BizUnit.HasValue?objFilterParam.BizUnit.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM, objFilterParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE, objFilterParam.PageSize)
                
            };

            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_EXTRA_DAYS_GET_LIST, colParameters).Tables[0];
            return dtResult;
        }

        public static string GetExtraDaysByPK(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EXH_PK ,  objFilterParam.PK.HasValue ? objFilterParam.PK : (object)DBNull.Value),
            //  new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE,  objFilterParam.Active.HasValue ? objFilterParam.Active : (object)DBNull.Value),
            //  new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value)           
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_EXTRA_DAYS_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteExtraDays(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EXH_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_EXTRA_DAYS_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Payroll.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int ImportExtraDays(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
               // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_EXTRA_DAYS_IMP_SAVE, colParameters);
           // TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        //-------------Extra Days End-------

        public static int SaveGeneralAttendance(AttendanceGen employeeAttendance, out string EmpNames, out string TrxNo)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(employeeAttendance);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_EMP, string.Empty, 5000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_GEN_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            EmpNames = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_EMP]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetGeneralAttendanceDetails(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_PK, (objFilterParam.PK.HasValue && objFilterParam.PK > 0)?objFilterParam.PK.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ACTIVE, objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BIZUNIT, objFilterParam.BizUnit.HasValue?objFilterParam.BizUnit.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDesignation, objFilterParam.Designation.HasValue?objFilterParam.Designation.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empBranch, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDepartment, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empEmploymentType, objFilterParam.EmployeeType.HasValue?objFilterParam.EmployeeType.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCompany, objFilterParam.Company.HasValue?objFilterParam.Company.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCategory, objFilterParam.EmployeeCategory.HasValue?objFilterParam.EmployeeCategory.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK, objFilterParam.Employee.HasValue?objFilterParam.Employee.Value:(object)DBNull.Value),
                //new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EAR_FROM_DATE, objFilterParam.FromDate.HasValue?objFilterParam.FromDate.Value:(object)DBNull.Value)        
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_GEN_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        ///  Import General Attendance 
        /// </summary>
        /// <param name="pXML"></param>
        /// <param name="dtOut"></param>
        /// <returns></returns>
        public static int GeneralAttendanceImport(string pXML, ref DataTable dtOut)
        {
            DBService dbService;
            DBService.Parameters[] colParameters;

            dbService = new DBService();
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dtOut = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_ATTENDANCE_GEN_IMPORT_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

    }
}
