using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Payroll
{
    public class OtherAdditionDeductionDL
    {
        public static int? SaveAdditionDeductionDetails(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_OTHER_ADD_DED_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static DataTable GetAdditionDeductionList(FilterParameters gridParam, int bizUnit, int type, int payelmt)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
                new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_CLASS, type > 0 ? type : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_PAY_ELEMENT, payelmt > 0 ? payelmt : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_DATE_FROM, gridParam.FromDate.HasValue ? gridParam.FromDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_DATE_TO, gridParam.ToDate.HasValue ? gridParam.ToDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_OAH_DISP_NAME, string.IsNullOrEmpty(gridParam.Name) ? (object)DBNull.Value : gridParam.Name.Trim()),
                new DBService.Parameters(Parameters.P_EMP_PK1, (gridParam.Employee.HasValue && gridParam.Employee > 0) ? gridParam.Employee : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_OAH_NO, string.IsNullOrEmpty(gridParam.TrxNo) ? (object)DBNull.Value : gridParam.TrxNo.Trim()),
                new DBService.Parameters(Parameters.P_OAH_STATUS, (gridParam.Status.HasValue) ? gridParam.Status : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, (gridParam.UserPK.HasValue) ? gridParam.UserPK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_OAH_PK, (gridParam.PK.HasValue) ? gridParam.PK : (object)DBNull.Value)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_OTHER_ADD_DED_GET_LIST, colParameters);
        }

        public static string GetAdditionDeductionByPK(int bizUnit, int status, int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_OAH_PK, itemPK),
                //new DBService.Parameters(Parameters.P_EPW_PAYROLL_HDR, PayrollPK == 0 ? (object)DBNull.Value : PayrollPK)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_OTHER_ADD_DED_GET_KV, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteAdditionDeduction(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_OAH_PK , pk),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_LAST_MOD_DT , lastModifiedDate == string.Empty ? (object)DBNull.Value : lastModifiedDate),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_OTHER_ADD_DED_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }



        public static string GetAdditionDeduction(FilterParameters objFilterParams, decimal Amount, string Remarks)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_OAD_DATE, objFilterParams.Date.HasValue? objFilterParams.Date : (object)DBNull.Value), 
                new DBService.Parameters(Parameters.P_OAD_EMPLOYEE, objFilterParams.Employee.HasValue? objFilterParams.Employee : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_empEmploymentType, objFilterParams.EmploymentType.HasValue? objFilterParams.EmploymentType : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EMP_TYPE, objFilterParams.EmployeeType.HasValue? objFilterParams.EmployeeType : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_OAD_AMOUNT, Amount),
                new DBService.Parameters(Parameters.P_OAD_REMARKS, Remarks),
                new DBService.Parameters(Parameters.P_EmpBranch, objFilterParams.BranchLocation.HasValue? objFilterParams.BranchLocation : (object)DBNull.Value),  
                new DBService.Parameters(Parameters.P_empDepartment, objFilterParams.Department.HasValue? objFilterParams.Department : (object)DBNull.Value) ,
                new DBService.Parameters(Parameters.P_EPD_PAY_MODE, objFilterParams.PaymentMode.HasValue? objFilterParams.PaymentMode : (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_CURRENCY, objFilterParams.EmpCurrency.HasValue? objFilterParams.EmpCurrency : (object)DBNull.Value)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_OTHER_ADD_DED_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static DataTable GetAdditionDeductionNumbers(byte Active, int bizUnit, string searchValue)
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
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_OTHER_ADD_DED_AUTO, colParameters).Tables[0];
            return dtProcess;
        }

        public static int AdditionDeductionImport(string pXML, ref DataTable dtOut)
        {
            DBService dbService;
            DBService.Parameters[] colParameters;
            dbService = new DBService();
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dtOut = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_OTHER_ADD_DED_IMPORT_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
        }
    }
}
