using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Utilities.HRMS;
using System.Data;
using GTIService.Constants.HRMS.ESS;

namespace DataAccess.HRMS
{
    public class EmpLeaveRequestDL
    {
        #region AutoComplete
        public static DataTable GetAutoCompleteEmployeeESS(string searchKey = "", int active = 1, int? empPk = null, string toDate = null)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPNAME,searchKey),
                //new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_Active,active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK,(empPk.HasValue && empPk > 0) ? empPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_TO_DATE, (toDate!=string.Empty) ? toDate : (object)DBNull.Value)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.ESS.Procedures.SPHRM_ESS_LEAVE_REQUEST_EMP_GET, colParameters).Tables[0];
            return dtList;
        }
        #endregion

        public static DataSet GetESSLeaveEntryList(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  objFilterParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  objFilterParam.PageSize),
              new DBService.Parameters(Parameters.P_ESL_FROM_DT,  objFilterParam.FromDate.HasValue ? objFilterParam.FromDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ESL_TO_DT,  objFilterParam.ToDate.HasValue ? objFilterParam.ToDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ESL_STATUS, (objFilterParam.Status.HasValue) ? objFilterParam.Status : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  objFilterParam.UserPK.HasValue ? objFilterParam.UserPK : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ESL_EMPLOYEE,  objFilterParam.Employee > 0 ? objFilterParam.Employee : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ESL_DATE,  objFilterParam.Date.HasValue ? objFilterParam.Date : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_HOL_SKIP, (objFilterParam.LvHolidaySkip.HasValue) ? objFilterParam.LvHolidaySkip : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_OFFDAY_SKIP,  objFilterParam.LvOffDaySkip.HasValue ? objFilterParam.LvOffDaySkip : (object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_ESS_LEAVE_REQUEST_GET_LIST, colParameters);
        }


        public static int SaveEmployeeLeaveESS(string xmlstr, out string RetNo, out string empResignDate) //, out string empResignDate
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)  , 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_DT, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_ESS_LEAVE_REQUEST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            RetNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            empResignDate = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_DT]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            return result;
        }


        public static string GetLeaveDetails(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
             new DBService.Parameters(Parameters.P_ESL_PK ,  objFilterParam.PK.HasValue ? objFilterParam.PK : (object)DBNull.Value),
             // new DBService.Parameters(Parameters.P_ACTIVE,  objFilterParam.Active.HasValue ? objFilterParam.Active : (object)DBNull.Value),
            //  new DBService.Parameters(Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value)           
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_ESS_LEAVE_REQUEST_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteESSLeave(int CurrPK, DateTime LastModifiedTime, int lvExist = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_ESL_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
               // new DBService.Parameters(Parameters.P_LV_EXIST , lvExist)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_ESS_LEAVE_REQUEST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static double GetNoOfLeaves(FilterParameters objFilterParameters)
        {
            double BalanceLeave = 0;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
              new DBService.Parameters(Parameters.V_ESL_EMPLOYEE ,  objFilterParameters.Employee),
              new DBService.Parameters(Parameters.V_ESL_LV_FROM_DT,  objFilterParameters.FromDate),
              new DBService.Parameters(Parameters.V_ESL_LV_FROM_HALF ,  objFilterParameters.LvFromHalf),
              new DBService.Parameters(Parameters.V_ESL_LV_TO_DT,  objFilterParameters.ToDate),
              new DBService.Parameters(Parameters.V_ESL_LV_TO_HALF,  objFilterParameters.LvToHalf),
              new DBService.Parameters(Parameters.V_HOL_SKIP, objFilterParameters.LvHolidaySkip),
              new DBService.Parameters(Parameters.V_OFFDAY_SKIP, objFilterParameters.LvOffDaySkip),
              new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.ReturnValue, DBService.ParameterType.Number) 
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.FNHRM_EMP_NO_OF_LEAVE_GET, colParameters);
            BalanceLeave = Convert.ToDouble(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return BalanceLeave;
        }
    }
}
