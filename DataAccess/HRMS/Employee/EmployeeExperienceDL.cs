using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeExperienceDL
    {
        // Save Employee Experience
        public static int? SaveEmployeeExperience(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SAVE_EMPLOYEE_EXPERIENCE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        //Fill Exit reason
        public static DataTable GetExitReason(int conPK, string SearchKey)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, conPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 13) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_TEXT_VALUE , SearchKey +"%"),

                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }
        //Get Employee Experience By employee Id
        public static DataTable GetEmployeeExperienceList(int EmpPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EMPLOYEE, EmpPk) ,  
     
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_EXPERIENCE_LIST, colParameters);
        }

        //Get Employee Experience By Experience Id
        public static string GetEmployeeExperiencebyExperienceId(int expPk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EED_PK, expPk) 
             
            };

            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_EMPLOYEE_EXPERIENCE_LIST_EXPERIENCE_ID, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        //Delete Experience by Exdperience Id
        public static int DeleteExperiencebyExpId(int expPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EED_PK, expPk),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.DELETE_EMPLOYEE_EXPERIENNCE_BYEXPID, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ERP.Utilities.HRMS.Employee.P_RET_VAL]).Value);
        }

        //Get Currency

        public static DataTable GetCurrency(int Bizunit)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CURRENCY_PK, 0) ,
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CURRENCY_ACT, 1) ,  
               new DBService.Parameters( ERP.Utilities.HRMS.Employee.P_BIZUNIT , Bizunit==0?(object)DBNull.Value  : Bizunit), 
  
                
                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_CURRENCY, colParameters);
        }
        //Base Currency
        public static DataTable GetBaseCurrency()
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
               
               new DBService.Parameters( ERP.Utilities.HRMS.Employee.P_ACF_SETTING , "BASE CURRENCY"), 
           
                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_BASE_CURRENCY, colParameters);
        }
    }
}
