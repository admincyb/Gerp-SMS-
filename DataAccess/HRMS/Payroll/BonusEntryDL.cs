using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Payroll
{
    public class BonusEntryDL
    {

        public static DataTable GetEmployeeBonusList(string fromDate, string ToDate, int? bonusType, int bizUnit, int status, int pageNo, int pageSize)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, pageNo), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, pageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_FROM_DT, fromDate  ==  string.Empty ? null : fromDate),
                new DBService.Parameters(Parameters.P_TO_DT, ToDate  ==  string.Empty  ? null : ToDate),
                new DBService.Parameters(Parameters.P_BONUS_TYPE, bonusType > 0 ? bonusType : null),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status)
            };
            DataTable dtres = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_BONUS_GET_LIST, colParameters);
            return dtres;
        }

        public static int? SaveBonusEntryDetails(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_BONUS_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static DataTable GetBonusType(int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_BON_PK, bizunit),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active)
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_BONUS_TYPE_GET_KV, colParameters);
            return dtresult;
        }

        public static DataTable GetBonusType(int currpk, int bizunit, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_BON_PK, currpk),
                 new DBService.Parameters(Parameters.P_BIZUNIT, bizunit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, active)
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_BONUS_TYPE_GET_KV, colParameters);
            return dtresult;
        }

        public static DataTable GetEmployeeBonusDetailsPopUp(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empReligion,objFilterParam.Religion ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_empSubReligion, objFilterParam.SubReligion ),
               // new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EmpState1, objFilterParam.State ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BON_PK, objFilterParam.BonusType ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BOH_DATE, objFilterParam.Date ),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_BOH_CURRENCY, objFilterParam.Currency ),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_DOJ_BEFORE, objFilterParam.EmpDoj),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_EmpStatePK1, objFilterParam.StatePk),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_TO_DATE, objFilterParam.ToDate)
            };
            DataTable dtEmployee = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_BONUS_EMP_GET, colParameters);
            return dtEmployee;
        }

        public static string GetEmployeeDetailsByPK(int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_BOH_PK, itemPK)
                
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_BONUS_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteEmployeeBonus(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_BOH_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_BONUS_DELETE, colParameters);
            //Procedures.
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

    }
}
