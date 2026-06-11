using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;
using System.Data;
namespace DataAccess.HRMS.Payroll
{
   public class BulkAppraisalDL
    {
       public static DataSet GetEmployeeAppraisalList(FilterParameters objFilterParam)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  objFilterParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  objFilterParam.PageSize),
              new DBService.Parameters(Parameters.P_FROM_DT,  objFilterParam.FromDate.HasValue ? objFilterParam.FromDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_TO_DT,  objFilterParam.ToDate.HasValue ? objFilterParam.ToDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EBH_TYPE,  objFilterParam.Type.HasValue ? objFilterParam.Type : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EBH_NO,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EBH_STATUS, (objFilterParam.Status.HasValue) ? objFilterParam.Status : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, (objFilterParam.UserPK.HasValue) ? objFilterParam.UserPK : (object)DBNull.Value),
            };
           return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BULK_APPRAISAL_GET_LIST, colParameters);
       }

       public static string GetEmployeeAppraisalDetails(FilterParameters objFilterParam)
       {
           string strRetVal = "";
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(Parameters.P_BED_EMP_DESGN,  objFilterParam.Designation.HasValue ? objFilterParam.Designation : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BED_EMP_DEPT, (objFilterParam.Department.HasValue) ? objFilterParam.Department : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_LAST_APPR_DATE, objFilterParam.EmpAppDate ),
              new DBService.Parameters(Parameters.P_EMP_DOJ,  objFilterParam.EmpDoj),
              new DBService.Parameters(Parameters.P_TO_DATE,  objFilterParam.Date),
             // new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, (objFilterParam.UserPK.HasValue) ? objFilterParam.UserPK : (object)DBNull.Value),
            };

           DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BULK_APPRAISAL_EMP_GET, colParameters).Tables[0];
           foreach (DataRow dr in dtxml.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }

       public static int? SaveBulkAppraisalDetails(string strxml, out string TrxNo, out DataTable dtErrorList)
       {
           dtErrorList = new DataTable();
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
           DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BULK_APPRAISAL_WKF_SAVE, colParameters);
           if (dsResult != null && dsResult.Tables.Count > 0)
               dtErrorList = dsResult.Tables[0];
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
           int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
           return result;
       }


       public static string GetSalaryAppraisalByPk(int curPk)
       {
           string strRetVal = "";
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_EBH_PK, curPk),
            };

           DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_BULK_APPRAISAL_GET_XML, colParameters).Tables[0];
           foreach (DataRow dr in dtxml.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }

       public static int DeleteSalaryAppraisal(int CurrPK, DateTime LastModifiedTime)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_EBH_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
           dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_BULK_APPRAISAL_DELETE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
           return result;
       }
    }
}
