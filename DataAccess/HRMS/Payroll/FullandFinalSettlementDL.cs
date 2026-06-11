using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;

namespace DataAccess.HRMS.Payroll
{
    public class FullandFinalSettlementDL
    {
       

        public static string GetEmployeePayrollList(string strxml)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                //new DBService.Parameters(Parameters.P_EMP_PK, empPK == 0 ? (object)DBNull.Value : empPK) , 
                //new DBService.Parameters(Parameters.P_EMP_TYPE, employeeTypePK == 0 ? (object)DBNull.Value : employeeTypePK),
                //new DBService.Parameters(Parameters.P_EMP_BRANCH, empBranchPK == 0 ? (object)DBNull.Value : empBranchPK),
                //new DBService.Parameters(Parameters.P_EPH_PK, currPK == 0 ? (object)DBNull.Value : currPK),
                //new DBService.Parameters(Parameters.P_EMP_PAYROLL_TYPE, empPayrollTypePK == 0 ? (object)DBNull.Value : empPayrollTypePK),
                //new DBService.Parameters(Parameters.P_EPH_COMPANY, cmpPK == 0 ? (object)DBNull.Value : cmpPK),
                //new DBService.Parameters(Parameters.P_EMP_EMPLOYMENT_TYPE, empTypePK == 0 ? (object)DBNull.Value : empTypePK) 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_DTL_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? ProcessPayrollDetails(string strxml, out string TrxNo, out DataTable dtErrorList)
        {
            dtErrorList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_FINAL_SETTLEMENT_SAVE, colParameters);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtErrorList = dsResult.Tables[0];
            //int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static DataTable GetPayrollProcessPeriodList(ERP.Utilities.HRMS.FilterParameters gridParam, int bizUnit, int processMode)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
                new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_EPH_PAYROLL_TYPE, processMode),
                new DBService.Parameters(Parameters.P_EPH_MONTH_FROM, gridParam.FromDate.HasValue? gridParam.FromDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_MONTH_TO, gridParam.ToDate.HasValue? gridParam.ToDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_STATUS, gridParam.Status.HasValue? gridParam.Status : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_PK ,  gridParam.PK.HasValue? gridParam.PK : (object)DBNull.Value)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_FINAL_SETTLEMNT_GET_LIST, colParameters);
        }

        public static string GetEmployeePayrollDetailList(int epsPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_EPS_PK, epsPK == 0 ? (object)DBNull.Value : epsPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_PAY_DTL_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? SaveEmployeePayroll(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_PAY_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetPayrollType(int PK, int bizUnit, int status, int processMode = 0, int? UserPK = null )
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_PTM_PK, PK),
                new DBService.Parameters(Parameters.P_PTM_PRC_MODE, processMode == 0 ? (object)DBNull.Value : processMode),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, (UserPK.HasValue && UserPK > 0)? UserPK : (object)DBNull.Value)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_PAYROLL_TYPE_GET_KV, colParameters);
        }

        public static string GetWorkingDaysDetails(int PayrollPK, int bizUnit, int status, int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_EPW_PK, itemPK),
                new DBService.Parameters(Parameters.P_EPW_PAYROLL_HDR, PayrollPK == 0 ? (object)DBNull.Value : PayrollPK)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_WD_DTL_GET_KV, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteProcessPeriod(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_EPH_PK , pk),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_LAST_MOD_DT , lastModifiedDate == string.Empty ? (object)DBNull.Value : lastModifiedDate),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int DeleteEmployeePayroll(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xml),                
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_DTL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataSet GetPayslipRPT(int currPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EPS_PK, currPk) , 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SALARY_SLIP_OUTPUT_RPT, colParameters);
        }

        public static DataSet GetSalaryStatementRPT(int currPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.EPH_PK, currPk) , 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_STMNT_OUTPUT_RPT, colParameters);
        }
        public static DataSet GetPaySlipMultipleReport(string xmlPaySlip ,string payslipSp)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlPaySlip), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, payslipSp, colParameters);
        }

        public static DataSet GetPayrollPreprocess(string xmlPreprocess)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlPreprocess), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_SAL_PRE_PROCESS_GET, colParameters);
        }

        public static DataSet GetPayrollSalaryOutRPT(int currPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_EPH_PK, currPk) , 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_OUTPUT_RPT, colParameters);
        }

        public static DataTable GetPayroll(int? PayrollPk, int bizUnit)
        {            
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_EPH_PK, (PayrollPk.HasValue && PayrollPk > 0) ? PayrollPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_PAYROLL_GET_KV, colParameters).Tables[0];
        }
        public static DataTable GetNonPayrollEmployees(int payrollType, string fromDate, string toDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_PTM_PK, (payrollType > 0) ? payrollType : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_FROM_DATE, (fromDate == string.Empty ? (object)DBNull.Value : fromDate)),
                new DBService.Parameters(Parameters.P_TO_DATE, (toDate == string.Empty ? (object)DBNull.Value : toDate)),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_PAYROLL_PEND_EMP_GET, colParameters).Tables[0];
        }
    }
}

