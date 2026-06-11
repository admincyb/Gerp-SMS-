using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Payroll
{
    public class SalaryPaymentDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetSalaryPaymentList(FilterParameters gridParam, int? bankPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, gridParam.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, gridParam.BizUnit),
                new DBService.Parameters(Parameters.P_PSH_PAYMNT_MODE, gridParam.PaymentMode <= 0 ? null : gridParam.PaymentMode),
                new DBService.Parameters(Parameters.P_PSH_BANK, bankPk <=  0 ? null : bankPk),
                new DBService.Parameters(Parameters.P_FROM_DT, gridParam.FromDate ),
                new DBService.Parameters(Parameters.P_TO_DT, gridParam.ToDate ),
                new DBService.Parameters(Parameters.P_PSH_CHEQUE_NO, gridParam.Name == string.Empty ? null : gridParam.Name),
                 new DBService.Parameters(Parameters.P_PSH_STATUS, (gridParam.Status.HasValue) ? gridParam.Status : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, gridParam.Active),
                new DBService.Parameters(Parameters.P_PSH_PK, gridParam.PK ),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, gridParam.UserPK )
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_GET_LIST, colParameters);

        }
        public static string GetSalaryPaymentDetails(FilterParameters objFilterParams)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(Parameters.P_empCompany, objFilterParams.Company.HasValue? objFilterParams.Company : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_XacEmpType, objFilterParams.EmployeeType.HasValue? objFilterParams.EmployeeType : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_empBranch, objFilterParams.BranchLocation.HasValue? objFilterParams.BranchLocation : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_empEmploymentType, objFilterParams.EmploymentType.HasValue? objFilterParams.EmploymentType : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPD_PAY_BANK, objFilterParams.EmpBank.HasValue? objFilterParams.EmpBank : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPD_PAY_MODE,  objFilterParams.PaymentMode.HasValue? objFilterParams.PaymentMode : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_PRC_MODE,  objFilterParams.ProcessMode.HasValue? objFilterParams.ProcessMode : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_PAYROLL_TYPE,  objFilterParams.payrollType.HasValue? objFilterParams.payrollType : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT,  objFilterParams.BizUnit.HasValue? objFilterParams.BizUnit : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, objFilterParams.Active.HasValue? objFilterParams.Active : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_CURRENCY,  objFilterParams.EmpCurrency.HasValue? objFilterParams.EmpCurrency : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EPH_PAYROLL_MONTH,  objFilterParams.SalaryMonth),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_DTL_GET, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? SaveSalaryPaymentDetails(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            if (result > 0)
                System.Web.HttpRuntime.Cache.Insert(GTIService.Constants.Common.Parameters_Common.GetSmsStatus, true);
            return result;
        }

        public static string GetSalaryPaymentByPK(int bizUnit, int status, int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_PSH_PK, itemPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_GET_KV, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int DeleteSalaryPayment(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_PSH_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataSet GetSalaryPaymentRPT(int currPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_PSP_PK , currPk), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BANK_STMT_OUTPUT_RPT, colParameters);
        }
        public static DataSet GetEmpSalaryPaymentRPT(int currPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_PSP_PK , currPk), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BANK_STMT_RPT, colParameters);
        }

        public static DataTable GetDATDetails(int PymntModDetPk, out string srtResult)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {      
                new DBService.Parameters(Parameters.P_PSP_PK, PymntModDetPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_TEXT , string.Empty, 50000000,ParameterDirection.Output, DBService.ParameterType.NVarChar)     
            };
            DataTable dtResult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_MODE_DTL_DAT_GET, colParameters);
            srtResult = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_TEXT]).Value);
            return dtResult;

        }
        public static DataTable GetSalaryPaymentNumbers(byte Active, int bizUnit, string searchValue)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.SEARCHVALAUTO, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_SAL_PAYMENT_AUTO, colParameters).Tables[0];
            return dtProcess;
        }
    }
}
