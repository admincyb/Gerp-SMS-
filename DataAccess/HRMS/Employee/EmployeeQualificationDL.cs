using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeQualificationDL
    {
        // Save Employee  Qualifiaction
        public static int? SaveEmployeeQualifiaction(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SAVE_EMPLOYEE_QUALIFICATION, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        //Fill Qualification Type
        public static DataTable GetQualificationType(int conPK)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 12) ,  

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }


        public static DataTable GetQualificationStatus(string configType, int configPK = 0, int active = 1, int bizUnit = 1)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_PK,configPK),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_TYPE,configType),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT,bizUnit)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_CONFIG_MST_GET_KV, colParameters).Tables[0];

            return dtList;
        }


        //Get Employee Qualification By employee Id
        public static DataTable GetEmployeeQualificationList(int EmpPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EMPLOYEE, EmpPk) ,  
     
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_QUALIFICATION_LIST, colParameters);
        }

         //Get Employee Qualification By employee Id
        public static string GetEmployeeQualificationbyQualificationId(int qualfPk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EQD_PK, qualfPk) 
             
            };

            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_QUALIFICATION_LIST_BY_QUALIFICATIONID, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int DeleteQualificationbyQualId(int qualfPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EQD_PK, qualfPk),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.DELETE_EMPLOYEE_QUALIFICATION_QUALIFICATIONID, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ERP.Utilities.HRMS.Employee.P_RET_VAL]).Value);
        }

    }
}
