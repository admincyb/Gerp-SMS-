using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Payroll
{
    public class HRYearCloseDL 
    {
        public static DataSet GetLeaveList(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  objFilterParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  objFilterParam.PageSize),
              new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BIZUNIT, objFilterParam.BizUnit),
              new DBService.Parameters(Parameters.P_CUR_DATE,  objFilterParam.FromDate.HasValue ? objFilterParam.FromDate : (object)DBNull.Value),
             // new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  objFilterParam.UserPK.HasValue ? objFilterParam.UserPK : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_YEAR_MST_GET_LIST, colParameters);
        }

        public static DataTable GetLeaveType(int? ltmPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTM_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_BIZUNIT, bizUnit),
               // new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LTM_CARRY_FWD, carryFWD),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Admin.Masters.Procedures.SPHRM_LEAVE_TYPE_MST_GET_KV, colParameters).Tables[0];
        }

        public static DataTable GetSalaryDetails(int active, int bizUnit, int? curPk=null,string curDate=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_HYR_PK, curPk!=null ? curPk:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                 new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                  new DBService.Parameters(Parameters.P_CUR_DATE, curDate!=null ? curDate:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_YEAR_MST_GET_KV, colParameters).Tables[0];
        }

        public static int SaveHRYearClause(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_YEAR_END_DATA, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
