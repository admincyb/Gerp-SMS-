using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
using System.Web;
namespace DataAccess.Administration.Masters
{
    public class SendSMSDL
    {
        public static DataSet GetSendSMSList(ERP.Utilities.HRMS.FilterParameters ObjFilterParameters)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
             new DBService.Parameters(Parameters.P_PAGE_NO , ObjFilterParameters.PageNumber),
             new DBService.Parameters(Parameters.P_PAGE_SIZE,  ObjFilterParameters.PageSize),
             new DBService.Parameters(Parameters.P_FROM_DATE,  ObjFilterParameters.FromDate.HasValue ? ObjFilterParameters.FromDate : (object)DBNull.Value),
             new DBService.Parameters(Parameters.P_TO_DATE,  ObjFilterParameters.ToDate.HasValue ? ObjFilterParameters.ToDate : (object)DBNull.Value),
             new DBService.Parameters(Parameters.P_SMQ_TO,  ObjFilterParameters.MobileNo),
             // new DBService.Parameters(Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_SMS_QUEUE_TRX_GET_LIST, colParameters);
        }

        public static DataTable GetEmployeeDetails(ERP.Utilities.HRMS.FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(Parameters.P_EMP_PK, objFilterParam.Employee.HasValue?objFilterParam.Employee.Value:(object)DBNull.Value),        
                new DBService.Parameters(Parameters.P_EMP_DEPARTMENT, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EMP_BRANCH, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),    
                new DBService.Parameters(Parameters.P_EMP_COMPANY, objFilterParam.Company.HasValue?objFilterParam.Company.Value:(object)DBNull.Value),   
                new DBService.Parameters(Parameters.P_EMP_ACTIVE, objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_SMS_QUEUE_TRX_EMP_GET, colParameters).Tables[0];
        }


        public static int? SaveSendSMSDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
             //   new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_SMS_QUEUE_TRX_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            if (result>0)
                HttpRuntime.Cache.Insert(GTIService.Constants.Common.Parameters_Common.GetSmsStatus, true);
            return result;
        }

        public static int? DeleteSMSDetails(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_SMS_QUEUE_TRX_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        
    }
}
