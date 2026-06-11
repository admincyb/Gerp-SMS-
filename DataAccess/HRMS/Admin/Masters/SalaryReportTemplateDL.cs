using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Employee;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Admin.Masters
{
    public class SalaryReportTemplateDL
    {
        /// <summary>
        /// Method to Save Salary Report Template
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveSalaryReportTemplateDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_XML , strxml),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SAL_RPT_TMP_HDR_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
            //return 0;
        }

        /// <summary>
        /// Pay element Dropdown filling
        /// </summary>
        /// <param name="ActiveStatus"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetPayElementDetails(int? currPK, int ActiveStatus, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataTable dtParams = new DataTable();
            colParameters = new DBService.Parameters[] 
            {        
                 new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PEL_PK, currPK.HasValue?currPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, ActiveStatus),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit == 0 ? (object) DBNull.Value : bizUnit)  
            };
            dtParams = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_PAY_ELEMENT_GET_KV, colParameters).Tables[0];
            return dtParams;
        }

        /// <summary>
        /// Method to Get Bonus Type List
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalaryReportTemplateList(int? currPK, int active, int pageNo, int pageSize, int bizUnit, string TemplateName = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_SRT_PK, currPK.HasValue?currPK:(object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ACTIVE, active),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_NUM , pageNo),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_PAGE_SIZE,  pageSize),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BIZUNIT, bizUnit),
              //new DBService.Parameters(Parameters.P_SORT_BY, "BON_CODE"),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_SRT_NAME, TemplateName!=null ? TemplateName!=string.Empty?TemplateName:(object)DBNull.Value:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SAL_RPT_TMP_HDR_GET_KV, colParameters);
            //return null;
        }

        /// <summary>
        /// Get Salary Report Template details -Edit
        /// </summary>
        /// <param name="currPK"></param>
        /// <returns></returns>
        public static string GetSalaryReportTemplateDetails(int? currPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_SRT_PK, currPK.HasValue?currPK:(object)DBNull.Value),
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SAL_RPT_TMP_HDR_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Delete Salary Report Template List
        /// </summary>
        /// <param name="pk"></param>
        /// <param name="lastModifiedDate"></param>
        /// <returns></returns>
        public static int DeleteSalaryTemplateMaster(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_SRT_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_SAL_RPT_TMP_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
            //return 0;
        }
    }
}
