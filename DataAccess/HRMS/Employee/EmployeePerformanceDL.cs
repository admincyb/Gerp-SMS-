using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.HRMS.Employee
{
    public class EmployeePerformanceDL
    {

        public static DataTable GetAction(int? GROUP_TYPE_VALUE, int BIZUNIT, int ACTIVE, int? GROUP_Pk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, GROUP_TYPE_VALUE==0?null:GROUP_TYPE_VALUE) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CNG_PK, GROUP_Pk<=0?null:GROUP_Pk) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_BIZUNIT, BIZUNIT) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, ACTIVE) ,        
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_CONST_MST_GET_KV, colParameters);
        }

        public static DataTable GetCategory(int? CNG_GRP_TYPE, int BIZUNIT, int ACTIVE, string CNG_SPL_COND)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CNG_GRP_TYPE, CNG_GRP_TYPE==0?null:CNG_GRP_TYPE) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CNG_BIZUNIT,BIZUNIT) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, ACTIVE) ,                  
                 new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CNG_SPL_COND, CNG_SPL_COND==string.Empty?null:CNG_SPL_COND) ,  
    
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPADM_CONST_GRP_GET_KV, colParameters);
        }

        public static DataTable GetEmployeePerformanceList(int EmpPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EMPLOYEE, EmpPk) ,  
     
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PERFORMANCE_DTL_GET_LIST, colParameters);
        }


        public static int? SaveEmployeePerformance(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PERFORMANCE_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        public static string GetEmployeePerformanceByPk(int pk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EPD_PK, pk) 
             
            };

            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PERFORMANCE_DTL_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }



        public static int DeleteEmployeePerformanceById(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EPD_PK, pk),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.Configurations.Company.Parameters.PRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.SPHRM_EMP_PERFORMANCE_DTL_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ERP.Utilities.HRMS.Employee.P_RET_VAL]).Value);
        }


    }
}
