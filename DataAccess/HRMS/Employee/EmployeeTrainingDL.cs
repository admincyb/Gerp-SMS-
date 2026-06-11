using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Employee;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeTrainingDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetEmployeeTrainingList(string fromDate, string ToDate, string topic, int bizUnit, int status, int pageNo, int pageSize)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, pageNo), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_TRN_MONTH_FROM, fromDate  ==  string.Empty ? null : fromDate),
                new DBService.Parameters(Parameters.P_TRN_MONTH_TO, ToDate  ==  string.Empty  ? null : ToDate),
                new DBService.Parameters(Parameters.P_ETA_TOPIC, topic ==  string.Empty ? null : topic),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRAINING_GET_LIST, colParameters);

        }
        public static string GetEmployeeTrainingDetailsPopUp(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empCompany,objFilterParam.Company ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EmployeeType, objFilterParam.EmployeeType ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empBranch, objFilterParam.BranchLocation ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empEmploymentType, objFilterParam.EmploymentType),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empDepartment, objFilterParam.Department),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empPK, objFilterParam.Employee),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, objFilterParam.Active),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_TO_DATE, objFilterParam.ToDate)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRAINING_DTL_GET, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? SaveEmployeeTrainingDetails(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRAINING_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetEmployeeTrainingByPK(int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_ETA_PK, itemPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRAINING_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int DeleteEmployeeTraining(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_ETA_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRAINING_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataSet GetEmployeeTrainingReport(int currPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.HDR_PK , currPK),
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRAINING_OUTPUT_RPT, colParameters);
        }
    }
}
