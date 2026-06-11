using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities;
using GTIService.Constants.HRMS.Payroll;

namespace DataAccess.HRMS.Payroll
{
    public class AppraisalDetailsDL
    {
        /// <summary>
        /// for get all list
        /// </summary>
        /// <param name="gridParam"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetList(BusinessObject.GridPrams gridParam, int bsu, string tranName = null, int? type = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, gridParam.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, gridParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_FROM_DT,gridParam.FromDate!=null?gridParam.FromDate != string.Empty ?Convert.ToDateTime(gridParam.FromDate): (object)DBNull.Value :  (object)DBNull.Value) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_TO_DT,gridParam.ToDate!=null?gridParam.ToDate != string.Empty ?Convert.ToDateTime(gridParam.ToDate): (object)DBNull.Value :  (object)DBNull.Value) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_TRN_NAME, tranName !=null?tranName!=string.Empty?tranName:(object)DBNull.Value:(object)DBNull.Value) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_EIH_TYPE, type!=null?type>-1?type:(object)DBNull.Value:(object)DBNull.Value) 
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_GET_LIST, colParameters);
        }
        /// <summary>
        /// for get individual record
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <returns></returns>
        public static string GetAppraisalEdit(string CurrPK)
        {
            string strRetVal = string.Empty;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(Parameters.P_EIH_PK, CurrPK) 
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// save appraisal details
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static int? SaveAppraisal(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                    
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// delete appraisal details
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int? DeleteAppraisal(int CurrPK, DateTime dateTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                    
                new DBService.Parameters(Parameters.P_EIH_PK, CurrPK),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, dateTime),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
