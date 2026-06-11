using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using BusinessObject;
using GTIService.Constants.Administration.Masters;
namespace DataAccess.Administration.Masters
{
   public class FundRequisitionDeptDL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="bsu"></param>
        /// <returns></returns>
        public static DataTable GetFundRequisitionDeptList(GridPrams grid, User objUser, string trxNo, int reqDeptPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM, grid.PageNumber), 
                new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE, grid.PageSize),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_BIZUNIT, objUser.SBUID),
                new DBService.Parameters(Parameters.P_DFH_NO, trxNo == string.Empty ? (object) DBNull.Value : trxNo),              
                new DBService.Parameters(GTIService.Constants.Common.Common.P_FROM_DT, grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                new DBService.Parameters(GTIService.Constants.Common.Common.P_TO_DT, grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                new DBService.Parameters(Parameters.P_DFH_STATUS, string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),
                new DBService.Parameters(Parameters.P_DFH_REQ_DEPT, reqDeptPk >0?reqDeptPk :(object)DBNull.Value) ,               
                new DBService.Parameters(Parameters.P_USER_PK, objUser.PKUser)
            };
            return dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPINV_DEPT_FUND_REQ_GET_LIST, colParameters);

        }
        public static int? SaveFundRequisitionDeptDetails(string strxml, out string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_DEPT_FUND_REQ_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetFundRequisitionDeptByPK(int itemPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters.P_DFH_PK, itemPK)
            };
            DataTable dtxml = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPINV_DEPT_FUND_REQ_GET, colParameters);
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int DeleteFundRequisitionDept(int pk, string lastModifiedDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_DFH_PK , pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_LAST_MOD_DT , lastModifiedDate==string.Empty?(object)DBNull.Value:lastModifiedDate),
                new DBService.Parameters(GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_DEPT_FUND_REQ_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Admin.Masters.Parameters.P_RET_VAL]).Value);
            return result;
        }
        /// <summary>
        /// Method to get Auto Complete Search for Different Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetFundRequisitionNoAutocomplete(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE  ,  searchValue),
              new DBService.Parameters(Parameters.P_BIZUNIT  ,  objUser.SBUID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_DEPT_FUND_REQ_AUTO, colParameters).Tables[0];
        }
        //Fund Requisition Dept wise Output Report
        public static DataSet GetFundRequisitionDeptReport(int dfhPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.P_DFH_PK , dfhPK==0?(object) DBNull.Value:dfhPK)
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_DEPT_FUND_REQ_RPT, colParameters);
            return dsList;
        }
    }
}
