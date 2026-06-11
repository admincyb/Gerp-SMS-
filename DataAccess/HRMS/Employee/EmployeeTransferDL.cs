using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Employee;
using ERP.Utilities.HRMS;

namespace DataAccess.HRMS.Employee
{
    public class EmployeeTransferDL
    {
        public static int? SaveEmployeeTransfer(string xmlDoc, out string trxNo, out DataTable dtErrorList)
        {
            dtErrorList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRANSFER_WKF_SAVE, colParameters);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtErrorList = dsResult.Tables[0];
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            trxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static DataTable GetTransferReason(int ConPK, int Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_CON_PK, ConPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 27) , 
                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }

        public static DataTable GetEmpTransferList(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, objFilterParam.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, objFilterParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK, objFilterParam.UserPK), 
                new DBService.Parameters(Parameters.P_FROM_DATE, objFilterParam.FromDate.HasValue? objFilterParam.FromDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_TO_DATE, objFilterParam.ToDate.HasValue? objFilterParam.ToDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EFH_FROM, (objFilterParam.BranchLocation.HasValue && objFilterParam.BranchLocation > 0)? objFilterParam.BranchLocation : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EFH_TO, (objFilterParam.ToBranchLocation.HasValue && objFilterParam.ToBranchLocation > 0)? objFilterParam.ToBranchLocation : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EFH_PK, (objFilterParam.PK.HasValue && objFilterParam.PK > 0)? objFilterParam.PK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EFH_STATUS, (objFilterParam.Status.HasValue)? objFilterParam.Status : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EMP_PK, (objFilterParam.Employee.HasValue && objFilterParam.Employee > 0) ? objFilterParam.Employee : (object)DBNull.Value),
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRANSFER_GET_LIST, colParameters);
        }

        public static string GetEmpTransferDetails(int CurrPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                   
                new DBService.Parameters(Parameters.P_EFH_PK, CurrPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRANSFER_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? DeleteEmpTransfer(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_EFH_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRANSFER_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataTable GetEmpTransferNumbers(byte Active, int bizUnit, string searchValue)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.SEARCHVALAUTO, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRANSFER_AUTO, colParameters).Tables[0];
            return dtProcess;
        }

        public static DataSet GetEmployeeTransferReport(int currPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_EFHPK , currPK),
            };

            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_TRANSFER_OUTPUT_RPT, colParameters);
        }
    }
}
