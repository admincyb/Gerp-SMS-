using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;


namespace DataAccess.HRMS.Payroll
{
    public class OvertimeCalculatorDL
    {
        //Get Employee Monthly OT Master List
        public static string GetMonthlyOtList(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(Parameters.P_empCompany, objFilterParam.Company.HasValue?objFilterParam.Company :(object)DBNull.Value) ,
               new DBService.Parameters(Parameters.P_empBranch, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation:(object)DBNull.Value),
               new DBService.Parameters(Parameters.P_empDepartment, objFilterParam.Department.HasValue?objFilterParam.Department:(object)DBNull.Value) ,
               new DBService.Parameters(Parameters.P_empEmploymentType, objFilterParam.EmploymentType.HasValue?objFilterParam.EmploymentType:(object)DBNull.Value),
               new DBService.Parameters(Parameters.P_empDesignation, objFilterParam.Designation.HasValue?objFilterParam.Designation:(object)DBNull.Value) ,
               new DBService.Parameters(Parameters.P_empPK, objFilterParam.Employee.HasValue?(object)objFilterParam.Employee:DBNull.Value) ,
               new DBService.Parameters(Parameters.P_empCategory, objFilterParam.EmployeeCategory.HasValue?objFilterParam.EmployeeCategory.Value:(object)DBNull.Value),
               new DBService.Parameters(Parameters.P_BIZUNIT, objFilterParam.BizUnit==0?(object)DBNull.Value:objFilterParam.BizUnit),
               new DBService.Parameters(Parameters.P_EOE_PK, objFilterParam.PK==0?(object)DBNull.Value:objFilterParam.PK),
               new DBService.Parameters(Parameters.P_EPD_EMP_TYPE, objFilterParam.EmployeeType.HasValue?objFilterParam.EmployeeType:(object)DBNull.Value),
               new DBService.Parameters(Parameters.P_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate:(object)DBNull.Value)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OT_DTL_GET, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }


        /// <summary>
        /// Method to Save Employee Monthly OT Master
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveMonthlyOtMaster(string xmlstr, out string EmpNames, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_EMP, string.Empty, 5000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OT_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            EmpNames = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_EMP]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }
        /// <summary>
        /// Method to Save Other Leave Entry
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveOtherLeaveEntry(string xmlstr, out string EmpNames, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_EMP, string.Empty, 5000,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OTHER_LEAVE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            EmpNames = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_EMP]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Yearly Ot List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetYearlyOtListListingPage(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, objFilterParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, objFilterParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE,objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),

                new DBService.Parameters(Parameters.P_EOE_FROM_DATE, objFilterParam.FromDate.HasValue?objFilterParam.FromDate.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOE_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOE_BRANCH, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOE_EMPDEPARTMENT, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOE_PK, (objFilterParam.PK.HasValue) ? objFilterParam.PK : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OT_DTL_GET_LIST, colParameters);
        }

        /// <summary>
        /// Method to Get Yearly Other leave entry List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetYearlyOtherLeaveEntryListingPage(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, objFilterParam.PageNumber),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, objFilterParam.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objFilterParam.BizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE,objFilterParam.Active.HasValue?objFilterParam.Active.Value:(object)DBNull.Value),

                new DBService.Parameters(Parameters.P_EOL_FROM_DATE, objFilterParam.FromDate.HasValue?objFilterParam.FromDate.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOL_TO_DATE, objFilterParam.ToDate.HasValue?objFilterParam.ToDate.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOL_BRANCH, objFilterParam.BranchLocation.HasValue?objFilterParam.BranchLocation.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOL_EMPDEPARTMENT, objFilterParam.Department.HasValue?objFilterParam.Department.Value:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOL_PK, (objFilterParam.PK.HasValue) ? objFilterParam.PK : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOL_EMP_PK,objFilterParam.Employee<=0?null: objFilterParam.Employee)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OTHER_LEAVE_HDR_GET_LIST, colParameters);
        }
        public static string GetOTDetailsByPK(int bizUnit, int status, int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_EOE_PK, itemPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OT_DTL_GET_XML, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        public static string OTHERLEAVEENTRYDETAILSBYPK(int bizUnit, int status, int EOLPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_EOL_PK, EOLPK),
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OTHER_DTL_GET, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        /// <summary>
        /// Method to delete OT Details
        /// </summary>
        /// <param name="HeaderPk">EOT PK </param>
        /// <param name="DetailPk">EOE PK </param>
        /// <param name="lastModDate">Modified Date</param>
        /// <returns></returns>
        public static int? DeleteOTDetails(int? HeaderPk, int? DetailPk, DateTime? lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_EOT_PK,DetailPk.HasValue? DetailPk : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOE_PK,HeaderPk.HasValue? HeaderPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,lastModDate.HasValue?lastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OT_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Method to delete Other Leave Entry Details
        /// </summary>
        /// <param name="HeaderPk">EOL PK </param>
        /// <param name="DetailPk">EOD PK </param>
        /// <param name="lastModDate">Modified Date</param>
        /// <returns></returns>
        public static int? DeleteOtherLeaveEntryDetails(int? HeaderPk, int? DetailPk, DateTime? lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_EOD_PK,DetailPk.HasValue? DetailPk : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EOL_PK,HeaderPk.HasValue? HeaderPk : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,lastModDate.HasValue?lastModDate : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OTHER_LEAVE_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataTable GetOvertimeDetailsNumbers(byte Active, int bizUnit, string searchValue)
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
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OT_HDR_AUTO, colParameters).Tables[0];
            return dtProcess;
        }
        public static DataTable GetOtherLeaveEntryNumbers(byte Active, int bizUnit, string searchValue)
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
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OTHER_LEAVE_HDR_AUTO, colParameters).Tables[0];
            return dtProcess;
        }
    }
}
