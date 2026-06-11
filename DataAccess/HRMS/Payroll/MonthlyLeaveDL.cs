using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;
using BusinessObject.HRMS.Payroll;

namespace DataAccess.HRMS.Payroll
{
    public class MonthlyLeaveDL
    {
        //Get Employee Monthly Leave Master List
        public static string GetEmployeeMonthlyLeaveList(int employeeID, DateTime? date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ELD_EMPLOYEE, employeeID==0?(object)DBNull.Value:employeeID) ,                  
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_ELD_MONTH, date.HasValue? date : (object)DBNull.Value)
            };
            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_EMP_EMP_LEAVE_DTL_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }


        /// <summary>
        /// Method to Save Employee Monthly LeaveMaster
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveEmployeeMonthlyLeaveMaster(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Yearly Leave List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetYearlyLeaveListListingPage(BusinessObject.GridPrams gridParam, int salaryYearPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == Fields.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + grid.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),
              //new DBService.Parameters(Parameters.P_FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),              
              //new DBService.Parameters(Parameters.P_SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.DirectStockTransfer.Fields.GRH_DATE|| grid.SortBy == GTIService.Constants.DirectStockTransfer.Fields.GRH_NO ?GTIService.Constants.DirectStockTransfer.Fields.GRH_PK : grid.SortBy),
              //new DBService.Parameters(Parameters.P_SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              //new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              //new DBService.Parameters(Parameters.P_USER_PK ,  objUser.PKUser),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
              new DBService.Parameters(Parameters.P_ELH_YEAR, salaryYearPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_DTL_GET_LIST, colParameters);
        }

        /// <summary>
        /// Method to Get Salary Pks
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSalaryPkList(int hyrPk, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
              new DBService.Parameters(Parameters.P_HYR_PK ,  hyrPk),
              new DBService.Parameters(Parameters.P_ACTIVE,  active),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_YEAR_MST_GET_KV, colParameters);
        }

        public static double GetBalanceLeave(int EmployeePk, int LeaveType, DateTime? DateUpTo)
        {
            double BalanceLeave = 0;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
              new DBService.Parameters(Parameters.P_EMP_PK ,  EmployeePk),
              new DBService.Parameters(Parameters.P_LEAVE_TYPE,  LeaveType),
              new DBService.Parameters(Parameters.P_DATE_UPTO, DateUpTo.HasValue? DateUpTo : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.ReturnValue, DBService.ParameterType.Number) 
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.FNHRM_EMP_LEAVE_BAL_GET, colParameters);
            BalanceLeave = Convert.ToDouble(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return BalanceLeave;
        }

        public static int SaveEmployeeLeave(string xmlstr, out string RetNo,out string empResignDate)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)  , 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_DT, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            RetNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            empResignDate = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_DT]).Value);
            return result;
        }

        public static DataSet GetLeaveList(FilterParameters objFilterParam)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  objFilterParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  objFilterParam.PageSize),
              new DBService.Parameters(Parameters.P_ELR_FROM_DATE,  objFilterParam.FromDate.HasValue ? objFilterParam.FromDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ELR_TO_DATE,  objFilterParam.ToDate.HasValue ? objFilterParam.ToDate : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ELR_BRANCH,  objFilterParam.BranchLocation.HasValue ? objFilterParam.BranchLocation : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_EMP_PK,  objFilterParam.Employee.HasValue ? objFilterParam.Employee : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,  objFilterParam.UserPK.HasValue ? objFilterParam.UserPK : (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_HDR_GET_LIST, colParameters);
        }

        public static string GetLeaveDetails(FilterParameters objFilterParam)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(Parameters.P_ELR_PK ,  objFilterParam.PK.HasValue ? objFilterParam.PK : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_ACTIVE,  objFilterParam.Active.HasValue ? objFilterParam.Active : (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_BIZUNIT,  objFilterParam.BizUnit.HasValue ? objFilterParam.BizUnit : (object)DBNull.Value)           
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_LEAVE_DTL_GET_KV, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteLeave(int CurrPK, DateTime LastModifiedTime, int lvExist = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_ELR_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_LV_EXIST , lvExist)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int DeleteDtlLeave(int CurrPK, DateTime LastModifiedTime,int lvExist=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null; 
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_ELD_PK , CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT , LastModifiedTime),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(Parameters.P_LV_EXIST , lvExist)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_DTL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static int DeleteLeaveDays(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)  , 
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LEAVE_DAY_DTL_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }
    }

}
