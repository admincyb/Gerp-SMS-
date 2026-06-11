using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Payroll
{
  public  class EmployeeAppraisalDetailsDL
    {

      public static DataSet GetEmployeeAppraisalDetails(FilterParameters objFilterParam)
      {
          DBService dbService = new DBService();
          DBService.Parameters[] colParameters = null;
          colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  objFilterParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  objFilterParam.PageSize),
              new DBService.Parameters(Parameters.P_FROM_DT,  objFilterParam.FromDate.HasValue ? objFilterParam.FromDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_TO_DT,  objFilterParam.ToDate.HasValue ? objFilterParam.ToDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EIH_TYPE,  objFilterParam.Type.HasValue ? objFilterParam.Type : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EIH_EMPLOYEE,  objFilterParam.Employee.HasValue ? objFilterParam.Employee : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EIH_STATUS, (objFilterParam.Status.HasValue) ? objFilterParam.Status : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, (objFilterParam.UserPK.HasValue) ? objFilterParam.UserPK : (object)DBNull.Value),
            };
          return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_NEW_GET_LIST, colParameters);
      }

      public static string GetEmployeeAppraisalByPk(int curPk, int CurrEmployeePK)
      {
          string strRetVal = "";
          DBService dbService = new DBService();
          DBService.Parameters[] colParameters = null;
          colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_EIH_PK, curPk),
                new DBService.Parameters(Parameters.P_EIH_EMPLOYEE, CurrEmployeePK),
            };

          DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_NEW_GET_XML, colParameters).Tables[0];
          foreach (DataRow dr in dtxml.Rows)
          {
              strRetVal += dr[0].ToString();
          }
          return strRetVal;
      }

      public static int? SaveEmployeeAppraisalDetails(string strxml, out string TrxNo)
      {
          DBService dbService = new DBService();
          DBService.Parameters[] colParameters = null;
          colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
          int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPSPHRM_EMP_APPRAISAL_NEW_WKF_SAVE, colParameters);
          int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
          TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
          int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
          return result;
      }

      public static int DeleteEmployeeAppraisal(int CurrPK, DateTime LastModifiedTime)
      {
          DBService dbService = new DBService();
          DBService.Parameters[] colParameters = null;
          colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_EIH_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
          dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_APPRAISAL_NEW_DELETE, colParameters);
          int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
          return result;
      }
    }
}
