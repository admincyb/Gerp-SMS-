using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.HRMS.Payroll;

namespace DataAccess.HRMS.Payroll
{
    public class LoansAndAdvancesDL
    {
        //To get Loans & Advance details for listing page
        public static DataSet GetLoansAndAdvancesList(BusinessObject.GridPrams gridParam, int branchPK, int emptypePK, int categoryPK = 0, int SearchEmp = 0)
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
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus)
              new DBService.Parameters(Parameters.P_EmpBranch, branchPK == 0 ? (object)DBNull.Value : branchPK) ,
              new DBService.Parameters(Parameters.P_empEmploymentType, emptypePK == 0 ? (object)DBNull.Value : emptypePK) ,
                new DBService.Parameters(Parameters.P_ELM_PAY_ELEMENT, categoryPK == 0 ? (object)DBNull.Value : categoryPK),
                new DBService.Parameters("@P_ELM_EMPLOYEE", SearchEmp == 0 ? (object)DBNull.Value  : SearchEmp)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LOAN_GET_LIST, colParameters);
        }

        public static DataTable GetLoanAdvanceItemType(int TypePK, int status, int bizUnit, int payelementPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_ELT_PK, TypePK == 0 ? (object)DBNull.Value : TypePK) ,
                new DBService.Parameters(GTIService.Constants.Common.Common.P_ACTIVE, status) , 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_ELT_PAY_ELEMENT, payelementPK)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPHRM_LOAN_TYPE_GET_KV, colParameters);
        }

        public static int? SaveLoanAdvanceHeader(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LOAN_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetEmpLoanDetails(int elmPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters.P_ELM_PK,  elmPK == 0 ? (object) DBNull.Value :  elmPK)                
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LOAN_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static DataTable GetBranchLocation()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ACTIVE, 1) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_TYPE_VALUE, 21) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_GROUP_VALUE, 7) 
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST, colParameters);
        }



      //Fill Employee Loan And Advance History
        public static DataTable GetEMPLoanHistory(int EmpPK, int bizUnit)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_ELM_EMPLOYEE, EmpPK) ,  
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_BIZUNIT, bizUnit) ,  


                
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_EMP_LOAN_HISTORY_GET, colParameters);
        }


        /// <summary>
        /// Method to Delete StockTransfer Details
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeleteLoansAndAdvances(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_ELM_PK , pk),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(ERP.Utilities.HRMS.Employee.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_EMP_LOAN_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

    }
}
