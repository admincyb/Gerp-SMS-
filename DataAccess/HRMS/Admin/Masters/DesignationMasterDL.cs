using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
    public class DesignationMasterDL
    {
        public static DataSet GetDesignationList(int? currPK, int active, int pageNo, int pageSize, int bizUnit, int? jobCateg, int? jobGrd, string DesignationCode = null, string DesignationName = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgPK, currPK.HasValue?currPK:(object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_NO , pageNo),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  pageSize),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
              new DBService.Parameters(Parameters.P_SORT_BY, "dsgCode"),
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_dsgCode, DesignationCode!=null ? DesignationCode!=string.Empty?DesignationCode:(object)DBNull.Value:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_dsgName, DesignationName!=null ? DesignationName!=string.Empty?DesignationName:(object)DBNull.Value:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgjobCategory, jobCateg > 0 ? jobCateg:(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgjobGrade, jobGrd > 0 ? jobGrd:(object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EmpDesignation_GET_KV, colParameters);
        }

        public static DataTable GetSDesignationType(int? ltmPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgPK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EmpDesignation_GET_KV, colParameters).Tables[0];
        }


        /// <summary>
        /// Method to Save Designation
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveDesignationMaster(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EmpDesignation_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Delete StockTransfer Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteDesignationTypeMaster(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgPK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EmpDesignation_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int UpdateDesignationTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {        
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgPK  , currPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE  , status),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK  , userPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT  , lastModDate == string.Empty  ? (object)DBNull.Value : lastModDate),  
              new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_EmpDesignation_ACTIVATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Employee.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static string GetDocTypeList(int pk)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_dsgPK , pk)
            };

            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EmpDesignation_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
    }
}
