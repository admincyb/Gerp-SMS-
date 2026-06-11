using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;
using BusinessObject.CommonManagement;

namespace DataAccess.Finance
{
    public class ClosingStockDL
    {
        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetFCRList(GridPrams grid, User objUser, string vNo, int BankPk, int PoPk, int Active, int? Status = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid != null)
            {
                colParameters = new DBService.Parameters[]
                {
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                    //Filtration by voucher no
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : grid.SearchValue+"%"),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.FITERSTATUS,  Status == null ? (object) DBNull.Value : Status ),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, Active),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.HRH_PK,  PoPk == 0 ? (object) DBNull.Value :  PoPk.ToString()),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.DTL_VOUCHER_NO,  vNo == null? (object) DBNull.Value :  vNo),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.CBM_PK,  BankPk == 0 ? (object) DBNull.Value :  BankPk.ToString())
                };
            }
            else
            {
                colParameters = new DBService.Parameters[]
                {
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, Active),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.HRH_PK,  PoPk == 0 ? (object) DBNull.Value :  PoPk.ToString()),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.CBM_PK,  BankPk == 0 ? (object) DBNull.Value :  BankPk.ToString())
                };
            }
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETFCRLIST, colParameters);
            return dsList;
        }
        /// <summary>
        /// Get FCHold Revert Details 
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="accountPK"></param>
        /// <param name="curPK"></param>
        /// <returns></returns>
        public static DataSet GetFCHoldRevertDetails(int PK, int accountPK, int curPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(Parameters.HRH_PK, PK==0?(object) DBNull.Value:PK),
               new DBService.Parameters(Parameters.CBM_PK, accountPK),
               new DBService.Parameters(Parameters.CUR_PK, curPK),
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETFCHOLDDTL, colParameters);
            return dsList;
        }
        /// <summary>
        /// Get FinYear
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFinYear(string year, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(Parameters.P_VALUE, year),
               new DBService.Parameters(Parameters.P_BIZUNIT, bizunit),
              };
            DataTable dsList = new DataTable();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_MST_AUTO, colParameters).Tables[0];

            return dsList;
        }
         /// <summary>
         /// Get department
         /// </summary>
         /// <returns></returns>
        public static DataTable GetDepartment(int FinYearPK, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters(Parameters.P_FYR_PK, FinYearPK),
               new DBService.Parameters(Parameters.P_BIZUNIT, bizunit),
              };
            DataTable dsList = new DataTable();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_INV_DEPT_MAP_GET, colParameters).Tables[0];

            return dsList;
        }
        /// <summary>
        /// Save FCHold Revert Details 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static long SaveFCHoldRevertDetails(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SAVEFCHOLD, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="hrhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteFCHoldRevertDetails(int hrhPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.HRH_PK, hrhPK==0?(object) DBNull.Value:hrhPK),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEFCHOLD, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        public static DataTable GetFCVoucherNumberAuto(byte Active, int bizUnit, string searchValue)
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
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.FCVOUCHERNUMBERAUTO, colParameters).Tables[0];
            return dtProcess;
        }


        public static DataTable GetClosingStockList(int PageNo, int PageSize, DateTime? AsOnDate, string StockNo, int Status, User CurrentUser, int ClosingStkPk = 0)
        {
            DataTable dtStockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_NUM,  PageNo == 0 ? (object) DBNull.Value :  PageNo),
              new DBService.Parameters(GTIService.Constants.Common.Common.P_PAGE_SIZE,  PageSize == 0 ? (object) DBNull.Value :  PageSize),
              new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSH_AS_ON, AsOnDate.HasValue ? AsOnDate : (object) DBNull.Value),
              new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSH_NO, StockNo),
              new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSH_STATUS, Status),
              new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSH_PK, (ClosingStkPk > 0)? ClosingStkPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK,  CurrentUser.PKUser)
            };
            dtStockList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_CLOSING_STOCK_GET_LIST, colParameters).Tables[0];
            return dtStockList;
        }
        public static String GetStockDetails(int FinYrPK, int bizunit,int DeptPK)
        {
            DataTable dtStockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK,FinYrPK == 0 ? (object) DBNull.Value :FinYrPK),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT,bizunit == 0 ? (object) DBNull.Value :bizunit),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_DEPT_PK,DeptPK < 0 ? (object) DBNull.Value :DeptPK),
            };
            dtStockList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET, colParameters).Tables[0];
            string retStr = "";
            for (int i = 0; i < dtStockList.Rows.Count; i++)
                retStr += dtStockList.Rows[i][0].ToString();
            return retStr;

        }
        public static String GetStockDetailsByPK(int IohPk, int bizunit, int finyearpk,int? DeptPK=null)
        {
            DataTable dtStockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK,finyearpk == 0 ? (object) DBNull.Value :finyearpk),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT,bizunit == 0 ? (object) DBNull.Value :bizunit),
             new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IOH_PK, IohPk),
             new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_DEPT_PK, DeptPK==0?(object) DBNull.Value :DeptPK),
            };
            dtStockList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET_KV, colParameters).Tables[0];
            string retStr = "";
            for (int i = 0; i < dtStockList.Rows.Count; i++)
                retStr += dtStockList.Rows[i][0].ToString();
            return retStr;

        }
        public static DataTable GetStockList(int bizunit, int? finyrpk = null)
        {
            DataTable dtStockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_FYR_PK,finyrpk == 0 ? (object) DBNull.Value :finyrpk),
              new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT,bizunit == 0 ? (object) DBNull.Value :bizunit),

            };
            dtStockList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_INV_ITEM_DEPT_OPENING_STOCK_GET_LIST, colParameters).Tables[0];
            return dtStockList;

        }

        public static string GetClosingStockList(DateTime AsOnDate, int BizUnit, int LSH_PK)
        {
            string strRetVal = "";
            DataTable dtStockXml = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_AS_ON, AsOnDate),
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, BizUnit),
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSH_PK, LSH_PK == 0 ? (object) DBNull.Value :  LSH_PK)
            };
            dtStockXml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_CLOSING_STOCK_ITEM_GET, colParameters).Tables[0];
            foreach (DataRow dr in dtStockXml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? SaveClosingStockDetails(string xmlDoc, ref string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_CLOSING_STOCK_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }
        public static int? SaveStockClosingDetails(string xmlDoc,ref string TrxNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_INV_ITEM_DEPT_OPENING_STOCK_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            TrxNo = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            return result;
        }

        public static string GetClosingStockByPk(int LSH_PK, int BizUnit, byte Active)
        {
            string strRetVal = "";
            DataTable dtStockXml = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSH_PK, LSH_PK == 0 ? (object) DBNull.Value :  LSH_PK),
               new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active),
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, BizUnit)
            };
            dtStockXml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_CLOSING_STOCK_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtStockXml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }


        public static int? DeleteClosingStock(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_LSH_PK, CurrPK == 0 ? (object) DBNull.Value : CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, LastModifiedTime),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_CLOSING_STOCK_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static int? DeleteStockClosing(int CurrPK, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_IOH_PK, CurrPK == 0 ? (object) DBNull.Value : CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER_PK, userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPINV_INV_ITEM_DEPT_OPENING_STOCK_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetClosingStockHistory(int ItemCategoryPk)
        {
            DataTable dtStockHistory = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LSD_ITEM_CAT, ItemCategoryPk)

            };
            dtStockHistory = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPINV_CLOSING_STOCK_DTL_HISTORY_GET, colParameters).Tables[0];
            return dtStockHistory;
        }
    }
}
