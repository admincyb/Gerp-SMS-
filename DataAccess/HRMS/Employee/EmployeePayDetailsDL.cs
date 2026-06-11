using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities;
using BusinessObject.HRMS.Employee;

namespace DataAccess.HRMS.Employee
{
    public class EmployeePayDetailsDL
    {
        public static EmployeePayDetailsBO GetEmployeePayDetailsByID(int empPk, int active)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_EMPLOYEE,empPk)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PAY_DTL_GET_XML, colParameters).Tables[0];

            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }

            EmployeePayDetailsBO empPayDetails = null;
            if (!objXml.IsNullOrEmptyOrWhitespace())
                empPayDetails = CommonFunctions.XmlDeserialize<EmployeePayDetailsBO>(objXml);

            return empPayDetails;
        }
        public static int Save(EmployeePayDetailsBO payDetails,out DateTime EmpLastModDate)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(payDetails);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_XML,xmlDoc),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PAY_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            EmpLastModDate = Convert.ToDateTime(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }
        public static int DeletePayDetails(int payDetailsID, DateTime lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_PK,payDetailsID),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_LAST_MOD_DT , lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PAY_DTL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }



        public static DataTable GetPayrollTypeUserMapping(int? CurrUserPk, int EmployeePk, out int EmpPayrollType)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            EmpPayrollType = 0;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_USER_PK,CurrUserPk.HasValue? CurrUserPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK,EmployeePk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empPayRollType , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number) 
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_PAYROLL_TYPE_USER_MAP_GET, colParameters).Tables[0];
            EmpPayrollType = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_empPayRollType]).Value);
            return dtList;
        }
    }
}
