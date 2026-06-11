using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using GTIService.Constants.HRMS.Admin.Masters;

namespace DataAccess.HRMS.Admin.Masters
{
    public class EmployeeLeaveMasterDL
    {
        //Get Employee Leave Master List
        public static string GetEmployeeLeaveList(int currPk, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK, currPk==0?(object)DBNull.Value:currPk) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_BIZUNIT, bizUnit),
            };

            System.Data.Common.DbDataReader dtr = dbService.ExecuteReader(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_GET_XML, colParameters);
            string result = string.Empty;
            while (dtr.Read())
            {
                result += dtr.GetString(0);
            }
            return result;
        }

        /// <summary>
        /// Method to Save Employee LeaveMaster
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveEmployeeLeaveMaster(string xmlstr, out DataTable dtErrorList)
        {
            dtErrorList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
                
            };
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_SAVE, colParameters);
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_SAVE, colParameters);
            if (dsResult != null && dsResult.Tables.Count > 0)
                dtErrorList = dsResult.Tables[0];
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Delete EmployeeLeaveMaster
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteEmployeeLeaveMaster(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK , pk),
                new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)     
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        /// <summary>
        /// Method to Get Current Leave Credit
        /// </summary>
        /// <param name="employeeID"></param>
        ///<param name="leaveTypeID"></param>
        ///<param name="date"></param>
        /// <returns>int</returns>
        public static int GetCurrentLeaveCredit(int employeeID, int leaveTypeID, string date)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{     
            //    new DBService.Parameters(Parameters.P_GRH_PK , pk),
            //    new DBService.Parameters(Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
            //    new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            //};
            //dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_GRN_DIRECT_DELETE, colParameters);
            //int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            int result = 0;
            return result;
        }

        /// <summary>
        /// Method to Get Leave Credit List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetEmployeeLeaveListListingPage(BusinessObject.GridPrams gridParam, int bizUnit, string FilterFromDate, string FilterToDate, string Employee)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
              //new DBService.Parameters(Parameters.P_SER_NAME , grid.SearchBy ==Fields.STRINGEMPTY ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(Parameters.P_SER_VAL , grid.SearchValue == Fields.STRINGEMPTY ? (object)DBNull.Value : Fields.VALUE_PERC + grid.SearchValue + Fields.VALUE_PERC),
              new DBService.Parameters(Parameters.P_PAGE_NUM ,  gridParam.PageNumber),
              new DBService.Parameters(Parameters.P_PAGE_SIZE,  gridParam.PageSize),
              new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_DATE_FROM, FilterFromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(FilterFromDate) ),
              new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_DATE_TO, FilterToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(FilterToDate) ),
              new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ELH_EMPLOYEE, Employee==string.Empty?(object)DBNull.Value:Employee) ,

             // new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK, gridParam.SearchValue==string.Empty?(object)DBNull.Value:gridParam.SearchValue) 
              //new DBService.Parameters(Parameters.P_FIELDS, grid.Fields == Fields.STRINGEMPTY ? Fields.VALUE_STAR : grid.Fields),              
              //new DBService.Parameters(Parameters.P_SORT_BY,  grid.SortBy == null ||grid.SortBy ==GTIService.Constants.DirectStockTransfer.Fields.GRH_DATE|| grid.SortBy == GTIService.Constants.DirectStockTransfer.Fields.GRH_NO ?GTIService.Constants.DirectStockTransfer.Fields.GRH_PK : grid.SortBy),
              //new DBService.Parameters(Parameters.P_SORT_DIR , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
             // new DBService.Parameters(Parameters.P_FROM_DT, grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(Parameters.P_TO_DT, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              //new DBService.Parameters(Parameters.P_USER_PK ,  objUser.PKUser),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
              new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
             // new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_EOH_PK, gridParam.SearchValue==string.Empty?(object)DBNull.Value:gridParam.SearchValue) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_OB_LEAVE_GET_LIST, colParameters);
        }

        public static DataTable GetLeaveCredt(int? lvType, int? empPk, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_ELV_LEAVE_TYPE, lvType.HasValue?lvType:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ELV_EMPLOYEE, empPk.HasValue?empPk:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMPLOYEE_LEAVE_TYPE_GET, colParameters).Tables[0];
        }


        public static DataTable GetOBLeaveEmployeeListByFilter(int? empPK,  int? branch = null, int? EmpDept = null, int? EmpType = null,int? leaveType = null, string toDate = null)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EMPLOYEE_PK,empPK.HasValue? (empPK >= 0 ? empPK : (object)DBNull.Value) : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EmpBranch,branch.HasValue? (branch >= 0 ? branch : (object)DBNull.Value) : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_empDepartment, (EmpDept.HasValue && EmpDept > 0) ? EmpDept : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_EPD_EMP_TYPE,EmpType.HasValue? (EmpType >= 0 ? EmpType : (object)DBNull.Value) : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_TO_DATE, (toDate!=string.Empty) ? toDate : (object)DBNull.Value),
                new DBService.Parameters(Parameters.P_EMP_LEAVE_TYPE, leaveType.HasValue? (leaveType >= 0 ? leaveType : (object)DBNull.Value) : (object)DBNull.Value),
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_OB_LEAVE_EMPLOYEE_GET, colParameters).Tables[0];
            return dtList;
        }

    }
}
