using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
    public class SalaryTemplateDL
    {
        public static DataTable GetSalaryTemplateKv(int bizUnit, int pk = 0, int active = 1, int payrolltype = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_STE_PK  , pk), 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , active),    
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
              new DBService.Parameters(Parameters.P_STE_PAYROLL_TYPE, payrolltype == 0 ? (object)DBNull.Value : payrolltype),
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure
                , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SALARY_TEMP_GET_KV
                , colParameters);
            return dsSet.Tables[0];
        }

        /// <summary>
        /// Method to Save Salary Template
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveSalaryTemplate(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SALARY_TEMP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Salary Templates for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalaryTemplateList(BusinessObject.GridPrams gridParam, int bizUnit, int payrolltype, string tempalteCode = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.P_STE_NAME , gridParam.SearchValue == Parameters.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + gridParam.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),             
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
              new DBService.Parameters(Parameters.P_STE_PAYROLL_TYPE, payrolltype == 0 ? (object)DBNull.Value : payrolltype),
              new DBService.Parameters(Parameters.P_STE_CODE, !string.IsNullOrEmpty(tempalteCode) ? tempalteCode : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_SALARY_TEMP_GET_LIST, colParameters);
        }

        //Get Salary Template
        public static string GetSalaryTemplate(int pk,int empPk=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_STE_PK, pk==0?(object)DBNull.Value:pk), 
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_empPK, empPk==0?(object)DBNull.Value:empPk) 
            };

            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_SALARY_TEMP_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// Method to Delete StockTransfer Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteSalaryTemplate(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_STE_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_SALARY_TEMP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int UpdateSalaryTemplateStatus(int currPK, int status, int userPK, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_STE_PK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SALARY_TEMP_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataTable GetEmployeePayElement(int TemplateDetailPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_STS_PK  , TemplateDetailPk)
            };
            DataSet dsSet = new DataSet();
            dsSet = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SALARY_TEMP_DTL_EMP_GET, colParameters);
            return dsSet.Tables[0];
        }

        public static int? UpdateEmployeeSalaryTemplate(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_XML  , xmlstr),            
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SALARY_TEMP_DTL_UPDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }
    }
}
